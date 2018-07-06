/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      // Symbolic Link Effects on File Systems Functions: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365682(v=vs.85).aspx


      /// <summary>Copy/move a Non-/Transacted file or directory including its children to a new location, <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless
      ///   <paramref name="cma.MoveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>This Move method works across disk volumes, and it does not throw an exception if the
      ///   source and destination are the same. </para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into
      ///   that directory, you get an IOException.</para>
      /// </remarks>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveCore(ErrorHandler errorFilter, bool retry, CopyMoveArguments cma, bool driveChecked, bool isFolder, string sourceFilePath, string destinationFilePath, CopyMoveResult copyMoveResult)
      {
         #region Setup

         string sourcePathLp;
         string destinationPathLp;

         cma = ValidateCopyMoveArguments(cma, sourceFilePath, destinationFilePath, out sourcePathLp, out destinationPathLp);
         
         if (!driveChecked)
         {
            // Check for local or network drives, such as: "C:" or "\\server\c$" (but not for "\\?\GLOBALROOT\").
            if (!sourcePathLp.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               Directory.ExistsDriveOrFolderOrFile(cma.Transaction, sourcePathLp, isFolder, (int) Win32Errors.NO_ERROR, true, false);


            // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.
            if (!cma.DelayUntilReboot)
               Directory.ExistsDriveOrFolderOrFile(cma.Transaction, destinationPathLp, isFolder, (int) Win32Errors.NO_ERROR, true, false);
         }
         

         // Setup callback function for progress notifications.

         var raiseException = null == cma.ProgressHandler;
         
         var routine = !raiseException

            ? (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, streamNumber, callbackReason, sourceFile, destinationFile, data) =>

               cma.ProgressHandler(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, (int) streamNumber, callbackReason, cma.UserProgressData)

            : (NativeMethods.NativeCopyMoveProgressRoutine) null;


         var copyMoveRes = copyMoveResult ?? new CopyMoveResult(sourceFilePath, destinationFilePath, cma.IsCopy, isFolder, cma.PreserveDates, cma.EmulateMove);

         var isMove = !cma.IsCopy;

         var isSingleFileAction = null == copyMoveResult && !isFolder || copyMoveRes.IsFile;


         ////cma.PreserveDates = cma.PreserveDates && cma.IsCopy && !isFolder;
         //if (isSingleFileAction && cma.IsCopy)
         //{
         //   cma.PreserveDates = HasPreserveDates(cma.CopyOptions);
         //   cma.CopyOptions &= ~CopyOptions.PreserveDates;  // Remove.
         //}


         var attempts = 1;
         var retryTimeout = 0;

         if (retry)
         {
            if (null != errorFilter)
            {
               attempts += cma.DirectoryEnumerationFilters.ErrorRetry;

               retryTimeout = cma.DirectoryEnumerationFilters.ErrorRetryTimeout;
            }

            else
            {
               if (cma.Retry <= 0)
                  cma.Retry = 2;

               if (cma.RetryTimeout <= 0)
                  cma.RetryTimeout = 10;

               attempts += cma.Retry;

               retryTimeout = cma.RetryTimeout;
            }
         }


         // Calling start on a running Stopwatch is a no-op.
         copyMoveRes.Stopwatch.Start();

         #endregion // Setup


         while (attempts-- > 0)
         {
            // MSDN: If this flag is set to TRUE during the copy/move operation, the operation is canceled.
            // Otherwise, the copy/move operation will continue to completion.
            bool cancel;

            copyMoveRes.ErrorCode = (int) Win32Errors.NO_ERROR;

            copyMoveRes.IsCanceled = false;

            int lastError;

            
            if (!cma.DelayUntilReboot)
            {
               // Ensure the file's parent directory exists.

               var parentFolder = Directory.GetParentCore(cma.Transaction, destinationPathLp, PathFormat.LongFullPath);

               if (null != parentFolder)
                  parentFolder.Create();
            }


            if (CopyMoveNative(cma, isMove, sourcePathLp, destinationPathLp, routine, out cancel, out lastError))
            {
               if (!isFolder)
                  copyMoveRes.TotalFiles++;


               // We take an extra hit by getting the file size for a single file Copy or Move action.
               if (isSingleFileAction)
                  copyMoveRes.TotalBytes = GetSizeCore(null, cma.Transaction, destinationPathLp, true, PathFormat.LongFullPath);


               if (cma.PreserveDates)
                  CopyTimestampsCore(cma.Transaction, sourcePathLp, destinationPathLp, false, PathFormat.LongFullPath);


               break;
            }


            // The Copy/Move action failed or is canceled.

            copyMoveRes.ErrorCode = lastError;

            copyMoveRes.IsCanceled = cancel;

            
            // Report the Exception back to the caller.
            if (null != errorFilter)
            {
               var continueCopyMove = errorFilter(lastError, new Win32Exception(lastError).Message,Path.GetCleanExceptionPath(destinationPathLp));

               if (!continueCopyMove)
               {
                  copyMoveRes.IsCanceled = true;
                  break;
               }
            }


            if (!cancel)
            {
               if (retry)
                  copyMoveRes.Retries++;

               retry = attempts > 0;

               // Remove any read-only/hidden attribute, which might also fail.

               RestartMoveOrThrowException(retry, lastError, isFolder, isMove, cma, sourcePathLp, destinationPathLp);

               if (retry)
               {
                  if (null != errorFilter && null != cma.DirectoryEnumerationFilters.CancellationToken)
                  {
                     if (cma.DirectoryEnumerationFilters.CancellationToken.WaitHandle.WaitOne(retryTimeout * 1000))
                     {
                        copyMoveRes.IsCanceled = true;
                        break;
                     }
                  }

                  else
                     using (var waitEvent = new ManualResetEvent(false))
                        waitEvent.WaitOne(retryTimeout * 1000);
               }
            }
         }

         
         if (isSingleFileAction)
            copyMoveRes.Stopwatch.Stop();

         copyMoveResult = copyMoveRes;

         return copyMoveResult;
      }


      [SecurityCritical]
      private static bool CopyMoveNative(CopyMoveArguments cma, bool isMove, string sourcePathLp, string destinationPathLp, NativeMethods.NativeCopyMoveProgressRoutine routine, out bool cancel, out int lastError)
      {
         cancel = false;

         var success = null == cma.Transaction || !NativeMethods.IsAtLeastWindowsVista

            // CopyFileEx() / CopyFileTransacted() / MoveFileWithProgress() / MoveFileTransacted()
            // 2013-04-15: MSDN confirms LongPath usage.


            ? isMove
               ? NativeMethods.MoveFileWithProgress(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, (MoveOptions) cma.MoveOptions)

               : NativeMethods.CopyFileEx(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, out cancel, (CopyOptions) cma.CopyOptions)

            : isMove
               ? NativeMethods.MoveFileTransacted(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, (MoveOptions) cma.MoveOptions, cma.Transaction.SafeHandle)

               : NativeMethods.CopyFileTransacted(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, out cancel, (CopyOptions) cma.CopyOptions, cma.Transaction.SafeHandle);


         lastError = Marshal.GetLastWin32Error();


         return success;


         // MSDN: If lpProgressRoutine returns PROGRESS_CANCEL due to the user canceling the operation,
         // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
         // In this case, the partially copied destination file is deleted.
         //
         // If lpProgressRoutine returns PROGRESS_STOP due to the user stopping the operation,
         // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
         // In this case, the partially copied destination file is left intact.


         // Note: MoveFileXxx fails if one of the paths is a UNC path, even though both paths refer to the same volume.
         // For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst

         // MoveFileXxx fails if it cannot access the registry. The function stores the locations of the files to be renamed at restart in the following registry value:
         //
         //    HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations
         //
         // This registry value is of type REG_MULTI_SZ. Each rename operation stores one of the following NULL-terminated strings, depending on whether the rename is a delete or not:
         //
         //    szDstFile\0\0              : indicates that the file szDstFile is to be deleted on reboot.
         //    szSrcFile\0szDstFile\0     : indicates that szSrcFile is to be renamed szDstFile on reboot.
      }
   }
}

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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
      internal static CopyMoveResult CopyMoveCore(CopyMoveArguments cma, bool driveChecked, bool isFolder, string sourcePath, string destinationPath, CopyMoveResult copyMoveResult)
      {
         #region Setup

         string sourcePathLp;
         string destinationPathLp;
         bool isCopy;

         // A Move action fallback using Copy + Delete.
         bool emulateMove;

         // A file or folder will be deleted or renamed on Computer startup.
         bool delayUntilReboot;
         bool deleteOnStartup;


         cma = ValidateAndUpdatePathsAndOptions(cma, sourcePath, destinationPath, out sourcePathLp, out destinationPathLp);

         //ValidateAndUpdatePathsAndOptions(cma.Transaction, sourcePath, destinationPath, cma.CopyOptions, cma.MoveOptions, cma.PathFormat, out sourcePathLp, out destinationPathLp, out isCopy, out emulateMove, out delayUntilReboot, out deleteOnStartup);


         if (!driveChecked)
         {
            // Check for local or network drives, such as: "C:" or "\\server\c$" (but not for "\\?\GLOBALROOT\").
            if (!sourcePathLp.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               Directory.ExistsDriveOrFolderOrFile(cma.Transaction, sourcePathLp, isFolder, (int) Win32Errors.NO_ERROR, true, false);


            // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.
            if (!cma.DelayUntilReboot)
               Directory.ExistsDriveOrFolderOrFile(cma.Transaction, destinationPathLp, isFolder, (int) Win32Errors.NO_ERROR, true, false);
         }


         // MSDN: If this flag is set to TRUE during the copy/move operation, the operation is canceled.
         // Otherwise, the copy/move operation will continue to completion.
         bool cancel;

         var raiseException = null == cma.ProgressHandler;


         // Setup callback function for progress notifications.

         var routine = !raiseException

            ? (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, streamNumber, callbackReason, sourceFile, destinationFile, data) =>

               cma.ProgressHandler(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, (int) streamNumber, callbackReason, cma.UserProgressData)

            : (NativeMethods.NativeCopyMoveProgressRoutine) null;


         var copyMoveRes = copyMoveResult ?? new CopyMoveResult(sourcePath, destinationPath, cma.IsCopy, isFolder, cma.PreserveDates, cma.EmulateMove);

         var isMove = !cma.IsCopy;
         var isSingleFileAction = null == copyMoveResult && !isFolder || copyMoveRes.IsFile;

         cma.PreserveDates = cma.PreserveDates && cma.IsCopy && !isFolder;


         // Calling start on a running Stopwatch is a no-op.
         copyMoveRes.Stopwatch.Start();

         #endregion // Setup


      startCopyMove:

         copyMoveRes.ErrorCode = (int) Win32Errors.NO_ERROR;

         int lastError;


         if (CopyMoveNative(cma, isMove, sourcePathLp, destinationPathLp, routine, out cancel, out lastError))

         //if (CopyMoveNative(cma.Transaction, isMove, sourcePathLp, destinationPathLp, routine, cma.CopyOptions, cma.MoveOptions, out cancel, out lastError))
         {
            if (!isFolder)
               copyMoveRes.TotalFiles++;


            //// Reset file system object attributes to ReadOnly.
            //if (HasReplaceExisting(moveOptions))
            //   SetAttributesCore(isFolder, transaction, destinationPathLp, FileAttributes.ReadOnly, PathFormat.LongFullPath);


            if (isSingleFileAction)
               // We take an extra hit by getting the file size for a single file Copy or Move action.
               copyMoveRes.TotalBytes = GetSizeCore(cma.Transaction, null, destinationPathLp, false, PathFormat.LongFullPath);


            if (cma.PreserveDates)
               CopyTimestampsCore(cma.Transaction, sourcePathLp, destinationPathLp, false, PathFormat.LongFullPath);
         }


         // Copy/Move action failed or canceled.
         else
         {
            // MSDN: If lpProgressRoutine returns PROGRESS_CANCEL due to the user canceling the operation,
            // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
            // In this case, the partially copied destination file is deleted.
            //
            // If lpProgressRoutine returns PROGRESS_STOP due to the user stopping the operation,
            // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
            // In this case, the partially copied destination file is left intact.


            copyMoveRes.ErrorCode = lastError;

            copyMoveRes.IsCanceled = lastError == Win32Errors.ERROR_REQUEST_ABORTED;

            if (!copyMoveRes.IsCanceled)
            {
               var attemptRetry = cma.Retry > 0 && cma.RetryTimeout > 0;

               for (var attempts = attemptRetry ? cma.Retry : 1; ; attempts--)
               {
                  if (RestartCopyMoveOrThrowException(attemptRetry, lastError, isFolder, isMove, cma, sourcePathLp, destinationPathLp))
                  {
                     // The file/folder read-only attribute is removed, restart the Copy/Move action.

                     goto startCopyMove;
                  }


                  if (attemptRetry)
                  {
                     Thread.Sleep(cma.RetryTimeout * 1000);

                     attemptRetry = attempts > 1;
                  }
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


      [SecurityCritical]
      private static bool RestartCopyMoveOrThrowException(bool attemptRetry, int lastError, bool isFolder, bool isMove, CopyMoveArguments cma, string sourcePathLp, string destinationPathLp)
      {
         var restart = false;
         var srcExists = ExistsCore(cma.Transaction, isFolder, sourcePathLp, PathFormat.LongFullPath);
         var dstExists = ExistsCore(cma.Transaction, isFolder, destinationPathLp, PathFormat.LongFullPath);


         switch ((uint) lastError)
         {
            // File.Copy()
            // File.Move()
            // MSDN: .NET 3.5+: FileNotFoundException: sourcePath was not found. 
            //
            // File.Copy()
            // File.Move()
            // Directory.Move()
            // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified in sourcePath or destinationPath is invalid (for example, it is on an unmapped drive).
            case Win32Errors.ERROR_FILE_NOT_FOUND: // On files.
            case Win32Errors.ERROR_PATH_NOT_FOUND: // On folders.

               if (!srcExists)
                  Directory.ExistsDriveOrFolderOrFile(cma.Transaction, sourcePathLp, isFolder, lastError, false, true);

               if (!dstExists)
                  Directory.ExistsDriveOrFolderOrFile(cma.Transaction, destinationPathLp, isFolder, lastError, false, true);

               break;


            case Win32Errors.ERROR_NOT_READY: // DeviceNotReadyException: Floppy device or network drive not ready.
               Directory.ExistsDriveOrFolderOrFile(cma.Transaction, sourcePathLp, false, lastError, true, false);
               Directory.ExistsDriveOrFolderOrFile(cma.Transaction, destinationPathLp, false, lastError, true, false);
               break;


            // File.Copy()
            // Directory.Copy()
            case Win32Errors.ERROR_ALREADY_EXISTS: // On folders.
            case Win32Errors.ERROR_FILE_EXISTS:    // On files.
               lastError = (int)(isFolder ? Win32Errors.ERROR_ALREADY_EXISTS : Win32Errors.ERROR_FILE_EXISTS);

               if (!attemptRetry)
                  NativeError.ThrowException(lastError, null, destinationPathLp);

               break;


            default:
               var attrs = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
               FillAttributeInfoCore(cma.Transaction, destinationPathLp, ref attrs, false, false);

               var destIsFolder = IsDirectory(attrs.dwFileAttributes);


               // For a number of error codes (sharing violation, path not found, etc)
               // we don't know if the problem was with the source or destination file.

               // Check if destination directory already exists.
               // Directory.Move()
               // MSDN: .NET 3.5+: IOException: destDirName already exists.

               if (destIsFolder && dstExists && !attemptRetry)
                  NativeError.ThrowException(Win32Errors.ERROR_ALREADY_EXISTS, destinationPathLp);




               if (isMove)
               {
                  // Ensure that the source file or folder exists.
                  // Directory.Move()
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified by sourceDirName is invalid (for example, it is on an unmapped drive). 

                  if (!srcExists && !attemptRetry)
                     NativeError.ThrowException(isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND, sourcePathLp);
               }


               // Try reading the source file.
               var fileNameLp = destinationPathLp;

               if (!isFolder)
               {
                  using (var safeHandle = CreateFileCore(cma.Transaction, sourcePathLp, ExtendedFileAttributes.Normal, null, FileMode.Open, 0, FileShare.Read, false, false, PathFormat.LongFullPath))
                     if (null != safeHandle)
                        fileNameLp = sourcePathLp;
               }


               if (lastError == Win32Errors.ERROR_ACCESS_DENIED)
               {
                  // File.Copy()
                  // File.Move()
                  // MSDN: .NET 3.5+: IOException: An I/O error has occurred.


                  // Directory exists with the same name as the file.
                  if (dstExists && !isFolder && destIsFolder && !attemptRetry)
                     NativeError.ThrowException(lastError, null, string.Format(CultureInfo.InvariantCulture, Resources.Target_File_Is_A_Directory, destinationPathLp));


                  if (isMove)
                  {
                     if (IsReadOnlyOrHidden(attrs.dwFileAttributes))
                     {
                        // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only.
                        if (CanOverwrite(cma.MoveOptions))
                        {
                           // Reset file system object attributes.
                           SetAttributesCore(cma.Transaction, isFolder, destinationPathLp, FileAttributes.Normal, PathFormat.LongFullPath);

                           restart = true;
                           break;
                        }


                        // MSDN: .NET 3.5+: UnauthorizedAccessException: destinationPath is read-only.
                        // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                        // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.

                        if (!attemptRetry)
                           throw new FileReadOnlyException(destinationPathLp);
                     }
                  }
               }


               // MSDN: .NET 3.5+: An I/O error has occurred. 
               // File.Copy(): IOException: destinationPath exists and overwrite is false.
               // File.Move(): The destination file already exists or sourcePath was not found.

               if (!attemptRetry)
                  NativeError.ThrowException(lastError, null, fileNameLp);

               break;
         }


         return restart;
      }


      internal static CopyMoveArguments ValidateAndUpdatePathsAndOptions(CopyMoveArguments cma, string sourcePath, string destinationPath, out string sourcePathLp, out string destinationPathLp)
      {
         if (sourcePath == string.Empty)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "sourcePath");

         if (destinationPath == string.Empty)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "destinationPath");


         // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
         // Do not use StringComparison.OrdinalIgnoreCase to allow renaming a folder with different casing.

         if (null != sourcePath && sourcePath.Equals(destinationPath, StringComparison.Ordinal))
            NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPath);


         sourcePathLp = sourcePath;
         destinationPathLp = destinationPath;

         cma.IsCopy = IsCopyAction(cma.CopyOptions, cma.MoveOptions);

         var isMove = !cma.IsCopy;
         cma.EmulateMove = false;

         cma.DelayUntilReboot = isMove && VerifyDelayUntilReboot(sourcePath, cma.MoveOptions, cma.PathFormat);

         // When destinationPath is null, the file or folder needs to be removed on Computer startup.
         cma.DeleteOnStartup = cma.DelayUntilReboot && null == destinationPath;


         if (cma.PathFormat == PathFormat.RelativePath)
         {
            if (null == sourcePath)
               throw new ArgumentNullException("sourcePath");


            // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.

            if (!cma.DelayUntilReboot && null == destinationPath)
               throw new ArgumentNullException("destinationPath");


            // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameters before moving the directory.
            // TrimEnd() is also applied for AlphaFS implementation of method Directory.Copy(), .NET does not have this method.


            const GetFullPathOptions fullPathOptions = GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator;

            
            // Check for local or network drives, such as: "C:" or "\\server\c$" (but not for "\\?\GLOBALROOT\").

            if (!sourcePath.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))

               Path.CheckSupportedPathFormat(sourcePath, true, true);


            cma.SourcePathLp = Path.GetExtendedLengthPathCore(cma.Transaction, sourcePath, cma.PathFormat, fullPathOptions);


            if (!cma.DeleteOnStartup)
            {
               Path.CheckSupportedPathFormat(destinationPath, true, true);

               cma.DestinationPathLp = Path.GetExtendedLengthPathCore(cma.Transaction, destinationPath, cma.PathFormat, fullPathOptions);
            }


            cma.PathFormat = PathFormat.LongFullPath;
         }
         
         else
         {
            cma.SourcePathLp = sourcePathLp;
            cma.DestinationPathLp = destinationPathLp;
         }


         return cma;
      }


      private static bool VerifyDelayUntilReboot(string sourcePath, MoveOptions? moveOptions, PathFormat pathFormat)
      {
         var delayUntilReboot = HasDelayUntilReboot(moveOptions);

         if (delayUntilReboot)
         {
            if (AllowEmulate(moveOptions))
               throw new ArgumentException(Resources.MoveOptionsDelayUntilReboot_Not_Allowed_With_MoveOptionsCopyAllowed, "moveOptions");


            // MoveFileXxx: (lpExistingFileName) If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT,
            // the file cannot exist on a remote share, because delayed operations are performed before the network is available.

            if (Path.IsUncPathCore(sourcePath, pathFormat != PathFormat.LongFullPath, false))
               throw new ArgumentException(Resources.MoveOptionsDelayUntilReboot_Not_Allowed_With_NetworkPath, "moveOptions");
         }

         return delayUntilReboot;
      }
   }
}

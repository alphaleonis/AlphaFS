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
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>[AlphaFS] Copy/move a Non-/Transacted file or directory including its children to a new location, <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="cma.moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveCore(CopyMoveArguments cma)
      {
         #region Setup

         string unusedSourcePath;
         string unusedDestinationPath;

         cma = File.ValidateAndUpdatePathsAndOptions(cma, cma.SourcePath, cma.DestinationPath, out unusedSourcePath, out unusedDestinationPath);


         // Directory.Move is applicable to both folders and files.
         var isFile = File.ExistsCore(cma.Transaction, false, cma.SourcePathLp, PathFormat.LongFullPath);


         // Check for local or network drives, such as: "C:" or "\\server\c$" (but not for "\\?\GLOBALROOT\").
         if (!cma.SourcePathLp.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
            ExistsDriveOrFolderOrFile(cma.Transaction, cma.SourcePathLp, !isFile, (int) Win32Errors.NO_ERROR, true, false);


         // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.
         if (!cma.DelayUntilReboot)
            ExistsDriveOrFolderOrFile(cma.Transaction, cma.DestinationPathLp, !isFile, (int) Win32Errors.NO_ERROR, true, false);


         // Process Move action options, possible fallback to Copy action.
         if (!cma.IsCopy && !cma.DeleteOnStartup)
            cma = ValidateAndUpdateCopyMoveAction(cma);


         var copyMoveResult = new CopyMoveResult(cma, !isFile);

         var errorFilter = null != cma.DirectoryEnumerationFilters && null != cma.DirectoryEnumerationFilters.ErrorFilter ? cma.DirectoryEnumerationFilters.ErrorFilter : null;

         var retry = null != errorFilter && (cma.DirectoryEnumerationFilters.ErrorRetry > 0 || cma.DirectoryEnumerationFilters.ErrorRetryTimeout > 0);

         if (retry)
         {
            if (cma.DirectoryEnumerationFilters.ErrorRetry <= 0)
               cma.DirectoryEnumerationFilters.ErrorRetry = 2;

            if (cma.DirectoryEnumerationFilters.ErrorRetryTimeout <= 0)
               cma.DirectoryEnumerationFilters.ErrorRetryTimeout = 10;
         }


         // Calling start on a running Stopwatch is a no-op.
         copyMoveResult.Stopwatch.Start();

         #endregion // Setup


         if (cma.IsCopy)
         {
            // Copy folder SymbolicLinks.
            // Cannot be done by CopyFileEx() so emulate this.

            if (File.HasCopySymbolicLink(cma.CopyOptions))
            {
               var lvi = File.GetLinkTargetInfoCore(cma.Transaction, cma.SourcePathLp, true, PathFormat.LongFullPath);

               if (null != lvi)
               {
                  File.CreateSymbolicLinkCore(cma.Transaction, cma.DestinationPathLp, lvi.SubstituteName, SymbolicLinkTarget.Directory, PathFormat.LongFullPath);

                  copyMoveResult.TotalFolders = 1;
               }
            }

            else
            {
               if (isFile)
                  File.CopyMoveCore(errorFilter, retry, cma, true, false, cma.SourcePathLp, cma.DestinationPathLp, copyMoveResult);

               else
                  CopyEmulateMoveDirectoryCore(errorFilter, retry, cma, copyMoveResult);
            }
         }


         // Move

         else
         {
            // AlphaFS feature to overcome a MoveFileXxx limitation.
            // MoveOptions.ReplaceExisting: This value cannot be used if lpNewFileName or lpExistingFileName names a directory.

            if (!isFile && !cma.DelayUntilReboot && File.CanOverwrite(cma.MoveOptions))

               DeleteDirectoryCore(cma.Transaction, null, cma.DestinationPathLp, true, true, true, PathFormat.LongFullPath);


            // 2017-06-07: A large target directory will probably create a progress-less delay in UI.
            // One way to get around this is to perform the delete in the File.CopyMove method.


            // Moves a file or directory, including its children.
            // Copies an existing directory, including its children to a new directory.

            File.CopyMoveCore(errorFilter, retry, cma, true, !isFile, cma.SourcePathLp, cma.DestinationPathLp, copyMoveResult);


            // If the move happened on the same drive, we have no knowledge of the number of files/folders.
            // However, we do know that the one folder was moved successfully.

            if (copyMoveResult.ErrorCode == Win32Errors.NO_ERROR)
               copyMoveResult.TotalFolders = 1;
         }


         copyMoveResult.Stopwatch.Stop();

         return copyMoveResult;
      }


      [SecurityCritical]
      private static CopyMoveArguments ValidateAndUpdateCopyMoveAction(CopyMoveArguments cma)
      {
         // Determine if a Move action or Copy action-fallback is possible.
         cma.IsCopy = false;
         cma.EmulateMove = false;


         // Compare the root part of both paths.
         var equalRootPaths = Path.GetPathRoot(cma.SourcePathLp, false).Equals(Path.GetPathRoot(cma.DestinationPathLp, false), StringComparison.OrdinalIgnoreCase);


         // Method Volume.IsSameVolume() returns true when both paths refer to the same volume, even if one of the paths is a UNC path.
         // For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst
         var isSameVolume = equalRootPaths || Volume.IsSameVolume(cma.SourcePathLp, cma.DestinationPathLp);


         var isMove = isSameVolume && equalRootPaths;

         if (!isMove)
         {
            // A Move() can be emulated by using Copy() and Delete(), but only if the MoveOptions.CopyAllowed flag is set.
            isMove = File.AllowEmulate(cma.MoveOptions);

            // MSDN: .NET3.5+: IOException: An attempt was made to move a directory to a different volume.
            if (!isMove)
               NativeError.ThrowException(Win32Errors.ERROR_NOT_SAME_DEVICE, cma.SourcePathLp, cma.DestinationPathLp);
         }


         // The MoveFileXxx methods fail when:
         // - A directory is being moved;
         // - One of the paths is a UNC path, even though both paths refer to the same volume.
         //   For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst
         if (isMove)
         {
            var srcIsUncPath = Path.IsUncPathCore(cma.SourcePathLp, false, false);
            var dstIsUncPath = Path.IsUncPathCore(cma.DestinationPathLp, false, false);

            isMove = srcIsUncPath == dstIsUncPath;
         }


         isMove = isMove && isSameVolume && equalRootPaths;


         // Emulate Move().
         if (!isMove)
         {
            cma.MoveOptions = null;

            cma.IsCopy = true;
            cma.EmulateMove = true;
            cma.CopyOptions = CopyOptions.None;
         }


         return cma;
      }
   }
}

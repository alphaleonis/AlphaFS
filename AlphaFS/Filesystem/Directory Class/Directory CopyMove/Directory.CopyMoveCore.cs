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
using System.Collections.Generic;
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
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="retry">The number of retries on failed copies.</param>
      /// <param name="retryTimeout">A <see cref="TimeSpan"/> that specifies the wait time between retries.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise. This parameter is ignored for move operations.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveCore(int retry, TimeSpan? retryTimeout, KernelTransaction transaction, string sourcePath, string destinationPath, bool preserveDates,

         CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string sourcePathLp;
         string destinationPathLp;
         bool isCopy;
         
         // A Move action fallback using Copy + Delete.
         bool emulateMove;

         // A file or folder will be deleted or renamed on Computer startup.
         bool delayUntilReboot;
         bool deleteOnStartup;
         

         File.ValidateAndUpdatePathsAndOptions(transaction, sourcePath, destinationPath, copyOptions, moveOptions, pathFormat, out sourcePathLp, out destinationPathLp, out isCopy, out emulateMove, out delayUntilReboot, out deleteOnStartup);


         // Directory.Move is applicable to both folders and files.
         var isFile = File.ExistsCore(transaction, false, sourcePath, PathFormat.LongFullPath);


         // Check for local or network drives, such as: "C:" or "\\server\c$" (but not for "\\?\GLOBALROOT\").
         if (!sourcePathLp.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
            ExistsDriveOrFolderOrFile(transaction, sourcePathLp, !isFile, (int) Win32Errors.NO_ERROR, true, false);
         

         // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.
         if (!delayUntilReboot)
            ExistsDriveOrFolderOrFile(transaction, destinationPathLp, !isFile, (int) Win32Errors.NO_ERROR, true, false);
         
         
         // Process Move action options, possible fallback to Copy action.
         if (!isCopy && !deleteOnStartup)
            ValidateAndUpdateCopyMoveAction(sourcePathLp, destinationPathLp, copyOptions, moveOptions, out copyOptions, out moveOptions, out isCopy, out emulateMove);
         
         
         pathFormat = PathFormat.LongFullPath;
         
         var copyMoveResult = new CopyMoveResult(sourcePath, destinationPath, isCopy, !isFile, preserveDates, emulateMove);

         // Calling start on a running Stopwatch is a no-op.
         copyMoveResult.Stopwatch.Start();


         if (isCopy)
         {
            // Copy folder SymbolicLinks.
            // Cannot be done by CopyFileEx() so emulate this.

            if (File.HasCopySymbolicLink(copyOptions))
            {
               var lvi = File.GetLinkTargetInfoCore(transaction, sourcePathLp, true, pathFormat);

               if (null != lvi)
               {
                  File.CreateSymbolicLinkCore(transaction, destinationPathLp, lvi.SubstituteName, SymbolicLinkTarget.Directory, pathFormat);

                  copyMoveResult.TotalFolders = 1;
               }
            }

            else
            {
               if (isFile)
                  File.CopyMoveCore(retry, retryTimeout, transaction, true, false, sourcePathLp, destinationPathLp, copyOptions, null, preserveDates, progressHandler, userProgressData, copyMoveResult, PathFormat.LongFullPath);

               else
                  CopyDeleteDirectoryCore(retry, retryTimeout, transaction, sourcePathLp, destinationPathLp, preserveDates, emulateMove, copyOptions, progressHandler, userProgressData, copyMoveResult);
            }
         }

         // Move
         else
         {
            // AlphaFS feature to overcome a MoveFileXxx limitation.
            // MoveOptions.ReplaceExisting: This value cannot be used if lpNewFileName or lpExistingFileName names a directory.

            if (!isFile && !delayUntilReboot && File.CanOverwrite(moveOptions))
               DeleteDirectoryCore(transaction, null, destinationPathLp, true, true, true, pathFormat);

            // 2017-06-07: A large target directory will probably create a progress-less delay in UI.
            // One way to get around this is to perform the delete in the File.CopyMove method.


            // Moves a file or directory, including its children.
            // Copies an existing directory, including its children to a new directory.

            File.CopyMoveCore(retry, retryTimeout, transaction, true, !isFile, sourcePathLp, destinationPathLp, copyOptions, moveOptions, preserveDates, progressHandler, userProgressData, copyMoveResult, pathFormat);


            // If the move happened on the same drive, we have no knowledge of the number of files/folders.
            // However, we do know that the one folder was moved successfully.

            if (copyMoveResult.ErrorCode == Win32Errors.NO_ERROR)
               copyMoveResult.TotalFolders = 1;
         }


         copyMoveResult.Stopwatch.Stop();

         return copyMoveResult;
      }




      internal static void CopyDeleteDirectoryCore(int retry, TimeSpan? retryTimeout, KernelTransaction transaction, string sourcePathLp, string destinationPathLp, bool preserveDates, bool emulateMove,
         
         CopyOptions? copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, CopyMoveResult copyMoveResult)
      {
         var dirs = new Queue<string>(NativeMethods.DefaultFileBufferSize);

         dirs.Enqueue(sourcePathLp);
         

         while (dirs.Count > 0)
         {
            var srcLp = dirs.Dequeue();

            // TODO 2018-01-09: Not 100% yet with local + UNC paths.
            var dstLp = srcLp.Replace(sourcePathLp, destinationPathLp);


            // Traverse the source folder, processing files and folders.

            foreach (var fseiSource in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, transaction, srcLp, Path.WildcardStarMatchAll, null, null, null, PathFormat.LongFullPath))
            {
               var fseiSourcePath = fseiSource.LongFullPath;
               var fseiDestinationPath = Path.CombineCore(false, dstLp, fseiSource.FileName);


               if (fseiSource.IsDirectory)
               {
                  CreateDirectoryCore(true, transaction, fseiDestinationPath, null, null, false, PathFormat.LongFullPath);

                  copyMoveResult.TotalFolders++;

                  dirs.Enqueue(fseiSourcePath);
               }


               // File.
               else
               {
                  // Ensure the file's parent directory exists.

                  var parentFolder = GetParentCore(transaction, fseiDestinationPath, PathFormat.LongFullPath);

                  if (null != parentFolder)
                  {
                     var fileParentFolder = Path.GetLongPathCore(parentFolder.FullName, GetFullPathOptions.None);

                     CreateDirectoryCore(true, transaction, fileParentFolder, null, null, false, PathFormat.LongFullPath);
                  }


                  // File count is done in File.CopyMoveCore method.

                  File.CopyMoveCore(retry, retryTimeout, transaction, true, false, fseiSourcePath, fseiDestinationPath, copyOptions, null, preserveDates, progressHandler, userProgressData, copyMoveResult, PathFormat.LongFullPath);

                  if (copyMoveResult.IsCanceled)
                  {
                     // Break while loop.
                     dirs.Clear();

                     // Break foreach loop.
                     break;
                  }


                  if (copyMoveResult.ErrorCode == Win32Errors.NO_ERROR)
                  {
                     copyMoveResult.TotalBytes += fseiSource.FileSize;

                     if (emulateMove)
                        File.DeleteFileCore(transaction, fseiSourcePath, true, PathFormat.LongFullPath);
                  }
               }
            }
         }


         if (copyMoveResult.ErrorCode == Win32Errors.NO_ERROR)
         {
            if (preserveDates)
            {
               // TODO 2018-01-09: Not 100% yet with local + UNC paths.
               var dstLp = sourcePathLp.Replace(sourcePathLp, destinationPathLp);


               // Traverse the source folder, processing subfolders.

               foreach (var fseiSource in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(true, transaction, sourcePathLp, Path.WildcardStarMatchAll, null, null, null, PathFormat.LongFullPath))

                  File.CopyTimestampsCore(transaction, fseiSource.LongFullPath, Path.CombineCore(false, dstLp, fseiSource.FileName), false, PathFormat.LongFullPath);

               // TODO: When enabled on Computer, FindFirstFile will change the last accessed time.
            }


            if (emulateMove)
               DeleteDirectoryCore(transaction, null, sourcePathLp, true, true, true, PathFormat.LongFullPath);
         }
      }




      private static void ValidateAndUpdateCopyMoveAction(string sourcePathLp, string destinationPathLp, CopyOptions? copyOptions, MoveOptions? moveOptions, out CopyOptions? newCopyOptions, out MoveOptions? newMoveOptions, out bool isCopy, out bool emulateMove)
      {
         // Determine if a Move action or Copy action-fallback is possible.
         isCopy = false;
         emulateMove = false;


         // Compare the root part of both paths.
         var equalRootPaths = Path.GetPathRoot(sourcePathLp, false).Equals(Path.GetPathRoot(destinationPathLp, false), StringComparison.OrdinalIgnoreCase);


         // Method Volume.IsSameVolume() returns true when both paths refer to the same volume, even if one of the paths is a UNC path.
         // For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst
         var isSameVolume = equalRootPaths || Volume.IsSameVolume(sourcePathLp, destinationPathLp);


         var isMove = isSameVolume && equalRootPaths;

         if (!isMove)
         {
            // A Move() can be emulated by using Copy() and Delete(), but only if the MoveOptions.CopyAllowed flag is set.
            isMove = File.AllowEmulate(moveOptions);

            // MSDN: .NET3.5+: IOException: An attempt was made to move a directory to a different volume.
            if (!isMove)
               NativeError.ThrowException(Win32Errors.ERROR_NOT_SAME_DEVICE, sourcePathLp, destinationPathLp);
         }


         // The MoveFileXxx methods fail when:
         // - A directory is being moved;
         // - One of the paths is a UNC path, even though both paths refer to the same volume.
         //   For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst
         if (isMove)
         {
            var srcIsUncPath = Path.IsUncPathCore(sourcePathLp, false, false);
            var dstIsUncPath = Path.IsUncPathCore(destinationPathLp, false, false);

            isMove = srcIsUncPath == dstIsUncPath;
         }


         isMove = isMove && isSameVolume && equalRootPaths;


         // Emulate Move().
         if (!isMove)
         {
            moveOptions = null;

            isCopy = true;
            emulateMove = true;
            copyOptions = CopyOptions.None;
         }


         newCopyOptions = copyOptions;
         newMoveOptions = moveOptions;
      }
   }
}

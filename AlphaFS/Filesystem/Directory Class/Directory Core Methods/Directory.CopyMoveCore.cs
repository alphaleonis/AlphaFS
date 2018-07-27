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
      /// <summary>Copy/move a Non-/Transacted file or directory including its children to a new location, <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="copyMoveArguments.moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
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
      internal static CopyMoveResult CopyMoveCore(CopyMoveArguments copyMoveArguments, RetryArguments retryArguments = null)
      {
         #region Setup

         if (null == copyMoveArguments)
            throw new ArgumentNullException("copyMoveArguments");


         var fsei = File.GetFileSystemEntryInfoCore(copyMoveArguments.Transaction, true, copyMoveArguments.SourcePath, true, copyMoveArguments.PathFormat);

         var isFolder = null == fsei || fsei.IsDirectory;

         // Directory.Move is applicable to both files and folders.

         File.ValidateCopyMoveArguments(copyMoveArguments, false, isFolder);
         

         var filters = null != copyMoveArguments.DirectoryEnumerationFilters;

         var errorFilter = filters && null != copyMoveArguments.DirectoryEnumerationFilters.ErrorFilter ? copyMoveArguments.DirectoryEnumerationFilters.ErrorFilter : null;

         if (null == retryArguments && filters && null != copyMoveArguments.DirectoryEnumerationFilters.RetryArguments)
            retryArguments = copyMoveArguments.DirectoryEnumerationFilters.RetryArguments;

         var retry = null != retryArguments && retryArguments.Retry > 0;


         var copyMoveResult = new CopyMoveResult(isFolder, copyMoveArguments);

         copyMoveResult.Stopwatch.Start();

         #endregion // Setup


         if (copyMoveArguments.IsCopy)
         {
            // Copy folder SymbolicLinks.
            // Cannot be done by CopyFileEx() so emulate this.

            if (File.HasCopySymbolicLink(copyMoveArguments.CopyOptions))
            {
               var lvi = File.GetLinkTargetInfoCore(copyMoveArguments.Transaction, copyMoveArguments.SourcePathLp, true, PathFormat.LongFullPath);

               if (null != lvi)
               {
                  File.CreateSymbolicLinkCore(copyMoveArguments.Transaction, copyMoveArguments.DestinationPathLp, lvi.SubstituteName, SymbolicLinkTarget.Directory, PathFormat.LongFullPath);

                  copyMoveResult.TotalFolders = 1;
               }
            }

            else
            {
               if (isFolder)
                  CopyMoveDirectoryCore(retry, copyMoveArguments, retryArguments, copyMoveResult);

               else
                  File.CopyMoveCore(false, true, copyMoveArguments.SourcePathLp, copyMoveArguments.DestinationPathLp, copyMoveArguments, retryArguments, copyMoveResult);
            }
         }


         // Move

         else
         {
            // AlphaFS feature to overcome a MoveFileXxx limitation.
            // MoveOptions.ReplaceExisting: This value cannot be used if lpNewFileName or lpExistingFileName names a directory.

            if (isFolder && !copyMoveArguments.DelayUntilReboot && File.HasReplaceExisting(copyMoveArguments.MoveOptions))

               // 2017-06-07: A large target directory will probably create a progress-less delay in UI.
               // One way to get around this is to perform the delete in the File.CopyMove method.

               DeleteDirectoryCore(new DeleteArguments
               {
                  IsDirectory = true,
                  Recursive = true,
                  ContinueOnNotFound = true,
                  IgnoreReadOnly = true,

                  DirectoryEnumerationFilters = copyMoveArguments.DirectoryEnumerationFilters,
                  Transaction = copyMoveArguments.Transaction,
                  TargetPathLongPath = copyMoveArguments.DestinationPathLp,
                  PathFormat = PathFormat.LongFullPath

               }, retryArguments);
            

            // Moves a file or directory, including its children.
            // Copies an existing directory, including its children to a new directory.

            File.CopyMoveCore(isFolder, true, copyMoveArguments.SourcePathLp, copyMoveArguments.DestinationPathLp, copyMoveArguments, retryArguments, copyMoveResult);


            // If the move happened on the same drive, we have no knowledge of the number of files/folders.
            // However, we do know that the one folder was moved successfully.
            
            if (isFolder && copyMoveResult.ErrorCode == Win32Errors.NO_ERROR)

               copyMoveResult.TotalFolders = 1;
         }


         copyMoveResult.Stopwatch.Stop();

         return copyMoveResult;
      }
   }
}

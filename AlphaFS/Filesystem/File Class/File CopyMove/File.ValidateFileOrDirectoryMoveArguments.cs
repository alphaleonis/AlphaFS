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

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      internal static CopyMoveArguments ValidateFileOrDirectoryMoveArguments(CopyMoveArguments copyMoveArguments, bool driveChecked, bool isFolder)
      {
         string unusedSourcePathLp;
         string unusedDestinationPathLp;

         return ValidateFileOrDirectoryMoveArguments(copyMoveArguments, driveChecked, isFolder, copyMoveArguments.SourcePath, copyMoveArguments.DestinationPath, out unusedSourcePathLp, out unusedDestinationPathLp);
      }


      /// <summary>Validates and updates the file/directory copy/move arguments and updates them accordingly. This happens only once per <see cref="CopyMoveArguments"/> instance.</summary>
      private static CopyMoveArguments ValidateFileOrDirectoryMoveArguments(CopyMoveArguments copyMoveArguments, bool driveChecked, bool isFolder, string sourcePath, string destinationPath, out string sourcePathLp, out string destinationPathLp)
      {
         if (null == copyMoveArguments)
            throw new ArgumentNullException("copyMoveArguments");
         

         sourcePathLp = sourcePath;
         destinationPathLp = destinationPath;
         
         if (copyMoveArguments.PathsChecked)
            return copyMoveArguments;


         copyMoveArguments.IsCopy = IsCopyAction(copyMoveArguments);

         if (!copyMoveArguments.IsCopy)
            copyMoveArguments.DelayUntilReboot = VerifyDelayUntilReboot(sourcePath, copyMoveArguments.MoveOptions, copyMoveArguments.PathFormat);


         if (copyMoveArguments.PathFormat != PathFormat.LongFullPath)
         {
            if (null == sourcePath)
               throw new ArgumentNullException("sourcePath");
            
            // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.

            if (!copyMoveArguments.DelayUntilReboot && null == destinationPath)
               throw new ArgumentNullException("destinationPath");
            

            if (sourcePath.Trim().Length == 0)
               throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "sourcePath");

            if (null != destinationPath && destinationPath.Trim().Length == 0)
               throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "destinationPath");


            // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
            // Do not use StringComparison.OrdinalIgnoreCase to allow renaming a folder with different casing.

            if (sourcePath.Equals(destinationPath, StringComparison.Ordinal))
               NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPath);


            if (!driveChecked)
            {
               // Check for local or network drives, such as: "C:" or "\\server\c$" (but not for "\\?\GLOBALROOT\").
               if (!sourcePath.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
                  Directory.ExistsDriveOrFolderOrFile(copyMoveArguments.Transaction, sourcePath, isFolder, (int) Win32Errors.NO_ERROR, true, false);


               // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.
               if (!copyMoveArguments.DelayUntilReboot)
                  Directory.ExistsDriveOrFolderOrFile(copyMoveArguments.Transaction, destinationPath, isFolder, (int) Win32Errors.NO_ERROR, true, false);
            }


            // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameters before moving the directory.
            // TrimEnd() is also applied for AlphaFS implementation of method Directory.Copy(), .NET does not have this method.

            const GetFullPathOptions fullPathOptions = GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator;


            sourcePathLp = Path.GetExtendedLengthPathCore(copyMoveArguments.Transaction, sourcePath, copyMoveArguments.PathFormat, fullPathOptions);

            if (isFolder || !copyMoveArguments.IsCopy)
               copyMoveArguments.SourcePathLp = sourcePathLp;


            // When destinationPath is null, the file/folder needs to be removed on Computer startup.

            copyMoveArguments.DeleteOnStartup = copyMoveArguments.DelayUntilReboot && null == destinationPath;
            
            if (!copyMoveArguments.DeleteOnStartup)
            {
               Path.CheckSupportedPathFormat(destinationPath, true, true);

               destinationPathLp = Path.GetExtendedLengthPathCore(copyMoveArguments.Transaction, destinationPath, copyMoveArguments.PathFormat, fullPathOptions);


               if (isFolder || !copyMoveArguments.IsCopy)
               {
                  copyMoveArguments.DestinationPathLp = destinationPathLp;

                  // Process Move action options, possible fallback to Copy action.

                  if (!copyMoveArguments.IsCopy)
                     Directory.ValidateMoveAction(copyMoveArguments);
               }


               if (copyMoveArguments.IsCopy)
               {
                  copyMoveArguments.CopyTimestamps = HasCopyTimestamps(copyMoveArguments.CopyOptions);

                  // Remove the AlphaFS flag since it is unknown to the native Win32 CopyFile/MoveFile functions.
                  if (copyMoveArguments.CopyTimestamps)
                     copyMoveArguments.CopyOptions &= ~CopyOptions.CopyTimestamp;


                  copyMoveArguments.GetSize = HasCopyOptionsGetSize(copyMoveArguments.CopyOptions);
                  
                  // Remove the AlphaFS flag since it is unknown to the native Win32 CopyFile/MoveFile functions.
                  if (copyMoveArguments.GetSize)
                     copyMoveArguments.CopyOptions &= ~CopyOptions.GetSizes;
               }

               else
               {
                  copyMoveArguments.GetSize = HasMoveOptionsGetSize(copyMoveArguments.MoveOptions);

                  // Remove the AlphaFS flag since it is unknown to the native Win32 CopyFile/MoveFile functions.
                  if (copyMoveArguments.GetSize)
                     copyMoveArguments.MoveOptions &= ~MoveOptions.GetSizes;
               }
            }


            // Setup callback function for progress notifications.

            if (null == copyMoveArguments.Routine && null != copyMoveArguments.ProgressHandler)
            {
               copyMoveArguments.Routine = (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, streamNumber, callbackReason, sourceFile, destinationFile, data) =>

                     copyMoveArguments.ProgressHandler(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, (int) streamNumber, callbackReason, copyMoveArguments.UserProgressData);
            }


            copyMoveArguments.PathFormat = PathFormat.LongFullPath;

            copyMoveArguments.PathsChecked = true;
         }
         

         return copyMoveArguments;
      }
   }
}

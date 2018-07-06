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
      internal static CopyMoveArguments ValidateCopyMoveArguments(CopyMoveArguments cma, string sourcePath, string destinationPath, out string sourcePathLp, out string destinationPathLp)
      {
         sourcePathLp = sourcePath;
         destinationPathLp = destinationPath;

         if (cma.PathsValidatedAndUpdated)
            return cma;


         if (null != sourcePath && sourcePath.Trim().Length == 0)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "sourcePath");

         if (null != destinationPath && destinationPath.Trim().Length == 0)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "destinationPath");


         // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
         // Do not use StringComparison.OrdinalIgnoreCase to allow renaming a folder with different casing.

         if (null != sourcePath && sourcePath.Equals(destinationPath, StringComparison.Ordinal))
            NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPath);
         

         cma.IsCopy = IsCopyAction(cma.CopyOptions, cma.MoveOptions);

         var isMove = !cma.IsCopy;
         cma.EmulateMove = false;

         cma.DelayUntilReboot = isMove && VerifyDelayUntilReboot(sourcePath, cma.MoveOptions, cma.PathFormat);

         // When destinationPath is null, the file/folder needs to be removed on Computer startup.
         cma.DeleteOnStartup = cma.DelayUntilReboot && null == destinationPath;
         

         if (cma.PathFormat != PathFormat.LongFullPath)
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


            cma.PreserveDates = HasPreserveDates(cma.CopyOptions);

            // Remove the AlphaFS flag since it is unknown to the native Win32 CopyFile/MoveFile.

            if (cma.PreserveDates)
               cma.CopyOptions &= ~CopyOptions.PreserveDates;


            cma.PathFormat = PathFormat.LongFullPath;

            cma.PathsValidatedAndUpdated = true;
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
            if (HasCopyAllowed(moveOptions))
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

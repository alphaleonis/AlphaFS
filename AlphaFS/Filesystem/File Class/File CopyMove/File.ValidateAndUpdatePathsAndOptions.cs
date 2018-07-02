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
      internal static CopyMoveArguments ValidateAndUpdatePathsAndOptions(CopyMoveArguments cma, string sourcePath, string destinationPath, out string sourcePathLp, out string destinationPathLp)
      {
         if (cma.PathsValidatedAndUpdated)
         {
            sourcePathLp = sourcePath;
            destinationPathLp = destinationPath;
            return cma;
         }


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

            cma.PathsValidatedAndUpdated = true;
         }

         else
         {
            cma.SourcePathLp = sourcePathLp;
            cma.DestinationPathLp = destinationPathLp;
         }


         return cma;
      }
   }
}

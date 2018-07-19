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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      [SecurityCritical]
      internal static void ValidateMoveAction(CopyMoveArguments copyMoveArguments)
      {
         // Determine if a Move action or Copy action-fallback is possible.

         copyMoveArguments.IsCopy = false;
         copyMoveArguments.EmulateMove = false;
         

         // Compare the root part of both paths.

         var equalRootPaths = Path.GetPathRoot(copyMoveArguments.SourcePathLp, false).Equals(Path.GetPathRoot(copyMoveArguments.DestinationPathLp, false), StringComparison.OrdinalIgnoreCase);
         

         // Method Volume.IsSameVolume() returns true when both paths refer to the same volume, even if one of the paths is a UNC path.
         // For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst

         var isSameVolume = equalRootPaths || Volume.IsSameVolume(copyMoveArguments.SourcePathLp, copyMoveArguments.DestinationPathLp);
         
         var isMove = isSameVolume && equalRootPaths;

         if (!isMove)
         {
            // A Move() can be emulated by using Copy() and Delete(), but only if the MoveOptions.CopyAllowed flag is set.

            isMove = File.HasCopyAllowed(copyMoveArguments.MoveOptions);


            // MSDN: .NET3.5+: IOException: An attempt was made to move a directory to a different volume.

            if (!isMove)
               NativeError.ThrowException(Win32Errors.ERROR_NOT_SAME_DEVICE, copyMoveArguments.SourcePathLp, copyMoveArguments.DestinationPathLp);
         }


         // The MoveFileXxx methods fail when:
         // - A directory is being moved;
         // - One of the paths is a UNC path, even though both paths refer to the same volume.
         //   For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst

         if (isMove)
         {
            var srcIsUncPath = Path.IsUncPathCore(copyMoveArguments.SourcePathLp, false, false);
            var dstIsUncPath = Path.IsUncPathCore(copyMoveArguments.DestinationPathLp, false, false);

            isMove = srcIsUncPath == dstIsUncPath;
         }


         isMove = isMove && isSameVolume && equalRootPaths;


         // Emulate Move().
         if (!isMove)
         {
            copyMoveArguments.IsCopy = true;
            copyMoveArguments.EmulateMove = true;

            copyMoveArguments.CopyOptions = File.HasMoveOptionsGetSize(copyMoveArguments.MoveOptions) ? CopyOptions.GetSizes : CopyOptions.None;
            
            copyMoveArguments.MoveOptions = null;
         }
      }
   }
}

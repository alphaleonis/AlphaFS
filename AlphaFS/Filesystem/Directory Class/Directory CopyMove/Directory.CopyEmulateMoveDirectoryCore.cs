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

using System.Collections.Generic;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      [SecurityCritical]
      internal static void CopyEmulateMoveDirectoryCore(ErrorHandler errorFilter, bool retry, CopyMoveArguments cma, CopyMoveResult copyMoveResult)
      {
         var dirs = new Queue<string>(NativeMethods.DefaultFileBufferSize);

         dirs.Enqueue(cma.SourcePathLp);


         while (dirs.Count > 0)
         {
            var srcLp = dirs.Dequeue();

            // TODO 2018-01-09: Not 100% yet with local + UNC paths.
            var dstLp = srcLp.Replace(cma.SourcePathLp, cma.DestinationPathLp);
            

            // Traverse the source folder, processing files and folders.

            foreach (var fseiSource in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, cma.Transaction, srcLp, Path.WildcardStarMatchAll, null, null, cma.DirectoryEnumerationFilters, PathFormat.LongFullPath))
            {
               var fseiSourcePath = fseiSource.LongFullPath;

               var fseiDestinationPath = Path.CombineCore(false, dstLp, fseiSource.FileName);

               if (fseiSource.IsDirectory)
               {
                  CreateDirectoryCore(true, cma.Transaction, fseiDestinationPath, null, null, false, PathFormat.LongFullPath);

                  copyMoveResult.TotalFolders++;

                  dirs.Enqueue(fseiSourcePath);
               }

               else
               {
                  // File count is done in File.CopyMoveCore method.

                  File.CopyMoveCore(errorFilter, retry, cma, true, false, fseiSourcePath, fseiDestinationPath, copyMoveResult);

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

                     if (cma.EmulateMove)
                        File.DeleteFileCore(cma.Transaction, fseiSourcePath, true, PathFormat.LongFullPath);
                  }
               }
            }
         }


         if (!copyMoveResult.IsCanceled && copyMoveResult.ErrorCode == Win32Errors.NO_ERROR)
         {
            if (cma.CopyTimestamps)
               CopyFolderTimestamps(cma);

            if (cma.EmulateMove)
               DeleteDirectoryCore(cma.Transaction, null, cma.SourcePathLp, true, true, true, PathFormat.LongFullPath);
         }
      }


      private static void CopyFolderTimestamps(CopyMoveArguments cma)
      {
         // TODO 2018-01-09: Not 100% yet with local + UNC paths.
         var dstLp = cma.SourcePathLp.Replace(cma.SourcePathLp, cma.DestinationPathLp);


         // Traverse the source folder, processing only folders.

         foreach (var fseiSource in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(true, cma.Transaction, cma.SourcePathLp, Path.WildcardStarMatchAll, null, null, cma.DirectoryEnumerationFilters, PathFormat.LongFullPath))
         { 
            File.CopyTimestampsCore(cma.Transaction, fseiSource.LongFullPath, Path.CombineCore(false, dstLp, fseiSource.FileName), false, PathFormat.LongFullPath);
         }

         
         // Process the root directory, the given path.

         File.CopyTimestampsCore(cma.Transaction, cma.SourcePathLp, cma.DestinationPathLp, false, PathFormat.LongFullPath);


         // TODO: When enabled on Computer, FindFirstFile will change the last accessed time.
      }
   }
}

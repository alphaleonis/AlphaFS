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
   public static partial class File
   {
      /// <summary>Deletes a Non-/Transacted file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="deleteArguments"></param>
      /// <param name="deleteResult"></param>
      [SecurityCritical]
      internal static DeleteResult DeleteFileCore(DeleteArguments deleteArguments, DeleteResult deleteResult)
      {
         #region Setup

         if (null == deleteArguments)
            throw new ArgumentNullException("deleteArguments");


         var pathLp = deleteArguments.TargetFsoPathLp ?? deleteArguments.TargetFsoPath;

         if (!deleteArguments.PathsChecked)
         {
            if (deleteArguments.PathFormat == PathFormat.RelativePath)
               Path.CheckSupportedPathFormat(pathLp, true, true);

            pathLp = Path.GetExtendedLengthPathCore(deleteArguments.Transaction, pathLp, deleteArguments.PathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

            if (null == pathLp)
               throw new ArgumentNullException("deleteArguments.TargetFsoPath");

            deleteArguments.PathsChecked = true;
         }
         

         var isSingleFileAction = null == deleteResult;

         deleteResult = deleteResult ?? new DeleteResult(false, pathLp);

         // Calling start on a running Stopwatch is a no-op.
         deleteResult.Stopwatch.Start();


         // We take an extra hit by getting the file size for a single file delete action.

         if (isSingleFileAction && deleteArguments.GetSize)
            deleteResult.TotalBytes = GetSizeCore(null, deleteArguments.Transaction, false, pathLp, true, PathFormat.LongFullPath);

         #endregion // Setup


         DeleteFileNative(pathLp, deleteArguments);

         deleteResult.TotalFiles++;


         if (isSingleFileAction)
            deleteResult.Stopwatch.Stop();


         return deleteResult;
      }
   }
}

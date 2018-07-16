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
using System.ComponentModel;
using System.Security;
using System.Threading;

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
      /// <param name="retry"></param>
      /// <param name="deleteArguments"></param>
      /// <param name="deleteResult"></param>
      [SecurityCritical]
      internal static DeleteResult DeleteFileCore(bool retry, DeleteArguments deleteArguments, DeleteResult deleteResult)
      {
         #region Setup

         if (null == deleteArguments)
            throw new ArgumentNullException("deleteArguments");


         var pathLp = deleteArguments.TargetFsoPathLp ?? deleteArguments.TargetFsoPath;

         if (!deleteArguments.PathsChecked)
         {
            if (null == pathLp)
               throw new ArgumentNullException("deleteArguments.TargetFsoPath");

            if (deleteArguments.PathFormat == PathFormat.RelativePath)
               Path.CheckSupportedPathFormat(pathLp, true, true);

            pathLp = Path.GetExtendedLengthPathCore(deleteArguments.Transaction, pathLp, deleteArguments.PathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);
            
            deleteArguments.PathsChecked = true;
         }
         

         var isSingleFileAction = null == deleteResult;

         if (null == deleteResult)
            deleteResult = new DeleteResult(false, pathLp);


         var attempts = 1;

         var retryTimeout = 0;

         var errorFilter = null != deleteArguments.DirectoryEnumerationFilters && null != deleteArguments.DirectoryEnumerationFilters.ErrorFilter ? deleteArguments.DirectoryEnumerationFilters.ErrorFilter : null;

         if (retry)
         {
            if (null != errorFilter)
            {
               attempts += deleteArguments.DirectoryEnumerationFilters.ErrorRetry;

               retryTimeout = deleteArguments.DirectoryEnumerationFilters.ErrorRetryTimeout;
            }

            else
            {
               if (deleteArguments.Retry <= 0)
                  deleteArguments.Retry = 2;

               if (deleteArguments.RetryTimeout <= 0)
                  deleteArguments.RetryTimeout = 10;

               attempts += deleteArguments.Retry;

               retryTimeout = deleteArguments.RetryTimeout;
            }
         }


         // Calling start on a running Stopwatch is a no-op.
         deleteResult.Stopwatch.Start();


         // We take an extra hit by getting the file size for a single file delete action.

         if (isSingleFileAction && deleteArguments.GetSize)
            deleteResult.TotalBytes = GetSizeCore(null, deleteArguments.Transaction, false, pathLp, true, PathFormat.LongFullPath);

         #endregion // Setup
         

         while (attempts-- > 0)
         {
            deleteResult.ErrorCode = (int) Win32Errors.NO_ERROR;


            int lastError;

            if (DeleteFileNative(null != errorFilter, pathLp, deleteArguments, out lastError))
            {
               deleteResult.ErrorCode = lastError;
               deleteResult.TotalFiles++;
            }
            
            else
            {
               deleteResult.ErrorCode = lastError;


               // Report the Exception back to the caller.
               if (null != errorFilter)
               {
                  var continueDelete = errorFilter(lastError, new Win32Exception(lastError).Message, Path.GetCleanExceptionPath(pathLp));

                  if (!continueDelete)
                     break;
               }

               if (retry)
                  deleteResult.Retries++;

               retry = attempts > 0 && retryTimeout > 0;

               if (retry)
               {
                  if (null != errorFilter && null != deleteArguments.DirectoryEnumerationFilters.CancellationToken)
                  {
                     if (deleteArguments.DirectoryEnumerationFilters.CancellationToken.WaitHandle.WaitOne(retryTimeout * 1000))
                        break;
                  }

                  else
                     Utils.Sleep(retryTimeout);
               }
            }
         }


         if (isSingleFileAction)
            deleteResult.Stopwatch.Stop();


         return deleteResult;
      }
   }
}

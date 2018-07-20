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
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>Deletes a Non-/Transacted file.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="deleteArguments"></param>
      /// <param name="deleteResult"></param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
      [SecurityCritical]
      internal static DeleteResult DeleteFileCore(DeleteArguments deleteArguments, DeleteResult deleteResult = null)
      {
         #region Setup

         if (null == deleteArguments)
            throw new ArgumentNullException("deleteArguments");

         DirectoryEnumerationFilters filters = null;
         ErrorHandler errorFilter = null;
         var errorFilterActive = false;

         var pathLp = deleteArguments.TargetPathLongPath ?? deleteArguments.TargetPath;


         if (!deleteArguments.PathsChecked)
         {
            if (null == pathLp)
               throw new ArgumentNullException("deleteArguments.TargetPath");

            if (deleteArguments.PathFormat == PathFormat.RelativePath)
               Path.CheckSupportedPathFormat(pathLp, true, true);

            pathLp = Path.GetExtendedLengthPathCore(deleteArguments.Transaction, pathLp, deleteArguments.PathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

            if (null == pathLp)
               return null;


            filters = deleteArguments.DirectoryEnumerationFilters;

            errorFilter = null != filters && null != filters.ErrorFilter ? filters.ErrorFilter : null;

            errorFilterActive = null != errorFilter;


            deleteArguments.PathsChecked = true;
         }

         
         var isSingleFileAction = null == deleteResult;

         var getSize = isSingleFileAction && deleteArguments.GetSize;


         if (null == deleteResult)
            deleteResult = new DeleteResult(false, Path.GetRegularPathCore(pathLp, GetFullPathOptions.None, false));

         
         var attempts = 1;

         var retryTimeout = 0;
         
         var retry = null != filters && (filters.ErrorRetry > 0 || filters.ErrorRetryTimeout > 0);

         if (retry)
         {
            if (errorFilterActive)
            {
               attempts += filters.ErrorRetry;

               retryTimeout = filters.ErrorRetryTimeout;
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
         
         #endregion // Setup


         while (attempts-- > 0)
         {
            deleteResult.ErrorCode = (int) Win32Errors.NO_ERROR;

            
            // We take an extra hit by getting the file size for a single file delete action.

            if (getSize)
               deleteResult.TotalBytes = GetSizeCore(null, deleteArguments.Transaction, false, pathLp, true, PathFormat.LongFullPath);


            int lastError;

            var success = DeleteFileNative(pathLp, errorFilterActive, deleteArguments, out lastError);

            deleteResult.ErrorCode = lastError;
            

            if (success)
               deleteResult.TotalFiles++;
            
            else
            {
               // Report the Exception back to the caller.
               if (errorFilterActive)
               {
                  var continueDelete = errorFilter(lastError, new Win32Exception(lastError).Message + (retry ? " Retry active: " + retry.ToString() + " , attempts." + deleteResult.Retries : string.Empty), Path.GetCleanExceptionPath(pathLp));

                  if (!continueDelete)
                     break;
               }
               

               if (retry)
                  deleteResult.Retries++;


               retry = attempts > 0 && retryTimeout > 0;

               if (retry)
               {
                  if (errorFilterActive)
                  {
                     if (filters.CancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(retryTimeout)))
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

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
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>The RemoveDirectory function marks a directory for deletion on close. Therefore, the directory is not removed until the last handle to the directory is closed.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="deleteArguments"></param>
      /// <param name="deleteResult"></param>
      [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "retryTimeout")]
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
      [SecurityCritical]
      internal static DeleteResult DeleteDirectoryCore(DeleteArguments deleteArguments, DeleteResult deleteResult = null)
      {
         #region Setup
         
         if (null == deleteArguments)
            throw new ArgumentNullException("deleteArguments");

         DirectoryEnumerationFilters filters = null;
         ErrorHandler errorFilter;
         var errorFilterActive = false;

         var fseiSource = deleteArguments.EntryInfo;


         if (!deleteArguments.PathsChecked)
         {
            var pathLp = deleteArguments.TargetPathLongPath ?? deleteArguments.TargetPath;

            if (null == pathLp && null == fseiSource)
               throw new ArgumentNullException("deleteArguments.TargetPath");

            if (null == fseiSource)
            {
               if (deleteArguments.PathFormat == PathFormat.RelativePath)
                  Path.CheckSupportedPathFormat(pathLp, true, true);

               pathLp = Path.GetExtendedLengthPathCore(deleteArguments.Transaction, pathLp, deleteArguments.PathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

               if (null == pathLp)
                  return null;
               
               fseiSource = File.GetFileSystemEntryInfoCore(deleteArguments.Transaction, true, pathLp, deleteArguments.ContinueOnNotFound, PathFormat.LongFullPath);

               if (null == fseiSource)
                  return null;
            }
            

            filters = deleteArguments.DirectoryEnumerationFilters;

            errorFilter = null != filters && null != filters.ErrorFilter ? filters.ErrorFilter : null;

            errorFilterActive = null != errorFilter;
            

            deleteArguments.PathsChecked = true;
         }

         else if (null == fseiSource)
            return null;


         if (null == deleteResult)
            deleteResult = new DeleteResult(fseiSource.IsDirectory, fseiSource.FullPath);

         
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


         PrepareDirectoryForDelete(fseiSource, deleteArguments.Transaction, deleteArguments.IgnoreReadOnly);


         // Do not follow mount points nor symbolic links, but do delete the reparse point itself.
         // If directory is reparse point, disable recursion.

         if (deleteArguments.Recursive && !fseiSource.IsReparsePoint)
         {
            // The stack will contain the entire folder structure to prevent any open directory handles because of enumeration.
            // The root folder is at the bottom of the stack.

            var dirs = new Stack<string>(NativeMethods.DefaultFileBufferSize);

            var delArgs = Utils.CopyFrom(deleteArguments);

            
            foreach (var fsei in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, deleteArguments.Transaction, fseiSource.LongFullPath, Path.WildcardStarMatchAll, null, DirectoryEnumerationOptions.Recursive, filters, PathFormat.LongFullPath))
            {
               PrepareDirectoryForDelete(fsei, deleteArguments.Transaction, deleteArguments.IgnoreReadOnly);

               if (deleteArguments.GetSize)
                  deleteResult.TotalBytes += File.GetSizeCore(null, deleteArguments.Transaction, fsei.IsDirectory, fsei.LongFullPath, true, PathFormat.LongFullPath);


               if (fsei.IsDirectory)
                  dirs.Push(fsei.LongFullPath);

               else
               {
                  delArgs.TargetPathLongPath = fsei.LongFullPath;

                  delArgs.Attributes = fsei.Attributes;
                  
                  File.DeleteFileCore(delArgs, deleteResult);
               }
            }


            while (dirs.Count > 0)
            {
               delArgs.TargetPathLongPath = dirs.Pop();

               delArgs.Attributes = 0;

               DeleteDirectoryNative(delArgs);

               deleteResult.TotalFolders++;
            }
         }


         DeleteDirectoryNative(new DeleteArguments(fseiSource));

         deleteResult.TotalFolders++;


         deleteResult.Stopwatch.Stop();

         return deleteResult;
      }


      private static void PrepareDirectoryForDelete(FileSystemEntryInfo fsei, KernelTransaction transaction, bool ignoreReadOnly)
      {
         // Check to see if the folder is a mount point and unmount it. Only then is it safe to delete the actual folder.

         if (fsei.IsMountPoint)

            DeleteJunctionCore(fsei, transaction, null, false, PathFormat.LongFullPath);


         // Reset attributes to Normal if we already know the facts.

         if (ignoreReadOnly && (fsei.IsReadOnly || fsei.IsHidden))

            File.SetAttributesCore(transaction, fsei.IsDirectory, fsei.LongFullPath, FileAttributes.Normal, PathFormat.LongFullPath);
      }
   }
}

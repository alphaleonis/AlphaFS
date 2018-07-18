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
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <remarks>The RemoveDirectory function marks a directory for deletion on close. Therefore, the directory is not removed until the last handle to the directory is closed.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="continueOnNotFound"></param>
      /// <param name="fsEntryInfo"></param>
      /// <param name="deleteArguments"></param>
      /// <param name="deleteResult"></param>
      [SecurityCritical]
      internal static DeleteResult DeleteDirectoryCore(bool continueOnNotFound, FileSystemEntryInfo fsEntryInfo, DeleteArguments deleteArguments, DeleteResult deleteResult)
      {
         #region Setup
         
         if (null == deleteArguments)
            throw new ArgumentNullException("deleteArguments");

         DirectoryEnumerationFilters filters = null;
         ErrorHandler errorFilter;
         var errorFilterActive = false;


         var pathLp = deleteArguments.TargetFsoPathLp ?? deleteArguments.TargetFsoPath;


         if (!deleteArguments.PathsChecked)
         {
            if (null == pathLp && null == fsEntryInfo)
               throw new ArgumentNullException("deleteArguments.TargetFsoPath");

            if (null == fsEntryInfo)
            {
               if (deleteArguments.PathFormat == PathFormat.RelativePath)
                  Path.CheckSupportedPathFormat(pathLp, true, true);

               pathLp = Path.GetExtendedLengthPathCore(deleteArguments.Transaction, pathLp, deleteArguments.PathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

               if (null == pathLp)
                  return null;


               fsEntryInfo = File.GetFileSystemEntryInfoCore(deleteArguments.Transaction, true, pathLp, false, PathFormat.LongFullPath);

               if (null == fsEntryInfo)
                  return null;
            }
            

            filters = deleteArguments.DirectoryEnumerationFilters;

            errorFilter = null != filters && null != filters.ErrorFilter ? filters.ErrorFilter : null;

            errorFilterActive = null != errorFilter;
            

            deleteArguments.PathsChecked = true;
         }

         else if (null == fsEntryInfo)
            return null;


         if (null == deleteResult)
            deleteResult = new DeleteResult(fsEntryInfo.IsDirectory, fsEntryInfo.FullPath);
         

         var attempts = 1;

         var retryTimeout = 0;


         //if (retry)
         //{
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
         //}


         // Calling start on a running Stopwatch is a no-op.
         deleteResult.Stopwatch.Start();

         #endregion // Setup


         PrepareDirectoryForDelete(deleteArguments.Transaction, fsEntryInfo, deleteArguments.IgnoreReadOnly);


         // Do not follow mount points nor symbolic links, but do delete the reparse point itself.
         // If directory is reparse point, disable recursion.

         if (deleteArguments.Recursive && !fsEntryInfo.IsReparsePoint)
         {
            // The stack will contain the entire folder structure to prevent any open directory handles because of enumeration.
            // The root folder is at the bottom of the stack.

            var dirs = new Stack<string>(NativeMethods.DefaultFileBufferSize);
            
            foreach (var fsei in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, deleteArguments.Transaction, fsEntryInfo.LongFullPath, Path.WildcardStarMatchAll, null, DirectoryEnumerationOptions.Recursive, filters, PathFormat.LongFullPath))
            {
               PrepareDirectoryForDelete(deleteArguments.Transaction, fsei, deleteArguments.IgnoreReadOnly);

               if (fsei.IsDirectory)
                  dirs.Push(fsei.LongFullPath);

               else
               {
                  deleteArguments.TargetFsoPathLp = fsei.LongFullPath;
                  deleteArguments.Attributes = fsei.Attributes;

                  File.DeleteFileCore(deleteArguments, deleteResult);

                  deleteResult.TotalBytes += fsei.FileSize;
               }
            }


            while (dirs.Count > 0)
            {
               DeleteDirectoryNative(deleteArguments.Transaction, dirs.Pop(), deleteArguments.IgnoreReadOnly, continueOnNotFound, 0);

               deleteResult.TotalFolders++;
            }
         }


         DeleteDirectoryNative(deleteArguments.Transaction, fsEntryInfo.LongFullPath, deleteArguments.IgnoreReadOnly, continueOnNotFound, fsEntryInfo.Attributes);

         deleteResult.TotalFolders++;


         deleteResult.Stopwatch.Stop();

         return deleteResult;
      }


      private static void PrepareDirectoryForDelete(KernelTransaction transaction, FileSystemEntryInfo fsei, bool ignoreReadOnly)
      {
         // Check to see if the folder is a mount point and unmount it. Only then is it safe to delete the actual folder.

         if (fsei.IsMountPoint)

            DeleteJunctionCore(transaction, fsei, null, false, PathFormat.LongFullPath);


         // Reset attributes to Normal if we already know the facts.

         if (ignoreReadOnly && (fsei.IsReadOnly || fsei.IsHidden))

            File.SetAttributesCore(transaction, fsei.IsDirectory, fsei.LongFullPath, FileAttributes.Normal, PathFormat.LongFullPath);
      }
   }
}

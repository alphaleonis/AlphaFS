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
   public static partial class Directory
   {
      #region Obsolete
      
      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      [Obsolete("Argument recursive and ignoreReadOnly are obsolete. Use overload that supports the DeleteArguments argument.")]
      [SecurityCritical]
      public static DeleteResult Delete(string path, bool recursive, bool ignoreReadOnly)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = path,

            Recursive = recursive,

            IgnoreReadOnly = ignoreReadOnly,

         }, null);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [Obsolete("Argument recursive and ignoreReadOnly are obsolete. Use overload that supports the DeleteArguments argument.")]
      [SecurityCritical]
      public static DeleteResult Delete(string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = pathFormat != PathFormat.LongFullPath ? path : null,

            TargetFsoPathLp = pathFormat == PathFormat.LongFullPath ? path : null,

            Recursive = recursive,

            IgnoreReadOnly = ignoreReadOnly,

            PathFormat = pathFormat

         }, null);
      }

      #endregion // Obsolete


      #region .NET

      /// <summary>[.NET] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = path

         }, null);
      }


      /// <summary>[.NET] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      [SecurityCritical]
      public static void Delete(string path, bool recursive)
      {
         DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = path,

            Recursive = recursive

         }, null);
      }

      #endregion // .NET


      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult Delete(string path, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = pathFormat != PathFormat.LongFullPath ? path : null,

            TargetFsoPathLp = pathFormat == PathFormat.LongFullPath ? path : null,

            PathFormat = pathFormat

         }, null);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult Delete(string path, bool recursive, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = pathFormat != PathFormat.LongFullPath ? path : null,

            TargetFsoPathLp = pathFormat == PathFormat.LongFullPath ? path : null,

            Recursive = recursive,

            PathFormat = pathFormat

         }, null);
      }


      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="deleteArguments"></param>
      [SecurityCritical]
      public static DeleteResult Delete(DeleteArguments deleteArguments)
      {
         return DeleteDirectoryCore(null, deleteArguments, null);
      }

      


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static DeleteResult Delete(string path, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = pathFormat != PathFormat.LongFullPath ? path : null,

            TargetFsoPathLp = pathFormat == PathFormat.LongFullPath ? path : null,

            ContinueOnNotFound = (options & DirectoryEnumerationOptions.ContinueOnException) != 0,

            Recursive = (options & DirectoryEnumerationOptions.Recursive) != 0,

            PathFormat = pathFormat

         }, null);
      }


      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <param name="filters">The specification of custom filters to be used in the process.</param>
      [SecurityCritical]
      public static DeleteResult Delete(string path, DirectoryEnumerationFilters filters, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = pathFormat != PathFormat.LongFullPath ? path : null,

            TargetFsoPathLp = pathFormat == PathFormat.LongFullPath ? path : null,

            DirectoryEnumerationFilters = filters,

            PathFormat = pathFormat

         }, null);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="filters">The specification of custom filters to be used in the process.</param>
      [SecurityCritical]
      public static DeleteResult Delete(string path, DirectoryEnumerationOptions options, DirectoryEnumerationFilters filters, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(null, new DeleteArguments
         {
            TargetFsoPath = pathFormat != PathFormat.LongFullPath ? path : null,

            TargetFsoPathLp = pathFormat == PathFormat.LongFullPath ? path : null,

            ContinueOnNotFound = (options & DirectoryEnumerationOptions.ContinueOnException) != 0,

            Recursive = (options & DirectoryEnumerationOptions.Recursive) != 0,

            DirectoryEnumerationFilters = filters,

            PathFormat = pathFormat

         }, null);
      }
   }
}

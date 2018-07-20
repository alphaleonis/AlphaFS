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
      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path)
      {
         return DeleteDirectoryCore(new DeleteArguments {Transaction = transaction, TargetPath = path});
      }


      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            PathFormat = pathFormat
         });
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool recursive)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            Recursive = recursive
         });
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool recursive, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            Recursive = recursive,
            PathFormat = pathFormat
         });
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            Recursive = recursive,
            IgnoreReadOnly = ignoreReadOnly
         });
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            Recursive = recursive,
            IgnoreReadOnly = ignoreReadOnly,
            PathFormat = pathFormat
         });
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="getSize"><c>true</c> to retrieve the directory size; the sum of all streams.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool getSize)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            Recursive = recursive,
            IgnoreReadOnly = ignoreReadOnly,
            GetSize = getSize
         });
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in <paramref name="path"/>. <c>false</c> otherwise.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="getSize"><c>true</c> to retrieve the directory size; the sum of all streams.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool getSize, PathFormat pathFormat)
      {
         return DeleteDirectoryCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            Recursive = recursive,
            IgnoreReadOnly = ignoreReadOnly,
            GetSize = getSize,
            PathFormat = pathFormat
         });
      }
   }
}

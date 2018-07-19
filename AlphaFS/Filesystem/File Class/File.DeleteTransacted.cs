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
      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path)
      {
         return DeleteFileCore(new DeleteArguments {Transaction = transaction, TargetPath = path});
      }


      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return DeleteFileCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            PathFormat = pathFormat
         });
      }


      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>      
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool ignoreReadOnly)
      {
         return DeleteFileCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            IgnoreReadOnly = ignoreReadOnly
         });
      }


      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         return DeleteFileCore(new DeleteArguments
         {
            Transaction = transaction,
            TargetPath = path,
            IgnoreReadOnly = ignoreReadOnly,
            PathFormat = pathFormat
         });
      }


      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <returns>A <see cref="DeleteResult"/> instance with details of the Delete action.</returns>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="deleteArguments">A <see cref="DeleteArguments"/> instance.</param>
      [SecurityCritical]
      public static DeleteResult DeleteTransacted(KernelTransaction transaction, string path, DeleteArguments deleteArguments)
      {
         deleteArguments.Transaction = transaction;
         deleteArguments.TargetPath = path;

         return DeleteFileCore(deleteArguments);
      }
   }
}

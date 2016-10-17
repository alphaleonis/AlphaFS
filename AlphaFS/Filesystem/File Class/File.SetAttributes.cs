/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region SetAttributes

      /// <summary>Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
      /// <overloads>Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.</overloads>
      [SecurityCritical]
      public static void SetAttributes(string path, FileAttributes fileAttributes)
      {
         SetAttributesCore(false, null, path, fileAttributes, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetAttributes(string path, FileAttributes fileAttributes, PathFormat pathFormat)
      {
         SetAttributesCore(false, null, path, fileAttributes, pathFormat);
      }


      #region Transactional

      /// <summary>[AlphaFS] Sets the specified <see cref="FileAttributes"/> of the file on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>      
      [SecurityCritical]
      public static void SetAttributesTransacted(KernelTransaction transaction, string path, FileAttributes fileAttributes)
      {
         SetAttributesCore(false, transaction, path, fileAttributes, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Sets the specified <see cref="FileAttributes"/> of the file on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetAttributesTransacted(KernelTransaction transaction, string path, FileAttributes fileAttributes, PathFormat pathFormat)
      {
         SetAttributesCore(false, transaction, path, fileAttributes, pathFormat);
      }

      #endregion // Transacted

      #endregion // SetAttributes

      #region Internal Methods

      /// <summary>Sets the attributes for a Non-/Transacted file/directory.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using the SetAttributes method.
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file or directory whose attributes are to be set.</param>
      /// <param name="fileAttributes">
      ///   The attributes to set for the file or directory. Note that all other values override <see cref="FileAttributes.Normal"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void SetAttributesCore(bool isFolder, KernelTransaction transaction, string path, FileAttributes fileAttributes, PathFormat pathFormat)
      {
         var pathLp = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

         var success = transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // SetFileAttributes()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.SetFileAttributes(pathLp, fileAttributes)
            : NativeMethods.SetFileAttributesTransacted(pathLp, fileAttributes, transaction.SafeHandle);


         var lastError = (uint) Marshal.GetLastWin32Error();
         if (!success)
         {
            switch (lastError)
            {
               // MSDN: .NET 3.5+: ArgumentException: FileSystemInfo().Attributes
               case Win32Errors.ERROR_INVALID_PARAMETER:
                  throw new ArgumentException(Resources.Invalid_File_Attribute);

               case Win32Errors.ERROR_FILE_NOT_FOUND:
                  if (isFolder)
                     lastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;

                  // MSDN: .NET 3.5+: DirectoryNotFoundException: The specified path is invalid, (for example, it is on an unmapped drive).
                  // MSDN: .NET 3.5+: FileNotFoundException: The file cannot be found.
                  NativeError.ThrowException(lastError, pathLp);
                  break;
            }

            NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // Internal Methods
   }
}

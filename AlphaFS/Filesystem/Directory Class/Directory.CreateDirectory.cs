/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region .NET

      /// <summary>Creates all directories and subdirectories in the specified path unless they already exist.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path)
      {
         return CreateDirectoryInternal(null, path, null, null, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      #region AlphaFS

      #region Non-Transactional

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, null, null, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, bool compress)
      {
         return CreateDirectoryInternal(null, path, null, null, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, null, null, compress, pathFormat);
      }



      /// <summary>Creates all the directories in the specified path, unless the already exist, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, compress, pathFormat);
      }



      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, bool compress)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, compress, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, compress, pathFormat);
      }
      
      #endregion // Non-Transactional

      #region Transactional
      
      /// <summary>Creates all directories and subdirectories in the specified path unless they already exist.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path)
      {
         return CreateDirectoryInternal(transaction, path, null, null, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, null, null, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, null, null, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, null, null, compress, pathFormat);
      }



      /// <summary>Creates all the directories in the specified path, unless the already exist, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, compress, pathFormat);
      }



      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, compress, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, false, pathFormat);
      }



      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, compress, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity, bool compress, PathFormat pathFormat)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, compress, pathFormat);
      }

      #endregion // Transactional
      
      #region Internal Methods

      /// <summary>[AlphaFS] Unified method CreateDirectoryInternal() to create a new directory with the attributes of a specified template directory (if one is specified). 
      /// If the underlying file system supports security on files and directories, the function
      /// applies the specified security descriptor to the new directory. The new directory retains
      /// the other attributes of the specified template directory.
      /// </summary>
      /// <returns>
      /// <para>Returns an object that represents the directory at the specified path.</para>
      /// <para>This object is returned regardless of whether a directory at the specified path already exists.</para>
      /// </returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory. May be <see langword="null"/> to indicate that no template should be used.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> access control to apply to the directory, may be null.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static DirectoryInfo CreateDirectoryInternal(KernelTransaction transaction, string path, string templatePath, ObjectSecurity directorySecurity, bool compress, PathFormat pathFormat)
      {
         if (pathFormat == PathFormat.RelativePath)
         {
            if (path != null && path[0] == Path.VolumeSeparatorChar)
               throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.PathFormatUnsupported, path));

            if (templatePath != null && templatePath[0] == Path.VolumeSeparatorChar)
               throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.PathFormatUnsupported, templatePath));

            Path.CheckValidPath(path, true, true);
            Path.CheckValidPath(templatePath, true, true);
         }
         else
            // MSDN:. NET 3.5+: NotSupportedException: Path contains a colon character (:) that is not part of a drive label ("C:\").
            Path.CheckValidPath(path, false, false);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

         // Return DirectoryInfo instance if the directory specified by path already exists.
         if (File.ExistsInternal(true, transaction, pathLp, PathFormat.LongFullPath))
            return new DirectoryInfo(transaction, pathLp, PathFormat.LongFullPath);

         // MSDN: .NET 3.5+: IOException: The directory specified by path is a file or the network name was not found.
         if (File.ExistsInternal(false, transaction, pathLp, PathFormat.LongFullPath))
            NativeError.ThrowException(Win32Errors.ERROR_ALREADY_EXISTS, pathLp, true);


         string templatePathLp = Utils.IsNullOrWhiteSpace(templatePath)
            ? null
            : Path.GetExtendedLengthPathInternal(transaction, templatePath, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

         #region Construct Full Path

         string longPathPrefix = Path.IsUncPath(path, false) ? Path.LongPathUncPrefix : Path.LongPathPrefix;
         path = Path.GetRegularPathInternal(pathLp, false, false, false, false);

         int length = path.Length;
         if (length >= 2 && Path.IsDVsc(path[length - 1], false))
            --length;

         int rootLength = Path.GetRootLength(path, false);
         if (length == 2 && Path.IsDVsc(path[1], false))
            throw new ArgumentException(Resources.CannotCreateDirectory, path);


         // Check if directories are missing.
         Stack<string> list = new Stack<string>(100);

         if (length > rootLength)
         {
            for (int index = length - 1; index >= rootLength; --index)
            {
               string path1 = path.Substring(0, index + 1);
               string path2 = longPathPrefix + path1.TrimStart('\\');

               if (!File.ExistsInternal(true, transaction, path2, PathFormat.LongFullPath))
                  list.Push(path2);

               while (index > rootLength && !Path.IsDVsc(path[index], false))
                  --index;
            }
         }

         #endregion // Construct Full Path

         // Directory security.
         using (Security.NativeMethods.SecurityAttributes securityAttributes = new Security.NativeMethods.SecurityAttributes(directorySecurity))
         {
            // Create the directory paths.
            while (list.Count > 0)
            {
               string folderLp = list.Pop();

               // In the ANSI version of this function, the name is limited to 248 characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-01-13: MSDN confirms LongPath usage.

               bool createOk = (transaction == null || !NativeMethods.IsAtLeastWindowsVista
                  ? (templatePathLp == null
                     ? NativeMethods.CreateDirectory(folderLp, securityAttributes)
                     : NativeMethods.CreateDirectoryEx(templatePathLp, folderLp, securityAttributes))
                  : NativeMethods.CreateDirectoryTransacted(templatePathLp, folderLp, securityAttributes, transaction.SafeHandle));

               if (!createOk)
               {
                  int lastError = Marshal.GetLastWin32Error();

                  switch ((uint)lastError)
                  {
                     // MSDN: .NET 3.5+: If the directory already exists, this method does nothing.
                     // MSDN: .NET 3.5+: IOException: The directory specified by path is a file.
                     case Win32Errors.ERROR_ALREADY_EXISTS:
                        if (File.ExistsInternal(false, transaction, pathLp, PathFormat.LongFullPath))
                           NativeError.ThrowException(lastError, pathLp, true);

                        if (File.ExistsInternal(false, transaction, folderLp, PathFormat.LongFullPath))
                           NativeError.ThrowException(Win32Errors.ERROR_PATH_NOT_FOUND, folderLp, true);
                        break;

                     case Win32Errors.ERROR_BAD_NET_NAME:
                        NativeError.ThrowException(lastError, pathLp, true);
                        break;

                     case Win32Errors.ERROR_DIRECTORY:
                        // MSDN: .NET 3.5+: NotSupportedException: path contains a colon character (:) that is not part of a drive label ("C:\").
                        throw new NotSupportedException(String.Format(CultureInfo.CurrentCulture, Resources.PathFormatUnsupported, path));

                     default:
                        NativeError.ThrowException(lastError, folderLp);
                        break;
                  }
               }
               else if (compress)
                  Device.ToggleCompressionInternal(true, transaction, folderLp, true, PathFormat.LongFullPath);
            }
         }

         return new DirectoryInfo(transaction, pathLp, PathFormat.FullPath);
      }

      #endregion // CreateDirectoryInternal

      #endregion // AlphaFS
   }
}

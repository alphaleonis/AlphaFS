/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using SearchOption = System.IO.SearchOption;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Exposes static methods for creating, moving, and enumerating through directories and subdirectories.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   public static class Directory
   {
      #region .NET

      #region CreateDirectory

      #region .NET

      /// <summary>Creates all directories and subdirectories in the specified path unless they already exist.</summary>
      /// <param name="path">The directory to create.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path)
      {
         return CreateDirectoryInternal(null, path, null, null, false, false);
      }

      /// <summary>Creates all the directories in the specified path, unless the already exist, applying the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region Compress

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(null, path, null, null, compress, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, compress, isFullPath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, compress, isFullPath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, compress, isFullPath);
      }

      #endregion // Compress

      #endregion // IsFullPath

      #region Compress

      #region .NET

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, bool compress)
      {
         return CreateDirectoryInternal(null, path, null, null, compress, false);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(null, path, null, directorySecurity, compress, false);
      }

      #endregion // .NET

      #region Template

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, bool compress)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, compress, false);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, compress, false);
      }

      #endregion // Template

      #endregion // Compress

      #region Template

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath)
      {
         return CreateDirectoryInternal(null, path, templatePath, null, false, false);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(string path, string templatePath, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(null, path, templatePath, directorySecurity, false, false);
      }

      #endregion // Template
      
      #region Transacted

      #region IsFullPath

      #region Compress

      #region .NET

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(transaction, path, null, null, compress, isFullPath);
      }
      /// <summary>
      ///   [AlphaFS] Creates all the directories in the specified path, applying the specified Windows
      ///   security.
      /// </summary>
      /// <remarks>
      ///   MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/>
      ///   parameter before creating the directory.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///   <list type="definition">
      ///    <item>
      ///      <term>
      ///        <see langword="true" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="false" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> will be checked and resolved to an absolute path. Unicode
      ///        prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="null" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      ///   </description>
      ///    </item>
      ///   </list>
      /// </param>
      /// <returns>
      ///   An object that represents the directory at the specified path. This object is returned
      ///   regardless of whether a directory at the specified path already exists.
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, compress, isFullPath);
      }

      #endregion // .NET

      #region Template

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, compress, isFullPath);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity, bool compress, bool? isFullPath)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, compress, isFullPath);
      }

      #endregion // Template

      #endregion // Compress

      #endregion // IsFullPath

      #region .NET

      /// <summary>Creates all directories and subdirectories in the specified path unless they already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path)
      {
         return CreateDirectoryInternal(transaction, path, null, null, false, false);
      }

      /// <summary>Creates all the directories in the specified path, unless the already exist, applying the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, false, false);
      }

      #endregion // .NET

      #region Compress

      #region .NET

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, null, null, compress, false);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path, applying the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, null, directorySecurity, compress, false);
      }

      #endregion // .NET

      #region Template

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, compress, false);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, compress, false);
      }

      #endregion // Template

      #endregion // Compress

      #region Template

      /// <summary>[AlphaFS] Creates a new directory, with the attributes of a specified template directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, null, false, false);
      }

      /// <summary>[AlphaFS] Creates all the directories in the specified path of a specified template directory and applies the specified Windows security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> and <paramref name="templatePath"/> parameters before creating the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static DirectoryInfo CreateDirectory(KernelTransaction transaction, string path, string templatePath, DirectorySecurity directorySecurity)
      {
         return CreateDirectoryInternal(transaction, path, templatePath, directorySecurity, false, false);
      }

      #endregion // Template

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // CreateDirectory

      #region Delete

      #region .NET

      /// <summary>Deletes an empty directory from a specified path.</summary>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
       [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteDirectoryInternal(null, null, path, false, false, true, false, false);
      }

      /// <summary>Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      [SecurityCritical]
      public static void Delete(string path, bool recursive)
      {
         DeleteDirectoryInternal(null, null, path, recursive, false, !recursive, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, bool ignoreReadOnly, bool? isFullPath)
      {
         DeleteDirectoryInternal(null, null, path, recursive, ignoreReadOnly, !recursive, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>      
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteDirectoryInternal(null, null, path, recursive, ignoreReadOnly, !recursive, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool? isFullPath)
      {
         DeleteDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, !recursive, false, isFullPath);
      }

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>

      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path)
      {
         DeleteDirectoryInternal(null, transaction, path, false, false, true, false, false);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>

      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteDirectoryInternal(null, transaction, path, recursive, false, !recursive, false, false);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, !recursive, false, false);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Delete

      #region EnumerateDirectories

      #region .NET

      /// <summary>Returns an enumerable collection of directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, false);
      }

      /// <summary>Returns an enumerable collection of directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Folders, false);
      }

      /// <summary>Returns an enumerable collection of directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Folders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, enumOptions, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Returns an enumerable collection of directory instances in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of directory instances that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Folders, false);
      }

      /// <summary>Returns an enumerable collection of directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Folders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, enumOptions, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      #region .NET

      /// <summary>Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, false);
      }

      /// <summary>Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Files, false);
      }

      /// <summary>Returns an enumerable collection of file names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Files;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, enumOptions, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Files, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances instances that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Files;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, enumOptions, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // EnumerateFiles

      #region EnumerateFileSystemEntries

      #region .NET

      /// <summary>Returns an enumerable collection of file names and directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of <see cref="FileSystemEntryInfo"/> entries in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, enumOptions, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region Transacted

      /// <summary>Returns an enumerable collection of file names and directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of <see cref="FileSystemEntryInfo"/> entries in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, enumOptions, false);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // EnumerateFileSystemEntries

      #region Exists

      #region .NET
      /// <summary>
      ///   Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/>  parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine
      ///   if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      /// </remarks>
      /// <param name="path">The path to test.</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing directory.</para>
      ///   <para>Returns <see langword="false"/> if the directory does not exist or an error occurs when trying
      ///   to determine if the specified file exists.</para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(string path)
      {
         return File.ExistsInternal(true, null, path, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Determines whether the given path refers to an existing directory on disk.</summary>
      /// <returns>
      /// <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing directory.</para>
      /// <para>Returns <see langword="false"/> if the directory does not exist or an error occurs when trying to determine if the specified file exists.</para>
      /// </returns>
      /// <remarks>
      /// <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the <paramref name="path"/> parameter before checking whether the directory exists.</para>
      /// <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine if the specified file exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      /// <param name="path">The path to test.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static bool Exists(string path, bool? isFullPath)
      {
         return File.ExistsInternal(true, null, path, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET
      /// <summary>
      ///   [AlphaFS] Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/> parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to
      ///   determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      /// <param name="isFullPath">
      ///   <list type="definition">
      ///    <item>
      ///      <term>
      ///        <see langword="true" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="false" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> will be checked and resolved to an absolute path. Unicode
      ///        prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="null" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      ///   </description>
      ///    </item>
      ///   </list>
      /// </param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing
      ///   directory.</para>
      ///   <para>Returns <see langword="false"/> if the directory does not exist or an error occurs
      ///   when trying to determine if the specified file exists.</para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.ExistsInternal(true, transaction, path, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Determines whether the given path refers to an existing directory on disk.</summary>
      /// <returns>
      /// <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing directory.</para>
      /// <para>Returns <see langword="false"/> if the directory does not exist or an error occurs when trying to determine if the specified file exists.</para>
      /// </returns>
      /// <remarks>
      /// <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the <paramref name="path"/> parameter before checking whether the directory exists.</para>
      /// <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine if the specified file exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path)
      {
         return File.ExistsInternal(true, transaction, path, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Exists

      #region GetAccessControl

      #region .NET

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the specified directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>

      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, false);
      }

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory described by the <paramref name="path"/> parameter. </returns>

      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, includeSections, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the specified directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>

      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, bool? isFullPath)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, isFullPath);
      }

      /// <summary>[AlphaFS] Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory described by the <paramref name="path"/> parameter. </returns>

      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections, bool? isFullPath)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, includeSections, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // GetAccessControl

      #region GetCreationTime

      #region .NET

      /// <summary>Gets the creation date and time of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path)
      {
         return File.GetCreationTimeInternal(null, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path, bool? isFullPath)
      {
         return File.GetCreationTimeInternal(null, path, false, isFullPath).ToLocalTime();
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the creation date and time of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetCreationTimeInternal(transaction, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path)
      {
         return File.GetCreationTimeInternal(transaction, path, false, false).ToLocalTime();
      }

      #endregion //.NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetCreationTime

      #region GetCreationTimeUtc

      #region .NET

      /// <summary>Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path)
      {
         return File.GetCreationTimeInternal(null, path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path, bool? isFullPath)
      {
         return File.GetCreationTimeInternal(null, path, true, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetCreationTimeInternal(transaction, path, true, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetCreationTimeInternal(transaction, path, true, false);
      }

      #endregion //.NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetCreationTimeUtc

      #region GetCurrentDirectory (.NET)

      #region .NET

      /// <summary>Gets the current working directory of the application.</summary>
      /// <returns>The path of the current working directory without a backslash (\).</returns>

      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetCurrentDirectory()
      {
         return System.IO.Directory.GetCurrentDirectory();
      }

      #endregion // .NET

      #endregion // GetCurrentDirectory (.NET)

      #region GetDirectories

      #region .NET

      /// <summary>Returns the names of subdirectories (including their paths) in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) of subdirectories in the specified path, or an empty array if no directories are found.</returns>
      /// <remarks><para>The names returned by this method are prefixed with the directory information provided in path.</para>
      /// <para>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>      
      /// <param name="path">The directory to search.</param>
      [SecurityCritical]
      public static string[] GetDirectories(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, false).ToArray();
      }

      /// <summary>Returns the names of subdirectories (including their paths) that match the specified search pattern in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) of the subdirectories that match the search pattern in the specified directory, or an empty array if no directories are found.</returns>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetDirectories(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Folders, false).ToArray();
      }

      /// <summary>Returns the names of the subdirectories (including their paths) that match the specified search pattern in the specified directory, and optionally searches subdirectories.
      /// </summary>
      /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Folders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, enumOptions, false).ToArray();
      }

      #endregion // .NET

      #region AlphaFS

      #region Transacted

      #region .NET

      /// <summary>Returns the names of subdirectories (including their paths) in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) of subdirectories in the specified path, or an empty array if no directories are found.</returns>
      /// <remarks>The names returned by this method are prefixed with the directory information provided in path.</remarks>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      [SecurityCritical]
      public static string[] GetDirectories(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, false).ToArray();
      }

      /// <summary>Returns the names of subdirectories (including their paths) that match the specified search pattern in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) of the subdirectories that match the search pattern in the specified directory, or an empty array if no directories are found.</returns>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetDirectories(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Folders, false).ToArray();
      }

      /// <summary>Returns the names of the subdirectories (including their paths) that match the specified search pattern in the specified directory, and optionally searches subdirectories.
      /// </summary>
      /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetDirectories(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Folders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, enumOptions, false).ToArray();
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetDirectories

      #region GetDirectoryRoot

      #region .NET

      /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
      /// <param name="path">The path of a file or directory.</param>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      [SecurityCritical]
      public static string GetDirectoryRoot(string path)
      {
         return GetDirectoryRootInternal(null, path, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static string GetDirectoryRoot(string path, bool? isFullPath)
      {
         return GetDirectoryRootInternal(null, path, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static string GetDirectoryRoot(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetDirectoryRootInternal(transaction, path, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Returns the volume information, root information, or both for the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path of a file or directory.</param>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      [SecurityCritical]
      public static string GetDirectoryRoot(KernelTransaction transaction, string path)
      {
         return GetDirectoryRootInternal(transaction, path, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetDirectoryRoot

      #region GetFiles

      #region .NET

      /// <summary>Returns the names of files (including their paths) in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) for the files in the specified directory, or an empty array if no files are found.</returns>
      /// <remarks>The returned file names are appended to the supplied <paramref name="path"/> parameter.</remarks>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required. </remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory to search.</param>
      [SecurityCritical]
      public static string[] GetFiles(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, false).ToArray();
      }

      /// <summary>Returns the names of files (including their paths) that match the specified search pattern in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern, or an empty array if no files are found.</returns>
      /// <remarks>The returned file names are appended to the supplied <paramref name="path"/> parameter.</remarks>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required. </remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFiles(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Files, false).ToArray();
      }

      /// <summary>Gets the names of the subdirectories (including their paths) that match the specified search pattern in the current directory, and optionally searches subdirectories.
      /// </summary>
      /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern and option, or an empty array if no files are found.</returns>
      /// <remarks>The returned file names are appended to the supplied <paramref name="path"/> parameter.</remarks>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required. </remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Files;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, enumOptions, false).ToArray();
      }

      #endregion // .NET

      #region AlphaFS

      #region Transacted

      #region .NET

      /// <summary>Returns the names of files (including their paths) in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) for the files in the specified directory, or an empty array if no files are found.</returns>
      /// <remarks>The returned file names are appended to the supplied <paramref name="path"/> parameter.</remarks>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required. </remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      [SecurityCritical]
      public static string[] GetFiles(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, false).ToArray();
      }

      /// <summary>Returns the names of files (including their paths) that match the specified search pattern in the specified directory.
      /// </summary>
      /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern, or an empty array if no files are found.</returns>
      /// <remarks>The returned file names are appended to the supplied <paramref name="path"/> parameter.</remarks>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required. </remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFiles(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Files, false).ToArray();
      }

      /// <summary>Gets the names of the subdirectories (including their paths) that match the specified search pattern in the current directory, and optionally searches subdirectories.
      /// </summary>
      /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern and option, or an empty array if no files are found.</returns>
      /// <remarks>The returned file names are appended to the supplied <paramref name="path"/> parameter.</remarks>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required. </remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFiles(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Files;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, enumOptions, false).ToArray();
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetFiles

      #region GetFileSystemEntries

      #region .NET

      /// <summary>Returns the names of all files and subdirectories in the specified directory.
      /// </summary>
      /// <returns>An string[] array of the names of files and subdirectories in the specified directory.</returns>
      /// <remarks>
      /// The EnumerateFileSystemEntries and GetFileSystemEntries methods differ as follows: When you use EnumerateFileSystemEntries,
      /// you can start enumerating the collection of entries before the whole collection is returned; when you use GetFileSystemEntries,
      /// you must wait for the whole array of entries to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory for which file and subdirectory names are returned.</param>
      [SecurityCritical]
      public static string[] GetFileSystemEntries(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false).ToArray();
      }

      /// <summary>Returns an array of file system entries that match the specified search criteria.
      /// </summary>
      /// <returns>An string[] array of file system entries that match the specified search criteria.</returns>
      /// <remarks>
      /// The EnumerateFileSystemEntries and GetFileSystemEntries methods differ as follows: When you use EnumerateFileSystemEntries,
      /// you can start enumerating the collection of entries before the whole collection is returned; when you use GetFileSystemEntries,
      /// you must wait for the whole array of entries to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The path to be searched.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFileSystemEntries(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, false).ToArray();
      }

      /// <summary>Gets an array of all the file names and directory names that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.
      /// </summary>
      /// <returns>An string[] array of file system entries that match the specified search criteria.</returns>
      /// <remarks>
      /// The EnumerateFileSystemEntries and GetFileSystemEntries methods differ as follows: When you use EnumerateFileSystemEntries,
      /// you can start enumerating the collection of entries before the whole collection is returned; when you use GetFileSystemEntries,
      /// you must wait for the whole array of entries to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, enumOptions, false).ToArray();
      }

      #endregion // .NET

      #region AlphaFS

      #region Transacted

      #region .NET

      /// <summary>Returns the names of all files and subdirectories in the specified directory.
      /// </summary>
      /// <returns>An string[] array of the names of files and subdirectories in the specified directory.</returns>
      /// <remarks>
      /// The EnumerateFileSystemEntries and GetFileSystemEntries methods differ as follows: When you use EnumerateFileSystemEntries,
      /// you can start enumerating the collection of entries before the whole collection is returned; when you use GetFileSystemEntries,
      /// you must wait for the whole array of entries to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which file and subdirectory names are returned.</param>
      [SecurityCritical]
      public static string[] GetFileSystemEntries(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false).ToArray();
      }

      /// <summary>Returns an array of file system entries that match the specified search criteria.
      /// </summary>
      /// <returns>An string[] array of file system entries that match the specified search criteria.</returns>
      /// <remarks>
      /// The EnumerateFileSystemEntries and GetFileSystemEntries methods differ as follows: When you use EnumerateFileSystemEntries,
      /// you can start enumerating the collection of entries before the whole collection is returned; when you use GetFileSystemEntries,
      /// you must wait for the whole array of entries to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to be searched.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFileSystemEntries(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, false).ToArray();
      }

      /// <summary>Gets an array of all the file names and directory names that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.
      /// </summary>
      /// <returns>An string[] array of file system entries that match the specified search criteria.</returns>
      /// <remarks>
      /// The EnumerateFileSystemEntries and GetFileSystemEntries methods differ as follows: When you use EnumerateFileSystemEntries,
      /// you can start enumerating the collection of entries before the whole collection is returned; when you use GetFileSystemEntries,
      /// you must wait for the whole array of entries to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      [SecurityCritical]
      public static string[] GetFileSystemEntries(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, enumOptions, false).ToArray();
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetFileSystemEntries

      #region GetLastAccessTime

      #region .NET

      /// <summary>Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path)
      {
         return File.GetLastAccessTimeInternal(null, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path, bool? isFullPath)
      {
         return File.GetLastAccessTimeInternal(null, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetLastAccessTimeInternal(transaction, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path)
      {
         return File.GetLastAccessTimeInternal(transaction, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastAccessTime

      #region GetLastAccessTimeUtc

      #region .NET

      /// <summary>Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path)
      {
         return File.GetLastAccessTimeInternal(null, path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path, bool? isFullPath)
      {
         return File.GetLastAccessTimeInternal(null, path, true, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetLastAccessTimeInternal(transaction, path, true, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetLastAccessTimeInternal(transaction, path, true, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastAccessTimeUtc

      #region GetLastWriteTime

      #region .NET

      /// <summary>Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path)
      {
         return File.GetLastWriteTimeInternal(null, path, false, false).ToLocalTime();
      }

      #endregion //.NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path, bool? isFullPath)
      {
         return File.GetLastWriteTimeInternal(null, path, false, isFullPath).ToLocalTime();
      }

      #endregion //.NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetLastWriteTimeInternal(transaction, path, false, isFullPath).ToLocalTime();
      }

      #endregion //.NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path)
      {
         return File.GetLastWriteTimeInternal(transaction, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastWriteTime

      #region GetLastWriteTimeUtc

      #region .NET

      /// <summary>Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path)
      {
         return File.GetLastWriteTimeInternal(null, path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path, bool? isFullPath)
      {
         return File.GetLastWriteTimeInternal(null, path, true, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetLastWriteTimeInternal(transaction, path, true, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetLastWriteTimeInternal(transaction, path, true, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastWriteTimeUtc

      #region GetLogicalDrives

      #region .NET

      /// <summary>Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".</summary>
      /// <returns>An array of type <see cref="String"/> that represents the logical drives on a computer.</returns>
      [SecurityCritical]
      public static string[] GetLogicalDrives()
      {
         return EnumerateLogicalDrivesInternal(false, false).Select(drive => drive.Name).ToArray();
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".</summary>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      /// <returns>An array of type <see cref="String"/> that represents the logical drives on a computer.</returns>
      [SecurityCritical]
      public static string[] GetLogicalDrives(bool fromEnvironment, bool isReady)
      {
         return EnumerateLogicalDrivesInternal(fromEnvironment, isReady).Select(drive => drive.Name).ToArray();
      }

      #endregion // AlphaFS

      #endregion // GetLogicalDrives

      #region GetParent

      #region .NET

      /// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      [SecurityCritical]
      public static DirectoryInfo GetParent(string path)
      {
         return GetParentInternal(null, path, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static DirectoryInfo GetParent(string path, bool? isFullPath)
      {
         return GetParentInternal(null, path, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static DirectoryInfo GetParent(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetParentInternal(transaction, path, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      [SecurityCritical]
      public static DirectoryInfo GetParent(KernelTransaction transaction, string path)
      {
         return GetParentInternal(transaction, path, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetParent

      #region Move

      #region .NET

      /// <summary>Moves a file or a directory and its contents to a new location.      
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, MoveOptions.None, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath, bool? isFullPath)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, MoveOptions.None, null, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" />is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath, bool? isFullPath)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, MoveOptions.None, null, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, MoveOptions.None, null, null, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Move

      #region SetAccessControl

      #region .NET

      /// <summary>Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, AccessControlSections.All, false);
      }

      /// <summary>Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <remarks>Note that unlike <see cref="System.IO.File.SetAccessControl"/> this method does <b>not</b> automatically
      /// determine what parts of the specified <see cref="DirectorySecurity"/> instance has been modified. Instead, the
      /// parameter <paramref name="includeSections"/> is used to specify what entries from <paramref name="directorySecurity"/> to apply to <paramref name="path"/>.</remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, AccessControlSections includeSections)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, includeSections, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, bool? isFullPath)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, AccessControlSections.All, isFullPath);
      }

      /// <summary>[AlphaFS] Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>Note that unlike <see cref="System.IO.File.SetAccessControl"/> this method does <b>not</b> automatically
      /// determine what parts of the specified <see cref="DirectorySecurity"/> instance has been modified. Instead, the
      /// parameter <paramref name="includeSections"/> is used to specify what entries from <paramref name="directorySecurity"/> to apply to <paramref name="path"/>.</remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, AccessControlSections includeSections, bool? isFullPath)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, includeSections, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // SetAccessControl

      #region SetCreationTime

      #region .NET

      /// <summary>Sets the date and time the directory was created.</summary>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTime.ToUniversalTime(), null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time the directory was created.</summary>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTime.ToUniversalTime(), null, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time the directory was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTime.ToUniversalTime(), null, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time the directory was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTime.ToUniversalTime(), null, null, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetCreationTime

      #region SetCreationTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the directory was created.</summary>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTimeUtc, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the directory was created.</summary>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTimeUtc, null, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the directory was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTimeUtc, null, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the directory was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTimeUtc, null, null, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetCreationTimeUtc

      #region SetCurrentDirectory (.NET)

      #region .NET

      /// <summary>Sets the application's current working directory to the specified directory.</summary>
      /// <param name="path">The path to which the current working directory is set.</param>

      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public static void SetCurrentDirectory(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathRp = Path.GetRegularPathInternal(path, false, false, false, true);

         // System.IO SetCurrentDirectory() does not handle long paths.
         System.IO.Directory.SetCurrentDirectory(path.Length > 255 ? Path.GetShort83Path(pathRp) : pathRp);
      }
      
      #endregion // .NET

      #endregion // SetCurrentDirectory (.NET)

      #region SetLastAccessTime

      #region .NET

      /// <summary>Sets the date and time that the specified directory was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, lastAccessTime.ToUniversalTime(), null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified directory was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, lastAccessTime.ToUniversalTime(), null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, lastAccessTime.ToUniversalTime(), null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, lastAccessTime.ToUniversalTime(), null, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastAccessTime

      #region SetLastAccessTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, lastAccessTimeUtc, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, lastAccessTimeUtc, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, lastAccessTimeUtc, null, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, lastAccessTimeUtc, null, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastAccessTimeUtc

      #region SetLastWriteTime

      #region .NET

      /// <summary>Sets the date and time that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, null, lastWriteTime.ToUniversalTime(), false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, null, lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, null, lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, null, lastWriteTime.ToUniversalTime(), false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastWriteTime

      #region SetLastWriteTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, null, lastWriteTimeUtc, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, null, null, lastWriteTimeUtc, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region Transacted

      #region IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, null, lastWriteTimeUtc, isFullPath);
      }

      #endregion // .NET

      #endregion // IsFullPath

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, null, null, lastWriteTimeUtc, false);
      }

      #endregion // .NET

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastWriteTimeUtc

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents, bool? isFullPath)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, null, path, name, contents, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>

      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, null, path, name, contents, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents, bool? isFullPath)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, transaction, path, name, contents, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>

      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, transaction, path, name, contents, false);
      }

      #endregion Transacted

      #endregion // AddStream

      #region Compress

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Compress(string path, bool? isFullPath)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, isFullPath);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Compress(string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <remarks>This will only compress the root items, non recursive.</remarks>

      [SecurityCritical]
      public static void Compress(string path)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, false);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>

      [SecurityCritical]
      public static void Compress(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, bool? isFullPath)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, isFullPath);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <remarks>This will only compress the root items, non recursive.</remarks>

      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, false);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>

      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, false);
      }

      #endregion // Transacted

      #endregion // Compress

      #region Copy1

      #region IsFullPath

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Copy1(string sourcePath, string destinationPath, CopyOptions copyOptions, bool? isFullPath)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static CopyMoveResult Copy1(string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         return CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Copy1(string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, null, null, false);
      }

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy1(string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Copy1(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool? isFullPath)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      /// </summary>
      /// and the possibility of notifying the application of its progress through a callback function.
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static CopyMoveResult Copy1(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Copy1(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, null, null, false);
      }

      /// <summary>[AlphaFS] Copies a file or a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy1(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, false);
      }

      #endregion // Transacted

      #endregion // Copy1

      #region CountFileSystemObjects

      #region IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, isFullPath).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, isFullPath).Count();
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, false).Count();
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, isFullPath).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, isFullPath).Count();
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, false).Count();
      }

      #endregion // Transacted

      #endregion // CountFileSystemObjects

      #region DisableCompression

      #region IsFullPath

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="path">A path to a directory to decompress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory.
      /// However, newly created files and directories will be uncompressed.</remarks>

      [SecurityCritical]
      public static void DisableCompression(string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(true, null, path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="path">A path to a directory to decompress.</param>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory.
      /// However, newly created files and directories will be uncompressed.</remarks>

      [SecurityCritical]
      public static void DisableCompression(string path)
      {
         Device.ToggleCompressionInternal(true, null, path, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <param name="path">A path to a directory to decompress.</param>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory.
      /// However, newly created files and directories will be uncompressed.</remarks>

      [SecurityCritical]
      public static void DisableCompression(KernelTransaction transaction, string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(true, transaction, path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory to decompress.</param>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory.
      /// However, newly created files and directories will be uncompressed.</remarks>

      [SecurityCritical]
      public static void DisableCompression(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(true, transaction, path, false, false);
      }

      #endregion // Transacted

      #endregion // DisableCompression

      #region Decompress

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>

      [SecurityCritical]
      public static void Decompress(string path, bool? isFullPath)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, isFullPath);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Decompress(string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>

      [SecurityCritical]
      public static void Decompress(string path)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, false);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>

      [SecurityCritical]
      public static void Decompress(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, bool? isFullPath)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, isFullPath);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>

      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, false);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>

      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, false);
      }

      #endregion // Transacted

      #endregion // Decompress

      #region Decrypt

      #region IsFullPath

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Decrypt(string path, bool? isFullPath)
      {
         EncryptDecryptDirectoryInternal(path, false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      /// <param name="recursive"><see langword="true"/> to decrypt the directory recursively. <see langword="false"/> only decrypt files and directories in the root of <paramref name="path"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Decrypt(string path, bool recursive, bool? isFullPath)
      {
         EncryptDecryptDirectoryInternal(path, false, recursive, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>

      [SecurityCritical]
      public static void Decrypt(string path)
      {
         EncryptDecryptDirectoryInternal(path, false, false, false);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      /// <param name="recursive"><see langword="true"/> to decrypt the directory recursively. <see langword="false"/> only decrypt files and directories in the root of <paramref name="path"/>.</param>

      [SecurityCritical]
      public static void Decrypt(string path, bool recursive)
      {
         EncryptDecryptDirectoryInternal(path, false, recursive, false);
      }

      #endregion // Decrypt

      #region DeleteEmpty

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void DeleteEmpty(string path, bool recursive, bool ignoreReadOnly, bool? isFullPath)
      {
         DeleteEmptyDirectoryInternal(null, null, path, recursive, ignoreReadOnly, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(string path, bool recursive)
      {
         DeleteEmptyDirectoryInternal(null, null, path, recursive, false, true, false);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptyDirectoryInternal(null, null, path, recursive, ignoreReadOnly, true, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void DeleteEmpty(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool? isFullPath)
      {
         DeleteEmptyDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteEmptyDirectoryInternal(null, transaction, path, recursive, false, true, false);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptyDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, true, false);
      }

      #endregion // Transacted

      #endregion // DeleteEmpty

      #region DisableEncryption

      #region IsFullPath

      /// <summary>[AlphaFS] Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to disable encryption.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0"</remarks>
      [SecurityCritical]
      public static void DisableEncryption(string path, bool? isFullPath)
      {
         EnableDisableEncryptionInternal(path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to disable encryption.</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0"</remarks>

      [SecurityCritical]
      public static void DisableEncryption(string path)
      {
         EnableDisableEncryptionInternal(path, false, false);
      }

      #endregion // DisableEncryption

      #region EnableCompression

      #region IsFullPath

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="path">A path to a directory to compress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory.
      /// However, newly created files and directories will be compressed.</remarks>
      [SecurityCritical]
      public static void EnableCompression(string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(true, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="path">A path to a directory to compress.</param>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory.
      /// However, newly created files and directories will be compressed.</remarks>      
      [SecurityCritical]
      public static void EnableCompression(string path)
      {
         Device.ToggleCompressionInternal(true, null, path, true, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory to compress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory.
      /// However, newly created files and directories will be compressed.</remarks>
      [SecurityCritical]
      public static void EnableCompression(KernelTransaction transaction, string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(true, transaction, path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory to compress.</param>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory.
      /// However, newly created files and directories will be compressed.</remarks>

      [SecurityCritical]
      public static void EnableCompression(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(true, transaction, path, true, false);
      }

      #endregion // Transacted

      #endregion //EnableCompression

      #region EnableEncryption

      #region IsFullPath

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to enable encryption.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=1"</remarks>
      [SecurityCritical]
      public static void EnableEncryption(string path, bool? isFullPath)
      {
         EnableDisableEncryptionInternal(path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to enable encryption.</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=1"</remarks>

      [SecurityCritical]
      public static void EnableEncryption(string path)
      {
         EnableDisableEncryptionInternal(path, true, false);
      }

      #endregion // EnableEncryption

      #region EnumerateFileIdBothDirectoryInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, bool? isFullPath)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, FileShare.ReadWrite, false, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, FileShare shareMode, bool? isFullPath)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, shareMode, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>

      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, FileShare.ReadWrite, false, false);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      

      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, FileShare shareMode)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, shareMode, false, false);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory handle specified.</summary>
      /// <param name="handle">An open handle to the directory from which to retrieve information.</param>
      /// <returns>An IEnumerable of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    

      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(SafeFileHandle handle)
      {
         // FileShare has no effect since a handle is already opened.
         return EnumerateFileIdBothDirectoryInfoInternal(null, handle, null, FileShare.ReadWrite, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, FileShare.ReadWrite, false, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode, bool? isFullPath)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, shareMode, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, FileShare.ReadWrite, false, false);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>

      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, shareMode, false, false);
      }

      #endregion // Transacted

      #endregion // EnumerateFileIdBothDirectoryInfo

      #region EnumerateFileSystemEntryInfos

      #region IsFullPath

      /// <summary>
      /// [AlphaFS] Returns an enumerable collection of file system entries in a specified path.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, isFullPath);
      }
      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of file system entries that match a
      ///   <paramref name="searchPattern"/> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///   The type to return. This may be one of the following types:
      ///   <list type="definition">
      ///   <item>
      ///      <term>
      ///        <see cref="FileSystemInfo"/>
      ///      </term>
      ///      <description>
      ///        This method will return instances of <see cref="DirectoryInfo"/>,
      ///        <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <term>
      ///        <see cref="string"/>
      ///      </term>
      ///      <description>
      ///        This method will return the full path of each item.
      ///      </description>
      ///   </item>
      ///   </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories in
      ///   <paramref name="path"/>. This parameter can contain a combination of valid literal path and
      ///   wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)
      ///   characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, isFullPath);
      }
      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of file system entries that match a
      ///   <paramref name="searchPattern"/> in a specified path using
      ///   <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <typeparam name="T">
      ///   The type to return. This may be one of the following types:
      ///   <list type="definition">
      ///   <item>
      ///      <term>
      ///        <see cref="FileSystemInfo"/>
      ///      </term>
      ///      <description>
      ///        This method will return instances of <see cref="DirectoryInfo"/>,
      ///        <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <term>
      ///        <see cref="string"/>
      ///      </term>
      ///      <description>
      ///        This method will return the full path of each item.
      ///      </description>
      ///   </item>
      ///   </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories in
      ///   <paramref name="path"/>. This parameter can contain a</para>
      ///   <para>combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///   <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions">
      ///   <see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be
      ///   enumerated.
      /// </param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException">.</exception>
      /// <exception cref="ArgumentNullException">.</exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, directoryEnumerationOptions, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>
      /// [AlphaFS] Returns an enumerable collection of file system entries in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>
      /// [AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern" /> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern"><para>The search string to match against the names of directories in <paramref name="path" />. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll" /> and <see cref="Path.WildcardQuestion" />)</para>
      /// <para>characters, but doesn't support regular expressions.</para></param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, false);
      }
      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of file system entries that match a
      ///   <paramref name="searchPattern"/> in a specified path using
      ///   <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <typeparam name="T">
      ///   The type to return. This may be one of the following types:
      ///   <list type="definition">
      ///   <item>
      ///      <term>
      ///        <see cref="FileSystemInfo"/>
      ///      </term>
      ///      <description>
      ///        This method will return instances of <see cref="DirectoryInfo"/>,
      ///        <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <term>
      ///        <see cref="string"/>
      ///      </term>
      ///      <description>
      ///        This method will return the full path of each item.
      ///      </description>
      ///   </item>
      ///   </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in
      ///   <paramref name="path"/>. This parameter can contain a
      ///   combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and
      ///   <see cref="Path.WildcardQuestion"/>)
      ///   characters, but doesn't support regular expressions.
      /// </param>
      /// <param name="directoryEnumerationOptions">
      ///   <see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be
      ///   enumerated.
      /// </param>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, directoryEnumerationOptions, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, directoryEnumerationOptions, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, directoryEnumerationOptions, false);
      }

      #endregion // Transacted

      #endregion // EnumerateFileSystemEntryInfos

      #region EnumerateLogicalDrives

      /// <summary>[AlphaFS] Enumerates the drive names of all logical drives on a computer.</summary>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      /// <returns>An IEnumerable of type <see cref="Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on a computer.</returns>

      [SecurityCritical]
      public static IEnumerable<DriveInfo> EnumerateLogicalDrives(bool fromEnvironment, bool isReady)
      {
         return EnumerateLogicalDrivesInternal(fromEnvironment, isReady);
      }

      #endregion // EnumerateLogicalDrives

      #region EnumerateStreams

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, bool? isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType, bool? isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, handle, null, null, null, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType, bool? isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, false);
      }

      #endregion // Transacted

      #endregion // EnumerateStreams

      #region Encrypt

      #region IsFullPath

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Encrypt(string path, bool? isFullPath)
      {
         EncryptDecryptDirectoryInternal(path, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="recursive"><see langword="true"/> to encrypt the directory recursively. <see langword="false"/> only encrypt files and directories in the root of <paramref name="path"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Encrypt(string path, bool recursive, bool? isFullPath)
      {
         EncryptDecryptDirectoryInternal(path, true, recursive, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>

      [SecurityCritical]
      public static void Encrypt(string path)
      {
         EncryptDecryptDirectoryInternal(path, true, false, false);
      }

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="recursive"><see langword="true"/> to encrypt the directory recursively. <see langword="false"/> only encrypt files and directories in the root of <paramref name="path"/>.</param>

      [SecurityCritical]
      public static void Encrypt(string path, bool recursive)
      {
         EncryptDecryptDirectoryInternal(path, true, recursive, false);
      }

      #endregion // Encrypt

      #region GetChangeTime

      #region IsFullPath
      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <param name="path">
      ///   The directory for which to obtain creation date and time information.
      /// </param>
      /// <param name="isFullPath">
      ///   <list type="definition">
      ///    <item>
      ///      <term>
      ///        <see langword="true" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="false" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> will be checked and resolved to an absolute path. Unicode
      ///        prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="null" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      ///   </description>
      ///    </item>
      ///   </list>
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path, bool? isFullPath)
      {         
         return File.GetChangeTimeInternal(true, null, null, path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path)
      {
         return File.GetChangeTimeInternal(true, null, null, path, false, false);
      }
      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <param name="safeHandle">
      ///   An open handle to the directory from which to retrieve information.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(SafeFileHandle safeHandle)
      {
         return File.GetChangeTimeInternal(true, null, safeHandle, null, false, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, false, false);
      }

      #endregion // Transacted

      #endregion // GetChangeTime

      #region GetChangeTimeUtc

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path, bool? isFullPath)
      {
         return File.GetChangeTimeInternal(true, null, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path)
      {
         return File.GetChangeTimeInternal(true, null, null, path, true, false);
      }

      /// <summary>
      ///   Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified
      ///   directory.
      /// </summary>
      /// <param name="safeHandle">
      ///   An open handle to the directory from which to retrieve information.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(SafeFileHandle safeHandle)
      {
         return File.GetChangeTimeInternal(true, null, safeHandle, null, true, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.
      /// </summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, true, false);
      }

      #endregion // Transacted

      #endregion // GetChangeTimeUtc

      #region GetProperties

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, bool? isFullPath)
      {
         return GetPropertiesInternal(null, path, DirectoryEnumerationOptions.FilesAndFolders, isFullPath);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return GetPropertiesInternal(null, path, directoryEnumerationOptions, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// <para>Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object,</para>
      /// <para>plus additional ones: Total, File, Size, Error</para>
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path)
      {
         return GetPropertiesInternal(null, path, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return GetPropertiesInternal(null, path, directoryEnumerationOptions, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetPropertiesInternal(transaction, path, DirectoryEnumerationOptions.FilesAndFolders, isFullPath);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         return GetPropertiesInternal(transaction, path, directoryEnumerationOptions, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path)
      {
         return GetPropertiesInternal(transaction, path, DirectoryEnumerationOptions.FilesAndFolders, false);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return GetPropertiesInternal(transaction, path, directoryEnumerationOptions, false);
      }

      #endregion // Transacted

      #endregion // GetProperties

      #region GetStreamSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The number of bytes used by all data streams.</returns>

      [SecurityCritical]
      public static long GetStreamSize(string path, bool? isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The number of bytes used by a named stream.</returns>

      [SecurityCritical]
      public static long GetStreamSize(string path, string name, bool? isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, name, StreamType.Data, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>

      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type, bool? isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, type, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <returns>The number of bytes used by all data streams.</returns>

      [SecurityCritical]
      public static long GetStreamSize(string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>

      [SecurityCritical]
      public static long GetStreamSize(string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, name, StreamType.Data, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>

      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, type, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>

      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, handle, null, name, StreamType.Data, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>

      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, handle, null, null, type, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The number of bytes used by all data streams.</returns>

      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The number of bytes used by a named stream.</returns>

      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name, bool? isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, name, StreamType.Data, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type, bool? isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, type, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <returns>The number of bytes used by all data streams.</returns>

      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>

      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, name, null, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>

      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, type, false);
      }

      #endregion // Transacted

      #endregion GetStreamSize

      #region HasInheritedPermissions

      #region IsFullPath

      /// <summary>[AlphaFS] Check if the directory has permission inheritance enabled.</summary>
      /// <param name="path">The full path to the directory to check.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns><see langword="true"/> if permission inheritance is enabled, <see langword="false"/> if permission inheritance is disabled.</returns>
      public static bool HasInheritedPermissions(string path, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         //DirectorySecurity acl = GetAccessControl(directoryPath);
         DirectorySecurity acl = File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, isFullPath);

         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Check if the directory has permission inheritance enabled.</summary>
      /// <param name="path">The full path to the directory to check.</param>
      /// <returns><see langword="true"/> if permission inheritance is enabled, <see langword="false"/> if permission inheritance is disabled.</returns>
      public static bool HasInheritedPermissions(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         DirectorySecurity acl = File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, false);

         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #endregion // HasInheritedPermissions

      #region Move1

      #region IsFullPath

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Move1(string sourcePath, string destinationPath, MoveOptions moveOptions, bool? isFullPath)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static CopyMoveResult Move1(string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         return CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Move1(string sourcePath, string destinationPath, MoveOptions moveOptions)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, null, null, false);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move1(string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void Move1(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, bool? isFullPath)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="sourcePath" /> and <paramref name="destinationPath" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static CopyMoveResult Move1(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.
      /// </summary>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Move1(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, null, null, false);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move1(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, false);
      }

      #endregion // Transacted

      #endregion // Move1

      #region RemoveStream

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(string path, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, null, isFullPath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(string path, string name, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, name, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>

      [SecurityCritical]
      public static void RemoveStream(string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, null, false);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>

      [SecurityCritical]
      public static void RemoveStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, name, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, null, isFullPath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>

      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, name, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>

      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, null, false);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>

      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, name, false);
      }

      #endregion Transacted

      #endregion // RemoveStream

      #region SetTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>

      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), false);
      }

      #endregion // Transacted

      #endregion // SetTimestamps

      #region SetTimestampsUtc

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>

      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, false);
      }

      #endregion // Transacted

      #endregion // SetTimestampsUtc

      #region TransferTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <param name="isFullPath">
      /// <para><see langword="true"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are an absolute path. Unicode prefix is applied.</para>
      /// <para><see langword="false"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> will be checked and resolved to absolute paths. Unicode prefix is applied.</para>
      /// <para><see langword="null"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath, bool? isFullPath)
      {
         File.TransferTimestampsInternal(true, null, sourcePath, destinationPath, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath)
      {
         File.TransferTimestampsInternal(true, null, sourcePath, destinationPath, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <param name="isFullPath">
      /// <para><see langword="true"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are an absolute path. Unicode prefix is applied.</para>
      /// <para><see langword="false"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> will be checked and resolved to absolute paths. Unicode prefix is applied.</para>
      /// <para><see langword="null"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>

      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath, bool? isFullPath)
      {
         File.TransferTimestampsInternal(true, transaction, sourcePath, destinationPath, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>

      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         File.TransferTimestampsInternal(true, transaction, sourcePath, destinationPath, false);
      }

      #endregion // Transacted

      #endregion // TransferTimestamps


      #region Unified Internals

      #region CompressDecompressInternal

      /// <summary>[AlphaFS] Unified method CompressDecompressInternal() to compress/decompress Non-/Transacted files/directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="searchPattern">
      ///    <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      ///    <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///    <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="compress"><see langword="true"/> compress, when <see langword="false"/> decompress.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      internal static void CompressDecompressInternal(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool compress, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, true, false, true, true);

         // Process directories and files.
         foreach (string fso in EnumerateFileSystemEntryInfosInternal<string>(transaction, pathLp, searchPattern, directoryEnumerationOptions | DirectoryEnumerationOptions.AsLongPath, null))
            Device.ToggleCompressionInternal(true, transaction, fso, compress, null);

         // Compress the root directory, the given path.
         Device.ToggleCompressionInternal(true, transaction, pathLp, compress, null);
      }

      #endregion // CompressDecompressInternal

      #region CopyMoveInternal

      /// <summary>[AlphaFS] Unified method CopyMoveInternal() to copy/move a Non-/Transacted file or directory including its children to a new location,
      /// <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      /// and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      /// <para>This Move method works across disk volumes, and it does not throw an exception if the source and destination are the same. </para>
      /// <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="isFullPath">
      ///    <para><see langword="true"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are an absolute path. Unicode prefix is applied.</para>
      ///    <para><see langword="false"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> will be checked and resolved to absolute paths. Unicode prefix is applied.</para>
      ///    <para><see langword="null"/> <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveInternal(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         #region Setup

         if (isFullPath != null && (bool)!isFullPath)
         {
            Path.CheckValidPath(sourcePath, true, true);
            Path.CheckValidPath(destinationPath, true, true);
         }
         else
         {
            // MSDN:. NET 3.5+: NotSupportedException: Path contains a colon character (:) that is not part of a drive label ("C:\").
            Path.CheckValidPath(sourcePath, false, false);
            Path.CheckValidPath(destinationPath, false, false);
         }

         // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameters before moving the directory.
         // TrimEnd() is also applied for AlphaFS implementation of method Directory.Copy(), .NET does not have this method.

         string sourcePathLp = isFullPath == null
            ? sourcePath
            : (bool)isFullPath
               ? Path.GetLongPathInternal(sourcePath, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(transaction, sourcePath, true, false, false, true, false, false, false);
#else
 : Path.GetFullPathInternal(transaction, sourcePath, true, true, false, true, false, false, false);
#endif

         string destinationPathLp = isFullPath == null
            ? destinationPath
            : (bool)isFullPath
               ? Path.GetLongPathInternal(destinationPath, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(transaction, destinationPath, true, false, false, true, false, false, false);
#else
 : Path.GetFullPathInternal(transaction, destinationPath, true, true, false, true, false, false, false);
#endif

         // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
         if (sourcePathLp.Equals(destinationPathLp, StringComparison.OrdinalIgnoreCase))
            NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPathLp, true);


         // Determine Copy or Move action.
         bool doCopy = copyOptions != null;
         bool doMove = !doCopy && moveOptions != null;

         if ((!doCopy && !doMove) || (doCopy && doMove))
            throw new NotSupportedException(Resources.UndeterminedCopyMoveAction);

         bool overwrite = doCopy
            ? (((CopyOptions)copyOptions & CopyOptions.FailIfExists) != CopyOptions.FailIfExists)
            : (((MoveOptions)moveOptions & MoveOptions.ReplaceExisting) == MoveOptions.ReplaceExisting);

         CopyMoveResult cmr = new CopyMoveResult(sourcePathLp, destinationPathLp, true, doMove, false, (int)Win32Errors.ERROR_SUCCESS);

         #endregion //Setup

         #region Copy

         if (doCopy)
         {
            CreateDirectoryInternal(transaction, destinationPathLp, null, null, false, null);

            foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, sourcePathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null))
            {
               string newDestinationPathLp = Path.CombineInternal(false, destinationPathLp, fsei.FileName);

               cmr = fsei.IsDirectory
                  ? CopyMoveInternal(transaction, fsei.LongFullPath, newDestinationPathLp, copyOptions, null, progressHandler, userProgressData, null)
                  : File.CopyMoveInternal(false, transaction, fsei.LongFullPath, newDestinationPathLp, false, copyOptions, null, progressHandler, userProgressData, null);

               if (cmr.IsCanceled)
                  return cmr;
            }
         }

         #endregion // Copy

         #region Move

         else
         {
            // MSDN: .NET3.5+: IOException: An attempt was made to move a directory to a different volume.
            if (((MoveOptions)moveOptions & MoveOptions.CopyAllowed) != MoveOptions.CopyAllowed)
               if (!Path.GetPathRoot(sourcePathLp, false).Equals(Path.GetPathRoot(destinationPathLp, false), StringComparison.OrdinalIgnoreCase))
                  NativeError.ThrowException(Win32Errors.ERROR_NOT_SAME_DEVICE, destinationPathLp, true);


            // MoveOptions.ReplaceExisting: This value cannot be used if lpNewFileName or lpExistingFileName names a directory.
            if (overwrite && File.ExistsInternal(true, transaction, destinationPathLp, null))
               DeleteDirectoryInternal(null, transaction, destinationPathLp, true, true, false, true, null);


            // Moves a file or directory, including its children.
            // Copies an existing directory, including its children to a new directory.
            cmr = File.CopyMoveInternal(true, transaction, sourcePathLp, destinationPathLp, false, null, moveOptions, progressHandler, userProgressData, null);
         }

         #endregion // Move

         // The copy/move operation succeeded or was canceled.
         return cmr;
      }

      #endregion // CopyMoveInternal

      #region CreateDirectoryInternal

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
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to create.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory. May be <see langword="null"/> to indicate that no template should be used.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> access control to apply to the directory, may be null.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static DirectoryInfo CreateDirectoryInternal(KernelTransaction transaction, string path, string templatePath, ObjectSecurity directorySecurity, bool compress, bool? isFullPath)
      {
         if (isFullPath != null && (bool)!isFullPath)
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

         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(transaction, path, true, false, false, true, false, false, false);
#else
            // MSDN: .NET 4+: Trailing spaces are removed from the end of the path parameter before creating the directory.
               : Path.GetFullPathInternal(transaction, path, true, true, false, true, false, false, false);
#endif

         // Return DirectoryInfo instance if the directory specified by path already exists.
         if (File.ExistsInternal(true, transaction, pathLp, null))
            return new DirectoryInfo(transaction, pathLp, true);

         // MSDN: .NET 3.5+: IOException: The directory specified by path is a file or the network name was not found.
         if (File.ExistsInternal(false, transaction, pathLp, null))
            NativeError.ThrowException(Win32Errors.ERROR_ALREADY_EXISTS, pathLp, true);


         string templatePathLp = Utils.IsNullOrWhiteSpace(templatePath)
            ? null
            : isFullPath == null
               ? templatePath
               : (bool)isFullPath
                  ? Path.GetLongPathInternal(templatePath, false, false, false, false)
#if NET35
                  : Path.GetFullPathInternal(transaction, templatePath, true, false, false, true, false, false, false);
#else
            // MSDN: .NET 4+: Trailing spaces are removed from the end of the path parameter before creating the directory.
                  : Path.GetFullPathInternal(transaction, templatePath, true, true, false, true, false, false, false);
#endif

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

               if (!File.ExistsInternal(true, transaction, path2, null))
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
                        if (File.ExistsInternal(false, transaction, pathLp, null))
                           NativeError.ThrowException(lastError, pathLp, true);

                        if (File.ExistsInternal(false, transaction, folderLp, null))
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
                  Device.ToggleCompressionInternal(true, transaction, folderLp, true, null);
            }
         }

         return new DirectoryInfo(transaction, pathLp, true);
      }

      #endregion // CreateDirectoryInternal

      #region GetDirectoryRootInternal

      /// <summary>[AlphaFS] Unified method GetDirectoryRootInternal() to return the volume information, root information, or both for the specified path.
      /// <returns>
      /// <para>Returns the volume information, root information, or both for the specified path,</para>
      /// <para> or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</para>
      /// </returns>
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      internal static string GetDirectoryRootInternal(KernelTransaction transaction, string path, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, false, false, true, false);

         pathLp = Path.GetRegularPathInternal(pathLp, false, false, false, false);
         string rootPath = Path.GetPathRoot(pathLp, false);

         return Utils.IsNullOrWhiteSpace(rootPath) ? null : rootPath;
      }

      #endregion // GetDirectoryRootInternal

      #region GetParentInternal

      /// <summary>
      ///   [AlphaFS] Unified method GetParent() to retrieve the parent directory of the specified path, including both absolute and relative
      ///   paths.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <param name="isFullPath">
      ///   <list type="definition">
      ///    <item>
      ///      <term>
      ///        <see langword="true" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="false" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      ///   </description>
      ///    </item>
      ///    <item>
      ///      <term>
      ///        <see langword="null" />
      ///      </term>
      ///      <description>
      ///        <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      ///   </description>
      ///    </item>
      ///   </list>
      /// </param>
      /// <returns>
      ///   Returns the parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a
      ///   UNC server or share name.
      /// </returns>
      [SecurityCritical]
      internal static DirectoryInfo GetParentInternal(KernelTransaction transaction, string path, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, false, false, true, false);

         pathLp = Path.GetRegularPathInternal(pathLp, false, false, false, false);
         string dirName = Path.GetDirectoryName(pathLp, false);

         return Utils.IsNullOrWhiteSpace(dirName) ? null : new DirectoryInfo(transaction, dirName, false);
      }

      #endregion // GetParentInternal

      #region DeleteDirectoryInternal

      /// <summary>[AlphaFS] Unified method DeleteDirectoryInternal() to delete a Non-/Transacted directory.
      /// </summary>
      /// <remarks>
      /// <para>The RemoveDirectory function marks a directory for deletion on close. Therefore, the directory is not removed until the last handle to the directory is closed.</para>
      /// <para>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      /// <param name="fileSystemEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fileSystemEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove. Use either <paramref name="path"/> or <paramref name="fileSystemEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> to remove all files and subdirectories recursively; <see langword="false"/> otherwise only the top level empty directory.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only attribute of files and directories.</param>
      /// <param name="requireEmpty"><see langword="true"/> requires the the directory must be empty.</param>
      /// <param name="continueOnNotExist"><see langword="true"/> does not throw an Exception when the file system object does not exist.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static void DeleteDirectoryInternal(FileSystemEntryInfo fileSystemEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool requireEmpty, bool continueOnNotExist, bool? isFullPath)
      {
         #region Setup

         if (isFullPath != null && (bool)!isFullPath)
            Path.CheckValidPath(path, true, true);

         if (fileSystemEntryInfo == null)
         {
            // MSDN: .NET 3.5+: DirectoryNotFoundException:
            // Path does not exist or could not be found.
            // Path refers to a file instead of a directory.
            // The specified path is invalid (for example, it is on an unmapped drive). 

            fileSystemEntryInfo = File.GetFileSystemEntryInternal<FileSystemEntryInfo>(transaction,
               isFullPath == null
                  ? path
                  : (bool)isFullPath
                     ? Path.GetLongPathInternal(path, false, false, false, false)
#if NET35
                     : Path.GetFullPathInternal(transaction, path, true, false, false, true, false, false, false),
#else
               // MSDN: .NET 4+: Trailing spaces are removed from the end of the path parameter before deleting the directory.
                     : Path.GetFullPathInternal(transaction, path, true, true, false, true, false, false, false),
#endif
 continueOnNotExist, null);
         }

         if (fileSystemEntryInfo == null)
            return;

         string pathLp = fileSystemEntryInfo.LongFullPath;

         #endregion // Setup

         // Do not follow mount points nor symbolic links, but do delete the reparse point itself.

         // If directory is reparse point, disable recursion.
         if (recursive && fileSystemEntryInfo.IsReparsePoint)
            recursive = false;


         // Check to see if this is a mount point, and unmount it.
         if (fileSystemEntryInfo.IsMountPoint)
         {
            int lastError = Volume.DeleteVolumeMountPointInternal(pathLp, true);

            if (lastError != Win32Errors.ERROR_SUCCESS && lastError != Win32Errors.ERROR_PATH_NOT_FOUND)
               NativeError.ThrowException(lastError, pathLp);

            // Now it is safe to delete the actual directory.
         }


         if (recursive)
         {
            // Enumerate all file system objects.
            foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null))
            {
               if (fsei.IsDirectory)
                  DeleteDirectoryInternal(fsei, transaction, null, true, ignoreReadOnly, requireEmpty, true, null);
               else
                  File.DeleteFileInternal(transaction, fsei.LongFullPath, ignoreReadOnly, null);
            }
         }

         #region Remove

      startRemoveDirectory:

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // RemoveDirectory() / RemoveDirectoryTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-09-09: MSDN confirms LongPath usage.

            // RemoveDirectory on a symbolic link will remove the link itself.

            ? NativeMethods.RemoveDirectory(pathLp)
            : NativeMethods.RemoveDirectoryTransacted(pathLp, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_DIR_NOT_EMPTY:
                  if (requireEmpty)
                     // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only, or recursive is false and path is not an empty directory. 
                     NativeError.ThrowException(lastError, pathLp, true);

                  goto startRemoveDirectory;


               case Win32Errors.ERROR_DIRECTORY:
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: Path refers to a file instead of a directory.
                  if (File.ExistsInternal(false, transaction, path, null))
                     throw new DirectoryNotFoundException(String.Format(CultureInfo.CurrentCulture, "({0}) {1}",
                        Win32Errors.ERROR_INVALID_PARAMETER, String.Format(CultureInfo.CurrentCulture, Resources.FileExistsWithSameNameSpecifiedByPath, pathLp)));
                  break;


               case Win32Errors.ERROR_PATH_NOT_FOUND:
                  if (continueOnNotExist)
                     return;
                  break;

               case Win32Errors.ERROR_SHARING_VIOLATION:
                  // MSDN: .NET 3.5+: IOException: The directory is being used by another process or there is an open handle on the directory.
                  NativeError.ThrowException(lastError, pathLp, true);
                  break;

               case Win32Errors.ERROR_ACCESS_DENIED:
                  NativeMethods.Win32FileAttributeData data = new NativeMethods.Win32FileAttributeData();
                  int dataInitialised = File.FillAttributeInfoInternal(transaction, pathLp, ref data, false, true);

                  if (data.FileAttributes != (FileAttributes)(-1))
                  {
                     if ((data.FileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                     {
                        // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only, or recursive is false and path is not an empty directory.

                        if (ignoreReadOnly)
                        {
                           // Reset directory attributes.
                           File.SetAttributesInternal(true, transaction, pathLp, FileAttributes.Normal, true, null);
                           goto startRemoveDirectory;
                        }

                        // MSDN: .NET 3.5+: IOException: The directory is read-only or contains a read-only file.
                        NativeError.ThrowException(Win32Errors.ERROR_FILE_READ_ONLY, pathLp, true);
                     }
                  }

                  if (dataInitialised == Win32Errors.ERROR_SUCCESS)
                     // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
                     NativeError.ThrowException(lastError, pathLp);

                  break;
            }

            // MSDN: .NET 3.5+: IOException:
            // A file with the same name and location specified by path exists.
            // The directory specified by path is read-only, or recursive is false and path is not an empty directory. 
            // The directory is the application's current working directory. 
            // The directory contains a read-only file.
            // The directory is being used by another process.

            // Throws IOException.
            NativeError.ThrowException(lastError, pathLp, true);
         }

         #endregion // Remove
      }

      #endregion // DeleteDirectoryInternal

      #region DeleteEmptyDirectoryInternal

      /// <summary>[AlphaFS] Unified method DeleteEmptyDirectoryInternal() to delete empty subdirectores from the specified directory.
      /// </summary>
      /// <remarks>Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the empty directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>      
      /// <param name="fileSystemEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fileSystemEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from. Use either <paramref name="path"/> or <paramref name="fileSystemEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="initialize">When <see langword="true"/> indicates the method is called externally.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      internal static void DeleteEmptyDirectoryInternal(FileSystemEntryInfo fileSystemEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool initialize, bool? isFullPath)
      {
         #region Setup

         if (fileSystemEntryInfo == null)
         {
            if (!File.ExistsInternal(true, transaction, path, isFullPath))
               NativeError.ThrowException(Win32Errors.ERROR_PATH_NOT_FOUND, path);

            fileSystemEntryInfo = File.GetFileSystemEntryInternal<FileSystemEntryInfo>(transaction,
               isFullPath == null
                  ? path
                  : (bool)isFullPath
                     ? Path.GetLongPathInternal(path, false, false, false, false)
#if NET35
                     : Path.GetFullPathInternal(transaction, path, true, false, false, true, false, true, true),
#else
 : Path.GetFullPathInternal(transaction, path, true, true, false, true, false, true, true),
#endif

 false, null);
         }

         if (fileSystemEntryInfo == null)
            throw new ArgumentNullException("path");

         string pathLp = fileSystemEntryInfo.LongFullPath;

         #endregion // Setup

         // Ensure path is a directory.
         if (!fileSystemEntryInfo.IsDirectory)
            throw new IOException(String.Format(CultureInfo.CurrentCulture, Resources.FileExistsWithSameNameSpecifiedByPath, pathLp));

         DirectoryEnumerationOptions dirEnumOptions = DirectoryEnumerationOptions.Folders;
         if (recursive)
            dirEnumOptions |= DirectoryEnumerationOptions.Recursive;

         foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, dirEnumOptions, null))
            DeleteEmptyDirectoryInternal(fsei, transaction, null, recursive, ignoreReadOnly, false, null);


         if (!EnumerateFileSystemEntryInfosInternal<string>(transaction, pathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null).Any())
         {
            // Prevent deleting path itself.
            if (!initialize)
               DeleteDirectoryInternal(fileSystemEntryInfo, transaction, null, false, ignoreReadOnly, true, true, null);
         }
      }

      #endregion // DeleteEmptyDirectoryInternal

      #region EnableDisableEncryptionInternal

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to enable encryption.</param>
      /// <param name="enable"><see langword="true"/> enabled encryption, <see langword="false"/> disables encryption.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0 | 1"</remarks>

      [SecurityCritical]
      internal static void EnableDisableEncryptionInternal(string path, bool enable, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(null, path, true, false, false, true, false, true, true);

         // EncryptionDisable()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.EncryptionDisable(pathLp, !enable))
            NativeError.ThrowException(pathLp);
      }

      #endregion // EnableDisableEncryptionInternal

      #region EncryptDecryptDirectoryInternal

      /// <summary>[AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a directory recursively so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="encrypt"><see langword="true"/> encrypt, <see langword="false"/> decrypt.</param>
      /// <param name="recursive"><see langword="true"/> to decrypt the directory recursively. <see langword="false"/> only decrypt files and directories in the root of <paramref name="path"/>.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      internal static void EncryptDecryptDirectoryInternal(string path, bool encrypt, bool recursive, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(null, path, true, false, false, true, false, true, true);


         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders | DirectoryEnumerationOptions.AsLongPath;
         if (recursive)
            enumOptions |= DirectoryEnumerationOptions.Recursive;


         // Process folders and files.
         foreach (string fso in EnumerateFileSystemEntryInfosInternal<string>(null, pathLp, Path.WildcardStarMatchAll, enumOptions, null))
            File.EncryptDecryptFileInternal(true, fso, encrypt, null);

         // Process the root folder, the given path.
         File.EncryptDecryptFileInternal(true, pathLp, encrypt, null);
      }

      #endregion // EncryptDecryptDirectoryInternal

      #region EnumerateFileIdBothDirectoryInfoInternal

      /// <summary>[AlphaFS] Unified method EnumerateFileIdBothDirectoryInfoInternal() to return an enumerable collection of information about files in the directory handle specified.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">An open handle to the directory from which to retrieve information.</param>
      /// <param name="path">A path to the directory.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="continueOnException">
      /// <para><see langword="true"/> suppress any Exception that might be thrown a result from a failure,</para>
      /// <para>such as ACLs protected directories or non-accessible reparse points.</para>
      /// </param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>An IEnumerable of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    
      /// <remarks>Either use <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</remarks>

      [SecurityCritical]
      internal static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfoInternal(KernelTransaction transaction, SafeFileHandle safeHandle, string path, FileShare shareMode, bool continueOnException, bool? isFullPath)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = isFullPath == null
               ? path
               : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, true, false, true, true);

            safeHandle = File.CreateFileInternal(transaction, pathLp, ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, FileSystemRights.ReadData, shareMode, true, null);
         }


         try
         {
            if (!NativeMethods.IsValidHandle(safeHandle, Marshal.GetLastWin32Error(), !continueOnException))
               yield break;

            // 2014-10-16: Number of returned items depends on the size of the buffer.
            // That does not seem right, investigate.
            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               NativeMethods.IsValidHandle(safeBuffer, Marshal.GetLastWin32Error());

               long fileNameOffset = Marshal.OffsetOf(typeof(NativeMethods.FileIdBothDirInfo), "FileName").ToInt64();

               while (NativeMethods.GetFileInformationByHandleEx(safeHandle, NativeMethods.FileInfoByHandleClass.FileIdBothDirectoryInfo, safeBuffer, NativeMethods.DefaultFileBufferSize))
               {
                  // CA2001:AvoidCallingProblematicMethods

                  IntPtr buffer = IntPtr.Zero;
                  bool successRef = false;
                  safeBuffer.DangerousAddRef(ref successRef);

                  // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                  if (successRef)
                     buffer = safeBuffer.DangerousGetHandle();

                  safeBuffer.DangerousRelease();

                  if (buffer == IntPtr.Zero)
                     NativeError.ThrowException(Resources.HandleDangerousRef);

                  // CA2001:AvoidCallingProblematicMethods


                  while (buffer != IntPtr.Zero)
                  {
                     NativeMethods.FileIdBothDirInfo fibdi = Utils.MarshalPtrToStructure<NativeMethods.FileIdBothDirInfo>(0, buffer);

                     string fileName = Marshal.PtrToStringUni(new IntPtr(fileNameOffset + buffer.ToInt64()), (int)(fibdi.FileNameLength / 2));

                     if (!Utils.IsNullOrWhiteSpace(fileName) &&
                         !fileName.Equals(Path.CurrentDirectoryPrefix, StringComparison.OrdinalIgnoreCase) &&
                         !fileName.Equals(Path.ParentDirectoryPrefix, StringComparison.OrdinalIgnoreCase))
                        yield return new FileIdBothDirectoryInfo(fibdi, fileName);


                     buffer = fibdi.NextEntryOffset == 0
                        ? IntPtr.Zero
                        : new IntPtr(buffer.ToInt64() + fibdi.NextEntryOffset);
                  }
               }

               int lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_SUCCESS:
                  case Win32Errors.ERROR_NO_MORE_FILES:
                  case Win32Errors.ERROR_HANDLE_EOF:
                     yield break;

                  default:
                     NativeError.ThrowException(lastError, path);
                     break;
               }
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // EnumerateFileIdBothDirectoryInfoInternal

      #region EnumerateFileSystemEntryInfosInternal

      /// <summary>[AlphaFS] Unified method EnumerateFileSystemEntryInfosInternal() to return an enumerable collection of file system entries in a specified path using <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <returns>
      /// <para>The return type is based on C# inference. Possible return types are:</para>
      /// <para> <see cref="string"/>- (full path), <see cref="FileSystemInfo"/>- (<see cref="DirectoryInfo"/> / <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instance.</para>
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      [SecurityCritical]
      internal static IEnumerable<T> EnumerateFileSystemEntryInfosInternal<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         Type typeOfT = typeof(T);

         // Need files or folders to enumerate.
         if ((directoryEnumerationOptions & DirectoryEnumerationOptions.FilesAndFolders) == DirectoryEnumerationOptions.None)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.FilesAndFolders;
         
         return new FindFileSystemEntryInfo
         {
            Transaction = transaction,
            InputPath = path,
            SearchPattern = searchPattern,
            IsFullPath = isFullPath,
            IsDirectory = true,

            Recursive = (directoryEnumerationOptions & DirectoryEnumerationOptions.Recursive) == DirectoryEnumerationOptions.Recursive,

            AsLongPath = (directoryEnumerationOptions & DirectoryEnumerationOptions.AsLongPath) == DirectoryEnumerationOptions.AsLongPath,

            AsString = typeOfT == typeof(string),

            AsFileSystemInfo = typeOfT == typeof(FileSystemInfo) || typeOfT.BaseType == typeof(FileSystemInfo),

            FileSystemObjectType = (directoryEnumerationOptions & DirectoryEnumerationOptions.FilesAndFolders) == DirectoryEnumerationOptions.FilesAndFolders
               ? (bool?)null
               : (directoryEnumerationOptions & DirectoryEnumerationOptions.Folders) == DirectoryEnumerationOptions.Folders,

            SkipReparsePoints = (directoryEnumerationOptions & DirectoryEnumerationOptions.SkipReparsePoints) == DirectoryEnumerationOptions.SkipReparsePoints,

            ContinueOnException = (directoryEnumerationOptions & DirectoryEnumerationOptions.ContinueOnException) == DirectoryEnumerationOptions.ContinueOnException

         }.Enumerate<T>().Select(fsei => (T)(object)fsei);
      }

      #endregion // EnumerateFileSystemEntryInfosInternal

      #region EnumerateLogicalDrivesInternal

      /// <summary>[AlphaFS] Unified method EnumerateLogicalDrivesInternal() to enumerate the drive names of all logical drives on a computer.</summary>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      /// <returns>An IEnumerable of type <see cref="Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on a computer.</returns>
      [SecurityCritical]
      internal static IEnumerable<DriveInfo> EnumerateLogicalDrivesInternal(bool fromEnvironment, bool isReady)
      {
         #region Get from Environment

         if (fromEnvironment)
         {
            IEnumerable<string> drivesEnv = isReady
               ? Environment.GetLogicalDrives().Where(ld => File.ExistsInternal(true, null, ld, true))
               : Environment.GetLogicalDrives().Select(ld => ld);

            foreach (string drive in drivesEnv)
            {
               // Optionally check Drive .IsReady.
               if (isReady)
               {
                  if (File.ExistsInternal(true, null, drive, true))
                     yield return new DriveInfo(drive);
               }
               else
                  yield return new DriveInfo(drive);
            }

            yield break;
         }

         #endregion // Get from Environment

         #region Get through NativeMethod

         uint lastError = NativeMethods.GetLogicalDrives();
         if (lastError == Win32Errors.ERROR_SUCCESS)
            // Throws IOException.
            NativeError.ThrowException((int)lastError, true);

         uint drives = lastError;
         int count = 0;
         while (drives != 0)
         {
            if ((drives & 1) != 0)
               ++count;

            drives >>= 1;
         }

         string[] result = new string[count];
         char[] root = { 'A', Path.VolumeSeparatorChar };

         drives = lastError;
         count = 0;

         while (drives != 0)
         {
            if ((drives & 1) != 0)
            {
               string drive = new string(root);

               if (isReady)
               {
                  // Optionally check Drive .IsReady.
                  if (File.ExistsInternal(true, null, drive, true))
                     yield return new DriveInfo(drive);
               }
               else
               {
                  // Ready or not.
                  yield return new DriveInfo(drive);
               }

               result[count++] = drive;
            }

            drives >>= 1;
            root[0]++;
         }

         #endregion // Get through NativeMethod
      }

      #endregion // EnumerateLogicalDrivesInternal

      #region GetPropertiesInternal

      /// <summary>[AlphaFS] Unified method GetPropertiesInternal() to gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: "Total", "File", "Size" and "SizeCompressed".
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Size:</b> is the total compressed size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="isFullPath">
      ///  <list type="definition">
      ///   <item>
      ///     <term>
      ///       <see langword="true" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="false" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> will be checked and resolved to an absolute path. Unicode prefix is applied.
      /// </description>
      ///   </item>
      ///   <item>
      ///     <term>
      ///       <see langword="null" />
      ///     </term>
      ///     <description>
      ///       <paramref name="path" /> is already an absolute path with Unicode prefix. Use as is.
      /// </description>
      ///   </item>
      /// </list></param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      internal static Dictionary<string, long> GetPropertiesInternal(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, bool? isFullPath)
      {
         const string propFile = "File";
         const string propTotal = "Total";
         const string propSize = "Size";
         long total = 0;
         long size = 0;
         Type typeOfAttrs = typeof(FileAttributes);
         Array attributes = Enum.GetValues(typeOfAttrs);
         Dictionary<string, long> props = Enum.GetNames(typeOfAttrs).OrderBy(attrs => attrs).ToDictionary<string, string, long>(name => name, name => 0);

         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, true, false, true, true);

         foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, directoryEnumerationOptions, null))
         {
            total++;

            if (!fsei.IsDirectory)
               size += fsei.FileSize;

            foreach (FileAttributes attributeMarker in attributes)
            {
               // Marker exists in flags.
               if ((fsei.Attributes & attributeMarker) == attributeMarker)

                  // Regular directory that will go to stack, adding directory flag ++
                  props[(((attributeMarker & FileAttributes.Directory) == FileAttributes.Directory) ? FileAttributes.Directory : attributeMarker).ToString()]++;
            }
         }

         // Adjust regular files count.
         props.Add(propFile, total - props[FileAttributes.Directory.ToString()] - props[FileAttributes.ReparsePoint.ToString()]);
         props.Add(propTotal, total);
         props.Add(propSize, size);

         return props;
      }

      #endregion // GetPropertiesInternal

      #endregion // Unified Internals

      #endregion // AlphaFS
   }
}
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
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   partial class DirectoryInfo
   {
      #region .NET

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>

      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path)
      {
         return CreateSubdirectoryCore(path, null, null, false);
      }

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
      {
         return CreateSubdirectoryCore(path, null, directorySecurity, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, bool compress)
      {
         return CreateSubdirectoryCore(path, null, null, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>

      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, string templatePath, bool compress)
      {
         return CreateSubdirectoryCore(path, templatePath, null, compress);
      }


      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryCore(path, null, directorySecurity, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryCore(path, templatePath, directorySecurity, compress);
      }

      #endregion // AlphaFS

      #region Internal Methods

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the DirectoryInfo class.</summary>
      /// <returns>The last directory specified in path as an <see cref="DirectoryInfo"/> object.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      [SecurityCritical]
      private DirectoryInfo CreateSubdirectoryCore(string path, string templatePath, ObjectSecurity directorySecurity, bool compress)
      {
         string pathLp = Path.CombineCore(false, LongFullName, path);
         string templatePathLp = templatePath == null ? null :
            Path.GetExtendedLengthPathCore(Transaction, templatePath, PathFormat.RelativePath, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

         if (string.Compare(LongFullName, 0, pathLp, 0, LongFullName.Length, StringComparison.OrdinalIgnoreCase) != 0)
            throw new ArgumentException(Resources.Invalid_Subpath, pathLp);

         return Directory.CreateDirectoryCore(Transaction, pathLp, templatePathLp, directorySecurity, compress, PathFormat.LongFullPath);
      }

      #endregion // Internal Methods
   }
}

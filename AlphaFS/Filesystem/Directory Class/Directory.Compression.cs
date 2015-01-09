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

using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region Compress

      #region Non-Transactional

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      /// <param name="path">A path that describes a directory to compress.</param>
      [SecurityCritical]
      public static void Compress(string path)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Compress(string path, PathFormat pathFormat)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, pathFormat);
      }



      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static void Compress(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Compress(string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, pathFormat);
      }
      
      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, pathFormat);
      }



      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to compress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, pathFormat);
      }

      #endregion // Transactional

      #endregion // Compress

      #region Decompress

      #region Non-Transactional

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      /// <param name="path">A path that describes a directory to decompress.</param>
      [SecurityCritical]
      public static void Decompress(string path)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decompress(string path, PathFormat pathFormat)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, pathFormat);
      }



      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static void Decompress(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decompress(string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         CompressDecompressInternal(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, pathFormat);
      }
      
      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, pathFormat);
      }

      
      
      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a directory to decompress.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         CompressDecompressInternal(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, pathFormat);
      }

      #endregion // Transactional

      #endregion // Decompress

      #region DisableCompression

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory. However, newly created files and directories will be uncompressed.</remarks>
      /// <param name="path">A path to a directory to decompress.</param>
      [SecurityCritical]
      public static void DisableCompression(string path)
      {
         Device.ToggleCompressionInternal(true, null, path, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory. However, newly created files and directories will be uncompressed.</remarks>
      /// <param name="path">A path to a directory to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DisableCompression(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(true, null, path, false, pathFormat);
      }



      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory. However, newly created files and directories will be uncompressed.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory to decompress.</param>
      [SecurityCritical]
      public static void DisableCompression(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(true, transaction, path, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Disables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method disables the directory-compression attribute. It will not decompress the current contents of the directory. However, newly created files and directories will be uncompressed.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <param name="path">A path to a directory to decompress.</param>
      [SecurityCritical]
      public static void DisableCompression(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(true, transaction, path, false, pathFormat);
      }
      
      #endregion // DisableCompression

      #region EnableCompression

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory. However, newly created files and directories will be compressed.</remarks>
      /// <param name="path">A path to a directory to compress.</param>
      [SecurityCritical]
      public static void EnableCompression(string path)
      {
         Device.ToggleCompressionInternal(true, null, path, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory. However, newly created files and directories will be compressed.</remarks>
      /// <param name="path">A path to a directory to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void EnableCompression(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(true, null, path, true, pathFormat);
      }



      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory. However, newly created files and directories will be compressed.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory to compress.</param>
      [SecurityCritical]
      public static void EnableCompression(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(true, transaction, path, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Enables NTFS compression of the specified directory and the files in it.</summary>
      /// <remarks>This method enables the directory-compression attribute. It will not compress the current contents of the directory. However, newly created files and directories will be compressed.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void EnableCompression(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(true, transaction, path, true, pathFormat);
      }
      
      #endregion // EnableCompression

      #region Internal Methods

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
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void CompressDecompressInternal(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, bool compress, PathFormat pathFormat)
      {
         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         // Process directories and files.
         foreach (var fso in EnumerateFileSystemEntryInfosInternal<string>(transaction, pathLp, searchPattern, directoryEnumerationOptions | DirectoryEnumerationOptions.AsLongPath, PathFormat.LongFullPath))
            Device.ToggleCompressionInternal(true, transaction, fso, compress, PathFormat.LongFullPath);

         // Compress the root directory, the given path.
         Device.ToggleCompressionInternal(true, transaction, pathLp, compress, PathFormat.LongFullPath);
      }

      #endregion // CompressDecompressInternal
   }
}

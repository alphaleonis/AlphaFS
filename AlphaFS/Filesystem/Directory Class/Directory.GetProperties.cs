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
using System.IO;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region GetProperties

      #region Non-Transactional

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
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, PathFormat pathFormat)
      {
         return GetPropertiesInternal(null, path, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
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
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return GetPropertiesInternal(null, path, directoryEnumerationOptions, pathFormat);
      }


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
         return GetPropertiesInternal(null, path, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         return GetPropertiesInternal(null, path, directoryEnumerationOptions, PathFormat.Relative);
      }
      
      #endregion

      #region Transactional

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
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetPropertiesInternal(transaction, path, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
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
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return GetPropertiesInternal(transaction, path, directoryEnumerationOptions, pathFormat);
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
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path)
      {
         return GetPropertiesInternal(transaction, path, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         return GetPropertiesInternal(transaction, path, directoryEnumerationOptions, PathFormat.Relative);
      }

      #endregion // Transactional

      #endregion // GetProperties

      #region Internal Methods

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
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      internal static Dictionary<string, long> GetPropertiesInternal(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         const string propFile = "File";
         const string propTotal = "Total";
         const string propSize = "Size";
         long total = 0;
         long size = 0;
         Type typeOfAttrs = typeof(FileAttributes);
         Array attributes = Enum.GetValues(typeOfAttrs);
         Dictionary<string, long> props = Enum.GetNames(typeOfAttrs).OrderBy(attrs => attrs).ToDictionary<string, string, long>(name => name, name => 0);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, directoryEnumerationOptions, PathFormat.LongFullPath))
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
   }
}

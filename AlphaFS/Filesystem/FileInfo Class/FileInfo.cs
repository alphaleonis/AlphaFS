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
using System.Globalization;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="FileStream"/> objects. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed partial class FileInfo : FileSystemInfo
   {
      #region Constructors

      #region .NET

      /// <summary>Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(string fileName) : this(null, fileName, PathFormat.RelativePath)
      {
      }

      #endregion // .NET

      #region AlphaFS

      #region Non-Transactional

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(string fileName, PathFormat pathFormat) : this(null, fileName, pathFormat)
      {
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(KernelTransaction transaction, string fileName) : this(transaction, fileName, PathFormat.RelativePath)
      {
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(KernelTransaction transaction, string fileName, PathFormat pathFormat)
      {
         InitializeCore(false, transaction, fileName, pathFormat);

         _name = Path.GetFileName(Path.RemoveTrailingDirectorySeparator(fileName, false), pathFormat != PathFormat.LongFullPath);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Constructors
      
      #region Properties

      #region .NET

      #region Directory

      /// <summary>Gets an instance of the parent directory.</summary>
      /// <value>A <see cref="DirectoryInfo"/> object representing the parent directory of this file.</value>
      /// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
      /// <exception cref="DirectoryNotFoundException"/>
      public DirectoryInfo Directory
      {
         get
         {
            string dirName = DirectoryName;
            return dirName == null ? null : new DirectoryInfo(Transaction, dirName, PathFormat.FullPath);
         }
      }

      #endregion // Directory

      #region DirectoryName

      /// <summary>Gets a string representing the directory's full path.</summary>
      /// <value>A string representing the directory's full path.</value>
      /// <remarks>
      ///   <para>To get the parent directory as a DirectoryInfo object, use the Directory property.</para>
      ///   <para>When first called, FileInfo calls Refresh and caches information about the file.</para>
      ///   <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      public string DirectoryName
      {
         [SecurityCritical] get { return Path.GetDirectoryName(FullPath, false); }
      }

      #endregion // DirectoryName

      #region Exists

      /// <summary>Gets a value indicating whether the file exists.</summary>
      /// <value><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</value>
      /// <remarks>
      ///   <para>The <see cref="Exists"/> property returns <see langword="false"/> if any error occurs while trying to determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      public override bool Exists
      {
         [SecurityCritical]
         get
         {
            try
            {
               if (DataInitialised == -1)
                  Refresh();

               FileAttributes attrs = Win32AttributeData.dwFileAttributes;
               return DataInitialised == 0 && attrs != (FileAttributes) (-1) && (attrs & FileAttributes.Directory) == 0;
            }
            catch
            {
               return false;
            }
         }
      }

      #endregion // Exists

      #region IsReadOnly

      /// <summary>Gets or sets a value that determines if the current file is read only.</summary>
      /// <value><see langword="true"/> if the current file is read only; otherwise, <see langword="false"/>.</value>
      /// <remarks>
      ///   <para>Use the IsReadOnly property to quickly determine or change whether the current file is read only.</para>
      ///   <para>When first called, FileInfo calls Refresh and caches information about the file.</para>
      ///   <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// </remarks>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      public bool IsReadOnly
      {
         get { return EntryInfo == null || EntryInfo.IsReadOnly; }

         set
         {
            if (value)
               Attributes |= FileAttributes.ReadOnly;
            else
               Attributes &= ~FileAttributes.ReadOnly;
         }
      }

      #endregion // IsReadOnly

      #region Length

      /// <summary>Gets the size, in bytes, of the current file.</summary>
      /// <value>The size of the current file in bytes.</value>
      /// <remarks>
      ///   <para>The value of the Length property is pre-cached</para>
      ///   <para>To get the latest value, call the Refresh method.</para>
      /// </remarks>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
      public long Length
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName);

            FileAttributes attrs = Win32AttributeData.dwFileAttributes;

            // MSDN: .NET 3.5+: FileNotFoundException: The file does not exist or the Length property is called for a directory.
            if (attrs == (FileAttributes) (-1))
               NativeError.ThrowException(Win32Errors.ERROR_FILE_NOT_FOUND, LongFullName);

            // MSDN: .NET 3.5+: FileNotFoundException: The file does not exist or the Length property is called for a directory.
            if ((attrs & FileAttributes.Directory) == FileAttributes.Directory)
               NativeError.ThrowException(Win32Errors.ERROR_FILE_NOT_FOUND, string.Format(CultureInfo.CurrentCulture, Resources.Target_File_Is_A_Directory, LongFullName));

            return Win32AttributeData.FileSize;
         }
      }

      #endregion // Length

      #region Name

      private string _name;

      /// <summary>Gets the name of the file.</summary>
      /// <value>The name of the file.</value>
      /// <remarks>
      ///   <para>The name of the file includes the file extension.</para>
      ///   <para>When first called, <see cref="FileInfo"/> calls Refresh and caches information about the file.</para>
      ///   <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      ///   <para>The name of the file includes the file extension.</para>
      /// </remarks>
      public override string Name
      {
         get { return _name; }
      }

      #endregion // Name

      #endregion // .NET

      #endregion // Properties
   }
}

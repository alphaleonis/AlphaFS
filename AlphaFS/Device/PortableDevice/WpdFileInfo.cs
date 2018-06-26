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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="T:FileStream"/> objects. This class cannot be inherited.</summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wpd")]
   [Serializable]
   public sealed partial class WpdFileInfo : WpdFileSystemInfo
   {
      #region Fields

      private string _name;
      private long _length = -1;

      #endregion // Fields


      #region Constructors

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.WpdFileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fullName"></param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public WpdFileInfo(string fullName) : this(null, Path.GetFileName(Path.RemoveTrailingDirectorySeparator(fullName, false), false), fullName)
      {
      }


      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.WpdFileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="objectId">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="name"></param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public WpdFileInfo(string objectId, string name) : this(objectId, name, null)
      {
      }


      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.WpdFileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="objectId">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="name"></param>
      /// <param name="fullName"></param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public WpdFileInfo(string objectId, string name, string fullName)
      {
         InitializeCore(false, objectId, fullName);

         _name = name;
      }

      #endregion // Constructors
      

      #region Properties

      #region .NET

      /// <summary>Gets an instance of the parent directory.</summary>
      /// <returns>A <see cref="T:DirectoryInfo"/> object representing the parent directory of this file.</returns>
      /// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
      public WpdDirectoryInfo Directory
      {
         get
         {
            var dirName = !Utils.IsNullOrWhiteSpace(DirectoryName) ? DirectoryName : Device.NativeMethods.WPD_DEVICE_OBJECT_ID;

            return null != dirName ? new WpdDirectoryInfo(dirName) : null;
         }
      }


      /// <summary>Gets the directory's full path.</summary>
      /// <returns>The directory's full path.</returns>
      public string DirectoryName
      {
         get { return Path.GetDirectoryName(FullPath); }
      }

      
      /// <summary>Gets a value indicating whether the file exists.</summary>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      public override bool Exists
      {
         //get { return EntryInfo != null && !EntryInfo.IsDirectory; }
         get { return true; }
      }


      /// <summary>Gets or sets a value that determines if the current file is read only.</summary>
      /// <returns><c>true</c> if the current file is read only, <c>false</c> otherwise.</returns>
      public bool IsReadOnly
      {
         get
         {
            return Attributes == (FileAttributes) (-1) || (Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
         }

         set
         {
            var fileInfo = this;

            fileInfo.Attributes = value ? fileInfo.Attributes | FileAttributes.ReadOnly : fileInfo.Attributes & ~FileAttributes.ReadOnly;
         }
      }

      
      /// <summary>Gets the size, in bytes, of the current file.</summary>
      /// <returns>The size of the current file in bytes.</returns>
      public long Length
      {
         get
         {
            if (_length == -1)
               Refresh();

            //_length = null != EntryInfo  ? EntryInfo.FileSize : -1;

            return _length;
         }

         internal set
         {
            _length = value;
         }
      }


      /// <summary>Gets the name of the file.</summary>
      /// <returns>The name of the file.</returns>
      /// <remarks>
      /// The name of the file includes the file extension.
      /// When first called, WpdFileInfo calls Refresh and caches information about the file. On subsequent calls, you must call Refresh to get the latest copy of the information.
      /// </remarks>
      public override string Name
      {
         get { return _name; }
      }

      #endregion // .NET

      #endregion // Properties


      #region Methods

      #region .NET

      /// <summary>Permanently deletes a file.</summary>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// <exception cref="Exception"/>
      public override void Delete()
      {
         //File.DeleteFileCore(null, FullName, false, PathFormat.FullPath);
         //File.DeleteFileInternal(null, null, false, null);
      }


      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public new void Refresh()
      {
         base.Refresh();
      }


      /// <summary>Returns the path as a string.</summary>
      /// <returns>The path.</returns>
      public override string ToString()
      {
         return DisplayPath;
      }

      #endregion // .NET

      #endregion // Methods
   }
}
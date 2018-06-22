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
using Alphaleonis.Win32.Filesystem;
using Path = Alphaleonis.Win32.Filesystem.Path;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using File = Alphaleonis.Win32.Filesystem.File;

namespace Alphaleonis.Win32.Device
{
   /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="T:FileStream"/> objects. This class cannot be inherited.</summary>
   [Serializable]
   public sealed class PortableDeviceFileInfo : PortableDeviceFileSystemInfo
   {
      #region Fields

      private long _length = -1;

      #endregion // Fields


      #region Constructors

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceFileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fullName"></param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public PortableDeviceFileInfo(string fullName)
      {
         InitializeCore(false, fullName, null);

         Name = Path.GetFileName(Path.RemoveTrailingDirectorySeparator(fullName, false), false);
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceFileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="objectId">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="name"></param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public PortableDeviceFileInfo(string objectId, string name)
      {
         InitializeCore(false, objectId, null);

         Name = name;
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceFileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="objectId">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="name"></param>
      /// <param name="fullName"></param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public PortableDeviceFileInfo(string objectId, string name, string fullName)
      {
         InitializeCore(false, objectId, fullName);

         Name = name;
      }

      #endregion // Constructors
      

      #region Properties

      #region .NET

      /// <summary>Gets an instance of the parent directory.</summary>
      /// <returns>A <see cref="T:DirectoryInfo"/> object representing the parent directory of this file.</returns>
      /// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
      public DirectoryInfo Directory
      {
         get
         {
            var dirName = !Utils.IsNullOrWhiteSpace(DirectoryName) ? DirectoryName : PortableDeviceConstants.DeviceObjectId;

            return null != dirName ? new DirectoryInfo(null, dirName, PathFormat.FullPath) : null;
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
         get { return EntryInfo != null && !EntryInfo.IsDirectory; }
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
      /// When first called, PortableDeviceFileInfo calls Refresh and caches information about the file. On subsequent calls, you must call Refresh to get the latest copy of the information.
      /// </remarks>
      public override string Name { get; internal set; }

      #endregion // .NET

      #endregion // Properties


      #region Methods

      #region .NET

      #region CopyTo

      #region .NET

      /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
      /// <param name="destFileName">The name of the new file to copy to.</param>
      /// <returns>A new file with a fully qualified path.</returns>
      /// <remarks>Use the CopyTo method to allow overwriting of an existing file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceFileInfo CopyTo(string destFileName)
      {
         return CopyToMoveToInternal(false, destFileName, CopyOptions.FailIfExists, null, null, null, false);
      }

      /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <param name="destFileName">The name of the new file to copy to.</param>
      /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
      /// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <c>true</c>. If the file exists and <paramref name="overwrite"/> is <c>false</c>, an <see cref="T:System.IO.IOException"/> is thrown.</returns>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceFileInfo CopyTo(string destFileName, bool overwrite)
      {
         return CopyToMoveToInternal(false, destFileName, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, false);
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <param name="destFileName">The name of the new file to copy to.</param>
      /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
      /// <param name="copyProgress"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <c>true</c>. If the file exists and <paramref name="overwrite"/> is <c>false</c>, an <see cref="T:System.IO.IOException"/> is thrown.</returns>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceFileInfo CopyTo(string destFileName, bool overwrite, CopyMoveProgressRoutine copyProgress, object userProgressData)
      {
         return CopyToMoveToInternal(false, destFileName, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, copyProgress, userProgressData, false);
      }

      #endregion // AlphaFS

      #endregion // CopyTo


      #region Delete

      #region .NET

      /// <summary>Permanently deletes a file.</summary>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      public override void Delete()
      {
         File.DeleteFileCore(null, FullName, false, PathFormat.FullPath);

         //File.DeleteFileInternal(null, null, false, null);

         Reset();
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Permanently deletes a file.</summary>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      public void Delete(bool ignoreReadOnly)
      {
         File.DeleteFileCore(null, FullName, ignoreReadOnly, PathFormat.FullPath);

         //File.DeleteFileInternal(null, null, ignoreReadOnly, null);

         Reset();
      }

      #endregion // AlphaFS

      #endregion // Delete


      #region MoveTo

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destFileName)
      {
         CopyToMoveToInternal(true, destFileName, null, MoveOptions.None, null, null, false);
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Moves a FileyInfo instance and its contents to a new path.</summary>
      /// <param name="destFileName">The path to the new location for sourceFileName.</param>
      /// <param name="moveProgress"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been moved.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destFileName, CopyMoveProgressRoutine moveProgress, object userProgressData)
      {
         CopyToMoveToInternal(true, destFileName, null, MoveOptions.None, moveProgress, userProgressData, false);
      }

      #endregion // AlphaFS

      #endregion // MoveTo


      #region .NET

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


      #region Replace

      #region .NET

      /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:PortableDeviceFileInfo"/> object, deleting the original file, and creating a backup of the replaced file.</summary>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.</param>
      /// <returns>A <see cref="T:PortableDeviceFileInfo"/> object that encapsulates information about the file described by the <paramref name="destinationFileName"/> parameter.</returns>
      /// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current <see cref="T:PortableDeviceFileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new <see cref="T:PortableDeviceFileInfo"/> object that describes the overwritten file.</remarks>
      /// <remarks>Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being replaced.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceFileInfo Replace(string destinationFileName, string destinationBackupFileName)
      {
         return Replace(destinationFileName, destinationBackupFileName, false, false);
      }

      /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:PortableDeviceFileInfo"/> object, deleting the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.</summary>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise, <c>false</c>.</param>
      /// <returns>A <see cref="T:PortableDeviceFileInfo"/> object that encapsulates information about the file described by the <paramref name="destinationFileName"/> parameter.</returns>
      /// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current <see cref="T:PortableDeviceFileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new <see cref="T:PortableDeviceFileInfo"/> object that describes the overwritten file.</remarks>
      /// <remarks>Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being replaced.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
      {
         return Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors, false);
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Replaces the contents of a specified file with the file described by the current <see cref="T:PortableDeviceFileInfo"/> object, deleting the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.</summary>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise, <c>false</c>.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <returns>A <see cref="T:PortableDeviceFileInfo"/> object that encapsulates information about the file described by the <paramref name="destinationFileName"/> parameter.</returns>
      /// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current <see cref="T:PortableDeviceFileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new <see cref="T:PortableDeviceFileInfo"/> object that describes the overwritten file.</remarks>
      /// <remarks>Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being replaced.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, bool? isFullPath)
      {
         //string destinationFileNameLp = isFullPath == null
         //   ? destinationFileName
         //   : (bool)isFullPath
         //      ? Path.GetLongPathInternal(destinationFileName, false, false, false, false)
         //      : Path.GetFullPathInternal(null, destinationFileName, true, false, false, true, false, true, false);

         //// Pass null to the destinationBackupFileName parameter if you do not want to create a backup of the file being replaced.
         //string destinationBackupFileNameLp = isFullPath == null
         //   ? destinationBackupFileName
         //   : (bool)isFullPath
         //      ? Path.GetLongPathInternal(destinationBackupFileName, false, false, false, false)
         //      : Path.GetFullPathInternal(null, destinationBackupFileName, true, false, false, true, false, true, false);

         //File.ReplaceInternal(null, destinationFileNameLp, destinationBackupFileNameLp, ignoreMetadataErrors, null);

         return new PortableDeviceFileInfo(null);
      }

      #endregion // AlphaFS

      #endregion // Replace

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Unified method CopyToMoveToInternal() to copy an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <param name="isMove"><c>true</c> indicates a file move, <c>false</c> indicates a file copy.</param>
      /// <param name="destFileName"><para>A full path string to the destination directory</para></param>
      /// <param name="copyOptions"><para>This parameter can be <c>null</c>. Use <see cref="T:CopyOptions"/> to specify how the file is to be copied.</para></param>
      /// <param name="moveOptions"><para>This parameter can be <c>null</c>. Use <see cref="T:MoveOptions"/> that specify how the file is to be moved.</para></param>
      /// <param name="copyProgress"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="destFileName"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="destFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="destFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <returns>When <paramref name="isMove"/> is <c>false</c> a new <see cref="T:PortableDeviceFileInfo"/> instance with a fully qualified path. Otherwise <c>null</c> is returned.</returns>
      /// <remarks>The attributes of the original file are retained in the copied file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <remarks>This Move method works across disk volumes, and it does not throw an exception if the source and destination are
      /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you
      /// get an IOException. You cannot use the Move method to overwrite an existing file.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private PortableDeviceFileInfo CopyToMoveToInternal(bool isMove, string destFileName, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine copyProgress, object userProgressData, bool? isFullPath)
      {
         return null;

         //string destFileNameLp = isFullPath == null
         //   ? destFileName
         //   : (bool)isFullPath
         //   ? Path.GetLongPathInternal(destFileName, false, false, false, false)
         //   : Path.GetFullPathInternal(null, destFileName, true, false, false, true, false, true, false);

         //File.CopyMoveInternal(isMove, null, null, destFileNameLp, false, copyOptions, moveOptions, copyProgress, userProgressData, null);

         //if (isMove)
         //{
         //   FullPath = Path.GetRegularPathInternal(destFileNameLp, false, false, false, false);

         //   OriginalPath = destFileName;
         //   DisplayPath = OriginalPath;

         //   Name = Path.GetFileName(destFileNameLp, true);

         //   // Flush any cached information about the file.
         //   Reset();
         //}

         //return isMove ? null : new PortableDeviceFileInfo(null);
      }

      #endregion // AlphaFS

      #endregion // Methods
   }
}
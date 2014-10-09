/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides the base class for both FileInfo and DirectoryInfo objects.</summary>
   [SerializableAttribute]
   [ComVisibleAttribute(true)]
   public abstract class FileSystemInfo : MarshalByRefObject
   {
      #region Methods

      #region .NET

      #region Delete

      /// <summary>Deletes a file or directory.</summary>
      [SecurityCritical]
      public abstract void Delete();

      #endregion // Delete

      #region Refresh

      /// <summary>Refreshes the state of the object.</summary>
      /// <remarks>
      /// FileSystemInfo.Refresh() takes a snapshot of the file from the current file system.
      /// Refresh cannot correct the underlying file system even if the file system returns incorrect or outdated information.
      /// This can happen on platforms such as Windows 98.
      /// Calls must be made to Refresh() before attempting to get the attribute information, or the information will be outdated.
      /// </remarks>
      [SecurityCritical]
      protected void Refresh()
      {
         // .NET uses GetFileAttributesEx() for this.

         _entryInfo = File.GetFileSystemEntryInfoInternal(IsDirectory, Transaction, LongFullName, true, true, null);
         _exists = _entryInfo != null;
      }

      #endregion // Refresh

      #region ToString

      /// <summary>Returns a string that represents the current object.</summary>
      /// <remarks>ToString is the major formatting method in the .NET Framework. It converts an object to its string representation so that it is suitable for display.</remarks>
      public override string ToString()
      {
         // "Alphaleonis.Win32.Filesystem.FileSystemInfo"
         return GetType().ToString();
      }

      #endregion // ToString

      #region Equality

      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; <c>false</c> otherwise.</returns>
      public override bool Equals(object obj)
      {
         if (obj == null || GetType() != obj.GetType())
            return false;

         FileSystemInfo other = obj as FileSystemInfo;

         return other != null && (other.Name != null &&
                                  (other.FullName.Equals(FullName, StringComparison.OrdinalIgnoreCase) &&
                                   other.Attributes.Equals(Attributes) &&
                                   other.CreationTimeUtc.Equals(CreationTimeUtc) &&
                                   other.LastWriteTimeUtc.Equals(LastWriteTimeUtc)));
      }

      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            int hash = !Utils.IsNullOrWhiteSpace(FullName) ? FullName.GetHashCode() : 17;

            if (!Utils.IsNullOrWhiteSpace(Name))
               hash = hash * 23 + Name.GetHashCode();

            hash = hash * 23 + Attributes.GetHashCode();
            hash = hash * 23 + CreationTimeUtc.GetHashCode();
            hash = hash * 23 + LastWriteTimeUtc.GetHashCode();

            return hash;
         }
      }
      
      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(FileSystemInfo left, FileSystemInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) ||
                !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }
      
      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(FileSystemInfo left, FileSystemInfo right)
      {
         return !(left == right);
      }

      #endregion  // Equality

      #endregion // .NET

      #region AlphaFS
      
      #region VerifyObjectExists

      /// <summary>[AlphaFS] Performs a <see cref="T:Refresh()"/> and checks that the file system object (folder, file) exists. If the file system object is not found, a <see cref="T:DirectoryNotFoundException"/> or <see cref="T:FileNotFoundException"/> is thrown.</summary>
      /// <exception cref="DirectoryNotFoundException"></exception>
      /// <exception cref="FileNotFoundException"></exception>
      private void VerifyObjectExists()
      {
         Refresh();

         if (!_exists)
            NativeError.ThrowException(IsDirectory ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND, LongFullName);
      }

      #endregion // VerifyObjectExists

      #region Reset

      /// <summary>[AlphaFS] Resets the state of the file system object to uninitialized.</summary>
      internal void Reset()
      {
         _entryInfo = null;
         _exists = false;
      }

      #endregion // Reset

      #region InitializeInternal

      /// <summary>[AlphaFS] Initializes the specified file name.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The full path and name of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      internal void InitializeInternal(bool isFolder, KernelTransaction transaction, string path, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(isFolder ? "path" : "filename");

         LongFullName = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, !isFolder);

         FullPath = Path.GetRegularPathInternal(LongFullName, false, false, false, false);

         IsDirectory = isFolder;
         Transaction = transaction;

         OriginalPath = FullPath.Length == 2 && (FullPath[1] == Path.VolumeSeparatorChar)
            ? Path.CurrentDirectoryPrefix
            : path;

         DisplayPath = OriginalPath.Length != 2 || OriginalPath[1] != Path.VolumeSeparatorChar
            ? OriginalPath
            : Path.CurrentDirectoryPrefix;
      }

      #endregion // InitializeInternal

      #endregion // AlphaFS
      
      #endregion // Methods

      #region Properties

      #region .NET

      #region Attributes

      /// <summary>Gets or sets the attributes for the current file or directory.</summary>
      /// <returns><see cref="T:System.IO.FileAttributes"/> of the current <see cref="T:FileSystemInfo"/>.</returns>
      public FileAttributes Attributes
      {
         [SecurityCritical]
         get
         {
            if (_entryInfo == null)
               Refresh();

            return _entryInfo != null ? _entryInfo.Attributes : (FileAttributes)(-1);
         }

         [SecurityCritical]
         set
         {
            if (_entryInfo == null)
               VerifyObjectExists();

            File.SetAttributesInternal(IsDirectory, Transaction, LongFullName, value, false, null);
            Reset();
         }
      }

      #endregion // Attributes

      #region CreationTime

      /// <summary>Gets or sets the creation time of the current file or directory.</summary>
      /// <returns>The creation date and time of the current <see cref="T:FileSystemInfo"/> object.</returns>
      /// <remarks>This value is expressed in local time.</remarks>
      public DateTime CreationTime
      {
         get { return CreationTimeUtc.ToLocalTime(); }
         set { CreationTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // CreationTime

      #region CreationTimeUtc

      /// <summary>Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.</summary>
      /// <returns>The creation date and time in UTC format of the current <see cref="T:FileSystemInfo"/> object.</returns>
      /// <remarks>This value is expressed in UTC time.</remarks>
      [ComVisible(false)]
      public DateTime CreationTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (_entryInfo == null)
               Refresh();

            return DateTime.FromFileTimeUtc(_entryInfo != null
               ? _entryInfo.Win32FindData.CreationTime.ToLong()
               : new NativeMethods.Win32FindData().CreationTime.ToLong());
         }

         set
         {
            if (_entryInfo == null)
               VerifyObjectExists();

            File.SetFsoDateTimeInternal(IsDirectory, Transaction, LongFullName, value, null, null, null);
            Reset();
         }
      }

      #endregion // CreationTimeUtc

      #region Exists
      
      private bool _exists;

      /// <summary>Gets a value indicating whether the file or directory exists.</summary>
      /// <returns><c>true</c> if the file or directory exists; <c>false</c> otherwise.</returns>
      /// <remarks>The <see cref="T:Exists"/> property returns <c>false</c> if any error occurs while trying to determine if the specified file or directory exists. This can occur in situations that raise exceptions such as passing a directory- or file name with invalid characters or too many characters, a failing or missing disk, or if the caller does not have permission to read the file or directory.</remarks>
      public abstract bool Exists { get; }

      #endregion // Exists

      #region Extension

      /// <summary>Gets the extension part of the file.</summary>
      /// <returns>The <see cref="T:System.IO.FileSystemInfo"/> extension.</returns>
      public string Extension
      {
         get { return System.IO.Path.GetExtension(FullPath); }
      }

      #endregion // Extension

      #region FullName

      /// <summary>Gets the full path of the file or directory.</summary>
      /// <returns>The full path.</returns>
      public virtual string FullName
      {
         [SecurityCritical]
         get { return FullPath; }
      }

      #endregion // FullName

      #region LastAccessTime

      /// <summary>Gets or sets the time the current file or directory was last accessed.</summary>
      /// <returns>The time that the current file or directory was last accessed.</returns>
      /// <remarks>This value is expressed in local time.</remarks>
      /// <remarks>When first called, <see cref="T:FileSystemInfo"/> calls Refresh and returns the cached information on APIs to get attributes and so on. On subsequent calls, you must call Refresh to get the latest copy of the information. 
      /// If the file described in the <see cref="T:FileSystemInfo"/> object does not exist, this property will return 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time. 
      /// </remarks>
      public DateTime LastAccessTime
      {
         get { return LastAccessTimeUtc.ToLocalTime(); }
         set { LastAccessTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // LastAccessTime

      #region LastAccessTimeUtc

      /// <summary>Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.</summary>
      /// <returns>The UTC time that the current file or directory was last accessed.</returns>
      /// <remarks>This value is expressed in UTC time.</remarks>
      /// <remarks>When first called, <see cref="T:FileSystemInfo"/> calls Refresh and returns the cached information on APIs to get attributes and so on. On subsequent calls, you must call Refresh to get the latest copy of the information. 
      /// If the file described in the <see cref="T:FileSystemInfo"/> object does not exist, this property will return 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time. 
      /// </remarks>
      [ComVisible(false)]
      public DateTime LastAccessTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (_entryInfo == null)
               Refresh();

            return DateTime.FromFileTimeUtc(_entryInfo != null
               ? _entryInfo.Win32FindData.LastAccessTime.ToLong()
               : new NativeMethods.Win32FindData().LastAccessTime.ToLong());
         }

         set
         {
            if (_entryInfo == null)
               VerifyObjectExists();

            File.SetFsoDateTimeInternal(IsDirectory, Transaction, LongFullName, null, value, null, null);
            Reset();
         }
      }

      #endregion // LastAccessTimeUtc

      #region LastWriteTime

      /// <summary>Gets or sets the time when the current file or directory was last written to.</summary>
      /// <returns>The time the current file was last written.</returns>
      /// <remarks>This value is expressed in local time.</remarks>
      public DateTime LastWriteTime
      {
         get { return LastWriteTimeUtc.ToLocalTime(); }
         set { LastWriteTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // LastWriteTime

      #region LastWriteTimeUtc

      /// <summary>Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.</summary>
      /// <returns>The UTC time when the current file was last written to.</returns>
      /// <remarks>This value is expressed in UTC time.</remarks>
      [ComVisible(false)]
      public DateTime LastWriteTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (_entryInfo == null)
               Refresh();

            return DateTime.FromFileTimeUtc(_entryInfo != null
               ? _entryInfo.Win32FindData.LastWriteTime.ToLong()
               : new NativeMethods.Win32FindData().LastWriteTime.ToLong());
         }

         set
         {
            if (_entryInfo == null)
               VerifyObjectExists();

            File.SetFsoDateTimeInternal(IsDirectory, Transaction, LongFullName, null, null, value, null);
            Reset();
         }
      }

      #endregion // LastWriteTimeUtc

      #region Name

      /// <summary>For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.</summary>
      /// <returns>The name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension.</returns>
      public abstract string Name { get; }

      #endregion // Name

      #endregion // .NET

      #region AlphaFS

      #region DisplayPath

      /// <summary>Returns the path as a string.</summary>
      protected internal string DisplayPath { get; set; }

      #endregion // DisplayPath

      #region EntryInfo

      private FileSystemEntryInfo _entryInfo;

      /// <summary>[AlphaFS] Gets the instance of the <see cref="T:FileSystemEntryInfo"/> class.</summary>
      public FileSystemEntryInfo EntryInfo
      {
         get
         {
            if (_entryInfo == null)
               Refresh();

            return _entryInfo;
         }

         internal set { _entryInfo = value; }
      }

      #endregion // EntryInfo

      #region IsDirectory

      /// <summary>[AlphaFS] The initial "IsDirectory" indicator that was passed to the constructor.</summary>
      protected internal bool IsDirectory { get; set; }

      #endregion // IsDirectory

      #region LongFullName

      /// <summary>The full path of the file system object in Unicode (LongPath) format.</summary>
      public string LongFullName { get; internal set; }

      #endregion // LongFullName

      #region Transaction

      [NonSerialized]
      private KernelTransaction _transaction;

      /// <summary>[AlphaFS] Represents the KernelTransaction that was passed to the constructor.</summary>
      public KernelTransaction Transaction
      {
         get { return _transaction; }
         internal set { _transaction = value; }
      }

      #endregion // Transaction

      #endregion // AlphaFS

      #endregion // Properties

      #region Fields

      #region .NET

      /// <summary>Represents the fully qualified path of the file or directory.</summary>
      /// <remarks>Classes derived from <see cref="T:FileSystemInfo"/> can use the FullPath field to determine the full path of the object being manipulated.</remarks>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string FullPath;

      /// <summary>The path originally specified by the user, whether relative or absolute.</summary>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string OriginalPath;

      #endregion // .NET
       
      #endregion // Fields
   }
}
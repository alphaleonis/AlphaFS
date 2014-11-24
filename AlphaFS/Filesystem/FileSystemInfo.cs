/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides the base class for both <see cref="T:FileInfo"/> and <see cref="T:DirectoryInfo"/> objects.</summary>
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

      /// <summary>Refreshes the state of the object.
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>FileSystemInfo.Refresh() takes a snapshot of the file from the current file system.</para>
      /// <para>Refresh cannot correct the underlying file system even if the file system returns incorrect or outdated information.</para>
      /// <para>This can happen on platforms such as Windows 98.</para>
      /// <para>Calls must be made to Refresh() before attempting to get the attribute information, or the information will be outdated.</para>
      /// </remarks>
      /// </summary>
      [SecurityCritical]
      protected void Refresh()
      {
         DataInitialised = File.FillAttributeInfo(Transaction, LongFullName, ref Win32AttributeData, false, false);
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
      /// <returns><c>true</c> if the specified Object is equal to the current Object; otherwise, <c>false</c>.</returns>
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

      // A random prime number will be picked and added to the HashCode, each time an instance is created.
      [NonSerialized] private readonly int _random = new Random().Next(0, 19);
      [NonSerialized] private static readonly int[] Primes = { 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919 };

      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         string fullName = FullName;
         string name = Name;

         unchecked
         {
            int hash = Primes[_random];

            if (!Utils.IsNullOrWhiteSpace(fullName))
               hash = hash * Primes[1] + fullName.GetHashCode();

            if (!Utils.IsNullOrWhiteSpace(name))
               hash = hash * Primes[1] + name.GetHashCode();

            hash = hash * Primes[1] + Attributes.GetHashCode();
            hash = hash * Primes[1] + CreationTimeUtc.GetHashCode();
            hash = hash * Primes[1] + LastWriteTimeUtc.GetHashCode();

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

      #region RefreshEntryInfo

      /// <summary>Refreshes the state of the <see cref="T:FileSystemEntryInfo"/> EntryInfo instance.
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>FileSystemInfo.RefreshEntryInfo() takes a snapshot of the file from the current file system.</para>
      /// <para>Refresh cannot correct the underlying file system even if the file system returns incorrect or outdated information.</para>
      /// <para>This can happen on platforms such as Windows 98.</para>
      /// <para>Calls must be made to Refresh() before attempting to get the attribute information, or the information will be outdated.</para>
      /// </remarks>
      /// </summary>
      [SecurityCritical]
      protected void RefreshEntryInfo()
      {
         _entryInfo = File.GetFileSystemEntryInfoInternal(IsDirectory, Transaction, LongFullName, false, false, null);
   
         if (_entryInfo == null)
            DataInitialised = -1;
         else
         {
            DataInitialised = 0;
            Win32AttributeData = new NativeMethods.Win32FileAttributeData(_entryInfo.Win32FindData.FileAttributes, _entryInfo.Win32FindData.CreationTime, _entryInfo.Win32FindData.LastAccessTime, _entryInfo.Win32FindData.LastWriteTime, _entryInfo.Win32FindData.FileSizeHigh, _entryInfo.Win32FindData.FileSizeLow);
         }
      }

      #endregion // RefreshEntryInfo

      #region Reset

      /// <summary>[AlphaFS] Resets the state of the file system object to uninitialized.</summary>
      internal void Reset()
      {
         DataInitialised = -1;
      }

      #endregion // Reset

      #region InitializeInternal

      /// <summary>[AlphaFS] Initializes the specified file name.</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The full path and name of the file.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      internal void InitializeInternal(bool isFolder, KernelTransaction transaction, string path, bool? isFullPath)
      {
         if (isFullPath != null && (bool) !isFullPath)
            Path.CheckValidPath(path, true, true);

         LongFullName = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(transaction, path, true, false, false, !isFolder, true, false, false);
#else
               // (Not on MSDN): .NET 4+ Trailing spaces are removed from the end of the path parameter before creating the FileSystemInfo instance.
               : Path.GetFullPathInternal(transaction, path, true, true, false, !isFolder, true, false, false);
#endif

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

      /// <summary>Gets or sets the attributes for the current file or directory.
      /// <para>&#160;</para>
      /// <value><see cref="T:FileAttributes"/> of the current <see cref="T:FileSystemInfo"/>.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the CreationTime property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="FileNotFoundException">The specified file does not exist.</exception>
      /// <exception cref="DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      public FileAttributes Attributes
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            return Win32AttributeData.FileAttributes;
         }

         [SecurityCritical]
         set
         {
            File.SetAttributesInternal(IsDirectory, Transaction, LongFullName, value, false, null);
            Reset();
         }
      }

      #endregion // Attributes

      #region CreationTime

      /// <summary>Gets or sets the creation time of the current file or directory.
      /// <para>&#160;</para>
      /// <value>The creation date and time of the current <see cref="T:FileSystemInfo"/> object.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the CreationTime property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>This method may return an inaccurate value, because it uses native functions</para>
      /// <para>whose values may not be continuously updated by the operating system.</para>
      /// <para>&#160;</para>
      /// <para>If the file described in the FileSystemInfo object does not exist, this property will return</para>
      /// <para>12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.</para>
      /// <para>&#160;</para>
      /// <para>NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time.</para>
      /// <para>This process is known as file tunneling. As a result, it may be necessary to explicitly set the creation time</para>
      /// <para>of a file if you are overwriting or replacing an existing file.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      public DateTime CreationTime
      {
         [SecurityCritical] get { return CreationTimeUtc.ToLocalTime(); }
         [SecurityCritical] set { CreationTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // CreationTime

      #region CreationTimeUtc

      /// <summary>Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.
      /// <para>&#160;</para>
      /// <value>The creation date and time in UTC format of the current <see cref="T:FileSystemInfo"/> object.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the CreationTimeUtc property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>This method may return an inaccurate value, because it uses native functions</para>
      /// <para>whose values may not be continuously updated by the operating system.</para>
      /// <para>&#160;</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>If the file described in the FileSystemInfo object does not exist, this property will return</para>
      /// <para>12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC).</para>
      /// <para>&#160;</para>
      /// <para>NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time.</para>
      /// <para>This process is known as file tunneling. As a result, it may be necessary to explicitly set the creation time</para>
      /// <para>of a file if you are overwriting or replacing an existing file.</para>
      /// </remarks>
      /// <exception cref="DirectoryNotFoundException">The specified path is invalid; for example, it is on an unmapped drive.</exception>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      /// </summary>
      [ComVisible(false)]
      public DateTime CreationTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            return DateTime.FromFileTimeUtc(Win32AttributeData.CreationTime);
         }

         [SecurityCritical]
         set
         {
            File.SetFsoDateTimeInternal(IsDirectory, Transaction, LongFullName, value, null, null, null);
            Reset();
         }
      }

      #endregion // CreationTimeUtc

      #region Exists

      /// <summary>Gets a value indicating whether the file or directory exists.
      /// <para>&#160;</para>
      /// <value><c>true</c> if the file or directory exists; otherwise, <c>false</c>.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The <see cref="T:Exists"/> property returns <c>false</c> if any error occurs while trying to determine if the specified file or directory exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a directory- or file name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the file or directory.</para>
      /// </remarks>
      /// </summary>
      public abstract bool Exists { get; }

      #endregion // Exists

      #region Extension

      /// <summary>Gets the string representing the extension part of the file.
      /// <para>&#160;</para>
      /// <value>A string containing the <see cref="T:FileSystemInfo"/> extension.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The Extension property returns the <see cref="T:FileSystemInfo"/> extension, including the period (.).</para>
      /// <para>For example, for a file c:\NewFile.txt, this property returns ".txt".</para>
      /// </remarks>
      /// </summary>
      public string Extension
      {
         get { return Path.GetExtension(FullPath, false); }
      }

      #endregion // Extension

      #region FullName

      /// <summary>Gets the full path of the directory or file.
      /// <para>&#160;</para>
      /// <value>A string containing the full path.</value>
      /// </summary>
      public virtual string FullName
      {
         [SecurityCritical]
         get { return FullPath; }
      }

      #endregion // FullName

      #region LastAccessTime

      /// <summary>Gets or sets the time the current file or directory was last accessed.
      /// <para>&#160;</para>
      /// <value>The time that the current file or directory was last accessed.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the LastAccessTime property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>This method may return an inaccurate value, because it uses native functions</para>
      /// <para>whose values may not be continuously updated by the operating system.</para>
      /// <para>&#160;</para>
      /// <para>If the file described in the FileSystemInfo object does not exist, this property will return</para>
      /// <para>12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      public DateTime LastAccessTime
      {
         [SecurityCritical] get { return LastAccessTimeUtc.ToLocalTime(); }
         [SecurityCritical] set { LastAccessTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // LastAccessTime

      #region LastAccessTimeUtc

      /// <summary>Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.
      /// <para>&#160;</para>
      /// <value>The UTC time that the current file or directory was last accessed.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the LastAccessTimeUtc property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>This method may return an inaccurate value, because it uses native functions</para>
      /// <para>whose values may not be continuously updated by the operating system.</para>
      /// <para>&#160;</para>
      /// <para>If the file described in the FileSystemInfo object does not exist, this property will return</para>
      /// <para>12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      [ComVisible(false)]
      public DateTime LastAccessTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            return DateTime.FromFileTimeUtc(Win32AttributeData.LastAccessTime);
         }

         [SecurityCritical]
         set
         {
            File.SetFsoDateTimeInternal(IsDirectory, Transaction, LongFullName, null, value, null, null);
            Reset();
         }
      }

      #endregion // LastAccessTimeUtc

      #region LastWriteTime

      /// <summary>Gets or sets the time when the current file or directory was last written to.
      /// <para>&#160;</para>
      /// <value>The time the current file was last written.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the LastWriteTime property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>This method may return an inaccurate value, because it uses native functions</para>
      /// <para>whose values may not be continuously updated by the operating system.</para>
      /// <para>&#160;</para>
      /// <para>If the file described in the FileSystemInfo object does not exist, this property will return</para>
      /// <para>12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      public DateTime LastWriteTime
      {
         get { return LastWriteTimeUtc.ToLocalTime(); }
         set { LastWriteTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // LastWriteTime

      #region LastWriteTimeUtc

      /// <summary>Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.
      /// <para>&#160;</para>
      /// <value>The UTC time when the current file was last written to.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the LastWriteTimeUtc property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// <para>&#160;</para>
      /// <para>This method may return an inaccurate value, because it uses native functions</para>
      /// <para>whose values may not be continuously updated by the operating system.</para>
      /// <para>&#160;</para>
      /// <para>If the file described in the FileSystemInfo object does not exist, this property will return</para>
      /// <para>12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="IOException">Refresh cannot initialize the data.</exception>
      [ComVisible(false)]
      public DateTime LastWriteTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            return DateTime.FromFileTimeUtc(Win32AttributeData.LastWriteTime);
         }

         [SecurityCritical]
         set
         {
            File.SetFsoDateTimeInternal(IsDirectory, Transaction, LongFullName, null, null, value, null);
            Reset();
         }
      }

      #endregion // LastWriteTimeUtc

      #region Name

      /// <summary>For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists.
      /// <para>Otherwise, the Name property gets the name of the directory.</para>
      /// <para>&#160;</para>
      /// <value>
      /// <para>A string that is the name of the parent directory, the name of the last directory in the hierarchy,</para>
      /// <para>or the name of a file, including the file name extension.</para>
      /// </value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>For a directory, Name returns only the name of the parent directory, such as Dir, not c:\Dir.</para>
      /// <para>For a subdirectory, Name returns only the name of the subdirectory, such as Sub1, not c:\Dir\Sub1.</para>
      /// <para>For a file, Name returns only the file name and file name extension, such as MyFile.txt, not c:\Dir\Myfile.txt.</para>
      /// </remarks>
      /// </summary>
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
         [SecurityCritical]
         get
         {
            if (_entryInfo == null)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               RefreshEntryInfo();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            return _entryInfo;
         }

         internal set
         {
            _entryInfo = value;
         }
      }

      #endregion // EntryInfo

      #region IsDirectory

      /// <summary>[AlphaFS] The initial "IsDirectory" indicator that was passed to the constructor.</summary>
      protected internal bool IsDirectory { get; set; }

      #endregion // IsDirectory

      #region LengthStreams

      private long _lengthStreams = -1;

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all streams.</returns>
      [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "We do the same for FileInfo.Length property.")]
      public long LengthStreams
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, LongFullName, true);

            
            bool isFolder = IsDirectory || Name == Path.CurrentDirectoryPrefix;
            
            _lengthStreams = AlternateDataStreamInfo.GetStreamSizeInternal(isFolder, Transaction, null, LongFullName, null, null, null);

            return _lengthStreams;
         }
      }

      #endregion // LengthStreams

      #region LongFullName

      /// <summary>The full path of the file system object in Unicode (LongPath) format.</summary>
      protected string LongFullName { get; set; }

      #endregion // LongFullName

      #region Transaction

      [NonSerialized] private KernelTransaction _transaction;

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

      // We use this field in conjunction with the Refresh methods, if we succeed
      // we store a zero, on failure we store the HResult in it so that we can
      // give back a generic error back.
      [NonSerialized] internal int DataInitialised = -1;

      // The pre-cached FileSystemInfo information.
      [NonSerialized] internal NativeMethods.Win32FileAttributeData Win32AttributeData;

      #region .NET

      /// <summary>Represents the fully qualified path of the file or directory.
      /// <remarks>
      /// <para>Classes derived from <see cref="T:FileSystemInfo"/> can use the FullPath field</para>
      /// <para>to determine the full path of the object being manipulated.</para>
      /// </remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string FullPath;

      /// <summary>The path originally specified by the user, whether relative or absolute.</summary>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string OriginalPath;

      #endregion // .NET
       
      #endregion // Fields
   }
}
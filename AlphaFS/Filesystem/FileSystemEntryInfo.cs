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
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Represents information about a file system entry.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [SerializableAttribute]
   [SecurityCritical]
   public sealed class FileSystemEntryInfo
   {
      /// <summary>Initializes a new instance of the <see cref="FileSystemEntryInfo"/> class.</summary>
      /// <param name="findData">The NativeMethods.WIN32_FIND_DATA structure.</param>
      internal FileSystemEntryInfo(NativeMethods.WIN32_FIND_DATA findData)
      {
         Win32FindData = findData;
      }


      #region Properties

      /// <summary>Gets the 8.3 version of the filename.</summary>
      /// <value>the 8.3 version of the filename.</value>
      public string AlternateFileName
      {
         get { return Win32FindData.cAlternateFileName; }
      }


      /// <summary>Gets the attributes.</summary>
      /// <value>The attributes.</value>
      public FileAttributes Attributes
      {
         get { return Win32FindData.dwFileAttributes; }
      }


      /// <summary>Gets the time this entry was created.</summary>
      /// <value>The time this entry was created.</value>
      public DateTime CreationTime
      {
         get { return CreationTimeUtc.ToLocalTime(); }
      }


      /// <summary>Gets the time, in coordinated universal time (UTC), this entry was created.</summary>
      /// <value>The time, in coordinated universal time (UTC), this entry was created.</value>
      public DateTime CreationTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(Win32FindData.ftCreationTime); }
      }


      /// <summary>Gets the name of the file.</summary>
      /// <value>The name of the file.</value>
      public string FileName
      {
         get { return Win32FindData.cFileName; }
      }


      /// <summary>Gets the size of the file.</summary>
      /// <value>The size of the file.</value>
      public long FileSize
      {
         get { return NativeMethods.ToLong(Win32FindData.nFileSizeHigh, Win32FindData.nFileSizeLow); }
      }


      private string _fullPath;

      /// <summary>The full path of the file system object.</summary>
      public string FullPath
      {
         get { return _fullPath; }
         set
         {
            LongFullPath = value;
            _fullPath = Path.GetRegularPathCore(LongFullPath, GetFullPathOptions.None, false);
         }
      }


      /// <summary>Gets a value indicating whether this instance is compressed.</summary>
      /// <value><see langword="true"/> if this instance is compressed; otherwise, <see langword="false"/>.</value>
      /// <remarks>
      /// It is not possible to change the compression status of a File object by using the SetAttributes method.
      /// Instead, you must actually compress the file using either a compression tool or one of the classes in the <see cref="System.IO.Compression"/> namespace.
      /// </remarks>
      public bool IsCompressed
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.Compressed) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance is hidden, and thus is not included in an ordinary directory listing.</summary>
      /// <value><see langword="true"/> if this instance is hidden; otherwise, <see langword="false"/>.</value>
      public bool IsHidden
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.Hidden) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
      /// <value><see langword="true"/> if this instance represents a directory; otherwise, <see langword="false"/>.</value>
      public bool IsDirectory
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.Directory) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance is encrypted (EFS).</summary>
      /// <value><see langword="true"/> if this instance is encrypted (EFS); otherwise, <see langword="false"/>.</value>
      /// <remarks>
      /// For a file, this means that all data in the file is encrypted.
      /// For a directory, this means that encryption is the default for newly created files and directories.
      /// </remarks>
      public bool IsEncrypted
      {
         get { return Attributes != (FileAttributes) (-1) && (Attributes & FileAttributes.Encrypted) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance is a mount point.</summary>
      /// <value><see langword="true"/> if this instance is a mount point; otherwise, <see langword="false"/>.</value>
      public bool IsMountPoint
      {
         get { return ReparsePointTag == ReparsePointTag.MountPoint; }
      }


      /// <summary>Gets a value indicating whether this instance is offline. The data of the file is not immediately available.</summary>
      /// <value><see langword="true"/> if this instance is offline; otherwise, <see langword="false"/>.</value>
      public bool IsOffline
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.Offline) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance is read-only.</summary>
      /// <value><see langword="true"/> if this instance is read-only; otherwise, <see langword="false"/>.</value>
      public bool IsReadOnly
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.ReadOnly) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance contains a reparse point, which is a block of user-defined data associated with a file or a directory.</summary>
      /// <value><see langword="true"/> if this instance contains a reparse point; otherwise, <see langword="false"/>.</value>
      public bool IsReparsePoint
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.ReparsePoint) != 0; }
      }


      /// <summary>Gets a value indicating whether this instance is a symbolic link.</summary>
      /// <value><see langword="true"/> if this instance is a symbolic link; otherwise, <see langword="false"/>.</value>
      public bool IsSymbolicLink
      {
         get { return ReparsePointTag == ReparsePointTag.SymLink; }
      }


      /// <summary>Gets the time this entry was last accessed.</summary>
      /// <value>The time this entry was last accessed.</value>
      public DateTime LastAccessTime
      {
         get { return LastAccessTimeUtc.ToLocalTime(); }
      }


      /// <summary>Gets the time, in coordinated universal time (UTC), this entry was last accessed.</summary>
      /// <value>The time, in coordinated universal time (UTC), this entry was last accessed.</value>
      public DateTime LastAccessTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(Win32FindData.ftLastAccessTime); }
      }


      /// <summary>Gets the time this entry was last modified.</summary>
      /// <value>The time this entry was last modified.</value>
      public DateTime LastWriteTime
      {
         get { return LastWriteTimeUtc.ToLocalTime(); }
      }


      /// <summary>Gets the time, in coordinated universal time (UTC), this entry was last modified.</summary>
      /// <value>The time, in coordinated universal time (UTC), this entry was last modified.</value>
      public DateTime LastWriteTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(Win32FindData.ftLastWriteTime); }
      }


      private string _longFullPath;

      /// <summary>The full path of the file system object in Unicode (LongPath) format.</summary>
      public string LongFullPath
      {
         get { return _longFullPath; }
         private set { _longFullPath = Path.GetLongPathCore(value, GetFullPathOptions.None); }
      }


      /// <summary>Gets the reparse point tag of this entry.</summary>
      /// <value>The reparse point tag of this entry.</value>
      public ReparsePointTag ReparsePointTag
      {
         get { return IsReparsePoint ? Win32FindData.dwReserved0 : ReparsePointTag.None; }
      }


      /// <summary>Gets internal WIN32 FIND Data</summary>
      internal NativeMethods.WIN32_FIND_DATA Win32FindData { get; private set; }

      #endregion // Properties


      #region Methods

      /// <summary>Returns the <see cref="FullPath"/> of the <see cref="FileSystemEntryInfo"/> instance.</summary>
      /// <returns>The <see cref="FullPath"/> instance as a string.</returns>
      public override string ToString()
      {
         return FullPath;
      }

      #endregion // Methods
   }
}

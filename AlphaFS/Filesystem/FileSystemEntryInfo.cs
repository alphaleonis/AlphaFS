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
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Represents information about a file system entry.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [Serializable]
   [SecurityCritical]
   public sealed class FileSystemEntryInfo
   {
      /// <summary>Initializes a new instance of the <see cref="FileSystemEntryInfo"/> class.</summary>
      /// <param name="findData">The NativeMethods.WIN32_FIND_DATA structure.</param>
      internal FileSystemEntryInfo(NativeMethods.WIN32_FIND_DATA findData)
      {
         Win32FindData = findData;
      }




      /// <summary>Returns the <see cref="FullPath"/> of the <see cref="FileSystemEntryInfo"/> instance.</summary>
      public override string ToString()
      {
         return FullPath;
      }





      /// <summary>The instance 8.3 version of the filename.</summary>
      /// <remarks>This property is always empty when <see cref="NativeMethods.FINDEX_INFO_LEVELS.Basic"/> is used.</remarks>
      public string AlternateFileName
      {
         get { return Win32FindData.cAlternateFileName; }
      }


      /// <summary>The instance attributes.</summary>
      public FileAttributes Attributes
      {
         get { return Win32FindData.dwFileAttributes; }
      }


      /// <summary>The instance creation time.</summary>
      public DateTime CreationTime
      {
         get { return CreationTimeUtc.ToLocalTime(); }
      }


      /// <summary>The instance creation time, in coordinated universal time (UTC).</summary>
      public DateTime CreationTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(Win32FindData.ftCreationTime); }
      }


      /// <summary>The instance file extension.</summary>
      public string Extension
      {
         get { return Path.GetExtension(Win32FindData.cFileName, false); }
      }


      /// <summary>The instance file name.</summary>
      public string FileName
      {
         get { return Win32FindData.cFileName; }
      }


      /// <summary>The instance file size.</summary>
      public long FileSize
      {
         get { return NativeMethods.ToLong(Win32FindData.nFileSizeHigh, Win32FindData.nFileSizeLow); }
      }


      private string _fullPath;
      /// <summary>The instance full path.</summary>
      public string FullPath
      {
         get { return _fullPath; }

         set
         {
            LongFullPath = value;
            _fullPath = Path.GetRegularPathCore(LongFullPath, GetFullPathOptions.None, false);
         }
      }


      /// <summary>The instance is a candidate for backup or removal. </summary>
      public bool IsArchive
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Archive) != 0; }
      }


      /// <summary>The instance is compressed.</summary>
      public bool IsCompressed
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Compressed) != 0; }
      }


      /// <summary>Reserved for future use.</summary>
      public bool IsDevice
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Device) != 0; }
      }


      /// <summary>The instance is a directory.</summary>
      public bool IsDirectory
      {
         get { return File.IsDirectory(Attributes); }
      }


      /// <summary>The instance is encrypted. For a file, this means that all data in the file is encrypted. For a directory, this means that encryption is the default for newly created files and directories.</summary>
      public bool IsEncrypted
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Encrypted) != 0; }
      }


      /// <summary>The instance is hidden, and thus is not included in an ordinary directory listing.</summary>
      public bool IsHidden
      {
         get { return File.IsHidden(Attributes); }
      }


      /// <summary>The instance is a mount point. Applicable to local directories and local volumes.</summary>
      public bool IsMountPoint
      {
         get { return ReparsePointTag == ReparsePointTag.MountPoint; }
      }


      /// <summary>The instance is a standard file that has no special attributes. This attribute is valid only if it is used alone.</summary>
      public bool IsNormal
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Normal) != 0; }
      }


      /// <summary>The instance will not be indexed by the operating system's content indexing service.</summary>
      public bool IsNotContentIndexed
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.NotContentIndexed) != 0; }
      }


      /// <summary>The instance is offline. The data of the file is not immediately available.</summary>
      public bool IsOffline
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Offline) != 0; }
      }


      /// <summary>The instance is read-only.</summary>
      public bool IsReadOnly
      {
         get { return File.IsReadOnly(Attributes); }
      }


      /// <summary>The instance contains a reparse point, which is a block of user-defined data associated with a file or a directory.</summary>
      public bool IsReparsePoint
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.ReparsePoint) != 0; }
      }


      /// <summary>The instance is a sparse file. Sparse files are typically large files whose data consists of mostly zeros.</summary>
      public bool IsSparseFile
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.SparseFile) != 0; }
      }


      /// <summary>The instance is a symbolic link.</summary>
      public bool IsSymbolicLink
      {
         get { return ReparsePointTag == ReparsePointTag.SymLink; }
      }


      /// <summary>The instance is a system file. That is, the file is part of the operating system or is used exclusively by the operating system.</summary>
      public bool IsSystem
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.System) != 0; }
      }


      /// <summary>The instance is temporary. A temporary file contains data that is needed while an application is executing but is not needed after the application is finished.
      /// File systems try to keep all the data in memory for quicker access rather than flushing the data back to mass storage.
      /// A temporary file should be deleted by the application as soon as it is no longer needed.</summary>
      public bool IsTemporary
      {
         get { return File.HasValidAttributes(Attributes) && (Attributes & FileAttributes.Temporary) != 0; }
      }


      /// <summary>The instance time this entry was last accessed.</summary>
      public DateTime LastAccessTime
      {
         get { return LastAccessTimeUtc.ToLocalTime(); }
      }


      /// <summary>The instance time, in coordinated universal time (UTC), this entry was last accessed.</summary>
      public DateTime LastAccessTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(Win32FindData.ftLastAccessTime); }
      }


      /// <summary>The instance time this entry was last modified.</summary>
      public DateTime LastWriteTime
      {
         get { return LastWriteTimeUtc.ToLocalTime(); }
      }


      /// <summary>The instance time, in coordinated universal time (UTC), this entry was last modified.</summary>
      public DateTime LastWriteTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(Win32FindData.ftLastWriteTime); }
      }


      private string _longFullPath;
      /// <summary>The instance full path in long path format.</summary>
      public string LongFullPath
      {
         get { return _longFullPath; }

         private set { _longFullPath = Path.GetLongPathCore(value, GetFullPathOptions.None); }
      }


      /// <summary>The instance reparse point tag.</summary>
      public ReparsePointTag ReparsePointTag
      {
         get { return IsReparsePoint ? Win32FindData.dwReserved0 : ReparsePointTag.None; }
      }


      /// <summary>The instance internal WIN32 FIND Data</summary>
      internal NativeMethods.WIN32_FIND_DATA Win32FindData { get; private set; }
   }
}

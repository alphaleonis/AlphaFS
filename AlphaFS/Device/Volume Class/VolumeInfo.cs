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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Contains information about a filesystem Volume.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class VolumeInfo
   {
      #region Fields

      [NonSerialized] private readonly bool _continueOnAccessError;
      [NonSerialized] private readonly SafeFileHandle _volumeHandle;
      [NonSerialized] private string _guid;
      [NonSerialized] private Device.NativeMethods.VOLUME_INFO_FLAGS _volumeInfoAttributes;

      #endregion // Private Fields


      #region Constructors

      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: \\server\share.</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public VolumeInfo(string driveName)
      {
         if (null == driveName)
            throw new ArgumentNullException("driveName");

         if (driveName.Trim().Length == 0)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "driveName");


         if (!driveName.StartsWith(Path.LongPathPrefix, StringComparison.Ordinal))
            driveName = Path.IsUncPathCore(driveName, false, false) ? Path.GetLongPathCore(driveName, GetFullPathOptions.None) : Path.LongPathPrefix + driveName;

         else
         {
            driveName = driveName.Length == 1 ? driveName + Path.VolumeSeparatorChar : Path.GetPathRoot(driveName, false);

            if (null != driveName && !driveName.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               driveName = Path.GetPathRoot(driveName, false);
         }


         if (Utils.IsNullOrWhiteSpace(driveName))
            throw new ArgumentException(Resources.InvalidDriveLetterArgument, "driveName");


         Name = Path.AddTrailingDirectorySeparator(driveName, false);

         _volumeHandle = null;
      }


      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: "\\server\share".</param>
      /// <param name="refresh">Refreshes the state of the object.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public VolumeInfo(string driveName, bool refresh, bool continueOnException) : this(driveName)
      {
         _continueOnAccessError = continueOnException;

         if (refresh)
            Refresh();
      }


      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <param name="volumeHandle">An instance to a <see cref="SafeFileHandle"/> handle.</param>
      [SecurityCritical]
      public VolumeInfo(SafeFileHandle volumeHandle)
      {
         Utils.IsValidHandle(volumeHandle);

         _volumeHandle = volumeHandle;
      }


      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <param name="volumeHandle">An instance to a <see cref="SafeFileHandle"/> handle.</param>
      /// <param name="refresh">Refreshes the state of the object.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public VolumeInfo(SafeFileHandle volumeHandle, bool refresh, bool continueOnException) : this(volumeHandle)
      {
         _continueOnAccessError = continueOnException;

         if (refresh)
            Refresh();
      }

      #endregion // Constructors


      /// <summary>Returns the full path of the volume.</summary>
      /// <returns>Returns a string that represents this instance.</returns>
      public override string ToString()
      {
         return Guid;
      }


      #region Properties

      /// <summary>The specified volume supports preserved case of file names when it places a name on disk.</summary>
      public bool CasePreservedNames
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_CASE_PRESERVED_NAMES) != 0; }
      }


      /// <summary>The specified volume supports case-sensitive file names.</summary>
      public bool CaseSensitiveSearch
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_CASE_SENSITIVE_SEARCH) != 0; }
      }


      /// <summary>The specified volume supports file-based compression.</summary>
      public bool Compression
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_FILE_COMPRESSION) != 0; }
      }


      /// <summary>The specified volume is a direct access (DAX) volume.</summary>
      public bool DirectAccess
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_DAX_VOLUME) != 0; }
      }


      /// <summary>Gets the drive type.</summary>
      /// <returns>One of the <see cref="System.IO.DriveType"/> values.</returns>
      /// <remarks>
      /// The DriveType property indicates whether a drive is any of: CDRom, Fixed, Unknown, Network, NoRootDirectory,
      /// Ram, Removable, or Unknown. Values are listed in the <see cref="System.IO.DriveType"/> enumeration.
      /// </remarks>
      internal DriveType DriveType { get; private set; }


      /// <summary>Gets the name of the file system, for example, the FAT file system or the NTFS file system.</summary>
      /// <value>The name of the file system.</value>
      public string FileSystemName { get; private set; }


      /// <summary>The full path to the volume.</summary>
      public string FullPath { get; private set; }


      /// <summary>The volume <see cref="Guid"/>.</summary>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      public string Guid
      {
         get
         {
            if (null == _guid)
            {
               try
               {
                  _guid = !Utils.IsNullOrWhiteSpace(FullPath) ? Volume.GetVolumeGuid(FullPath) : null;
               }
               catch
               {
               }
            }

            return _guid;
         }
      }


      /// <summary>Gets the maximum length of a file name component that the file system supports.</summary>
      /// <value>The maximum length of a file name component that the file system supports.</value>      
      public int MaximumComponentLength { get; set; }


      /// <summary>Gets the label of the volume.</summary>
      /// <returns>The label of the volume.</returns>
      /// <remarks>This property is the label assigned to the volume, such as: "MyDrive"</remarks>
      public string Name { get; private set; }


      /// <summary>The specified volume supports named streams.</summary>
      public bool NamedStreams
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_NAMED_STREAMS) != 0; }
      }


      /// <summary>The specified volume preserves and enforces access control lists (ACL).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Acls")]
      public bool PersistentAcls
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_PERSISTENT_ACLS) != 0; }
      }


      /// <summary>The specified volume is read-only.</summary>
      public bool ReadOnlyVolume
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_READ_ONLY_VOLUME) != 0; }
      }


      /// <summary>The specified volume supports a single sequential write.</summary>
      public bool SequentialWriteOnce
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SEQUENTIAL_WRITE_ONCE) != 0; }
      }


      /// <summary>Gets the volume serial number that the operating system assigns when a hard disk is formatted.</summary>
      /// <value>The volume serial number that the operating system assigns when a hard disk is formatted.</value>
      public long SerialNumber { get; private set; }


      /// <summary>The specified volume supports the Encrypted File System (EFS).</summary>
      public bool SupportsEncryption
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_ENCRYPTION) != 0; }
      }


      /// <summary>The specified volume supports extended attributes.</summary>
      public bool SupportsExtendedAttributes
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_EXTENDED_ATTRIBUTES) != 0; }
      }


      /// <summary>The specified volume supports hard links.</summary>
      public bool SupportsHardLinks
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_HARD_LINKS) != 0; }
      }


      /// <summary>The specified volume supports object identifiers.</summary>
      public bool SupportsObjectIds
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_OBJECT_IDS) != 0; }
      }


      /// <summary>The file system supports open by FileID.</summary>
      public bool SupportsOpenByFileId
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_OPEN_BY_FILE_ID) != 0; }
      }


      /// <summary>The specified volume supports remote storage. (This property does not appear on MSDN)</summary>
      public bool SupportsRemoteStorage
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_REMOTE_STORAGE) != 0; }
      }


      /// <summary>The specified volume supports re-parse points.</summary>
      public bool SupportsReparsePoints
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_REPARSE_POINTS) != 0; }
      }


      /// <summary>The specified volume supports sparse files.</summary>
      public bool SupportsSparseFiles
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_SPARSE_FILES) != 0; }
      }


      /// <summary>The specified volume supports transactions.</summary>
      public bool SupportsTransactions
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_TRANSACTIONS) != 0; }
      }


      /// <summary>The specified volume supports update sequence number (USN) journals.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Usn")]
      public bool SupportsUsnJournal
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_SUPPORTS_USN_JOURNAL) != 0; }
      }


      /// <summary>The specified volume supports Unicode in file names as they appear on disk.</summary>
      public bool UnicodeOnDisk
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_UNICODE_ON_DISK) != 0; }
      }


      /// <summary>The specified volume is a compressed volume, for example, a DoubleSpace volume.</summary>
      public bool VolumeIsCompressed
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_VOLUME_IS_COMPRESSED) != 0; }
      }


      /// <summary>The specified volume supports disk quotas.</summary>
      public bool VolumeQuotas
      {
         get { return (_volumeInfoAttributes & Device.NativeMethods.VOLUME_INFO_FLAGS.FILE_VOLUME_QUOTAS) != 0; }
      }

      #endregion // Properties


      #region Methods

      /// <summary>Refreshes the state of the object.</summary>
      public void Refresh()
      {
         var volumeNameBuffer = new StringBuilder(NativeMethods.MaxPath + 1);
         var fileSystemNameBuffer = new StringBuilder(NativeMethods.MaxPath + 1);
         int maximumComponentLength;
         uint serialNumber;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            uint lastError;

            do
            {
               var success = null != _volumeHandle && NativeMethods.IsAtLeastWindowsVista

                  ? Device.NativeMethods.GetVolumeInformationByHandle(_volumeHandle, volumeNameBuffer, (uint) volumeNameBuffer.Capacity, out serialNumber, out maximumComponentLength, out _volumeInfoAttributes, fileSystemNameBuffer, (uint) fileSystemNameBuffer.Capacity)

                  // A trailing backslash is required.
                  : Device.NativeMethods.GetVolumeInformation(Path.AddTrailingDirectorySeparator(Name, false), volumeNameBuffer, (uint) volumeNameBuffer.Capacity, out serialNumber, out maximumComponentLength, out _volumeInfoAttributes, fileSystemNameBuffer, (uint) fileSystemNameBuffer.Capacity);


               lastError = (uint) Marshal.GetLastWin32Error();

               if (success)
                  break;

               switch (lastError)
               {
                  case Win32Errors.ERROR_NOT_READY:
                     if (!_continueOnAccessError)
                        throw new DeviceNotReadyException(Name, true);
                     break;

                  case Win32Errors.ERROR_MORE_DATA:
                     //This code should never execute.
                     volumeNameBuffer.Capacity *= 2;
                     fileSystemNameBuffer.Capacity *= 2;
                     break;

                  default:
                     if (!_continueOnAccessError)
                        NativeError.ThrowException(lastError, Name);
                     break;
               }

            } while (lastError == Win32Errors.ERROR_MORE_DATA);
         }


         FullPath = Path.GetRegularPathCore(Name, GetFullPathOptions.None, false);

         Name = volumeNameBuffer.ToString();
         
         MaximumComponentLength = maximumComponentLength;

         SerialNumber = serialNumber;

         FileSystemName = fileSystemNameBuffer.ToString();
         FileSystemName = !Utils.IsNullOrWhiteSpace(FileSystemName) ? FileSystemName : null;
         
         DriveType = Volume.GetDriveType(FullPath);
      }

      #endregion // Methods
   }
}

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

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Contains information about a filesystem Volume.</summary>
   [SerializableAttribute]
   [SecurityCritical]
   public sealed class VolumeInfo
   {
      #region Constructor

      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="volumeName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: \\server\share.</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      public VolumeInfo(string volumeName)
      {
         if (Utils.IsNullOrWhiteSpace(volumeName))
            throw new ArgumentNullException("volumeName");

         if (!volumeName.StartsWith(Path.LongPathPrefix, StringComparison.OrdinalIgnoreCase))
            volumeName = Path.IsUncPathCore(volumeName, false, false)
               ? Path.GetLongPathCore(volumeName, GetFullPathOptions.None)
               : Path.LongPathPrefix + volumeName;
         else
         {
            if (volumeName.Length == 1)
               volumeName += Path.VolumeSeparatorChar;
            else if (!volumeName.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               volumeName = Path.GetPathRoot(volumeName, false);
         }

         if (Utils.IsNullOrWhiteSpace(volumeName))
            throw new ArgumentException("Argument must be a drive letter (\"C\"), RootDir (\"C:\\\") or UNC path (\"\\\\server\\share\")");

         Name = Path.AddTrailingDirectorySeparator(volumeName, false);

         _volumeHandle = null;
      }

      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: "\\server\share".</param>
      /// <param name="refresh">Refreshes the state of the object.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
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
         _volumeHandle = volumeHandle;
      }

      /// <summary>Initializes a VolumeInfo instance.</summary>
      /// <param name="volumeHandle">An instance to a <see cref="SafeFileHandle"/> handle.</param>
      /// <param name="refresh">Refreshes the state of the object.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public VolumeInfo(SafeFileHandle volumeHandle, bool refresh, bool continueOnException) : this(volumeHandle)
      {
         _continueOnAccessError = continueOnException;

         if (refresh)
            Refresh();
      }

      #endregion // Constructor

      #region Fields

      [NonSerialized] private readonly bool _continueOnAccessError;
      [NonSerialized] private readonly SafeFileHandle _volumeHandle;
      [NonSerialized] private NativeMethods.VolumeInfoAttributes _volumeInfoAttributes;
      #endregion // Fields

      #region Methods

      #region Refresh

      /// <summary>Refreshes the state of the object.</summary>
      public void Refresh()
      {
         var volumeNameBuffer = new StringBuilder(NativeMethods.MaxPath + 1);
         var fileSystemNameBuffer = new StringBuilder(NativeMethods.MaxPath + 1);
         int maximumComponentLength;
         uint serialNumber;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            // GetVolumeInformationXxx()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            uint lastError;

            do
            {
               if (!(_volumeHandle != null && NativeMethods.IsAtLeastWindowsVista

                  // GetVolumeInformationByHandle() / GetVolumeInformation()
                  // In the ANSI version of this function, the name is limited to 248 characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

                  ? NativeMethods.GetVolumeInformationByHandle(_volumeHandle, volumeNameBuffer, (uint) volumeNameBuffer.Capacity, out serialNumber, out maximumComponentLength, out _volumeInfoAttributes, fileSystemNameBuffer, (uint) fileSystemNameBuffer.Capacity)
                  : NativeMethods.GetVolumeInformation(Path.AddTrailingDirectorySeparator(Name, false), volumeNameBuffer, (uint) volumeNameBuffer.Capacity, out serialNumber, out maximumComponentLength, out _volumeInfoAttributes, fileSystemNameBuffer, (uint) fileSystemNameBuffer.Capacity))
                  // A trailing backslash is required.
                  )
               {
                  lastError = (uint) Marshal.GetLastWin32Error();
                  switch (lastError)
                  {
                     case Win32Errors.ERROR_NOT_READY:
                        if (!_continueOnAccessError)
                           throw new DeviceNotReadyException();
                        break;

                     case Win32Errors.ERROR_MORE_DATA:
                        // With a large enough buffer this code never executes.
                        volumeNameBuffer.Capacity = volumeNameBuffer.Capacity*2;
                        fileSystemNameBuffer.Capacity = fileSystemNameBuffer.Capacity*2;
                        break;

                     default:
                        if (!_continueOnAccessError)
                           NativeError.ThrowException(Name);
                        break;
                  }
               }
               else
                  break;

            } while (lastError == Win32Errors.ERROR_MORE_DATA);
         }

         FullPath = Path.GetRegularPathCore(Name, GetFullPathOptions.None, false);
         Name = volumeNameBuffer.ToString();

         FileSystemName = fileSystemNameBuffer.ToString();
         FileSystemName = Utils.IsNullOrWhiteSpace(FileSystemName) ? null : FileSystemName;

         MaximumComponentLength = maximumComponentLength;
         SerialNumber = serialNumber;
      }

      #endregion // Refresh

      #region ToString

      /// <summary>Returns the full path of the volume.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return Guid;
      }

      #endregion // ToString

      #endregion // Methods

      #region Properties

      #region CasePreservedNames

      /// <summary>The specified volume supports preserved case of file names when it places a name on disk.</summary>
      public bool CasePreservedNames
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.CasePreservedNames) == NativeMethods.VolumeInfoAttributes.CasePreservedNames; }
      }

      #endregion // CasePreservedNames

      #region CaseSensitiveSearch

      /// <summary>The specified volume supports case-sensitive file names.</summary>
      public bool CaseSensitiveSearch
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.CaseSensitiveSearch) == NativeMethods.VolumeInfoAttributes.CaseSensitiveSearch; }
      }

      #endregion // CaseSensitiveSearch

      #region Compression

      /// <summary>The specified volume supports file-based compression.</summary>
      public bool Compression
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.Compression) == NativeMethods.VolumeInfoAttributes.Compression; }
      }

      #endregion // Compression

      #region FileSystemName

      /// <summary>Gets the name of the file system, for example, the FAT file system or the NTFS file system.</summary>
      /// <value>The name of the file system.</value>
      public string FileSystemName { get; private set; }

      #endregion // FileSystemName

      #region FullPath

      /// <summary>The full path to the volume.</summary>
      public string FullPath { get; private set; }

      #endregion // FullPath

      #region Guid

      private string _guid;

      /// <summary>The volume GUID.</summary>

      public string Guid
      {
         get
         {
            if (Utils.IsNullOrWhiteSpace(_guid))
               _guid = !Utils.IsNullOrWhiteSpace(FullPath) ? Volume.GetUniqueVolumeNameForPath(FullPath) : null;

            return _guid;
         }
      }

      #endregion // Guid

      #region MaximumComponentLength

      /// <summary>Gets the maximum length of a file name component that the file system supports.</summary>
      /// <value>The maximum length of a file name component that the file system supports.</value>      
      public int MaximumComponentLength { get; set; }

      #endregion // MaximumComponentLength

      #region Name

      /// <summary>Gets the label of the volume.</summary>
      /// <returns>The label of the volume.</returns>
      /// <remarks>This property is the label assigned to the volume, such "MyDrive"</remarks>
      public string Name { get; private set; }

      #endregion // Name

      #region NamedStreams

      /// <summary>The specified volume supports named streams.</summary>
      public bool NamedStreams
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.NamedStreams) == NativeMethods.VolumeInfoAttributes.NamedStreams; }
      }

      #endregion // NamedStreams

      #region PersistentAcls

      /// <summary>The specified volume preserves and enforces access control lists (ACL).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Acls")]
      public bool PersistentAcls
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.PersistentAcls) == NativeMethods.VolumeInfoAttributes.PersistentAcls; }
      }

      #endregion // PersistentAcls

      #region ReadOnlyVolume

      /// <summary>The specified volume is read-only.</summary>
      public bool ReadOnlyVolume
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.ReadOnlyVolume) == NativeMethods.VolumeInfoAttributes.ReadOnlyVolume; }
      }

      #endregion // ReadOnlyVolume

      #region SequentialWriteOnce

      /// <summary>The specified volume supports a single sequential write.</summary>
      public bool SequentialWriteOnce
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SequentialWriteOnce) == NativeMethods.VolumeInfoAttributes.SequentialWriteOnce; }
      }

      #endregion // SequentialWriteOnce

      #region SerialNumber

      /// <summary>Gets the volume serial number that the operating system assigns when a hard disk is formatted.</summary>
      /// <value>The volume serial number that the operating system assigns when a hard disk is formatted.</value>
      public long SerialNumber { get; private set; }

      #endregion // SerialNumber

      #region SupportsEncryption

      /// <summary>The specified volume supports the Encrypted File System (EFS).</summary>
      public bool SupportsEncryption
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsEncryption) == NativeMethods.VolumeInfoAttributes.SupportsEncryption; }
      }

      #endregion // SupportsEncryption

      #region SupportsExtendedAttributes

      /// <summary>The specified volume supports extended attributes.</summary>
      public bool SupportsExtendedAttributes
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsExtendedAttributes) == NativeMethods.VolumeInfoAttributes.SupportsExtendedAttributes; }
      }

      #endregion // SupportsExtendedAttributes

      #region SupportsHardLinks

      /// <summary>The specified volume supports hard links.</summary>
      public bool SupportsHardLinks
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsHardLinks) == NativeMethods.VolumeInfoAttributes.SupportsHardLinks; }
      }

      #endregion // SupportsHardLinks

      #region SupportsObjectIds

      /// <summary>The specified volume supports object identifiers.</summary>
      public bool SupportsObjectIds
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsObjectIds) == NativeMethods.VolumeInfoAttributes.SupportsObjectIds; }
      }

      #endregion // SupportsObjectIds

      #region SupportsOpenByFileId

      /// <summary>The file system supports open by FileID.</summary>
      public bool SupportsOpenByFileId
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsOpenByFileId) == NativeMethods.VolumeInfoAttributes.SupportsOpenByFileId; }
      }

      #endregion // SupportsOpenByFileId

      #region SupportsRemoteStorage

      /// <summary>The specified volume supports remote storage. (This property does not appear on MSDN)</summary>
      public bool SupportsRemoteStorage
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsRemoteStorage) == NativeMethods.VolumeInfoAttributes.SupportsRemoteStorage; }
      }

      #endregion // SupportsRemoteStorage

      #region SupportsReparsePoints

      /// <summary>The specified volume supports re-parse points.</summary>
      public bool SupportsReparsePoints
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsReparsePoints) == NativeMethods.VolumeInfoAttributes.SupportsReparsePoints; }
      }

      #endregion // SupportsReparsePoints

      #region SupportsSparseFiles

      /// <summary>The specified volume supports sparse files.</summary>
      public bool SupportsSparseFiles
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsSparseFiles) == NativeMethods.VolumeInfoAttributes.SupportsSparseFiles; }
      }

      #endregion // SupportsSparseFiles

      #region SupportsTransactions

      /// <summary>The specified volume supports transactions.</summary>
      public bool SupportsTransactions
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsTransactions) == NativeMethods.VolumeInfoAttributes.SupportsTransactions; }
      }

      #endregion // SupportsTransactions

      #region SupportsUsnJournal

      /// <summary>The specified volume supports update sequence number (USN) journals.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Usn")]
      public bool SupportsUsnJournal
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.SupportsUsnJournal) == NativeMethods.VolumeInfoAttributes.SupportsUsnJournal; }
      }

      #endregion // SupportsUsnJournal

      #region UnicodeOnDisk

      /// <summary>The specified volume supports Unicode in file names as they appear on disk.</summary>
      public bool UnicodeOnDisk
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.UnicodeOnDisk) == NativeMethods.VolumeInfoAttributes.UnicodeOnDisk; }
      }

      #endregion // UnicodeOnDisk
      
      #region VolumeIsCompressed

      /// <summary>The specified volume is a compressed volume, for example, a DoubleSpace volume.</summary>
      public bool VolumeIsCompressed
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.VolumeIsCompressed) == NativeMethods.VolumeInfoAttributes.VolumeIsCompressed; }
      }

      #endregion // VolumeIsCompressed

      #region VolumeQuotas

      /// <summary>The specified volume supports disk quotas.</summary>
      public bool VolumeQuotas
      {
         get { return (_volumeInfoAttributes & NativeMethods.VolumeInfoAttributes.VolumeQuotas) == NativeMethods.VolumeInfoAttributes.VolumeQuotas; }
      }

      #endregion // VolumeQuotas

      #endregion // Properties
   }
}

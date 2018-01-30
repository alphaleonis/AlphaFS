/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to information on a local or remote drive.</summary>
   /// <remarks>
   /// This class models a drive and provides methods and properties to query for drive information.
   /// Use DriveInfo to determine what drives are available, and what type of drives they are.
   /// You can also query to determine the capacity and available free space on the drive.
   /// </remarks>
   [Serializable]
   [SecurityCritical]
   public sealed partial class DriveInfo
   {
      #region Private Fields

      [NonSerialized] private readonly VolumeInfo _volumeInfo;
      [NonSerialized] private readonly DiskSpaceInfo _dsi;
      [NonSerialized] private PhysicalDriveInfo _physicalDriveInfo;
      [NonSerialized] private bool _initDsie;
      [NonSerialized] private readonly string _name;
      [NonSerialized] private string _dosDeviceName;
      [NonSerialized] private DirectoryInfo _rootDirectory;

      #endregion // Private Fields


      #region Constructors

      #region .NET

      /// <summary>Provides access to information on the specified drive.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="driveName">
      ///   A valid drive path or drive letter.
      ///   <para>This can be either uppercase or lowercase,</para>
      ///   <para>'a' to 'z' or a network share in the format: \\server\share</para>
      /// </param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public DriveInfo(string driveName)
      {
         if (Utils.IsNullOrWhiteSpace(driveName))
            throw new ArgumentNullException("driveName");


         driveName = driveName.Length == 1 ? driveName + Path.VolumeSeparatorChar : Path.GetPathRoot(driveName, false);

         if (Utils.IsNullOrWhiteSpace(driveName))
            throw new ArgumentException(Resources.InvalidDriveLetterArgument, "driveName");


         _name = Path.AddTrailingDirectorySeparator(driveName, false);


         // Initiate VolumeInfo lazyload instance.
         _volumeInfo = new VolumeInfo(_name, false, true);


         // Initiate DiskSpaceInfo lazyload instance.
         _dsi = new DiskSpaceInfo(_name, null, false, true);


         // Initiate PhysicalDriveInfo lazyload instance.
         _physicalDriveInfo = null;
      }

      #endregion // .NET

      #endregion // Constructors


      #region Properties

      #region .NET

      /// <summary>Indicates the amount of available free space on a drive.</summary>
      /// <returns>The amount of free space available on the drive, in bytes.</returns>
      /// <remarks>This property indicates the amount of free space available on the drive. Note that this number may be different from the <see cref="TotalFreeSpace"/> number because this property takes into account disk quotas.</remarks>
      public long AvailableFreeSpace
      {
         get
         {
            GetDeviceInfo(3, 0);

            return null == _dsi ? 0 : _dsi.FreeBytesAvailable;
         }
      }


      /// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
      /// <remarks>Use DriveFormat to determine what formatting a drive uses.</remarks>
      public string DriveFormat
      {
         get { return (string)GetDeviceInfo(0, 1); }
      }


      /// <summary>Gets the drive type.</summary>
      /// <returns>One of the <see cref="System.IO.DriveType"/> values.</returns>
      /// <remarks>
      /// The DriveType property indicates whether a drive is any of: CDRom, Fixed, Unknown, Network, NoRootDirectory,
      /// Ram, Removable, or Unknown. Values are listed in the <see cref="System.IO.DriveType"/> enumeration.
      /// </remarks>
      public DriveType DriveType
      {
         get { return (DriveType)GetDeviceInfo(0, 3); }
      }


      /// <summary>Gets a value indicating whether a drive is ready.</summary>
      /// <returns><see langword="true"/> if the drive is ready; otherwise, <see langword="false"/>.</returns>
      /// <remarks>
      /// IsReady indicates whether a drive is ready. For example, it indicates whether a CD is in a CD drive or whether
      /// a removable storage device is ready for read/write operations. If you do not test whether a drive is ready, and
      /// it is not ready, querying the drive using DriveInfo will raise an IOException.
      /// 
      /// Do not rely on IsReady() to avoid catching exceptions from other members such as TotalSize, TotalFreeSpace, and DriveFormat.
      /// Between the time that your code checks IsReady and then accesses one of the other properties
      /// (even if the access occurs immediately after the check), a drive may have been disconnected or a disk may have been removed.
      /// </remarks>
      public bool IsReady
      {
         get { return File.ExistsCore(null, true, _name, PathFormat.LongFullPath); }
      }


      /// <summary>Gets the name of the drive.</summary>
      /// <returns>The name of the drive.</returns>
      /// <remarks>This property is the name assigned to the drive, such as C:\ or E:\</remarks>
      public string Name
      {
         get { return _name; }
      }


      /// <summary>Gets the root directory of a drive.</summary>
      /// <returns>A DirectoryInfo object that contains the root directory of the drive.</returns>
      public DirectoryInfo RootDirectory
      {
         get { return (DirectoryInfo)GetDeviceInfo(2, 0); }
      }


      /// <summary>Gets the total amount of free space available on a drive.</summary>
      /// <returns>The total free space available on a drive, in bytes.</returns>
      /// <remarks>This property indicates the total amount of free space available on the drive, not just what is available to the current user.</remarks>
      public long TotalFreeSpace
      {
         get
         {
            GetDeviceInfo(3, 0);

            return null == _dsi ? 0 : _dsi.TotalNumberOfFreeBytes;
         }
      }


      /// <summary>Gets the total size of storage space on a drive.</summary>
      /// <returns>The total size of the drive, in bytes.</returns>
      /// <remarks>This property indicates the total size of the drive in bytes, not just what is available to the current user.</remarks>
      public long TotalSize
      {
         get
         {
            GetDeviceInfo(3, 0);

            return null == _dsi ? 0 : _dsi.TotalNumberOfBytes;
         }
      }


      /// <summary>Gets or sets the volume label of a drive.</summary>
      /// <returns>The volume label.</returns>
      /// <remarks>
      /// The label length is determined by the operating system. For example, NTFS allows a volume label
      /// to be up to 32 characters long. Note that <see langword="null"/> is a valid VolumeLabel.
      /// </remarks>
      public string VolumeLabel
      {
         get { return (string)GetDeviceInfo(0, 2); }

         set { Volume.SetVolumeLabel(_name, value); }
      }

      #endregion // .NET


      /// <summary>[AlphaFS] Returns the <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> instance.</summary>
      public DiskSpaceInfo DiskSpaceInfo
      {
         get
         {
            GetDeviceInfo(3, 0);

            return _dsi;
         }
      }


      /// <summary>[AlphaFS] The MS-DOS device name.</summary>
      public string DosDeviceName
      {
         get { return (string)GetDeviceInfo(1, 0); }
      }


      /// <summary>[AlphaFS] Indicates if this drive is a SUBST.EXE / DefineDosDevice drive mapping.</summary>
      public bool IsDosDeviceSubstitute
      {
         get
         {
            return !Utils.IsNullOrWhiteSpace(DosDeviceName) && DosDeviceName.StartsWith(Path.NonInterpretedPathPrefix, StringComparison.OrdinalIgnoreCase);
         }
      }


      /// <summary>[AlphaFS] Indicates if this drive is a UNC path.</summary>
      public bool IsUnc
      {
         get
         {
            return !IsDosDeviceSubstitute && DriveType == DriveType.Network ||

                   // Handle Host devices with file systems: FAT/FAT32, UDF (CDRom), ...
                   _name.StartsWith(Path.UncPrefix, StringComparison.Ordinal) && DriveType == DriveType.NoRootDirectory && DriveFormat.Equals(DriveType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase);
         }
      }


      /// <summary>[AlphaFS] Determines whether the specified volume name is a defined volume on the current computer.</summary>
      public bool IsVolume
      {
         get { return null != GetDeviceInfo(0, 0); }
      }


      /// <summary>[AlphaFS] Contains information about the physical drive.</summary>
      /// <returns>A <see cref="PhysicalDriveInfo"/> object that contains information of the physical drive.</returns>
      public PhysicalDriveInfo PhysicalDriveInfo
      {
         get { return (PhysicalDriveInfo)GetDeviceInfo(4, 0); }
      }


      /// <summary>[AlphaFS] Contains information about a file system volume.</summary>
      /// <returns>A <see cref="VolumeInfo"/> object that contains file system volume information of the drive.</returns>
      public VolumeInfo VolumeInfo
      {
         get { return (VolumeInfo)GetDeviceInfo(0, 0); }
      }

      #endregion // Properties


      #region Methods

      #region .NET

      /// <summary>Retrieves the drive names of all logical drives on the Computer.</summary>
      /// <returns>An array of type <see cref="Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on the Computer.</returns>
      [SecurityCritical]
      public static DriveInfo[] GetDrives()
      {
         return EnumerateLogicalDrivesCore(false, false).ToArray();
      }


      /// <summary>Returns a drive name as a string.</summary>
      /// <returns>The name of the drive.</returns>
      /// <remarks>This method returns the Name property.</remarks>
      public override string ToString()
      {
         return _name;
      }

      #endregion // .NET


      /// <summary>[AlphaFS] Refreshes the state of the object.</summary>
      public void Refresh()
      {
         _physicalDriveInfo = null;

         _volumeInfo.Refresh();

         _dsi.Refresh();
      }

      #endregion // Methods
   }
}

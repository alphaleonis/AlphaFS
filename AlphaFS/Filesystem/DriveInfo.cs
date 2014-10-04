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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
#if NET35
using System.Security.Permissions;
#endif

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to information on a local or remote drive.</summary>
   /// <remarks>
   /// This class models a drive and provides methods and properties to query for drive information.
   /// Use DriveInfo to determine what drives are available, and what type of drives they are.
   /// You can also query to determine the capacity and available free space on the drive.
   /// </remarks>
   [SerializableAttribute]
   [SecurityCritical]
   public sealed class DriveInfo : ISerializable
   {
      //#region Class Internal Affairs

      //#region .NET
      
      ////// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      ///// <param name="obj">Another object to compare to.</param>
      ///// <returns><c>true</c> if the specified Object is equal to the current Object; <c>false</c> otherwise.</returns>
      //public override bool Equals(object obj)
      //{
      //   if (obj == null || GetType() != obj.GetType())
      //      return false;

      //   DriveInfo other = obj as DriveInfo;

      //   return other != null && (other.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) &&
      //                            other.RootDirectory.Name.Equals(RootDirectory.Name, StringComparison.OrdinalIgnoreCase));
      //}

      ///// <summary>Serves as a hash function for a particular type.</summary>
      ///// <returns>A hash code for the current Object.</returns>
      //public override int GetHashCode()
      //{
      //   unchecked
      //   {
      //      int hash = Primes[_random];

      //      if (!Utils.IsNullOrWhiteSpace(Name))
      //         hash = hash * Primes[1] + Name.GetHashCode();

      //      if (RootDirectory != null)
      //         if (!Utils.IsNullOrWhiteSpace(RootDirectory.Name))
      //            hash = hash * Primes[1] + RootDirectory.Name.GetHashCode();

      //      if (!Utils.IsNullOrWhiteSpace(DosDeviceName))
      //         hash = hash * Primes[1] + DosDeviceName.GetHashCode();

      //      if (!Utils.IsNullOrWhiteSpace(DriveFormat))
      //         hash = hash * Primes[1] + DriveFormat.GetHashCode();

      //      return hash;
      //      //hash = hash * Primes[1] + ClusterSize.GetHashCode();
      //      //return hash * Primes[1] + SectorsPerCluster.GetHashCode();
      //   }
      //}

      /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object with the data needed to serialize the target object.</summary>
      /// <param name="info"></param>
      /// <param name="context"></param>
      [SecurityCritical]    
#if NET35
      [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
#endif
      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         if (info != null)
            info.AddValue("_name", _name, typeof (string));
      }

      ///// <summary>Implements the operator ==</summary>
      ///// <param name="left">A.</param>
      ///// <param name="right">B.</param>
      ///// <returns>The result of the operator.</returns>
      //public static bool operator ==(DriveInfo left, DriveInfo right)
      //{
      //   return ReferenceEquals(left, null) && ReferenceEquals(right, null) ||
      //          !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      //}

      ///// <summary>Implements the operator !=</summary>
      ///// <param name="left">A.</param>
      ///// <param name="right">B.</param>
      ///// <returns>The result of the operator.</returns>
      //public static bool operator !=(DriveInfo left, DriveInfo right)
      //{
      //   return !(left == right);
      //}

      //#endregion // .NET

      //#region AlphaFS

      //// A random prime number will be picked and added to the HasCode, each time an instance is created.
      //[NonSerialized] private static readonly int[] Primes = { 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919 };
      //[NonSerialized] private readonly int _random = new Random().Next(0, 19);

      //#endregion // AlphaFS

      //#endregion // Class Internal Affairs

      #region Constructors

      #region .NET

      /// <summary>Provides access to information on the specified drive.</summary>
      /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z' or a network share in the format: \\server\share</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public DriveInfo(string driveName)
      {
         if (Utils.IsNullOrWhiteSpace(driveName))
            throw new ArgumentNullException("driveName");

         if (driveName.Length == 1)
            _name += Path.VolumeSeparatorChar;
         else
            _name = Path.GetPathRoot(driveName, false);

         if (Utils.IsNullOrWhiteSpace(_name))
            throw new ArgumentException("Argument must be a drive letter (\"C\"), RootDir (\"C:\\\") or UNC path (\"\\\\server\\share\")");

         // If an exception is thrown, the original drivePath is used.
         _name = Path.AddDirectorySeparator(_name, false);


         // Initiate VolumeInfo() lazyload instance.
         _volumeInfo = new VolumeInfo(_name, false, true);

         // Initiate DiskSpaceInfo() lazyload instance.
         _dsi = new DiskSpaceInfo(_name, null, false, true);
      }

      [SecurityCritical]
      private DriveInfo(SerializationInfo info, StreamingContext context)
      {
         _name = (string)info.GetValue("_name", typeof(string));
      }

      #endregion // .NET

      #endregion // Constructors
      
      #region Methods

      #region  .NET

      #region GetDrives()

      #region  .NET

      /// <summary>Retrieves the drive names of all logical drives on a computer.</summary>
      /// <returns>An array of type <see cref="T:Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on a computer.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static DriveInfo[] GetDrives()
      {
         return Directory.EnumerateLogicalDrivesInternal(false, false).ToArray();
      }

      #endregion // .NET
      
      #endregion // GetDrives()

      #region ToString

      /// <summary>Returns a drive name as a string.</summary>
      /// <returns>The name of the drive.</returns>
      /// <remarks>This method returns the Name property.</remarks>
      public override string ToString()
      {
         return _name;
      }

      #endregion // ToString

      #endregion // .NET

      #region AlphaFS

      #region EnumerateDrives

      /// <summary>[AlphaFS] Enumerates the drive names of all logical drives on a computer.</summary>
      /// <returns>An IEnumerable of type <see cref="T:Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on a computer.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static IEnumerable<DriveInfo> EnumerateDrives(bool fromEnvironment, bool isReady)
      {
         return Directory.EnumerateLogicalDrivesInternal(fromEnvironment, isReady);
      }

      #endregion // EnumerateDrives

      #region GetFreeDriveLetter

      /// <summary>[AlphaFS] Gets the first available drive letter on the local system.</summary>
      /// <returns>A drive letter as <see cref="T:char"/>. When no drive letters are available, an exception is thrown.</returns>
      /// <remarks>The letters "A" and "B" are reserved for floppy drives and will never be returned by this function.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      public static char GetFreeDriveLetter()
      {
         return GetFreeDriveLetter(false);
      }

      /// <summary>Gets an available drive letter on the local system.</summary>
      /// <param name="getLastAvailable">When <c>true</c> get the last available drive letter. When <c>false</c> gets the first available drive letter.</param>
      /// <returns>A drive letter as <see cref="T:char"/>. When no drive letters are available, an exception is thrown.</returns>
      /// <remarks>The letters "A" and "B" are reserved for floppy drives and will never be returned by this function.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
      public static char GetFreeDriveLetter(bool getLastAvailable)
      {
         IEnumerable<char> freeDriveLetters = "CDEFGHIJKLMNOPQRSTUVWXYZ".Except(Directory.EnumerateLogicalDrivesInternal(false, false).Select(d => d.Name[0]));

         try
         {
            return getLastAvailable ? freeDriveLetters.Last() : freeDriveLetters.First();
         }
         catch
         {
            throw new Exception("There are no drive letters available.");
         }
      }

      #endregion // GetFreeDriveLetter

      #region Private

      #region GetDeviceInfo

      [NonSerialized] private readonly VolumeInfo _volumeInfo;
      [NonSerialized] private readonly DiskSpaceInfo _dsi;
      [NonSerialized] private bool _initDsie;
      [NonSerialized] private DriveType? _driveType;
      [NonSerialized] private string _dosDeviceName;
      [NonSerialized] private DirectoryInfo _rootDirectory;
      

      /// <summary>Retrieves information about the file system and volume associated with the specified root file or directorystream.</summary>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private object GetDeviceInfo(int type, int mode)
      {
         try
         {
            switch (type)
            {
               #region Volume

               // VolumeInfo properties.
               case 0:
                  if (Utils.IsNullOrWhiteSpace(_volumeInfo.FullPath))
                     _volumeInfo.Refresh();

                  switch (mode)
                  {
                     case 0:
                        // IsVolume, VolumeInfo
                        return _volumeInfo;
                        
                     case 1:
                        // DriveFormat
                        return _volumeInfo == null ? DriveType.Unknown.ToString() : _volumeInfo.FileSystemName ?? DriveType.Unknown.ToString();

                     case 2:
                        // VolumeLabel
                        return _volumeInfo == null ? string.Empty : _volumeInfo.Name ?? string.Empty;
                  }
                  break;

               // Volume related.
               case 1:
                  switch (mode)
                  {
                     case 0:
                        // DosDeviceName

                        // Do not use ?? expression here.
                        if (_dosDeviceName == null)
                           _dosDeviceName = Volume.QueryDosDevice(Name).FirstOrDefault();
                        return _dosDeviceName;
                  }
                  break;

               #endregion // Volume

               #region Drive

               // Drive related.
               case 2:
                  switch (mode)
                  {
                     case 0:
                        // DriveType
                        // Do not use ?? expression here.
                        if (_driveType == null)
                           _driveType = Volume.GetDriveType(Name);
                        return _driveType;

                     case 1:
                        // RootDirectory

                        // Do not use ?? expression here.
                        if (_rootDirectory == null)
                           _rootDirectory = new DirectoryInfo(null, Name, false);
                        return _rootDirectory;
                  }
                  break;

               // DiskSpaceInfo related.
               case 3:
                  switch (mode)
                  {
                     case 0:
                        // AvailableFreeSpace, TotalFreeSpace, TotalSize, DiskSpaceInfo
                        if (!_initDsie)
                        {
                           _dsi.Refresh();
                           _initDsie = true;
                        }
                        break;
                  }
                  break;

               #endregion // Drive
            }
         }
         catch
         {
         }

         return type == 0 && mode > 0 ? string.Empty : null;
      }

      #endregion // GetDeviceInfo

      #endregion // Private

      #endregion //AlphaFS

      #endregion // Methods

      #region Properties

      #region .NET

      #region AvailableFreeSpace

      /// <summary>Indicates the amount of available free space on a drive.</summary>
      /// <returns>The amount of free space available on the drive, in bytes.</returns>
      /// <remarks>This property indicates the amount of free space available on the drive. Note that this number may be different from the <see cref="T:TotalFreeSpace"/> number because this property takes into account disk quotas.</remarks>
      public long AvailableFreeSpace
      {
         get
         {
            GetDeviceInfo(3, 0);
            return (long) (_dsi == null ? 0 : _dsi.FreeBytesAvailable);
         }
      }

      #endregion // AvailableFreeSpace

      #region DriveFormat

      /// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
      /// <remarks>Use DriveFormat to determine what formatting a drive uses.</remarks>
      public string DriveFormat
      {
         get { return (string) GetDeviceInfo(0, 1); }
      }

      #endregion // DriveFormat

      #region DriveType

      /// <summary>Gets the drive type.</summary>
      /// <returns>One of the <see cref="T:System.IO.DriveType"/> values.</returns>
      /// <remarks>
      /// The DriveType property indicates whether a drive is any of: CDRom, Fixed, Unknown, Network, NoRootDirectory,
      /// Ram, Removable, or Unknown. Values are listed in the <see cref="T:System.IO.DriveType"/> enumeration.
      /// </remarks>
      public DriveType DriveType
      {
         get { return (DriveType) GetDeviceInfo(2, 0); }
      }

      #endregion // DriveType

      #region IsReady

      /// <summary>Gets a value indicating whether a drive is ready.</summary>
      /// <returns><c>true</c> if the drive is ready; <c>false</c> otherwise.</returns>
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
         get { return File.ExistsInternal(true, null, Name, false); }
      }

      #endregion // IsReady

      #region Name

      private readonly string _name;

      /// <summary>Gets the name of the drive.</summary>
      /// <returns>The name of the drive.</returns>
      /// <remarks>This property is the name assigned to the drive, such as C:\ or E:\</remarks>
      public string Name
      {
         get { return _name; }
      }

      #endregion // Name

      #region RootDirectory

      /// <summary>Gets the root directory of a drive.</summary>
      /// <returns>A DirectoryInfo object that contains the root directory of the drive.</returns>
      public DirectoryInfo RootDirectory
      {
         get { return (DirectoryInfo) GetDeviceInfo(2, 1); }
      }

      #endregion // RootDirectory

      #region TotalFreeSpace

      /// <summary>Gets the total amount of free space available on a drive.</summary>
      /// <returns>The total free space available on a drive, in bytes.</returns>
      /// <remarks>This property indicates the total amount of free space available on the drive, not just what is available to the current user.</remarks>
      public long TotalFreeSpace
      {
         get
         {
            GetDeviceInfo(3, 0);
            return (long) (_dsi == null ? 0 : _dsi.TotalNumberOfFreeBytes);
         }
      }

      #endregion // TotalFreeSpace

      #region TotalSize

      /// <summary>Gets the total size of storage space on a drive.</summary>
      /// <returns>The total size of the drive, in bytes.</returns>
      /// <remarks>This property indicates the total size of the drive in bytes, not just what is available to the current user.</remarks>
      public long TotalSize
      {
         get
         {
            GetDeviceInfo(3, 0);
            return (long) (_dsi == null ? 0 : _dsi.TotalNumberOfBytes);
         }
      }

      #endregion // TotalSize

      #region VolumeLabel

      /// <summary>Gets or sets the volume label of a drive.</summary>
      /// <returns>The volume label.</returns>
      /// <remarks>
      /// The label length is determined by the operating system. For example, NTFS allows a volume label
      /// to be up to 32 characters long. Note that <c>null</c> is a valid VolumeLabel.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      public string VolumeLabel
      {
         get { return (string) GetDeviceInfo(0, 2); }
         set { Volume.SetVolumeLabel(Name, value); }
      }

      #endregion // VolumeLabel

      #endregion // .NET

      #region AlphaFS

      #region DiskSpaceInfo

      /// <summary>[AlphaFS] Returns the <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> instance.</summary>
      public DiskSpaceInfo DiskSpaceInfo
      {
         get
         {
            GetDeviceInfo(3, 0);
            return _dsi;
         }
      }

      #endregion // ClusterSize

      #region DosDeviceName

      /// <summary>[AlphaFS] The MS-DOS device name.</summary>
      public string DosDeviceName
      {
         get { return (string)GetDeviceInfo(1, 0); }
      }

      #endregion // DosDeviceName

      #region IsDosDeviceSubstitute

      /// <summary>[AlphaFS] Indicates if this drive is a SUBST.EXE / DefineDosDevice drive mapping.</summary>
      public bool IsDosDeviceSubstitute
      {
         get { return !Utils.IsNullOrWhiteSpace(DosDeviceName) && DosDeviceName.StartsWith(Path.SubstitutePrefix, StringComparison.OrdinalIgnoreCase); }
      }

      #endregion // IsDosDeviceSubstitute

      #region IsUnc

      /// <summary>[AlphaFS] Indicates if this drive is a UNC path.</summary>
      /// <remarks>Only retrieve this information if we're dealing with a real network share mapping: http://alphafs.codeplex.com/discussions/316583</remarks>
      public bool IsUnc
      {
         get { return !IsDosDeviceSubstitute && DriveType == DriveType.Network; }
      }

      #endregion // IsUnc

      #region IsVolume

      /// <summary>[AlphaFS] Determines whether the specified volume name is a defined volume on the current computer.</summary>
      public bool IsVolume
      {
         get { return GetDeviceInfo(0, 0) != null; }
      }

      #endregion // IsVolume

      #region VolumeInfo

      /// <summary>[AlphaFS] Contains information about a file-system volume.</summary>
      /// <returns>A VolumeInfo object that contains file-system volume information of the drive.</returns>
      public VolumeInfo VolumeInfo
      {
         get { return (VolumeInfo) GetDeviceInfo(0, 0); }
      }

      #endregion // VolumeInfo
      
      #endregion // AlphaFS

      #endregion // Properties
   }
}
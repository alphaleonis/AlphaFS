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

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Static class providing utility methods for working with Microsoft Windows devices and volumes.</summary>
   public static class Volume
   {
      #region DosDevice

      #region DefineDosDevice

      /// <summary>Defines, redefines, or deletes MS-DOS device names.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device the function is defining, redefining, or deleting.</param>
      /// <param name="targetPath">An MS-DOS path that will implement this device.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void DefineDosDevice(string deviceName, string targetPath)
      {
         DefineDosDeviceInternal(true, deviceName, targetPath, DosDeviceAttributes.None, false);
      }

      /// <summary>Defines, redefines, or deletes MS-DOS device names.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device the function is defining, redefining, or deleting.</param>
      /// <param name="targetPath">>An MS-DOS path that will implement this device. If <paramref name="deviceAttributes"/> parameter has the <see cref="T:DosDeviceAttributes.RawTargetPath"/> flag specified, <paramref name="targetPath"/> is used as is.</param>
      /// <param name="deviceAttributes">The controllable aspects of the DefineDosDevice function, <see cref="T:DosDeviceAttributes"/> flags which will be combined with the default.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void DefineDosDevice(string deviceName, string targetPath, DosDeviceAttributes deviceAttributes)
      {
         DefineDosDeviceInternal(true, deviceName, targetPath, deviceAttributes, false);
      }

      #endregion // DefineDosDevice

      #region DeleteDosDevice

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name specifying the device to delete.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName)
      {
         DefineDosDeviceInternal(false, deviceName, null, DosDeviceAttributes.RemoveDefinition, false);
      }

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device to delete.</param>
      /// <param name="targetPath">A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the <see cref="T:DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName, string targetPath)
      {
         DefineDosDeviceInternal(false, deviceName, targetPath, DosDeviceAttributes.RemoveDefinition, false);
      }

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device to delete.</param>
      /// <param name="targetPath">A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the <see cref="T:DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.</param>
      /// <param name="exactMatch">Only delete MS-DOS device on an exact name match. If <paramref name="exactMatch"/> is <c>true</c>, <paramref name="targetPath"/> must be the same path used to create the mapping.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName, string targetPath, bool exactMatch)
      {
         DefineDosDeviceInternal(false, deviceName, targetPath, DosDeviceAttributes.RemoveDefinition, exactMatch);
      }

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device to delete.</param>
      /// <param name="targetPath">A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the <see cref="T:DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.</param>
      /// <param name="exactMatch">Only delete MS-DOS device on an exact name match. If <paramref name="exactMatch"/> is <c>true</c>, <paramref name="targetPath"/> must be the same path used to create the mapping.</param>
      /// <param name="deviceAttributes">The controllable aspects of the DefineDosDevice function <see cref="T:DosDeviceAttributes"/> flags which will be combined with the default.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName, string targetPath, DosDeviceAttributes deviceAttributes, bool exactMatch)
      {
         DefineDosDeviceInternal(false, deviceName, targetPath, deviceAttributes, exactMatch);
      }

      #endregion // DeleteDosDevice
      
      #region QueryAllDosDevices

      /// <summary>Retrieves a list of all existing MS-DOS device names.</summary>
      /// <returns>An <see cref="T:IEnumerable{String}"/> with one or more existing MS-DOS device names.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static IEnumerable<string> QueryAllDosDevices()
      {
         return QueryDosDevice(null, null);
      }

      /// <summary>Retrieves a list of all existing MS-DOS device names.</summary>
      /// <param name="deviceName">
      /// (Optional, default: <c>null</c>) An MS-DOS device name string specifying the target of the query.
      /// This parameter can be "sort". In that case a sorted list of all existing MS-DOS device names is returned.
      /// This parameter can be <c>null</c>. In that case, the <see cref="T:QueryDosDevice"/> function will store a list of all
      /// existing MS-DOS device names into the buffer.
      ///</param>
      /// <returns>An <see cref="T:IEnumerable{String}"/> with or more existing MS-DOS device names.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static IEnumerable<string> QueryAllDosDevices(string deviceName)
      {
         return QueryDosDevice(null, deviceName);
      }

      #endregion // QueryAllDosDevices

      #region QueryDosDevice

      /// <summary>Retrieves information about MS-DOS device names. The function can obtain the current mapping for a
      /// particular MS-DOS device name. The function can also obtain a list of all existing MS-DOS device names.
      /// </summary>
      /// <param name="deviceName">
      /// An MS-DOS device name string, or part of, specifying the target of the query.
      /// This parameter can be <c>null</c>. In that case, the QueryDosDevice function will store a list of all
      /// existing MS-DOS device names into the buffer.
      ///</param>
      /// <param name="options">(Optional, default: <c>false</c>) If options[0] = <c>true</c> a sorted list will be returned.</param>
      /// <returns>An <see cref="T:IEnumerable{String}"/> with one or more existing MS-DOS device names.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static IEnumerable<string> QueryDosDevice(string deviceName, params string[] options)
      {
         // The device name cannot have a trailing backslash.
         deviceName = Path.RemoveDirectorySeparator(deviceName, false);
         bool searchFilter = false;

         // Only process options if a device is supplied.
         if (deviceName != null)
         {
            // Check that at least one "options[]" has something to say. If so, rebuild them.
            options = options != null && options.Any() ? new[] { deviceName, options[0] } : new[] { deviceName, string.Empty };

            searchFilter = !Path.IsLocalPath(deviceName, true);

            if (searchFilter)
               deviceName = null;
         }

         // Choose sorted output.
         bool doSort = options != null &&
                       options.Any(s => s != null && s.Equals("sort", StringComparison.OrdinalIgnoreCase));

         // Start with a larger buffer when using a searchFilter.
         uint bufferSize = (uint)(searchFilter || doSort || (deviceName == null && options == null) ? 32768 : 256);
         uint bufferResult = 0;

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (bufferResult == 0)
            {
               char[] cBuffer = new char[bufferSize];

               // QueryDosDevice()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2014-01-29: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               bufferResult = NativeMethods.QueryDosDevice(deviceName, cBuffer, bufferSize);
               int lastError = Marshal.GetLastWin32Error();

               if (bufferResult == 0)
                  switch ((uint) lastError)
                  {
                     case Win32Errors.ERROR_MORE_DATA:
                     case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                        bufferSize *= 2;
                        continue;

                     default:
                        NativeError.ThrowException(lastError, deviceName);
                        break;
                  }

               List<string> dosDev = new List<string>();
               StringBuilder buffer = new StringBuilder();

               for (int i = 0; i < bufferResult; i++)
               {
                  if (cBuffer[i] != Path.StringTerminatorChar)
                     buffer.Append(cBuffer[i]);

                  else if (buffer.Length > 0)
                  {
                     dosDev.Add(buffer.ToString());
                     buffer.Length = 0;
                  }
               }

               // Choose the yield back query; filtered or list.
               IEnumerable<string> selectQuery = (searchFilter)
                  ? dosDev.Where(dev => options != null && dev.StartsWith(options[0], StringComparison.OrdinalIgnoreCase))
                  : dosDev;

               foreach (string dev in (doSort) ? selectQuery.OrderBy(n => n) : selectQuery)
                  yield return dev;
            }
      }

      #endregion // QueryDosDevice

      #endregion // DosDevice

      #region Drive
      
      #region GetDriveFormat

      /// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
      /// <param name="drivePath">A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <returns>The name of the file system on the specified drive or <c>null</c>  on failure or if not available.</returns>
      /// <remarks>Use DriveFormat to determine what formatting a drive uses.</remarks>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetDriveFormat(string drivePath)
      {
         string fsName = new VolumeInfo(drivePath, true, true).FileSystemName;
         return Utils.IsNullOrWhiteSpace(fsName) ? null : fsName;
      }

      #endregion // GetDriveFormat

      #region GetDriveNameForNtDeviceName

      /// <summary>Gets the drive letter from an MS-DOS device name. For example: "\Device\HarddiskVolume2" returns "C:\"</summary>
      /// <param name="deviceName">An MS-DOS device name.</param>
      /// <returns>The drive letter from an MS-DOS device name.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Nt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nt")]
      public static string GetDriveNameForNtDeviceName(string deviceName)
      {
         return (from drive in Directory.EnumerateLogicalDrivesInternal(false, false)
            where drive.DosDeviceName.Equals(deviceName, StringComparison.OrdinalIgnoreCase)
            select drive.Name).FirstOrDefault();
      }
      
      #endregion // GetDriveNameForNtDeviceName

      #region GetDriveType

      /// <summary>Determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.</summary>
      /// <param name="drivePath">A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <returns>A <see cref="T:System.IO.DriveType"/> object.</returns>
      [SecurityCritical]
      public static DriveType GetDriveType(string drivePath)
      {
         // drivePath is allowed to be == null.

         drivePath = Path.AddDirectorySeparator(drivePath, false);

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            return NativeMethods.GetDriveType(drivePath);
      }

      #endregion // GetDriveType

      #region GetDiskFreeSpace

      /// <summary>Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total amount of free space, and the total amount of free space available to the user that is associated with the calling thread.</summary>
      /// <param name="drivePath">A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <param name="spaceInfoType"><c>null</c> gets both size- and disk cluster information. <c>true</c> Get only disk cluster information, <c>false</c> Get only size information.</param>
      /// <returns>A <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> class instance.</returns>
      /// <remarks>The calling application must have FILE_LIST_DIRECTORY access rights for this directory.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static DiskSpaceInfo GetDiskFreeSpace(string drivePath, bool? spaceInfoType)
      {
         return new DiskSpaceInfo(drivePath, spaceInfoType, true, true);
      }

      #endregion // GetDiskFreeSpace
      
      #region IsReady

      /// <summary>Gets a value indicating whether a drive is ready.</summary>
      /// <param name="drivePath">A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <returns><c>true</c> if <paramref name="drivePath"/> is ready; <c>false</c> otherwise.</returns>
      [SecurityCritical]
      public static bool IsReady(string drivePath)
      {
         return FileSystemInfo.ExistsInternal(true, null, drivePath, false);
      }

      #endregion // IsReady

      #endregion // Drive

      #region Volume

      #region DeleteVolumeLabel

      /// <summary>Deletes the label of a file system volume.</summary>
      /// <param name="rootPathName">The root directory of a file system volume. This is the volume the function will remove the label.</param>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static void DeleteVolumeLabel(string rootPathName)
      {
         if (Utils.IsNullOrWhiteSpace(rootPathName))
            throw new ArgumentNullException("rootPathName");

         SetVolumeLabel(rootPathName, null);
      }

      #endregion // DeleteVolumeLabel

      #region DeleteVolumeMountPoint

      /// <summary>Deletes a Drive letter or mounted folder.</summary>
      /// <param name="volumeMountPoint">The Drive letter or mounted folder to be deleted. For example, X:\ or Y:\MountX\.</param>
      /// <remarks>Deleting a mounted folder does not cause the underlying directory to be deleted.</remarks>
      /// <remarks>If the <paramref name="volumeMountPoint"/> parameter is a directory that is not a mounted folder, the function does nothing. The directory is not deleted.</remarks>
      /// <remarks>It's not an error to attempt to unmount a volume from a volume mount point when there is no volume actually mounted at that volume mount point.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void DeleteVolumeMountPoint(string volumeMountPoint)
      {
         DeleteVolumeMountPointInternal(volumeMountPoint, false);
      }
      
      #endregion // DeleteVolumeMountPoint
      
      #region EnumerateVolumeMountPoints

      /// <summary>Returns an enumerable collection of <see cref="T:String"/> of all mounted folders (volume mount points) on the specified volume.</summary>
      /// <param name="volumeGuid">A <see cref="T:string"/> containing the volume <see cref="T:Guid"/>.</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> of all volume mount points on the specified volume.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumeMountPoints(string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");

         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Argument_is_not_a_valid_Volume_GUID, volumeGuid);

         // A trailing backslash is required.
         volumeGuid = Path.AddDirectorySeparator(volumeGuid, false);

         StringBuilder buffer = new StringBuilder(NativeMethods.MaxPathUnicode);

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         using (SafeFindVolumeMountPointHandle handle = NativeMethods.FindFirstVolumeMountPoint(volumeGuid, buffer, (uint)buffer.Capacity))
         {
            int lastError;
            if (handle.IsInvalid)
            {
               lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_NO_MORE_FILES:
                  case Win32Errors.ERROR_PATH_NOT_FOUND: // Observed with USB stick, FAT32 formatted.
                     yield break;

                  default:
                     NativeError.ThrowException(lastError, volumeGuid);
                     break;
               }
            }

            yield return buffer.ToString();


            while (NativeMethods.FindNextVolumeMountPoint(handle, buffer, (uint)buffer.Capacity))
            {
               if (handle.IsInvalid)
               {
                  lastError = Marshal.GetLastWin32Error();
                  switch ((uint) lastError)
                  {
                     case Win32Errors.ERROR_NO_MORE_FILES:
                     case Win32Errors.ERROR_PATH_NOT_FOUND: // Observed with USB stick, FAT32 formatted.
                     case Win32Errors.ERROR_MORE_DATA:
                        yield break;

                     default:
                        NativeError.ThrowException(lastError, volumeGuid);
                        break;
                  }
               }

               yield return buffer.ToString();
            }
         }
      }

      #endregion // EnumerateVolumeMountPoints

      #region EnumerateVolumePathNames

      /// <summary>Returns an enumerable collection of <see cref="T:String"/> drive letters and mounted folder paths for the specified volume.</summary>
      /// <param name="volumeGuid">A volume <see cref="T:Guid"/> path: \\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> containing the path names for the specified volume.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumePathNames(string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");

         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Argument_is_not_a_valid_Volume_GUID, volumeGuid);

         string volName = Path.AddDirectorySeparator(volumeGuid, false);

         uint requiredLength = 10;
         char[] cBuffer = new char[requiredLength];

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (!NativeMethods.GetVolumePathNamesForVolumeName(volName, cBuffer, (uint)cBuffer.Length, out requiredLength))
            {
               int lastError = Marshal.GetLastWin32Error();

               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                  case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                     cBuffer = new char[requiredLength];
                     break;

                  default:
                     NativeError.ThrowException(lastError, volumeGuid);
                     break;
               }
            }

         StringBuilder buffer = new StringBuilder(cBuffer.Length);
         foreach (char c in cBuffer)
         {
            if (c != Path.StringTerminatorChar)
               buffer.Append(c);
            else
            {
               if (buffer.Length > 0)
               {
                  yield return buffer.ToString();
                  buffer.Length = 0;
               }
            }
         }
      }

      #endregion // EnumerateVolumePathNames

      #region EnumerateVolumes

      /// <summary>Returns an enumerable collection of <see cref="T:String"/> volumes on the computer.</summary>
      /// <returns>An enumerable collection of <see cref="T:String"/> volume names on the computer.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumes()
      {
         StringBuilder buffer = new StringBuilder(NativeMethods.MaxPathUnicode);

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         using (SafeFindVolumeHandle handle = NativeMethods.FindFirstVolume(buffer, (uint)buffer.Capacity))
         {
            while (!handle.IsInvalid)
            {
               if (NativeMethods.FindNextVolume(handle, buffer, (uint)buffer.Capacity))
                  yield return buffer.ToString();

               else
               {
                  int lastError = Marshal.GetLastWin32Error();
                  if (lastError == Win32Errors.ERROR_NO_MORE_FILES)
                     yield break;

                  NativeError.ThrowException(lastError);
               }
            }
         }
      }

      #endregion // EnumerateVolumes
      
      #region GetUniqueVolumeNameForPath

      /// <summary>Get the unique volume name for the given path.</summary>
      /// <param name="volumePathName">A path string. Both absolute and relative file and directory names,
      /// for example "..", is acceptable in this path.
      /// If you specify a relative file or directory name without a volume qualifier, GetUniqueVolumeNameForPath returns the Drive letter of the current volume.
      ///</param>
      /// <returns>The unique volume name of the form: "\\?\Volume{GUID}\", or <c>null</c> on error or if unavailable.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetUniqueVolumeNameForPath(string volumePathName)
      {
         if (Utils.IsNullOrWhiteSpace(volumePathName))
            throw new ArgumentNullException("volumePathName");

         try
         {
            return GetVolumeGuid(GetVolumePathName(volumePathName));
         }
         catch
         {
            return null;
         }
      }

      #endregion // GetUniqueVolumeNameForPath

      #region GetVolumeDeviceName

      /// <summary>Retrieves the Win32 Device name from the Volume name.</summary>
      /// <param name="volumeName">Name of the Volume</param>
      /// <returns>The Win32 Device name from the Volume name (for example: "\Device\HarddiskVolume2"), or <c>null</c> on error or if unavailable.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeDeviceName(string volumeName)
      {
         if (Utils.IsNullOrWhiteSpace(volumeName))
            throw new ArgumentNullException("volumeName");

         volumeName = Path.RemoveDirectorySeparator(volumeName, false);

         #region GlobalRoot

         if (volumeName.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
            return volumeName.Substring(Path.GlobalRootPrefix.Length);

         #endregion // GlobalRoot

         bool doQueryDos = false;

         #region Volume

         if (volumeName.StartsWith(Path.VolumePrefix, StringComparison.OrdinalIgnoreCase))
         {
            // Isolate the DOS Device from the Volume name, in the format: Volume{GUID}
            volumeName = volumeName.Substring(Path.LongPathPrefix.Length);
            doQueryDos = true;
         }

         #endregion // Volume

         #region Logical Drive

         // Check for Logical Drives: C:, D:, ...
         else if (Path.IsLocalPath(volumeName, true))
            doQueryDos = true;

         #endregion // Logical Drive

         if (doQueryDos)
         {
            try
            {
               // Get the real Device underneath.
               string dev = QueryDosDevice(volumeName).FirstOrDefault();
               return !Utils.IsNullOrWhiteSpace(dev) ? dev : null;
            }
            catch
            {
            }
         }

         return null;
      }

      #endregion // GetVolumeDeviceName

      #region GetVolumeDisplayName

      /// <summary>Gets the shortest display name for the specified <paramref name="volumeName"/>.</summary>
      /// <param name="volumeName">A volume <see cref="T:Guid"/> path: \\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</param>
      /// <returns>The shortest display name for the specified volume found, or <c>null</c> if no display names were found.</returns>
      /// <remarks>This method basically returns the shortest string returned by <see cref="T:EnumerateVolumePathNames"/></remarks>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeDisplayName(string volumeName)
      {
         string[] smallestMountPoint = { new string(Path.WildcardStarMatchAllChar, NativeMethods.MaxPathUnicode) };

         try
         {
            foreach (string m in EnumerateVolumePathNames(volumeName).Where(m => !Utils.IsNullOrWhiteSpace(m) && m.Length < smallestMountPoint[0].Length))
               smallestMountPoint[0] = m;
         }
         catch
         {
         }

         string result = smallestMountPoint[0][0] == Path.WildcardStarMatchAllChar ? null : smallestMountPoint[0];
         return Utils.IsNullOrWhiteSpace(result) ? null : result;
      }

      #endregion // GetVolumeDisplayName

      #region GetVolumeGuid

      /// <summary>Retrieves a volume <see cref="T:Guid"/> path for the volume that is associated with the specified volume mount point (drive letter, volume GUID path, or mounted folder).</summary>
      /// <param name="volumeMountPoint">The path of a mounted folder (for example, "Y:\MountX\") or a drive letter (for example, "X:\").</param>
      /// <returns>The unique volume name of the form: "\\?\Volume{GUID}\"</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "Marshal.GetLastWin32Error() is manipulated.")]
      [SecurityCritical]
      public static string GetVolumeGuid(string volumeMountPoint)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");

         // The string must end with a trailing backslash ('\').
         volumeMountPoint = Path.GetFullPathInternal(null, volumeMountPoint, true, false, true, false);

         StringBuilder volumeGuid = new StringBuilder(100);
         StringBuilder uniqueName = new StringBuilder(100);

         try
         {
            // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               // GetVolumeNameForVolumeMountPoint()
               // In the ANSI version of this function, the name is limited to 248 characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               return NativeMethods.GetVolumeNameForVolumeMountPoint(volumeMountPoint, volumeGuid, (uint) volumeGuid.Capacity)
                  ? NativeMethods.GetVolumeNameForVolumeMountPoint(Path.AddDirectorySeparator(volumeGuid.ToString(), false), uniqueName, (uint) uniqueName.Capacity)
                     ? uniqueName.ToString()
                     : null
                  : null;
            }
         }
         finally
         {
            int lastError = Marshal.GetLastWin32Error();

            // (1) When GetVolumeNameForVolumeMountPoint() succeeds,             lastError is set to Win32Errors.ERROR_MORE_DATA.
            // (2) When volumeMountPoint is a network drive mapping or UNC path, lastError is set to Win32Errors.ERROR_INVALID_PARAMETER.

            if (lastError != Win32Errors.ERROR_MORE_DATA && lastError != Win32Errors.ERROR_INVALID_PARAMETER)
               NativeError.ThrowException(lastError, volumeMountPoint);
         }
      }

      #endregion // GetVolumeGuid

      #region GetVolumeGuidForNtDeviceName

      /// <summary>Tranlates DosDevicePath to a Volume GUID.
      /// For example: "\Device\HarddiskVolumeX\path\filename.ext" can translate to: "\path\filename.ext" or: "\\?\Volume{GUID}\path\filename.ext".
      /// </summary>
      /// <param name="dosDevice">A DosDevicePath, for example: \Device\HarddiskVolumeX\path\filename.ext</param>
      /// <returns>A translated dos path.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Nt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nt")]
      public static string GetVolumeGuidForNtDeviceName(string dosDevice)
      {
         return (from drive in Directory.EnumerateLogicalDrivesInternal(false, false)
                 where drive.DosDeviceName.Equals(dosDevice, StringComparison.OrdinalIgnoreCase)
                 select drive.VolumeInfo.Guid).FirstOrDefault();
      }

      #endregion // GetVolumeGuidForNtDeviceName

      #region GetVolumeInformation

      /// <summary>Retrieves information about the file system and volume associated with the specified root file or directorystream.</summary>
      /// <param name="volumePath">A path that contains the root directory.</param>
      /// <returns>A <see cref="T:VolumeInfo"/> instance describing the volume associatied with the specified root directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static VolumeInfo GetVolumeInformation(string volumePath)
      {
         return new VolumeInfo(volumePath, true, false);
      }

      /// <summary>Retrieves information about the file system and volume associated with the specified root file or directorystream.</summary>
      /// <param name="volumeHandle">An instance to a <see cref="T:SafeFileHandle"/> handle.</param>
      /// <returns>A <see cref="T:VolumeInfo"/> instance describing the volume associatied with the specified root directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static VolumeInfo GetVolumeInformation(SafeFileHandle volumeHandle)
      {
         return new VolumeInfo(volumeHandle, true, true);
      }

      #endregion // GetVolumeInformation

      #region GetVolumeLabel

      /// <summary>Retrieve the label of a file system volume.</summary>
      /// <param name="volumePath">A path to a volume. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <returns>The the label of the file system volume. This function can return <c>string.Empty</c> since a volume label is generally not mandatory.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeLabel(string volumePath)
      {
         return new VolumeInfo(volumePath, true, true).Name;
      }

      #endregion // GetVolumeLabel

      #region GetVolumePathName

      /// <summary>Retrieves the volume mount point where the specified path is mounted.
      /// Returns the nearest volume root path for a given directory.
      /// </summary> 
      /// <param name="path">The path to the volume, for example: C:\Windows</param>
      /// <returns>The volume path name, for example: C:\windows --> C:\, in case of failure <paramref name="path"/> is returned.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetVolumePathName(string path)
      {
         // For the following set of examples, U: is mapped to the remote computer \\YourComputer\C$, and Q is a local drive. 
         // Get the root path of the Volume.
         //    Specified path                      Function returns
         //    \\YourComputer\C$\Windows           \\YourComputer\C$\
         //    \\?\UNC\YourComputer\C$\Windows     \\?\UNC\YourComputer\C$\
         //    Q:\Windows                          Q:\
         //    \\?\Q:\Windows                      \\?\Q:\
         //    \\.\Q:\Windows                      \\.\Q:\
         //    \\?\UNC\W:\Windows                  FALSE with error 123 because a specified remote path was not valid; W$ share does not exist or no user access granted.
         //    C:\COM2 (which exists)              \\.\COM2\
         //    C:\COM3 (non-existent)              FALSE with error 123 because a non-existent COM device was specified.

         // For the following set of examples, the paths contain invalid trailing path elements.
         //    Specified path                                                 Function returns
         //    G:\invalid (invalid path)	                                    G:\
         //    \\.\I:\aaa\invalid (invalid path)	                           \\.\I:\
         //    \\YourComputer\C$\invalid (invalid trailing path element)	   \\YourComputer\C$\

         // If a network share is specified, GetVolumePathName returns the shortest path for which GetDriveType returns DRIVE_REMOTE,
         // which means that the path is validated as a remote drive that exists, which the current user can access.

         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = Path.GetFullPathInternal(null, path, true, false, false, false);
         StringBuilder volumeRootPath = new StringBuilder(NativeMethods.MaxPathUnicode);
         bool getOk;
         int lastError;

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            // GetVolumePathName()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            getOk = NativeMethods.GetVolumePathName(pathLp, volumeRootPath, (uint)volumeRootPath.Capacity);
            lastError = Marshal.GetLastWin32Error();
         }

         if (getOk)
            return Path.GetRegularPathInternal(volumeRootPath.ToString(), false, false, false, false);

         switch ((uint)lastError)
         {
            // Don't throw exception on these errors.
            case Win32Errors.ERROR_NO_MORE_FILES:
            case Win32Errors.ERROR_INVALID_PARAMETER:
            case Win32Errors.ERROR_INVALID_NAME:
               break;

            default:
               NativeError.ThrowException(lastError, path);
               break;
         }

         // Return original path.
         return path;
      }

      #endregion // GetVolumePathName

      #region IsSameVolume

      /// <summary>Determines whether the volume of two file system objects is the same.</summary>
      /// <param name="path1">The first filesystem ojbect with full path information.</param>
      /// <param name="path2">The second file system object with full path information.</param>
      /// <returns><c>true</c> if both filesytem objects reside on the same volume, <c>false</c> otherwise.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static bool IsSameVolume(string path1, string path2)
      {
         try
         {
            VolumeInfo volInfo1 = new VolumeInfo(GetVolumePathName(path1), true, true);
            VolumeInfo volInfo2 = new VolumeInfo(GetVolumePathName(path2), true, true);

            return volInfo1.SerialNumber == volInfo2.SerialNumber;
         }
         catch { }

         return false;
      }

      #endregion // IsSameVolume

      #region IsVolume

      /// <summary>Determines whether the specified volume name is a defined volume on the current computer.</summary>
      /// <param name="volumeMountPoint">A path to a volume. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      [SecurityCritical]
      public static bool IsVolume(string volumeMountPoint)
      {
         return !Utils.IsNullOrWhiteSpace(GetVolumeGuid(volumeMountPoint));
      }

      #endregion // IsVolume

      #region SetVolumeLabel

      /// <summary>Sets the label of a file system volume.</summary>
      /// <param name="volumePath">A path to a volume. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <param name="volumeName">
      /// <para>The new label for the volume, name for the volume.</para>
      /// <para>If this parameter is <c>null</c>, the function deletes any existing label</para>
      /// <para>from the specified volume and does not assign a new label.</para>
      ///</param>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static void SetVolumeLabel(string volumePath, string volumeName)
      {
         // rootPathName == null is allowed, means current drive.

         // Setting volume label only applies to Logical Drives pointing to local resources.
         //if (!Path.IsLocalPath(rootPathName))
         //return false;

         volumePath = Path.AddDirectorySeparator(volumePath, false);

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         // NTFS uses a limit of 32 characters for the volume label as of Windows Server 2003.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            if (!NativeMethods.SetVolumeLabel(volumePath, volumeName))
               NativeError.ThrowException(volumePath, volumeName);
      }

      #endregion // SetVolumeLabel
      
      #region SetVolumeMountPoint

      /// <summary>Associates a volume with a Drive letter or a directory on another volume.</summary>
      /// <param name="volumeMountPoint">
      /// The user-mode path to be associated with the volume. This may be a Drive letter (for example, "X:\")
      /// or a directory on another volume (for example, "Y:\MountX\").
      ///</param>
      /// <param name="volumeGuid">A <see cref="T:string"/> containing the volume <see cref="T:Guid"/>.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
      [SecurityCritical]
      public static void SetVolumeMountPoint(string volumeMountPoint, string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");

         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");

         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Argument_is_not_a_valid_Volume_GUID, volumeGuid);

         volumeMountPoint = Path.GetFullPathInternal(null, volumeMountPoint, true, false, true, false);

         // This string must be of the form "\\?\Volume{GUID}\"
         volumeGuid = Path.AddDirectorySeparator(volumeGuid, false);


         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))

            // SetVolumeMountPoint()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-01-29: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            if (!NativeMethods.SetVolumeMountPoint(volumeMountPoint, volumeGuid))
            {
               int lastError = Marshal.GetLastWin32Error();

               // If the lpszVolumeMountPoint parameter contains a path to a mounted folder,
               // GetLastError returns ERROR_DIR_NOT_EMPTY, even if the directory is empty.

               if (lastError != Win32Errors.ERROR_DIR_NOT_EMPTY)
                  NativeError.ThrowException(lastError, volumeMountPoint, volumeGuid);
            }
      }

      #endregion // SetVolumeMountPoint

      #endregion // Volume

      #region Unified Internals

      /// <summary>Unified method DefineDosDeviceInternal() to define, redefine, or delete MS-DOS device names.</summary>
      /// <param name="isDefine"><c>true</c> defines a new MS-DOS device. <c>false</c> deletes a previously defined MS-DOS device.</param>
      /// <param name="deviceName">An MS-DOS device name string specifying the device the function is defining, redefining, or deleting.</param>
      /// <param name="targetPath">A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the <see cref="T:DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.</param>
      /// <param name="deviceAttributes">The controllable aspects of the DefineDosDevice function, <see cref="T:DosDeviceAttributes"/> flags which will be combined with the default.</param>
      /// <param name="exactMatch">Only delete MS-DOS device on an exact name match. If <paramref name="exactMatch"/> is <c>true</c>, <paramref name="targetPath"/> must be the same path used to create the mapping.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void DefineDosDeviceInternal(bool isDefine, string deviceName, string targetPath, DosDeviceAttributes deviceAttributes, bool exactMatch)
      {
         if (Utils.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentNullException("deviceName");

         if (isDefine)
         {
            // targetPath is allowed to be null.

            // In no case is a trailing backslash ("\") allowed.
            deviceName = Path.GetRegularPathInternal(deviceName, true, false, false, true);

            // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
               if (!NativeMethods.DefineDosDevice(deviceAttributes, deviceName, targetPath))
                  NativeError.ThrowException(deviceName, targetPath);
         }
         else
         {
            // A pointer to a path string that will implement this device.
            // The string is an MS-DOS path string unless the DDD_RAW_TARGET_PATH flag is specified, in which case this string is a path string.

            if (exactMatch && !Utils.IsNullOrWhiteSpace(targetPath))
               deviceAttributes = deviceAttributes | DosDeviceAttributes.ExactMatchOnRemove | DosDeviceAttributes.RawTargetPath;

            // Remove the MS-DOS device name. First, get the name of the Windows NT device
            // from the symbolic link and then delete the symbolic link from the namespace.

            DefineDosDevice(deviceName, targetPath, deviceAttributes);
         }
      }

      /// <summary>Unified method DeleteVolumeMountPointInternal() to delete a Drive letter or mounted folder.</summary>
      /// <param name="volumeMountPoint">The Drive letter or mounted folder to be deleted. For example, X:\ or Y:\MountX\.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <remarks>Deleting a mounted folder does not cause the underlying directory to be deleted.</remarks>
      /// <remarks>It's not an error to attempt to unmount a volume from a volume mount point when there is no volume actually mounted at that volume mount point.</remarks>
      /// <returns>If completed successfully returns <see cref="Win32Errors.ERROR_SUCCESS"/>, otherwise the last error number.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static int DeleteVolumeMountPointInternal(string volumeMountPoint, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");

         int lastError = (int)Win32Errors.ERROR_SUCCESS;

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            // DeleteVolumeMountPoint()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            if (!NativeMethods.DeleteVolumeMountPoint(Path.AddDirectorySeparator(volumeMountPoint, false)))
               lastError = Marshal.GetLastWin32Error();

            if (lastError != Win32Errors.ERROR_SUCCESS && !continueOnException)
            {
               if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND)
                  lastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;

               NativeError.ThrowException(lastError, volumeMountPoint);
            }
         }

         return lastError;
      }

      #endregion Unified Internals
   }
}
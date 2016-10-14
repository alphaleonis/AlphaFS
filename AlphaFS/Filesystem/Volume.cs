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
      
      [SecurityCritical]
      public static void DefineDosDevice(string deviceName, string targetPath)
      {
         DefineDosDeviceCore(true, deviceName, targetPath, DosDeviceAttributes.None, false);
      }

      /// <summary>Defines, redefines, or deletes MS-DOS device names.</summary>
      /// <param name="deviceName">
      ///   An MS-DOS device name string specifying the device the function is defining, redefining, or deleting.
      /// </param>
      /// <param name="targetPath">
      ///   &gt;An MS-DOS path that will implement this device. If <paramref name="deviceAttributes"/> parameter has the
      ///   <see cref="DosDeviceAttributes.RawTargetPath"/> flag specified, <paramref name="targetPath"/> is used as is.
      /// </param>
      /// <param name="deviceAttributes">
      ///   The controllable aspects of the DefineDosDevice function, <see cref="DosDeviceAttributes"/> flags which will be combined with the
      ///   default.
      /// </param>      
      [SecurityCritical]
      public static void DefineDosDevice(string deviceName, string targetPath, DosDeviceAttributes deviceAttributes)
      {
         DefineDosDeviceCore(true, deviceName, targetPath, deviceAttributes, false);
      }

      #endregion // DefineDosDevice

      #region DeleteDosDevice

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name specifying the device to delete.</param>      
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName)
      {
         DefineDosDeviceCore(false, deviceName, null, DosDeviceAttributes.RemoveDefinition, false);
      }

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device to delete.</param>
      /// <param name="targetPath">
      ///   A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the
      ///   <see cref="DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.
      /// </param>      
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName, string targetPath)
      {
         DefineDosDeviceCore(false, deviceName, targetPath, DosDeviceAttributes.RemoveDefinition, false);
      }

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device to delete.</param>
      /// <param name="targetPath">
      ///   A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the
      ///   <see cref="DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.
      /// </param>
      /// <param name="exactMatch">
      ///   Only delete MS-DOS device on an exact name match. If <paramref name="exactMatch"/> is <see langword="true"/>,
      ///   <paramref name="targetPath"/> must be the same path used to create the mapping.
      /// </param>      
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName, string targetPath, bool exactMatch)
      {
         DefineDosDeviceCore(false, deviceName, targetPath, DosDeviceAttributes.RemoveDefinition, exactMatch);
      }

      /// <summary>Deletes an MS-DOS device name.</summary>
      /// <param name="deviceName">An MS-DOS device name string specifying the device to delete.</param>
      /// <param name="targetPath">
      ///   A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the
      ///   <see cref="DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.
      /// </param>
      /// <param name="deviceAttributes">
      ///   The controllable aspects of the DefineDosDevice function <see cref="DosDeviceAttributes"/> flags which will be combined with the
      ///   default.
      /// </param>
      /// <param name="exactMatch">
      ///   Only delete MS-DOS device on an exact name match. If <paramref name="exactMatch"/> is <see langword="true"/>,
      ///   <paramref name="targetPath"/> must be the same path used to create the mapping.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static void DeleteDosDevice(string deviceName, string targetPath, DosDeviceAttributes deviceAttributes, bool exactMatch)
      {
         DefineDosDeviceCore(false, deviceName, targetPath, deviceAttributes, exactMatch);
      }

      #endregion // DeleteDosDevice
      
      #region QueryAllDosDevices

      /// <summary>Retrieves a list of all existing MS-DOS device names.</summary>
      /// <returns>An <see cref="IEnumerable{String}"/> with one or more existing MS-DOS device names.</returns>      
      [SecurityCritical]
      public static IEnumerable<string> QueryAllDosDevices()
      {
         return QueryDosDevice(null, null);
      }

      /// <summary>Retrieves a list of all existing MS-DOS device names.</summary>
      /// <param name="deviceName">
      ///   (Optional, default: <see langword="null"/>) An MS-DOS device name string specifying the target of the query. This parameter can be
      ///   "sort". In that case a sorted list of all existing MS-DOS device names is returned. This parameter can be <see langword="null"/>.
      ///   In that case, the <see cref="QueryDosDevice"/> function will store a list of all existing MS-DOS device names into the buffer.
      /// </param>
      /// <returns>An <see cref="IEnumerable{String}"/> with or more existing MS-DOS device names.</returns>      
      [SecurityCritical]
      public static IEnumerable<string> QueryAllDosDevices(string deviceName)
      {
         return QueryDosDevice(null, deviceName);
      }

      #endregion // QueryAllDosDevices

      #region QueryDosDevice

      /// <summary>
      ///   Retrieves information about MS-DOS device names. The function can obtain the current mapping for a particular MS-DOS device name.
      ///   The function can also obtain a list of all existing MS-DOS device names.
      /// </summary>
      /// <param name="deviceName">
      ///   An MS-DOS device name string, or part of, specifying the target of the query. This parameter can be <see langword="null"/>. In that
      ///   case, the QueryDosDevice function will store a list of all existing MS-DOS device names into the buffer.
      /// </param>
      /// <param name="options">
      ///   (Optional, default: <see langword="false"/>) If options[0] = <see langword="true"/> a sorted list will be returned.
      /// </param>
      /// <returns>An <see cref="IEnumerable{String}"/> with one or more existing MS-DOS device names.</returns>      
      [SecurityCritical]
      public static IEnumerable<string> QueryDosDevice(string deviceName, params string[] options)
      {
         // deviceName is allowed to be null.
         // The deviceName cannot have a trailing backslash.
         deviceName = Path.RemoveTrailingDirectorySeparator(deviceName, false);

         var searchFilter = deviceName != null;

         // Only process options if a device is supplied.
         if (searchFilter)
         {
            // Check that at least one "options[]" has something to say. If so, rebuild them.
            options = options != null && options.Any() ? new[] { deviceName, options[0] } : new[] { deviceName, string.Empty };

            deviceName = null;
         }
         
         // Choose sorted output.
         var doSort = options != null &&
                       options.Any(s => s != null && s.Equals("sort", StringComparison.OrdinalIgnoreCase));

         // Start with a larger buffer when using a searchFilter.
         var bufferSize = (uint) (searchFilter || doSort || (options == null) ? 8*NativeMethods.DefaultFileBufferSize : 256);
         uint bufferResult = 0;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (bufferResult == 0)
            {
               var cBuffer = new char[bufferSize];

               // QueryDosDevice()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2014-01-29: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               bufferResult = NativeMethods.QueryDosDevice(deviceName, cBuffer, bufferSize);
               var lastError = Marshal.GetLastWin32Error();

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

               var dosDev = new List<string>();
               var buffer = new StringBuilder();

               for (var i = 0; i < bufferResult; i++)
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
               var selectQuery = searchFilter
                  ? dosDev.Where(dev => options != null && dev.StartsWith(options[0], StringComparison.OrdinalIgnoreCase))
                  : dosDev;

               foreach (var dev in (doSort) ? selectQuery.OrderBy(n => n) : selectQuery)
                  yield return dev;
            }
      }

      #endregion // QueryDosDevice

      #endregion // DosDevice

      #region Drive
      
      #region GetDriveFormat

      /// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
      /// <remarks>Use DriveFormat to determine what formatting a drive uses.</remarks>
      /// <param name="drivePath">
      ///   A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <returns>The name of the file system on the specified drive or <see langword="null"/>  on failure or if not available.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetDriveFormat(string drivePath)
      {
         var fsName = new VolumeInfo(drivePath, true, true).FileSystemName;
         return Utils.IsNullOrWhiteSpace(fsName) ? null : fsName;
      }

      #endregion // GetDriveFormat

      #region GetDriveNameForNtDeviceName

      /// <summary>Gets the drive letter from an MS-DOS device name. For example: "\Device\HarddiskVolume2" returns "C:\".</summary>
      /// <param name="deviceName">An MS-DOS device name.</param>
      /// <returns>The drive letter from an MS-DOS device name.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Nt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nt")]
      public static string GetDriveNameForNtDeviceName(string deviceName)
      {
         return (from drive in Directory.EnumerateLogicalDrivesCore(false, false)
            where drive.DosDeviceName.Equals(deviceName, StringComparison.OrdinalIgnoreCase)
            select drive.Name).FirstOrDefault();
      }
      
      #endregion // GetDriveNameForNtDeviceName

      #region GetCurrentDriveType

      /// <summary>
      ///   Determines, based on the root of the current directory, whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network
      ///   drive.
      /// </summary>
      /// <returns>A <see cref="DriveType"/> object.</returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static DriveType GetCurrentDriveType()
      {
         return GetDriveType(null);
      }

      #endregion // GetCurrentDriveType

      #region GetDriveType

      /// <summary>Determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.</summary>
      /// <param name="drivePath">A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</param>
      /// <returns>A <see cref="System.IO.DriveType"/> object.</returns>
      [SecurityCritical]
      public static DriveType GetDriveType(string drivePath)
      {
         // drivePath is allowed to be == null.

         drivePath = Path.AddTrailingDirectorySeparator(drivePath, false);

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups. 
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            return NativeMethods.GetDriveType(drivePath);
      }

      #endregion // GetDriveType

      #region GetDiskFreeSpace

      /// <summary>
      ///   Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total
      ///   amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
      /// </summary>
      /// <remarks>The calling application must have FILE_LIST_DIRECTORY access rights for this directory.</remarks>
      /// <param name="drivePath">
      ///   A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <returns>A <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> class instance.</returns>      
      [SecurityCritical]
      public static DiskSpaceInfo GetDiskFreeSpace(string drivePath)
      {
         return new DiskSpaceInfo(drivePath, null, true, true);
      }

      /// <summary>
      ///   Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total
      ///   amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
      /// </summary>
      /// <remarks>The calling application must have FILE_LIST_DIRECTORY access rights for this directory.</remarks>
      /// <param name="drivePath">
      ///   A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <param name="spaceInfoType">
      ///   <see langword="null"/> gets both size- and disk cluster information. <see langword="true"/> Get only disk cluster information,
      ///   <see langword="false"/> Get only size information.
      /// </param>
      /// <returns>A <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> class instance.</returns>      
      [SecurityCritical]
      public static DiskSpaceInfo GetDiskFreeSpace(string drivePath, bool? spaceInfoType)
      {
         return new DiskSpaceInfo(drivePath, spaceInfoType, true, true);
      }

      #endregion // GetDiskFreeSpace
      
      #region IsReady

      /// <summary>Gets a value indicating whether a drive is ready.</summary>
      /// <param name="drivePath">
      ///   A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <returns><see langword="true"/> if <paramref name="drivePath"/> is ready; otherwise, <see langword="false"/>.</returns>
      [SecurityCritical]
      public static bool IsReady(string drivePath)
      {
         return File.ExistsCore(true, null, drivePath, PathFormat.FullPath);
      }

      #endregion // IsReady

      #endregion // Drive

      #region Volume

      #region DeleteCurrentVolumeLabel

      /// <summary>Deletes the label of the file system volume that is the root of the current directory.
      /// </summary>
      [SecurityCritical]
      public static void DeleteCurrentVolumeLabel()
      {
         SetVolumeLabel(null, null);
      }
      #endregion // DeleteCurrentVolumeLabel

      #region DeleteVolumeLabel

      /// <summary>Deletes the label of a file system volume.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="rootPathName">The root directory of a file system volume. This is the volume the function will remove the label.</param>
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
      /// <remarks>Deleting a mounted folder does not cause the underlying directory to be deleted.</remarks>
      /// <remarks>
      ///   If the <paramref name="volumeMountPoint"/> parameter is a directory that is not a mounted folder, the function does nothing. The
      ///   directory is not deleted.
      /// </remarks>
      /// <remarks>
      ///   It's not an error to attempt to unmount a volume from a volume mount point when there is no volume actually mounted at that volume
      ///   mount point.
      /// </remarks>
      /// <param name="volumeMountPoint">The Drive letter or mounted folder to be deleted. For example, X:\ or Y:\MountX\.</param>      
      [SecurityCritical]
      public static void DeleteVolumeMountPoint(string volumeMountPoint)
      {
         DeleteVolumeMountPointCore(volumeMountPoint, false);
      }
      
      #endregion // DeleteVolumeMountPoint
      
      #region EnumerateVolumeMountPoints

      /// <summary>
      ///   Returns an enumerable collection of <see cref="String"/> of all mounted folders (volume mount points) on the specified volume.
      /// </summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="volumeGuid">A <see cref="string"/> containing the volume <see cref="Guid"/>.</param>
      /// <returns>An enumerable collection of <see cref="String"/> of all volume mount points on the specified volume.</returns>      
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumeMountPoints(string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");

         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Not_A_Valid_Guid, volumeGuid);

         // A trailing backslash is required.
         volumeGuid = Path.AddTrailingDirectorySeparator(volumeGuid, false);

         var buffer = new StringBuilder(NativeMethods.MaxPathUnicode);

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         using (var handle = NativeMethods.FindFirstVolumeMountPoint(volumeGuid, buffer, (uint)buffer.Capacity))
         {
            var lastError = Marshal.GetLastWin32Error();

            if (handle.IsInvalid)
            {
               handle.Close();

               switch ((uint) lastError)
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
               lastError = Marshal.GetLastWin32Error();

               if (handle.IsInvalid)
               {
                  handle.Close();

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

      /// <summary>
      ///   Returns an enumerable collection of <see cref="String"/> drive letters and mounted folder paths for the specified volume.
      /// </summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="volumeGuid">A volume <see cref="Guid"/> path: \\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\.</param>
      /// <returns>An enumerable collection of <see cref="String"/> containing the path names for the specified volume.</returns>      
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumePathNames(string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");

         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Not_A_Valid_Guid, volumeGuid);

         var volName = Path.AddTrailingDirectorySeparator(volumeGuid, false);

         uint requiredLength = 10;
         var cBuffer = new char[requiredLength];

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (!NativeMethods.GetVolumePathNamesForVolumeName(volName, cBuffer, (uint)cBuffer.Length, out requiredLength))
            {
               var lastError = Marshal.GetLastWin32Error();

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

         var buffer = new StringBuilder(cBuffer.Length);
         foreach (var c in cBuffer)
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

      /// <summary>Returns an enumerable collection of <see cref="String"/> volumes on the computer.</summary>
      /// <returns>An enumerable collection of <see cref="String"/> volume names on the computer.</returns>      
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumes()
      {
         var buffer = new StringBuilder(NativeMethods.MaxPathUnicode);

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         using (var handle = NativeMethods.FindFirstVolume(buffer, (uint)buffer.Capacity))
         {
            while (handle != null && !handle.IsInvalid)
            {
               if (NativeMethods.FindNextVolume(handle, buffer, (uint)buffer.Capacity))
                  yield return buffer.ToString();

               else
               {
                  var lastError = Marshal.GetLastWin32Error();

                  handle.Close();

                  if (lastError == Win32Errors.ERROR_NO_MORE_FILES)
                     yield break;

                  NativeError.ThrowException(lastError);
               }
            }
         }
      }

      #endregion // EnumerateVolumes
      
      #region GetUniqueVolumeNameForPath

      /// <summary>
      ///   Get the unique volume name for the given path.
      /// </summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumePathName">
      ///   A path string. Both absolute and relative file and directory names, for example "..", is acceptable in this path. If you specify a
      ///   relative file or directory name without a volume qualifier, GetUniqueVolumeNameForPath returns the Drive letter of the current
      ///   volume.
      /// </param>
      /// <returns>
      ///   <para>Returns the unique volume name in the form: "\\?\Volume{GUID}\",</para>
      ///   <para>or <see langword="null"/> on error or if unavailable.</para>
      /// </returns>
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
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumeName">Name of the Volume.</param>
      /// <returns>
      ///   The Win32 Device name from the Volume name (for example: "\Device\HarddiskVolume2"), or <see langword="null"/> on error or if
      ///   unavailable.
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeDeviceName(string volumeName)
      {
         if (Utils.IsNullOrWhiteSpace(volumeName))
            throw new ArgumentNullException("volumeName");

         volumeName = Path.RemoveTrailingDirectorySeparator(volumeName, false);

         #region GlobalRoot

         if (volumeName.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
            return volumeName.Substring(Path.GlobalRootPrefix.Length);

         #endregion // GlobalRoot

         bool doQueryDos;

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
         else
         {
            // Don't use char.IsLetter() here as that can be misleading.
            // The only valid drive letters are: a-z and A-Z.
            var c = volumeName[0];
            doQueryDos = (volumeName[1] == Path.VolumeSeparatorChar && ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')));
         }

         #endregion // Logical Drive

         if (doQueryDos)
         {
            try
            {
               // Get the real Device underneath.
               var dev = QueryDosDevice(volumeName).FirstOrDefault();
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
      /// <remarks>This method basically returns the shortest string returned by <see cref="EnumerateVolumePathNames"/></remarks>
      /// <param name="volumeName">A volume <see cref="Guid"/> path: \\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\.</param>
      /// <returns>
      ///   The shortest display name for the specified volume found, or <see langword="null"/> if no display names were found.
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeDisplayName(string volumeName)
      {
         string[] smallestMountPoint = { new string(Path.WildcardStarMatchAllChar, NativeMethods.MaxPathUnicode) };

         try
         {
            foreach (var m in EnumerateVolumePathNames(volumeName).Where(m => !Utils.IsNullOrWhiteSpace(m) && m.Length < smallestMountPoint[0].Length))
               smallestMountPoint[0] = m;
         }
         catch
         {
         }

         var result = smallestMountPoint[0][0] == Path.WildcardStarMatchAllChar ? null : smallestMountPoint[0];
         return Utils.IsNullOrWhiteSpace(result) ? null : result;
      }

      #endregion // GetVolumeDisplayName

      #region GetVolumeGuid

      /// <summary>
      ///   Retrieves a volume <see cref="Guid"/> path for the volume that is associated with the specified volume mount point (drive letter,
      ///   volume GUID path, or mounted folder).
      /// </summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumeMountPoint">
      ///   The path of a mounted folder (for example, "Y:\MountX\") or a drive letter (for example, "X:\").
      /// </param>
      /// <returns>The unique volume name of the form: "\\?\Volume{GUID}\".</returns>      
      [SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "Marshal.GetLastWin32Error() is manipulated.")]
      [SecurityCritical]
      public static string GetVolumeGuid(string volumeMountPoint)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");

         // The string must end with a trailing backslash ('\').
         volumeMountPoint = Path.GetFullPathCore(null, volumeMountPoint, GetFullPathOptions.AsLongPath | GetFullPathOptions.AddTrailingDirectorySeparator | GetFullPathOptions.FullCheck);            

         var volumeGuid = new StringBuilder(100);
         var uniqueName = new StringBuilder(100);

         try
         {
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               // GetVolumeNameForVolumeMountPoint()
               // In the ANSI version of this function, the name is limited to 248 characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               return NativeMethods.GetVolumeNameForVolumeMountPoint(volumeMountPoint, volumeGuid, (uint) volumeGuid.Capacity)
                  ? NativeMethods.GetVolumeNameForVolumeMountPoint(Path.AddTrailingDirectorySeparator(volumeGuid.ToString(), false), uniqueName, (uint) uniqueName.Capacity)
                     ? uniqueName.ToString()
                     : null
                  : null;
            }
         }
         finally
         {
            var lastError = (uint) Marshal.GetLastWin32Error();

            switch (lastError)
            {
               case Win32Errors.ERROR_INVALID_NAME:
                  NativeError.ThrowException(lastError, volumeMountPoint);
                  break;

               case Win32Errors.ERROR_MORE_DATA:
                  // (1) When GetVolumeNameForVolumeMountPoint() succeeds, lastError is set to Win32Errors.ERROR_MORE_DATA.
                  break;

               default:
                  // (2) When volumeMountPoint is a network drive mapping or UNC path, lastError is set to Win32Errors.ERROR_INVALID_PARAMETER.

                  // Throw IOException.
                  NativeError.ThrowException(lastError, volumeMountPoint);
                  break;
            }
         }
      }

      #endregion // GetVolumeGuid

      #region GetVolumeGuidForNtDeviceName

      /// <summary>
      ///   Tranlates DosDevicePath to a Volume GUID. For example: "\Device\HarddiskVolumeX\path\filename.ext" can translate to: "\path\
      ///   filename.ext" or: "\\?\Volume{GUID}\path\filename.ext".
      /// </summary>
      /// <param name="dosDevice">A DosDevicePath, for example: \Device\HarddiskVolumeX\path\filename.ext.</param>
      /// <returns>A translated dos path.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Nt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nt")]
      public static string GetVolumeGuidForNtDeviceName(string dosDevice)
      {
         return (from drive in Directory.EnumerateLogicalDrivesCore(false, false)
                 where drive.DosDeviceName.Equals(dosDevice, StringComparison.OrdinalIgnoreCase)
                 select drive.VolumeInfo.Guid).FirstOrDefault();
      }

      #endregion // GetVolumeGuidForNtDeviceName

      #region GetVolumeInfo

      /// <summary>Retrieves information about the file system and volume associated with the specified root file or directorystream.</summary>
      /// <param name="volumePath">A path that contains the root directory.</param>
      /// <returns>A <see cref="VolumeInfo"/> instance describing the volume associatied with the specified root directory.</returns>      
      [SecurityCritical]
      public static VolumeInfo GetVolumeInfo(string volumePath)
      {
         return new VolumeInfo(volumePath, true, false);
      }

      /// <summary>Retrieves information about the file system and volume associated with the specified root file or directorystream.</summary>
      /// <param name="volumeHandle">An instance to a <see cref="SafeFileHandle"/> handle.</param>
      /// <returns>A <see cref="VolumeInfo"/> instance describing the volume associatied with the specified root directory.</returns>      
      [SecurityCritical]
      public static VolumeInfo GetVolumeInfo(SafeFileHandle volumeHandle)
      {
         return new VolumeInfo(volumeHandle, true, true);
      }

      #endregion // GetVolumeInfo

      #region GetVolumeLabel

      /// <summary>Retrieve the label of a file system volume.</summary>
      /// <param name="volumePath">
      ///   A path to a volume. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <returns>
      ///   The the label of the file system volume. This function can return <c>string.Empty</c> since a volume label is generally not
      ///   mandatory.
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeLabel(string volumePath)
      {
         return new VolumeInfo(volumePath, true, true).Name;
      }

      #endregion // GetVolumeLabel

      #region GetVolumePathName

      /// <summary>Retrieves the volume mount point where the specified path is mounted.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to the volume, for example: "C:\Windows".</param>
      /// <returns>
      ///   <para>Returns the nearest volume root path for a given directory.</para>
      ///   <para>The volume path name, for example: "C:\Windows" returns: "C:\".</para>
      /// </returns>
      [SecurityCritical]
      public static string GetVolumePathName(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");
         
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            var volumeRootPath = new StringBuilder(NativeMethods.MaxPathUnicode / 32);
            var pathLp = Path.GetFullPathCore(null, path, GetFullPathOptions.AsLongPath | GetFullPathOptions.FullCheck);

            // GetVolumePathName()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            var getOk = NativeMethods.GetVolumePathName(pathLp, volumeRootPath, (uint) volumeRootPath.Capacity);
            var lastError = Marshal.GetLastWin32Error();

            if (getOk)
               return Path.GetRegularPathCore(volumeRootPath.ToString(), GetFullPathOptions.None, false);

            switch ((uint) lastError)
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
      }

      #endregion // GetVolumePathName

      #region IsSameVolume

      /// <summary>Determines whether the volume of two file system objects is the same.</summary>
      /// <param name="path1">The first filesystem ojbect with full path information.</param>
      /// <param name="path2">The second file system object with full path information.</param>
      /// <returns><see langword="true"/> if both filesytem objects reside on the same volume, <see langword="false"/> otherwise.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static bool IsSameVolume(string path1, string path2)
      {
         try
         {
            var volInfo1 = new VolumeInfo(GetVolumePathName(path1), true, true);
            var volInfo2 = new VolumeInfo(GetVolumePathName(path2), true, true);

            return volInfo1.SerialNumber == volInfo2.SerialNumber;
         }
         catch { }

         return false;
      }

      #endregion // IsSameVolume

      #region IsVolume

      /// <summary>Determines whether the specified volume name is a defined volume on the current computer.</summary>
      /// <param name="volumeMountPoint">
      ///   A path to a volume. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <returns><see langword="true"/> on success, <see langword="false"/> otherwise.</returns>
      [SecurityCritical]
      public static bool IsVolume(string volumeMountPoint)
      {
         return !Utils.IsNullOrWhiteSpace(GetVolumeGuid(volumeMountPoint));
      }

      #endregion // IsVolume

      #region SetCurrentVolumeLabel

      /// <summary>Sets the label of the file system volume that is the root of the current directory.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumeName">A name for the volume.</param>
      [SecurityCritical]
      public static void SetCurrentVolumeLabel(string volumeName)
      {
         if (Utils.IsNullOrWhiteSpace(volumeName))
            throw new ArgumentNullException("volumeName");

         if (!NativeMethods.SetVolumeLabel(null, volumeName))
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
      }

      #endregion // SetCurrentVolumeLabel

      #region SetVolumeLabel

      /// <summary>Sets the label of a file system volume.</summary>
      /// <param name="volumePath">
      ///   <para>A path to a volume. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"</para>
      ///   <para>If this parameter is <see langword="null"/>, the function uses the current drive.</para>
      /// </param>
      /// <param name="volumeName">
      ///   <para>A name for the volume.</para>
      ///   <para>If this parameter is <see langword="null"/>, the function deletes any existing label</para>
      ///   <para>from the specified volume and does not assign a new label.</para>
      /// </param>
      [SecurityCritical]
      public static void SetVolumeLabel(string volumePath, string volumeName)
      {
         // rootPathName == null is allowed, means current drive.

         // Setting volume label only applies to Logical Drives pointing to local resources.
         //if (!Path.IsLocalPath(rootPathName))
         //return false;

         volumePath = Path.AddTrailingDirectorySeparator(volumePath, false);

         // NTFS uses a limit of 32 characters for the volume label as of Windows Server 2003.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            if (!NativeMethods.SetVolumeLabel(volumePath, volumeName))
               NativeError.ThrowException(volumePath, volumeName);
      }

      #endregion // SetVolumeLabel
      
      #region SetVolumeMountPoint

      /// <summary>Associates a volume with a Drive letter or a directory on another volume.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="volumeMountPoint">
      ///   The user-mode path to be associated with the volume. This may be a Drive letter (for example, "X:\")
      ///   or a directory on another volume (for example, "Y:\MountX\").
      /// </param>
      /// <param name="volumeGuid">A <see cref="string"/> containing the volume <see cref="Guid"/>.</param>      
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public static void SetVolumeMountPoint(string volumeMountPoint, string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");

         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");

         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Not_A_Valid_Guid, volumeGuid);

         volumeMountPoint = Path.GetFullPathCore(null, volumeMountPoint, GetFullPathOptions.AsLongPath | GetFullPathOptions.AddTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

         // This string must be of the form "\\?\Volume{GUID}\"
         volumeGuid = Path.AddTrailingDirectorySeparator(volumeGuid, false);


         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))

            // SetVolumeMountPoint()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-01-29: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            if (!NativeMethods.SetVolumeMountPoint(volumeMountPoint, volumeGuid))
            {
               var lastError = Marshal.GetLastWin32Error();

               // If the lpszVolumeMountPoint parameter contains a path to a mounted folder,
               // GetLastError returns ERROR_DIR_NOT_EMPTY, even if the directory is empty.

               if (lastError != Win32Errors.ERROR_DIR_NOT_EMPTY)
                  NativeError.ThrowException(lastError, volumeGuid);
            }
      }

      #endregion // SetVolumeMountPoint

      #endregion // Volume


      #region Internal Methods

      /// <summary>Defines, redefines, or deletes MS-DOS device names.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="isDefine">
      ///   <see langword="true"/> defines a new MS-DOS device. <see langword="false"/> deletes a previously defined MS-DOS device.
      /// </param>
      /// <param name="deviceName">
      ///   An MS-DOS device name string specifying the device the function is defining, redefining, or deleting.
      /// </param>
      /// <param name="targetPath">
      ///   A pointer to a path string that will implement this device. The string is an MS-DOS path string unless the
      ///   <see cref="DosDeviceAttributes.RawTargetPath"/> flag is specified, in which case this string is a path string.
      /// </param>
      /// <param name="deviceAttributes">
      ///   The controllable aspects of the DefineDosDevice function, <see cref="DosDeviceAttributes"/> flags which will be combined with the
      ///   default.
      /// </param>
      /// <param name="exactMatch">
      ///   Only delete MS-DOS device on an exact name match. If <paramref name="exactMatch"/> is <see langword="true"/>,
      ///   <paramref name="targetPath"/> must be the same path used to create the mapping.
      /// </param>
      ///
      /// <returns><see langword="true"/> on success, <see langword="false"/> otherwise.</returns>      
      [SecurityCritical]
      internal static void DefineDosDeviceCore(bool isDefine, string deviceName, string targetPath, DosDeviceAttributes deviceAttributes, bool exactMatch)
      {
         if (Utils.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentNullException("deviceName");

         if (isDefine)
         {
            // targetPath is allowed to be null.

            // In no case is a trailing backslash ("\") allowed.
            deviceName = Path.GetRegularPathCore(deviceName, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars, false);

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

      /// <summary>Deletes a Drive letter or mounted folder.</summary>
      /// <remarks>
      ///   <para>It's not an error to attempt to unmount a volume from a volume mount point when there is no volume actually mounted at that volume mount point.</para>
      ///   <para>Deleting a mounted folder does not cause the underlying directory to be deleted.</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumeMountPoint">The Drive letter or mounted folder to be deleted. For example, X:\ or Y:\MountX\.</param>
      /// <param name="continueOnException">
      ///   <see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.
      /// </param>
      /// <returns>If completed successfully returns <see cref="Win32Errors.ERROR_SUCCESS"/>, otherwise the last error number.</returns>      
      [SecurityCritical]
      internal static int DeleteVolumeMountPointCore(string volumeMountPoint, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");

         var lastError = (int) Win32Errors.ERROR_SUCCESS;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            // DeleteVolumeMountPoint()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            if (!NativeMethods.DeleteVolumeMountPoint(Path.AddTrailingDirectorySeparator(volumeMountPoint, false)))
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

      #endregion // Internal Methods
   }
}

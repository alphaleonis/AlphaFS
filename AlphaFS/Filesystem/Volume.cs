/* Copyright (c) 2008-2009 Peter Palotas
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
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Static class providing utility methods for working with Microsoft Windows devices and volumes. Most
    /// of the methods in this class is simple convenience methods for native Win32 API-calls to make them 
    /// simpler to use from managed languages.
    /// </summary>
    public static class Volume
    {
        /// <summary>
        /// Retrieves the name of all volumes on the computer.
        /// </summary>
        /// <remarks>Requires Windows Vista, Windows XP, or Windows 2000 Professional.</remarks>
        /// <returns>An array containing the volume names on the computer.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        static public string[] GetVolumes()
        {
            StringBuilder sb = new StringBuilder(NativeMethods.MAX_PATH);

            SafeHandle h = NativeMethods.FindFirstVolumeW(sb, sb.Capacity);

            if (h.IsInvalid)
            {
                throw new Win32Exception();
            }

            List<string> volumes = new List<string>();
            volumes.Add(sb.ToString());

            while (NativeMethods.FindNextVolumeW(h, sb, sb.Capacity))
            {
                volumes.Add(sb.ToString());
            }

            int result = Marshal.GetLastWin32Error();

            h.Close();

            if (result != Win32Errors.ERROR_NO_MORE_FILES)
            {
                throw new Win32Exception(result);
            }

            return volumes.ToArray();

        }

        /// <summary>
        /// Retrieves the names of all volume mount points on the specified volume.
        /// </summary>
        /// <remarks>Requires Windows Vista, Windows XP, or Windows 2000 Professional.</remarks>
        /// <param name="volumeName">The unique volume name of the volume to scan for volume mount points. A trailing backslash is required.</param>
        /// <returns>The names of all volume mount points on the specified volume.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        static public string[] GetVolumeMountPoints(string volumeName)
        {
            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            StringBuilder name = new StringBuilder(32768);
            SafeHandle h = NativeMethods.FindFirstVolumeMountPointW(volumeName, name, name.Capacity);

            if (h.IsInvalid)
            {
                if (Marshal.GetLastWin32Error() == Win32Errors.ERROR_NO_MORE_FILES)
                    return new string[0];

                throw new Win32Exception();
            }

            List<string> mountPoints = new List<string>();
            mountPoints.Add(name.ToString());

            while (NativeMethods.FindNextVolumeMountPointW(h, name, name.Capacity))
            {
                mountPoints.Add(name.ToString());
            }

            int result = Marshal.GetLastWin32Error();

            h.Close();

            if (result != Win32Errors.ERROR_NO_MORE_FILES)
            {
                throw new Win32Exception(result);
            }

            return mountPoints.ToArray();
        }

        /// <summary>
        /// Determines whether a disk drive containing the current directory is a removable, fixed, CD-ROM, RAM disk, or network drive.
        /// </summary>
        /// <remarks>Requires Windows Vista, Windows XP, or Windows 2000 Professional.</remarks>
        /// <returns>A value indicating whether a disk drive containing the current directory is a removable, fixed, CD-ROM, RAM disk, or network drive.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        static public DriveType GetCurrentDriveType()
        {
            return (DriveType)NativeMethods.GetDriveType(null);
        }

        /// <summary>
        /// Determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.
        /// </summary>
        /// <remarks>Requires Windows Vista, Windows XP, or Windows 2000 Professional.</remarks>
        /// <param name="volumeName">The root directory for the drive. A trailing backslash is required. </param>
        /// <returns>A value indicating whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        static public DriveType GetDriveType(string volumeName)
        {
            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            return (DriveType)NativeMethods.GetDriveType(volumeName);
        }

        /// <summary>
        /// Returns an array of strings that specify valid drives in the system.
        /// </summary>
        /// <remarks>Each string in the buffer may be used wherever a root directory is required, such as for the GetDriveType and GetDiskFreeSpace functions.</remarks>
        /// <returns>An array of strings that specify valid drives in the system.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        static public string[] GetLogicalDrives()
        {
            char[] drives = new char[256];

            uint result = NativeMethods.GetLogicalDriveStringsW((uint)drives.Length, drives);

            if (result > drives.Length)
            {
                drives = new char[result + 1];
                result = NativeMethods.GetLogicalDriveStringsW((uint)drives.Length, drives);
            }

            if (result == 0)
                throw new Win32Exception();

            List<string> l = new List<string>();

            StringBuilder b = new StringBuilder();
            foreach (char c in drives)
            {
                if (c != '\0')
                {
                    b.Append(c);
                }
                else
                {
                    if (b.Length > 0)
                        l.Add(b.ToString());
                    b.Length = 0;
                }
            }

            return l.ToArray();

        }

        /// <summary>
        /// Retrieves information about the file system and volume associated with the specified root directory.
        /// </summary>
        /// <param name="rootPathName">The root directory of the volume to be described.</param>
        /// <returns>A <see cref="VolumeInfo"/> instance describing the volume associatied with the specified root directory</returns>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="rootPathName"/> was not a valid volume name</exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static VolumeInfo GetVolumeInformation(string rootPathName)
        {
            StringBuilder volumeNameBuffer = new StringBuilder(NativeMethods.MAX_PATH + 1);
            StringBuilder fileSystemNameBuffer = new StringBuilder(NativeMethods.MAX_PATH + 1);
            uint serialNumber = 0;
            uint maximumComponentLength = 0;
            uint fileSystemFlags = 0;

            if (!NativeMethods.GetVolumeInformationW(rootPathName, volumeNameBuffer, volumeNameBuffer.Capacity,
                out serialNumber, out maximumComponentLength, out fileSystemFlags, fileSystemNameBuffer,
                fileSystemNameBuffer.Capacity))
            {
                int error = Marshal.GetLastWin32Error();
                Exception e = Marshal.GetExceptionForHR(Win32Errors.GetHRFromWin32Error((uint)error));
                if (error == Win32Errors.ERROR_NOT_READY)
                    throw new DeviceNotReadyException("Device not ready", e);

                throw e;
            }

            return new VolumeInfo(volumeNameBuffer.ToString(), fileSystemFlags, serialNumber,
                (int)maximumComponentLength, fileSystemNameBuffer.ToString());
        }

        /// <summary>
        /// Retrieves information about the file system and volume associated with the specified <see cref="System.IO.FileStream"/>.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A <see cref="VolumeInfo"/> instance describing the volume associatied with the specified root directory</returns>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="file"/> was not a valid volume name</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static VolumeInfo GetVolumeInformation(FileStream file)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            StringBuilder volumeNameBuffer = new StringBuilder(NativeMethods.MAX_PATH + 1);
            StringBuilder fileSystemNameBuffer = new StringBuilder(NativeMethods.MAX_PATH + 1);
            uint serialNumber = 0;
            uint maximumComponentLength = 0;
            uint fileSystemFlags = 0;

            if (!NativeMethods.GetVolumeInformationByHandleW(file.SafeFileHandle, volumeNameBuffer, volumeNameBuffer.Capacity,
                out serialNumber, out maximumComponentLength, out fileSystemFlags, fileSystemNameBuffer,
                fileSystemNameBuffer.Capacity))
            {
                int error = Marshal.GetLastWin32Error();
                Exception e = Marshal.GetExceptionForHR(Win32Errors.GetHRFromWin32Error((uint)error));
                if (error == Win32Errors.ERROR_NOT_READY)
                    throw new DeviceNotReadyException("Device not ready", e);

                throw e;
            }

            return new VolumeInfo(volumeNameBuffer.ToString(), fileSystemFlags, serialNumber,
                (int)maximumComponentLength, fileSystemNameBuffer.ToString());
        }

        /// <summary>
        /// Sets the label of a file system volume.
        /// </summary>
        /// <param name="rootPathName">The root directory of a file system volume. This is the volume the function will label. A trailing backslash is required.</param>
        /// <param name="volumeName">A name for the volume.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rootPathName"/> or <paramref name="volumeName"/> is a <see langword="null"/> reference.</exception>        
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetVolumeLabel(string rootPathName, string volumeName)
        {
            if (rootPathName == null)
                throw new ArgumentNullException("rootPathName");

            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            if (!NativeMethods.SetVolumeLabelW(rootPathName, volumeName))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Sets the label of the file system volume that is the root of the current directory
        /// </summary>
        /// <param name="volumeName">A name for the volume.</param>
        /// <exception cref="ArgumentNullException"><paramref name="volumeName"/> is a <see langword="null"/> reference.</exception>        
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetCurrentVolumeLabel(string volumeName)
        {
            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            if (!NativeMethods.SetVolumeLabelW(null, volumeName))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Deletes the label of a file system volume.
        /// </summary>
        /// <param name="rootPathName">The root directory of a file system volume. This is the volume the function will label. A trailing backslash is required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rootPathName"/> is a <see langword="null"/> reference.</exception>        
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void DeleteVolumeLabel(string rootPathName)
        {
            if (rootPathName == null)
                throw new ArgumentNullException("rootPathName");

            if (!NativeMethods.SetVolumeLabelW(rootPathName, null))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Deletes the label of the file system volume that is the root of the current directory
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void DeleteCurrentVolumeLabel()
        {
            if (!NativeMethods.SetVolumeLabelW(null, null))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Mounts the specified volume at the specified volume mount point.
        /// </summary>
        /// <param name="volumeMountPoint">The volume mount point where the volume is to be mounted. This may be a root directory (X:\) or a directory on a volume (X:\mnt\). The string must end with a trailing backslash ('\').</param>
        /// <param name="volumeName">The volume to be mounted. This string must be a unique volume name of the form "\\?\Volume{GUID}\" where GUID is a GUID that identifies the volume. The \\?\ turns off path parsing and is ignored as part of the path. For example, "\\?\C:\myworld\private" is seen as "C:\myworld\private".</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetVolumeMountPoint(string volumeMountPoint, string volumeName)
        {
            if (volumeMountPoint == null)
                throw new ArgumentNullException("volumeMountPoint");

            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            if (!NativeMethods.SetVolumeMountPointW(volumeMountPoint, volumeName))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Unmounts the volume from the specified volume mount point.
        /// </summary>
        /// <param name="volumeMountPoint">The volume mount point to be unmounted. This may be a root directory (X:\, in which case the DOS drive letter assignment is removed) or a directory on a volume (X:\mnt\). A trailing backslash is required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="volumeMountPoint"/> is <see langword="null"/></exception>
        /// <remarks>It is not an error to attempt to unmount a volume from a volume mount point when there is no volume actually mounted at that volume mount point.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void DeleteVolumeMountPoint(string volumeMountPoint)
        {
            if (volumeMountPoint == null)
                throw new ArgumentNullException("volumeMountPoint");

            if (!NativeMethods.DeleteVolumeMountPointW(volumeMountPoint))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        /// <summary>
        /// Defines or redefines MS-DOS device names.
        /// </summary>
        /// <param name="targetPath">A MS-DOS path string that will implement this device.</param>
        /// <param name="deviceName">An MS-DOS device name string specifying the device the function is 
        /// defining or redefining. The device name string must not have a trailing colon, unless a drive 
        /// letter (C or D, for example) is being defined or redefined. In no case is a trailing backslash allowed.</param>
        /// <returns><c>true</c> upon success, or <c>false</c> otherwise.</returns>
        /// <remarks>Call Marshal.GetLastWin32Error() to receive additional error information if this method fails.</remarks>
        public static bool DefineDosDevice(string targetPath, string deviceName)
        {
            return NativeMethods.DefineDosDeviceW(0, deviceName, targetPath);
        }

        /// <summary>
        /// Deletes an MS-DOS device name.
        /// </summary>
        /// <param name="deviceName">An MS-DOS device name string specifying the device the function is 
        /// deleting. The device name string must not have a trailing colon, unless a drive 
        /// letter (C or D, for example) is being deleted. In no case is a trailing backslash allowed.</param>
        /// <returns><c>true</c> upon success, or <c>false</c> otherwise.</returns>
        /// <remarks>Call Marshal.GetLastWin32Error() to receive additional error information if this method fails.</remarks>
        public static bool DeleteDosDevice(string deviceName)
        {
            return NativeMethods.DefineDosDeviceW(NativeMethods.DDD_REMOVE_DEFINITION, deviceName, null);
        }

        /// <summary>
        /// Retrieves information about MS-DOS device names. 
        /// The function can obtain the current mapping for a particular MS-DOS device name. 
        /// The function can also obtain a list of all existing MS-DOS device names.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An MS-DOS device name string specifying the target of the query. The device name cannot have a 
        /// trailing backslash. This parameter can be <see langword="null"/>. In that case, the QueryDosDevice function 
        /// will return an array of all existing MS-DOS device names</returns>
        /// <remarks>See documentation on MSDN for the Windows QueryDosDevice() method for more information.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string[] QueryDosDevice(string device)
        {
            uint returnSize = 0;
            int maxSize = 1024;

            List<string> l = new List<string>();

            while (returnSize == 0)
            {
                char[] buffer = new char[maxSize];

                returnSize = NativeMethods.QueryDosDeviceW(device, buffer, (uint)buffer.Length);
                int lastError = Marshal.GetLastWin32Error();
                if (returnSize > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < returnSize; i++)
                    {
                        if (buffer[i] != '\0')
                            sb.Append(buffer[i]);
                        else if (sb.Length > 0)
                        {
                            l.Add(sb.ToString());
                            sb.Length = 0;
                        }
                    }
                }
                else if (lastError == Win32Errors.ERROR_INSUFFICIENT_BUFFER)
                {
                    maxSize *= 2;
                }
                else
                {
                    Marshal.ThrowExceptionForHR(lastError);
                }
            }
            // Actually we never get to here
            return l.ToArray();
        }

        /// <summary>
        /// Retreives a list of all existing MS-DOS device names. 
        /// </summary>
        /// <returns>A list of all existing MS-DOS device names.</returns>
        /// <remarks>
        /// <para>This is equivalent to calling <c>QueryDosDevice(null)</c></para>
        /// <para>See documentation on MSDN for the Windows QueryDosDevice() method for more information.</para></remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string[] QueryAllDosDevices()
        {
            return QueryDosDevice(null);
        }

        /// <summary>
        /// Gets the shortest display name for the specified <paramref name="volumeName"/>.
        /// </summary>
        /// <param name="volumeName">The volume name.</param>
        /// <returns>The shortest display name for the specified volume found, or <see langword="null"/> if no display names were found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="volumeName"/> is a <see langword="null"/> reference</exception>
        /// <exception cref="Win32Exception">An error occured during a system call, such as the volume name specified was invalid or did not exist.</exception>
        /// <remarks>This method basically returns the shortest string returned by <see cref="GetVolumePathNamesForVolume"/></remarks>        
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string GetDisplayNameForVolume(string volumeName)
        {
            string[] volumeMountPoints = GetVolumePathNamesForVolume(volumeName);

            if (volumeMountPoints.Length == 0)
                return null;

            string smallestMountPoint = volumeMountPoints[0];
            for (int i = 1; i < volumeMountPoints.Length; i++)
            {
                if (volumeMountPoints[i].Length < smallestMountPoint.Length)
                    smallestMountPoint = volumeMountPoints[i];
            }
            return smallestMountPoint;
        }

        /// <summary>
        /// Retrieves a list of path names for the specified volume name.
        /// </summary>
        /// <param name="volumeName">The volume name.</param>
        /// <returns>An array containing the path names for the specified volume.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="volumeName"/> is a <see langword="null"/> reference</exception>
        /// <exception cref="System.IO.FileNotFoundException">The volume name specified was invalid, did not exist or was not ready.</exception>
        /// <remarks>For more information about this method see the MSDN documentation on GetVolumePathNamesForVolumeName().</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string[] GetVolumePathNamesForVolume(string volumeName)
        {
            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            uint requiredLength = 0;
            char[] buffer = new char[NativeMethods.MAX_PATH];

            if (!NativeMethods.GetVolumePathNamesForVolumeNameW(volumeName, buffer, (uint)buffer.Length, ref requiredLength))
            {
                // Not enough room in buffer perhaps? Try a bigger one
                buffer = new char[requiredLength];
                if (!NativeMethods.GetVolumePathNamesForVolumeNameW(volumeName, buffer, (uint)buffer.Length, ref requiredLength))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            List<string> displayNames = new List<string>();
            StringBuilder displayName = new StringBuilder();

            for (int i = 0; i < requiredLength; i++)
            {
                if (buffer[i] == '\0')
                {
                    if (displayName.Length > 0)
                        displayNames.Add(displayName.ToString());
                    displayName.Length = 0;
                }
                else
                {
                    displayName.Append(buffer[i]);
                }
            }

            return displayNames.ToArray();
        }

        /// <summary>
        /// Determines whether the specified volume name is a defined volume on the current computer.
        /// </summary>
        /// <param name="volumePath">A string representing the path to a volume (eg. "C:\", "D:", "P:\Mountpoint\Backup", "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\"). A trailing backslash is required.</param>
        /// <returns>
        /// 	<c>true</c> if the specified volume is a defined volume; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The trailing backslash is optional</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="volumePath"/> is a <see langword="null"/> reference</exception>
        /// <exception cref="Win32Exception">Upon error retreiving the volume name</exception>
        public static bool IsVolume(string volumePath)
        {
            if (volumePath == null)
                throw new ArgumentNullException("volumePath");

            if (volumePath.Length == 0)
                return false;

            StringBuilder volumeNameBuilder = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumePath, volumeNameBuilder, (uint)volumeNameBuilder.Capacity))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves the unique volume name for the specified volume mount point or root directory.
        /// </summary>
        /// <param name="mountPoint">The path of a volume mount point (with or without a trailing backslash, "\") or a drive letter indicating a root directory (eg. "C:" or "D:\"). A trailing backslash is required.</param>
        /// <returns>The unique volume name of the form "\\?\Volume{GUID}\" where GUID is the GUID that identifies the volume.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="mountPoint"/> is a <see langword="null"/> reference</exception>
        /// <exception cref="ArgumentException"><paramref name="mountPoint"/> is an empty string</exception>        
        /// <exception cref="Win32Exception">Upon error retreiving the volume name</exception>
        /// <remarks>See the MSDN documentation on the method GetVolumeNameForVolumeMountPoint() for more information.</remarks>
        public static string GetUniqueVolumeNameForVolumeMountPoint(string mountPoint)
        {
            if (mountPoint == null)
                throw new ArgumentNullException("mountPoint");

            if (mountPoint.Length == 0)
                throw new ArgumentException("Mount point must be non-empty");

            // Get the volume name alias. This may be different from the unique volume name in some
            // rare cases.
            StringBuilder volumeName = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(mountPoint, volumeName, (uint)volumeName.Capacity))
                throw new Win32Exception();

            // Get the unique volume name
            StringBuilder uniqueVolumeName = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumeName.ToString(), uniqueVolumeName, (uint)volumeName.Capacity))
                throw new Win32Exception();

            return uniqueVolumeName.ToString();
        }

        /// <summary>
        /// Retrieves the unique name of the volume mount point where the specified path is mounted.
        /// </summary>
        /// <param name="path">The input path. Both absolute and relative file and 
        /// directory names, for example ".", are acceptable in this path.
        /// If you specify a relative directory or file name without a volume qualifier, 
        /// GetUniqueVolumeNameForPath returns the drive letter of the boot volume. A trailing backslash is required.</param>
        /// <returns>The unique name of the volume mount point where the specified path is mounted</returns>
        /// <remarks>See the MSDN documentation on the method GetVolumePathName() for more information.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is a <see langword="null"/> reference</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is an empty string</exception>        
        /// <exception cref="Win32Exception">Upon error retreiving the volume name</exception>
        public static string GetUniqueVolumeNameForPath(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (path.Length == 0)
                throw new ArgumentException("Mount point must be non-empty");


            // Get the root path of the volume
            StringBuilder volumeRootPath = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumePathNameW(path, volumeRootPath, (uint)volumeRootPath.Capacity))
                throw new Win32Exception();

            // Get the volume name alias (might be different from the unique volume name in rare cases)
            StringBuilder volumeName = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumeRootPath.ToString(), volumeName, (uint)volumeName.Capacity))
                throw new Win32Exception();

            // Gte the unique volume name
            StringBuilder uniqueVolumeName = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumeName.ToString(), uniqueVolumeName, (uint)uniqueVolumeName.Capacity))
                throw new Win32Exception();

            return uniqueVolumeName.ToString();
        }

        /// <summary>
        ///  Retreives the Win32 device name from the volume name
        /// </summary>
        /// <param name="volumeName">Name of the volume. A trailing backslash is not allowed.</param>
        /// <returns>The Win32 device name from the volume name</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string GetDeviceForVolumeName(string volumeName)
        {
            if (volumeName == null)
                throw new ArgumentNullException("volumeName");

            if (volumeName.Length == 0)
                throw new ArgumentException("Volume name must be non-empty");

            // Eliminate the GLOBALROOT prefix if present
            const string globalRootPrefix = @"\\?\GLOBALROOT";

            if (volumeName.StartsWith(globalRootPrefix, StringComparison.OrdinalIgnoreCase))
                return volumeName.Substring(globalRootPrefix.Length);

            // If this is a volume name, get the device
            const string dosPrefix = @"\\?\";
            const string volumePrefix = @"\\?\Volume";

            if (volumeName.StartsWith(volumePrefix, StringComparison.OrdinalIgnoreCase))
            {
                // Isolate the DOS device for the volume name (in the format Volume{GUID})
                string dosDevice = volumeName.Substring(dosPrefix.Length);

                // Get the real device underneath
                return QueryDosDevice(dosDevice)[0];
            }

            return volumeName;
        }

        /// <summary>
        /// Retrieves information about the amount of space that is available on a disk volume, 
        /// which is the total amount of space, the total amount of free space, and the total amount of 
        /// free space available to the user that is associated with the calling thread.
        /// </summary>
        /// <param name="directory">A directory on the disk. 
        /// If this parameter is NULL, the function uses the root of the current disk. 
        /// If this parameter is a UNC name, it must include a trailing backslash, for example, "\\MyServer\MyShare\".
        /// This parameter does not have to specify the root directory on a disk. The function accepts any directory on a disk.
        /// 
        /// The calling application must have FILE_LIST_DIRECTORY access rights for this directory.
        /// </param>
        /// <returns>A <see cref="DiskSpaceInfo"/> object containing the requested information.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DiskSpaceInfo GetDiskFreeSpace(string directory)
        {
            UInt64 freeBytesAvailable, totalNumberOfBytes, totalNumberOfFreeBytes;
            if (!NativeMethods.GetDiskFreeSpaceEx(directory, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes))
                throw new Win32Exception();
            return new DiskSpaceInfo(freeBytesAvailable, totalNumberOfBytes, totalNumberOfFreeBytes);
        }
    }
}

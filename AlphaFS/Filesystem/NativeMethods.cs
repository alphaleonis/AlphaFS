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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using SecurityNativeMethods = Alphaleonis.Win32.Security.NativeMethods;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      #region Internal Utility

      #region Fields

      internal static readonly bool IsAtLeastWindows7 = OperatingSystemInfo.IsAtLeast(OsVersionName.Windows7);
      internal static readonly bool IsAtLeastWindowsVista = OperatingSystemInfo.IsAtLeast(OsVersionName.WindowsVista);

      #region DefaultFileBufferSize

      /// <summary>DefaultFileBufferSize = 4096; Default type buffer size used for reading and writing files.</summary>
      internal const int DefaultFileBufferSize = 4096;

      #endregion // DefaultFileBufferSize

      #region DefaultFileEncoding

      /// <summary>DefaultFileEncoding = Encoding.UTF8; Default type of Encoding used for reading and writing files.</summary>
      internal static readonly Encoding DefaultFileEncoding = Encoding.UTF8;

      #endregion // DefaultFileEncoding

      #region CopyOptionsFail

      /// <summary>Combination of <see cref="T:CopyOptions.FailIfExists"/> and <see cref="T:CopyOptions.NoBuffering"/></summary>
      internal const CopyOptions CopyOptionsFail = CopyOptions.FailIfExists | CopyOptions.NoBuffering;

      #endregion // CopyOptionsFail

      #region CopyOptionsNone

      /// <summary>Combination of <see cref="T:CopyOptions.None"/> and <see cref="T:CopyOptions.NoBuffering"/></summary>
      internal const CopyOptions CopyOptionsNone = CopyOptions.None | CopyOptions.NoBuffering;

      #endregion // CopyOptionsNone
      
      #region MaxPath

      /// <summary>MaxPath = 260
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. 
      /// </summary>
      internal const int MaxPath = 260;

      #endregion // MaxPath

      #region MaxPathUnicode

      /// <summary>MaxPathUnicode = 32000</summary>
      internal const int MaxPathUnicode = 32000;

      #endregion // MaxPathUnicode

      #region MoveOptionsReplace

      /// <summary>Combination of <see cref="T:MoveOptions.ReplaceExisting"/> and <see cref="T:MoveOptions.CopyAllowed"/></summary>
      internal const MoveOptions MoveOptionsReplace = MoveOptions.ReplaceExisting | MoveOptions.CopyAllowed;

      #endregion // MoveOptionsReplace

      #endregion // Fields

      #region Methods

      #region EnumMemberToList

      internal static IEnumerable<T> EnumMemberToList<T>()
      {
         Type enumType = typeof(T);

         // Can't use generic type constraints on value types, so have to do check like this.
         if (enumType.BaseType != typeof (Enum))
            throw new ArgumentException("T must be of type System.Enum");

         //Array enumValArray = Enum.GetValues(enumType);
         //List<T> enumValList = new List<T>(enumValArray.Length);
         IOrderedEnumerable<T> enumValArray = Enum.GetValues(enumType).Cast<T>().OrderBy(e => e.ToString());
         List<T> enumValList = new List<T>(enumValArray.Count());

         enumValList.AddRange(enumValArray.Select(val => (T) Enum.Parse(enumType, val.ToString())));
         return enumValList;
      }

      #endregion // EnumMemberToList

      #region GetEnumDescription

      [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
      internal static string GetEnumDescription(Enum value)
      {
         FieldInfo fi = value.GetType().GetField(value.ToString());
         DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);
         return attributes.Length > 0 ? attributes[0].Description : value.ToString();
      }

      #endregion // GetEnumDescription
      
      #region GetHighOrderDword

      internal static uint GetHighOrderDword(long highPart)
      {
         return (uint)((highPart >> 32) & 0xFFFFFFFF);
      }

      #endregion // GetHighOrderDword

      #region GetLowOrderDword

      internal static uint GetLowOrderDword(long lowPart)
      {
         return (uint)(lowPart & 0xFFFFFFFF);
      }

      #endregion // GetLowOrderDword

      #region GetStructure

      internal static T GetStructure<T>(int offset, IntPtr buffer) where T : struct
      {
         T structure = new T();
         return (T)Marshal.PtrToStructure(new IntPtr(buffer.ToInt64() + offset * Marshal.SizeOf(structure)), typeof(T));
      }

      #endregion // GetStructure

      #region IsValidHandle

      /// <summary>Check is the current handle is not null, not closed and not invalid.</summary>
      /// <param name="handle">The current handle to check.</param>
      /// <param name="throwException"><c>true</c> will throw an <exception cref="Resources.HandleInvalid"/>, <c>false</c> will not raise this exception..</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <exception cref="ArgumentException"></exception>
      internal static bool IsValidHandle(SafeHandle handle, bool throwException = true)
      {
         if (handle == null || handle.IsClosed || handle.IsInvalid)
         {
            if (throwException)
               throw new ArgumentException(Resources.HandleInvalid);

            return false;
         }

         return true;
      }

      /// <summary>Check is the current handle is not null, not closed and not invalid.</summary>
      /// <param name="handle">The current handle to check.</param>
      /// <param name="lastError">The result of Marshal.GetLastWin32Error()</param>
      /// <param name="throwException"><c>true</c> will throw an <exception cref="Resources.HandleInvalidWin32Error"/>, <c>false</c> will not raise this exception..</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <exception cref="ArgumentException"></exception>
      internal static bool IsValidHandle(SafeHandle handle, int lastError, bool throwException = true)
      {
         if (handle == null || handle.IsClosed || handle.IsInvalid)
         {
            if (throwException)
               throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.HandleInvalidWin32Error, lastError));

            return false;
         }

         return true;
      }


      #endregion // IsValidHandle

      #region LuidToLong

      internal static ulong LuidToLong(Luid luid)
      {
         ulong high = (((ulong)luid.HighPart) << 32);
         ulong low = (((ulong)luid.LowPart) & 0x00000000FFFFFFFF);
         return high | low;
      }

      #endregion // LuidToLong

      #region LongToLuid

      internal static Luid LongToLuid(ulong lluid)
      {
         return new Luid { HighPart = (uint)(lluid >> 32), LowPart = (uint)(lluid & 0xFFFFFFFF) };
      }

      #endregion // LongToLuid

      #region SetErrorMode

      /// <summary>Controls whether the system will handle the specified types of serious errors or whether the process will handle them.</summary>
      /// <returns>The return value is the previous state of the error-mode bit attributes.</returns>
      /// <remarks>
      /// Because the error mode is set for the entire process, you must ensure that multi-threaded applications
      /// do not set different error-mode attributes. Doing so can lead to inconsistent error handling.
      /// </remarks>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      private static extern ErrorMode SetErrorMode(ErrorMode uMode);

      #endregion // SetErrorMode

      #region SetThreadErrorMode

      /// <summary>Controls whether the system will handle the specified types of serious errors or whether the calling thread will handle them.</summary>
      /// <returns>The return value is the previous state of the error-mode bit attributes.</returns>
      /// <remarks>
      /// Because the error mode is set for the entire process, you must ensure that multi-threaded applications
      /// do not set different error-mode attributes. Doing so can lead to inconsistent error handling.
      /// </remarks>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows 7 [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 R2 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool SetThreadErrorMode(ErrorMode dwNewMode, [MarshalAs(UnmanagedType.U4)] out ErrorMode lpOldMode);

      #endregion // SetThreadErrorMode

      #region ToLong

      internal static long ToLong(uint highPart, uint lowPart)
      {
         return (((long)highPart) << 32) | (((long)lowPart) & 0xFFFFFFFF);
      }

      #endregion // ToLong

      #region UnitSizeToText

      /// <summary>Convert a number of type T to string with UnitSize or Percentage suffixed.</summary>
      internal static string UnitSizeToText<T>(T numberOfBytes, bool usePercent = false)
      {
         string template = "{0:0.00}{1}";
         string sfx = "bytes";

         double bytes = Convert.ToDouble(numberOfBytes, CultureInfo.InvariantCulture);

         if (bytes >= 1152921504606846976) { sfx = "EB"; bytes /= 1152921504606846976; }
         else if (bytes >= 1125899906842624) { sfx = "PB"; bytes /= 1125899906842624; }
         else if (bytes >= 1099511627776) { sfx = "TB"; bytes /= 1099511627776; }
         else if (bytes >= 1073741824) { sfx = "GB"; bytes /= 1073741824; }
         else if (bytes >= 1048576) { sfx = "MB"; bytes /= 1048576; }
         else if (bytes >= 1024) { sfx = "KB"; bytes /= 1024; }

         else if (!usePercent)
            // Will return "512 bytes" instead of "512,00 bytes".
            template = "{0:0}{1}";

         return string.Format(CultureInfo.CurrentCulture, template, bytes, usePercent ? "%" : " " + sfx);
      }

      /// <summary>Calculates a percentage value.</summary>
      /// <param name="currentValue"></param>
      /// <param name="minimumValue"></param>
      /// <param name="maximumValue"></param>
      internal static double PercentCalculate(double currentValue, double minimumValue, double maximumValue)
      {
         return (currentValue < 0 || maximumValue <= 0) ? 0 : currentValue * 100 / (maximumValue - minimumValue);
      }

      #endregion // UnitSizeToText

      #endregion // Methods
      
      #endregion // Internal Utility
      

      #region Backup/Stream Management

      #region BackupRead

      /// <summary>The BackupRead function can be used to back up a file or directory, including the security information.
      /// The function reads data associated with a specified file or directory into a buffer, which can then be written to the backup medium using the WriteFile function.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero, indicating that an I/O error occurred. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>This function is not intended for use in backing up files encrypted under the Encrypted File System. Use ReadEncryptedFileRaw for that purpose.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BackupRead(SafeFileHandle hFile, SafeHandle lpBuffer, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToRead, [MarshalAs(UnmanagedType.U4)] out uint lpNumberOfBytesRead, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, out IntPtr lpContext);

      #endregion // BackupRead

      #region BackupSeek

      /// <summary>The BackupSeek function seeks forward in a data stream initially accessed by using the BackupRead or BackupWrite function.</summary>
      /// <returns>
      /// If the function could seek the requested amount, the function returns a nonzero value.
      /// If the function could not seek the requested amount, the function returns zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// Applications use the BackupSeek function to skip portions of a data stream that cause errors.
      /// This function does not seek across stream headers. For example, this function cannot be used to skip the stream name.
      /// If an application attempts to seek past the end of a substream, the function fails, the lpdwLowByteSeeked and lpdwHighByteSeeked parameters
      /// indicate the actual number of bytes the function seeks, and the file position is placed at the start of the next stream header.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BackupSeek(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U4)] uint dwLowBytesToSeek, [MarshalAs(UnmanagedType.U4)] uint dwHighBytesToSeek, [MarshalAs(UnmanagedType.U4)] out uint lpdwLowBytesSeeked, [MarshalAs(UnmanagedType.U4)] out uint lpdwHighBytesSeeked, out IntPtr lpContext);

      #endregion // BackupSeek

      #region BackupWrite

      /// <summary>The BackupWrite function can be used to restore a file or directory that was backed up using BackupRead.
      /// Use the ReadFile function to get a stream of data from the backup medium, then use BackupWrite to write the data to the specified file or directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero, indicating that an I/O error occurred. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>This function is not intended for use in restoring files encrypted under the Encrypted File System. Use WriteEncryptedFileRaw for that purpose.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BackupWrite(SafeFileHandle hFile, SafeGlobalMemoryBufferHandle lpBuffer, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToWrite, [MarshalAs(UnmanagedType.U4)] out uint lpNumberOfBytesWritten, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, out IntPtr lpContext);

      #endregion // BackupWrite

      #endregion // Backup/Stream Management


      #region Device Management

      #region CM_Connect_Machine

      /// <summary>The CM_Connect_Machine function creates a connection to a remote machine.</summary>
      /// <returns>If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      /// <remarks>Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.</remarks>
      /// <remarks>You cannot access remote machines when running on these versions of Windows.</remarks>
      /// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Connect_Machine([MarshalAs(UnmanagedType.LPWStr)] string uncServerName, ref SafeCmConnectMachineHandle phMachine);

      #endregion // CM_Connect_Machine

      #region CM_Get_Device_ID_Ex

      /// <summary>The CM_Get_Device_ID_Ex function retrieves the device instance ID for a specified device instance on a local or a remote machine.</summary>
      /// <returns>If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Get_Device_ID_Ex([MarshalAs(UnmanagedType.U4)] uint dnDevInst, SafeGlobalMemoryBufferHandle buffer, [MarshalAs(UnmanagedType.U4)] uint bufferLen, [MarshalAs(UnmanagedType.U4)] uint ulFlags, SafeCmConnectMachineHandle hMachine);

      #endregion // CM_Get_Device_ID_Ex

      #region CM_Disconnect_Machine

      /// <summary>The CM_Disconnect_Machine function removes a connection to a remote machine.</summary>
      /// <returns>If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      /// <remarks>Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.</remarks>
      /// <remarks>You cannot access remote machines when running on these versions of Windows. </remarks>
      /// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff538005%28v=vs.85%29.aspx</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Disconnect_Machine(IntPtr hMachine);

      #endregion // CM_Disconnect_Machine

      #region CM_Get_Parent_Ex

      /// <summary>The CM_Get_Parent_Ex function obtains a device instance handle to the parent node of a specified device node (devnode) in a local or a remote machine's device tree.</summary>
      /// <returns>If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      /// <remarks>Using this function to access remote machines is not supported</remarks>
      /// <remarks>beginning with Windows 8 and Windows Server 2012, as this functionality has been removed.</remarks>
      /// <remarks>http://msdn.microsoft.com/en-us/library/windows/hardware/ff538615%28v=vs.85%29.aspx</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Get_Parent_Ex([MarshalAs(UnmanagedType.U4)] out uint pdnDevInst, [MarshalAs(UnmanagedType.U4)] uint dnDevInst, [MarshalAs(UnmanagedType.U4)] uint ulFlags, SafeCmConnectMachineHandle hMachine);

      #endregion // CM_Get_Parent_Ex

      #region DeviceIoControl

      /// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeviceIoControl(SafeFileHandle hDevice, [MarshalAs(UnmanagedType.U4)] uint dwIoControlCode, IntPtr lpInBuffer, [MarshalAs(UnmanagedType.U4)] uint nInBufferSize, SafeGlobalMemoryBufferHandle lpOutBuffer, [MarshalAs(UnmanagedType.U4)] uint nOutBufferSize, [MarshalAs(UnmanagedType.U4)] out uint lpBytesReturned, IntPtr lpOverlapped);

      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeviceIoControl(SafeFileHandle hDevice, [MarshalAs(UnmanagedType.U4)] uint dwIoControlCode, [MarshalAs(UnmanagedType.AsAny)] object lpInBuffer, [MarshalAs(UnmanagedType.U4)] uint nInBufferSize, [MarshalAs(UnmanagedType.AsAny)] [Out] object lpOutBuffer, [MarshalAs(UnmanagedType.U4)] uint nOutBufferSize, [MarshalAs(UnmanagedType.U4)] out uint lpBytesReturned, IntPtr lpOverlapped);

      #endregion // DeviceIoControl

      #region SetupDiDestroyDeviceInfoList

      /// <summary>The SetupDiDestroyDeviceInfoList function deletes a device information set and frees all associated memory.</summary>
      /// <returns>The function returns TRUE if it is successful. Otherwise, it returns FALSE and the logged error can be retrieved with a call to GetLastError.</returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

      #endregion // SetupDiDestroyDeviceInfoList

      #region SetupDiEnumDeviceInterfaces

      /// <summary>The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that are contained in a device information set.</summary>
      /// <returns>SetupDiEnumDeviceInterfaces returns TRUE if the function completed without error. If the function completed with an error, FALSE is returned and the error code for the failure can be retrieved by calling GetLastError.</returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiEnumDeviceInterfaces(SafeHandle hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, [MarshalAs(UnmanagedType.U4)] uint memberIndex, ref SpDeviceInterfaceData deviceInterfaceData);

      #endregion // SetupDiEnumDeviceInterfaces

      #region SetupDiGetClassDevsEx

      /// <summary>The SetupDiGetClassDevsEx function returns a handle to a device information set that contains requested device information elements for a local or a remote computer.</summary>
      /// <returns>
      /// If the operation succeeds, SetupDiGetClassDevsEx returns a handle to a device information set that contains all installed devices that matched the supplied parameters.
      /// If the operation fails, the function returns INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      internal static extern SafeSetupDiClassDevsExHandle SetupDiGetClassDevsEx(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, [MarshalAs(UnmanagedType.U4)] SetupDiGetClassDevsExFlags devsExFlags, IntPtr deviceInfoSet, [MarshalAs(UnmanagedType.LPWStr)] string machineName, IntPtr reserved);

      #endregion // SetupDiGetClassDevsEx

      #region SetupDiGetDeviceInterfaceDetail

      /// <summary>The SetupDiGetDeviceInterfaceDetail function returns details about a device interface.</summary>
      /// <returns>
      /// SetupDiGetDeviceInterfaceDetail returns TRUE if the function completed without error.
      /// If the function completed with an error, FALSE is returned and the error code for the failure can be retrieved by calling GetLastError.
      /// </returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiGetDeviceInterfaceDetail(SafeHandle hDevInfo, ref SpDeviceInterfaceData deviceInterfaceData, ref SpDeviceInterfaceDetailData deviceInterfaceDetailData, [MarshalAs(UnmanagedType.U4)] uint deviceInterfaceDetailDataSize, IntPtr requiredSize, ref SpDeviceInfoData deviceInfoData);

      #endregion // SetupDiGetDeviceInterfaceDetail

      #region SetupDiGetDeviceRegistryProperty

      /// <summary>The SetupDiGetDeviceRegistryProperty function retrieves a specified Plug and Play device property.</summary>
      /// <returns>
      /// SetupDiGetDeviceRegistryProperty returns TRUE if the call was successful.
      /// Otherwise, it returns FALSE and the logged error can be retrieved by making a call to GetLastError.
      /// SetupDiGetDeviceRegistryProperty returns the ERROR_INVALID_DATA error code if the requested property does not exist for a device or if the property data is not valid.
      /// </returns>
      /// <remarks>Available in Microsoft Windows 2000 and later versions of Windows.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiGetDeviceRegistryProperty(SafeHandle deviceInfoSet, ref SpDeviceInfoData deviceInfoData, SetupDiGetDeviceRegistryPropertyEnum property, [MarshalAs(UnmanagedType.U4)] out uint propertyRegDataType, SafeGlobalMemoryBufferHandle propertyBuffer, [MarshalAs(UnmanagedType.U4)] uint propertyBufferSize, IntPtr requiredSize);

      #endregion // SetupDiGetDeviceRegistryProperty

      #endregion // Device Management


      #region Directory Management

      #region CreateDirectory

      /// <summary>Creates a new directory. 
      /// If the underlying file system supports security on files and directories, the function applies a specified security descriptor to the new directory.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateDirectoryW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool CreateDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes);

      #endregion // CreateDirectory

      #region CreateDirectoryEx

      /// <summary>Creates a new directory with the attributes of a specified template directory.
      /// If the underlying file system supports security on files and directories, the function applies a specified security descriptor to the new directory.
      /// The new directory retains the other attributes of the specified template directory.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateDirectoryExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool CreateDirectoryEx([MarshalAs(UnmanagedType.LPWStr)] string lpTemplateDirectory, [MarshalAs(UnmanagedType.LPWStr)] string lpPathName, Security.NativeMethods.SecurityAttributes lpSecurityAttributes);

      #endregion // CreateDirectoryEx

      #region CreateDirectoryTransacted

      /// <summary>Creates a new directory as a transacted operation, with the attributes of a specified template directory.
      /// If the underlying file system supports security on files and directories, the function applies a specified security descriptor to the new directory.
      /// The new directory retains the other attributes of the specified template directory.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateDirectoryTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateDirectoryTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpTemplateDirectory, [MarshalAs(UnmanagedType.LPWStr)] string lpNewDirectory, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes, SafeHandle hTransaction);

      #endregion // CreateDirectoryTransacted

      #region RemoveDirectory

      /// <summary>Deletes an existing empty directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>The RemoveDirectory function marks a directory for deletion on close. Therefore, the directory is not removed until the last handle to the directory is closed.</remarks>
      /// <remarks>
      /// RemoveDirectory removes a directory junction, even if the contents of the target are not empty; the function removes directory
      /// junctions regardless of the state of the target object. For more information on junctions, see Hard Links and Junctions.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RemoveDirectoryW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool RemoveDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);

      #endregion // RemoveDirectory

      #region RemoveDirectoryTransacted

      /// <summary>Deletes an existing empty directory as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// RemoveDirectory removes a directory junction, even if the contents of the target are not empty; the function removes directory
      /// junctions regardless of the state of the target object. For more information on junctions, see Hard Links and Junctions.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RemoveDirectoryTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool RemoveDirectoryTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, SafeHandle hTransaction);

      #endregion // RemoveDirectoryTransacted

      #endregion // Directory Management


      #region Disk Management

      #region GetDiskFreeSpace

      /// <summary>Retrieves information about the specified disk, including the amount of free space on the disk.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// Symbolic link behavior; if the path points to a symbolic link, the operation is performed on the target.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetDiskFreeSpaceW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName, out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters, out uint lpTotalNumberOfClusters);

      #endregion // GetDiskFreeSpace

      #region GetDiskFreeSpaceEx

      /// <summary>Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space,
      /// the total amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// Symbolic link behavior; if the path points to a symbolic link, the operation is performed on the target.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetDiskFreeSpaceExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetDiskFreeSpaceEx([MarshalAs(UnmanagedType.LPWStr)] string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

      #endregion // GetDiskFreeSpaceEx

      #region GetDriveType

      /// <summary>Determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive.</summary>
      /// <returns>The return value specifies the type of drive, which can be one of the following <see cref="T:DriveType"/> values.</returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetDriveTypeW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal extern static DriveType GetDriveType([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName);

      #endregion // GetDriveType

      #region GetLogicalDrives

      /// <summary>Retrieves a bitmask representing the currently available disk drives.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a bitmask representing the currently available disk drives.
      /// Bit position 0 (the least-significant bit) is drive A, bit position 1 is drive B, bit position 2 is drive C, and so on.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetLogicalDrives();

      #endregion // GetLogicalDrives

      #endregion // Disk Management


      #region File Management

      #region AssocXxx

      #region AssocCreate

      /// <summary>Returns a pointer to an IQueryAssociations object.</summary>
      /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint AssocCreate(Guid clsid, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IQueryAssociations ppv);

      #endregion // AssocCreate

      #region AssocQueryString

      /// <summary>Searches for and retrieves a file or protocol association-related string from the registry.</summary>
      /// <returns>Return value Type: HRESULT. Returns a standard COM error value, including the following: S_OK, E_POINTER and S_FALSE.</returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "AssocQueryStringW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint AssocQueryString(Shell32.AssociationAttributes flags, Shell32.AssociationString str, [MarshalAs(UnmanagedType.LPWStr)] string pszAssoc, [MarshalAs(UnmanagedType.LPWStr)] string pszExtra, StringBuilder pszOut, [MarshalAs(UnmanagedType.U4)] out uint pcchOut);

      #endregion // AssocQueryString

      #region IQueryAssociations

      internal static readonly Guid ClsidQueryAssociations = new Guid("A07034FD-6CAA-4954-AC3F-97A27216F98A");
      internal const string QueryAssociationsGuid = "C46CA590-3C3F-11D2-BEE6-0000F805CA57";

      /// <summary>Exposes methods that simplify the process of retrieving information stored in the registry in association with defining a file type or protocol and associating it with an application.</summary>
      [Guid(QueryAssociationsGuid), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
      internal interface IQueryAssociations
      {
         /// <summary>Initializes the IQueryAssociations interface and sets the root key to the appropriate ProgID.</summary>
         /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
         /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         void Init(Shell32.AssociationAttributes flags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssoc, IntPtr hkProgid, IntPtr hwnd);
         //[return: MarshalAs(UnmanagedType.U4)]
         //uint Init(Shell32.AssociationAttributes flags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssoc, IntPtr hkProgid, IntPtr hwnd);

         /// <summary>Searches for and retrieves a file or protocol association-related string from the registry.</summary>
         /// <returns>Returns a standard COM error value, including the following: S_OK, E_POINTER, S_FALSE</returns>
         /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         void GetString(Shell32.AssociationAttributes flags, Shell32.AssociationString str, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, StringBuilder pwszOut, [MarshalAs(UnmanagedType.I4)] out int pcchOut);
         //[return: MarshalAs(UnmanagedType.U4)]
         //void GetString(Shell32.AssociationAttributes flags, Shell32.AssociationString str, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, StringBuilder pwszOut, [MarshalAs(UnmanagedType.I4)] out int pcchOut);

         ///// <summary>Searches for and retrieves a file or protocol association-related key from the registry.</summary>
         ///// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
         ///// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         ///// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         //[return: MarshalAs(UnmanagedType.U4)]
         //uint GetKey(Shell32.AssociationAttributes flags, Shell32.AssociationKey str, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, out UIntPtr phkeyOut);

         ///// <summary>Searches for and retrieves file or protocol association-related binary data from the registry.</summary>
         ///// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
         ///// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         ///// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         //[return: MarshalAs(UnmanagedType.U4)]
         //uint GetData(Shell32.AssociationAttributes flags, Shell32.AssociationData data, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] out byte[] pvOut, [MarshalAs(UnmanagedType.I4)] out int pcbOut);

         ///// <summary>This method is not implemented.</summary>
         //void GetEnum();
      }

      #endregion // IQueryAssociations

      #endregion // AssocXxx

      #region CopyFileEx

      /// <summary>Copies an existing file to a new file, notifying the application of its progress through a callback function.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>This function fails with ERROR_ACCESS_DENIED if the destination file already exists and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.</remarks>
      /// <remarks>This function preserves extended attributes, OLE structured storage, NTFS file system alternate data streams, security resource attributes, and file attributes.</remarks>
      /// <remarks>Windows 7, Windows Server 2008 R2, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  Security resource attributes (ATTRIBUTE_SECURITY_INFORMATION) for the existing file are not copied to the new file until Windows 8 and Windows Server 2012.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CopyFileExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CopyFileEx([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, CopyMoveProgressDelegate lpProgressRoutine, IntPtr lpData, out int pbCancel, CopyOptions dwCopyFlags);

      #endregion // CopyFileEx

      #region CopyFileTransacted

      /// <summary>Copies an existing file to a new file as a transacted operation, notifying the application of its progress through a callback function.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>This function fails with ERROR_ACCESS_DENIED if the destination file already exists and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.</remarks>
      /// <remarks>This function preserves extended attributes, OLE structured storage, NTFS file system alternate data streams, security attributes, and file attributes.</remarks>
      /// <remarks>Windows 7, Windows Server 2008 R2, Windows Server 2008, and Windows Vista:  Security resource attributes (ATTRIBUTE_SECURITY_INFORMATION) for the existing file are not copied to the new file until Windows 8 and Windows Server 2012.</remarks>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CopyFileTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CopyFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, CopyMoveProgressDelegate lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.I4)] out int pbCancel, CopyOptions dwCopyFlags, SafeHandle hTransaction);

      #endregion // CopyFileTransacted

      #region CopyMoveProgressDelegate

      /// <summary>Delegate used by CopyFileEx()/CopyFileTransacted() and MoveFileWithProgress()/MoveFileTransacted() to report progress about the operation.</summary>
      internal delegate CopyMoveProgressResult CopyMoveProgressDelegate(long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, [MarshalAs(UnmanagedType.U4)] uint streamNumber, CopyMoveProgressCallbackReason callbackReason, IntPtr sourceFile, IntPtr destinationFile, IntPtr data);

      #endregion // CopyMoveProgressDelegate

      #region CreateFile

      /// <summary>Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.</summary>
      /// <returns>
      /// If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.
      /// If the function fails, the return value is Win32Errors.ERROR_INVALID_HANDLE. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateFileW")]
      internal static extern SafeFileHandle CreateFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] EFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

      #endregion // CreateFile

      #region CreateFileMapping

      /// <summary>Creates or opens a named or unnamed file mapping object for a specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a handle to the newly created file mapping object.
      /// If the function fails, the return value is <c>null</c>.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode, EntryPoint = "CreateFileMappingW")]
      internal static extern SafeFileHandle CreateFileMapping(SafeFileHandle hFile, SafeHandle lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] uint flProtect, [MarshalAs(UnmanagedType.U4)] uint dwMaximumSizeHigh, [MarshalAs(UnmanagedType.U4)] uint dwMaximumSizeLow, [MarshalAs(UnmanagedType.LPWStr)] string lpName);

      #endregion // CreateFileMapping

      #region CreateFileTransacted

      /// <summary>Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.</summary>
      /// <returns>
      /// If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.
      /// If the function fails, the return value is Win32Errors.ERROR_INVALID_HANDLE". To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateFileTransactedW")]
      internal static extern SafeFileHandle CreateFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] EFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile, SafeHandle hTransaction, IntPtr pusMiniVersion, IntPtr pExtendedParameter);

      #endregion // CreateFileTransacted

      #region CreateHardLink

      /// <summary>Establishes a hard link between an existing file and a new file. 
      /// This function is only supported on the NTFS file system, and only for files, not directories.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateHardLinkW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateHardLink([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, IntPtr lpSecurityAttributes);

      #endregion // CreateHardLink

      #region CreateHardLinkTransacted

      /// <summary>Establishes a hard link between an existing file and a new file as a transacted operation.
      /// This function is only supported on the NTFS file system, and only for files, not directories.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateHardLinkTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateHardLinkTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, IntPtr lpSecurityAttributes, SafeHandle hTransaction);

      #endregion // CreateHardLinkTransacted

      #region CreateSymbolicLink

      /// <summary>Creates a symbolic link.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateSymbolicLinkW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateSymbolicLink([MarshalAs(UnmanagedType.LPWStr)] string lpSymlinkFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetFileName, [MarshalAs(UnmanagedType.U4)] SymbolicLinkTarget dwFlags);

      #endregion // CreateSymbolicLink

      #region CreateSymbolicLinkTransacted

      /// <summary>Creates a symbolic link as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateSymbolicLinkTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateSymbolicLinkTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpSymlinkFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetFileName, [MarshalAs(UnmanagedType.U4)] SymbolicLinkTarget dwFlags, SafeHandle hTransaction);

      #endregion // CreateSymbolicLinkTransacted

      #region DecryptFile

      /// <summary>Decrypts an encrypted file or directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// The DecryptFile function requires exclusive access to the file being decrypted, and will fail if another process is using the file.
      /// If the file is not encrypted, DecryptFile simply returns a nonzero value, which indicates success.
      /// If lpFileName specifies a read-only file, the function fails and GetLastError returns ERROR_FILE_READ_ONLY.
      /// If lpFileName specifies a directory that contains a read-only file, the functions succeeds but the directory is not decrypted.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DecryptFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DecryptFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint dwReserved);

      #endregion // DecryptFile

      #region DeleteFile

      /// <summary>Deletes an existing file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>If an application attempts to delete a file that does not exist, the DeleteFile function fails with ERROR_FILE_NOT_FOUND.</remarks>
      /// <remarks>If the file is a read-only file, the function fails with ERROR_ACCESS_DENIED.</remarks>
      /// <remarks>If the path points to a symbolic link, the symbolic link is deleted, not the target. To delete a target, you must call CreateFile and specify FILE_FLAG_DELETE_ON_CLOSE.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeleteFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeleteFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

      #endregion // DeleteFile

      #region DeleteFileTransacted

      /// <summary>Deletes an existing file as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeleteFileTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeleteFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, SafeHandle hTransaction);

      #endregion // DeleteFileTransacted

      #region EncryptFile

      /// <summary>Encrypts a file or directory. All data streams in a file are encrypted. All new files created in an encrypted directory are encrypted.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// The EncryptFile function requires exclusive access to the file being encrypted, and will fail if another process is using the file.
      /// If the file is already encrypted, EncryptFile simply returns a nonzero value, which indicates success. If the file is compressed,
      /// EncryptFile will decompress the file before encrypting it. If lpFileName specifies a read-only file, the function fails and GetLastError
      /// returns ERROR_FILE_READ_ONLY. If lpFileName specifies a directory that contains a read-only file, the functions succeeds but the directory is not encrypted.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "EncryptFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool EncryptFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

      #endregion // EncryptFile

      #region EncryptionDisable

      /// <summary>Disables or enables encryption of the specified directory and the files in it.
      /// It does not affect encryption of subdirectories below the indicated directory. 
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// EncryptionDisable() disables encryption of directories and files.
      /// It does not affect the visibility of files with the FILE_ATTRIBUTE_SYSTEM attribute set.
      /// This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0|1"
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool EncryptionDisable([MarshalAs(UnmanagedType.LPWStr)] string dirPath, [MarshalAs(UnmanagedType.Bool)] bool disable);

      #endregion // EncryptionDisable

      #region FileEncryptionStatus

      /// <summary>Retrieves the encryption status of the specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FileEncryptionStatusW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FileEncryptionStatus([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, out FileEncryptionStatus lpStatus);

      #endregion // FileEncryptionStatus

      #region FindClose

      /// <summary>Closes a file search handle opened by the FindFirstFile, FindFirstFileEx, FindFirstFileNameW, FindFirstFileNameTransactedW, FindFirstFileTransacted, FindFirstStreamTransactedW, or FindFirstStreamW functions.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindClose(IntPtr hFindFile);

      #endregion // FindClose

      #region FindFirstFileEx

      /// <summary>Searches a directory for a file or subdirectory with a name and attributes that match those specified.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a search handle used in a subsequent call to FindNextFile or FindClose, and the lpFindFileData parameter contains information about the first file or directory found.
      /// If the function fails or fails to locate files from the search string in the lpFileName parameter, the return value is INVALID_HANDLE_VALUE and the contents of lpFindFileData are indeterminate. To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>A trailing backslash is not allowed and will be removed.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileExW")]
      internal static extern SafeFindFileHandle FindFirstFileEx([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, FindExInfoLevels fInfoLevelId, out Win32FindData lpFindFileData, FindExSearchOps fSearchOp, IntPtr lpSearchFilter, FindExAdditionalFlags dwAdditionalFlags);

      #endregion // FindFirstFileEx

      #region FindFirstFileTransacted

      /// <summary>Searches a directory for a file or subdirectory with a name that matches a specific name as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a search handle used in a subsequent call to FindNextFile or FindClose, and the lpFindFileData parameter contains information about the first file or directory found.
      /// If the function fails or fails to locate files from the search string in the lpFileName parameter, the return value is INVALID_HANDLE_VALUE and the contents of lpFindFileData are indeterminate. To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>A trailing backslash is not allowed and will be removed.</remarks>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileTransactedW")]
      internal static extern SafeFindFileHandle FindFirstFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, FindExInfoLevels fInfoLevelId, out Win32FindData lpFindFileData, FindExSearchOps fSearchOp, IntPtr lpSearchFilter, FindExAdditionalFlags dwAdditionalFlags, SafeHandle hTransaction);

      #endregion // FindFirstFileTransacted
      
      #region FindFirstFileName

      /// <summary>Creates an enumeration of all the hard links to the specified file. 
      /// The FindFirstFileNameW function returns a handle to the enumeration that can be used on subsequent calls to the FindNextFileNameW function.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is a search handle that can be used with the FindNextFileNameW function or closed with the FindClose function.
      /// If the function fails, the return value is INVALID_HANDLE_VALUE (0xffffffff). To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileNameW")]
      internal static extern SafeFindFileHandle FindFirstFileName([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint dwFlags, [MarshalAs(UnmanagedType.U4)] out uint stringLength, StringBuilder linkName);

      #endregion // FindFirstFileName

      #region FindFirstFileNameTransacted

      /// <summary>Creates an enumeration of all the hard links to the specified file as a transacted operation. The function returns a handle to the enumeration that can be used on subsequent calls to the FindNextFileNameW function.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a search handle that can be used with the FindNextFileNameW function or closed with the FindClose function.
      /// If the function fails, the return value is INVALID_HANDLE_VALUE (0xffffffff). To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileNameTransactedW")]
      internal static extern SafeFindFileHandle FindFirstFileNameTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint dwFlags, [MarshalAs(UnmanagedType.U4)] out uint stringLength, StringBuilder linkName, SafeHandle hTransaction);

      #endregion // FindFirstFileNameTransacted

      #region FindNextFile

      /// <summary>Continues a file search from a previous call to the FindFirstFile, FindFirstFileEx, or FindFirstFileTransacted functions.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero and the lpFindFileData parameter contains information about the next file or directory found.
      /// If the function fails, the return value is zero and the contents of lpFindFileData are indeterminate. To get extended error information, call the GetLastError function.
      /// If the function fails because no more matching files can be found, the GetLastError function returns ERROR_NO_MORE_FILES.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindNextFile(SafeFindFileHandle hFindFile, out Win32FindData lpFindFileData);

      #endregion // FindNextFile

      #region FindNextFileName

      /// <summary>Continues enumerating the hard links to a file using the handle returned by a successful call to the FindFirstFileName function.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
      /// If no matching files can be found, the GetLastError function returns ERROR_HANDLE_EOF.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextFileNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindNextFileName(SafeFindFileHandle hFindStream, [MarshalAs(UnmanagedType.U4)] out uint stringLength, StringBuilder linkName);

      #endregion // FindNextFileName

      #region FlushFileBuffers

      /// <summary>Flushes the buffers of a specified file and causes all buffered data to be written to a file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FlushFileBuffers(SafeFileHandle hFile);

      #endregion // FlushFileBuffers

      #region GetCompressedFileSize

      /// <summary>Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is the low-order DWORD of the actual number of bytes of disk storage used to store the specified file, and if lpFileSizeHigh is non-NULL, the function puts the high-order DWORD of that actual value into the DWORD pointed to by that parameter. This is the compressed file size for compressed files, the actual file size for noncompressed files.
      /// If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE. To get extended error information, call GetLastError.
      /// If the return value is INVALID_FILE_SIZE and lpFileSizeHigh is non-NULL, an application must call GetLastError to determine whether the function has succeeded (value is NO_ERROR) or failed (value is other than NO_ERROR).
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetCompressedFileSizeW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetCompressedFileSize([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

      #endregion // GetCompressedFileSize

      #region GetCompressedFileSizeTransacted

      /// <summary>Retrieves the actual number of bytes of disk storage used to store a specified file as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is the low-order DWORD of the actual number of bytes of disk storage used to store the specified file, and if lpFileSizeHigh is non-NULL, the function puts the high-order DWORD of that actual value into the DWORD pointed to by that parameter. This is the compressed file size for compressed files, the actual file size for noncompressed files.
      /// If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE. To get extended error information, call GetLastError.
      /// If the return value is INVALID_FILE_SIZE and lpFileSizeHigh is non-NULL, an application must call GetLastError to determine whether the function has succeeded (value is NO_ERROR) or failed (value is other than NO_ERROR).
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetCompressedFileSizeTransactedW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetCompressedFileSizeTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh, SafeHandle hTransaction);

      #endregion // GetCompressedFileSizeTransacted

      #region GetFileAttributesEx

      /// <summary>Retrieves file system attributes for a specified file or directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value contains the attributes of the specified file or directory. For a list of attribute values and their descriptions, see <see cref="T:FileAttributes"/> constants.
      /// If the function fails, the return value is INVALID_FILE_ATTRIBUTES. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFileAttributesW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern FileAttributes GetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

      #endregion // GetFileAttributesEx

      #region GetFileAttributesEx

      /// <summary>Retrieves attributes for a specified file or directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// The GetFileAttributes function retrieves file system attribute information. GetFileAttributesEx can obtain other sets of file or directory attribute information.
      /// Currently, GetFileAttributesEx retrieves a set of standard attributes that is a superset of the file system attribute information.
      /// When the GetFileAttributesEx function is called on a directory that is a mounted folder, it returns the attributes of the directory, not those of
      /// the root directory in the volume that the mounted folder associates with the directory. To obtain the attributes of the associated volume,
      /// call GetVolumeNameForVolumeMountPoint to obtain the name of the associated volume. Then use the resulting name in a call to GetFileAttributesEx.
      /// The results are the attributes of the root directory on the associated volume.
      /// Symbolic link behavior: If the path points to a symbolic link, the function returns attributes for the symbolic link.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFileAttributesExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileAttributesEx([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] GetFileExInfoLevels fInfoLevelId, out Win32FileAttributeData lpFileInformation);

      #endregion // GetFileAttributesEx

      #region GetFileAttributesTransacted

      /// <summary>Retrieves file system attributes for a specified file or directory as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// Transacted Operations
      /// If a file is open for modification in a transaction, no other thread can open the file for modification until the transaction is committed.
      /// Conversely, if a file is open for modification outside of a transaction, no transacted thread can open the file for modification until the
      /// non-transacted handle is closed. If a non-transacted thread has a handle opened to modify a file, a call to GetFileAttributesTransacted for
      /// that file will fail with an ERROR_TRANSACTIONAL_CONFLICT error.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFileAttributesTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileAttributesTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] GetFileExInfoLevels fInfoLevelId, out Win32FileAttributeData lpFileInformation, SafeHandle hTransaction);

      #endregion // GetFileAttributesTransacted

      #region GetFileInformationByHandle

      /// <summary>Retrieves file information for the specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero and file information data is contained in the buffer pointed to by the lpFileInformation parameter.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// Depending on the underlying network features of the operating system and the type of server connected to,
      /// the GetFileInformationByHandle function may fail, return partial information, or full information for the given file.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out ByHandleFileInformation lpFileInformation);

      #endregion // GetFileInformationByHandle

      #region GetFileInformationByHandleEx

      /// <summary>Retrieves file information for the specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero and file information data is contained in the buffer pointed to by the lpFileInformation parameter.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps | Windows Store apps]</remarks>
      /// <remarks>Redistributable: Windows SDK on Windows Server 2003 and Windows XP.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileInformationByHandleEx(SafeFileHandle hFile, FileInfoByHandleClass fileInformationClass, SafeHandle lpFileInformation, [MarshalAs(UnmanagedType.U4)] uint dwBufferSize);

      #endregion // GetFileInformationByHandleEx

      #region GetFileSizeEx

      /// <summary>Retrieves the size of the specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileSizeEx(SafeFileHandle hFile, out long lpFileSize);

      #endregion // GetFileSizeEx

      #region GetFinalPathNameByHandle

      /// <summary>Retrieves the final path for the specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFinalPathNameByHandleW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetFinalPathNameByHandle(SafeFileHandle hFile, StringBuilder lpszFilePath, [MarshalAs(UnmanagedType.U4)] uint cchFilePath, FinalPathFormats dwFlags);

      #endregion // GetFinalPathNameByHandle
      
      #region GetMappedFileName

      /// <summary>Checks whether the specified address is within a memory-mapped file in the address space of the specified process.
      /// If so, the function returns the name of the memory-mapped file.
      /// </summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("psapi.dll", SetLastError = false, CharSet = CharSet.Unicode, EntryPoint = "GetMappedFileNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetMappedFileName(IntPtr hProcess, SafeLocalMemoryBufferHandle lpv, StringBuilder lpFilename, [MarshalAs(UnmanagedType.U4)] uint nSize);

      #endregion // GetMappedFileName

      #region LockFile

      /// <summary>Locks the specified file for exclusive access by the calling process.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero (TRUE).
      /// If the function fails, the return value is zero (FALSE). To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool LockFile(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetLow, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetHigh, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToLockLow, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToLockHigh);

      #endregion // LockFile

      #region MapViewOfFile

      /// <summary>Maps a view of a file mapping into the address space of a calling process.</summary>
      /// <returns>
      /// If the function succeeds, the return value is the starting address of the mapped view.
      /// If the function fails, the return value is <c>null</c>.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      internal static extern SafeLocalMemoryBufferHandle MapViewOfFile(SafeFileHandle hFileMappingObject, [MarshalAs(UnmanagedType.U4)] uint dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetHigh, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

      #endregion // MapViewOfFile

      #region MoveFileWithProgress

      /// <summary>Moves a file or directory, including its children. You can provide a callback function that receives progress notifications.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "MoveFileWithProgressW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool MoveFileWithProgress([MarshalAs(UnmanagedType.LPWStr)] string existingFileName, [MarshalAs(UnmanagedType.LPWStr)] string newFileName, CopyMoveProgressDelegate lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.U4)] MoveOptions dwFlags);

      #endregion // MoveFileWithProgress

      #region MoveFileTransacted

      /// <summary>Moves an existing file or a directory, including its children, as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "MoveFileTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool MoveFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, CopyMoveProgressDelegate lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.U4)] MoveOptions dwCopyFlags, SafeHandle hTransaction);

      #endregion // MoveFileTransacted

      #region PathFileExists

      /// <summary>Determines whether a path to a file system object such as a file or folder is valid.</summary>
      /// <returns><c>true</c> if the file exists; <c>false</c> otherwise. Call GetLastError for extended error information.</returns>
      /// <remarks>
      /// This function tests the validity of the path.
      /// A path specified by Universal Naming Convention (UNC) is limited to a file only; that is, \\server\share\file is permitted.
      /// A network share path to a server or server share is not permitted; that is, \\server or \\server\share.
      /// This function returns FALSE if a mounted remote drive is out of service.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PathFileExistsW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool PathFileExists([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

      #endregion // PathFileExists

      #region ReplaceFile

      /// <summary>Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "ReplaceFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool ReplaceFile([MarshalAs(UnmanagedType.LPWStr)] string lpReplacedFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpReplacementFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpBackupFileName, FileSystemRights dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

      #endregion // ReplaceFile

      #region SetFileAttributes

      /// <summary>Sets the attributes for a file or directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetFileAttributesW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes);

      #endregion // SetFileAttributes

      #region SetFileAttributesTransacted

      /// <summary>Sets the attributes for a file or directory as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetFileAttributesTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetFileAttributesTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes, SafeHandle hTransaction);

      #endregion // SetFileAttributesTransacted

      #region SetFileTime

      /// <summary>Sets the date and time that the specified file or directory was created, last accessed, or last modified.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetFileTime(SafeFileHandle hFile, SafeGlobalMemoryBufferHandle lpCreationTime, SafeGlobalMemoryBufferHandle lpLastAccessTime, SafeGlobalMemoryBufferHandle lpLastWriteTime);

      #endregion // SetFileTime

      #region ShGetFileInfo

      /// <summary>Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.</summary>
      /// <remarks>You should call this function from a background thread. Failure to do so could cause the UI to stop responding.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SHGetFileInfoW")]
      internal static extern IntPtr ShGetFileInfo([MarshalAs(UnmanagedType.LPWStr)] string pszPath, FileAttributes dwFileAttributes, [MarshalAs(UnmanagedType.Struct)] out Shell32.FileInfo psfi, [MarshalAs(UnmanagedType.U4)] uint cbFileInfo, [MarshalAs(UnmanagedType.U4)] Shell32.FileAttributes uFlags);

      #endregion // ShGetFileInfo

      #region UnlockFile

      /// <summary>Unlocks a region in an open file. Unlocking a region enables other processes to access the region.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UnlockFile(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetLow, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetHigh, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToUnlockLow, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToUnlockHigh);

      #endregion // UnlockFile

      #region UnmapViewOfFile

      /// <summary>Unmaps a mapped view of a file from the calling process's address space.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UnmapViewOfFile(SafeLocalMemoryBufferHandle lpBaseAddress);

      #endregion // UnmapViewOfFile

      #endregion // File Management


      #region Handle and Object Management

      #region CloseHandle

      /// <summary>Closes an open object handle.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CloseHandle(IntPtr hObject);

      #endregion // CloseHandle

      #endregion // Handle and Object Management


      #region Kernel Transaction Manager

      #region CreateTransaction

      /// <summary>Creates a new transaction object.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a handle to the transaction. 
      /// If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("ktmw32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      internal static extern SafeKernelTransactionHandle CreateTransaction([MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpTransactionAttributes, IntPtr uow, [MarshalAs(UnmanagedType.U4)] uint createOptions, [MarshalAs(UnmanagedType.U4)] uint isolationLevel, [MarshalAs(UnmanagedType.U4)] uint isolationFlags, [MarshalAs(UnmanagedType.U4)] uint timeout, [MarshalAs(UnmanagedType.LPWStr)] string description);

      #endregion // CreateTransaction

      #region CommitTransaction

      /// <summary>Requests that the specified transaction be committed.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is 0 (zero). To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("ktmw32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CommitTransaction(SafeHandle hTrans);

      #endregion // CommitTransaction

      #region RollbackTransaction

      /// <summary>Requests that the specified transaction be rolled back. This function is synchronous.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call the GetLastError function.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("ktmw32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool RollbackTransaction(SafeHandle hTrans);

      #endregion // RollbackTransaction

      #endregion // Kernel Transaction Manager


      #region Path Management

      #region GetFullPathName

      /// <summary>Retrieves the full path and file name of the specified file or directory.</summary>
      /// <returns>If the function fails for any other reason, the return value is zero. To get extended error information, call GetLastError.</returns>
      /// <remarks>The GetFullPathName function is not recommended for multithreaded applications or shared library code.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFullPathNameW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetFullPathName([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint nBufferLength, StringBuilder lpBuffer, IntPtr lpFilePart);

      #endregion // GetFullPathName

      #region GetFullPathNameTransacted

      /// <summary>Retrieves the full path and file name of the specified file or directory as a transacted operation.</summary>
      /// <returns>If the function fails for any other reason, the return value is zero. To get extended error information, call GetLastError.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFullPathNameTransactedW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetFullPathNameTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint nBufferLength, StringBuilder lpBuffer, IntPtr lpFilePart, SafeHandle hTransaction);

      #endregion // GetFullPathNameTransacted

      #region GetLongPathName

      /// <summary>Converts the specified path to its long form.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetLongPathNameW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetLongPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszShortPath, StringBuilder lpszLongPath, [MarshalAs(UnmanagedType.U4)] uint cchBuffer);

      #endregion // GetLongPathName

      #region GetLongPathNameTransacted

      /// <summary>Converts the specified path to its long form as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetLongPathNameTransactedW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetLongPathNameTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpszShortPath, StringBuilder lpszLongPath, [MarshalAs(UnmanagedType.U4)] uint cchBuffer, SafeHandle hTransaction);

      #endregion // GetLongPathNameTransacted

      #region GetShortPathName

      /// <summary>Retrieves the short path form of the specified path.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetShortPathNameW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszLongPath, StringBuilder lpszShortPath, [MarshalAs(UnmanagedType.U4)] uint cchBuffer);

      #endregion // GetShortPathName

      #region PathCreateFromUrl

      /// <summary>Converts a file URL to a Microsoft MS-DOS path.</summary>
      /// <returns>Type: HRESULT
      /// If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PathCreateFromUrlW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint PathCreateFromUrl([MarshalAs(UnmanagedType.LPWStr)] string pszUrl, StringBuilder pszPath, [MarshalAs(UnmanagedType.U4)] ref uint pcchPath, [MarshalAs(UnmanagedType.U4)] uint dwFlags);

      #endregion // PathCreateFromUrl

      #region PathCreateFromUrlAlloc

      /// <summary>Creates a path from a file URL.</summary>
      /// <returns>Type: HRESULT
      /// If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint PathCreateFromUrlAlloc([MarshalAs(UnmanagedType.LPWStr)] string pszIn, out StringBuilder pszPath, [MarshalAs(UnmanagedType.U4)] uint dwFlags);

      #endregion // PathCreateFromUrlAlloc

      #region UrlCreateFromPath

      /// <summary>Converts a Microsoft MS-DOS path to a canonicalized URL.</summary>
      /// <returns>Type: HRESULT
      /// Returns S_FALSE if pszPath is already in URL format. In this case, pszPath will simply be copied to pszUrl.
      /// Otherwise, it returns S_OK if successful or a standard COM error value if not.
      /// </returns>
      /// <remarks>
      /// UrlCreateFromPath does not support extended paths. These are paths that include the extended-length path prefix "\\?\".
      /// </remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "UrlCreateFromPathW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint UrlCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, StringBuilder pszUrl, ref uint pcchUrl, [MarshalAs(UnmanagedType.U4)] uint dwFlags);

      #endregion // UrlCreateFromPath

      #region UrlIs

      /// <summary>Tests whether a URL is a specified type.</summary>
      /// <returns>
      /// Type: BOOL
      /// For all but one of the URL types, UrlIs returns <c>true</c> if the URL is the specified type, <c>true</c> otherwise.
      /// If UrlIs is set to <see cref="T:Shell32.UrlType.IsAppliable"/>, UrlIs will attempt to determine the URL scheme.
      /// If the function is able to determine a scheme, it returns <c>true</c>, or <c>false</c>.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "UrlIsW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UrlIs([MarshalAs(UnmanagedType.LPWStr)] string pszUrl, Shell32.UrlType urlIs);

      #endregion // UrlIs

      #endregion // Path Management


      #region Volume Management

      #region DefineDosDevice

      /// <summary>Defines, redefines, or deletes MS-DOS device names.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DefineDosDeviceW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DefineDosDevice(DosDeviceAttributes dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetPath);

      #endregion // DefineDosDevice

      #region DeleteVolumeMountPoint

      /// <summary>Deletes a drive letter or mounted folder.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeleteVolumeMountPointW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool DeleteVolumeMountPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeMountPoint);

      #endregion // DeleteVolumeMountPoint

      #region FindFirstVolume

      /// <summary>Retrieves the name of a volume on a computer. FindFirstVolume is used to begin scanning the volumes of a computer.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a search handle used in a subsequent call to the FindNextVolume and FindVolumeClose functions.
      /// If the function fails to find any volumes, the return value is the INVALID_HANDLE_VALUE error code. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstVolumeW")]
      internal extern static SafeFindVolumeHandle FindFirstVolume(StringBuilder lpszVolumeName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

      #endregion // FindFirstVolume

      #region FindFirstVolumeMountPoint

      /// <summary>Retrieves the name of a mounted folder on the specified volume. FindFirstVolumeMountPoint is used to begin scanning the mounted folders on a volume.</summary>
      /// <returns>
      /// If the function succeeds, the return value is a search handle used in a subsequent call to the FindNextVolumeMountPoint and FindVolumeMountPointClose functions.
      /// If the function fails to find a mounted folder on the volume, the return value is the INVALID_HANDLE_VALUE error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstVolumeMountPointW")]
      internal extern static SafeFindVolumeMountPointHandle FindFirstVolumeMountPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszRootPathName, StringBuilder lpszVolumeMountPoint, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

      #endregion // FindFirstVolumeMountPoint

      #region FindNextVolume

      /// <summary>Continues a volume search started by a call to the FindFirstVolume function. FindNextVolume finds one volume per call.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextVolumeW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool FindNextVolume(SafeFindVolumeHandle hFindVolume, StringBuilder lpszVolumeName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

      #endregion // FindNextVolume

      #region FindNextVolumeMountPoint

      /// <summary>Continues a mounted folder search started by a call to the FindFirstVolumeMountPoint function. FindNextVolumeMountPoint finds one mounted folder per call.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError. If no more mounted folders can be found, the GetLastError function returns the ERROR_NO_MORE_FILES error code.
      /// In that case, close the search with the FindVolumeMountPointClose function.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextVolumeMountPointW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool FindNextVolumeMountPoint(SafeFindVolumeMountPointHandle hFindVolume, StringBuilder lpszVolumeName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

      #endregion // FindNextVolumeMountPoint

      #region FindVolumeClose

      /// <summary>Closes the specified volume search handle.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool FindVolumeClose(IntPtr hFindVolume);

      #endregion // FindVolumeClose

      #region FindVolumeMountPointClose

      /// <summary>Closes the specified mounted folder search handle.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>SetLastError is set to <c>false</c>.</remarks>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool FindVolumeMountPointClose(IntPtr hFindVolume);

      #endregion // FindVolumeMountPointClose

      #region GetVolumeInformation

      /// <summary>Retrieves information about the file system and volume associated with the specified root directory.</summary>
      /// <returns>
      /// If all the requested information is retrieved, the return value is nonzero.
      /// If not all the requested information is retrieved, the return value is zero.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      /// <remarks>"lpRootPathName" must end with a trailing backslash.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetVolumeInformationW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool GetVolumeInformation([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName, StringBuilder lpVolumeNameBuffer, [MarshalAs(UnmanagedType.U4)] uint nVolumeNameSize, [MarshalAs(UnmanagedType.U4)] out uint lpVolumeSerialNumber, [MarshalAs(UnmanagedType.U4)] out uint lpMaximumComponentLength, [MarshalAs(UnmanagedType.U4)] out VolumeInfoAttributes lpFileSystemAttributes, StringBuilder lpFileSystemNameBuffer, [MarshalAs(UnmanagedType.U4)] uint nFileSystemNameSize);

      #endregion // GetVolumeInformation

      #region GetVolumeInformationByHandle

      /// <summary>Retrieves information about the file system and volume associated with the specified file.</summary>
      /// <returns>
      /// If all the requested information is retrieved, the return value is nonzero.
      /// If not all the requested information is retrieved, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>To retrieve the current compression state of a file or directory, use FSCTL_GET_COMPRESSION.</remarks>
      /// <remarks>SMB does not support volume management functions.</remarks>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetVolumeInformationByHandleW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool GetVolumeInformationByHandle(SafeFileHandle hFile, StringBuilder lpVolumeNameBuffer, [MarshalAs(UnmanagedType.U4)] uint nVolumeNameSize, [MarshalAs(UnmanagedType.U4)] out uint lpVolumeSerialNumber, [MarshalAs(UnmanagedType.U4)] out uint lpMaximumComponentLength, out VolumeInfoAttributes lpFileSystemAttributes, StringBuilder lpFileSystemNameBuffer, [MarshalAs(UnmanagedType.U4)] uint nFileSystemNameSize);

      #endregion // GetVolumeInformationByHandle

      #region GetVolumeNameForVolumeMountPoint

      /// <summary>Retrieves a volume GUID path for the volume that is associated with the specified volume mount point (drive letter, volume GUID path, or mounted folder).</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Use GetVolumeNameForVolumeMountPoint to obtain a volume GUID path for use with functions such as SetVolumeMountPoint and FindFirstVolumeMountPoint that require a volume GUID path as an input parameter.</remarks>
      /// <remarks>SMB does not support volume management functions.</remarks>
      /// <remarks>Mount points aren't supported by ReFS volumes.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetVolumeNameForVolumeMountPointW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetVolumeNameForVolumeMountPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeMountPoint, StringBuilder lpszVolumeName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

      #endregion // GetVolumeNameForVolumeMountPoint

      #region GetVolumePathName

      /// <summary>Retrieves the volume mount point where the specified path is mounted.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetVolumePathNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetVolumePathName([MarshalAs(UnmanagedType.LPWStr)] string lpszFileName, StringBuilder lpszVolumePathName, [MarshalAs(UnmanagedType.U4)] uint cchBufferLength);

      #endregion // GetVolumePathName

      #region GetVolumePathNamesForVolumeName

      /// <summary>Retrieves a list of drive letters and mounted folder paths for the specified volume.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetVolumePathNamesForVolumeNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetVolumePathNamesForVolumeName([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName, char[] lpszVolumePathNames, [MarshalAs(UnmanagedType.U4)] uint cchBuferLength, [MarshalAs(UnmanagedType.U4)] out uint lpcchReturnLength);

      #endregion // GetVolumePathNamesForVolumeName
      
      #region SetVolumeLabel

      /// <summary>Sets the label of a file system volume.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      /// <remarks>"lpRootPathName" must end with a trailing backslash.</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetVolumeLabelW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool SetVolumeLabel([MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName, [MarshalAs(UnmanagedType.LPWStr)] string lpVolumeName);

      #endregion // SetVolumeLabel

      #region SetVolumeMountPoint

      /// <summary>Associates a volume with a drive letter or a directory on another volume.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetVolumeMountPointW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool SetVolumeMountPoint([MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeMountPoint, [MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName);

      #endregion // SetVolumeMountPoint

      #region QueryDosDevice

      /// <summary>Retrieves information about MS-DOS device names.</summary>
      /// <returns>
      /// If the function succeeds, the return value is the number of TCHARs stored into the buffer pointed to by lpTargetPath.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// If the buffer is too small, the function fails and the last error code is ERROR_INSUFFICIENT_BUFFER.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "QueryDosDeviceW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint QueryDosDevice([MarshalAs(UnmanagedType.LPWStr)] string lpDeviceName, char[] lpTargetPath, [MarshalAs(UnmanagedType.U4)] uint ucchMax);

      #endregion // QueryDosDevice

      #endregion // Volume Management
   }
}
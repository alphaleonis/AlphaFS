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
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      #region CM_Xxx

      /// <summary>The CM_Connect_Machine function creates a connection to a remote machine.</summary>
      /// <returns>
      ///   <para>If the operation succeeds, the function returns CR_SUCCESS.</para>
      ///   <para>Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.</para>
      ///   <para>You cannot access remote machines when running on these versions of Windows.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CM_Connect_MachineW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Connect_Machine([MarshalAs(UnmanagedType.LPWStr)] string uncServerName, out SafeCmConnectMachineHandle phMachine);


      /// <summary>The CM_Get_Device_ID_Ex function retrieves the device instance ID for a specified device instance on a local or a remote machine.</summary>
      /// <returns>
      ///   <para>If the operation succeeds, the function returns CR_SUCCESS.</para>
      ///   <para>Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.</para>
      ///   <para>You cannot access remote machines when running on these versions of Windows.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CM_Get_Device_ID_ExW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Get_Device_ID_Ex([MarshalAs(UnmanagedType.U4)] uint dnDevInst, SafeGlobalMemoryBufferHandle buffer, [MarshalAs(UnmanagedType.U4)] uint bufferLen, [MarshalAs(UnmanagedType.U4)] uint ulFlags, SafeCmConnectMachineHandle hMachine);


      /// <summary>The CM_Disconnect_Machine function removes a connection to a remote machine.</summary>
      /// <returns>
      ///   <para>If the operation succeeds, the function returns CR_SUCCESS.</para>
      ///   <para>Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.</para>
      ///   <para>You cannot access remote machines when running on these versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>SetLastError is set to <c>false</c>.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = false, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Disconnect_Machine(IntPtr hMachine);


      /// <summary>The CM_Get_Parent_Ex function obtains a device instance handle to the parent node of a specified device node (devnode) in a local or a remote machine's device tree.</summary>
      /// <returns>
      ///   <para>If the operation succeeds, the function returns CR_SUCCESS.</para>
      ///   <para>Otherwise, it returns one of the CR_-prefixed error codes defined in Cfgmgr32.h.</para>
      /// </returns>
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.</para>
      ///   <para>You cannot access remote machines when running on these versions of Windows.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.I4)]
      internal static extern int CM_Get_Parent_Ex([MarshalAs(UnmanagedType.U4)] out uint pdnDevInst, [MarshalAs(UnmanagedType.U4)] uint dnDevInst, [MarshalAs(UnmanagedType.U4)] uint ulFlags, SafeCmConnectMachineHandle hMachine);

      #endregion // CM_Xxx


      #region DeviceIoControl

      /// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
      /// <remarks>
      ///   <para>To retrieve a handle to the device, you must call the <see cref="Filesystem.NativeMethods.CreateFile"/> function with either the name of a device or the name of the driver associated with a device.</para>
      ///   <para>To specify a device name, use the following format: <c>\\.\DeviceName</c></para>
      ///   <para>Minimum supported client: Windows XP</para>
      ///   <para>Minimum supported server: Windows Server 2003</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>If the operation completes successfully, the return value is nonzero.</para>
      ///   <para>If the operation fails or is pending, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeviceIoControl(SafeFileHandle hDevice, [MarshalAs(UnmanagedType.U4)] IoControlCode dwIoControlCode, IntPtr lpInBuffer, [MarshalAs(UnmanagedType.U4)] uint nInBufferSize, SafeGlobalMemoryBufferHandle lpOutBuffer, [MarshalAs(UnmanagedType.U4)] uint nOutBufferSize, IntPtr lpBytesReturned, IntPtr lpOverlapped);


      /// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
      /// <remarks>
      ///   <para>To retrieve a handle to the device, you must call the <see cref="Filesystem.NativeMethods.CreateFile"/> function with either the name of a device or the name of the driver associated with a device.</para>
      ///   <para>To specify a device name, use the following format: <c>\\.\DeviceName</c></para>
      ///   <para>Minimum supported client: Windows XP</para>
      ///   <para>Minimum supported server: Windows Server 2003</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>If the operation completes successfully, the return value is nonzero.</para>
      ///   <para>If the operation fails or is pending, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeviceIoControlAnyObjectGetSet(SafeFileHandle hDevice, [MarshalAs(UnmanagedType.U4)] IoControlCode dwIoControlCode, [MarshalAs(UnmanagedType.AsAny)] object lpInBuffer, [MarshalAs(UnmanagedType.U4)] uint nInBufferSize, SafeGlobalMemoryBufferHandle lpOutBuffer, [MarshalAs(UnmanagedType.U4)] uint nOutBufferSize, IntPtr lpBytesReturned, IntPtr lpOverlapped);


      /// <summary>Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
      /// <remarks>
      ///   <para>To retrieve a handle to the device, you must call the <see cref="Filesystem.NativeMethods.CreateFile"/> function with either the name of a device or the name of the driver associated with a device.</para>
      ///   <para>To specify a device name, use the following format: <c>\\.\DeviceName</c></para>
      ///   <para>Minimum supported client: Windows XP</para>
      ///   <para>Minimum supported server: Windows Server 2003</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>If the operation completes successfully, the return value is nonzero.</para>
      ///   <para>If the operation fails or is pending, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeviceIoControl"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeviceIoJunctions(SafeFileHandle hDevice, [MarshalAs(UnmanagedType.U4)] IoControlCode dwIoControlCode, SafeGlobalMemoryBufferHandle lpInBuffer, [MarshalAs(UnmanagedType.U4)] uint nInBufferSize, IntPtr lpOutBuffer, [MarshalAs(UnmanagedType.U4)] uint nOutBufferSize, IntPtr lpBytesReturned, IntPtr lpOverlapped);

      #endregion // DeviceIoControl


      #region SetupDiXxx

      /// <summary>The SetupDiDestroyDeviceInfoList function deletes a device information set and frees all associated memory.
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>SetLastError is set to <c>false</c>.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>The function returns TRUE if it is successful.</para>
      ///   <para>Otherwise, it returns FALSE and the logged error can be retrieved with a call to GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = false, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);


      /// <summary>The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that are contained in a device information set.
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>SetupDiEnumDeviceInterfaces returns TRUE if the function completed without error.</para>
      ///   <para>If the function completed with an error, FALSE is returned and the error code for the failure can be retrieved by calling GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiEnumDeviceInterfaces(SafeHandle hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, [MarshalAs(UnmanagedType.U4)] uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);


      /// <summary>The SetupDiGetClassDevsEx function returns a handle to a device information set that contains requested device information elements for a local or a remote computer.
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>If the operation succeeds, SetupDiGetClassDevsEx returns a handle to a device information set that contains all installed devices that matched the supplied parameters.</para>
      ///   <para>If the operation fails, the function returns INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      internal static extern SafeSetupDiClassDevsExHandle SetupDiGetClassDevsEx(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, [MarshalAs(UnmanagedType.U4)] DEVICE_INFORMATION_FLAGS devsExFlags, IntPtr deviceInfoSet, [MarshalAs(UnmanagedType.LPWStr)] string machineName, IntPtr reserved);


      /// <summary>The SetupDiGetDeviceInterfaceDetail function returns details about a device interface.
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///   <para>&#160;</para>
      ///   <para>The interface detail returned by this function consists of a device path that can be passed to Win32 functions such as CreateFile.</para>
      ///   <para>Do not attempt to parse the device path symbolic name. The device path can be reused across system starts.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>SetupDiGetDeviceInterfaceDetail returns TRUE if the function completed without error.</para>
      ///   <para>If the function completed with an error, FALSE is returned and the error code for the failure can be retrieved by calling GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiGetDeviceInterfaceDetail(SafeHandle hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, [MarshalAs(UnmanagedType.U4)] uint deviceInterfaceDetailDataSize, IntPtr requiredSize, ref SP_DEVINFO_DATA deviceInfoData);



      /// <summary>The SetupDiGetDeviceProperty function retrieves a device instance property.
      /// <remarks>
      ///   <para>Available in Windows Vista and later versions of Windows.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>SetupDiGetDeviceProperty returns TRUE if it is successful.</para>
      ///   <para>Otherwise, it returns FALSE, and the logged error can be retrieved by calling GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetDevicePropertyW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiGetDeviceProperty(SafeHandle deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propkey, [MarshalAs(UnmanagedType.U8)] ref ulong propertyDataType, byte[] propertyBuffer, int propertyBufferSize, ref int requiredSize, uint flags);



      //[SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      //[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetDevicePropertyW"), SuppressUnmanagedCodeSecurity]
      //[return: MarshalAs(UnmanagedType.Bool)]
      //internal static extern bool SetupDiGetDeviceProperty3(SafeHandle deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propkey, [MarshalAs(UnmanagedType.U8)] ref ulong propertyDataType, Guid propertyBuffer, int propertyBufferSize, ref int requiredSize, uint flags);


      ///// <summary>The SetupDiGetDeviceProperty function retrieves a device instance property.
      ///// <remarks>
      /////   <para>Available in Windows Vista and later versions of Windows.</para>
      ///// </remarks>
      ///// </summary>
      ///// <returns>
      /////   <para>SetupDiGetDeviceProperty returns TRUE if it is successful.</para>
      /////   <para>Otherwise, it returns FALSE, and the logged error can be retrieved by calling GetLastError.</para>
      ///// </returns>
      //[SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      //[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetDevicePropertyW"), SuppressUnmanagedCodeSecurity]
      //[return: MarshalAs(UnmanagedType.Bool)]
      //internal static extern bool SetupDiGetDeviceProperty2(SafeHandle deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref DEVPROPKEY propkey, [MarshalAs(UnmanagedType.U8)] ref ulong propertyDataType, SafeGlobalMemoryBufferHandle propertyBuffer, int propertyBufferSize, ref int requiredSize, [MarshalAs(UnmanagedType.U4)] uint flags);


      ///// <summary>The SetupDiGetDeviceRegistryProperty function retrieves a specified Plug and Play device property.
      ///// <remarks>
      /////   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      ///// </remarks>
      ///// </summary>
      ///// <returns>
      /////   <para>SetupDiGetDeviceRegistryProperty returns TRUE if the call was successful.</para>
      /////   <para>Otherwise, it returns FALSE and the logged error can be retrieved by making a call to GetLastError.</para>
      /////   <para>SetupDiGetDeviceRegistryProperty returns the ERROR_INVALID_DATA error code if the requested property does not exist for a device or if the property data is not valid.</para>
      ///// </returns>
      //[SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      //[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      //[return: MarshalAs(UnmanagedType.Bool)]
      //internal static extern bool SetupDiGetDeviceRegistryProperty(SafeHandle deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, SPDRP property, ref int propertyRegDataType, byte[] propertyBuffer, [MarshalAs(UnmanagedType.U4)] uint propertyBufferSize, ref int requiredSize);


      /// <summary>The SetupDiGetDeviceRegistryProperty function retrieves a specified Plug and Play device property.
      /// <remarks>
      ///   <para>Available in Microsoft Windows 2000 and later versions of Windows.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>SetupDiGetDeviceRegistryProperty returns TRUE if the call was successful.</para>
      ///   <para>Otherwise, it returns FALSE and the logged error can be retrieved by making a call to GetLastError.</para>
      ///   <para>SetupDiGetDeviceRegistryProperty returns the ERROR_INVALID_DATA error code if the requested property does not exist for a device or if the property data is not valid.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetupDiGetDeviceRegistryProperty(SafeHandle deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, SPDRP property, IntPtr propertyRegDataType, SafeGlobalMemoryBufferHandle propertyBuffer, [MarshalAs(UnmanagedType.U4)] uint propertyBufferSize, IntPtr requiredSize);

      #endregion // SetupDiXxx


      /// <summary>Retrieves the current power state of the specified device. This function cannot be used to query the power state of a display device.</summary>
      /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
      /// <remarks>
      ///   Minimum supported client: Windows XP [desktop apps only]
      ///   Minimum supported server: Windows Server 2003 [desktop apps only]
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetDevicePowerState(SafeHandle hDevice, out bool pfOn);
   }
}

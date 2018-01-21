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

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides static methods to retrieve device resource information from a local or remote host.</summary>
   public static partial class Device
   {
      // Helper Methods.


      /// <summary>Builds a DeviceInfo Data structure.</summary>
      /// <returns>An initialized NativeMethods.SP_DEVINFO_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVINFO_DATA CreateDeviceInfoDataInstance()
      {
         var did = new NativeMethods.SP_DEVINFO_DATA();
         did.cbSize = (uint) Marshal.SizeOf(did);

         return did;
      }


      /// <summary>Builds a Device Interface Data structure.</summary>
      /// <returns>An initialized NativeMethods.SP_DEVICE_INTERFACE_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVICE_INTERFACE_DATA CreateDeviceInterfaceDataInstance()
      {
         var did = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
         did.cbSize = (uint) Marshal.SizeOf(did);

         return did;
      }


      /// <summary>Builds a Device Interface Detail Data structure.</summary>
      /// <returns>An initialized NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA GetDeviceInterfaceDetailDataInstance(SafeHandle safeHandle, NativeMethods.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, NativeMethods.SP_DEVINFO_DATA deviceInfoData)
      {
         // Build a Device Interface Detail Data structure.
         var didd = new NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA
         {
            cbSize = (uint) (IntPtr.Size == 4 ? IntPtr.Size + UnicodeEncoding.CharSize : 8)
         };

         // Get device interace details.
         var success = NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref deviceInterfaceData, ref didd, NativeMethods.DefaultFileBufferSize, IntPtr.Zero, ref deviceInfoData);

         var lastError = Marshal.GetLastWin32Error();
         if (!success)
            NativeError.ThrowException(lastError);

         return didd;
      }
      

      [SecurityCritical]
      private static string GetDeviceRegistryProperty(SafeHandle safeHandle, NativeMethods.SP_DEVINFO_DATA deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum property)
      {
         NativeMethods.IsValidHandle(safeHandle);


         const int bufferSize = NativeMethods.DefaultFileBufferSize;

         using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
         {
            uint regType;

            var success = NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, property, out regType, safeBuffer, bufferSize, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();


            // MSDN: SetupDiGetDeviceRegistryProperty returns the ERROR_INVALID_DATA error code if the requested property
            // does not exist for a device or if the property data is not valid.

            if (!success && lastError != Win32Errors.ERROR_INVALID_DATA)
               NativeError.ThrowException(lastError);


            return safeBuffer.PtrToStringUni();
         }
      }


      [SecurityCritical]
      private static int GetDoubledBufferSizeOrThrowException(int lastError, SafeHandle safeBuffer, int bufferSize, string path)
      {
         if (null != safeBuffer && !safeBuffer.IsClosed)
            safeBuffer.Close();


         switch ((uint) lastError)
         {
            case Win32Errors.ERROR_MORE_DATA:
            case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
               bufferSize = 2 * bufferSize;
               break;


            default:
               NativeMethods.IsValidHandle(safeBuffer, lastError, string.Format(CultureInfo.InvariantCulture, "Buffer size: {0}. Path: {1}", bufferSize.ToString(CultureInfo.InvariantCulture), path));
               break;
         }


         return bufferSize;
      }




      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object needs to be disposed by caller.")]
      [SecurityCritical]
      internal static SafeGlobalMemoryBufferHandle GetDeviceIoData<T>(SafeFileHandle safeHandle, NativeMethods.IoControlCode controlCode, string path, int size = -1)
      {
         NativeMethods.IsValidHandle(safeHandle);


         var bufferSize = size > -1 ? size : Marshal.SizeOf(typeof(T));

         while (true)
         {
            uint bytesReturned;

            var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize);

            var success = NativeMethods.DeviceIoControl(safeHandle, controlCode, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, out bytesReturned, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();

            if (success)
               return safeBuffer;


            bufferSize = GetDoubledBufferSizeOrThrowException(lastError, safeBuffer, bufferSize, path);
         }
      }


      /// <summary>Invokes InvokeIoControl with the specified input and specified size.</summary>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object needs to be disposed by caller.")]
      [SecurityCritical]
      internal static SafeGlobalMemoryBufferHandle InvokeDeviceIoData<T>(SafeFileHandle safeHandle, NativeMethods.IoControlCode controlCode, T anyObject, string path, int size = -1)
      {
         NativeMethods.IsValidHandle(safeHandle);


         var bufferSize = size > -1 ? size : Marshal.SizeOf(anyObject);

         while (true)
         {
            uint bytesReturned;

            var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize);

            var success = NativeMethods.DeviceIoControlAnyObject(safeHandle, controlCode, anyObject, (uint) bufferSize, safeBuffer, (uint) safeBuffer.Capacity, out bytesReturned, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();


            if (success)
               return safeBuffer;


            bufferSize = GetDoubledBufferSizeOrThrowException(lastError, safeBuffer, bufferSize, path);
         }
      }
      

      [SecurityCritical]
      private static void SetDeviceProperties(SafeHandle safeHandle, DeviceInfo deviceInfo, NativeMethods.SP_DEVINFO_DATA deviceInfoData)
      {
         deviceInfo.BaseContainerId = new Guid(GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.BaseContainerId));

         deviceInfo.ClassGuid = new Guid(GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.ClassGuid));
         
         deviceInfo.CompatibleIds = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.CompatibleIds);

         deviceInfo.DeviceClass = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Class);

         deviceInfo.DeviceDescription = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.DeviceDescription);

         deviceInfo.Driver = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Driver);

         deviceInfo.EnumeratorName = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.EnumeratorName);

         deviceInfo.FriendlyName = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.FriendlyName);

         deviceInfo.HardwareId = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.HardwareId);

         deviceInfo.LocationInformation = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.LocationInformation);

         deviceInfo.LocationPaths = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.LocationPaths);

         deviceInfo.Manufacturer = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Manufacturer);

         deviceInfo.PhysicalDeviceObjectName = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.PhysicalDeviceObjectName);

         deviceInfo.Service = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Service);
      }


      [SecurityCritical]
      private static void SetPhysicalDiskProperties(SafeHandle safeHandle, DeviceInfo deviceInfo, NativeMethods.SP_DEVINFO_DATA deviceInfoData)
      {
         deviceInfo.DeviceClass  = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Class);

         deviceInfo.FriendlyName = GetDeviceRegistryProperty(safeHandle, deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.FriendlyName);
      }


      //private static uint IOCTL_STORAGE_QUERY_PROPERTY = CTL_CODE((uint) NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MASS_STORAGE, 0x500, 0,0);
      //private static uint CTL_CODE(uint deviceType, uint function, uint method, uint access)
      //{
      //   return (deviceType << 16) | (access << 14) | (function << 2) | method;
      //}
   }
}

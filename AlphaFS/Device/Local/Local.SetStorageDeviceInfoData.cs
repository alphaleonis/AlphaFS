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

using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      [SecurityCritical]
      private static void SetStorageDeviceInfoData(bool isElevated, bool isDevice, SafeFileHandle safeFileHandle, string localDevicePath, StorageDeviceInfo storageDeviceInfo)
      {
         var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
         {
            PropertyId = NativeMethods.STORAGE_PROPERTY_ID.StorageDeviceProperty,
            QueryType = NativeMethods.STORAGE_QUERY_TYPE.PropertyStandardQuery
         };
         

         using (var safeBuffer = InvokeDeviceIoData(safeFileHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, localDevicePath, Filesystem.NativeMethods.DefaultFileBufferSize / 2))
         {
            var deviceDescriptor = safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_DESCRIPTOR>();


            storageDeviceInfo.BusType = (StorageBusType) deviceDescriptor.BusType;

            storageDeviceInfo.CommandQueueing = deviceDescriptor.CommandQueueing;

            storageDeviceInfo.ProductId = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.ProductIdOffset).Trim();

            storageDeviceInfo.ProductRevision = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.ProductRevisionOffset).Trim();

            storageDeviceInfo.RemovableMedia = deviceDescriptor.RemovableMedia;

            storageDeviceInfo.SerialNumber = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.SerialNumberOffset).Trim();

            storageDeviceInfo.VendorId = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.VendorIdOffset).Trim();


            if (Utils.IsNullOrWhiteSpace(storageDeviceInfo.ProductRevision) || storageDeviceInfo.ProductRevision.Length == 1)
               storageDeviceInfo.ProductRevision = string.Empty;

            if (Utils.IsNullOrWhiteSpace(storageDeviceInfo.SerialNumber) || storageDeviceInfo.SerialNumber.Length == 1)
               storageDeviceInfo.SerialNumber = string.Empty;
            
            if (Utils.IsNullOrWhiteSpace(storageDeviceInfo.VendorId) || storageDeviceInfo.VendorId.Length == 1)
               storageDeviceInfo.VendorId = string.Empty;
         }


         // Get disk total size.
         
         if (isElevated)
         {
            int lastError;

            using (var safeBuffer = GetDeviceIoData<long>(safeFileHandle,NativeMethods.IoControlCode.IOCTL_DISK_GET_LENGTH_INFO, localDevicePath, out lastError))

               storageDeviceInfo.TotalSize = null != safeBuffer ? safeBuffer.ReadInt64() : 0;
         }


         //// A logical drive path like \\.\D: fails on a dynamic disk.

         //else
         //{
         //   if (isDevice)
         //   {
         //      //var dosDeviceName = Volume.QueryDosDevice(Path.GetRegularPathCore(localDevicePath, GetFullPathOptions.None, false));

         //      //storageDeviceInfo.TotalSize = new DiskSpaceInfo(dosDeviceName, false, true, true).TotalNumberOfBytes;
         //   }

         //   else
         //   {
         //      //if (lastError == Win32Errors.ERROR_INVALID_FUNCTION)
         //      storageDeviceInfo.TotalSize = new DiskSpaceInfo(localDevicePath, false, true, true).TotalNumberOfBytes;
         //   }
         //}
      }
   }
}

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
using System.Security;
using Alphaleonis.Win32.Filesystem;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves the storage adapter of the device that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="safeFileHandle"/>.</returns>
      /// <exception cref="Exception"/>
      [SecurityCritical]
      internal static StorageAdapterInfo GetStorageAdapterInfo(SafeFileHandle safeFileHandle, int deviceNumber, string localDevicePath, string busReportedDeviceDescription)
      {
         var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
         {
            PropertyId = NativeMethods.STORAGE_PROPERTY_ID.StorageAdapterProperty,
            QueryType = NativeMethods.STORAGE_QUERY_TYPE.PropertyStandardQuery
         };


         using (var safeBuffer = InvokeDeviceIoData(safeFileHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, localDevicePath, Filesystem.NativeMethods.DefaultFileBufferSize / 8))

            if (null != safeBuffer)
            {
               var storageAdapterInfo = new StorageAdapterInfo(deviceNumber, safeBuffer.PtrToStructure<NativeMethods.STORAGE_ADAPTER_DESCRIPTOR>());


               if (!Utils.IsNullOrWhiteSpace(busReportedDeviceDescription))

                  storageAdapterInfo.BusReportedDeviceDescription = busReportedDeviceDescription;


               return storageAdapterInfo;
            }

         return null;
      }
   }
}

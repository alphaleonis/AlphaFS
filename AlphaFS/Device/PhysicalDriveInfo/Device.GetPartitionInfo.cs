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

using System.Globalization;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      public static StorageAdapterInfo GetPartitionInfo(string devicePath)
      {
         return GetPartitionInfoCore(devicePath);
      }




      internal static StorageAdapterInfo GetPartitionInfoCore(string devicePath)
      {
         string logicalDrive;

         var pathToDevice = GetDevicePath(devicePath, out logicalDrive);

         if (Utils.IsNullOrWhiteSpace(pathToDevice))
            return null;


         using (var safeHandle = OpenPhysicalDrive(pathToDevice, FileSystemRights.Read))

            return GetPartitionInfoNative(safeHandle, devicePath);
      }


      internal static StorageAdapterInfo GetPartitionInfoNative(SafeFileHandle safeHandle, string pathToDevice)
      {
         var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
         {
            PropertyId = NativeMethods.STORAGE_PROPERTY_ID.StorageAdapterProperty,
            QueryType = NativeMethods.STORAGE_QUERY_TYPE.PropertyStandardQuery
         };

         SafeFileHandle safeHandleRetry = null;
         var isRetry = false;


      StartGetData:
         
         // Get storage adapter info.

         using (var safeBuffer = InvokeDeviceIoData(isRetry ? safeHandleRetry : safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, pathToDevice, NativeMethods.DefaultFileBufferSize / 4))
         {
            if (null == safeBuffer)
            {
               // Assumption through observation: devicePath is a logical drive that points to a Dynamic disk.


               var volDiskExtents = GetVolumeDiskExtents(isRetry ? safeHandleRetry : safeHandle);

               if (volDiskExtents.HasValue)
               {
                  // Use the first disk extent.

                  pathToDevice = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.PhysicalDrivePrefix, volDiskExtents.Value.Extents[0].DiskNumber.ToString(CultureInfo.InvariantCulture));


                  safeHandleRetry = OpenPhysicalDrive(pathToDevice, FileSystemRights.Read);

                  isRetry = null != safeHandleRetry && !safeHandleRetry.IsClosed && !safeHandleRetry.IsInvalid;
               }


               if (isRetry)
                  goto StartGetData;

               return null;
            }


            using (safeBuffer)
            {
               if (isRetry && !safeHandleRetry.IsClosed)
                  safeHandleRetry.Close();

               return new StorageAdapterInfo(safeBuffer.PtrToStructure<NativeMethods.STORAGE_ADAPTER_DESCRIPTOR>(0));
            }
         }
      }
   }
}

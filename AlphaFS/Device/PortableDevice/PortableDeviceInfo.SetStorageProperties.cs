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
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      private void SetStorageProperties(IPortableDeviceValues devicePropertyValues)
      {
         ulong ulongValue;
         string stringValue;


         #region Recommended Properties

         try
         {
            float floatValue;
            devicePropertyValues.GetFloatValue(ref PortableDeviceConstants.DevicePowerLevel, out floatValue);
            PowerLevel = Convert.ToInt32(floatValue);
         }
         catch { PowerLevel = -1; }

         #endregion // Recommended Properties


         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref PortableDeviceConstants.StorageCapacity, out ulongValue);
            StorageCapacity = (long) ulongValue;
         }
         catch { StorageCapacity = -1; }


         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref PortableDeviceConstants.StorageCapacityInObjects, out ulongValue);
            StorageCapacityInObjects = (long) ulongValue;
         }
         catch { StorageCapacityInObjects = -1; }


         try
         {
            devicePropertyValues.GetStringValue(ref PortableDeviceConstants.StorageDescription, out stringValue);
            StorageDescription = stringValue ?? string.Empty;
         }
         catch { StorageDescription = string.Empty; }


         try
         {
            devicePropertyValues.GetStringValue(ref PortableDeviceConstants.StorageFileSystemType, out stringValue);
            StorageFileSystemType = stringValue ?? string.Empty;
         }
         catch { StorageFileSystemType = string.Empty; }


         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref PortableDeviceConstants.StorageFreeSpaceInBytes, out ulongValue);
            StorageFreeSpaceInBytes = (long) ulongValue;
         }
         catch { StorageFreeSpaceInBytes = -1; }
         

         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref PortableDeviceConstants.StorageFreeSpaceInObjects, out ulongValue);
            StorageFreeSpaceInObjects = (long) ulongValue;
         }
         catch { StorageFreeSpaceInObjects = -1; }
         

         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref PortableDeviceConstants.StorageMaxObjectSize, out ulongValue);
            StorageMaxObjectSize = (long) ulongValue;
         }
         catch { StorageMaxObjectSize = -1; }


         try
         {
            devicePropertyValues.GetStringValue(ref PortableDeviceConstants.StorageSerialNumber, out stringValue);
            StorageSerialNumber = stringValue ?? string.Empty;
         }
         catch { StorageSerialNumber = string.Empty; }


         try
         {
            uint uintValue;
            devicePropertyValues.GetUnsignedIntegerValue(ref PortableDeviceConstants.StorageType, out uintValue);
            StorageType = (PortableDeviceStorageType) uintValue;
         }
         catch { StorageType = PortableDeviceStorageType.Undefined; }
      }
   }
}

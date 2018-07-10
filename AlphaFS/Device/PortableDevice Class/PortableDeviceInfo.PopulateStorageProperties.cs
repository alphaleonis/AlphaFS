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

using System.Diagnostics.CodeAnalysis;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      private void PopulateStorageProperties(IPortableDeviceValues devicePropertyValues)
      {
         ulong ulongValue;
         string stringValue;


         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref NativeMethods.WPD_STORAGE_CAPACITY, out ulongValue);
            StorageCapacity = (long) ulongValue;
         }
         catch { StorageCapacity = -1; }


         //try
         //{
         //   devicePropertyValues.GetUnsignedLargeIntegerValue(ref NativeMethods.WPD_STORAGE_CAPACITY_IN_OBJECTS, out ulongValue);
         //   WPD_STORAGE_CAPACITY_IN_OBJECTS = (long) ulongValue;
         //}
         //catch { WPD_STORAGE_CAPACITY_IN_OBJECTS = -1; }


         try
         {
            devicePropertyValues.GetStringValue(ref NativeMethods.WPD_STORAGE_DESCRIPTION, out stringValue);
            StorageDescription = stringValue ?? string.Empty;
         }
         catch { StorageDescription = string.Empty; }


         try
         {
            devicePropertyValues.GetStringValue(ref NativeMethods.WPD_STORAGE_FILE_SYSTEM_TYPE, out stringValue);
            StorageFileSystemType = stringValue ?? string.Empty;
         }
         catch { StorageFileSystemType = string.Empty; }


         try
         {
            devicePropertyValues.GetUnsignedLargeIntegerValue(ref NativeMethods.WPD_STORAGE_FREE_SPACE_IN_BYTES, out ulongValue);
            StorageFreeSpaceInBytes = (long) ulongValue;
         }
         catch { StorageFreeSpaceInBytes = -1; }
         

         //try
         //{
         //   devicePropertyValues.GetUnsignedLargeIntegerValue(ref NativeMethods.WPD_STORAGE_FREE_SPACE_IN_OBJECTS, out ulongValue);
         //   WPD_STORAGE_FREE_SPACE_IN_OBJECTS = (long) ulongValue;
         //}
         //catch { WPD_STORAGE_FREE_SPACE_IN_OBJECTS = -1; }
         

         //try
         //{
         //   devicePropertyValues.GetUnsignedLargeIntegerValue(ref NativeMethods.WPD_STORAGE_MAX_OBJECT_SIZE, out ulongValue);
         //   WPD_STORAGE_MAX_OBJECT_SIZE = (long) ulongValue;
         //}
         //catch { WPD_STORAGE_MAX_OBJECT_SIZE = -1; }


         try
         {
            devicePropertyValues.GetStringValue(ref NativeMethods.WPD_STORAGE_SERIAL_NUMBER, out stringValue);
            StorageSerialNumber = stringValue ?? string.Empty;
         }
         catch { StorageSerialNumber = string.Empty; }


         try
         {
            uint uintValue;
            devicePropertyValues.GetUnsignedIntegerValue(ref NativeMethods.WPD_STORAGE_TYPE, out uintValue);
            StorageType = (PortableDeviceStorageType) uintValue;
         }
         catch { StorageType = PortableDeviceStorageType.Undefined; }
      }
   }
}

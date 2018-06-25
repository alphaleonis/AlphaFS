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
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      private bool PopulateDeviceProperties(IPortableDeviceValues devicePropertyValues)
      {
         uint uintValue;
         string stringValue;


         try
         {
            devicePropertyValues.GetStringValue(ref NativeMethods.WPD_DEVICE_PROTOCOL, out stringValue);

            if (null != stringValue)
            {
               if (stringValue.Equals(NativeMethods.MassStorageClassProtocol, StringComparison.OrdinalIgnoreCase))

                  DeviceProtocol = PortableDeviceProtocol.Ums;


               else if (stringValue.Equals(NativeMethods.MediaTransferProtocol, StringComparison.OrdinalIgnoreCase))

                  DeviceProtocol = PortableDeviceProtocol.Mtp;
            }
         }
         catch { return false; }


         if (_mtpOnly && DeviceProtocol != PortableDeviceProtocol.Mtp)
            return false;


         #region Required Properties

         // DeviceObjectId
         // Manufacturer
         // Model


         try
         {
            devicePropertyValues.GetStringValue(ref NativeMethods.WPD_DEVICE_FIRMWARE_VERSION, out stringValue);
            DeviceFirmwareVersion = stringValue ?? string.Empty;
         }
         catch { DeviceFirmwareVersion = string.Empty; }


         try
         {
            devicePropertyValues.GetStringValue(ref NativeMethods.WPD_DEVICE_SERIAL_NUMBER, out stringValue);
            DeviceSerialNumber = stringValue ?? string.Empty;
         }
         catch { DeviceSerialNumber = string.Empty; }

         #endregion // Required Properties


         #region Recommended Properties

         // FriendlyName

         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref NativeMethods.WPD_DEVICE_POWER_LEVEL, out uintValue);
            DevicePowerLevel = (int) uintValue;
         }
         catch { DevicePowerLevel = -1; }


         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref NativeMethods.WPD_DEVICE_POWER_SOURCE, out uintValue);
            DevicePowerSource = (PortableDevicePowerSource) uintValue;
         }
         catch { DevicePowerSource = PortableDevicePowerSource.Unknown; }


         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref NativeMethods.WPD_DEVICE_TYPE, out uintValue);
            DeviceType = (PortableDeviceType) uintValue;
         }
         catch { DeviceType = PortableDeviceType.Unknown; }


         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref NativeMethods.WPD_DEVICE_TRANSPORT, out uintValue);
            TransportType = (PortableDeviceTransportType) uintValue;
         }
         catch { TransportType = PortableDeviceTransportType.Unspecified; }
         
         #endregion // Recommended Properties


         return true;
      }
   }
}

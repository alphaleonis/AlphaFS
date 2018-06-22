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

using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      private bool SetDeviceProperties(IPortableDeviceValues devicePropertyValues)
      {
         uint uintValue;
         string stringValue;


         try
         {
            devicePropertyValues.GetStringValue(ref PortableDeviceConstants.DeviceProtocol, out stringValue);

            switch (stringValue.ToUpperInvariant())
            {
               case PortableDeviceConstants.MassStorageClassProtocol:
                  Protocol = PortableDeviceProtocol.Ums;
                  break;

               case PortableDeviceConstants.MediaTransferProtocol:
                  Protocol = PortableDeviceProtocol.Mtp;
                  break;
            }
         }
         catch { return false; }


         if (_mtpOnly && Protocol != PortableDeviceProtocol.Mtp)
            return false;


         #region Required Properties

         // No try/catch since these properties are required, assume that these properties are present.
         // DeviceObjectId
         // Manufacturer
         // Model


         try
         {
            devicePropertyValues.GetStringValue(ref PortableDeviceConstants.DeviceFirmwareVersion, out stringValue);
            FirmwareVersion = stringValue ?? string.Empty;
         }
         catch { FirmwareVersion = string.Empty; }


         try
         {
            devicePropertyValues.GetStringValue(ref PortableDeviceConstants.DeviceSerialNumber, out stringValue);
            SerialNumber = stringValue ?? string.Empty;
         }
         catch { SerialNumber = string.Empty; }

         #endregion // Required Properties


         #region Recommended Properties

         // FriendlyName


         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref PortableDeviceConstants.DeviceType, out uintValue);
            DeviceType = (PortableDeviceType) uintValue;
         }
         catch { DeviceType = PortableDeviceType.Unknown; }


         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref PortableDeviceConstants.DevicePowerSource, out uintValue);
            PowerSource = (PortableDevicePowerSource) uintValue;
         }
         catch { PowerSource = PortableDevicePowerSource.Unknown; }


         try
         {
            devicePropertyValues.GetUnsignedIntegerValue(ref PortableDeviceConstants.DeviceTransportType, out uintValue);
            TransportType = (PortableDeviceTransportType) uintValue;
         }
         catch { TransportType = PortableDeviceTransportType.Unspecified; }

         #endregion // Recommended Properties


         return true;
      }
   }
}

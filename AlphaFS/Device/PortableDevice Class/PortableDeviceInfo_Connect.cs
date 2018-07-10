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
using PortableDeviceTypesLib;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo : IDisposable
   {
      /// <summary>Connects to the Portable Device.</summary>
      public void Connect()
      {
         if (IsConnected)
            return;

         // MSDN: The application is not required to send any key/value pairs. However, sending data might improve performance.
         // Typical key/value pairs include the application name, major and minor version, and build number.


         var clientInfo = (IPortableDeviceValues) new PortableDeviceValuesClass();

         clientInfo.SetStringValue(ref NativeMethods.WPD_CLIENT_NAME, _product.Name);
         clientInfo.SetUnsignedIntegerValue(ref NativeMethods.WPD_CLIENT_MAJOR_VERSION, (uint) _version.Major);
         clientInfo.SetUnsignedIntegerValue(ref NativeMethods.WPD_CLIENT_MINOR_VERSION, (uint) _version.Minor);
         clientInfo.SetUnsignedIntegerValue(ref NativeMethods.WPD_CLIENT_REVISION, (uint) _version.Revision);

         PortableDevice.Open(DeviceId, clientInfo);

         IsConnected = CreatePortableDeviceInstance();
      }
      
      
      /// <summary>Disconnects from the Portable Device.</summary>
      public void Disconnect()
      {
         if (null != PortableDevice)
         {
            PortableDevice.Close();
            IsConnected = false;
         }
      }
   }
}

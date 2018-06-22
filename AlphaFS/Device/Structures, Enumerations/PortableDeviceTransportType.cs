/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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

namespace Alphaleonis.Win32.Device
{
   /// <summary>Specifies the inheritance relationship for a service.</summary>
   public enum PortableDeviceTransportType
   {
      /// <summary>The transport type was not specified.</summary>
      Unspecified = NativeMethods.WPD_DEVICE_TRANSPORTS.WPD_DEVICE_TRANSPORT_UNSPECIFIED,

      /// <summary>The device is connected through Universal Serial Bus (USB).</summary>
      Usb = NativeMethods.WPD_DEVICE_TRANSPORTS.WPD_DEVICE_TRANSPORT_USB,

      /// <summary>The device is connected through Internet Protocol (IP).</summary>
      IP = NativeMethods.WPD_DEVICE_TRANSPORTS.WPD_DEVICE_TRANSPORT_IP,

      /// <summary>The device is connected through Bluetooth.</summary>
      Bluetooth = NativeMethods.WPD_DEVICE_TRANSPORTS.WPD_DEVICE_TRANSPORT_BLUETOOTH
   }
}

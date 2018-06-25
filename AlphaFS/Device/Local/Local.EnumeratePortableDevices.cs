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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Enumerates all available Windows Portable Devices (WPD) on the Computer.</summary>
      /// <returns>Returns <see cref="T:IEnumerable{PortableDeviceInfo}"/> instances of type <see cref="T:DeviceGuid.WpdDevice"/> on the Computer.</returns>
      [SecurityCritical]
      public static IEnumerable<PortableDeviceInfo> EnumeratePortableDevices()
      {
         return EnumeratePortableDevicesCore(false, false);
      }


      /// <summary>[AlphaFS] Enumerates all available Windows Portable Devices (WPD) on the Computer.</summary>
      /// <param name="connect"><c>true</c> connects to the Portable Device as soon as the instance is created. <c>false</c> does not connect to the device. Use method <see cref="M:Connect()"/> to manually connect.</param>
      /// <returns>Returns <see cref="T:IEnumerable{PortableDeviceInfo}"/> instances of type <see cref="T:DeviceGuid.WpdDevice"/> on the Computer.</returns>
      [SecurityCritical]
      public static IEnumerable<PortableDeviceInfo> EnumeratePortableDevices(bool connect)
      {
         return EnumeratePortableDevicesCore(connect, false);
      }


      /// <summary>[AlphaFS] Enumerates all available Windows Portable Devices (WPD) on the local host, that use the <see cref="T:PortableDeviceProtocol.MediaTransferProtocol"/>.</summary>
      /// <param name="connect"><c>true</c> connects to the Portable Device as soon as the instance is created. <c>false</c> does not connect to the device. Use method <see cref="M:Connect()"/> to manually connect.</param>
      /// <returns>Returns <see cref="T:IEnumerable{PortableDeviceInfo}"/> instances of type <see cref="T:DeviceGuid.WpdDevice"/> on the Computer.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mtp")]
      [SecurityCritical]
      public static IEnumerable<PortableDeviceInfo> EnumeratePortableMtpDevices(bool connect)
      {
         return EnumeratePortableDevicesCore(connect, true);
      }




      [SecurityCritical]
      internal static IEnumerable<PortableDeviceInfo> EnumeratePortableDevicesCore(bool connect, bool mtpOnly)
      {
         foreach (var device in EnumerateDevicesCore(null, new[] {DeviceGuid.Wpd}, false))
         {
            var portableDeviceInfo = new PortableDeviceInfo(device, connect, mtpOnly);

            if (mtpOnly && portableDeviceInfo.DeviceProtocol != PortableDeviceProtocol.Mtp)
               continue;

            yield return portableDeviceInfo;
         }
      }
   }
}

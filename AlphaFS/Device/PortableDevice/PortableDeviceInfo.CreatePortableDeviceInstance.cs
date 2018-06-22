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

using PortableDeviceApiLib;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      /// <summary>Gets the properties of the Portable Device.</summary>
      /// <remarks>This method retrieves the Microsoft Required properties as well as some Recommended- and Optional properties.</remarks>
      private bool CreatePortableDeviceInstance()
      {
         IPortableDeviceContent deviceContent;
         IPortableDeviceProperties deviceProperties;
         IPortableDeviceValues devicePropertyValues;

         PortableDevice.Content(out deviceContent);
         deviceContent.Properties(out deviceProperties);


         // The GetValues method retrieves a list of specified properties from a specified object on a device.
         // 2nd argument null to get all properties of the device.
         deviceProperties.GetValues(PortableDeviceConstants.DeviceObjectId, null, out devicePropertyValues);


         if (SetDeviceProperties(devicePropertyValues))
         {
            uint fetched = 0;
            string objectId = null;

            try
            {
               // The EnumObjects method retrieves an interface that is used to enumerate the immediate child objects of an object.
               // It has an optional filter that can enumerate objects with specific properties. 

               IEnumPortableDeviceObjectIDs objectIds;

               deviceContent.EnumObjects(0, PortableDeviceConstants.DeviceObjectId, null, out objectIds);

               if (null != objectIds)
                  objectIds.Next(1, out objectId, ref fetched);
            }
            catch { }

            if (fetched <= 0)
               return false;


            deviceProperties.GetValues(objectId, null, out devicePropertyValues);

            SetStorageProperties(devicePropertyValues);

            return true;
         }

         return false;
      }
   }
}

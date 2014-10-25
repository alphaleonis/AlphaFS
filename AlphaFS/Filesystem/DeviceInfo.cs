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

using Alphaleonis.Win32.Network;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to information of a device, on a local or remote host.</summary>
   [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
   [SerializableAttribute]
   [SecurityCritical]
   public sealed class DeviceInfo
   {
      #region Constructors

      [NonSerialized] private readonly string _hostName;

      /// <summary>Initializes a DeviceInfo class.</summary>
      [SecurityCritical]
      public DeviceInfo()
      {
         _hostName = Host.GetUncName();
      }

      /// <summary>Initializes a DeviceInfo class.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      [SecurityCritical]
      public DeviceInfo(string host)
      {
         _hostName = Host.GetUncName(host);
      }

      #endregion // Constructors

      #region Methods

      #region EnumerateDevices

      /// <summary>Enumerates all available devices on the local host.</summary>
      /// <param name="deviceGuid">One of the <see cref="T:DeviceGuid"/> devices.</param>
      /// <returns>Returns <see cref="T:IEnumerable{DeviceInfo}"/> instances of type <see cref="T:DeviceGuid"/> from the local host.</returns>
      [SecurityCritical]
      public IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid deviceGuid)
      {
         return Device.EnumerateDevicesInternal(null, _hostName, deviceGuid);
      }
      
      #endregion // EnumerateDevices
      
      #endregion // Methods

      #region Properties

      #region BaseContainerId

      /// <summary>Represents the <see cref="T:Guid"/> value of the base container identifier (ID) .The Windows Plug and Play (PnP) manager assigns this value to the device node (devnode).</summary>
      public Guid BaseContainerId { get; internal set; }

      #endregion // BaseContainerId

      #region Class

      /// <summary>Represents the name of the device setup class that a device instance belongs to.</summary>
      public string Class { get; internal set; }

      #endregion // Class

      #region ClassGuid

      /// <summary>Represents the <see cref="T:Guid"/> of the device setup class that a device instance belongs to.</summary>
      public Guid ClassGuid { get; internal set; }

      #endregion // ClassGuid
      
      #region CompatibleIds

      /// <summary>Represents the list of compatible identifiers for a device instance.</summary>
      public string CompatibleIds { get; internal set; }

      #endregion // CompatibleIds

      #region DeviceDescription

      /// <summary>Represents a description of a device instance.</summary>
      public string DeviceDescription { get; internal set; }

      #endregion // DeviceDescription

      #region DevicePath

      /// <summary>The device interface path.</summary>
      public string DevicePath { get; internal set; }

      #endregion // DevicePath

      #region Driver

      /// <summary>Represents the registry entry name of the driver key for a device instance.</summary>
      public string Driver { get; internal set; }

      #endregion // Driver

      #region EnumeratorName

      /// <summary>Represents the name of the enumerator for a device instance.</summary>
      public string EnumeratorName { get; internal set; }

      #endregion // EnumeratorName

      #region FriendlyName

      /// <summary>Represents the friendly name of a device instance.</summary>
      public string FriendlyName { get; internal set; }

      #endregion // FriendlyName

      #region HardwareId

      /// <summary>Represents the list of hardware identifiers for a device instance.</summary>
      public string HardwareId { get; internal set; }

      #endregion // HardwareId

      #region InstanceId

      /// <summary>Gets the instance Id of the device.</summary>
      public string InstanceId { get; internal set; }

      #endregion // InstanceId

      #region LocationInformation

      /// <summary>Represents the bus-specific physical location of a device instance.</summary>
      public string LocationInformation { get; internal set; }

      #endregion // LocationInformation

      #region LocationPaths

      /// <summary>Represents the location of a device instance in the device tree.</summary>
      public string LocationPaths { get; internal set; }

      #endregion // LocationPaths

      #region Manufacturer

      /// <summary>Represents the name of the manufacturer of a device instance.</summary>
      public string Manufacturer { get; internal set; }

      #endregion // Manufacturer

      #region PhysicalDeviceObjectName

      /// <summary>Encapsulates the physical device location information provided by a device's firmware to Windows.</summary>
      public string PhysicalDeviceObjectName { get; internal set; }

      #endregion // PhysicalDeviceObjectName

      #region Service

      /// <summary>Represents the name of the service that is installed for a device instance.</summary>
      public string Service { get; internal set; }

      #endregion // Service

      #endregion // Properties
   }
}
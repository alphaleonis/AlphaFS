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

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      /// <summary>Enumerates the possible values of the PropertyId member of the <see cref="STORAGE_PROPERTY_QUERY"/> structure passed as input to the IOCTL_STORAGE_QUERY_PROPERTY request to retrieve the properties of a storage device or adapter.</summary>
      /// <remarks>
      ///   Minimum supported client: Windows XP [desktop apps only]
      ///   Minimum supported server: Windows Server 2003 [desktop apps only]
      /// </remarks>
      public enum STORAGE_PROPERTY_ID
      {
         /// <summary>Indicates that the caller is querying for the device descriptor, <see cref="STORAGE_DEVICE_DESCRIPTOR"/>.</summary>
         StorageDeviceProperty = 0,

         /// <summary>Indicates that the caller is querying for the adapter descriptor, <see cref="STORAGE_ADAPTER_DESCRIPTOR"/>.</summary>
         StorageAdapterProperty

         ///// <summary>Indicates that the caller is querying for the device identifiers provided with the SCSI vital product data pages. Data is returned using the STORAGE_DEVICE_ID_DESCRIPTOR structure.</summary>
         //StorageDeviceIdProperty,

         ///// <summary>Indicates that the caller is querying for the unique device identifiers. Data is returned using the STORAGE_DEVICE_UNIQUE_IDENTIFIER structure.</summary>
         //StorageDeviceUniqueIdProperty,

         ///// <summary>Indicates that the caller is querying for the write cache property. Data is returned using the STORAGE_WRITE_CACHE_PROPERTY structure.</summary>
         //StorageDeviceWriteCacheProperty,

         ///// <summary>Reserved for system use.</summary>
         //StorageMiniportProperty,

         ///// <summary>Indicates that the caller is querying for the access alignment descriptor, STORAGE_ACCESS_ALIGNMENT_DESCRIPTOR.</summary>
         //StorageAccessAlignmentProperty,

         ///// <summary>Indicates that the caller is querying for the seek penalty descriptor, DEVICE_SEEK_PENALTY_DESCRIPTOR.</summary>
         //StorageDeviceSeekPenaltyProperty,

         ///// <summary>Indicates that the caller is querying for the trim descriptor, DEVICE_TRIM_DESCRIPTOR.</summary>
         //StorageDeviceTrimProperty,

         ///// <summary>Reserved for system use.</summary>
         //StorageDeviceWriteAggregationProperty,

         ///// <summary>Reserved for system use.</summary>
         //StorageDeviceDeviceTelemetryProperty,

         ///// <summary>Indicates that the caller is querying for the logical block provisioning property. Data is returned using the DEVICE_LB_PROVISIONING_DESCRIPTOR structure.</summary>
         //StorageDeviceLBProvisioningProperty,

         ///// <summary>Indicates that the caller is querying for the device power descriptor. Data is returned using the DEVICE_POWER_DESCRIPTOR structure.</summary>
         //StorageDevicePowerProperty,

         ///// <summary>Indicates that the caller is querying for the copy offload parameters property. Data is returned using the DEVICE_COPY_OFFLOAD_DESCRIPTOR structure.</summary>
         //StorageDeviceCopyOffloadProperty,

         ///// <summary>Reserved for system use.</summary>
         //StorageDeviceResiliencyProperty,

         ///// <summary>Indicates that the caller is querying for the medium product type. Data is returned using the STORAGE_MEDIUM_PRODUCT_TYPE_DESCRIPTOR structure.</summary>
         //StorageDeviceMediumProductType,

         ///// <summary>Reserved for system use.</summary>
         //StorageAdapterCryptoProperty,

         ///// <summary>Indicates that the caller is querying for the device I/O capability property. Data is returned using the DEVICE_IO_CAPABILITY_DESCRIPTOR structure.</summary>
         //StorageDeviceIoCapabilityProperty = 48,

         ///// <summary>Indicates that the caller is querying for protocol-specific data from the adapter. Data is returned using the STORAGE_PROTOCOL_DATA_DESCRIPTOR structure. See the remarks for more info.</summary>
         //StorageAdapterProtocolSpecificProperty,

         ///// <summary>Indicates that the caller is querying for protocol-specific data from the device. Data is returned using the STORAGE_PROTOCOL_DATA_DESCRIPTOR structure. See the remarks for more info.</summary>
         //StorageDeviceProtocolSpecificProperty,

         ///// <summary>Indicates that the caller is querying temperature data from the adapter. Data is returned using the STORAGE_TEMPERATURE_DATA_DESCRIPTOR structure.</summary>
         //StorageAdapterTemperatureProperty,

         ///// <summary>Indicates that the caller is querying for temperature data from the device. Data is returned using the STORAGE_TEMPERATURE_DATA_DESCRIPTOR structure.</summary>
         //StorageDeviceTemperatureProperty,

         ///// <summary>Indicates that the caller is querying for topology information from the adapter. Data is returned using the STORAGE_PHYSICAL_TOPOLOGY_DESCRIPTOR structure.</summary>
         //StorageAdapterPhysicalTopologyProperty,

         ///// <summary>Indicates that the caller is querying for topology information from the device. Data is returned using the STORAGE_PHYSICAL_TOPOLOGY_DESCRIPTOR structure.</summary>
         //StorageDevicePhysicalTopologyProperty,

         ///// <summary>Indicates that the caller is querying for attributes information from the device. Data is returned using the STORAGE_DEVICE_ATTRIBUTES_DESCRIPTOR structure.</summary>
         //StorageDeviceAttributesProperty
      }
   }
}

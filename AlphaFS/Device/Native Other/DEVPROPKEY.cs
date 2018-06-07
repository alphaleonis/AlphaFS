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
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      /// <summary>In Windows Vista and later versions of Windows, the DEVPROPKEY structure represents a device property key for a device property in the unified device property model.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct DEVPROPKEY
      {
         /// <summary>A globally unique ID (GUID) for the device property.</summary>
         public Guid fmtid;

         /// <summary>A value that identifies the device property.</summary>
         public ulong pid;
      }



      private static class ClassCategories
      {
         //public static readonly Guid Name = CreateGuid(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac);
         public static readonly Guid Device1 = CreateGuid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0);
         //public static readonly Guid Device2 = CreateGuid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7);
         //public static readonly Guid Device3 = CreateGuid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e);
         //public static readonly Guid Device4 = CreateGuid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57);
         //public static readonly Guid Device5 = CreateGuid(0x8c7ed206, 0x3f8a, 0x4827, 0xb3, 0xab, 0xae, 0x9e, 0x1f, 0xae, 0xfc, 0x6c);
         //public static readonly Guid Device6 = CreateGuid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b);
         public static readonly Guid Numa = CreateGuid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2);
         //public static readonly Guid Device7 = CreateGuid(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29);
         //public static readonly Guid Device8 = CreateGuid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6);
         //public static readonly Guid DeviceSafeRemovel = CreateGuid(0xafd97640, 0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55);
         //public static readonly Guid DriverPackage = CreateGuid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32);
         //public static readonly Guid DeviceClass1 = CreateGuid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b);
         //public static readonly Guid DeviceClass2 = CreateGuid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66);
         //public static readonly Guid DeviceClass3 = CreateGuid(0x713d1703, 0xa2e2, 0x49f5, 0x92, 0x14, 0x56, 0x47, 0x2e, 0xf3, 0xda, 0x5c);
         //public static readonly Guid DeviceInterface = CreateGuid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22);
         //public static readonly Guid DeviceInterfaceClass = CreateGuid(0x14c83a99, 0x0b3f, 0x44b7, 0xbe, 0x4c, 0xa1, 0x78, 0xd3, 0x99, 0x05, 0x64);
         //public static readonly Guid AudioEndpoint = CreateGuid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e);
         //public static readonly Guid AudioEngine1 = CreateGuid(0xf19f064d, 0x82c, 0x4e27, 0xbc, 0x73, 0x68, 0x82, 0xa1, 0xbb, 0x8e, 0x4c);
         //public static readonly Guid AudioEngine2 = CreateGuid(0xe4870e26, 0x3cc5, 0x4cd2, 0xba, 0x46, 0xca, 0xa, 0x9a, 0x70, 0xed, 0x4);


         // Needed for .NET Standard 1.0 compliance .
         private static Guid CreateGuid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
         {
            return new Guid(unchecked((int)a), unchecked((short)b), unchecked((short)c), d, e, f, g, h, i, j, k);
         }
      }


      public class DEVPROPKEYS
      {
         //public static DEVPROPKEY Name = new DEVPROPKEY {fmtid = ClassCategories.Name, pid = 10 }; // DEVPROP_TYPE_STRING

         // Device properties. These correspond to the old setupapi SPDRP_XXX properties.

         //public static DEVPROPKEY DeviceDeviceDesc = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 2};  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceHardwareIds = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 3};  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceCompatibleIds = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 4 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceService = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 6 };  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClass = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 9 };  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassGuid = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 10 }; // DEVPROP_TYPE_GUID
         //public static DEVPROPKEY DeviceDriver = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 11 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceConfigFlags = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 12 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceManufacturer = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 13 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceFriendlyName = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 14 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceLocationInfo = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 15 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DevicePdoName = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 16 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceCapabilities = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 17 }; // DEVPROP_TYPE_UNINT32
         //public static DEVPROPKEY DeviceUiNumber = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 18 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceUpperFilters = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 19 }; // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceLowerFilters = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 20 }; // DEVPROP_TYPE_STRING_LIST
         public static DEVPROPKEY DeviceBusTypeGuid = new DEVPROPKEY { fmtid = ClassCategories.Device1, pid = 21 }; // DEVPROP_TYPE_GUID
         //public static DEVPROPKEY DeviceLegacyBusType = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 22 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceBusNumber = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 23 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceEnumeratorName = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 24 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceSecurity = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 25 }; // DEVPROP_TYPE_SECURITY_DESCRIPTOR
         //public static DEVPROPKEY DeviceSecuritySds = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 26 }; // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
         //public static DEVPROPKEY DeviceDevType = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 27 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceExclusive = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 28 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceCharacteristics = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 29 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceAddress = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 30 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceUiNumberDescFormat = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 31 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DevicePowerData = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 32 }; // DEVPROP_TYPE_BINARY
         //public static DEVPROPKEY DeviceRemovalPolicy = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 33 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceRemovalPolicyDefault = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 34 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceRemovalPolicyOverride = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 35 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceInstallState = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 36 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceLocationPaths = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 37 }; // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceBaseContainerId = new DEVPROPKEY {fmtid = ClassCategories.Device1, pid = 38 }; // DEVPROP_TYPE_GUID


         // Device properties. These correspond to a device's status and problem code.

         //public static DEVPROPKEY DeviceDevNodeStatus = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 2 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceProblemCode = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 3 }; // DEVPROP_TYPE_UINT32


         // Device properties. These correspond to device relations.

         //public static DEVPROPKEY DeviceEjectionRelations = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 4 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceRemovalRelations = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 5 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DevicePowerRelations = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 6 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceBusRelationsn = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 7 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceParent = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 8 };  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceChildren = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 9 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceSiblings = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 10 }; // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceTransportRelations = new DEVPROPKEY {fmtid = ClassCategories.Device2, pid = 11 }; // DEVPROP_TYPE_STRING_LIST


         // Other Device properties.

         //public static DEVPROPKEY DeviceReported = new DEVPROPKEY {fmtid = ClassCategories.Device3, pid = 2};    // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceLegacy = new DEVPROPKEY {fmtid = ClassCategories.Device3, pid = 3};    // DEVPROP_TYPE_BOOLEAN

         //public static DEVPROPKEY DeviceInstanceId = new DEVPROPKEY {fmtid = ClassCategories.Device4, pid = 256 };  // DEVPROP_TYPE_STRING

         //public static DEVPROPKEY DeviceContainerId = new DEVPROPKEY {fmtid = ClassCategories.Device5, pid = 2 }; // DEVPROP_TYPE_GUID

         //public static DEVPROPKEY DeviceModelId = new DEVPROPKEY {fmtid = ClassCategories.Device6, pid = 2 }; // DEVPROP_TYPE_GUID
         //public static DEVPROPKEY DeviceFriendlyNameAttributes = new DEVPROPKEY {fmtid = ClassCategories.Device6, pid = 3 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceManufacturerAttributes = new DEVPROPKEY {fmtid = ClassCategories.Device6, pid = 4 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DevicePresenceNotForDeviceb = new DEVPROPKEY {fmtid = ClassCategories.Device6, pid = 5 }; // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceSignalStrength = new DEVPROPKEY {fmtid = ClassCategories.Device6, pid = 6 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceIsAssociateableByUserAction = new DEVPROPKEY {fmtid = ClassCategories.Device6, pid = 7 }; // DEVPROP_TYPE_BOOLEAN

         //public static DEVPROPKEY DeviceInstallInProgress = new DEVPROPKEY { fmtid = ClassCategories.Device7, pid = 9 };     // DEVPROP_TYPE_BOOLEAN

         //public static DEVPROPKEY NumaProximityDomain = new DEVPROPKEY {fmtid = ClassCategories.Numa, pid = 1 };     // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceDhpRebalancePolicy = new DEVPROPKEY {fmtid = ClassCategories.Numa, pid = 2 };     // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceNumaNode = new DEVPROPKEY {fmtid = ClassCategories.Numa, pid = 3};     // DEVPROP_TYPE_UINT32
         public static DEVPROPKEY DeviceBusReportedDeviceDesc = new DEVPROPKEY { fmtid = ClassCategories.Numa, pid = 4 };     // DEVPROP_TYPE_STRING


         // Device driver properties.

         //public static DEVPROPKEY DeviceDriverDate = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 2 };   // DEVPROP_TYPE_FILETIME
         //public static DEVPROPKEY DeviceDriverVersion = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 3 };   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverDesc = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 4};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverInfPath = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 5};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverInfSection = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 6};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverInfSectionExt = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 7};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceMatchingDeviceId = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 8};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverProvider = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 9};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverPropPageProvider = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 10 };  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverCoInstallers = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 11 };  // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceResourcePickerTags = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 12 };  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceResourcePickerExceptions = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 13 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceDriverRank = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 14 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceDriverLogoLevel = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 15 }; // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceNoConnectSound = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 17 }; // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceGenericDriverInstalled = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 18 }; // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceAdditionalSoftwareRequested = new DEVPROPKEY {fmtid = ClassCategories.Device8, pid = 19 }; // DEVPROP_TYPE_BOOLEAN


         // Device safe-removal properties.

         //public static DEVPROPKEY DeviceSafeRemovalRequired = new DEVPROPKEY {fmtid = ClassCategories.DeviceSafeRemovel, pid = 2 }; // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceSafeRemovalRequiredOverride = new DEVPROPKEY {fmtid = ClassCategories.DeviceSafeRemovel, pid = 3 }; // DEVPROP_TYPE_BOOLEAN


         // Device properties that were set by the driver package that was installed on the device.

         //public static DEVPROPKEY DrvPkgModel = new DEVPROPKEY {fmtid = ClassCategories.DriverPackage, pid = 2};     // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DrvPkgVendorWebSite = new DEVPROPKEY {fmtid = ClassCategories.DriverPackage, pid = 3};     // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DrvPkgDetailedDescription = new DEVPROPKEY {fmtid = ClassCategories.DriverPackage, pid = 4};     // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DrvPkgDocumentationLink = new DEVPROPKEY {fmtid = ClassCategories.DriverPackage, pid = 5};     // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DrvPkgIcon = new DEVPROPKEY {fmtid = ClassCategories.DriverPackage, pid = 6};     // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DrvPkgBrandingIcon = new DEVPROPKEY {fmtid = ClassCategories.DriverPackage, pid = 7};     // DEVPROP_TYPE_STRING_LIST


         // Device setup class properties. These correspond to the old setupapi SPCRP_XXX properties.

         //public static DEVPROPKEY DeviceClassUpperFilters = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 19};    // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceClassLowerFilters = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 20};    // DEVPROP_TYPE_STRING_LIST
         //public static DEVPROPKEY DeviceClassSecurity = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 25};    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
         //public static DEVPROPKEY DeviceClassSecuritySds = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 26};    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
         //public static DEVPROPKEY DeviceClassDevType = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 27};    // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceClassExclusive = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 28};    // DEVPROP_TYPE_UINT32
         //public static DEVPROPKEY DeviceClassCharacteristics = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass1, pid = 29};    // DEVPROP_TYPE_UINT32


         // Device setup class properties. These correspond to registry values under the device class GUID key.

         //public static DEVPROPKEY DeviceClassName = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 2 };   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassClassName = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 3};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassIcon = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 4};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassClassInstaller = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 5};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassPropPageProvider = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 6};   // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassNoInstallClass = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 7};   // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceClassNoDisplayClass = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 8};   // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceClassSilentInstall = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 9};   // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceClassNoUseClass = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 10 };  // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceClassDefaultService = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 11 };  // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceClassIconPath = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass2, pid = 12 };  // DEVPROP_TYPE_STRING_LIST


         // Other Device setup class properties.

         //public static DEVPROPKEY DeviceClassClassCoInstallers = new DEVPROPKEY {fmtid = ClassCategories.DeviceClass3, pid = 2 }; // DEVPROP_TYPE_STRING_LIST


         // Device interface properties.

         //public static DEVPROPKEY DeviceInterfaceFriendlyName = new DEVPROPKEY {fmtid = ClassCategories.DeviceInterface, pid = 2 }; // DEVPROP_TYPE_STRING
         //public static DEVPROPKEY DeviceInterfaceEnabled = new DEVPROPKEY {fmtid = ClassCategories.DeviceInterface, pid = 3 }; // DEVPROP_TYPE_BOOLEAN
         //public static DEVPROPKEY DeviceInterfaceClassGuid = new DEVPROPKEY {fmtid = ClassCategories.DeviceInterface, pid = 4 }; // DEVPROP_TYPE_GUID


         // Device interface class properties.

         //public static DEVPROPKEY DeviceInterfaceClassDefaultInterface = new DEVPROPKEY {fmtid = ClassCategories.DeviceInterfaceClass, pid = 2};  // DEVPROP_TYPE_STRING

         //public static DEVPROPKEY AudioEndpointFormFactor = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 0};
         //public static DEVPROPKEY AudioEndpointControlPanelPageProvider = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 1};
         //public static DEVPROPKEY AudioEndpointAssociation = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 2};
         //public static DEVPROPKEY AudioEndpointPhysicalSpeakers = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 3};
         //public static DEVPROPKEY AudioEndpointGuid = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 4};
         //public static DEVPROPKEY AudioEndpointDisableSysFx = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 5};
         //public static DEVPROPKEY AudioEndpointFullRangeSpeakers = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 6};
         //public static DEVPROPKEY AudioEndpointSupportsEventDrivenMode = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 7};
         //public static DEVPROPKEY AudioEndpointJackSubType = new DEVPROPKEY {fmtid = ClassCategories.AudioEndpoint, pid = 8};
         //public static DEVPROPKEY AudioEngineDeviceFormat = new DEVPROPKEY {fmtid = ClassCategories.AudioEngine1, pid = 0};
         //public static DEVPROPKEY AudioEngineOemFormat = new DEVPROPKEY {fmtid = ClassCategories.AudioEngine2, pid = 3};
      }
   }
}

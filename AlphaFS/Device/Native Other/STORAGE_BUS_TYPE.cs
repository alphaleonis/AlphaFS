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
      /// <summary>Provides a symbolic means of representing storage bus types.</summary>
      /// <remarks>
      ///   Minimum supported client: Windows XP [desktop apps only]
      ///   Minimum supported server: Windows Server 2003 [desktop apps only]
      /// </remarks>
      internal enum STORAGE_BUS_TYPE
      {
         /// <summary>Indicates an unknown bus type.</summary>
         BusTypeUnknown = 0,

         /// <summary>Indicates a SCSI bus type.</summary>
         BusTypeScsi = 1,

         /// <summary>Indicates an ATAPI bus type.</summary>
         BusTypeAtapi = 2,

         /// <summary>Indicates an ATA bus type.</summary>
         BusTypeAta = 3,

         /// <summary>Indicates an IEEE 1394 bus type.</summary>
         BusType1394 = 4,

         /// <summary>Indicates an SSA bus type.</summary>
         BusTypeSsa = 5,

         /// <summary>Indicates a fiber channel bus type.</summary>
         BusTypeFibre = 6,

         /// <summary>Indicates a USB bus type.</summary>
         BusTypeUsb = 7,

         /// <summary>Indicates a RAID bus type.</summary>
         BusTypeRAID = 8,

         /// <summary>Indicates an iSCSI bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeiScsi = 9,

         /// <summary>Indicates a serial-attached SCSI (SAS) bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeSas = 10,

         /// <summary>Indicates a SATA bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeSata = 11,

         /// <summary>Indicates a secure digital (SD) bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeSd = 12,

         /// <summary>Indicates a multimedia card (MMC) bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeMmc = 13,

         /// <summary>Indicates a virtual bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeVirtual = 14,

         /// <summary>Indicates a file-backed virtual bus type.</summary>
         /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
         BusTypeFileBackedVirtual = 15,

         /// <summary>Indicates the maximum value for this value.</summary>
         BusTypeMax = 16,

         /// <summary>The maximum value of the storage bus type range.</summary>
         BusTypeMaxReserved = 127
      }
   }
}

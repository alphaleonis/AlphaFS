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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Device
{
   /// <summary>Provides a symbolic means of representing storage bus types.</summary>
   public enum StorageBusType
   {
      /// <summary>Indicates an unknown bus type.</summary>
      Unknown = NativeMethods.STORAGE_BUS_TYPE.BusTypeUnknown,

      /// <summary>Indicates a SCSI bus type.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Scsi")]
      Scsi = NativeMethods.STORAGE_BUS_TYPE.BusTypeScsi,

      /// <summary>Indicates an ATAPI bus type.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Atapi")]
      Atapi = NativeMethods.STORAGE_BUS_TYPE.BusTypeAtapi,

      /// <summary>Indicates an ATA bus type.</summary>
      Ata = NativeMethods.STORAGE_BUS_TYPE.BusTypeAta,

      /// <summary>Indicates an IEEE 1394 bus type.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ieee")]
      Ieee1394 = NativeMethods.STORAGE_BUS_TYPE.BusType1394,

      /// <summary>Indicates an SSA bus type.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ssa")]
      Ssa = NativeMethods.STORAGE_BUS_TYPE.BusTypeSsa,

      /// <summary>Indicates a fiber channel bus type.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fibre")]
      Fibre = NativeMethods.STORAGE_BUS_TYPE.BusTypeFibre,

      /// <summary>Indicates a USB bus type.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Usb")]
      Usb = NativeMethods.STORAGE_BUS_TYPE.BusTypeUsb,

      /// <summary>Indicates a RAID bus type.</summary>
      Raid = NativeMethods.STORAGE_BUS_TYPE.BusTypeRAID,

      /// <summary>Indicates an iSCSI bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Scsi")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "i")]
      iScsi = NativeMethods.STORAGE_BUS_TYPE.BusTypeiScsi,

      /// <summary>Indicates a serial-attached SCSI (SAS) bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sas")]
      Sas = NativeMethods.STORAGE_BUS_TYPE.BusTypeSas,

      /// <summary>Indicates a SATA bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sata")]
      Sata = NativeMethods.STORAGE_BUS_TYPE.BusTypeSata,

      /// <summary>Indicates a secure digital (SD) bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sd")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Sd")]
      Sd = NativeMethods.STORAGE_BUS_TYPE.BusTypeSd,

      /// <summary>Indicates a multimedia card (MMC) bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mmc")]
      Mmc = NativeMethods.STORAGE_BUS_TYPE.BusTypeMmc,

      /// <summary>Indicates a virtual bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      Virtual = NativeMethods.STORAGE_BUS_TYPE.BusTypeVirtual,

      /// <summary>Indicates a file-backed virtual bus type.</summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported before Windows Vista and Windows Server 2008.</remarks>
      FileBackedVirtual = NativeMethods.STORAGE_BUS_TYPE.BusTypeFileBackedVirtual,
   }
}

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
   /// <summary>Specifies various types of storage media.</summary>
   [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
   public enum StorageMediaType
   {
      /// <summary>Format is unknown.</summary>
      Unknown = NativeMethods.MEDIA_TYPE.Unknown,

      /// <summary>A 5.25" floppy, with 1.2MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      Floppy51Pt2512 = NativeMethods.MEDIA_TYPE.F5_1Pt2_512,

      /// <summary>A 3.5" floppy, with 1.44MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      Floppy31Pt44512 = NativeMethods.MEDIA_TYPE.F3_1Pt44_512,

      /// <summary>A 3.5" floppy, with 2.88MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      Floppy32Pt88512 = NativeMethods.MEDIA_TYPE.F3_2Pt88_512,

      /// <summary>A 3.5" floppy, with 20.8MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      Floppy320Pt8512 = NativeMethods.MEDIA_TYPE.F3_20Pt8_512,

      /// <summary>A 3.5" floppy, with 720KB and 512 bytes/sector.</summary>
      Floppy3720512 = NativeMethods.MEDIA_TYPE.F3_720_512,

      /// <summary>A 5.25" floppy, with 360KB and 512 bytes/sector.</summary>
      Floppy5360512 = NativeMethods.MEDIA_TYPE.F5_360_512,

      /// <summary>A 5.25" floppy, with 320KB and 512 bytes/sector.</summary>
      Floppy5320512 = NativeMethods.MEDIA_TYPE.F5_320_512,

      /// <summary>A 5.25" floppy, with 320KB and 1024 bytes/sector.</summary>
      Floppy53201024 = NativeMethods.MEDIA_TYPE.F5_320_1024,

      /// <summary>A 5.25" floppy, with 180KB and 512 bytes/sector.</summary>
      Floppy5180512 = NativeMethods.MEDIA_TYPE.F5_180_512,

      /// <summary>A 5.25" floppy, with 160KB and 512 bytes/sector.</summary>
      Floppy5160512 = NativeMethods.MEDIA_TYPE.F5_160_512,

      /// <summary>Removable media other than floppy.</summary>
      RemovableMedia = NativeMethods.MEDIA_TYPE.RemovableMedia,

      /// <summary>Fixed hard disk media.</summary>
      FixedMedia = NativeMethods.MEDIA_TYPE.FixedMedia,

      /// <summary>A 3.5" floppy, with 120MB and 512 bytes/sector.</summary>
      Floppy3120M512 = NativeMethods.MEDIA_TYPE.F3_120M_512,

      /// <summary>A 3.5" floppy, with 640KB and 512 bytes/sector.</summary>
      Floppy3640512 = NativeMethods.MEDIA_TYPE.F3_640_512,

      /// <summary>A 5.25" floppy, with 640KB and 512 bytes/sector.</summary>
      Floppy5640512 = NativeMethods.MEDIA_TYPE.F5_640_512,

      /// <summary>A 5.25" floppy, with 720KB and 512 bytes/sector.</summary>
      Floppy5720512 = NativeMethods.MEDIA_TYPE.F5_720_512,

      /// <summary>A 3.5" floppy, with 1.2MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      Floppy31Pt2512 = NativeMethods.MEDIA_TYPE.F3_1Pt2_512,

      /// <summary>A 3.5" floppy, with 1.23MB and 1024 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      Floppy31Pt231024 = NativeMethods.MEDIA_TYPE.F3_1Pt23_1024,

      /// <summary>A 5.25" floppy, with 1.23MB and 1024 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pt")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pt")]
      Floppy51Pt231024 = NativeMethods.MEDIA_TYPE.F5_1Pt23_1024,

      /// <summary>A 3.5" floppy, with 128MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mb")]
      Floppy3128Mb512 = NativeMethods.MEDIA_TYPE.F3_128Mb_512,

      /// <summary>A 3.5" floppy, with 230MB and 512 bytes/sector.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mb")]
      Floppy3230Mb512 = NativeMethods.MEDIA_TYPE.F3_230Mb_512,

      /// <summary>An 8" floppy, with 256KB and 128 bytes/sector.</summary>
      Floppy8256128 = NativeMethods.MEDIA_TYPE.F8_256_128,

      /// <summary>A 3.5" floppy, with 200MB and 512 bytes/sector. (HiFD).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mb")]
      Floppy3200Mb512 = NativeMethods.MEDIA_TYPE.F3_200Mb_512,

      /// <summary>A 3.5" floppy, with 240MB and 512 bytes/sector. (HiFD).</summary>
      Floppy3240M512 = NativeMethods.MEDIA_TYPE.F3_240M_512,

      /// <summary>A 3.5" floppy, with 32MB and 512 bytes/sector.</summary>
      Floppy332M512 = NativeMethods.MEDIA_TYPE.F3_32M_512,

      /// <summary>One of the following tape types: DAT, DDS1, DDS2, and so on.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dds")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      Dds4Mm = NativeMethods.MEDIA_TYPE.DDS_4mm,

      /// <summary>MiniQIC tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qic")]
      MiniQic = NativeMethods.MEDIA_TYPE.MiniQic,

      /// <summary>Travan tape (TR-1, TR-2, TR-3, and so on).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Travan")]
      Travan = NativeMethods.MEDIA_TYPE.Travan,

      /// <summary>QIC tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qic")]
      Qic = NativeMethods.MEDIA_TYPE.QIC,

      /// <summary>An 8mm Exabyte metal particle tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mp")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mp")]
      Mp8Mm = NativeMethods.MEDIA_TYPE.MP_8mm,

      /// <summary>An 8mm Exabyte advanced metal evaporative tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ame")]
      Ame8Mm = NativeMethods.MEDIA_TYPE.AME_8mm,

      /// <summary>An 8mm Sony AIT1 tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ait")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      Ait18Mm = NativeMethods.MEDIA_TYPE.AIT1_8mm,

      /// <summary>DLT compact tape (IIIxt or IV).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dlt")]
      Dlt = NativeMethods.MEDIA_TYPE.DLT,

      /// <summary>Philips NCTP tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nctp")]
      Nctp = NativeMethods.MEDIA_TYPE.NCTP,

      /// <summary>IBM 3480 tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ibm")]
      Ibm3480 = NativeMethods.MEDIA_TYPE.IBM_3480,

      /// <summary>IBM 3490E tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ibm")]
      Ibm3490E = NativeMethods.MEDIA_TYPE.IBM_3490E,

      /// <summary>IBM Magstar 3590 tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ibm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Magstar")]
      IbmMagstar3590 = NativeMethods.MEDIA_TYPE.IBM_Magstar_3590,

      /// <summary>IBM Magstar MP tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Magstar")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ibm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mp")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mp")]
      IbmMagstarMp = NativeMethods.MEDIA_TYPE.IBM_Magstar_MP,

      /// <summary>STK data D3 tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stk")]
      StkDataD3 = NativeMethods.MEDIA_TYPE.STK_DATA_D3,

      /// <summary>Sony DTF tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dtf")]
      SonyDtf = NativeMethods.MEDIA_TYPE.SONY_DTF,

      /// <summary>A 6mm digital videotape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dv")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dv")]
      Dv6Mm = NativeMethods.MEDIA_TYPE.DV_6mm,

      /// <summary>Exabyte DMI tape (or compatible).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dmi")]
      Dmi = NativeMethods.MEDIA_TYPE.DMI,

      /// <summary>Sony D2S or D2L tape.</summary>
      SonyD2 = NativeMethods.MEDIA_TYPE.SONY_D2,

      /// <summary>Cleaner (all drive types that support cleaners).</summary>
      CleanerCartridge = NativeMethods.MEDIA_TYPE.CLEANER_CARTRIDGE,

      /// <summary>CD.</summary>
      CDRom = NativeMethods.MEDIA_TYPE.CD_ROM,

      /// <summary>CD (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cd")]
      CdR = NativeMethods.MEDIA_TYPE.CD_R,

      /// <summary>CD (rewriteable).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cd")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      CdRw = NativeMethods.MEDIA_TYPE.CD_RW,

      /// <summary>DVD.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dvd")]
      DvdRom = NativeMethods.MEDIA_TYPE.DVD_ROM,

      /// <summary>DVD (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dvd")]
      DvdR = NativeMethods.MEDIA_TYPE.DVD_R,

      /// <summary>DVD (rewriteable).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dvd")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      DvdRw = NativeMethods.MEDIA_TYPE.DVD_RW,

      /// <summary>Magneto-optical 3.5" (rewriteable).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mo")]
      Mo3Rw = NativeMethods.MEDIA_TYPE.MO_3_RW,

      /// <summary>Magneto-optical 5.25" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mo")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      Mo5Wo = NativeMethods.MEDIA_TYPE.MO_5_WO,

      /// <summary>Magneto-optical 5.25" (rewriteable; not LIMDOW).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mo")]
      Mo5Rw = NativeMethods.MEDIA_TYPE.MO_5_RW,

      /// <summary>Magneto-optical 5.25" (rewriteable; LIMDOW).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Limdow")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mo")]
      Mo5Limdow = NativeMethods.MEDIA_TYPE.MO_5_LIMDOW,

      /// <summary>Phase change 5.25" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pc")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      Pc5Wo = NativeMethods.MEDIA_TYPE.PC_5_WO,

      /// <summary>Phase change 5.25" (rewriteable).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pc")]
      Pc5Rw = NativeMethods.MEDIA_TYPE.PC_5_RW,

      /// <summary>Phase change dual (rewriteable).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pd")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pd")]
      Pd5Rw = NativeMethods.MEDIA_TYPE.PD_5_RW,

      /// <summary>Ablative 5.25" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abl")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      Abl5Wo = NativeMethods.MEDIA_TYPE.ABL_5_WO,

      /// <summary>Pinnacle Apex 4.6GB (rewriteable)</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      PinnacleApex5Rw = NativeMethods.MEDIA_TYPE.PINNACLE_APEX_5_RW,

      /// <summary>Sony 12" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      Sony12Wo = NativeMethods.MEDIA_TYPE.SONY_12_WO,

      /// <summary>Philips/LMS 12" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      Philips12Wo = NativeMethods.MEDIA_TYPE.PHILIPS_12_WO,

      /// <summary>Hitachi 12" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      Hitachi12Wo = NativeMethods.MEDIA_TYPE.HITACHI_12_WO,

      /// <summary>Cygnet/ATG 12" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      Cygnet12Wo = NativeMethods.MEDIA_TYPE.CYGNET_12_WO,

      /// <summary>Kodak 14" (write once).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wo")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wo")]
      Kodak14Wo = NativeMethods.MEDIA_TYPE.KODAK_14_WO,

      /// <summary>MO near field recording (Terastor).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mo")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nfr")]
      MoNfr525 = NativeMethods.MEDIA_TYPE.MO_NFR_525,

      /// <summary>Nikon 12" (rewriteable).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Rw")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rw")]
      Nikon12Rw = NativeMethods.MEDIA_TYPE.NIKON_12_RW,

      /// <summary>Iomega Zip.</summary>
      IomegaZip = NativeMethods.MEDIA_TYPE.IOMEGA_ZIP,

      /// <summary>Iomega Jaz.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Jaz")]
      IomegaJaz = NativeMethods.MEDIA_TYPE.IOMEGA_JAZ,

      /// <summary>Syquest EZ135.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ez")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ez")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Syquest")]
      SyquestEz135 = NativeMethods.MEDIA_TYPE.SYQUEST_EZ135,

      /// <summary>Syquest EzFlyer.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ezflyer")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Syquest")]
      SyquestEzflyer = NativeMethods.MEDIA_TYPE.SYQUEST_EZFLYER,

      /// <summary>Syquest SyJet.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Syjet")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Syquest")]
      SyquestSyjet = NativeMethods.MEDIA_TYPE.SYQUEST_SYJET,

      /// <summary>Avatar 2.5" floppy.</summary>
      AvatarF2 = NativeMethods.MEDIA_TYPE.AVATAR_F2,

      /// <summary>An 8mm Hitachi tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mp")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mp")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      Mp28Mm = NativeMethods.MEDIA_TYPE.MP2_8mm,

      /// <summary>Ampex DST small tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dst")]
      DstS = NativeMethods.MEDIA_TYPE.DST_S,

      /// <summary>Ampex DST medium tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dst")]
      DstM = NativeMethods.MEDIA_TYPE.DST_M,

      /// <summary>Ampex DST large tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dst")]
      DstL = NativeMethods.MEDIA_TYPE.DST_L,

      /// <summary>Ecrix 8mm tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vxa")]
      VxaTape1 = NativeMethods.MEDIA_TYPE.VXATape_1,

      /// <summary>Ecrix 8mm tape.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vxa")]
      VxaTape2 = NativeMethods.MEDIA_TYPE.VXATape_2,

      /// <summary>STK 9840.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stk")]
      Stk9840 = NativeMethods.MEDIA_TYPE.STK_9840,

      /// <summary>LTO Ultrium (IBM, HP, Seagate).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lto")]
      LtoUltrium = NativeMethods.MEDIA_TYPE.LTO_Ultrium,

      /// <summary>LTO Accelis (IBM, HP, Seagate).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Accelis")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lto")]
      LtoAccelis = NativeMethods.MEDIA_TYPE.LTO_Accelis,

      /// <summary>DVD-RAM.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dvd")]
      DvdRam = NativeMethods.MEDIA_TYPE.DVD_RAM,

      /// <summary>AIT tape (AIT2 or higher).</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ait")]
      Ait8Mm = NativeMethods.MEDIA_TYPE.AIT_8mm,

      /// <summary>OnStream ADR1.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Adr")]
      Adr1 = NativeMethods.MEDIA_TYPE.ADR_1,

      /// <summary>OnStream ADR2.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Adr")]
      Adr2 = NativeMethods.MEDIA_TYPE.ADR_2,

      /// <summary>STK 9940.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stk")]
      Stk9940 = NativeMethods.MEDIA_TYPE.STK_9940,

      /// <summary>SAIT tape.
      /// <para>Windows Server 2003: This is not supported before Windows Server 2003 with SP1.</para>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sait")]
      Sait = NativeMethods.MEDIA_TYPE.SAIT,

      /// <summary>Exabyte VXA tape.
      /// <para>Windows Server 2008: This is not supported before Windows Server 2008.</para>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vxa")]
      VxaTape = NativeMethods.MEDIA_TYPE.VXATape
   }
}

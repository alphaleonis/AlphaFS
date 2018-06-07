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
      /// <summary>Specifies various types of storage media. Parameters and members of type STORAGE_MEDIA_TYPE also accept values from the MEDIA_TYPE enumeration type.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      public enum STORAGE_MEDIA_TYPE
      {
         /// <summary>Format is unknown.</summary>
         Unknown = 0,

         /// <summary>A 5.25" floppy, with 1.2MB and 512 bytes/sector.</summary>
         F5_1Pt2_512 = 1,

         /// <summary>A 3.5" floppy, with 1.44MB and 512 bytes/sector.</summary>
         F3_1Pt44_512 = 2,

         /// <summary>A 3.5" floppy, with 2.88MB and 512 bytes/sector.</summary>
         F3_2Pt88_512 = 3,

         /// <summary>A 3.5" floppy, with 20.8MB and 512 bytes/sector.</summary>
         F3_20Pt8_512 = 4,

         /// <summary>A 3.5" floppy, with 720KB and 512 bytes/sector.</summary>
         F3_720_512 = 5,

         /// <summary>A 5.25" floppy, with 360KB and 512 bytes/sector.</summary>
         F5_360_512 = 6,

         /// <summary>A 5.25" floppy, with 320KB and 512 bytes/sector.</summary>
         F5_320_512 = 7,

         /// <summary>A 5.25" floppy, with 320KB and 1024 bytes/sector.</summary>
         F5_320_1024 = 8,

         /// <summary>A 5.25" floppy, with 180KB and 512 bytes/sector.</summary>
         F5_180_512 = 9,

         /// <summary>A 5.25" floppy, with 160KB and 512 bytes/sector.</summary>
         F5_160_512 = 10,

         /// <summary>Removable media other than floppy.</summary>
         RemovableMedia = 11,

         /// <summary>Fixed hard disk media.</summary>
         FixedMedia = 12,

         /// <summary>A 3.5" floppy, with 120MB and 512 bytes/sector.</summary>
         F3_120M_512 = 13,

         /// <summary>A 3.5" floppy, with 640KB and 512 bytes/sector.</summary>
         F3_640_512 = 14,

         /// <summary>A 5.25" floppy, with 640KB and 512 bytes/sector.</summary>
         F5_640_512 = 15,

         /// <summary>A 5.25" floppy, with 720KB and 512 bytes/sector.</summary>
         F5_720_512 = 16,

         /// <summary>A 3.5" floppy, with 1.2MB and 512 bytes/sector.</summary>
         F3_1Pt2_512 = 17,

         /// <summary>A 3.5" floppy, with 1.23MB and 1024 bytes/sector.</summary>
         F3_1Pt23_1024 = 18,

         /// <summary>A 5.25" floppy, with 1.23MB and 1024 bytes/sector.</summary>
         F5_1Pt23_1024 = 19,

         /// <summary>A 3.5" floppy, with 128MB and 512 bytes/sector.</summary>
         F3_128Mb_512 = 20,

         /// <summary>A 3.5" floppy, with 230MB and 512 bytes/sector.</summary>
         F3_230Mb_512 = 21,

         /// <summary>An 8" floppy, with 256KB and 128 bytes/sector.</summary>
         F8_256_128 = 22,

         /// <summary>A 3.5" floppy, with 200MB and 512 bytes/sector. (HiFD).</summary>
         F3_200Mb_512 = 23,

         /// <summary>A 3.5" floppy, with 240MB and 512 bytes/sector. (HiFD).</summary>
         F3_240M_512 = 24,

         /// <summary>A 3.5" floppy, with 32MB and 512 bytes/sector.</summary>
         F3_32M_512 = 25,

         /// <summary>One of the following tape types: DAT, DDS1, DDS2, and so on.</summary>
         DDS_4mm = 32,

         /// <summary>MiniQIC tape.</summary>
         MiniQic = 33,

         /// <summary>Travan tape (TR-1, TR-2, TR-3, and so on).</summary>
         Travan = 34,

         /// <summary>QIC tape.</summary>
         QIC = 35,

         /// <summary>An 8mm Exabyte metal particle tape.</summary>
         MP_8mm = 36,

         /// <summary>An 8mm Exabyte advanced metal evaporative tape.</summary>
         AME_8mm = 37,

         /// <summary>An 8mm Sony AIT1 tape.</summary>
         AIT1_8mm = 38,

         /// <summary>DLT compact tape (IIIxt or IV).</summary>
         DLT = 39,

         /// <summary>Philips NCTP tape.</summary>
         NCTP = 40,

         /// <summary>IBM 3480 tape.</summary>
         IBM_3480 = 41,

         /// <summary>IBM 3490E tape.</summary>
         IBM_3490E = 42,

         /// <summary>IBM Magstar 3590 tape.</summary>
         IBM_Magstar_3590 = 43,

         /// <summary>IBM Magstar MP tape.</summary>
         IBM_Magstar_MP = 44,

         /// <summary>STK data D3 tape.</summary>
         STK_DATA_D3 = 45,

         /// <summary>Sony DTF tape.</summary>
         SONY_DTF = 46,

         /// <summary>A 6mm digital videotape.</summary>
         DV_6mm = 47,

         /// <summary>Exabyte DMI tape (or compatible).</summary>
         DMI = 48,

         /// <summary>Sony D2S or D2L tape.</summary>
         SONY_D2 = 49,

         /// <summary>Cleaner (all drive types that support cleaners).</summary>
         CLEANER_CARTRIDGE = 50,

         /// <summary>CD.</summary>
         CD_ROM = 51,

         /// <summary>CD (write once).</summary>
         CD_R = 52,

         /// <summary>CD (rewriteable).</summary>
         CD_RW = 53,

         /// <summary>DVD.</summary>
         DVD_ROM = 54,

         /// <summary>DVD (write once).</summary>
         DVD_R = 55,

         /// <summary>DVD (rewriteable).</summary>
         DVD_RW = 56,

         /// <summary>Magneto-optical 3.5" (rewriteable).</summary>
         MO_3_RW = 57,

         /// <summary>Magneto-optical 5.25" (write once).</summary>
         MO_5_WO = 58,

         /// <summary>Magneto-optical 5.25" (rewriteable; not LIMDOW).</summary>
         MO_5_RW = 59,

         /// <summary>Magneto-optical 5.25" (rewriteable; LIMDOW).</summary>
         MO_5_LIMDOW = 60,

         /// <summary>Phase change 5.25" (write once).</summary>
         PC_5_WO = 61,

         /// <summary>Phase change 5.25" (rewriteable).</summary>
         PC_5_RW = 62,

         /// <summary>Phase change dual (rewriteable).</summary>
         PD_5_RW = 63,

         /// <summary>Ablative 5.25" (write once).</summary>
         ABL_5_WO = 64,

         /// <summary>Pinnacle Apex 4.6GB (rewriteable)</summary>
         PINNACLE_APEX_5_RW = 65,

         /// <summary>Sony 12" (write once).</summary>
         SONY_12_WO = 66,

         /// <summary>Philips/LMS 12" (write once).</summary>
         PHILIPS_12_WO = 67,

         /// <summary>Hitachi 12" (write once).</summary>
         HITACHI_12_WO = 68,

         /// <summary>Cygnet/ATG 12" (write once).</summary>
         CYGNET_12_WO = 69,

         /// <summary>Kodak 14" (write once).</summary>
         KODAK_14_WO = 70,

         /// <summary>MO near field recording (Terastor).</summary>
         MO_NFR_525 = 71,

         /// <summary>Nikon 12" (rewriteable).</summary>
         NIKON_12_RW = 72,

         /// <summary>Iomega Zip.</summary>
         IOMEGA_ZIP = 73,

         /// <summary>Iomega Jaz.</summary>
         IOMEGA_JAZ = 74,

         /// <summary>Syquest EZ135.</summary>
         SYQUEST_EZ135 = 75,

         /// <summary>Syquest EzFlyer.</summary>
         SYQUEST_EZFLYER = 76,

         /// <summary>Syquest SyJet.</summary>
         SYQUEST_SYJET = 77,

         /// <summary>Avatar 2.5" floppy.</summary>
         AVATAR_F2 = 78,

         /// <summary>An 8mm Hitachi tape.</summary>
         MP2_8mm = 79,

         /// <summary>Ampex DST small tape.</summary>
         DST_S = 80,

         /// <summary>Ampex DST medium tape.</summary>
         DST_M = 81,

         /// <summary>Ampex DST large tape.</summary>
         DST_L = 82,

         /// <summary>Ecrix 8mm tape.</summary>
         VXATape_1 = 83,

         /// <summary>Ecrix 8mm tape.</summary>
         VXATape_2 = 84,

         /// <summary>STK 9840.</summary>
         STK_9840 = 85,

         /// <summary>LTO Ultrium (IBM, HP, Seagate).</summary>
         LTO_Ultrium = 86,

         /// <summary>LTO Accelis (IBM, HP, Seagate).</summary>
         LTO_Accelis = 87,

         /// <summary>DVD-RAM.</summary>
         DVD_RAM = 88,

         /// <summary>AIT tape (AIT2 or higher).</summary>
         AIT_8mm = 89,

         /// <summary>OnStream ADR1.</summary>
         ADR_1 = 90,

         /// <summary>OnStream ADR2.</summary>
         ADR_2 = 91,

         /// <summary>STK 9940.</summary>
         STK_9940 = 92,

         /// <summary>SAIT tape.
         /// <para>Windows Server 2003: This is not supported before Windows Server 2003 with SP1.</para>
         /// </summary>
         SAIT = 93,

         /// <summary>Exabyte VXA tape.
         /// <para>Windows Server 2008: This is not supported before Windows Server 2008.</para>
         /// </summary>
         VXATape = 94
      }
   }
}

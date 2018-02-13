/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Represents the various forms of device media.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      internal enum MEDIA_TYPE
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
         F3_32M_512 = 25
      }
   }
}

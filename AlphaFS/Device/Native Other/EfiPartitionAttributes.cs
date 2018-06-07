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
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Device
{
   /// <summary>The Extensible Firmware Interface (EFI) partition attributes.</summary>
   [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Efi")]
   [Flags]
   public enum EfiPartitionAttributes
   {
      /// <summary>None.</summary>
      None = 0,

      /// <summary>If this attribute is set, the partition is required by a Computer to function properly.</summary>
      PlatformRequired,

      /// <summary>If this attribute is set, the partition is an MBR partition.</summary>
      BiosBoot = 4,

      /// <summary>If this attribute is set, the partition is read-only.</summary>
      ReadOnly,

      /// <summary>If this attribute is set, the partition is a shadow copy of another partition.</summary>
      ShadowCopy,

      /// <summary>If this attribute is set, the partition is not detected by the Mount Manager.</summary>
      Hidden,

      /// <summary>If this attribute is set, the partition does not receive a drive letter by default when the disk is moved to another Computer or when the disk is seen for the first time by a Computer.</summary>
      NoDriveLetter
   }
}

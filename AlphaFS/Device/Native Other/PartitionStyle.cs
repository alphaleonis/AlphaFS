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
   /// <summary>Represents the format of a partition.</summary>
   /// <remarks>
   /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
   /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
   /// </remarks>
   public enum PartitionStyle
   {
      /// <summary>Master boot record (MBR) format. This corresponds to standard AT-style MBR partitions.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mbr")]
      Mbr = NativeMethods.PARTITION_STYLE.PARTITION_STYLE_MBR,

      /// <summary>GUID Partition Table (GPT) format.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt")]
      Gpt = NativeMethods.PARTITION_STYLE.PARTITION_STYLE_GPT,

      /// <summary>Partition not formatted in either of the recognized formats; MBR or GPT.</summary>
      Raw = NativeMethods.PARTITION_STYLE.PARTITION_STYLE_RAW
   }
}

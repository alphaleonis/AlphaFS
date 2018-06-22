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

namespace Alphaleonis.Win32.Device
{
   /// <summary>Describes the different Windows Portable Device storage types. </summary>
   public enum PortableDeviceStorageType
   {
      /// <summary>The storage is of an undefined type.</summary>
      Undefined = NativeMethods.WPD_STORAGE_TYPE_VALUES.WPD_STORAGE_TYPE_UNDEFINED,

      /// <summary>The storage is non-removable and read-only.</summary>
      FixedRom = NativeMethods.WPD_STORAGE_TYPE_VALUES.WPD_STORAGE_TYPE_FIXED_ROM,

      /// <summary>The storage is removable and is read-only.</summary>
      RemovableRom = NativeMethods.WPD_STORAGE_TYPE_VALUES.WPD_STORAGE_TYPE_REMOVABLE_ROM,

      /// <summary>The storage is non-removable and is read/write capable.</summary>
      FixedRam = NativeMethods.WPD_STORAGE_TYPE_VALUES.WPD_STORAGE_TYPE_FIXED_RAM,

      /// <summary>The storage is removable and is read/write capable.</summary>
      RemovableRam = NativeMethods.WPD_STORAGE_TYPE_VALUES.WPD_STORAGE_TYPE_REMOVABLE_RAM
   }
}

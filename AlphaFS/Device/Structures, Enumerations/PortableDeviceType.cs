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

using PortableDeviceApiLib;

namespace Alphaleonis.Win32.Device
{
   /// <summary>Describes the different Windows Portable Device (WPD) types commonly used to determine the basic classification and visual appearance of a portable device.</summary>
   /// <remarks>
   /// <see cref="PortableDeviceType"/> are read using the <see cref="IPortableDeviceManager"/> interface.
   /// WPD applications may use these values to determine the generic visual appearance of the device.
   /// That is, a camera picture is displayed for camera-like devices, a mobile phone picture is displayed for phone-like devices, and so on.
   /// Note: WPD applications must use the capabilities of the portable device to determine functionally, not the <see cref="PortableDeviceType"/> value.
   /// </remarks>
   public enum PortableDeviceType
   {
      /// <summary>(-1) Unknown.</summary>
      Unknown = -1,

      /// <summary>A generic WPD that includes multifunction devices that do not fall into one of the other <see cref="PortableDeviceType"/> enumeration values.</summary>
      Generic = 0,

      /// <summary>A camera device, such as a digital still camera.</summary>
      Camera = 1,

      /// <summary>A media player device that supports playing audio, video, or viewing pictures, such as a portable music player or portable media center.</summary>
      /// <remarks>Not all of this functionally is classified as a <see cref="MediaPlayer"/>. For example, portable music player devices are classified as <see cref="MediaPlayer"/>.</remarks>
      MediaPlayer = 2,

      /// <summary>A phone device, such as a mobile phone.</summary>
      Phone = 3,

      /// <summary>A video device.</summary>
      Video = 4,

      /// <summary>A personal information manager device.</summary>
      PersonalInformationManager = 5,

      /// <summary>An audio recorder device. </summary>
      AudioRecorder = 6
   }
}

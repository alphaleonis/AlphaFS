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
      /// <summary>The storage device type.</summary>
      internal enum DEVICE_TYPE
      {
         /// <summary>FILE_DEVICE_BEEP parameter.</summary>
         FILE_DEVICE_BEEP = 1,

         /// <summary>FILE_DEVICE_CD_ROM parameter.</summary>
         FILE_DEVICE_CD_ROM = 2,

         /// <summary>FILE_DEVICE_CD_ROM_FILE_SYSTEM parameter.</summary>
         FILE_DEVICE_CD_ROM_FILE_SYSTEM = 3,

         /// <summary>FILE_DEVICE_CONTROLLER parameter.</summary>
         FILE_DEVICE_CONTROLLER = 4,

         /// <summary>FILE_DEVICE_DATALINK parameter.</summary>
         FILE_DEVICE_DATALINK = 5,

         /// <summary>FILE_DEVICE_DFS parameter.</summary>
         FILE_DEVICE_DFS = 6,

         /// <summary>FILE_DEVICE_DISK parameter.</summary>
         FILE_DEVICE_DISK = 7,

         /// <summary>FILE_DEVICE_DISK_FILE_SYSTEM parameter.</summary>
         FILE_DEVICE_DISK_FILE_SYSTEM = 8,

         /// <summary>FILE_DEVICE_FILE_SYSTEM parameter.</summary>
         FILE_DEVICE_FILE_SYSTEM = 9,

         /// <summary>FILE_DEVICE_INPORT_PORT parameter.</summary>
         FILE_DEVICE_INPORT_PORT = 10,

         /// <summary>FILE_DEVICE_KEYBOARD parameter.</summary>
         FILE_DEVICE_KEYBOARD = 11,

         /// <summary>FILE_DEVICE_MAILSLOT parameter.</summary>
         FILE_DEVICE_MAILSLOT = 12,

         /// <summary>FILE_DEVICE_MIDI_IN parameter.</summary>
         FILE_DEVICE_MIDI_IN = 13,

         /// <summary>FILE_DEVICE_MIDI_OUT parameter.</summary>
         FILE_DEVICE_MIDI_OUT = 14,

         /// <summary>FILE_DEVICE_MOUSE parameter.</summary>
         FILE_DEVICE_MOUSE = 15,

         /// <summary>FILE_DEVICE_MULTI_UNC_PROVIDER parameter.</summary>
         FILE_DEVICE_MULTI_UNC_PROVIDER = 16,

         /// <summary>FILE_DEVICE_NAMED_PIPE parameter.</summary>
         FILE_DEVICE_NAMED_PIPE = 17,

         /// <summary>FILE_DEVICE_NETWORK parameter.</summary>
         FILE_DEVICE_NETWORK = 18,

         /// <summary>FILE_DEVICE_NETWORK_BROWSER parameter.</summary>
         FILE_DEVICE_NETWORK_BROWSER = 19,

         /// <summary>FILE_DEVICE_NETWORK_FILE_SYSTEM parameter.</summary>
         FILE_DEVICE_NETWORK_FILE_SYSTEM = 20,

         /// <summary>FILE_DEVICE_NULL parameter.</summary>
         FILE_DEVICE_NULL = 21,

         /// <summary>FILE_DEVICE_PARALLEL_PORT parameter.</summary>
         FILE_DEVICE_PARALLEL_PORT = 22,

         /// <summary>FILE_DEVICE_PHYSICAL_NETCARD parameter.</summary>
         FILE_DEVICE_PHYSICAL_NETCARD = 23,

         /// <summary>FILE_DEVICE_PRINTER parameter.</summary>
         FILE_DEVICE_PRINTER = 24,

         /// <summary>FILE_DEVICE_SCANNER parameter.</summary>
         FILE_DEVICE_SCANNER = 25,

         /// <summary>FILE_DEVICE_SERIAL_MOUSE_PORT parameter.</summary>
         FILE_DEVICE_SERIAL_MOUSE_PORT = 26,

         /// <summary>FILE_DEVICE_SERIAL_PORT parameter.</summary>
         FILE_DEVICE_SERIAL_PORT = 27,

         /// <summary>FILE_DEVICE_SCREEN parameter.</summary>
         FILE_DEVICE_SCREEN = 28,

         /// <summary>FILE_DEVICE_SOUND parameter.</summary>
         FILE_DEVICE_SOUND = 29,

         /// <summary>FILE_DEVICE_STREAMS parameter.</summary>
         FILE_DEVICE_STREAMS = 30,

         /// <summary>FILE_DEVICE_TAPE parameter.</summary>
         FILE_DEVICE_TAPE = 31,

         /// <summary>FILE_DEVICE_TAPE_FILE_SYSTEM parameter.</summary>
         FILE_DEVICE_TAPE_FILE_SYSTEM = 32,

         /// <summary>FILE_DEVICE_TRANSPORT parameter.</summary>
         FILE_DEVICE_TRANSPORT = 33,

         /// <summary>FILE_DEVICE_UNKNOWN parameter.</summary>
         FILE_DEVICE_UNKNOWN = 34,

         /// <summary>FILE_DEVICE_VIDEO parameter.</summary>
         FILE_DEVICE_VIDEO = 35,

         /// <summary>FILE_DEVICE_VIRTUAL_DISK parameter.</summary>
         FILE_DEVICE_VIRTUAL_DISK = 36,

         /// <summary>FILE_DEVICE_WAVE_IN parameter.</summary>
         FILE_DEVICE_WAVE_IN = 37,

         /// <summary>FILE_DEVICE_WAVE_OUT parameter.</summary>
         FILE_DEVICE_WAVE_OUT = 38,

         /// <summary>FILE_DEVICE_8042_PORT parameter.</summary>
         FILE_DEVICE_8042_PORT = 39,

         /// <summary>FILE_DEVICE_NETWORK_REDIRECTOR parameter.</summary>
         FILE_DEVICE_NETWORK_REDIRECTOR = 40,

         /// <summary>FILE_DEVICE_BATTERY parameter.</summary>
         FILE_DEVICE_BATTERY = 41,

         /// <summary>FILE_DEVICE_BUS_EXTENDER parameter.</summary>
         FILE_DEVICE_BUS_EXTENDER = 42,

         /// <summary>FILE_DEVICE_MODEM parameter.</summary>
         FILE_DEVICE_MODEM = 43,

         /// <summary>FILE_DEVICE_VDM parameter.</summary>
         FILE_DEVICE_VDM = 44,

         /// <summary>FILE_DEVICE_MASS_STORAGE parameter.</summary>
         FILE_DEVICE_MASS_STORAGE = 45,

         /// <summary>FILE_DEVICE_SMB parameter.</summary>
         FILE_DEVICE_SMB = 46,

         /// <summary>FILE_DEVICE_KS parameter.</summary>
         FILE_DEVICE_KS = 47,

         /// <summary>FILE_DEVICE_CHANGER parameter.</summary>
         FILE_DEVICE_CHANGER = 48,

         /// <summary>FILE_DEVICE_SMARTCARD parameter.</summary>
         FILE_DEVICE_SMARTCARD = 49,

         /// <summary>FILE_DEVICE_ACPI parameter.</summary>
         FILE_DEVICE_ACPI = 50,

         /// <summary>FILE_DEVICE_DVD parameter.</summary>
         FILE_DEVICE_DVD = 51,

         /// <summary>FILE_DEVICE_FULLSCREEN_VIDEO parameter.</summary>
         FILE_DEVICE_FULLSCREEN_VIDEO = 52,

         /// <summary>FILE_DEVICE_DFS_FILE_SYSTEM parameter.</summary>
         FILE_DEVICE_DFS_FILE_SYSTEM = 53,

         /// <summary>FILE_DEVICE_DFS_VOLUME parameter.</summary>
         FILE_DEVICE_DFS_VOLUME = 54,

         /// <summary>FILE_DEVICE_SERENUM parameter.</summary>
         FILE_DEVICE_SERENUM = 55,

         /// <summary>FILE_DEVICE_TERMSRV parameter.</summary>
         FILE_DEVICE_TERMSRV = 56,

         /// <summary>FILE_DEVICE_KSEC parameter.</summary>
         FILE_DEVICE_KSEC = 57,

         /// <summary>FILE_DEVICE_FIPS parameter.</summary>
         FILE_DEVICE_FIPS = 58,

         /// <summary>FILE_DEVICE_INFINIBAND parameter.</summary>
         FILE_DEVICE_INFINIBAND = 59,

         /// <summary>FILE_DEVICE_VMBUS parameter.</summary>
         FILE_DEVICE_VMBUS = 62,

         /// <summary>FILE_DEVICE_CRYPT_PROVIDER parameter.</summary>
         FILE_DEVICE_CRYPT_PROVIDER = 63,

         /// <summary>FILE_DEVICE_WPD parameter.</summary>
         FILE_DEVICE_WPD = 64,

         /// <summary>FILE_DEVICE_BLUETOOTH parameter.</summary>
         FILE_DEVICE_BLUETOOTH = 65,

         /// <summary>FILE_DEVICE_MT_COMPOSITE parameter.</summary>
         FILE_DEVICE_MT_COMPOSITE = 66,

         /// <summary>FILE_DEVICE_MT_TRANSPORT parameter.</summary>
         FILE_DEVICE_MT_TRANSPORT = 67,

         /// <summary>FILE_DEVICE_BIOMETRIC parameter.</summary>
         FILE_DEVICE_BIOMETRIC = 68,

         /// <summary>FILE_DEVICE_PMI parameter.</summary>
         FILE_DEVICE_PMI = 69,

         /// <summary>FILE_DEVICE_EHSTOR parameter.</summary>
         FILE_DEVICE_EHSTOR = 70,

         /// <summary>FILE_DEVICE_DEVAPI parameter.</summary>
         FILE_DEVICE_DEVAPI = 71,

         /// <summary>FILE_DEVICE_GPIO parameter.</summary>
         FILE_DEVICE_GPIO = 72,

         /// <summary>FILE_DEVICE_USBEX parameter.</summary>
         FILE_DEVICE_USBEX = 73,

         /// <summary>FILE_DEVICE_CONSOLE parameter.</summary>
         FILE_DEVICE_CONSOLE = 80,

         /// <summary>FILE_DEVICE_NFP parameter.</summary>
         FILE_DEVICE_NFP = 81,

         /// <summary>FILE_DEVICE_SYSENV parameter.</summary>
         FILE_DEVICE_SYSENV = 82,

         /// <summary>FILE_DEVICE_MASS_STORAGE parameter.</summary>
         FILE_DEVICE_VOLUME = 86
      }
   }
}

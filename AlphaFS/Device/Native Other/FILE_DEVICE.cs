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
      internal enum FILE_DEVICE
      {
         /// <summary>BEEP parameter.</summary>
         BEEP = 1,

         /// <summary>CD_ROM parameter.</summary>
         CD_ROM = 2,

         /// <summary>CD_ROM_FILE_SYSTEM parameter.</summary>
         CD_ROM_FILE_SYSTEM = 3,

         /// <summary>CONTROLLER parameter.</summary>
         CONTROLLER = 4,

         /// <summary>DATALINK parameter.</summary>
         DATALINK = 5,

         /// <summary>DFS parameter.</summary>
         DFS = 6,

         /// <summary>DISK parameter.</summary>
         DISK = 7,

         /// <summary>DISK_FILE_SYSTEM parameter.</summary>
         DISK_FILE_SYSTEM = 8,

         /// <summary>FILE_SYSTEM parameter.</summary>
         FILE_SYSTEM = 9,

         /// <summary>INPORT_PORT parameter.</summary>
         INPORT_PORT = 10,

         /// <summary>KEYBOARD parameter.</summary>
         KEYBOARD = 11,

         /// <summary>MAILSLOT parameter.</summary>
         MAILSLOT = 12,

         /// <summary>MIDI_IN parameter.</summary>
         MIDI_IN = 13,

         /// <summary>MIDI_OUT parameter.</summary>
         MIDI_OUT = 14,

         /// <summary>MOUSE parameter.</summary>
         MOUSE = 15,

         /// <summary>MULTI_UNC_PROVIDER parameter.</summary>
         MULTI_UNC_PROVIDER = 16,

         /// <summary>NAMED_PIPE parameter.</summary>
         NAMED_PIPE = 17,

         /// <summary>NETWORK parameter.</summary>
         NETWORK = 18,

         /// <summary>NETWORK_BROWSER parameter.</summary>
         NETWORK_BROWSER = 19,

         /// <summary>NETWORK_FILE_SYSTEM parameter.</summary>
         NETWORK_FILE_SYSTEM = 20,

         /// <summary>NULL parameter.</summary>
         NULL = 21,

         /// <summary>PARALLEL_PORT parameter.</summary>
         PARALLEL_PORT = 22,

         /// <summary>PHYSICAL_NETCARD parameter.</summary>
         PHYSICAL_NETCARD = 23,

         /// <summary>PRINTER parameter.</summary>
         PRINTER = 24,

         /// <summary>SCANNER parameter.</summary>
         SCANNER = 25,

         /// <summary>SERIAL_MOUSE_PORT parameter.</summary>
         SERIAL_MOUSE_PORT = 26,

         /// <summary>SERIAL_PORT parameter.</summary>
         SERIAL_PORT = 27,

         /// <summary>SCREEN parameter.</summary>
         SCREEN = 28,

         /// <summary>SOUND parameter.</summary>
         SOUND = 29,

         /// <summary>STREAMS parameter.</summary>
         STREAMS = 30,

         /// <summary>TAPE parameter.</summary>
         TAPE = 31,

         /// <summary>TAPE_FILE_SYSTEM parameter.</summary>
         TAPE_FILE_SYSTEM = 32,

         /// <summary>TRANSPORT parameter.</summary>
         TRANSPORT = 33,

         /// <summary>UNKNOWN parameter.</summary>
         UNKNOWN = 34,

         /// <summary>VIDEO parameter.</summary>
         VIDEO = 35,

         /// <summary>VIRTUAL_DISK parameter.</summary>
         VIRTUAL_DISK = 36,

         /// <summary>WAVE_IN parameter.</summary>
         WAVE_IN = 37,

         /// <summary>WAVE_OUT parameter.</summary>
         WAVE_OUT = 38,

         /// <summary>8042_PORT parameter.</summary>
         PORT_8042 = 39,

         /// <summary>NETWORK_REDIRECTOR parameter.</summary>
         NETWORK_REDIRECTOR = 40,

         /// <summary>BATTERY parameter.</summary>
         BATTERY = 41,

         /// <summary>BUS_EXTENDER parameter.</summary>
         BUS_EXTENDER = 42,

         /// <summary>MODEM parameter.</summary>
         MODEM = 43,

         /// <summary>VDM parameter.</summary>
         VDM = 44,

         /// <summary>MASS_STORAGE parameter.</summary>
         MASS_STORAGE = 45,

         /// <summary>SMB parameter.</summary>
         SMB = 46,

         /// <summary>KS parameter.</summary>
         KS = 47,

         /// <summary>CHANGER parameter.</summary>
         CHANGER = 48,

         /// <summary>SMARTCARD parameter.</summary>
         SMARTCARD = 49,

         /// <summary>ACPI parameter.</summary>
         ACPI = 50,

         /// <summary>DVD parameter.</summary>
         DVD = 51,

         /// <summary>FULLSCREEN_VIDEO parameter.</summary>
         FULLSCREEN_VIDEO = 52,

         /// <summary>DFS_FILE_SYSTEM parameter.</summary>
         DFS_FILE_SYSTEM = 53,

         /// <summary>DFS_VOLUME parameter.</summary>
         DFS_VOLUME = 54,

         /// <summary>SERENUM parameter.</summary>
         SERENUM = 55,

         /// <summary>TERMSRV parameter.</summary>
         TERMSRV = 56,

         /// <summary>KSEC parameter.</summary>
         KSEC = 57,

         /// <summary>FIPS parameter.</summary>
         FIPS = 58,

         /// <summary>INFINIBAND parameter.</summary>
         INFINIBAND = 59,

         /// <summary>VMBUS parameter.</summary>
         VMBUS = 62,

         /// <summary>CRYPT_PROVIDER parameter.</summary>
         CRYPT_PROVIDER = 63,

         /// <summary>WPD parameter.</summary>
         WPD = 64,

         /// <summary>BLUETOOTH parameter.</summary>
         BLUETOOTH = 65,

         /// <summary>MT_COMPOSITE parameter.</summary>
         MT_COMPOSITE = 66,

         /// <summary>MT_TRANSPORT parameter.</summary>
         MT_TRANSPORT = 67,

         /// <summary>BIOMETRIC parameter.</summary>
         BIOMETRIC = 68,

         /// <summary>PMI parameter.</summary>
         PMI = 69,

         /// <summary>EHSTOR parameter.</summary>
         EHSTOR = 70,

         /// <summary>DEVAPI parameter.</summary>
         DEVAPI = 71,

         /// <summary>GPIO parameter.</summary>
         GPIO = 72,

         /// <summary>USBEX parameter.</summary>
         USBEX = 73,

         /// <summary>CONSOLE parameter.</summary>
         CONSOLE = 80,

         /// <summary>NFP parameter.</summary>
         NFP = 81,

         /// <summary>SYSENV parameter.</summary>
         SYSENV = 82,

         /// <summary>MASS_STORAGE parameter.</summary>
         VOLUME = 86
      }
   }
}

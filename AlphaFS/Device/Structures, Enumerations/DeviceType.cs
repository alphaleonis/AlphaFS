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
   /// <summary>The type of device. Values from 0 through 32,767 are reserved for use by Microsoft. Values from 32,768 through 65,535 are reserved for use by other vendors.
   /// The following values are defined by Microsoft.</summary>
   [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Enum values should not be combinable.")]
   public enum DeviceType
   {
      /// <summary>Indicates an unknown storage device type.</summary>
      None = 0,

      /// <summary>BEEP parameter.</summary>
      Beep = NativeMethods.FILE_DEVICE.BEEP,

      /// <summary>CD_ROM parameter.</summary>
      CDRom = NativeMethods.FILE_DEVICE.CD_ROM, // .NET DriveInfo.DriveType property also uses "CDRom" instead of "CdRom".

      /// <summary>CD_ROM_FILE_SYSTEM parameter.</summary>
      CDRomFileSystem = NativeMethods.FILE_DEVICE.CD_ROM_FILE_SYSTEM,

      /// <summary>CONTROLLER parameter.</summary>
      Controller = NativeMethods.FILE_DEVICE.CONTROLLER,

      /// <summary>DATALINK parameter.</summary>
      DataLink = NativeMethods.FILE_DEVICE.DATALINK,

      /// <summary>DFS parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      Dfs = NativeMethods.FILE_DEVICE.DFS,

      /// <summary>DISK parameter.</summary>
      Disk = NativeMethods.FILE_DEVICE.DISK,

      /// <summary>DISK_FILE_SYSTEM parameter.</summary>
      DiskFileSystem = NativeMethods.FILE_DEVICE.DISK_FILE_SYSTEM,

      /// <summary>FILE_SYSTEM parameter.</summary>
      FileSystem = NativeMethods.FILE_DEVICE.FILE_SYSTEM,

      /// <summary>INPORT_PORT parameter.</summary>
      InPort = NativeMethods.FILE_DEVICE.INPORT_PORT,

      /// <summary>KEYBOARD parameter.</summary>
      Keyboard = NativeMethods.FILE_DEVICE.KEYBOARD,

      /// <summary>MAILSLOT parameter.</summary>
      MailSlot = NativeMethods.FILE_DEVICE.MAILSLOT,

      /// <summary>MIDI_IN parameter.</summary>
      MidiIn = NativeMethods.FILE_DEVICE.MIDI_IN,

      /// <summary>MIDI_OUT parameter.</summary>
      MidiOut = NativeMethods.FILE_DEVICE.MIDI_OUT,

      /// <summary>MOUSE parameter.</summary>
      Mouse = NativeMethods.FILE_DEVICE.MOUSE,

      /// <summary>MULTI_UNC_PROVIDER parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
      MultiUncProvider = NativeMethods.FILE_DEVICE.MULTI_UNC_PROVIDER,

      /// <summary>NAMED_PIPE parameter.</summary>
      NamedPipe = NativeMethods.FILE_DEVICE.NAMED_PIPE,

      /// <summary>NETWORK parameter.</summary>
      Network = NativeMethods.FILE_DEVICE.NETWORK,

      /// <summary>NETWORK_BROWSER parameter.</summary>
      NetworkBrowser = NativeMethods.FILE_DEVICE.NETWORK_BROWSER,

      /// <summary>NETWORK_FILE_SYSTEM parameter.</summary>
      NetworkFileSystem = NativeMethods.FILE_DEVICE.NETWORK_FILE_SYSTEM,

      /// <summary>NULL parameter.</summary>
      NullDevice = NativeMethods.FILE_DEVICE.NULL,

      /// <summary>PARALLEL_PORT parameter.</summary>
      ParallelPort = NativeMethods.FILE_DEVICE.PARALLEL_PORT,

      /// <summary>PHYSICAL_NETCARD parameter.</summary>
      PhysicalNetCard = NativeMethods.FILE_DEVICE.PHYSICAL_NETCARD,

      /// <summary>PRINTER parameter.</summary>
      Printer = NativeMethods.FILE_DEVICE.PRINTER,

      /// <summary>SCANNER parameter.</summary>
      Scanner = NativeMethods.FILE_DEVICE.SCANNER,

      /// <summary>SERIAL_MOUSE_PORT parameter.</summary>
      SerialMousePort = NativeMethods.FILE_DEVICE.SERIAL_MOUSE_PORT,

      /// <summary>SERIAL_PORT parameter.</summary>
      SerialPort = NativeMethods.FILE_DEVICE.SERIAL_PORT,

      /// <summary>SCREEN parameter.</summary>
      Screen = NativeMethods.FILE_DEVICE.SCREEN,

      /// <summary>SOUND parameter.</summary>
      Sound = NativeMethods.FILE_DEVICE.SOUND,

      /// <summary>STREAMS parameter.</summary>
      Streams = NativeMethods.FILE_DEVICE.STREAMS,

      /// <summary>TAPE parameter.</summary>
      Tape = NativeMethods.FILE_DEVICE.TAPE,

      /// <summary>TAPE_FILE_SYSTEM parameter.</summary>
      TapeFileSystem = NativeMethods.FILE_DEVICE.TAPE_FILE_SYSTEM,

      /// <summary>TRANSPORT parameter.</summary>
      Transport = NativeMethods.FILE_DEVICE.TRANSPORT,

      /// <summary>UNKNOWN parameter.</summary>
      Unknown = NativeMethods.FILE_DEVICE.UNKNOWN,

      /// <summary>VIDEO parameter.</summary>
      Video = NativeMethods.FILE_DEVICE.VIDEO,

      /// <summary>VIRTUAL_DISK parameter.</summary>
      VirtualDisk = NativeMethods.FILE_DEVICE.VIRTUAL_DISK,

      /// <summary>WAVE_IN parameter.</summary>
      WaveIn = NativeMethods.FILE_DEVICE.WAVE_IN,

      /// <summary>WAVE_OUT parameter.</summary>
      WaveOut = NativeMethods.FILE_DEVICE.WAVE_OUT,

      /// <summary>8042_PORT parameter.</summary>
      Port8042 = NativeMethods.FILE_DEVICE.PORT_8042,

      /// <summary>NETWORK_REDIRECTOR parameter.</summary>
      NetworkRedirector = NativeMethods.FILE_DEVICE.NETWORK_REDIRECTOR,

      /// <summary>BATTERY parameter.</summary>
      Battery = NativeMethods.FILE_DEVICE.BATTERY,

      /// <summary>BUS_EXTENDER parameter.</summary>
      BusExtender = NativeMethods.FILE_DEVICE.BUS_EXTENDER,

      /// <summary>MODEM parameter.</summary>
      Modem = NativeMethods.FILE_DEVICE.MODEM,

      /// <summary>VDM parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vdm")]
      Vdm = NativeMethods.FILE_DEVICE.VDM,

      /// <summary>MASS_STORAGE parameter.</summary>
      MassStorage = NativeMethods.FILE_DEVICE.MASS_STORAGE,

      /// <summary>SMB parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Smb")]
      Smb = NativeMethods.FILE_DEVICE.SMB,

      /// <summary>KS parameter.</summary>
      KS = NativeMethods.FILE_DEVICE.KS,

      /// <summary>CHANGER parameter.</summary>
      Changer = NativeMethods.FILE_DEVICE.CHANGER,

      /// <summary>SMARTCARD parameter.</summary>
      Smartcard = NativeMethods.FILE_DEVICE.SMARTCARD,

      /// <summary>ACPI parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Acpi")]
      Acpi = NativeMethods.FILE_DEVICE.ACPI,

      /// <summary>DVD parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dvd")]
      Dvd = NativeMethods.FILE_DEVICE.DVD,

      /// <summary>FULLSCREEN_VIDEO parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fullscreen")]
      FullscreenVideo = NativeMethods.FILE_DEVICE.FULLSCREEN_VIDEO,

      /// <summary>DFS_FILE_SYSTEM parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      DfsFileSystem = NativeMethods.FILE_DEVICE.DFS_FILE_SYSTEM,

      /// <summary>DFS_VOLUME parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      DfsVolume = NativeMethods.FILE_DEVICE.DFS_VOLUME,

      /// <summary>SERENUM parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Serenum")]
      Serenum = NativeMethods.FILE_DEVICE.SERENUM,

      /// <summary>TERMSRV parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Termsrv")]
      Termsrv = NativeMethods.FILE_DEVICE.TERMSRV,

      /// <summary>KSEC parameter.</summary>
      KSec = NativeMethods.FILE_DEVICE.KSEC,

      /// <summary>FIPS parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fips")]
      Fips = NativeMethods.FILE_DEVICE.FIPS,

      /// <summary>INFINIBAND parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infiniband")]
      Infiniband = NativeMethods.FILE_DEVICE.INFINIBAND,

      /// <summary>VMBUS parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Vm")]
      VmBus = NativeMethods.FILE_DEVICE.VMBUS,

      /// <summary>CRYPT_PROVIDER parameter.</summary>
      CryptProvider = NativeMethods.FILE_DEVICE.CRYPT_PROVIDER,

      /// <summary>WPD parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wpd")]
      Wpd = NativeMethods.FILE_DEVICE.WPD,

      /// <summary>BLUETOOTH parameter.</summary>
      Bluetooth = NativeMethods.FILE_DEVICE.BLUETOOTH,

      /// <summary>MT_COMPOSITE parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Mt")]
      MtComposite = NativeMethods.FILE_DEVICE.MT_COMPOSITE,

      /// <summary>MT_TRANSPORT parameter.</summary>
      MTTransport = NativeMethods.FILE_DEVICE.MT_TRANSPORT,

      /// <summary>BIOMETRIC parameter.</summary>
      Biometric = NativeMethods.FILE_DEVICE.BIOMETRIC,

      /// <summary>PMI parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pmi")]
      Pmi = NativeMethods.FILE_DEVICE.PMI,

      /// <summary>EHSTOR parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ehstor")]
      Ehstor = NativeMethods.FILE_DEVICE.EHSTOR,

      /// <summary>DEVAPI parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Devapi")]
      Devapi = NativeMethods.FILE_DEVICE.DEVAPI,

      /// <summary>GPIO parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpio")]
      Gpio = NativeMethods.FILE_DEVICE.GPIO,

      /// <summary>USBEX parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Usbex")]
      Usbex = NativeMethods.FILE_DEVICE.USBEX,

      /// <summary>CONSOLE parameter.</summary>
      Console = NativeMethods.FILE_DEVICE.CONSOLE,

      /// <summary>NFP parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nfp")]
      Nfp = NativeMethods.FILE_DEVICE.NFP,

      /// <summary>SYSENV parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sysenv")]
      Sysenv = NativeMethods.FILE_DEVICE.SYSENV
   }
}

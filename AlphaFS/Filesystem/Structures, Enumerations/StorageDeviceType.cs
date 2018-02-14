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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>The type of device. Values from 0 through 32,767 are reserved for use by Microsoft. Values from 32,768 through 65,535 are reserved for use by other vendors.
   /// The following values are defined by Microsoft.</summary>
   [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Enum values should not be combinable.")]
   public enum StorageDeviceType
   {
      /// <summary>Indicates an unknown storage device type.</summary>
      None = 0,

      /// <summary>FILE_DEVICE_BEEP parameter.</summary>
      Beep = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_BEEP,

      /// <summary>FILE_DEVICE_CD_ROM parameter.</summary>
      CDRom = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_CD_ROM,

      /// <summary>FILE_DEVICE_CD_ROM_FILE_SYSTEM parameter.</summary>
      CDRomFileSystem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_CD_ROM_FILE_SYSTEM,

      /// <summary>FILE_DEVICE_CONTROLLER parameter.</summary>
      Controller = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_CONTROLLER,

      /// <summary>FILE_DEVICE_DATALINK parameter.</summary>
      DataLink = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DATALINK,

      /// <summary>FILE_DEVICE_DFS parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DFS")]
      DFS = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DFS,

      /// <summary>FILE_DEVICE_DISK parameter.</summary>
      Disk = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DISK,

      /// <summary>FILE_DEVICE_DISK_FILE_SYSTEM parameter.</summary>
      DiskFileSystem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DISK_FILE_SYSTEM,

      /// <summary>FILE_DEVICE_FILE_SYSTEM parameter.</summary>
      FileSystem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_FILE_SYSTEM,

      /// <summary>FILE_DEVICE_INPORT_PORT parameter.</summary>
      InPort = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_INPORT_PORT,

      /// <summary>FILE_DEVICE_KEYBOARD parameter.</summary>
      Keyboard = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_KEYBOARD,

      /// <summary>FILE_DEVICE_MAILSLOT parameter.</summary>
      MailSlot = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MAILSLOT,

      /// <summary>FILE_DEVICE_MIDI_IN parameter.</summary>
      MidiIn = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MIDI_IN,

      /// <summary>FILE_DEVICE_MIDI_OUT parameter.</summary>
      MidiOut = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MIDI_OUT,

      /// <summary>FILE_DEVICE_MOUSE parameter.</summary>
      Mouse = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MOUSE,

      /// <summary>FILE_DEVICE_MULTI_UNC_PROVIDER parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
      MultiUncProvider = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MULTI_UNC_PROVIDER,

      /// <summary>FILE_DEVICE_NAMED_PIPE parameter.</summary>
      NamedPipe = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NAMED_PIPE,

      /// <summary>FILE_DEVICE_NETWORK parameter.</summary>
      Network = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NETWORK,

      /// <summary>FILE_DEVICE_NETWORK_BROWSER parameter.</summary>
      NetworkBrowser = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NETWORK_BROWSER,

      /// <summary>FILE_DEVICE_NETWORK_FILE_SYSTEM parameter.</summary>
      NetworkFileSystem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NETWORK_FILE_SYSTEM,

      /// <summary>FILE_DEVICE_NULL parameter.</summary>
      NullDevice = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NULL,

      /// <summary>FILE_DEVICE_PARALLEL_PORT parameter.</summary>
      ParallelPort = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_PARALLEL_PORT,

      /// <summary>FILE_DEVICE_PHYSICAL_NETCARD parameter.</summary>
      PhysicalNetCard = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_PHYSICAL_NETCARD,

      /// <summary>FILE_DEVICE_PRINTER parameter.</summary>
      Printer = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_PRINTER,

      /// <summary>FILE_DEVICE_SCANNER parameter.</summary>
      Scanner = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SCANNER,

      /// <summary>FILE_DEVICE_SERIAL_MOUSE_PORT parameter.</summary>
      SerialMousePort = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SERIAL_MOUSE_PORT,

      /// <summary>FILE_DEVICE_SERIAL_PORT parameter.</summary>
      SerialPort = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SERIAL_PORT,

      /// <summary>FILE_DEVICE_SCREEN parameter.</summary>
      Screen = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SCREEN,

      /// <summary>FILE_DEVICE_SOUND parameter.</summary>
      Sound = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SOUND,

      /// <summary>FILE_DEVICE_STREAMS parameter.</summary>
      Streams = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_STREAMS,

      /// <summary>FILE_DEVICE_TAPE parameter.</summary>
      Tape = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_TAPE,

      /// <summary>FILE_DEVICE_TAPE_FILE_SYSTEM parameter.</summary>
      TapeFileSystem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_TAPE_FILE_SYSTEM,

      /// <summary>FILE_DEVICE_TRANSPORT parameter.</summary>
      Transport = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_TRANSPORT,

      /// <summary>FILE_DEVICE_UNKNOWN parameter.</summary>
      Unknown = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_UNKNOWN,

      /// <summary>FILE_DEVICE_VIDEO parameter.</summary>
      Video = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_VIDEO,

      /// <summary>FILE_DEVICE_VIRTUAL_DISK parameter.</summary>
      VirtualDisk = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_VIRTUAL_DISK,

      /// <summary>FILE_DEVICE_WAVE_IN parameter.</summary>
      WaveIn = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_WAVE_IN,

      /// <summary>FILE_DEVICE_WAVE_OUT parameter.</summary>
      WaveOut = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_WAVE_OUT,

      /// <summary>FILE_DEVICE_8042_PORT parameter.</summary>
      Port8042 = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_8042_PORT,

      /// <summary>FILE_DEVICE_NETWORK_REDIRECTOR parameter.</summary>
      NetworkRedirector = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NETWORK_REDIRECTOR,

      /// <summary>FILE_DEVICE_BATTERY parameter.</summary>
      Battery = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_BATTERY,

      /// <summary>FILE_DEVICE_BUS_EXTENDER parameter.</summary>
      BusExtender = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_BUS_EXTENDER,

      /// <summary>FILE_DEVICE_MODEM parameter.</summary>
      Modem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MODEM,

      /// <summary>FILE_DEVICE_VDM parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VDM")]
      VDM = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_VDM,

      /// <summary>FILE_DEVICE_MASS_STORAGE parameter.</summary>
      MassStorage = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MASS_STORAGE,

      /// <summary>FILE_DEVICE_SMB parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SMB")]
      SMB = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SMB,

      /// <summary>FILE_DEVICE_KS parameter.</summary>
      KS = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_KS,

      /// <summary>FILE_DEVICE_CHANGER parameter.</summary>
      Changer = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_CHANGER,

      /// <summary>FILE_DEVICE_SMARTCARD parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SmartCard")]
      SmartCard = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SMARTCARD,

      /// <summary>FILE_DEVICE_ACPI parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ACPI")]
      ACPI = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_ACPI,

      /// <summary>FILE_DEVICE_DVD parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DVD")]
      DVD = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DVD,

      /// <summary>FILE_DEVICE_FULLSCREEN_VIDEO parameter.</summary>
      FullScreenVideo = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_FULLSCREEN_VIDEO,

      /// <summary>FILE_DEVICE_DFS_FILE_SYSTEM parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DEVICE")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DFS")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FILE")]
      [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
      DFSFileSystem = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DFS_FILE_SYSTEM,

      /// <summary>FILE_DEVICE_DFS_VOLUME parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DFS")]
      DFSVolume = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DFS_VOLUME,

      /// <summary>FILE_DEVICE_SERENUM parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Serenum")]
      Serenum = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SERENUM,

      /// <summary>FILE_DEVICE_TERMSRV parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Srv")]
      TermSrv = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_TERMSRV,

      /// <summary>FILE_DEVICE_KSEC parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "KSEC")]
      KSEC = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_KSEC,

      /// <summary>FILE_DEVICE_FIPS parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FIPS")]
      FIPS = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_FIPS,

      /// <summary>FILE_DEVICE_INFINIBAND parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infini")]
      InfiniBand = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_INFINIBAND,

      /// <summary>FILE_DEVICE_VMBUS parameter.</summary>
      VMBus = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_VMBUS,

      /// <summary>FILE_DEVICE_CRYPT_PROVIDER parameter.</summary>
      CryptProvider = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_CRYPT_PROVIDER,

      /// <summary>FILE_DEVICE_WPD parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "WPD")]
      WPD = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_WPD,

      /// <summary>FILE_DEVICE_BLUETOOTH parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "BlueTooth")]
      BlueTooth = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_BLUETOOTH,

      /// <summary>FILE_DEVICE_MT_COMPOSITE parameter.</summary>
      MTComposite = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MT_COMPOSITE,

      /// <summary>FILE_DEVICE_MT_TRANSPORT parameter.</summary>
      MTTransport = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_MT_TRANSPORT,

      /// <summary>FILE_DEVICE_BIOMETRIC parameter.</summary>
      Biometric = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_BIOMETRIC,

      /// <summary>FILE_DEVICE_PMI parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PMI")]
      PMI = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_PMI,

      /// <summary>FILE_DEVICE_EHSTOR parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stor")]
      EHStor = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_EHSTOR,

      /// <summary>FILE_DEVICE_DEVAPI parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Api")]
      DevApi = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_DEVAPI,

      /// <summary>FILE_DEVICE_GPIO parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "GPIO")]
      GPIO = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_GPIO,

      /// <summary>FILE_DEVICE_USBEX parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "USB")]
      [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
      USBEx = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_USBEX,

      /// <summary>FILE_DEVICE_CONSOLE parameter.</summary>
      Console = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_CONSOLE,

      /// <summary>FILE_DEVICE_NFP parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "NFP")]
      NFP = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_NFP,

      /// <summary>FILE_DEVICE_SYSENV parameter.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Env")]
      SysEnv = NativeMethods.STORAGE_DEVICE_TYPE.FILE_DEVICE_SYSENV
   }
}

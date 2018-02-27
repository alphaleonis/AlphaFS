////using Microsoft.VisualBasic;

//using System;
//using System.ComponentModel;
//using System.Runtime.InteropServices;
//using System.Security;
//using Microsoft.Win32.SafeHandles;

//namespace Alphaleonis.Win32.Filesystem
//{
//   internal static class IOCtl
//   {
//      private const int GENERIC_READ = unchecked((int) 0x80000000);
//      private const int FILE_SHARE_READ = 1;
//      private const int FILE_SHARE_WRITE = 2;
//      private const int OPEN_EXISTING = 3;

//      //private enum Partition : byte
//      //{
//      //   ENTRY_UNUSED = 0,
//      //   FAT_12 = 1,
//      //   XENIX_1 = 2,
//      //   XENIX_2 = 3,
//      //   FAT_16 = 4,
//      //   EXTENDED = 5,
//      //   HUGE = 6,
//      //   IFS = 7,
//      //   OS2BOOTMGR = 0xa,
//      //   FAT32 = 0xb,
//      //   FAT32_XINT13 = 0xc,
//      //   XINT13 = 0xe,
//      //   XINT13_EXTENDED = 0xf,
//      //   PREP = 0x41,
//      //   LDM = 0x42,
//      //   UNIX = 0x63
//      //}

//      [SuppressUnmanagedCodeSecurity]
//      private class NativeMethods
//      {

//         [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
//         internal static extern SafeFileHandle CreateFile(
//            string fileName,
//            int desiredAccess,
//            int shareMode,
//            IntPtr securityAttributes,
//            int creationDisposition,
//            int flagsAndAttributes,
//            IntPtr hTemplateFile);

//         [DllImport("kernel32", SetLastError = true)]
//         [return: MarshalAs(UnmanagedType.Bool)]
//         internal static extern bool DeviceIoControl(
//            SafeFileHandle hVol,
//            Filesystem.NativeMethods.IoControlCode controlCode,
//            IntPtr inBuffer,
//            uint inBufferSize,
//            IntPtr outBuffer,
//            uint outBufferSize,
//            ref uint bytesReturned,
//            IntPtr overlapped);

//      }


      


      


//      internal static void Main()
//      {
//         SendIoCtlDiskGetDriveLayoutEx(0);
//         SendIoCtlDiskGetDriveLayoutEx(1);
//         //SendIoCtlDiskGetDriveLayoutEx(2);
//      }


//      private static void SendIoCtlDiskGetDriveLayoutEx(int PhysicalDrive)
//      {

//         var lie = default(Filesystem.NativeMethods.DRIVE_LAYOUT_INFORMATION_EX);

//         Filesystem.NativeMethods.PARTITION_INFORMATION_EX[] pies = null;

//         using (var hDevice = NativeMethods.CreateFile("\\\\.\\PHYSICALDRIVE" + PhysicalDrive, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
//         {
//            if (hDevice.IsInvalid)
//               throw new Win32Exception();


//            // We don't know how many partitions there are, so we have to use a blob of memory...
//            var numPartitions = 1;
//            var done = false;

//            do
//            {
//               // 48 = the number of bytes in DRIVE_LAYOUT_INFORMATION_EX up to
//               // the first PARTITION_INFORMATION_EX in the array.
//               // And each PARTITION_INFORMATION_EX is 144 bytes.
//               var outBufferSize = 48 + numPartitions * 144;
//               var blob = default(IntPtr);
//               uint bytesReturned = 0;

//               try
//               {
//                  blob = Marshal.AllocHGlobal(outBufferSize);

//                  var result = NativeMethods.DeviceIoControl(hDevice, Filesystem.NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, IntPtr.Zero, 0, blob, (uint) outBufferSize, ref bytesReturned, IntPtr.Zero);

//                  // We expect that we might not have enough room in the output buffer.
//                  if (result == false)
//                  {
//                     // If the buffer wasn't too small, then something else went wrong.
//                     if (Marshal.GetLastWin32Error() != Win32Errors.ERROR_INSUFFICIENT_BUFFER)
//                        throw new Win32Exception();
//                     // We need more space on the next loop.
//                     numPartitions += 1;
//                  }
//                  else
//                  {
//                     // We got the size right, so stop looping.
//                     done = true;

//                     // Do something with the data here - we'll free the memory before we leave the loop.
//                     // First we grab the DRIVE_LAYOUT_INFORMATION_EX, it's at the start of the blob of memory:
//                     lie = (Filesystem.NativeMethods.DRIVE_LAYOUT_INFORMATION_EX) Marshal.PtrToStructure(blob, typeof(Filesystem.NativeMethods.DRIVE_LAYOUT_INFORMATION_EX));

//                     // Then loop and add the PARTITION_INFORMATION_EX structures to an array.
//                     pies = new Filesystem.NativeMethods.PARTITION_INFORMATION_EX[lie.PartitionCount];

//                     for (var i = 0; i <= lie.PartitionCount - 1; i++)
//                     {
//                        // Where is this structure in the blob of memory?
//                        var offset = new IntPtr(blob.ToInt64() + 48 + i * 144);

//                        pies[i] = (Filesystem.NativeMethods.PARTITION_INFORMATION_EX) Marshal.PtrToStructure(offset, typeof(Filesystem.NativeMethods.PARTITION_INFORMATION_EX));
//                     }
//                  }
//               }
//               finally
//               {
//                  Marshal.FreeHGlobal(blob);
//               }
//            } while (!done);
//         }


//         DumpInfo(lie, pies);
//      }


//      private static bool IsPart0Aligned(Filesystem.NativeMethods.PARTITION_INFORMATION_EX[] pies)
//      {
//         try
//         {
//            return pies[0].StartingOffset % 4096 == 0;
//         }
//         catch
//         {
//            return false;
//         }
//      }


//      private static void DumpInfo(Filesystem.NativeMethods.DRIVE_LAYOUT_INFORMATION_EX lie, Filesystem.NativeMethods.PARTITION_INFORMATION_EX[] pies)
//      {
//         //var aaa = new PartitionTypeGuid(PartitionType.LdmMetaDataGuid);


//         Console.WriteLine("\n\nPart 0 aligned:" + IsPart0Aligned(pies));


//         Console.WriteLine("Partition Style: {0}", lie.PartitionStyle);
//         Console.WriteLine("Partition Count: {0}", lie.PartitionCount);

//         switch (lie.PartitionStyle)
//         {
//            case PartitionStyle.Mbr:
//               Console.WriteLine("Mbr Signature: {0}", lie.Mbr.Signature);
//               break;
//            case PartitionStyle.Gpt:
//               Console.WriteLine("Gpt DiskId              : {0}", lie.Gpt.DiskId);
//               Console.WriteLine("Gpt StartingUsableOffset: {0}", lie.Gpt.StartingUsableOffset);
//               Console.WriteLine("Gpt UsableLength        : {0}", lie.Gpt.UsableLength);
//               Console.WriteLine("Gpt UsableLength        : {0}", Utils.UnitSizeToText(lie.Gpt.UsableLength));
//               Console.WriteLine("Gpt MaxPartitionCount   : {0}", lie.Gpt.MaxPartitionCount);
//               break;

//            default:
//               Console.WriteLine("RAW!");
//               break;
//         }


         
         
//         for (var i = 0; i <= lie.PartitionCount - 1; i++)
//         {
//            var storagePartitionInfo = new StoragePartitionInfo();

//            Console.WriteLine();
//            Console.WriteLine();
//            Console.WriteLine("Partition {0} info...", i + 1);
//            Console.WriteLine("-------------------");


//            var _with1 = pies[i];
//            Console.WriteLine("Partition style  : {0}", (PartitionStyle) _with1.PartitionStyle);
//            Console.WriteLine("Starting offset  : {0}", _with1.StartingOffset);
//            Console.WriteLine("Partition length : {0}", _with1.PartitionLength);
//            Console.WriteLine("Partition length : {0}", Utils.UnitSizeToText(_with1.PartitionLength));
//            Console.WriteLine("Partition number : {0}", _with1.PartitionNumber);
//            Console.WriteLine("Rewrite partition: {0}", _with1.RewritePartition);


//            switch ((PartitionStyle) _with1.PartitionStyle)
//            {
//               case PartitionStyle.Mbr:
//                  var _with2 = _with1.Mbr;
//                  Console.WriteLine("Mbr PartitionType - raw value: {0}", _with2.PartitionType);

//                  //Console.WriteLine("Mbr PartitionType - validNTFT: {0}", _with2.IsValidNTFT());

//                  //if (_with2.IsValidNTFT())
//                  //   Console.WriteLine("Mbr PartitionType - ntft: {0}", _with2.IsNTFT());

//                  //Console.WriteLine("Mbr PartitionType - decoded: {0}", _with2.GetPartition());

//                  Console.WriteLine("Mbr BootIndicator      : {0}", _with2.BootIndicator);
//                  Console.WriteLine("Mbr RecognizedPartition: {0}", _with2.RecognizedPartition);
//                  Console.WriteLine("Mbr HiddenSectors      : {0}", _with2.HiddenSectors);
//                  break;

//               case PartitionStyle.Gpt:
//                  var _with3 = _with1.Gpt;
//                  Console.WriteLine("Gpt PartitionType: {0}", _with3.PartitionType);
//                  Console.WriteLine("Gpt PartitionId  : {0}", _with3.PartitionId);
//                  Console.WriteLine("Gpt Attributes   : {0}", (PartitionAttributes) _with3.Attributes);
//                  Console.WriteLine("Gpt Name         : {0}", _with3.Name);
//                  break;

//               case PartitionStyle.Raw:
//                  Console.WriteLine("RAW!");
//                  break;
//            }
//         }
//      }
//   }
//}

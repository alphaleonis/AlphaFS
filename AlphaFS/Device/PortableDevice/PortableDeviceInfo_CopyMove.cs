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
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using PortableDeviceApiLib;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      /// <summary>[AlphaFS] Copies an existing file from the portable device, disallowing the overwriting of an existing file.</summary>
      /// <param name="fileInfo">An initialized <see cref="WpdFileSystemInfo"/> instance referencing the file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory or an existing file.</param>
      public void CopyFile(WpdFileInfo fileInfo, string destinationPath)
      {
         CopyFileCore(null, fileInfo, destinationPath, null);
      }


      /// <summary>[AlphaFS] Copies an existing file from the portable device, disallowing the overwriting of an existing file.</summary>
      /// <param name="fileInfo">An initialized <see cref="WpdFileInfo"/> instance referencing the file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory or an existing file.</param>
      /// <param name="transaction">The transaction.</param>
      public void CopyFile(WpdFileInfo fileInfo, string destinationPath, KernelTransaction transaction)
      {
         CopyFileCore(null, fileInfo, destinationPath, transaction);
      }


      ///// <summary>[AlphaFS] Copies an existing file to the portable device, disallowing the overwriting of an existing file.</summary>
      ///// <param name="fileName"></param>
      ///// <param name="parentObjectId"></param>
      //public void CopyFile(string fileName, string parentObjectId)
      //{
      //   //IPortableDeviceContent content;
      //   //PortableDevice.Content(out content);

      //   //IPortableDeviceValues values = GetRequiredPropertiesForContentType(fileName, parentObjectId);

      //   //IStream tempStream;
      //   //uint optimalWriteBufferSize = 0;
      //   //content.CreateObjectWithPropertiesAndData(values, out tempStream, ref optimalWriteBufferSize, null);

      //   //System.Runtime.InteropServices.ComTypes.IStream targetStream = (System.Runtime.InteropServices.ComTypes.IStream)tempStream;

      //   //try
      //   //{
      //   //   using (var sourceStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
      //   //   {
      //   //      var buffer = new byte[optimalWriteBufferSize];
      //   //      int bytesRead;

      //   //      do
      //   //      {
      //   //         bytesRead = sourceStream.Read(buffer, 0, (int) optimalWriteBufferSize);
      //   //         IntPtr pcbWritten = Marshal.AllocHGlobal(4);
      //   //         targetStream.Write(buffer, bytesRead, pcbWritten);
      //   //         Marshal.FreeHGlobal(pcbWritten);

      //   //      } while (bytesRead > 0);
      //   //   }

      //   //   targetStream.Commit(0);
      //   //}
      //   //finally
      //   //{
      //   //   Marshal.ReleaseComObject(tempStream);
      //   //}
      //}


      ///// <summary>[AlphaFS] Moves an existing file from the portable device to a local or remote destination, disallowing the overwriting of an existing file.</summary>
      ///// <param name="fileInfo"></param>
      ///// <param name="destinationPath"></param>
      //public void Move(WpdFileSystemInfo fileInfo, string destinationPath)
      //{
      //}


      ///// <summary>[AlphaFS] Moves an existing file to the portable device, disallowing the overwriting of an existing file.</summary>
      ///// <param name="fileName"></param>
      ///// <param name="parentObjectId"></param>
      //public void Move(string fileName, string parentObjectId)
      //{
      //}



      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.</summary>
      /// <param name="directoryInfo">An initialized <see cref="WpdDirectoryInfo"/> instance referencing the directory to copy.</param>
      /// <param name="destinationPath">The name of the destination directory. This cannot be an existing directory.</param>
      public void CopyDirectory(WpdDirectoryInfo directoryInfo, string destinationPath)
      {
         CopyDirectoryCore(directoryInfo, destinationPath, null);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.</summary>
      /// <param name="directoryInfo">An initialized <see cref="WpdDirectoryInfo"/> instance referencing the directory to copy.</param>
      /// <param name="destinationPath">The name of the destination directory. This cannot be an existing directory.</param>
      /// <param name="transaction">The transaction.</param>
      public void CopyDirectory(WpdDirectoryInfo directoryInfo, string destinationPath, KernelTransaction transaction)
      {
         CopyDirectoryCore(directoryInfo, destinationPath, transaction);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.</summary>
      /// <param name="directoryInfo">An initialized <see cref="WpdDirectoryInfo"/> instance referencing the directory to copy.</param>
      /// <param name="destinationPath">The name of the destination directory. This cannot be an existing directory.</param>
      /// <param name="transaction">The transaction.</param>
      internal void CopyDirectoryCore(WpdDirectoryInfo directoryInfo, string destinationPath, KernelTransaction transaction)
      {
         IPortableDeviceContent content;
         PortableDevice.Content(out content);

         IPortableDeviceResources resources;
         content.Transfer(out resources);

         
      }


      /// <summary>Copies an existing file from the portable device, disallowing the overwriting of an existing file.</summary>
      /// <param name="resources"></param>
      /// <param name="fileInfo">An initialized <see cref="WpdFileInfo"/> instance referencing the file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory or an existing file.</param>
      /// <param name="transaction">The transaction.</param>
      internal void CopyFileCore(IPortableDeviceResources resources, WpdFileInfo fileInfo, string destinationPath, KernelTransaction transaction)
      {
         if (null == fileInfo)
            throw new ArgumentNullException("fileInfo");

         if (null == destinationPath)
            throw new ArgumentNullException("destinationPath");

         if (destinationPath.Trim().Length == 0)
            throw new ArgumentException("destinationPath");

         if (null == resources)
         {
            IPortableDeviceContent content;
            PortableDevice.Content(out content);

            content.Transfer(out resources);
         }

         if (null == resources)
            throw new ArgumentNullException("resources");


         IStream fileOutStream = null;
         System.Runtime.InteropServices.ComTypes.IStream fileSourceStream = null;

         uint optimalBufferSize = 0;
         var property = new _tagpropertykey {fmtid = new Guid(0xE81E79BE, 0x34F0, 0x41BF, 0xB5, 0x3F, 0xF1, 0xA0, 0x6A, 0xE8, 0x78, 0x42), pid = 0};


         try
         {
            resources.GetStream(fileInfo.ObjectId, ref property, 0, ref optimalBufferSize, out fileOutStream);

            fileSourceStream = (System.Runtime.InteropServices.ComTypes.IStream) fileOutStream;


            var fileFullName = Path.GetExtendedLengthPathCore(transaction, Path.CombineCore(false, destinationPath, fileInfo.OriginalFileName), PathFormat.FullPath, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

            using (var safeFileHandle = File.CreateFileCore(transaction, fileFullName, ExtendedFileAttributes.Normal, null, FileMode.CreateNew, FileSystemRights.CreateFiles, FileShare.None, false, false, PathFormat.LongFullPath))

            using (var fileStream = new FileStream(safeFileHandle, FileAccess.Write, (int) optimalBufferSize))
            {
               var buffer = new byte[optimalBufferSize];
               int bytesRead;

               do
               {
                  using (var safeBuffer = new SafeGlobalMemoryBufferHandle(buffer.Length))
                  {
                     fileSourceStream.Read(buffer, safeBuffer.Capacity, safeBuffer.DangerousGetHandle());

                     bytesRead = safeBuffer.ReadInt32();
                  }

                  if (bytesRead > 0)
                     fileStream.Write(buffer, 0, bytesRead);

               } while (bytesRead > 0);
            }
         }
         finally
         {
            if (null != fileSourceStream)
               Marshal.ReleaseComObject(fileSourceStream);

            if (null != fileOutStream)
               Marshal.ReleaseComObject(fileOutStream);
         }
      }
   }
}

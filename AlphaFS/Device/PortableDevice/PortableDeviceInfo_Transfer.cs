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

using System.Diagnostics;
using System.Reflection;
using PortableDeviceTypesLib;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      #region CopyFrom

      /// <summary>Copies an existing file from the portable device to a local or remote destination, disallowing the overwriting of an existing file.</summary>
      /// <param name="path"></param>
      /// <param name="parentObjectId"></param>
      public void CopyFrom(string path, string parentObjectId)
      {
         //public void DownloadFile(PortableDeviceFile file, string saveToPath)
         //{
         //   IPortableDeviceContent content;
         //   PortableDeviceApiLib.IStream wpdStream;

         //   uint optimalTransferSize = 0;

         //   TagPropertyKey property = new TagPropertyKey
         //   {
         //      fmtid = new Guid(0xE81E79BE, 0x34F0, 0x41BF, 0xB5, 0x3F, 0xF1, 0xA0, 0x6A, 0xE8, 0x78, 0x42),
         //      pid = 0
         //   };

         //   _portableDevice.Content(out content);

         //   IPortableDeviceResources resources;
         //   content.Transfer(out resources);

         //   string filename = Path.GetFileName(file.Id);

         //   resources.GetStream(file.Id, ref property, 0, ref optimalTransferSize, out wpdStream);

         //   IStream sourceStream = (IStream)wpdStream;
         //   FileStream targetStream = new FileStream(Path.Combine(saveToPath, filename), FileMode.Create, FileAccess.Write);

         //   unsafe
         //   {
         //      var buffer = new byte[1024];
         //      int bytesRead;
         //      do
         //      {
         //         sourceStream.Read(buffer, 1024, new IntPtr(&bytesRead));
         //         targetStream.Write(buffer, 0, 1024);
         //      } while (bytesRead > 0);
         //      targetStream.Close();
         //   }
         //}
      }

      #endregion // CopyFrom

      #region CopyTo

      /// <summary>Copies an existing file to the portable device, disallowing the overwriting of an existing file.</summary>
      /// <param name="path"></param>
      /// <param name="parentObjectId"></param>
      public void CopyTo(string path, string parentObjectId)
      {
         //if (Utils.IsNullOrWhiteSpace(path))
         //   throw new ArgumentNullException("path");

         //if (Utils.IsNullOrWhiteSpace(path))
         //   throw new ArgumentNullException("parentObjectId");


         //IPortableDeviceContent content;
         //PortableDevice.Content(out content);

         //#region Get Properties

         //IPortableDeviceValues clientInfo = (IPortableDeviceValues) new PortableDeviceValues();

         //clientInfo.SetStringValue(ref WindowsPortableDeviceConstants.ObjectParentId, parentObjectId);
         //clientInfo.SetUnsignedLargeIntegerValue(ref WindowsPortableDeviceConstants.ObjectSize, (ulong)File.GetSizeInternal(null, null, path, false));

         //string fileName = Path.GetFileName(path);

         //clientInfo.SetStringValue(ref WindowsPortableDeviceConstants.ObjectOriginalFileName, fileName);
         //clientInfo.SetStringValue(ref WindowsPortableDeviceConstants.ObjectName, fileName);

         //#endregion // Get Properties

         //PortableDeviceApiLib.IStream tempStream;
         //uint optimalTransferSizeBytes = 0;
         //content.CreateObjectWithPropertiesAndData(clientInfo, out tempStream, ref optimalTransferSizeBytes, null);

         //IStream targetStream = (IStream)tempStream;
         //try
         //{
         //   using (FileStream sourceStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
         //   {
         //      var buffer = new byte[optimalTransferSizeBytes];
         //      int bytesRead;
         //      do
         //      {
         //         bytesRead = sourceStream.Read(buffer, 0, (int)optimalTransferSizeBytes);
         //         IntPtr pcbWritten = IntPtr.Zero;
         //         targetStream.Write(buffer, (int)optimalTransferSizeBytes, pcbWritten);
         //      } while (bytesRead > 0);
         //   }
         //   targetStream.Commit(0);
         //}
         //finally
         //{
         //   Marshal.ReleaseComObject(tempStream);
         //}
      }

      #endregion // CopyTo

      #region MoveFrom

      /// <summary>Moves an existing file from the portable device to a local or remote destination, disallowing the overwriting of an existing file.</summary>
      /// <param name="path"></param>
      /// <param name="parentObjectId"></param>
      public void MoveFrom(string path, string parentObjectId)
      {
      }

      #endregion // MoveFrom

      #region MoveTo

      /// <summary>Moves an existing file to the portable device, disallowing the overwriting of an existing file.</summary>
      /// <param name="path"></param>
      /// <param name="parentObjectId"></param>
      public void MoveTo(string path, string parentObjectId)
      {
      }

      #endregion // MoveTo
   }
}

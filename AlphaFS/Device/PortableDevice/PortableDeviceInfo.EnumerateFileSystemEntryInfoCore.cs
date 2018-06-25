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
using System.Collections.Generic;
using System.Security;
using System.Text;
using Alphaleonis.Win32.Filesystem;
using PortableDeviceApiLib;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      /// <summary>[AlphaFS] Returns an enumerable collection of file- and directory names from the root of the device.</summary>
      /// <returns>An enumerable collection of file-system entries from the root of the device.</returns>
      [SecurityCritical]
      public IEnumerable<WpdFileSystemInfo> EnumerateFileSystemEntries()
      {
         return EnumerateFileSystemEntryInfoCore(this, null, null, false, null);
      }


      /// <summary>[AlphaFS] Returns an enumerable collection of file- and directory names from the root of the device.</summary>
      /// <returns>An enumerable collection of file-system entries from the root of the device.</returns>
      [SecurityCritical]
      public IEnumerable<WpdFileSystemInfo> EnumerateFileSystemEntries(bool recurse)
      {
         return EnumerateFileSystemEntryInfoCore(this, null, null, recurse, null);
      }


      /// <summary>[AlphaFS] Returns an enumerable collection of file- and directory instances in a specified path.</summary>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="objectId"/>.</returns>
      /// <param name="objectId">The ID of the directory to search.</param>
      /// <param name="recurse"></param>
      [SecurityCritical]
      public IEnumerable<WpdFileSystemInfo> EnumerateFileSystemEntries(string objectId, bool recurse)
      {
         return EnumerateFileSystemEntryInfoCore(this, null, objectId, recurse, null);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="deviceContent">A Portable Device Content instance. If <c>null</c> is specified the <paramref name="deviceContent"/> is retrieved from the <paramref name="portableDeviceInfo"/> instance.</param>
      /// <param name="objectId">The ID of the directory to search.</param>
      /// <param name="recurse"></param>
      /// <param name="getFolders">
      ///    <c>true</c> folders will be returned.
      ///    <c>false</c> files will be returned.
      ///    <c>null</c> both folders and files will be returned.
      /// </param>
      internal static IEnumerable<WpdFileSystemInfo> EnumerateFileSystemEntryInfoCore(PortableDeviceInfo portableDeviceInfo, IPortableDeviceContent deviceContent, string objectId, bool recurse, bool? getFolders)
      {
         if (null == portableDeviceInfo)
            throw new ArgumentNullException("portableDeviceInfo");

         if (null == portableDeviceInfo.PortableDevice)
            throw new ArgumentNullException("portableDevice");


         // If no deviceContent is supplied, start at the root of the device.
         if (null == deviceContent)
            portableDeviceInfo.PortableDevice.Content(out deviceContent);


         if (null == deviceContent)
            yield break;


         var dirs = new Queue<string>(1000);

         dirs.Enqueue(!Utils.IsNullOrWhiteSpace(objectId) ? objectId : NativeMethods.DeviceObjectId);


         IPortableDeviceProperties deviceProperties;
         deviceContent.Properties(out deviceProperties);

         while (dirs.Count > 0)
         {
            // FindFirstFile()
            IEnumPortableDeviceObjectIDs objectIds;

            //try { deviceContent.EnumObjects(0, dirs.Dequeue(), null, out objectIds); }
            //catch { }

            deviceContent.EnumObjects(0, dirs.Dequeue(), null, out objectIds);


            uint retrieved;
            do
            {
               retrieved = 0;

               if (null != objectIds)
               {
                  string childName;

                  objectIds.Next(1, out childName, ref retrieved);

                  //try { objectIds.Next(1, out childName, ref retrieved); }
                  //catch { }

                  if (retrieved == 0)
                     continue;


                  var pdfsi = GetFileSystemInfo(deviceProperties, childName, portableDeviceInfo.DeviceProtocol == PortableDeviceProtocol.Mtp);

                  if (pdfsi.IsDirectory && recurse)
                     dirs.Enqueue(childName);


                  // Make sure the requested file system object type is returned.
                  // null = Return files and directories.
                  // true = Return only directories.
                  // false = Return only files.

                  switch (getFolders)
                  {
                     case null:
                        yield return pdfsi;
                        break;

                     case true:
                        if (pdfsi.IsDirectory)
                           yield return pdfsi;
                        break;

                     case false:
                        if (!pdfsi.IsDirectory)
                           yield return pdfsi;
                        break;
                  }
               }

               // FindNextFile()
            } while (retrieved > 0);
         }
      }


      /// <summary>[AlphaFS] Retrieves the properties of a file system object from the Portable Device.</summary>
      /// <param name="deviceProperties">The instance of properties from the Portable Device instance.</param>
      /// <param name="objectId">The object ID to retrieve the properties from.</param>
      /// <param name="mtpOnly"><c>true></c> The Portable Device uses the MTP protocol.</param>
      /// <returns>Returns a <see cref="T:WpdFileSystemInfo"/> instance.</returns>
      private static WpdFileSystemInfo GetFileSystemInfo(IPortableDeviceProperties deviceProperties, string objectId, bool mtpOnly)
      {
         if (null == deviceProperties)
            throw new ArgumentNullException("deviceProperties");

         if (Utils.IsNullOrWhiteSpace(objectId))
            objectId = NativeMethods.DeviceObjectId;

         var keys = (IPortableDeviceKeyCollection) new PortableDeviceTypesLib.PortableDeviceKeyCollectionClass();

         keys.Add(NativeMethods.WPD_OBJECT_CONTENT_TYPE);
         keys.Add(NativeMethods.WPD_OBJECT_NAME);
         keys.Add(NativeMethods.WPD_OBJECT_ORIGINAL_FILE_NAME);
         keys.Add(NativeMethods.WPD_OBJECT_PARENT_ID);
         keys.Add(NativeMethods.WPD_OBJECT_SIZE);

         IPortableDeviceValues objectProperties;
         deviceProperties.GetValues(objectId, keys, out objectProperties);


         // WPD_OBJECT_CONTENT_TYPE

         Guid objectContentType;
         objectProperties.GetGuidValue(ref NativeMethods.WPD_OBJECT_CONTENT_TYPE, out objectContentType);

         var isFolder = objectContentType == NativeMethods.WPD_CONTENT_TYPE_FOLDER || objectContentType == NativeMethods.WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT;


         // WPD_OBJECT_PARENT_ID

         string objectParentId;

         // Querying from the device root always return an empty string
         // because the FriendlyName is normally the assigned drive letter, such as USB disks.
         objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_PARENT_ID, out objectParentId);


         // WPD_OBJECT_NAME

         string objectName;

         objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_NAME, out objectName);


         // WPD_OBJECT_ORIGINAL_FILE_NAME

         string objectOriginalFileName;

         if (objectId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase))

            objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_NAME, out objectOriginalFileName);

         else
         {
            if (objectParentId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase))

               objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_NAME, out objectOriginalFileName);

            else
               objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_ORIGINAL_FILE_NAME, out objectOriginalFileName);
         }


         // WPD_OBJECT_SIZE

         ulong objectSize = 0;

         if (!isFolder)
            objectProperties.GetUnsignedLargeIntegerValue(ref NativeMethods.WPD_OBJECT_SIZE, out objectSize);


         // Full path equals the objectId on devices using the UMS protocol (USB disks).

         var fullPath = mtpOnly ? GetFullPath(deviceProperties, objectId) : objectId;


         return isFolder
            ? (WpdFileSystemInfo) new WpdDirectoryInfo(objectId, objectName, fullPath)
            {
               ContentType = objectContentType,
               OriginalFileName = objectOriginalFileName,
               ParentId = objectParentId,
            }
            : new WpdFileInfo(objectId, objectName, fullPath)
            {
               ContentType = objectContentType,
               OriginalFileName = objectOriginalFileName,
               ParentId = objectParentId,

               Length = (long) objectSize
            };
      }


      /// <summary>Returns the absolute path for the specified path string.</summary>
      /// <param name="deviceProperties"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      private static string GetFullPath(IPortableDeviceProperties deviceProperties, string objectId)
      {
         if (null == deviceProperties)
            throw new ArgumentNullException("deviceProperties");

         if (null == objectId)
            throw new ArgumentNullException("objectId");


         // No need to drill down if we are already at the device root.

         if (objectId.Length == 0 || objectId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase))

            return NativeMethods.DeviceObjectId;


         var keys = (IPortableDeviceKeyCollection) new PortableDeviceTypesLib.PortableDeviceKeyCollectionClass();

         keys.Add(NativeMethods.WPD_OBJECT_NAME);
         keys.Add(NativeMethods.WPD_OBJECT_ORIGINAL_FILE_NAME);
         keys.Add(NativeMethods.WPD_OBJECT_PARENT_ID);


         var folderFullPath = new Stack<string>(1000);

         var isDeviceRoot = false;
         

         while (!isDeviceRoot)
         {
            IPortableDeviceValues objectProperties;
            //deviceProperties.GetValues(objectId, null, out objectProperties);
            deviceProperties.GetValues(objectId, keys, out objectProperties);

            // WPD_OBJECT_NAME            : ".com.exent.android"        (as shown on enumeration)
            // WPD_OBJECT_ORIGINAL_FILE_NAME: ".com.exent.android.shared" (as shown in Explorer)

            string objectName;

            // WPD_OBJECT_PARENT_ID

            string objectParentId;

            // Querying from the device root always return an empty string
            // because the FriendlyName is normally the assigned drive letter, such as USB disks.
            objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_PARENT_ID, out objectParentId);


            // The Try/Catch construct will slow things down, better use a couple of if/then constructs.

            //try { objectProperties.GetStringValue(ref WindowsNativeMethods.WPD_OBJECT_ORIGINAL_FILE_NAME, out objectName); }
            //catch { objectProperties.GetStringValue(ref WindowsNativeMethods.WPD_OBJECT_NAME, out objectName); }

            // Querying from the device root for WPD_OBJECT_ORIGINAL_FILE_NAME, throws an Exception.

            if (objectId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase))

               objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_NAME, out objectName);

            else
            {
               if (objectParentId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase))

                  objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_NAME, out objectName);

               else
                  objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_ORIGINAL_FILE_NAME, out objectName);
            }


            if (!Utils.IsNullOrWhiteSpace(objectName))
               folderFullPath.Push(objectName);

            isDeviceRoot = objectParentId.Length == 0 || objectId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase);

            objectId = objectParentId;
         }

         // Assemble the full path and return.
         var fullPath = new StringBuilder(100);

         foreach (var folder in folderFullPath)
            fullPath.Append(folder + Path.DirectorySeparator);

         return Path.RemoveTrailingDirectorySeparator(fullPath.ToString(), false);
      }


      ///// <summary>Returns the parent ID of the object.</summary>
      ///// <param name="deviceProperties"></param>
      ///// <param name="objectId"></param>
      ///// <returns></returns>
      //private static string GetParentId(IPortableDeviceProperties deviceProperties, string objectId)
      //{
      //   if (deviceProperties == null)
      //      throw new ArgumentNullException("deviceProperties");

      //   if (Utils.IsNullOrWhiteSpace(objectId))
      //      throw new ArgumentNullException("objectId");

      //   if (objectId.Length == 0 || objectId.Equals(NativeMethods.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
      //      return NativeMethods.DeviceObjectId;

      //   IPortableDeviceValues objectProperties;
      //   string objectParentId;

      //   deviceProperties.GetValues(objectId, null, out objectProperties);

      //   // Querying from the device root always return an empty string
      //   // because the FriendlyName is normally the assigned drive letter, such as USB disks.
      //   objectProperties.GetStringValue(ref NativeMethods.WPD_OBJECT_PARENT_ID, out objectParentId);

      //   return objectParentId;
      //}
   }
}

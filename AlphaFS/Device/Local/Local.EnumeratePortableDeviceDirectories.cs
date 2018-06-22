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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using PortableDeviceApiLib;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <returns></returns>
      public static IEnumerable<PortableDeviceFileSystemInfo> EnumeratePortableDeviceDirectories(PortableDeviceInfo portableDeviceInfo)
      {
         return EnumerateFileSystemEntryInfoCore(portableDeviceInfo, null, null, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true);
      }


      /// <summary>Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns></returns>
      public static IEnumerable<PortableDeviceFileSystemInfo> EnumeratePortableDeviceDirectories(PortableDeviceInfo portableDeviceInfo, string searchPattern)
      {
         return EnumerateFileSystemEntryInfoCore(portableDeviceInfo, null, null, searchPattern, SearchOption.TopDirectoryOnly, true);
      }


      /// <summary>Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns></returns>
      public static IEnumerable<PortableDeviceFileSystemInfo> EnumeratePortableDeviceDirectories(PortableDeviceInfo portableDeviceInfo, string searchPattern, SearchOption searchOption)
      {
         return EnumerateFileSystemEntryInfoCore(portableDeviceInfo, null, null, searchPattern, searchOption, true);
      }


      /// <summary>Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="deviceContent">A Portable Device Content instance.</param>
      /// <returns></returns>
      public static IEnumerable<PortableDeviceFileSystemInfo> EnumeratePortableDeviceDirectories(PortableDeviceInfo portableDeviceInfo, IPortableDeviceContent deviceContent)
      {
         return EnumerateFileSystemEntryInfoCore(portableDeviceInfo, deviceContent, null, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true);
      }

      
      /// <summary>Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="deviceContent">A Portable Device Content instance.</param>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns></returns>
      public static IEnumerable<PortableDeviceFileSystemInfo> EnumeratePortableDeviceDirectories(PortableDeviceInfo portableDeviceInfo, IPortableDeviceContent deviceContent, string searchPattern)
      {
         return EnumerateFileSystemEntryInfoCore(portableDeviceInfo, deviceContent, null, searchPattern, SearchOption.TopDirectoryOnly, true);
      }


      /// <summary>Returns an enumerable collection of directory names in a specified path.</summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="deviceContent">A Portable Device Content instance.</param>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns></returns>
      public static IEnumerable<PortableDeviceFileSystemInfo> EnumeratePortableDeviceDirectories(PortableDeviceInfo portableDeviceInfo, IPortableDeviceContent deviceContent, string searchPattern, SearchOption searchOption)
      {
         return EnumerateFileSystemEntryInfoCore(portableDeviceInfo, deviceContent, null, searchPattern, searchOption, true);
      }




      /// <summary>Returns an enumerable collection of directory names in a specified path.
      /// <para>&#160;</para>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      /// </summary>
      /// <param name="portableDeviceInfo">The <see cref="T:PortableDeviceInfo"/> instance of the Portable Device.</param>
      /// <param name="deviceContent">A Portable Device Content instance. If <c>null</c> is specified the <paramref name="deviceContent"/> is retrieved from the <paramref name="portableDeviceInfo"/> instance.</param>
      /// <param name="objectId">The ID of the directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="getFolders">
      ///    <c>true</c> folders will be returned.
      ///    <c>false</c> files will be returned.
      ///    <c>null</c> both folders and files will be returned.
      /// </param>
      internal static IEnumerable<PortableDeviceFileSystemInfo> EnumerateFileSystemEntryInfoCore(PortableDeviceInfo portableDeviceInfo, IPortableDeviceContent deviceContent, string objectId, string searchPattern, SearchOption searchOption, bool? getFolders)
      {
         if (portableDeviceInfo == null)
            throw new ArgumentNullException("portableDeviceInfo");

         if (portableDeviceInfo.PortableDevice == null)
            throw new ArgumentNullException("portableDevice");

         // If no deviceContent is supplied, start at the root of the device.
         if (deviceContent == null)
            portableDeviceInfo.PortableDevice.Content(out deviceContent);

         if (deviceContent == null)
            yield break;

         bool searchAllDirs = searchOption == SearchOption.AllDirectories;

         Regex nameFilter = searchPattern == Path.WildcardStarMatchAll
            ? null
            : new Regex("^" + Regex.Escape(searchPattern).Replace(@"\.", ".").Replace(@"\*", ".*").Replace(@"\?", ".?") + "$", RegexOptions.IgnoreCase | RegexOptions.Compiled);


         Queue<string> dirs = new Queue<string>(1000);
         dirs.Enqueue(!Utils.IsNullOrWhiteSpace(objectId) ? objectId : PortableDeviceConstants.DeviceObjectId);


         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new Filesystem.NativeMethods.ChangeErrorMode(Filesystem.NativeMethods.ErrorMode.FailCriticalErrors))
         {
            IPortableDeviceProperties deviceProperties;
            deviceContent.Properties(out deviceProperties);

            while (dirs.Count > 0)
            {
               // FindFirstFile()
               IEnumPortableDeviceObjectIDs objectIds = null;

               try { deviceContent.EnumObjects(0, dirs.Dequeue(), null, out objectIds); }
               catch { }

               uint retrieved;
               do
               {
                  retrieved = 0;
                  if (objectIds != null)
                  {
                     string childName = null;

                     try { objectIds.Next(1, out childName, ref retrieved); }
                     catch { }

                     if (retrieved == 0)
                        continue;

                     PortableDeviceFileSystemInfo pdfsi = GetFileSystemInfo(deviceProperties, childName, portableDeviceInfo.Protocol == PortableDeviceProtocol.Mtp);
                     //pdfsi.PortableDeviceInfo = PortableDeviceInfo;

                     if (pdfsi.IsDirectory && searchAllDirs)
                        dirs.Enqueue(childName);

                     if (!(nameFilter == null || nameFilter.IsMatch(pdfsi.Name)))
                        continue;


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
      }


      /// <summary>[AlphaFS] Retrieves the properties of a file system object from the Portable Device.</summary>
      /// <param name="deviceProperties">The instance of properties from the Portable Device instance.</param>
      /// <param name="objectId">The object ID to retrieve the properties from.</param>
      /// <param name="usingMediaTransferProtocol"><c>true></c> The Portable Device uses the MTP protocol.</param>
      /// <returns>Returns a <see cref="T:PortableDeviceFileSystemInfo"/> instance.</returns>
      private static PortableDeviceFileSystemInfo GetFileSystemInfo(IPortableDeviceProperties deviceProperties, string objectId, bool usingMediaTransferProtocol)
      {
         // Setup

         if (deviceProperties == null)
            throw new ArgumentNullException("deviceProperties");

         if (Utils.IsNullOrWhiteSpace(objectId))
            objectId = PortableDeviceConstants.DeviceObjectId;

         IPortableDeviceKeyCollection keys = (IPortableDeviceKeyCollection)new PortableDeviceTypesLib.PortableDeviceKeyCollectionClass();

         keys.Add(PortableDeviceConstants.ObjectContentType);
         keys.Add(PortableDeviceConstants.ObjectName);
         keys.Add(PortableDeviceConstants.ObjectOriginalFileName);
         keys.Add(PortableDeviceConstants.ObjectParentId);
         keys.Add(PortableDeviceConstants.ObjectSize);

         IPortableDeviceValues objectProperties;
         deviceProperties.GetValues(objectId, keys, out objectProperties);


         // ObjectContentType

         Guid objectContentType;
         objectProperties.GetGuidValue(ref PortableDeviceConstants.ObjectContentType, out objectContentType);

         bool isFolder = objectContentType == PortableDeviceConstants.ContentTypeFolder || objectContentType == PortableDeviceConstants.ContentTypeFunctionalObject;


         // ObjectParentId

         string objectParentId;

         // Querying from the device root always return an empty string
         // because the FriendlyName is normally the assigned drive letter, such as USB disks.
         objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectParentId, out objectParentId);


         // ObjectName

         string objectName;

         objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectName, out objectName);


         // ObjectOriginalFileName

         string objectOriginalFileName;

         if (objectId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
            objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectName, out objectOriginalFileName);
         else
         {
            if (objectParentId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
               objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectName, out objectOriginalFileName);
            else
               objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectOriginalFileName, out objectOriginalFileName);
         }


         // region ObjectSize

         ulong objectSize = 0;

         if (!isFolder)
            objectProperties.GetUnsignedLargeIntegerValue(ref PortableDeviceConstants.ObjectSize, out objectSize);


         //if (objectId.StartsWith("o13A", StringComparison.OrdinalIgnoreCase))
         //{
         //   Console.WriteLine("Yep");
         //}


         // FullPath

         // On non usingMediaTransferProtocol devices, such as USB disks, full path equals the objectId.
         string fullPath = usingMediaTransferProtocol ? GetFullPath(deviceProperties, objectId) : objectId;


         return isFolder
            ? (PortableDeviceFileSystemInfo)new PortableDeviceDirectoryInfo(objectId, objectName, fullPath)
            {
               ContentType = objectContentType,
               OriginalFileName = objectOriginalFileName,
               ParentId = objectParentId,
            }
            : new PortableDeviceFileInfo(objectId, objectName, fullPath)
            {
               ContentType = objectContentType,
               Length = (long)objectSize,
               OriginalFileName = objectOriginalFileName,
               ParentId = objectParentId
            };
      }


      /// <summary>Returns the absolute path for the specified path string.</summary>
      /// <param name="deviceProperties"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      public static string GetFullPath(IPortableDeviceProperties deviceProperties, string objectId)
      {
         if (deviceProperties == null)
            throw new ArgumentNullException("deviceProperties");

         if (objectId == null)
            throw new ArgumentNullException("objectId");


         // No need to drill down if we are already at the device root.
         if (objectId.Length == 0 || objectId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
            return PortableDeviceConstants.DeviceObjectId;


         var keys = (IPortableDeviceKeyCollection)new PortableDeviceTypesLib.PortableDeviceKeyCollectionClass();

         keys.Add(PortableDeviceConstants.ObjectName);
         keys.Add(PortableDeviceConstants.ObjectOriginalFileName);
         keys.Add(PortableDeviceConstants.ObjectParentId);


         var folderFullPath = new Stack<string>(1000);
         var isDeviceRoot = false;

         while (!isDeviceRoot)
         {
            IPortableDeviceValues objectProperties;
            //deviceProperties.GetValues(objectId, null, out objectProperties);
            deviceProperties.GetValues(objectId, keys, out objectProperties);

            // ObjectName            : ".com.exent.android"        (as shown on enumeration)
            // ObjectOriginalFileName: ".com.exent.android.shared" (as shown in Explorer)

            string objectName;

            // ObjectParentId

            string objectParentId;

            // Querying from the device root always return an empty string
            // because the FriendlyName is normally the assigned drive letter, such as USB disks.
            objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectParentId, out objectParentId);


            // The Try/Catch construct will slow things down, better use a couple of if/then constructs.

            //try { objectProperties.GetStringValue(ref WindowsPortableDeviceConstants.ObjectOriginalFileName, out objectName); }
            //catch { objectProperties.GetStringValue(ref WindowsPortableDeviceConstants.ObjectName, out objectName); }

            // Querying from the device root for ObjectOriginalFileName, throws an Exception.

            if (objectId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
               objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectName, out objectName);
            else
            {
               if (objectParentId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
                  objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectName, out objectName);
               else
                  objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectOriginalFileName, out objectName);
            }


            if (!Utils.IsNullOrWhiteSpace(objectName))
               folderFullPath.Push(objectName);

            isDeviceRoot = objectParentId.Length == 0 || objectId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase);

            objectId = objectParentId;
         }

         // Assemble the full path and return.
         var fullPath = new StringBuilder(100);

         foreach (var folder in folderFullPath)
            fullPath.Append(folder + Path.DirectorySeparator);

         return Path.RemoveTrailingDirectorySeparator(fullPath.ToString(), false);
      }


      /// <summary>Returns the parent ID of the object.</summary>
      /// <param name="deviceProperties"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      private static string GetParentId(IPortableDeviceProperties deviceProperties, string objectId)
      {
         if (deviceProperties == null)
            throw new ArgumentNullException("deviceProperties");

         if (Utils.IsNullOrWhiteSpace(objectId))
            throw new ArgumentNullException("objectId");

         if (objectId.Length == 0 || objectId.Equals(PortableDeviceConstants.DeviceObjectId, StringComparison.OrdinalIgnoreCase))
            return PortableDeviceConstants.DeviceObjectId;

         IPortableDeviceValues objectProperties;
         string objectParentId;

         deviceProperties.GetValues(objectId, null, out objectProperties);

         // Querying from the device root always return an empty string
         // because the FriendlyName is normally the assigned drive letter, such as USB disks.
         objectProperties.GetStringValue(ref PortableDeviceConstants.ObjectParentId, out objectParentId);

         return objectParentId;
      }
   }
}

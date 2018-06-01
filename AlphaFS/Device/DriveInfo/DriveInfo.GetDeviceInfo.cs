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
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public sealed partial class DriveInfo
   {
      /// <summary>Retrieves information about the file system and volume associated with the specified root file or directorystream.</summary>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private object GetDeviceInfo(int type, int mode)
      {
         //try
         //{
         switch (type)
         {
            #region Volume

            // VolumeInfo properties.
            case 0:
               if (Utils.IsNullOrWhiteSpace(_volumeInfo.FullPath))
                  _volumeInfo.Refresh();


               switch (mode)
               {
                  case 0:
                     // IsVolume, VolumeInfo
                     return _volumeInfo;


                  case 1:
                     // DriveFormat
                     return null == _volumeInfo ? DriveType.Unknown.ToString() : _volumeInfo.FileSystemName ?? DriveType.Unknown.ToString();


                  case 2:
                     // VolumeLabel
                     return null == _volumeInfo ? string.Empty : _volumeInfo.Name ?? string.Empty;

                  case 3:
                     // DriveType
                     return null == _volumeInfo ? (object)string.Empty : _volumeInfo.DriveType;
               }

               break;


            // Volume related.
            case 1:
               if (mode == 0)
               {
                  // DosDeviceName
                  return _dosDeviceName ?? (_dosDeviceName = Volume.GetVolumeDeviceName(_name));
               }

               break;

            #endregion // Volume


            #region Drive

            // Drive related.
            case 2:
               if (mode == 0)
               {
                  // RootDirectory
                  return _rootDirectory ?? (_rootDirectory = new DirectoryInfo(null, _name, PathFormat.RelativePath));
               }

               break;


            // DiskSpaceInfo related.
            case 3:
               if (mode == 0)
               {
                  // AvailableFreeSpace, TotalFreeSpace, TotalSize, DiskSpaceInfo
                  if (!_initDsie)
                  {
                     _dsi.Refresh();
                     _initDsie = true;
                  }
               }

               break;

            #endregion // Drive
         }
         //}
         //catch
         //{
         //}

         return type == 0 && mode > 0 ? string.Empty : null;
      }
   }
}

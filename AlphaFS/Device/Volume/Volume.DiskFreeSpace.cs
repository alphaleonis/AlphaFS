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

using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Volume
   {
      /// <summary>[AlphaFS] 
      ///   Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total
      ///   amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
      /// </summary>
      /// <remarks>The calling application must have FILE_LIST_DIRECTORY access rights for this directory.</remarks>
      /// <param name="drivePath">
      ///   A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <returns>A <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> class instance.</returns>
      [SecurityCritical]
      public static DiskSpaceInfo GetDiskFreeSpace(string drivePath)
      {
         return new DiskSpaceInfo(drivePath, null, true, true);
      }


      /// <summary>[AlphaFS] 
      ///   Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total
      ///   amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
      /// </summary>
      /// <remarks>The calling application must have FILE_LIST_DIRECTORY access rights for this directory.</remarks>
      /// <param name="drivePath">
      ///   A path to a drive. For example: "C:\", "\\server\share", or "\\?\Volume{c0580d5e-2ad6-11dc-9924-806e6f6e6963}\".
      /// </param>
      /// <param name="spaceInfoType">
      ///   <see langword="null"/> gets both size- and disk cluster information. <see langword="true"/> Get only disk cluster information,
      ///   <see langword="false"/> Get only size information.
      /// </param>
      /// <returns>A <see ref="Alphaleonis.Win32.Filesystem.DiskSpaceInfo"/> class instance.</returns>
      [SecurityCritical]
      public static DiskSpaceInfo GetDiskFreeSpace(string drivePath, bool? spaceInfoType)
      {
         return new DiskSpaceInfo(drivePath, spaceInfoType, true, true);
      }
   }
}

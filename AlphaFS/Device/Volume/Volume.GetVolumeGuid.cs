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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Volume
   {
      /// <summary>[AlphaFS] Retrieves a volume <see cref="Guid"/> path for the volume that is associated with the specified volume mount point (drive letter, volume GUID path, or mounted folder).</summary>
      /// <returns>The unique volume name of the form: "\\?\Volume{GUID}\".</returns>
      /// <remarks>SMB does not support volume management functions.</remarks>
      /// <remarks>Mount points aren't supported by ReFS volumes.</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumeMountPoint">The path of a mounted folder (for example, "Y:\MountX\") or a drive letter (for example, "X:\").</param>
      [SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification =
         "Marshal.GetLastWin32Error() is manipulated.")]
      [SecurityCritical]
      public static string GetVolumeGuid(string volumeMountPoint)
      {
         if (Utils.IsNullOrWhiteSpace(volumeMountPoint))
            throw new ArgumentNullException("volumeMountPoint");


         // The string must end with a trailing backslash ('\').

         volumeMountPoint = Path.GetRegularPathCore(volumeMountPoint, GetFullPathOptions.AddTrailingDirectorySeparator | GetFullPathOptions.FullCheck, false);


         var volumeGuid = new StringBuilder(100);
         var uniqueName = new StringBuilder(100);
         bool success = false;
         int lastError = 0;


         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            // NTFS Curiosities (part 2): Volumes, volume names and mount points: Are volume names really unique?
            // https://blogs.msdn.microsoft.com/adioltean/2005/04/17/ntfs-curiosities-part-2-volumes-volume-names-and-mount-points/
            //
            // Interesting read why one would call GetVolumeNameForVolumeMountPoint() twice:
            //
            // Bottom line is: if you have disk/volume migrations, then you can expect multiple volume names for the same volume.
            // But whatever it happens, there is always a unique volume name for the current boot session. You can obtain this unique name by calling
            // GetVolumeNameForVolumeMountPoint once on your root, get the volume name, and then call GetVolumeNameForVolumeMountPoint again.
            // This will always return the unique volume name.


            // GetVolumeNameForVolumeMountPoint()
            // 2013-07-18: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            success = NativeMethods.GetVolumeNameForVolumeMountPoint(volumeMountPoint, volumeGuid, (uint)volumeGuid.Capacity);

            lastError = Marshal.GetLastWin32Error();

            if (!success)
               NativeError.ThrowException(lastError, volumeMountPoint);


            // The string must end with a trailing backslash.

            success = NativeMethods.GetVolumeNameForVolumeMountPoint(Path.AddTrailingDirectorySeparator(volumeGuid.ToString(), false), uniqueName, (uint)uniqueName.Capacity);

            lastError = Marshal.GetLastWin32Error();

            if (!success)
               NativeError.ThrowException(lastError, volumeMountPoint);


            return success ? uniqueName.ToString() : null;
         }
      }
   }
}

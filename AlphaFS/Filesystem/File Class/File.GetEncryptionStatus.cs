using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>      
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path, PathFormat pathFormat)
      {
         return GetEncryptionStatusInternal(path, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>      
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path)
      {
         return GetEncryptionStatusInternal(path, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Unified method GetEncryptionStatusInternal() to retrieve the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>
      [SecurityCritical]
      internal static FileEncryptionStatus GetEncryptionStatusInternal(string path, PathFormat pathFormat)
      {
         if (pathFormat != PathFormat.LongFullPath && Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         FileEncryptionStatus status;

         // FileEncryptionStatus()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.FileEncryptionStatus(pathLp, out status))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

         return status;
      }

   }
}

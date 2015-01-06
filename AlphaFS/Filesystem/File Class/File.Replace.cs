using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Replace

      /// <summary>
      ///   Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of
      ///   the replaced file.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>      
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
      {
         ReplaceInternal(sourceFileName, destinationFileName, destinationBackupFileName, false, PathFormat.Relative);
      }

      /// <summary>
      ///   Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of
      ///   the replaced file and optionally ignores merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the
      ///   replacement file; otherwise, <see langword="false"/>.
      /// </param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
      {
         ReplaceInternal(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors, PathFormat.Relative);
      }

      /// <summary>
      ///   [AlphaFS] Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a
      ///   backup of the replaced file and optionally ignores merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the
      ///   replacement file; otherwise, <see langword="false"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, PathFormat pathFormat)
      {
         ReplaceInternal(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors, pathFormat);
      }

      #endregion // Replace

      #region ReplaceInternal

      /// <summary>
      ///   [AlphaFS] Unified method ReplaceInternal() to replace the contents of a specified file with the contents of another file, deleting
      ///   the original file, and creating a backup of the replaced file and optionally ignores merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the
      ///   replacement file; otherwise, <see langword="false"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void ReplaceInternal(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, PathFormat pathFormat)
      {
         string sourceFileNameLp = Path.GetExtendedLengthPathInternal(null, sourceFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
         string destinationFileNameLp = Path.GetExtendedLengthPathInternal(null, destinationFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
         // Pass null to the destinationBackupFileName parameter if you do not want to create a backup of the file being replaced.
         string destinationBackupFileNameLp = Path.GetExtendedLengthPathInternal(null, destinationBackupFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         const int replacefileWriteThrough = 1;
         const int replacefileIgnoreMergeErrors = 2;

         FileSystemRights dwReplaceFlags = (FileSystemRights)replacefileWriteThrough;
         if (ignoreMetadataErrors)
            dwReplaceFlags |= (FileSystemRights)replacefileIgnoreMergeErrors;

         // ReplaceFile()
         // In the ANSI version of this function, the name is limited to MAX_PATH characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.ReplaceFile(destinationFileNameLp, sourceFileNameLp, destinationBackupFileNameLp, dwReplaceFlags, IntPtr.Zero, IntPtr.Zero))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), sourceFileNameLp, destinationFileNameLp);
      }

      #endregion // ReplaceInternal
   }
}

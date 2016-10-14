/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
         ReplaceCore(sourceFileName, destinationFileName, destinationBackupFileName, false, PathFormat.RelativePath);
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
         ReplaceCore(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors, PathFormat.RelativePath);
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
         ReplaceCore(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors, pathFormat);
      }

      #endregion // Replace

      #region ReplaceCore

      /// <summary>Replaces the contents of a specified file with the contents of another file, deleting
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
      internal static void ReplaceCore(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, PathFormat pathFormat)
      {
         const GetFullPathOptions options = GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck;

         string sourceFileNameLp = Path.GetExtendedLengthPathCore(null, sourceFileName, pathFormat, options);
         string destinationFileNameLp = Path.GetExtendedLengthPathCore(null, destinationFileName, pathFormat, options);

         // Pass null to the destinationBackupFileName parameter if you do not want to create a backup of the file being replaced.
         string destinationBackupFileNameLp = destinationBackupFileName == null
            ? null
            : Path.GetExtendedLengthPathCore(null, destinationBackupFileName, pathFormat, options);

         const int replacefileWriteThrough = 1;
         const int replacefileIgnoreMergeErrors = 2;

         FileSystemRights dwReplaceFlags = (FileSystemRights) replacefileWriteThrough;
         if (ignoreMetadataErrors)
            dwReplaceFlags |= (FileSystemRights) replacefileIgnoreMergeErrors;

         // ReplaceFile()
         // In the ANSI version of this function, the name is limited to MAX_PATH characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.ReplaceFile(destinationFileNameLp, sourceFileNameLp, destinationBackupFileNameLp, dwReplaceFlags, IntPtr.Zero, IntPtr.Zero))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), sourceFileNameLp, destinationFileNameLp);
      }

      #endregion // ReplaceCore
   }
}

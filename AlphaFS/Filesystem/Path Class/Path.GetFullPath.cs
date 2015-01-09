/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region Non-Transactional

      /// <summary>Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>
      /// <para>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</para>
      /// <para>GetFullPath is not recommended for multithreaded applications or shared library code.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException">path contains a colon (":") that is not part of a volume identifier (for example, "c:\").</exception>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      [SecurityCritical]
      public static string GetFullPath(string path)
      {
         CheckValidPath(path, true, true);

         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." : Current directory.
         if (path != null)
         {
            string tackle = GetRegularPathInternal(path, false, false, false, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         return GetFullPathInternal(null, path, false, GetFullPathOptions.None);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      [SecurityCritical]
      public static string GetFullPath(string path, bool asLongPath)
      {
         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." : Current directory.
         if (path != null)
         {
            string tackle = GetRegularPathInternal(path, false, false, false, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         return GetFullPathInternal(null, path, asLongPath, GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      /// <param name="addTrailingDirectorySeparator"><see langword="true"/> adds a trailing <see cref="DirectorySeparatorChar"/> character to <paramref name="path"/>, when absent.</param>
      /// <param name="removeTrailingDirectorySeparator"><see langword="true"/> removes the trailing <see cref="DirectorySeparatorChar"/> character from <paramref name="path"/>, when present.</param>
      [SecurityCritical]
      public static string GetFullPath(string path, bool asLongPath, bool addTrailingDirectorySeparator, bool removeTrailingDirectorySeparator)
      {
         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." : Current directory.
         if (path != null)
         {
            string tackle = GetRegularPathInternal(path, false, false, false, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         return GetFullPathInternal(null, path, asLongPath, (addTrailingDirectorySeparator ? GetFullPathOptions.AddTrailingDirectorySeparator : 0) | (removeTrailingDirectorySeparator ? GetFullPathOptions.RemoveTrailingDirectorySeparator : 0) | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
      }

      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException">path contains a colon (":") that is not part of a volume identifier (for example, "c:\").</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path)
      {
         CheckValidPath(path, true, true);

         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." : Current directory.
         if (path != null)
         {
            string tackle = GetRegularPathInternal(path, false, false, false, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         return GetFullPathInternal(transaction, path, false, GetFullPathOptions.None);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path, bool asLongPath)
      {
         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." : Current directory.
         if (path != null)
         {
            string tackle = GetRegularPathInternal(path, false, false, false, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         return GetFullPathInternal(transaction, path, asLongPath, GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      /// <param name="addTrailingDirectorySeparator"><see langword="true"/> adds a trailing <see cref="DirectorySeparatorChar"/> character to <paramref name="path"/>, when absent.</param>
      /// <param name="removeTrailingDirectorySeparator"><see langword="true"/> removes the trailing <see cref="DirectorySeparatorChar"/> character from <paramref name="path"/>, when present.</param>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path, bool asLongPath, bool addTrailingDirectorySeparator, bool removeTrailingDirectorySeparator)
      {
         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." : Current directory.
         if (path != null)
         {
            string tackle = GetRegularPathInternal(path, false, false, false, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         return GetFullPathInternal(transaction, path, asLongPath, (addTrailingDirectorySeparator ? GetFullPathOptions.AddTrailingDirectorySeparator : 0) | (removeTrailingDirectorySeparator ? GetFullPathOptions.RemoveTrailingDirectorySeparator : 0) | GetFullPathOptions.CheckInvalidPathChars);
      }

      #endregion // Transactional

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method GetFullPathInternal() to retrieve the absolute path for the specified <paramref name="path"/> string.</summary>
      /// <returns>Returns the fully qualified location of <paramref name="path"/>, such as "C:\MyFile.txt".</returns>
      /// <remarks>
      ///   <para>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</para>
      ///   <para>GetFullPath does not work reliable with relative paths.</para>
      ///   <para>GetFullPath is not recommended for multithreaded applications or shared library code.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      /// <param name="options">Options for controlling the operation.</param>
      [SecurityCritical]
      internal static string GetFullPathInternal(KernelTransaction transaction, string path, bool asLongPath, GetFullPathOptions options)
      {
         if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
            CheckInvalidPathChars(path, (options & GetFullPathOptions.CheckAdditional) != 0);

         // Do not remove DirectorySeparator when path points to a drive like: "C:\"
         // In this case, removing DirectorySeparator will point to the current directory.

         if (path == null || path.Length <= 3 || path[1] != VolumeSeparatorChar)
         {
            options &= ~GetFullPathOptions.RemoveTrailingDirectorySeparator;
         }

         string pathLp = GetLongPathInternal(path, options);

         uint bufferSize = NativeMethods.MaxPathUnicode / 32;

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
         startGetFullPathName:

            var buffer = new StringBuilder((int)bufferSize);
            uint returnLength = (transaction == null || !NativeMethods.IsAtLeastWindowsVista

               // GetFullPathName() / GetFullPathNameTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.

               ? NativeMethods.GetFullPathName(pathLp, bufferSize, buffer, IntPtr.Zero)
               : NativeMethods.GetFullPathNameTransacted(pathLp, bufferSize, buffer, IntPtr.Zero, transaction.SafeHandle));

            if (returnLength != Win32Errors.NO_ERROR)
            {
               if (returnLength > bufferSize)
               {
                  bufferSize = returnLength;
                  goto startGetFullPathName;
               }
            }
            else
            {
               if ((options & GetFullPathOptions.ContinueOnNonExist) != 0)
                  return null;

               NativeError.ThrowException(pathLp);
            }

            return asLongPath
               ? GetLongPathInternal(buffer.ToString(), GetFullPathOptions.None)
               : GetRegularPathInternal(buffer.ToString(), false, false, false, false);
         }
      }

      #endregion // Internal Methods
   }
}
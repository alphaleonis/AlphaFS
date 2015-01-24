/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Globalization;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region HasExtension (.NET)

      /// <summary>Determines whether a path includes a file name extension.</summary>
      /// <returns><see langword="true"/> if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, <see langword="false"/>.</returns>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <param name="path">The path to search for an extension. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      [SecurityCritical]
      public static bool HasExtension(string path)
      {
         return System.IO.Path.HasExtension(path);
      }

      #endregion // HasExtension (.NET)

      #region IsPathRooted

      #region .NET

      /// <summary>Gets a value indicating whether the specified path string contains absolute or relative path information.</summary>
      /// <returns><see langword="true"/> if <paramref name="path"/> contains a root; otherwise, <see langword="false"/>.</returns>
      /// <remarks>
      ///   The IsPathRooted method returns true if the first character is a directory separator character such as
      ///   <see cref="DirectorySeparatorChar"/>, or if the path starts with a drive letter and colon (<see cref="VolumeSeparatorChar"/>). For
      ///   example, it returns true for path strings such as "\\MyDir\\MyFile.txt", "C:\\MyDir", or "C:MyDir". It returns <see langword="false"/> for
      ///   path strings such as "MyDir".
      /// </remarks>
      /// <remarks>This method does not verify that the path or file name exists.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to test. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      [SecurityCritical]
      public static bool IsPathRooted(string path)
      {
         return IsPathRooted(path, true);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Gets a value indicating whether the specified path string contains absolute or relative path information.</summary>
      /// <returns><see langword="true"/> if <paramref name="path"/> contains a root; otherwise, <see langword="false"/>.</returns>
      /// <remarks>
      ///   The IsPathRooted method returns true if the first character is a directory separator character such as
      ///   <see cref="DirectorySeparatorChar"/>, or if the path starts with a drive letter and colon (<see cref="VolumeSeparatorChar"/>). For
      ///   example, it returns true for path strings such as "\\MyDir\\MyFile.txt", "C:\\MyDir", or "C:MyDir". It returns <see langword="false"/> for
      ///   path strings such as "MyDir".
      /// </remarks>
      /// <remarks>This method does not verify that the path or file name exists.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to test. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      public static bool IsPathRooted(string path, bool checkInvalidPathChars)
      {
         if (path != null)
         {
            if (checkInvalidPathChars)
               CheckInvalidPathChars(path, false);

            int length = path.Length;

            if ((length >= 1 && IsDVsc(path[0], false)) ||
                (length >= 2 && IsDVsc(path[1], true)))
               return true;
         }

         return false;
      }

      #endregion // AlphaFS

      #endregion // IsPathRooted

      #region IsLocalPath

      #region AlphaFS

      /// <summary>[AlphaFS] Determines whether the specified path is a local path.</summary>
      /// <returns><see langword="true"/> if the specified path is a local path, <see langword="false"/> otherwise.</returns>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to check.</param>
      [SecurityCritical]
      public static bool IsLocalPath(string path)
      {
         return IsLocalPath(path, true);
      }

      /// <summary>[AlphaFS] Determines whether the specified path is a local path.</summary>
      /// <returns><see langword="true"/> if the specified path is a local path, <see langword="false"/> otherwise.</returns>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to check.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      public static bool IsLocalPath(string path, bool checkInvalidPathChars)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            return false;

         path = GetRegularPathInternal(path, checkInvalidPathChars ? GetFullPathOptions.CheckInvalidPathChars : 0);

         // Don't use char.IsLetter() here as that can be misleading.
         // The only valid drive letters are: a-z and A-Z.
         char c = path[0];
         return IsPathRooted(path, false) && ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) && path[1] == VolumeSeparatorChar;
      }

      #endregion // AlphaFS

      #endregion // IsLocalPath

      #region IsValidName

      #region AlphaFS
      
      /// <summary>[AlphaFS] Check if file or folder name has any invalid characters.</summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="name">File or folder name.</param>
      /// <returns>Returns <see langword="true"/> if name contains any invalid characters. Otherwise <see langword="false"/></returns>
      public static bool IsValidName(string name)
      {
         if (name == null)
            throw new ArgumentNullException("name");

         return name.IndexOfAny(GetInvalidFileNameChars()) < 0;
      }

      #endregion // AlphaFS

      #endregion // IsValidName

      #region Internal Methods

      /// <summary>Checks the validity of the path.</summary>
      /// <exception cref="NotSupportedException">Path contains a colon character (:) that is not part of a drive label ("C:\").</exception>
      /// <param name="path">A path to the file or directory.</param>
      /// <param name="checkInvalidPathChars">Checks that the path contains only valid path-characters.</param>
      /// <param name="checkAdditional">.</param>
      internal static void CheckValidPath(string path, bool checkInvalidPathChars, bool checkAdditional)
      {
         if (!Utils.IsNullOrWhiteSpace(path) && path.Length >= 2)
         {
            string regularPath = GetRegularPathInternal(path, GetFullPathOptions.None);

            if (regularPath.Length >= 2 && regularPath.IndexOf(VolumeSeparatorChar, 2) != -1)
               throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.PathFormatUnsupported, regularPath));
         }

         if (checkInvalidPathChars && path != null)
            CheckInvalidPathChars(path, checkAdditional);
      }

      /// <summary>[AlphaFS] Checks that the path contains only valid path-characters.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <param name="path">A path to the file or directory.</param>
      /// <param name="checkAdditional"><see langword="true"/> also checks for ? and * characters.</param>
      [SecurityCritical]
      private static void CheckInvalidPathChars(string path, bool checkAdditional)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (path.Length == 0 || Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentException(Resources.PathIsZeroLengthOrOnlyWhiteSpace, "path");

         // Will fail on a Unicode path.
         string pathRp = GetRegularPathInternal(path, GetFullPathOptions.None);

         for (int index = 0, l = pathRp.Length; index < l; ++index)
         {
            int num = pathRp[index];
            switch (num)
            {
               case 34:    // "  (quote)
               case 60:    // <  (less than)
               case 62:    // >  (greater than)
               case 124:   // |  (pipe)
                  throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.IllegalCharactersInPath, (char) num), pathRp);

               default:
                  // 32: space
                  if (num >= 32 && (!checkAdditional || num != WildcardQuestionChar && num != WildcardStarMatchAllChar))
                     continue;

                  goto case 34;
            }
         }
      }

      /// <summary>[AlphaFS] Tranlates DosDevicePath, Volume GUID. For example: "\Device\HarddiskVolumeX\path\filename.ext" can translate to: "\path\filename.ext" or: "\\?\Volume{GUID}\path\filename.ext".</summary>
      /// <returns>A translated dos path.</returns>
      /// <param name="dosDevice">A DosDevicePath, for example: \Device\HarddiskVolumeX\path\filename.ext.</param>
      /// <param name="deviceReplacement">Alternate path/device text, usually <c>string.Empty</c> or <see langword="null"/>.</param>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private static string DosDeviceToDosPath(string dosDevice, string deviceReplacement)
      {
         if (Utils.IsNullOrWhiteSpace(dosDevice))
            return string.Empty;

         foreach (string drive in Directory.EnumerateLogicalDrivesInternal(false, false).Select(drv => drv.Name))
         {
            try
            {
               string path = RemoveTrailingDirectorySeparator(drive, false);
               foreach (string devNt in Volume.QueryDosDevice(path).Where(dosDevice.StartsWith))
                  return dosDevice.Replace(devNt, deviceReplacement ?? path);
            }
            catch
            {
            }
         }
         return string.Empty;
      }

      /// <summary>[AlphaFS] Check if <paramref name="path"/> ends with a directory- and/or volume-separator character.</summary>
      /// <returns><see langword="true"/> if <paramref name="path"/> ends with a separator character.</returns>
      /// <param name="path">The patch to check.</param>
      /// <param name="checkVolumeSeparatorChar">
      ///   If <see langword="null"/>, checks for all separator characters: <see cref="DirectorySeparatorChar"/>,
      ///   <see cref="AltDirectorySeparatorChar"/>
      ///   and <see cref="VolumeSeparatorChar"/>
      ///   If <see langword="false"/>, only checks for: <see cref="DirectorySeparatorChar"/> and <see cref="AltDirectorySeparatorChar"/>
      ///   If <see langword="true"/>, only checks for: <see cref="VolumeSeparatorChar"/>
      /// </param>
      [SecurityCritical]
      internal static bool EndsWithDVsc(string path, bool? checkVolumeSeparatorChar)
      {
         return path != null && path.Length >= 1 && IsDVsc(path[path.Length - 1], checkVolumeSeparatorChar);
      }

      [SecurityCritical]
      internal static int GetRootLength(string path, bool checkInvalidPathChars)
      {
         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false);

         int index = 0;
         int length = path.Length;

         if (length >= 1 && IsDVsc(path[0], false))
         {
            index = 1;
            if (length >= 2 && IsDVsc(path[1], false))
            {
               index = 2;
               int num = 2;

               while (index < length && (!IsDVsc(path[index], false) || --num > 0))
                  ++index;
            }
         }
         else if (length >= 2 && IsDVsc(path[1], true))
         {
            index = 2;
            if (length >= 3 && IsDVsc(path[2], false))
               ++index;
         }

         return index;
      }

      /// <summary>[AlphaFS] Check if <paramref name="c"/> is a directory- and/or volume-separator character.</summary>
      /// <returns><see langword="true"/> if <paramref name="c"/> is a separator character.</returns>
      /// <param name="c">The character to check.</param>
      /// <param name="checkSeparatorChar">
      ///   If <see langword="null"/>, checks for all separator characters: <see cref="DirectorySeparatorChar"/>,
      ///   <see cref="AltDirectorySeparatorChar"/> and <see cref="VolumeSeparatorChar"/>
      ///   If <see langword="false"/>, only checks for: <see cref="DirectorySeparatorChar"/> and <see cref="AltDirectorySeparatorChar"/>
      ///   If <see langword="true"/> only checks for: <see cref="VolumeSeparatorChar"/>
      /// </param>
      [SecurityCritical]
      internal static bool IsDVsc(char c, bool? checkSeparatorChar)
      {
         return checkSeparatorChar == null

            // Check for all separator characters.
            ? c == DirectorySeparatorChar || c == AltDirectorySeparatorChar || c == VolumeSeparatorChar

            // Check for some separator characters.
            : ((bool)checkSeparatorChar
               ? c == VolumeSeparatorChar
               : c == DirectorySeparatorChar || c == AltDirectorySeparatorChar);
      }

      #endregion // Internal Methods
   }
}

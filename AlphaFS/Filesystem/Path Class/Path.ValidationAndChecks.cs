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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region HasExtension (.NET)

      /// <summary>Determines whether a path includes a file name extension.</summary>
      /// <returns><see langword="true"/> if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, <see langword="false"/>.</returns>
      /// <exception cref="ArgumentException"/>
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
      ///   The IsPathRooted method returns <see langword="true"/> if the first character is a directory separator character such as
      ///   <see cref="DirectorySeparatorChar"/>, or if the path starts with a drive letter and colon (<see cref="VolumeSeparatorChar"/>).
      ///   For example, it returns true for path strings such as "\\MyDir\\MyFile.txt", "C:\\MyDir", or "C:MyDir".
      ///   It returns <see langword="false"/> for path strings such as "MyDir".
      /// </remarks>
      /// <remarks>This method does not verify that the path or file name exists.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to test. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      [SecurityCritical]
      public static bool IsPathRooted(string path)
      {
         return IsPathRooted(path, true);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Gets a value indicating whether the specified path string contains absolute or relative path information.</summary>
      /// <returns><see langword="true"/> if <paramref name="path"/> contains a root; otherwise, <see langword="false"/>.</returns>
      /// <remarks>
      ///   The IsPathRooted method returns true if the first character is a directory separator character such as
      ///   <see cref="DirectorySeparatorChar"/>, or if the path starts with a drive letter and colon (<see cref="VolumeSeparatorChar"/>).
      ///   For example, it returns <see langword="true"/> for path strings such as "\\MyDir\\MyFile.txt", "C:\\MyDir", or "C:MyDir".
      ///   It returns <see langword="false"/> for path strings such as "MyDir".
      /// </remarks>
      /// <remarks>This method does not verify that the path or file name exists.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to test. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      public static bool IsPathRooted(string path, bool checkInvalidPathChars)
      {
         if (path != null)
         {
            if (checkInvalidPathChars)
               CheckInvalidPathChars(path, false, true);

            var length = path.Length;

            if ((length >= 1 && IsDVsc(path[0], false)) ||
                (length >= 2 && IsDVsc(path[1], true)))
               return true;
         }

         return false;
      }

      #endregion // IsPathRooted

      #region IsValidName

      /// <summary>[AlphaFS] Check if file or folder name has any invalid characters.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="name">File or folder name.</param>
      /// <returns><see langword="true"/> if name contains any invalid characters. Otherwise <see langword="false"/></returns>
      public static bool IsValidName(string name)
      {
         if (name == null)
            throw new ArgumentNullException("name");

         return name.IndexOfAny(GetInvalidFileNameChars()) < 0;
      }

      #endregion // IsValidName

      #region Internal Methods

      internal static void CheckInvalidUncPath(string path)
      {
         // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." which is the current directory.
         if (!IsLongPath(path) && path.StartsWith(UncPrefix, StringComparison.OrdinalIgnoreCase))
         {
            var tackle = GetRegularPathCore(path, GetFullPathOptions.None, false).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar)
               throw new ArgumentException(Resources.UNC_Path_Should_Match_Format);
         }
      }

      /// <summary>Checks that the given path format is supported.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="path">A path to the file or directory.</param>
      /// <param name="checkInvalidPathChars">Checks that the path contains only valid path-characters.</param>
      /// <param name="checkAdditional">.</param>
      internal static void CheckSupportedPathFormat(string path, bool checkInvalidPathChars, bool checkAdditional)
      {
         if (Utils.IsNullOrWhiteSpace(path) || path.Length < 2)
            return;

         var regularPath = GetRegularPathCore(path, GetFullPathOptions.None, false);

         var isArgumentException = (regularPath[0] == VolumeSeparatorChar);
         var throwException = (isArgumentException || (regularPath.Length >= 2 && regularPath.IndexOf(VolumeSeparatorChar, 2) != -1));

         if (throwException)
         {
            if (isArgumentException)
               throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Unsupported_Path_Format, regularPath));

            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.Unsupported_Path_Format, regularPath));
         }

         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, checkAdditional, false);
      }

      /// <summary>Checks that the path contains only valid path-characters.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">A path to the file or directory.</param>
      /// <param name="checkAdditional"><see langword="true"/> also checks for ? and * characters.</param>
      /// <param name="allowEmpty">When <see langword="false"/>, throws an <see cref="ArgumentException"/>.</param>
      [SecurityCritical]
      private static void CheckInvalidPathChars(string path, bool checkAdditional, bool allowEmpty)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (!allowEmpty && (path.Length == 0 || Utils.IsNullOrWhiteSpace(path)))
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "path");

         // Will fail on a Unicode path.
         var pathRp = GetRegularPathCore(path, GetFullPathOptions.None, allowEmpty);


         // Handle "Path.GlobalRootPrefix" and "Path.VolumePrefix".
         if (pathRp.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
            pathRp = pathRp.Replace(GlobalRootPrefix, string.Empty);

         if (pathRp.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase))
            pathRp = pathRp.Replace(VolumePrefix, string.Empty);


         for (int index = 0, l = pathRp.Length; index < l; ++index)
         {
            int num = pathRp[index];
            switch (num)
            {
               case 34:    // "  (quote)
               case 60:    // <  (less than)
               case 62:    // >  (greater than)
               case 124:   // |  (pipe)
                  throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Illegal_Characters_In_Path, (char) num), pathRp);

               default:
                  // 32: space
                  if (num >= 32 && (!checkAdditional || num != WildcardQuestionChar && num != WildcardStarMatchAllChar))
                     continue;

                  goto case 34;
            }
         }
      }
      
      /// <summary>Tranlates DosDevicePath, Volume GUID. For example: "\Device\HarddiskVolumeX\path\filename.ext" can translate to: "\path\filename.ext" or: "\\?\Volume{GUID}\path\filename.ext".</summary>
      /// <returns>A translated dos path.</returns>
      /// <param name="dosDevice">A DosDevicePath, for example: \Device\HarddiskVolumeX\path\filename.ext.</param>
      /// <param name="deviceReplacement">Alternate path/device text, usually <c>string.Empty</c> or <see langword="null"/>.</param>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private static string DosDeviceToDosPath(string dosDevice, string deviceReplacement)
      {
         if (Utils.IsNullOrWhiteSpace(dosDevice))
            return string.Empty;

         foreach (var drive in Directory.EnumerateLogicalDrivesCore(false, false).Select(drv => drv.Name))
         {
            try
            {
               var path = RemoveTrailingDirectorySeparator(drive, false);
               foreach (var devNt in Volume.QueryDosDevice(path).Where(dosDevice.StartsWith))
                  return dosDevice.Replace(devNt, deviceReplacement ?? path);
            }
            catch
            {
            }
         }
         return string.Empty;
      }

      [SecurityCritical]
      internal static int GetRootLength(string path, bool checkInvalidPathChars)
      {
         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false, false);

         var index = 0;
         var length = path.Length;

         if (length >= 1 && IsDVsc(path[0], false))
         {
            index = 1;
            if (length >= 2 && IsDVsc(path[1], false))
            {
               index = 2;
               var num = 2;

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

      /// <summary>Check if <paramref name="c"/> is a directory- and/or volume-separator character.</summary>
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


      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      private static string NormalizePath(string path, GetFullPathOptions options)
      {
         var newBuffer = new StringBuilder(NativeMethods.MaxPathUnicode);
         var index = 0;
         uint numSpaces = 0;
         uint numDots = 0;
         var fixupDirectorySeparator = false;
         
         // Number of significant chars other than potentially suppressible
         // dots and spaces since the last directory or volume separator char
         uint numSigChars = 0;

         var lastSigChar = -1; // Index of last significant character.

         // Whether this segment of the path (not the complete path) started
         // with a volume separator char.  Reject "c:...".
         var startedWithVolumeSeparator = false;
         var firstSegment = true;
         var lastDirectorySeparatorPos = 0;

         // LEGACY: This code is here for backwards compatibility reasons. It 
         // ensures that \\foo.cs\bar.cs stays \\foo.cs\bar.cs instead of being
         // turned into \foo.cs\bar.cs.
         if (path.Length > 0 && (path[0] == DirectorySeparatorChar || path[0] == AltDirectorySeparatorChar))
         {
            newBuffer.Append('\\');
            index++;
            lastSigChar = 0;
         }

         // Normalize the string, stripping out redundant dots, spaces, and slashes.
         while (index < path.Length)
         {
            var currentChar = path[index];

            // We handle both directory separators and dots specially.  For 
            // directory separators, we consume consecutive appearances.  
            // For dots, we consume all dots beyond the second in 
            // succession.  All other characters are added as is.  In 
            // addition we consume all spaces after the last other char
            // in a directory name up until the directory separator.

            if (currentChar == DirectorySeparatorChar || currentChar == AltDirectorySeparatorChar)
            {
               // If we have a path like "123.../foo", remove the trailing dots.
               // However, if we found "c:\temp\..\bar" or "c:\temp\...\bar", don't.
               // Also remove trailing spaces from both files & directory names.
               // This was agreed on with the OS team to fix undeletable directory
               // names ending in spaces.

               // If we saw a '\' as the previous last significant character and
               // are simply going to write out dots, suppress them.
               // If we only contain dots and slashes though, only allow
               // a string like [dot]+ [space]*.  Ignore everything else.
               // Legal: "\.. \", "\...\", "\. \"
               // Illegal: "\.. .\", "\. .\", "\ .\"

               if (numSigChars == 0)
               {
                  // Dot and space handling
                  if (numDots > 0)
                  {
                     // Look for ".[space]*" or "..[space]*"
                     var start = lastSigChar + 1;
                     if (path[start] != CurrentDirectoryPrefixChar)
                        throw new ArgumentException(path);

                     // Only allow "[dot]+[space]*", and normalize the 
                     // legal ones to "." or ".."
                     if (numDots >= 2)
                     {
                        // Reject "C:..."
                        if (startedWithVolumeSeparator && numDots > 2)
                           throw new ArgumentException(path);


                        if (path[start + 1] == CurrentDirectoryPrefixChar)
                        {
                           // Search for a space in the middle of the dots and throw
                           for (var i = start + 2; i < start + numDots; i++)
                           {
                              if (path[i] != CurrentDirectoryPrefixChar)
                                 throw new ArgumentException(path);
                           }

                           numDots = 2;
                        }

                        else
                        {
                           if (numDots > 1)
                              throw new ArgumentException(path);

                           numDots = 1;
                        }
                     }


                     if (numDots == 2)
                        newBuffer.Append(CurrentDirectoryPrefixChar);


                     newBuffer.Append(CurrentDirectoryPrefixChar);
                     fixupDirectorySeparator = false;

                     // Continue in this case, potentially writing out '\'.
                  }


                  if (numSpaces > 0 && firstSegment)
                  {
                     // Handle strings like " \\server\share".
                     if (index + 1 < path.Length && (path[index + 1] == DirectorySeparatorChar || path[index + 1] == AltDirectorySeparatorChar))
                        newBuffer.Append(DirectorySeparatorChar);
                  }
               }


               numDots = 0;
               numSpaces = 0;  // Suppress trailing spaces

               if (!fixupDirectorySeparator)
               {
                  fixupDirectorySeparator = true;
                  newBuffer.Append(DirectorySeparatorChar);
               }

               numSigChars = 0;
               lastSigChar = index;
               startedWithVolumeSeparator = false;
               firstSegment = false;


               var thisPos = newBuffer.Length - 1;
               if (thisPos - lastDirectorySeparatorPos > NativeMethods.MaxDirectoryLength)
                  throw new PathTooLongException(path);

               lastDirectorySeparatorPos = thisPos;
            } // if (Found directory separator)

            else if (currentChar == CurrentDirectoryPrefixChar)
            {
               // Reduce only multiple .'s only after slash to 2 dots. For
               // instance a...b is a valid file name.
               numDots++;
               // Don't flush out non-terminal spaces here, because they may in
               // the end not be significant.  Turn "c:\ . .\foo" -> "c:\foo"
               // which is the conclusion of removing trailing dots & spaces,
               // as well as folding multiple '\' characters.
            }

            else if (currentChar == ' ')
               numSpaces++;

            else
            {  // Normal character logic
               fixupDirectorySeparator = false;

               // To reject strings like "C:...\foo" and "C  :\foo"
               if (firstSegment && currentChar == VolumeSeparatorChar)
               {
                  // Only accept "C:", not "c :" or ":"
                  // Get a drive letter or ' ' if index is 0.
                  var driveLetter = (index > 0) ? path[index - 1] : ' ';

                  var validPath = (numDots == 0) && (numSigChars >= 1) && (driveLetter != ' ');
                  if (!validPath)
                     throw new ArgumentException(path);

                  startedWithVolumeSeparator = true;
                  // We need special logic to make " c:" work, we should not fix paths like "  foo::$DATA"
                  if (numSigChars > 1)
                  {
                     // Common case, simply do nothing
                     var spaceCount = 0; // How many spaces did we write out, numSpaces has already been reset.
                     while ((spaceCount < newBuffer.Length) && newBuffer[spaceCount] == ' ')
                        spaceCount++;

                     if (numSigChars - spaceCount == 1)
                     {
                        //Safe to update stack ptr directly
                        newBuffer.Length = 0;
                        newBuffer.Append(driveLetter);
                           // Overwrite spaces, we need a special case to not break "  foo" as a relative path.
                     }
                  }

                  numSigChars = 0;
               }

               else
                  numSigChars += 1 + numDots + numSpaces;

               // Copy any spaces & dots since the last significant character
               // to here.  Note we only counted the number of dots & spaces,
               // and don't know what order they're in.  Hence the copy.
               if (numDots > 0 || numSpaces > 0)
               {
                  var numCharsToCopy = lastSigChar >= 0 ? index - lastSigChar - 1 : index;

                  if (numCharsToCopy > 0)
                     for (var i = 0; i < numCharsToCopy; i++)
                        newBuffer.Append(path[lastSigChar + 1 + i]);

                  numDots = 0;
                  numSpaces = 0;
               }

               newBuffer.Append(currentChar);
               lastSigChar = index;
            }

            index++;
         }


         if (newBuffer.Length - 1 - lastDirectorySeparatorPos > NativeMethods.MaxDirectoryLength)
            throw new PathTooLongException(path);


         // Drop any trailing dots and spaces from file & directory names, EXCEPT
         // we MUST make sure that "C:\foo\.." is correctly handled.
         // Also handle "C:\foo\." -> "C:\foo", while "C:\." -> "C:\"
         if (numSigChars == 0)
         {
            if (numDots > 0)
            {
               // Look for ".[space]*" or "..[space]*"
               var start = lastSigChar + 1;

               if (path[start] != CurrentDirectoryPrefixChar)
                  throw new ArgumentException(path);

               
               // Only allow "[dot]+[space]*", and normalize the legal ones to "." or ".."
               if (numDots >= 2)
               {
                  // Reject "C:..."
                  if (startedWithVolumeSeparator && numDots > 2)
                     throw new ArgumentException(path);


                  if (path[start + 1] == CurrentDirectoryPrefixChar)
                  {
                     // Search for a space in the middle of the dots and throw
                     for (var i = start + 2; i < start + numDots; i++)
                        if (path[i] != CurrentDirectoryPrefixChar)
                           throw new ArgumentException(path);

                     numDots = 2;
                  }

                  else
                  {
                     if (numDots > 1)
                        throw new ArgumentException(path);

                     numDots = 1;
                  }
               }

               if (numDots == 2)
                  newBuffer.Append(CurrentDirectoryPrefixChar);

               newBuffer.Append(CurrentDirectoryPrefixChar);
            }
         }


         // If we ended up eating all the characters, bail out.
         if (newBuffer.Length == 0)
            throw new ArgumentException(path);


         // Disallow URL's here.  Some of our other Win32 API calls will reject
         // them later, so we might be better off rejecting them here.
         // Note we've probably turned them into "file:\D:\foo.tmp" by now.
         // But for compatibility, ensure that callers that aren't doing a 
         // full check aren't rejected here.
         if ((options & GetFullPathOptions.FullCheck) != 0)
         {
            var newBufferString = newBuffer.ToString();
            if (newBufferString.StartsWith("http:", StringComparison.OrdinalIgnoreCase) || newBufferString.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
               throw new ArgumentException(path);
         }

         // Call the Win32 API to do the final canonicalization step.
         var result = 1;


         if (result != 0)
         {
            /* Throw an ArgumentException for paths like \\, \\server, \\server\
               This check can only be properly done after normalizing, so
               \\foo\.. will be properly rejected.  Also, reject \\?\GLOBALROOT\
               (an internal kernel path) because it provides aliases for drives. */
            if (newBuffer.Length > 1 && newBuffer[0] == '\\' && newBuffer[1] == '\\')
            {
               var startIndex = 2;
               while (startIndex < result)
               {
                  if (newBuffer[startIndex] == '\\')
                  {
                     startIndex++;
                     break;
                  }

                  startIndex++;
               }

               if (startIndex == result)
                  throw new ArgumentException(path);
            }
         }

         
         return newBuffer.ToString();
      }

      #endregion // Internal Methods
   }
}

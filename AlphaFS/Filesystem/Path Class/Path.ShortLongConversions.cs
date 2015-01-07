using Alphaleonis.Win32.Network;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region GetLongPath

      /// <summary>
      ///   [AlphaFS] Makes a Unicode path (LongPath) of the specified <paramref name="path"/> by prefixing <see cref="LongPathPrefix"/>.
      /// </summary>
      /// <param name="path">The local or UNC path to the file or directory.</param>
      /// <returns>Returns the <paramref name="path"/> prefixed with a <see cref="LongPathPrefix"/>.</returns>
      [SecurityCritical]
      public static string GetLongPath(string path)
      {
         return GetLongPathInternal(path, GetFullPathOptions.None);
      }

      #endregion // GetLongPath

      #region GetLongFrom83ShortPath

      /// <summary>[AlphaFS] Converts the specified existing path to its regular long form.</summary>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>The regular full path.</returns>
      [SecurityCritical]
      public static string GetLongFrom83ShortPath(string path)
      {
         return GetLongShort83PathInternal(null, path, false);
      }

      /// <summary>[AlphaFS] Converts the specified existing path to its regular long form.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>The regular full path.</returns>
      [SecurityCritical]
      public static string GetLongFrom83ShortPath(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathInternal(transaction, path, false);
      }

      #endregion // GetLongFrom83ShortPath

      #region GetRegularPath

      /// <summary>
      ///   [AlphaFS] Gets the regular path from long prefixed one. i.e.: \\?\C:\Temp\file.txt to C:\Temp\file.txt or: \\?\UNC\Server\share\
      ///   file.txt to \\Server\share\file.txt.
      /// </summary>
      /// <remarks>This method does not handle paths with volume names, eg. \\?\Volume{GUID}\Folder\file.txt.</remarks>
      /// <param name="path">The path.</param>
      /// <returns>Regular form path string.</returns>
      [SecurityCritical]
      public static string GetRegularPath(string path)
      {
         return GetRegularPathInternal(path, false, false, false, true);
      }

      #endregion // GetRegularPath

      #region GetShort83Path

      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>A path that has the 8.3 path form.</returns>
      [SecurityCritical]
      public static string GetShort83Path(string path)
      {
         return GetLongShort83PathInternal(null, path, true);
      }

      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>A path that has the 8.3 path form.</returns>
      [SecurityCritical]
      public static string GetShort83Path(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathInternal(transaction, path, true);
      }

      #endregion // GetShort83Path

      #region IsLongPath

      /// <summary>
      ///   [AlphaFS] Determines whether the specified path starts with a <see cref="LongPathPrefix"/> or <see cref="LongPathUncPrefix"/>.
      /// </summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>Returns <see langword="true"/> if the specified path has a long path (UNC) prefix, <see langword="false"/> otherwise.</returns>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public static bool IsLongPath(string path)
      {
         return !Utils.IsNullOrWhiteSpace(path) && path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase);
      }

      #endregion // IsLongPath

      #region Internals

      /// <summary>
      ///   [AlphaFS] Unified method GetLongPathInternal() to get a long path (Unicode path) of the specified <paramref name="path"/>.
      /// </summary>
      /// <remarks>
      ///   <para>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the
      ///   associated volume.</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <param name="path">The path to the file or directory, this may also be an UNC path.</param>
      /// <param name="options">Options for controlling the operation.</param>
      /// <returns>Returns the <paramref name="path"/> as a long path, such as "\\?\C:\MyFile.txt".</returns>
      [SecurityCritical]
      internal static string GetLongPathInternal(string path, GetFullPathOptions options)
      {
         if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
            CheckInvalidPathChars(path, false);
         else
         {
            if (path == null)
               throw new ArgumentNullException("path");

            if (path.Length == 0 || Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentException(Resources.PathIsZeroLengthOrOnlyWhiteSpace, "path");
         }

         // MSDN: Notes to Callers
         // http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
         //
         // The .NET Framework 3.5 SP1 and earlier versions maintains an internal list of white-space characters that this method trims if trimChars is null or an empty array.
         // Starting with the .NET Framework 4, if trimChars is null or an empty array, the method trims all Unicode white-space characters
         // (that is, characters that produce a true return value when they are passed to the Char.IsWhiteSpace method). Because of this change,
         // the Trim() method in the .NET Framework 3.5 SP1 and earlier versions removes two characters, ZERO WIDTH SPACE (U+200B) and ZERO WIDTH NO-BREAK SPACE (U+FEFF),
         // that the Trim() method in the .NET Framework 4 and later versions does not remove. In addition, the Trim() method in the .NET Framework 3.5 SP1 and earlier versions
         // does not trim three Unicode white-space characters: MONGOLIAN VOWEL SEPARATOR (U+180E), NARROW NO-BREAK SPACE (U+202F), and MEDIUM MATHEMATICAL SPACE (U+205F).

         if ((options & GetFullPathOptions.TrimEnd) != 0)
            path = path.TrimEnd();

         if ((options & GetFullPathOptions.AddTrailingDirectorySeparator) != 0)
            path = AddDirectorySeparator(path, false);

         if ((options & GetFullPathOptions.RemoveTrailingDirectorySeparator) != 0)
            path = RemoveDirectorySeparator(path, false);

         if (path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase) ||
             path.StartsWith(LogicalDrivePrefix, StringComparison.OrdinalIgnoreCase))
            return path;

         //path = GetRegularPathInternal(path, false, trimEnd, addDirectorySeparator, removeDirectorySeparator);

         // ".", "C:"
         return path.Length > 2 && (IsLocalPath(path, false) || IsUncPath(path, false))
            ? path.StartsWith(UncPrefix, StringComparison.OrdinalIgnoreCase)
               ? LongPathUncPrefix + path.Substring(UncPrefix.Length)
               : LongPathPrefix + path
            : path;
      }

      /// <summary>
      ///   [AlphaFS] Unified method GetLongShort83PathInternal() to retrieve the short path form, or the regular long form of the specified
      ///   <paramref name="path"/>.
      /// </summary>
      /// <remarks>
      ///   <para>Will fail on NTFS volumes with disabled 8.3 name generation.</para>
      ///   <para>The path must actually exist to be able to get the short- or long path name.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <param name="getShort">
      ///   <see langword="true"/> to retrieve the short path form, <see langword="false"/> to retrieve the regular long form from the 8.3
      ///   <paramref name="path"/>.
      /// </param>
      /// <returns>If <paramref name="getShort"/> is <see langword="true"/>, a path of the 8.3 form otherwise the regular long form.</returns>
      [SecurityCritical]
      private static string GetLongShort83PathInternal(KernelTransaction transaction, string path, bool getShort)
      {
         string pathLp = GetFullPathInternal(transaction, path, true, GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         var buffer = new StringBuilder();
         uint actualLength = getShort ? NativeMethods.GetShortPathName(pathLp, null, 0) : (uint)path.Length;

         while (actualLength > buffer.Capacity)
         {
            buffer = new StringBuilder((int)actualLength);
            actualLength = getShort

               // GetShortPathName()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2014-01-29: MSDN confirms LongPath usage.

               ? NativeMethods.GetShortPathName(pathLp, buffer, (uint)buffer.Capacity)
               : transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // GetLongPathName()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2014-01-29: MSDN confirms LongPath usage.

                  ? NativeMethods.GetLongPathName(pathLp, buffer, (uint)buffer.Capacity)
                  : NativeMethods.GetLongPathNameTransacted(pathLp, buffer, (uint)buffer.Capacity, transaction.SafeHandle);

            if (actualLength == Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(pathLp);
         }

         return GetRegularPathInternal(buffer.ToString(), false, false, false, false);
      }

      /// <summary>[AlphaFS] Unified method GetRegularPathInternal() to get the regular path from a long path.</summary>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <param name="path">The path.</param>
      /// <param name="trimEnd"><see langword="true"/> removes trailing whitespace from <paramref name="path"/>.</param>
      /// <param name="addDirectorySeparator"><see langword="true"/> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><see langword="true"/> removes any directory separator to that path.</param>
      /// <param name="checkInvalidPathChars">Checks that the path contains only valid path-characters.</param>
      /// <returns>
      ///   <para>Returns the regular form of a long <paramref name="path"/>.</para>
      ///   <para>For example: "\\?\C:\Temp\file.txt" to: "C:\Temp\file.txt", or: "\\?\UNC\Server\share\file.txt" to: "\\Server\share\
      ///   file.txt".</para>
      /// </returns>
      [SecurityCritical]
      internal static string GetRegularPathInternal(string path, bool trimEnd, bool addDirectorySeparator, bool removeDirectorySeparator, bool checkInvalidPathChars)
      {
         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false);
         else
         {
            if (path == null)
               throw new ArgumentNullException("path");

            if (path.Length == 0 || Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentException(Resources.PathIsZeroLengthOrOnlyWhiteSpace, "path");
         }

         // MSDN: Notes to Callers
         // http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
         //
         // The .NET Framework 3.5 SP1 and earlier versions maintains an internal list of white-space characters that this method trims if trimChars is null or an empty array.
         // Starting with the .NET Framework 4, if trimChars is null or an empty array, the method trims all Unicode white-space characters
         // (that is, characters that produce a true return value when they are passed to the Char.IsWhiteSpace method). Because of this change,
         // the Trim() method in the .NET Framework 3.5 SP1 and earlier versions removes two characters, ZERO WIDTH SPACE (U+200B) and ZERO WIDTH NO-BREAK SPACE (U+FEFF),
         // that the Trim() method in the .NET Framework 4 and later versions does not remove. In addition, the Trim() method in the .NET Framework 3.5 SP1 and earlier versions
         // does not trim three Unicode white-space characters: MONGOLIAN VOWEL SEPARATOR (U+180E), NARROW NO-BREAK SPACE (U+202F), and MEDIUM MATHEMATICAL SPACE (U+205F).

         if (trimEnd) path = path.TrimEnd();

         if (removeDirectorySeparator)
            path = path.TrimEnd(DirectorySeparatorChar, AltDirectorySeparatorChar);

         if (addDirectorySeparator)
            path = AddDirectorySeparator(path, false);


         if (!path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase))
            return path;


         if (path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase))
            return UncPrefix + path.Substring(LongPathUncPrefix.Length);


         return path.Substring(LongPathPrefix.Length);
      }


      /// <summary>Gets the path as a long full path.</summary>
      /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">Full pathname of the source path to convert.</param>
      /// <param name="pathFormat">The path format to use.</param>
      /// <param name="options">Options for controlling the operation. Note that on .NET 3.5 the TrimEnd option has no effect.</param>
      /// <returns>The path as an extended length path.</returns>
      internal static string GetExtendedLengthPathInternal(KernelTransaction transaction, string sourcePath, PathFormat pathFormat, GetFullPathOptions options)
      {
         switch (pathFormat)
         {
            case PathFormat.LongFullPath:
               return sourcePath;

            case PathFormat.FullPath:
               return GetLongPathInternal(sourcePath, GetFullPathOptions.None);

            case PathFormat.Relative:
#if NET35
               // .NET 3.5 the TrimEnd option has no effect.
               options = options & ~GetFullPathOptions.TrimEnd;
#endif
               return GetFullPathInternal(transaction, sourcePath, true, options);

            default:
               throw new ArgumentException("Invalid value for " + typeof(PathFormat).Name + ": " + pathFormat);
         }
      }
      #endregion

   }
}

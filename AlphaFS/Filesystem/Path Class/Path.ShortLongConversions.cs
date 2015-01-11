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
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region GetLongPath

      /// <summary>[AlphaFS] Makes a Unicode path (LongPath) of the specified <paramref name="path"/> by prefixing <see cref="LongPathPrefix"/>.</summary>
      /// <returns>Returns the <paramref name="path"/> prefixed with a <see cref="LongPathPrefix"/>.</returns>
      /// <param name="path">The local or UNC path to the file or directory.</param>
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

      /// <summary>[AlphaFS] Gets the regular path from long prefixed one. i.e.: "\\?\C:\Temp\file.txt" to C:\Temp\file.txt" or: "\\?\UNC\Server\share\file.txt" to "\\Server\share\file.txt".</summary>
      /// <returns>Regular form path string.</returns>
      /// <remarks>This method does not handle paths with volume names, eg. \\?\Volume{GUID}\Folder\file.txt.</remarks>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetRegularPath(string path)
      {
         return GetRegularPathInternal(path, GetFullPathOptions.CheckInvalidPathChars);
      }

      #endregion // GetRegularPath

      #region GetShort83Path

      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetShort83Path(string path)
      {
         return GetLongShort83PathInternal(null, path, true);
      }

      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetShort83Path(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathInternal(transaction, path, true);
      }

      #endregion // GetShort83Path

      #region IsLongPath

      /// <summary>[AlphaFS] Determines whether the specified path starts with a <see cref="LongPathPrefix"/> or <see cref="LongPathUncPrefix"/>.</summary>
      /// <returns>Returns <see langword="true"/> if the specified path has a long path (UNC) prefix, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to the file or directory.</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public static bool IsLongPath(string path)
      {
         return !Utils.IsNullOrWhiteSpace(path) && path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase);
      }

      #endregion // IsLongPath

      #region Internals Methods

      /// <summary>[AlphaFS] Unified method GetLongPathInternal() to get a long path (Unicode path) of the specified <paramref name="path"/>.</summary>
      /// <returns>Returns the <paramref name="path"/> as a long path, such as "\\?\C:\MyFile.txt".</returns>
      /// <remarks>
      ///   <para>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</para>
      ///   MSDN: String.TrimEnd Method notes to Callers: http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <param name="path">The path to the file or directory, this may also be an UNC path.</param>
      /// <param name="options">Options for controlling the operation.</param>
      [SecurityCritical]
      internal static string GetLongPathInternal(string path, GetFullPathOptions options)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (path.Length == 0 || Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentException(Resources.PathIsZeroLengthOrOnlyWhiteSpace, "path");

         if (options != GetFullPathOptions.None)
         {
            if ((options & GetFullPathOptions.TrimEnd) != 0)
               path = path.TrimEnd();

            if ((options & GetFullPathOptions.AddTrailingDirectorySeparator) != 0)
               path = AddTrailingDirectorySeparator(path, false);

            if ((options & GetFullPathOptions.RemoveTrailingDirectorySeparator) != 0)
               path = RemoveTrailingDirectorySeparator(path, false);

            if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
               CheckInvalidPathChars(path, false);
         }

         
         if (path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase) ||
             path.StartsWith(LogicalDrivePrefix, StringComparison.OrdinalIgnoreCase))
            return path;

         // ".", "C:"
         return path.Length > 2 && (IsLocalPath(path, false) || IsUncPath(path, false))
            ? path.StartsWith(UncPrefix, StringComparison.OrdinalIgnoreCase)
               ? LongPathUncPrefix + path.Substring(UncPrefix.Length)
               : LongPathPrefix + path
            : path;
         // 2015-01-11 Issue #50: Path.GetLongPath() does not prefix on "C:", should it?
      }

      /// <summary>[AlphaFS] Unified method GetLongShort83PathInternal() to retrieve the short path form, or the regular long form of the specified <paramref name="path"/>.</summary>
      /// <returns>If <paramref name="getShort"/> is <see langword="true"/>, a path of the 8.3 form otherwise the regular long form.</returns>
      /// <remarks>
      ///   <para>Will fail on NTFS volumes with disabled 8.3 name generation.</para>
      ///   <para>The path must actually exist to be able to get the short- or long path name.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <param name="getShort"><see langword="true"/> to retrieve the short path form, <see langword="false"/> to retrieve the regular long form from the 8.3 <paramref name="path"/>.</param>
      [SecurityCritical]
      private static string GetLongShort83PathInternal(KernelTransaction transaction, string path, bool getShort)
      {
         string pathLp = GetFullPathInternal(transaction, path, GetFullPathOptions.AsLongPath | GetFullPathOptions.FullCheck);

         var buffer = new StringBuilder();
         uint actualLength = getShort ? NativeMethods.GetShortPathName(pathLp, null, 0) : (uint) path.Length;

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

         return GetRegularPathInternal(buffer.ToString(), GetFullPathOptions.None);
      }

      /// <summary>[AlphaFS] Unified method GetRegularPathInternal() to get the regular path from a long path.</summary>
      /// <returns>
      ///   <para>Returns the regular form of a long <paramref name="path"/>.</para>
      ///   <para>For example: "\\?\C:\Temp\file.txt" to: "C:\Temp\file.txt", or: "\\?\UNC\Server\share\file.txt" to: "\\Server\share\file.txt".</para>
      /// </returns>
      /// <remarks>
      ///   MSDN: String.TrimEnd Method notes to Callers: http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <param name="path">The path.</param>
      /// <param name="options">Options for controlling the operation.</param>
      [SecurityCritical]
      internal static string GetRegularPathInternal(string path, GetFullPathOptions options)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (path.Length == 0 || Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentException(Resources.PathIsZeroLengthOrOnlyWhiteSpace, "path");

         if (options != GetFullPathOptions.None)
         {
            if ((options & GetFullPathOptions.TrimEnd) != 0)
               path = path.TrimEnd();

            if ((options & GetFullPathOptions.AddTrailingDirectorySeparator) != 0)
               path = AddTrailingDirectorySeparator(path, false);

            if ((options & GetFullPathOptions.RemoveTrailingDirectorySeparator) != 0)
               path = RemoveTrailingDirectorySeparator(path, false);

            if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
               CheckInvalidPathChars(path, false);
         }

         
         if (!path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase))
            return path;

         return path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase)
            ? UncPrefix + path.Substring(LongPathUncPrefix.Length)
            : path.Substring(LongPathPrefix.Length);
      }

      /// <summary>Gets the path as a long full path.</summary>
      /// <returns>The path as an extended length path.</returns>
      /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">Full pathname of the source path to convert.</param>
      /// <param name="pathFormat">The path format to use.</param>
      /// <param name="options">Options for controlling the operation. Note that on .NET 3.5 the TrimEnd option has no effect.</param>
      internal static string GetExtendedLengthPathInternal(KernelTransaction transaction, string sourcePath, PathFormat pathFormat, GetFullPathOptions options)
      {
         switch (pathFormat)
         {
            case PathFormat.LongFullPath:
               return sourcePath;

            case PathFormat.FullPath:
               return GetLongPathInternal(sourcePath, GetFullPathOptions.None);

            case PathFormat.RelativePath:
#if NET35
               // .NET 3.5 the TrimEnd option has no effect.
               options = options & ~GetFullPathOptions.TrimEnd;
#endif
               return GetFullPathInternal(transaction, sourcePath, GetFullPathOptions.AsLongPath | options);

            default:
               throw new ArgumentException("Invalid value for " + typeof(PathFormat).Name + ": " + pathFormat);
         }
      }

      #endregion // Internals Methods
   }
}
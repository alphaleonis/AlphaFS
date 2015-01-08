using Alphaleonis.Win32.Network;
using System;
using System.Globalization;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region GetMappedConnectionName

      /// <summary>[AlphaFS] Gets the connection name of the locally mapped drive.</summary>
      /// <param name="path">The local path with drive name.</param>
      /// <returns>The server and share as: \\servername\sharename.</returns>
      ///
      /// <exception cref="System.IO.PathTooLongException">When <paramref name="path"/> exceeds maximum path length.</exception>
      [SecurityCritical]
      public static string GetMappedConnectionName(string path)
      {
         return Host.GetRemoteNameInfoInternal(path, true).ConnectionName;
      }

      #endregion // GetMappedConnectionName

      #region GetMappedUncName

      /// <summary>[AlphaFS] Gets the network share name from the locally mapped path.</summary>
      /// <param name="path">The local path with drive name.</param>
      /// <returns>The network share connection name of <paramref name="path"/>.</returns>
      ///
      /// <exception cref="System.IO.PathTooLongException">When <paramref name="path"/> exceeds maximum path length.</exception>
      [SecurityCritical]
      public static string GetMappedUncName(string path)
      {
         return Host.GetRemoteNameInfoInternal(path, true).UniversalName;
      }

      #endregion // GetMappedUncName

      #region IsUncPath

      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path.</summary>
      /// <param name="path">The path to check.</param>
      /// <returns>Returns <see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      [SecurityCritical]
      public static bool IsUncPath(string path)
      {
         return IsUncPath(path, true);
      }

      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path.</summary>
      /// <param name="path">The path to check.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>Returns <see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      [SecurityCritical]
      public static bool IsUncPath(string path, bool checkInvalidPathChars)
      {
         Uri uri;
         return Uri.TryCreate(GetRegularPathInternal(path, false, false, false, checkInvalidPathChars), UriKind.Absolute, out uri) && uri.IsUnc;
      }

      #endregion // IsUncPath

      #region LocalToUnc

      /// <summary>
      ///   [AlphaFS] Converts a local path to a network share path.
      ///   
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"</para>
      ///   <para>If a logical drive points to a network share path, the share path will be returned.</para>
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <returns>Returns a UNC path or <see langword="null"/> when <paramref name="localPath"/> is an empty string or <see langword="null"/>.</returns>
      [SecurityCritical]
      public static string LocalToUnc(string localPath)
      {
         return LocalToUncInternal(localPath, false, false, false, false);
      }

      /// <summary>
      ///   [AlphaFS] Converts a local path to a network share path.
      ///   
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"</para>
      ///   <para>If a logical drive points to a network share path, the share path will be returned.</para>
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="asLongPath">
      ///   <see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.
      /// </param>
      /// <returns>
      ///   Returns a UNC path in long path format or <see langword="null"/> when <paramref name="localPath"/> is an empty string or <see langword="null"/>.
      /// </returns>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, bool asLongPath)
      {
         return LocalToUncInternal(localPath, asLongPath, false, false, false);
      }

      /// <summary>
      ///   [AlphaFS] Converts a local path to a network share path.
      ///   
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"</para>
      ///   <para>If a logical drive points to a network share path, the share path will be returned.</para>
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      /// <param name="addTrailingDirectorySeparator"><see langword="true"/> adds a trailing <see cref="DirectorySeparatorChar"/> character to <paramref name="localPath"/>, when absent.</param>
      /// <param name="removeTrailingDirectorySeparator"><see langword="true"/> removes the trailing <see cref="DirectorySeparatorChar"/> character from <paramref name="localPath"/>, when present.</param>
      /// <returns>
      ///   Returns a UNC path in long path format or <see langword="null"/> when <paramref name="localPath"/> is an empty string or <see langword="null"/>.
      /// </returns>      
      [SecurityCritical]
      public static string LocalToUnc(string localPath, bool asLongPath, bool addTrailingDirectorySeparator, bool removeTrailingDirectorySeparator)
      {
         return LocalToUncInternal(localPath, asLongPath, false, addTrailingDirectorySeparator, removeTrailingDirectorySeparator);
      }

      #endregion // LocalToUnc

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method LocalToUncInternal() to converts a local path to a network share path. A Local path, e.g.: "C:\Windows"
      ///   will be returned as: "\\localhostname\C$\Windows" If a logical drive points to a network share path, the share path will be
      ///   returned.
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="asLongPath">
      ///   <see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular
      ///   path.
      /// </param>
      /// <param name="trimEnd"><see langword="true"/> removes trailing whitespace from <paramref name="localPath"/>.</param>
      /// <param name="addTrailingDirectorySeparator"><see langword="true"/> adds a trailing <see cref="DirectorySeparatorChar"/> character to <paramref name="localPath"/>, when absent.</param>
      /// <param name="removeTrailingDirectorySeparator"><see langword="true"/> removes the trailing <see cref="DirectorySeparatorChar"/> character from <paramref name="localPath"/>, when present.</param>
      /// <returns>
      ///   A UNC path or <see langword="null"/> when <paramref name="localPath"/> is an empty string or <see langword="null"/>.
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException"/>
      [SecurityCritical]
      internal static string LocalToUncInternal(string localPath, bool asLongPath, bool trimEnd, bool addTrailingDirectorySeparator, bool removeTrailingDirectorySeparator)
      {
         localPath = (localPath[0] == CurrentDirectoryPrefixChar) || !IsPathRooted(localPath, false)
            ? GetFullPathInternal(null, localPath, asLongPath, (trimEnd ? GetFullPathOptions.TrimEnd : 0) | (addTrailingDirectorySeparator ? GetFullPathOptions.AddTrailingDirectorySeparator : 0) | (removeTrailingDirectorySeparator ? GetFullPathOptions.RemoveTrailingDirectorySeparator : 0) | GetFullPathOptions.CheckInvalidPathChars)
            : GetRegularPathInternal(localPath, trimEnd, addTrailingDirectorySeparator, removeTrailingDirectorySeparator, true);

         if (IsUncPath(localPath, false))
            return localPath;

         string drive = IsLocalPath(localPath, false) ? GetPathRoot(localPath, false) : null;

         if (Utils.IsNullOrWhiteSpace(drive))
            return null;


         Network.NativeMethods.RemoteNameInfo unc = Host.GetRemoteNameInfoInternal(drive, true);

         if (!Utils.IsNullOrWhiteSpace(unc.ConnectionName))
            // Only leave trailing backslash if "localPath" also ends with backslash.
            return localPath.EndsWith(DirectorySeparator, StringComparison.OrdinalIgnoreCase) ? AddTrailingDirectorySeparator(unc.ConnectionName, false) : RemoveTrailingDirectorySeparator(unc.ConnectionName, false);

         // Split: localDrive[0] = "C", localDrive[1] = "\Windows"
         string[] localDrive = localPath.Split(VolumeSeparatorChar);

         // Return: "\\MachineName\C$\Windows"
         string pathUnc = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}${3}", Host.GetUncName(), DirectorySeparatorChar, localDrive[0], localDrive[1]);

         // Only leave trailing backslash if "localPath" also ends with backslash.
         return localPath.EndsWith(DirectorySeparator, StringComparison.OrdinalIgnoreCase) ? AddTrailingDirectorySeparator(pathUnc, false) : RemoveTrailingDirectorySeparator(pathUnc, false);
      }

      #endregion

   }
}

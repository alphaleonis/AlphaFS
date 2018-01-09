/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis.Win32.Network;
using System;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>[AlphaFS] Gets the connection name of the locally mapped drive.</summary>
      /// <returns>The server and share as: \\servername\sharename.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="path">The local path with drive name.</param>
      [SecurityCritical]
      public static string GetMappedConnectionName(string path)
      {
         return Host.GetRemoteNameInfoCore(path, true).lpConnectionName;
      }




      /// <summary>[AlphaFS] Gets the network share name from the locally mapped path.</summary>
      /// <returns>The network share connection name of <paramref name="path"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="path">The local path with drive name.</param>
      [SecurityCritical]
      public static string GetMappedUncName(string path)
      {
         return Host.GetRemoteNameInfoCore(path, true).lpUniversalName;
      }




      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path.</summary>
      /// <returns><see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to check.</param>
      [SecurityCritical]
      public static bool IsUncPath(string path)
      {
         return IsUncPathCore(path, false, true);
      }


      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path, optionally skip invalid path character check.</summary>
      /// <returns><see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to check.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      public static bool IsUncPath(string path, bool checkInvalidPathChars)
      {
         return IsUncPathCore(path, false, checkInvalidPathChars);
      }




      /// <summary>[AlphaFS] Converts a local path to a network share path.  
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\MachineName\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned instead.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      [SecurityCritical]
      public static string LocalToUnc(string localPath)
      {
         return LocalToUncCore(localPath, false, false, false);
      }


      /// <summary>[AlphaFS] Converts a local path to a network share path, optionally returning it in a long path format.
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\MachineName\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned instead.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, bool asLongPath)
      {
         return LocalToUncCore(localPath, asLongPath, false, false);
      }


      /// <summary>[AlphaFS] Converts a local path to a network share path, optionally returning it in a long path format and the ability to add or remove a trailing backslash.
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\MachineName\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned instead.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      /// <param name="addTrailingDirectorySeparator"><see langword="true"/> adds a trailing <see cref="DirectorySeparatorChar"/> character to <paramref name="localPath"/>, when absent.</param>
      /// <param name="removeTrailingDirectorySeparator"><see langword="true"/> removes the trailing <see cref="DirectorySeparatorChar"/> character from <paramref name="localPath"/>, when present.</param>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, bool asLongPath, bool addTrailingDirectorySeparator, bool removeTrailingDirectorySeparator)
      {
         return LocalToUncCore(localPath, asLongPath, addTrailingDirectorySeparator, removeTrailingDirectorySeparator);
      }


      #region Internal Methods

      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path, optionally skip invalid path character check.</summary>
      /// <returns><see langword="true"/> if the specified path is a Universal Naming Convention (UNC) path, <see langword="false"/> otherwise.</returns>
      /// <param name="path">The path to check.</param>
      /// <param name="isRegularPath">When <see langword="true"/> indicates that <paramref name="path"/> is already in regular path format.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      internal static bool IsUncPathCore(string path, bool isRegularPath, bool checkInvalidPathChars)
      {
         if (!isRegularPath)
            path = GetRegularPathCore(path, checkInvalidPathChars ? GetFullPathOptions.CheckInvalidPathChars : 0, false);

         else if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false, false);

         Uri uri;
         return Uri.TryCreate(path, UriKind.Absolute, out uri) && uri.IsUnc;
      }


      /// <summary>Converts a local path to a network share path.  
      ///   <para>A Local path, e.g.: "C:\Windows" will be returned as: "\\MachineName\C$\Windows".</para>
      ///   <para>If a logical drive points to a network share path (mapped drive), the share path will be returned instead.</para>
      /// </summary>
      /// <returns>On successful conversion a UNC path is returned.
      ///   <para>If the conversion fails, <paramref name="localPath"/> is returned.</para>
      ///   <para>If <paramref name="localPath"/> is an empty string or <see langword="null"/>, <see langword="null"/> is returned.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="localPath">A local path, e.g.: "C:\Windows".</param>
      /// <param name="asLongPath"><see langword="true"/> returns the path in long path (Unicode) format, when <see langword="false"/> returns the path as a regular path.</param>
      /// <param name="addTrailingDirectorySeparator"><see langword="true"/> adds a trailing <see cref="DirectorySeparatorChar"/> character to <paramref name="localPath"/>, when absent.</param>
      /// <param name="removeTrailingDirectorySeparator"><see langword="true"/> removes the trailing <see cref="DirectorySeparatorChar"/> character from <paramref name="localPath"/>, when present.</param>
      [SecurityCritical]
      internal static string LocalToUncCore(string localPath, bool asLongPath, bool addTrailingDirectorySeparator, bool removeTrailingDirectorySeparator)
      {
         if (Utils.IsNullOrWhiteSpace(localPath))
            return null;

         var returnPath = GetRegularPathCore(localPath, GetFullPathOptions.CheckInvalidPathChars, false);


         if (!IsUncPathCore(returnPath, true, false))
         {
            if (returnPath[0] == CurrentDirectoryPrefixChar || !IsPathRooted(returnPath, false))
               returnPath = GetFullPathCore(null, returnPath, GetFullPathOptions.None);

            var drive = GetPathRoot(returnPath, false);

            if (Utils.IsNullOrWhiteSpace(drive))
               return returnPath;


            var unc = Host.GetRemoteNameInfoCore(returnPath, true);

            if (!Utils.IsNullOrWhiteSpace(unc.lpUniversalName))
            {
               // Only leave trailing backslash if "localPath" also ends with backslash.

               returnPath = returnPath.EndsWith(DirectorySeparator, StringComparison.Ordinal)
                  ? AddTrailingDirectorySeparator(unc.lpUniversalName, false)
                  : RemoveTrailingDirectorySeparator(unc.lpUniversalName);

               return asLongPath ? GetLongPathCore(returnPath, GetFullPathOptions.None) : returnPath;
            }


            if (!Utils.IsNullOrWhiteSpace(unc.lpConnectionName))
            {
               // Only leave trailing backslash if "localPath" also ends with backslash.

               returnPath = returnPath.EndsWith(DirectorySeparator, StringComparison.Ordinal)
                  ? AddTrailingDirectorySeparator(unc.lpConnectionName, false)
                  : RemoveTrailingDirectorySeparator(unc.lpConnectionName);

               return asLongPath ? GetLongPathCore(returnPath, GetFullPathOptions.None) : returnPath;
            }


            // Split: localDrive[0] = "C", localDrive[1] = "\Windows"
            var localDrive = returnPath.Split(VolumeSeparatorChar);

            // Return: "\\MachineName\C$\Windows"
            returnPath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}${3}", Host.GetUncName(), DirectorySeparator, localDrive[0], localDrive[1]);
         }


         // Only leave trailing backslash if "localPath" also ends with backslash.
         addTrailingDirectorySeparator = addTrailingDirectorySeparator || returnPath.EndsWith(DirectorySeparator, StringComparison.Ordinal) && !removeTrailingDirectorySeparator;

         var options = (addTrailingDirectorySeparator ? GetFullPathOptions.AddTrailingDirectorySeparator : 0) |
                       (removeTrailingDirectorySeparator ? GetFullPathOptions.RemoveTrailingDirectorySeparator : 0);

         return asLongPath ? GetLongPathCore(returnPath, options) : returnPath;
      }

      #endregion // Internal Methods
   }
}

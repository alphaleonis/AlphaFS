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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using FileStream = System.IO.FileStream;
using StreamReader = System.IO.StreamReader;
using StreamWriter = System.IO.StreamWriter;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="FileStream"/> objects.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
   public static partial class File
   {
      #region Compress

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Compress(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>      
      [SecurityCritical]
      public static void Compress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, true, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>      
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // Compress

      #region CreateHardlink

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(string fileName, string existingFileName, PathFormat pathFormat)
      {
         CreateHardlinkInternal(null, fileName, existingFileName, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(string fileName, string existingFileName)
      {
         CreateHardlinkInternal(null, fileName, existingFileName, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(KernelTransaction transaction, string fileName, string existingFileName, PathFormat pathFormat)
      {
         CreateHardlinkInternal(transaction, fileName, existingFileName, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(KernelTransaction transaction, string fileName, string existingFileName)
      {
         CreateHardlinkInternal(transaction, fileName, existingFileName, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // CreateHardlink

      #region CreateSymbolicLink

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         CreateSymbolicLinkInternal(null, symlinkFileName, targetFileName, targetType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkInternal(null, symlinkFileName, targetFileName, targetType, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         CreateSymbolicLinkInternal(transaction, symlinkFileName, targetFileName, targetType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkInternal(transaction, symlinkFileName, targetFileName, targetType, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // CreateSymbolicLink

      #region Decompress

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Decompress(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>      
      [SecurityCritical]
      public static void Decompress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, false, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>      
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // Decompress

      #region EnumerateHardlinks

      #region IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(string path, PathFormat pathFormat)
      {
         return EnumerateHardlinksInternal(null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(string path)
      {
         return EnumerateHardlinksInternal(null, path, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return EnumerateHardlinksInternal(transaction, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(KernelTransaction transaction, string path)
      {
         return EnumerateHardlinksInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // EnumerateHardlinks

      #region EnumerateStreams

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by
      ///   <paramref name="handle"/>.
      /// </summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, handle, null, null, null, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // EnumerateStreams

      #region GetChangeTime

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, null, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path)
      {
         return GetChangeTimeInternal(false, null, null, path, false, PathFormat.Auto);
      }

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(SafeFileHandle safeHandle)
      {
         return GetChangeTimeInternal(false, null, safeHandle, null, false, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, transaction, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path)
      {
         return GetChangeTimeInternal(false, transaction, null, path, false, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetChangeTime

      #region GetChangeTimeUtc

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, null, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path)
      {
         return GetChangeTimeInternal(false, null, null, path, true, PathFormat.Auto);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(SafeFileHandle safeHandle)
      {
         return GetChangeTimeInternal(false, null, safeHandle, null, true, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, transaction, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path)
      {
         return GetChangeTimeInternal(false, transaction, null, path, true, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetChangeTimeUtc

      #region GetCompressedSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>
      ///   If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size
      ///   of the specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value
      ///   obtained is the sparse size of the specified file.
      /// </remarks>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(string path, PathFormat pathFormat)
      {
         return GetCompressedSizeInternal(null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>
      ///   If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size
      ///   of the specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value
      ///   obtained is the sparse size of the specified file.
      /// </remarks>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(string path)
      {
         return GetCompressedSizeInternal(null, path, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction. If the file
      ///   is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the
      ///   specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is
      ///   the sparse size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetCompressedSizeInternal(transaction, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction. If the file
      ///   is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the
      ///   specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is
      ///   the sparse size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(KernelTransaction transaction, string path)
      {
         return GetCompressedSizeInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetCompressedSize

      #region GetEncryptionStatus

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>      
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path, PathFormat pathFormat)
      {
         return GetEncryptionStatusInternal(path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>      
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path)
      {
         return GetEncryptionStatusInternal(path, PathFormat.Auto);
      }

      #endregion // GetEncryptionStatus

      #region GetFileInfoByHandle

      /// <summary>[AlphaFS] Retrieves file information for the specified <see cref="SafeFileHandle"/>.</summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the open file from which to retrieve the information.</param>
      /// <returns>A <see cref="ByHandleFileInfo"/> object containing the requested information.</returns>
      [SecurityCritical]
      public static ByHandleFileInfo GetFileInfoByHandle(SafeFileHandle handle)
      {
         NativeMethods.IsValidHandle(handle);

         NativeMethods.ByHandleFileInfo info;

         if (!NativeMethods.GetFileInformationByHandle(handle, out info))
            // Throws IOException.
            NativeError.ThrowException(Marshal.GetLastWin32Error(), true);

         return new ByHandleFileInfo(info);
      }

      #endregion // GetFileInfoByHandle

      #region GetFileSystemEntry

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path, PathFormat pathFormat)
      {
         return GetFileSystemEntryInfoInternal(null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="path">The path to the file or directory.</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path)
      {
         return GetFileSystemEntryInfoInternal(null, path, false, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetFileSystemEntryInfoInternal(transaction, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoInternal(transaction, path, false, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetFileSystemEntry

      #region GetLinkTargetInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(string path, PathFormat pathFormat)
      {
         return GetLinkTargetInfoInternal(null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="path">The path to the reparse point.</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(string path)
      {
         return GetLinkTargetInfoInternal(null, path, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetLinkTargetInfoInternal(transaction, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string path)
      {
         return GetLinkTargetInfoInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetLinkTargetInfo

      #region GetSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path, PathFormat pathFormat)
      {
         return GetSizeInternal(null, null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path)
      {
         return GetSizeInternal(null, null, path, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(SafeFileHandle handle)
      {
         return GetSizeInternal(null, handle, null, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetSizeInternal(transaction, null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path)
      {
         return GetSizeInternal(transaction, null, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetSize

      #region GetStreamSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, name, StreamType.Data, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, type, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, name, StreamType.Data, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, type, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, handle, null, name, StreamType.Data, PathFormat.ExtendedLength);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, handle, null, null, type, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, name, StreamType.Data, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, type, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, name, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, type, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion GetStreamSize

      #region RemoveStream

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>      
      [SecurityCritical]
      public static void RemoveStream(string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, PathFormat.Auto);
      }

      #endregion Transacted

      #endregion // RemoveStream

      #region SetTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>     
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // SetTimestamps

      #region SetTimestampsUtc

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // SetTimestampsUtc

      #region TransferTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         TransferTimestampsInternal(false, null, sourcePath, destinationPath, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>      
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath)
      {
         TransferTimestampsInternal(false, null, sourcePath, destinationPath, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         TransferTimestampsInternal(false, transaction, sourcePath, destinationPath, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>      
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         TransferTimestampsInternal(false, transaction, sourcePath, destinationPath, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // TransferTimestamps


      #region Unified Internals

      

      #region CreateHardlinkInternal

      /// <summary>
      ///   [AlphaFS] Unified method CreateHardlinkInternal() to establish a hard link between an existing file and a new file. This function
      ///   is only supported on the NTFS file system, and only for files, not directories.
      /// </summary>
      /// <exception cref="NotSupportedException">Thrown when the requested operation is not supported.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      internal static void CreateHardlinkInternal(KernelTransaction transaction, string fileName, string existingFileName, PathFormat pathFormat)
      {
         string fileNameLp = Path.GetExtendedLengthPathInternal(transaction, fileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         string existingFileNameLp = Path.GetExtendedLengthPathInternal(transaction, existingFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // CreateHardLink() / CreateHardLinkTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.CreateHardLink(fileNameLp, existingFileNameLp, IntPtr.Zero)
            : NativeMethods.CreateHardLinkTransacted(fileNameLp, existingFileNameLp, IntPtr.Zero, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_INVALID_FUNCTION:
                  throw new NotSupportedException(Resources.HardLinksOnNonNTFSPartitionsIsNotSupported);

               default:
                  // Throws IOException.
                  NativeError.ThrowException(lastError, fileNameLp, existingFileName, true);
                  break;
            }
         }
      }

      #endregion // CreateHardlinkInternal

      #region CreateSymbolicLinkInternal

      /// <summary>[AlphaFS] Unified method CreateSymbolicLinkInternal() to create a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void CreateSymbolicLinkInternal(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         string symlinkFileNameLp = Path.GetExtendedLengthPathInternal(transaction, symlinkFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         string targetFileNameLp = Path.GetExtendedLengthPathInternal(transaction, targetFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // CreateSymbolicLink() / CreateSymbolicLinkTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-02-14: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.CreateSymbolicLink(symlinkFileNameLp, targetFileNameLp, targetType)
            : NativeMethods.CreateSymbolicLinkTransacted(symlinkFileNameLp, targetFileNameLp, targetType, transaction.SafeHandle)))
            NativeError.ThrowException(symlinkFileNameLp, targetFileNameLp);
      }

      #endregion // CreateSymbolicLinkInternal

      

      

      

      #region EnumerateHardlinksInternal

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <exception cref="PlatformNotSupportedException">Thrown when a Platform Not Supported error condition occurs.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      internal static IEnumerable<string> EnumerateHardlinksInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         // Default buffer length, will be extended if needed, although this should not happen.
         uint length = NativeMethods.MaxPathUnicode;
         StringBuilder builder = new StringBuilder((int)length);


      getFindFirstFileName:

         using (SafeFindFileHandle handle = transaction == null

            // FindFirstFileName() / FindFirstFileNameTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.FindFirstFileName(pathLp, 0, out length, builder)
            : NativeMethods.FindFirstFileNameTransacted(pathLp, 0, out length, builder, transaction.SafeHandle))
         {
            if (handle.IsInvalid)
            {
               int lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                     builder = new StringBuilder((int)length);
                     handle.Close();
                     goto getFindFirstFileName;

                  default:
                     // If the function fails, the return value is INVALID_HANDLE_VALUE.
                     NativeError.ThrowException(lastError, pathLp);
                     break;
               }
            }

            yield return builder.ToString();


            //length = NativeMethods.MaxPathUnicode;
            //builder = new StringBuilder((int)length);

            do
            {
               while (!NativeMethods.FindNextFileName(handle, out length, builder))
               {
                  int lastError = Marshal.GetLastWin32Error();
                  switch ((uint)lastError)
                  {
                     // We've reached the end of the enumeration.
                     case Win32Errors.ERROR_HANDLE_EOF:
                        yield break;

                     case Win32Errors.ERROR_MORE_DATA:
                        builder = new StringBuilder((int)length);
                        continue;

                     default:
                        //If the function fails, the return value is zero (0).
                        // Throws IOException.
                        NativeError.ThrowException(lastError, true);
                        break;
                  }
               }

               yield return builder.ToString();

            } while (true);
         }
      }

      #endregion // EnumerateHardlinksInternal

      

      #region FillAttributeInfoInternal

      /// <summary>
      ///   Calls NativeMethods.GetFileAttributesEx to retrieve Win32FileAttributeData.
      ///   <para>Note that classes should use -1 as the uninitialized state for dataInitialized when relying on this method.</para>
      /// </summary>
      /// <remarks>No path (null, empty string) checking or normalization is performed.</remarks>
      /// <param name="transaction">.</param>
      /// <param name="pathLp">.</param>
      /// <param name="win32AttrData">[in,out].</param>
      /// <param name="tryagain">.</param>
      /// <param name="returnErrorOnNotFound">.</param>
      /// <returns>Returns 0 on success, otherwise a Win32 error code.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      internal static int FillAttributeInfoInternal(KernelTransaction transaction, string pathLp, ref NativeMethods.Win32FileAttributeData win32AttrData, bool tryagain, bool returnErrorOnNotFound)
      {
         int dataInitialised = (int)Win32Errors.ERROR_SUCCESS;

         #region Try Again

         // Someone has a handle to the file open, or other error.
         if (tryagain)
         {
            NativeMethods.Win32FindData findData;

            // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               bool error = false;

               SafeFindFileHandle handle = transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // FindFirstFileEx() / FindFirstFileTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  // A trailing backslash is not allowed.
                  ? NativeMethods.FindFirstFileEx(Path.RemoveDirectorySeparator(pathLp, false), NativeMethods.FindExInfoLevel, out findData, NativeMethods.FindExSearchOps.SearchNameMatch, IntPtr.Zero, NativeMethods.LargeCache)
                  : NativeMethods.FindFirstFileTransacted(Path.RemoveDirectorySeparator(pathLp, false), NativeMethods.FindExInfoLevel, out findData, NativeMethods.FindExSearchOps.SearchNameMatch, IntPtr.Zero, NativeMethods.LargeCache, transaction.SafeHandle);

               try
               {
                  if (handle.IsInvalid)
                  {
                     error = true;
                     dataInitialised = Marshal.GetLastWin32Error();

                     if (dataInitialised == Win32Errors.ERROR_FILE_NOT_FOUND ||
                         dataInitialised == Win32Errors.ERROR_PATH_NOT_FOUND ||
                         dataInitialised == Win32Errors.ERROR_NOT_READY) // Floppy device not ready.
                     {
                        if (!returnErrorOnNotFound)
                        {
                           // Return default value for backward compatibility
                           dataInitialised = (int)Win32Errors.ERROR_SUCCESS;
                           win32AttrData.FileAttributes = (FileAttributes)(-1);
                        }
                     }

                     return dataInitialised;
                  }
               }
               finally
               {
                  try
                  {
                     // Close the Win32 handle.
                     handle.Close();
                  }
                  catch
                  {
                     // If we're already returning an error, don't throw another one.
                     if (!error)
                        NativeError.ThrowException(dataInitialised, pathLp, true);
                  }
               }
            }

            // Copy the attribute information.
            win32AttrData = new NativeMethods.Win32FileAttributeData(findData);
         }

         #endregion // Try Again

         else
         {
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // GetFileAttributesEx() / GetFileAttributesTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  ? NativeMethods.GetFileAttributesEx(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData)
                  : NativeMethods.GetFileAttributesTransacted(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData, transaction.SafeHandle)))
               {
                  dataInitialised = Marshal.GetLastWin32Error();

                  if (dataInitialised != Win32Errors.ERROR_FILE_NOT_FOUND &&
                      dataInitialised != Win32Errors.ERROR_PATH_NOT_FOUND &&
                      dataInitialised != Win32Errors.ERROR_NOT_READY) // Floppy device not ready.
                  {
                     // In case someone latched onto the file. Take the perf hit only for failure.
                     return FillAttributeInfoInternal(transaction, pathLp, ref win32AttrData, true, returnErrorOnNotFound);
                  }

                  if (!returnErrorOnNotFound)
                  {
                     // Return default value for backward compbatibility.
                     dataInitialised = (int)Win32Errors.ERROR_SUCCESS;
                     win32AttrData.FileAttributes = (FileAttributes)(-1);
                  }
               }
            }
         }

         return dataInitialised;
      }

      #endregion //FillAttributeInfoInternal

      

      #region GetCompressedSizeInternal

      /// <summary>
      ///   [AlphaFS] Unified method GetCompressedSizeInternal() to retrieve the actual number of bytes of disk storage used to store a
      ///   specified file as part of a transaction. If the file is located on a volume that supports compression and the file is compressed,
      ///   the value obtained is the compressed size of the specified file. If the file is located on a volume that supports sparse files and
      ///   the file is a sparse file, the value obtained is the sparse size of the specified file.
      /// </summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>      
      [SecurityCritical]
      internal static long GetCompressedSizeInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         if (pathFormat != PathFormat.ExtendedLength && Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         uint fileSizeHigh;
         uint fileSizeLow = transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // GetCompressedFileSize() / GetCompressedFileSizeTransacted()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.GetCompressedFileSize(pathLp, out fileSizeHigh)
            : NativeMethods.GetCompressedFileSizeTransacted(pathLp, out fileSizeHigh, transaction.SafeHandle);

         // If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE.
         if (fileSizeLow == Win32Errors.ERROR_INVALID_FILE_SIZE && fileSizeHigh == 0)
            NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

         return NativeMethods.ToLong(fileSizeHigh, fileSizeLow);
      }

      #endregion // GetCompressedSizeInternal

      #region GetChangeTimeInternal

      /// <summary>[AlphaFS] Unified method GetChangeTimeInternal() to get the change date and time of the specified file.</summary>
      /// <remarks><para>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</para></remarks>
      /// <exception cref="PlatformNotSupportedException">Thrown when a Platform Not Supported error condition occurs.</exception>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <param name="getUtc">
      ///   <see langword="true"/> gets the Coordinated Universal Time (UTC), <see langword="false"/> gets the local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns a <see cref="System.DateTime"/> structure set to the change date and time for the specified file.</para>
      ///   <para>This value is expressed in local time.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SecurityCritical]
      internal static DateTime GetChangeTimeInternal(bool isFolder, KernelTransaction transaction, SafeFileHandle safeHandle, string path, bool getUtc, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (pathFormat != PathFormat.ExtendedLength && Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

            safeHandle = CreateFileInternal(transaction, pathLp, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.ReadData, FileShare.ReadWrite, true, PathFormat.ExtendedLength);
         }


         try
         {
            NativeMethods.IsValidHandle(safeHandle);

            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               NativeMethods.IsValidHandle(safeBuffer);

               if (!NativeMethods.GetFileInformationByHandleEx(safeHandle, NativeMethods.FileInfoByHandleClass.FileBasicInfo, safeBuffer, NativeMethods.DefaultFileBufferSize))
                  // Throws IOException.
                  NativeError.ThrowException(Marshal.GetLastWin32Error(), true);


               // CA2001:AvoidCallingProblematicMethods

               IntPtr buffer = IntPtr.Zero;
               bool successRef = false;
               safeBuffer.DangerousAddRef(ref successRef);

               // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
               if (successRef)
                  buffer = safeBuffer.DangerousGetHandle();

               safeBuffer.DangerousRelease();

               if (buffer == IntPtr.Zero)
                  NativeError.ThrowException(Resources.HandleDangerousRef);

               // CA2001:AvoidCallingProblematicMethods


               NativeMethods.FileTime changeTime = Utils.MarshalPtrToStructure<NativeMethods.FileBasicInfo>(0, buffer).ChangeTime;

               return getUtc
                  ? DateTime.FromFileTimeUtc(changeTime)
                  : DateTime.FromFileTime(changeTime);
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // GetChangeTimeInternal

      

      #region GetEncryptionStatusInternal

      /// <summary>[AlphaFS] Unified method GetEncryptionStatusInternal() to retrieve the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>
      [SecurityCritical]
      internal static FileEncryptionStatus GetEncryptionStatusInternal(string path, PathFormat pathFormat)
      {
         if (pathFormat != PathFormat.ExtendedLength && Utils.IsNullOrWhiteSpace(path))
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

      #endregion // GetEncryptionStatusInternal

      #region GetFileSystemEntryInfoInternal

      /// <summary>[AlphaFS] Unified method GetFileSystemEntryInfoInternal() to get a FileSystemEntryInfo from a Non-/Transacted directory/file.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory, or <c>null</c> on Exception when <paramref name="continueOnException"/> is <c>true</c>.</returns>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="continueOnException">
      ///    <para><c>true</c> suppress any Exception that might be thrown a result from a failure,</para>
      ///    <para>such as ACLs protected directories or non-accessible reparse points.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static FileSystemEntryInfo GetFileSystemEntryInfoInternal(KernelTransaction transaction, string path, bool continueOnException, PathFormat pathFormat)
      {
         // // Enable BasicSearch and LargeCache by default.
         var directoryEnumerationOptions = DirectoryEnumerationOptions.BasicSearch | DirectoryEnumerationOptions.LargeCache;

         if (continueOnException)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.ContinueOnException;

         return (new FindFileSystemEntryInfo(false, transaction, path, Path.WildcardQuestion, directoryEnumerationOptions, typeof(FileSystemEntryInfo), pathFormat)).Get<FileSystemEntryInfo>();
      }

      #endregion // GetFileSystemEntryInfoInternal

      #region GetLinkTargetInfoInternal

      /// <summary>
      ///   [AlphaFS] Unified method GetLinkTargetInfoInternal() to get information about the target of a mount point or symbolic link on an
      ///   NTFS file system.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfoInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         using (SafeFileHandle safeHandle = CreateFileInternal(transaction, path, ExtendedFileAttributes.OpenReparsePoint | ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, 0, FileShare.ReadWrite, true, pathFormat))
            return Device.GetLinkTargetInfoInternal(safeHandle);
      }

      #endregion // GetLinkTargetInfoInternal

      #region GetSizeInternal

      /// <summary>[AlphaFS] Unified method GetSizeInternal() to retrieve the file size, in bytes to store a specified file.</summary>
      /// <remarks>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static long GetSizeInternal(KernelTransaction transaction, SafeFileHandle safeHandle, string path, PathFormat pathFormat)
      {
         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

            safeHandle = CreateFileInternal(transaction, pathLp, ExtendedFileAttributes.None, null, FileMode.Open, FileSystemRights.ReadData, FileShare.Read, true, PathFormat.ExtendedLength);
         }


         long fileSize;

         try
         {
            NativeMethods.GetFileSizeEx(safeHandle, out fileSize);
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }

         return fileSize;
      }

      #endregion // GetSizeInternal
      

      #region TransferTimestampsInternal

      /// <summary>
      ///   [AlphaFS] Unified method TransferTimestampsInternal() to transfer the date and time stamps for the specified files and directories.
      /// </summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      /// <param name="isFolder">
      ///   Specifies that <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are a file or directory.
      /// </param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source path.</param>
      /// <param name="destinationPath">The destination path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void TransferTimestampsInternal(bool isFolder, KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         NativeMethods.Win32FileAttributeData attrs = GetAttributesExInternal<NativeMethods.Win32FileAttributeData>(transaction, sourcePath, pathFormat);

         SetFsoDateTimeInternal(isFolder, transaction, destinationPath, DateTime.FromFileTimeUtc(attrs.CreationTime), DateTime.FromFileTimeUtc(attrs.LastAccessTime), DateTime.FromFileTimeUtc(attrs.LastWriteTime), pathFormat);
      }

      #endregion // TransferTimestampsInternal

      

      

      #endregion // Unified Internals

   }
}
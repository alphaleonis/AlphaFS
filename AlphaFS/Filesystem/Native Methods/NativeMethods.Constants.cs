using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      public static readonly bool IsAtLeastWindows7 = OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows7);
      public static readonly bool IsAtLeastWindowsVista = OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsVista);

      /// <summary>The FindFirstFileEx function does not query the short file name, improving overall enumeration speed.
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The data is returned in a <see cref="NativeMethods.Win32FindData"/> structure,</para>
      /// <para>and cAlternateFileName member is always a NULL string.</para>
      /// <para>This value is not supported until Windows Server 2008 R2 and Windows 7.</para>
      /// </remarks>
      /// </summary>
      public static readonly FindExInfoLevels FindExInfoLevel = IsAtLeastWindows7 ? FindExInfoLevels.Basic : FindExInfoLevels.Standard;

      /// <summary>Uses a larger buffer for directory queries, which can increase performance of the find operation.</summary>
      /// <remarks>This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
      public static readonly FindExAdditionalFlags LargeCache = IsAtLeastWindows7 ? FindExAdditionalFlags.LargeFetch : FindExAdditionalFlags.None;

      /// <summary>DefaultFileBufferSize = 4096; Default type buffer size used for reading and writing files.</summary>
      public const int DefaultFileBufferSize = 4096;

      /// <summary>DefaultFileEncoding = Encoding.UTF8; Default type of Encoding used for reading and writing files.</summary>
      public static readonly Encoding DefaultFileEncoding = Encoding.UTF8;

      /// <summary>MaxPath = 260
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. 
      /// </summary>
      internal const int MaxPath = 260;

      /// <summary>MaxPathUnicode = 32000</summary>
      internal const int MaxPathUnicode = 32000;
   }
}

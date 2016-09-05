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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Retrieves the full path and file name of the specified file or directory.</summary>
      /// <returns>If the function fails for any other reason, the return value is zero. To get extended error information, call GetLastError.</returns>
      /// <remarks>The GetFullPathName function is not recommended for multithreaded applications or shared library code.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFullPathNameW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetFullPathName([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint nBufferLength, StringBuilder lpBuffer, IntPtr lpFilePart);

      /// <summary>Retrieves the full path and file name of the specified file or directory as a transacted operation.</summary>
      /// <returns>If the function fails for any other reason, the return value is zero. To get extended error information, call GetLastError.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFullPathNameTransactedW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetFullPathNameTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint nBufferLength, StringBuilder lpBuffer, IntPtr lpFilePart, SafeHandle hTransaction);

      /// <summary>Converts the specified path to its long form.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetLongPathNameW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetLongPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszShortPath, StringBuilder lpszLongPath, [MarshalAs(UnmanagedType.U4)] uint cchBuffer);

      /// <summary>Converts the specified path to its long form as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetLongPathNameTransactedW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetLongPathNameTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpszShortPath, StringBuilder lpszLongPath, [MarshalAs(UnmanagedType.U4)] uint cchBuffer, SafeHandle hTransaction);

      /// <summary>Retrieves the short path form of the specified path.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetShortPathNameW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPWStr)] string lpszLongPath, StringBuilder lpszShortPath, [MarshalAs(UnmanagedType.U4)] uint cchBuffer);

      /// <summary>Converts a file URL to a Microsoft MS-DOS path.</summary>
      /// <returns>Type: HRESULT
      /// If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PathCreateFromUrlW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint PathCreateFromUrl([MarshalAs(UnmanagedType.LPWStr)] string pszUrl, StringBuilder pszPath, [MarshalAs(UnmanagedType.U4)] ref uint pcchPath, [MarshalAs(UnmanagedType.U4)] uint dwFlags);

      /// <summary>Creates a path from a file URL.</summary>
      /// <returns>Type: HRESULT
      /// If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint PathCreateFromUrlAlloc([MarshalAs(UnmanagedType.LPWStr)] string pszIn, out StringBuilder pszPath, [MarshalAs(UnmanagedType.U4)] uint dwFlags);

      /// <summary>Converts a Microsoft MS-DOS path to a canonicalized URL.</summary>
      /// <returns>Type: HRESULT
      /// Returns S_FALSE if pszPath is already in URL format. In this case, pszPath will simply be copied to pszUrl.
      /// Otherwise, it returns S_OK if successful or a standard COM error value if not.
      /// </returns>
      /// <remarks>
      /// UrlCreateFromPath does not support extended paths. These are paths that include the extended-length path prefix "\\?\".
      /// </remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "UrlCreateFromPathW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint UrlCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, StringBuilder pszUrl, ref uint pcchUrl, [MarshalAs(UnmanagedType.U4)] uint dwFlags);

      /// <summary>Tests whether a URL is a specified type.</summary>
      /// <returns>
      /// Type: BOOL
      /// For all but one of the URL types, UrlIs returns <see langword="true"/> if the URL is the specified type, <see langword="true"/> otherwise.
      /// If UrlIs is set to <see cref="Shell32.UrlType.IsAppliable"/>, UrlIs will attempt to determine the URL scheme.
      /// If the function is able to determine a scheme, it returns <see langword="true"/>, or <see langword="false"/>.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "UrlIsW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UrlIs([MarshalAs(UnmanagedType.LPWStr)] string pszUrl, Shell32.UrlType urlIs);
   }
}

/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32
{
   /// <summary>[AlphaFS] Static class providing access to information about the operating system under which the assembly is executing.</summary>
   public static partial class OperatingSystem
   {
      private static class NativeMethods
      {
         internal const short VER_NT_WORKSTATION = 1;
         internal const short VER_NT_DOMAIN_CONTROLLER = 2;
         internal const short VER_NT_SERVER = 3;


         [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
         internal struct RTL_OSVERSIONINFOEXW
         {
            public int dwOSVersionInfoSize;
            public readonly int dwMajorVersion;
            public readonly int dwMinorVersion;
            public readonly int dwBuildNumber;
            public readonly int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string szCSDVersion;
            public readonly ushort wServicePackMajor;
            public readonly ushort wServicePackMinor;
            public readonly ushort wSuiteMask;
            public readonly byte wProductType;
            public readonly byte wReserved;
         }


         [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
         internal struct SYSTEM_INFO
         {
            public readonly EnumProcessorArchitecture wProcessorArchitecture;
            public readonly ushort wReserved;
            public readonly uint dwPageSize;
            public readonly IntPtr lpMinimumApplicationAddress;
            public readonly IntPtr lpMaximumApplicationAddress;
            public readonly IntPtr dwActiveProcessorMask;
            public readonly uint dwNumberOfProcessors;
            public readonly uint dwProcessorType;
            public readonly uint dwAllocationGranularity;
            public readonly ushort wProcessorLevel;
            public readonly ushort wProcessorRevision;
         }


         /// <summary>The RtlGetVersion routine returns version information about the currently running operating system.</summary>
         /// <returns>RtlGetVersion returns STATUS_SUCCESS.</returns>
         /// <remarks>Available starting with Windows 2000.</remarks>
         [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
         [DllImport("ntdll.dll", SetLastError = true, CharSet = CharSet.Unicode)]
         [return: MarshalAs(UnmanagedType.Bool)]
         internal static extern bool RtlGetVersion([MarshalAs(UnmanagedType.Struct)] ref RTL_OSVERSIONINFOEXW lpVersionInformation);


         /// <summary>Retrieves information about the current system to an application running under WOW64.
         /// If the function is called from a 64-bit application, it is equivalent to the GetSystemInfo function.
         /// </summary>
         /// <returns>This function does not return a value.</returns>
         /// <remarks>To determine whether a Win32-based application is running under WOW64, call the <see cref="IsWow64Process"/> function.</remarks>
         /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</remarks>
         /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</remarks>
         [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
         [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
         internal static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);


         /// <summary>Determines whether the specified process is running under WOW64.</summary>
         /// <returns>
         /// If the function succeeds, the return value is a nonzero value.
         /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
         /// </returns>
         /// <remarks>Minimum supported client: Windows Vista, Windows XP with SP2 [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows Server 2008, Windows Server 2003 with SP1 [desktop apps only]</remarks>
         [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
         [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
         [return: MarshalAs(UnmanagedType.Bool)]
         internal static extern bool IsWow64Process([In] IntPtr hProcess, [Out, MarshalAs(UnmanagedType.Bool)] out bool lpSystemInfo);
      }
   }
}

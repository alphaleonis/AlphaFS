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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32
{
   /// <summary>Static class providing access to information about the operating system under which the assembly is executing.</summary>
   public static class OperatingSystem
   {
      #region OperatingSystem Name Enum

      /// <summary>A set of flags that describe the named Windows versions.</summary>
      /// <remarks>The values of the enumeration are ordered. A later released operating system version has a higher number, so comparisons between named versions are meaningful.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      public enum EnumOsName
      {
         /// <summary>A Windows version earlier than Windows 2000.</summary>
         Earlier = -1,

         /// <summary>Windows 2000 (Server or Professional).</summary>
         Windows2000 = 0,

         /// <summary>Windows XP.</summary>
         WindowsXP = 1,

         /// <summary>Windows Server 2003.</summary>
         WindowsServer2003 = 2,

         /// <summary>Windows Vista.</summary>
         WindowsVista = 3,

         /// <summary>Windows Server 2008.</summary>
         WindowsServer2008 = 4,

         /// <summary>Windows 7.</summary>
         Windows7 = 5,

         /// <summary>Windows Server 2008 R2.</summary>
         WindowsServer2008R2 = 6,

         /// <summary>Windows 8.</summary>
         Windows8 = 7,

         /// <summary>Windows Server 2012.</summary>
         WindowsServer2012 = 8,

         /// <summary>Windows 8.1.</summary>
         Windows81 = 9,

         /// <summary>Windows Server 2012 R2.</summary>
         WindowsServer2012R2 = 10,

         /// <summary>A later version of Windows than currently installed.</summary>
         Later = 65535
      }

      #endregion // OperatingSystem Name Enum

      #region ProcessorArchitecture Name enum

      /// <summary>A set of flags to indicate the current processor architecture for which the operating system is targeted and running.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pa")]
      [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
      public enum EnumProcessorArchitecture : ushort
      {
         /// <summary>PROCESSOR_ARCHITECTURE_INTEL
         /// <para>The system is running a 32-bit version of Windows.</para>
         /// </summary>
         X86 = 0,

         /// <summary>PROCESSOR_ARCHITECTURE_IA64
         /// <para>The system is running on a Itanium processor.</para>
         /// </summary>
         [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
         [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
         IA64 = 6,

         /// <summary>PROCESSOR_ARCHITECTURE_AMD64
         /// <para>The system is running a 64-bit version of Windows.</para>
         /// </summary>
         X64 = 9,

         /// <summary>PROCESSOR_ARCHITECTURE_UNKNOWN
         /// <para>Unknown architecture.</para>
         /// </summary>
         Unknown = 65535
      }

      #endregion // ProcessorArchitecture Name enum


      #region Properties

      #region IsServer

      private static bool _isServer;

      /// <summary>Gets a value indicating whether the operating system is a server operating system.</summary>
      /// <value><see langword="true"/> if the current operating system is a server operating system; otherwise, <see langword="false"/>.</value>
      public static bool IsServer
      {
         get
         {
            if (_servicePackVersion == null)
               UpdateData();
            return _isServer;
         }
      }

      #endregion // IsServer

      #region IsWow64Process

      private static bool? _isWow64Process;

      /// <summary>Gets a value indicating whether the current process is running under WOW64.</summary>
      /// <value><see langword="true"/> if the current process is running under WOW64; otherwise, <see langword="false"/>.</value>
      public static bool IsWow64Process
      {
         get
         {
            if (_isWow64Process == null)
            {
               bool value;
               IntPtr processHandle = Process.GetCurrentProcess().Handle;

               if (!NativeMethods.IsWow64Process(processHandle, out value))
                  Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());

               // A pointer to a value that is set to TRUE if the process is running under WOW64.
               // If the process is running under 32-bit Windows, the value is set to FALSE.
               // If the process is a 64-bit application running under 64-bit Windows, the value is also set to FALSE.

               _isWow64Process = value;
            }

            return (bool) _isWow64Process;
         }
      }

      #endregion // IsWow64Process

      #region OSVersion

      /// <summary>Gets the numeric version of the operating system. This is the same as returned by <see cref="System.Environment.OSVersion"/>.</summary>            
      /// <value>The numeric version of the operating system.</value>
      public static Version OSVersion
      {
         get { return Environment.OSVersion.Version; }
      }

      #endregion // OSVersion

      #region VersionName

      private static EnumOsName _enumOsName = EnumOsName.Later;

      /// <summary>Gets the named version of the operating system.</summary>
      /// <value>The named version of the operating system.</value>
      public static EnumOsName VersionName
      {
         get
         {
            if (_servicePackVersion == null)
               UpdateData();
            return _enumOsName;
         }
      }

      #endregion // VersionName

      #region ProcessorArchitecture

      private static EnumProcessorArchitecture _processorArchitecture;

      /// <summary>Gets the processor architecture for which the operating system is targeted.</summary>
      /// <value>The processor architecture for which the operating system is targeted.</value>
      /// <remarks>If running under WOW64 this will return a 32-bit processor. Use <see cref="IsWow64Process"/> to determine if this is the case.</remarks>
      public static EnumProcessorArchitecture ProcessorArchitecture
      {
         get
         {
            if (_servicePackVersion == null)
               UpdateData();
            return _processorArchitecture;
         }
      }

      #endregion // ProcessorArchitecture

      #region ServicePackVersion

      private static Version _servicePackVersion;

      /// <summary>Gets the version of the service pack currently installed on the operating system.</summary>
      /// <value>The version of the service pack currently installed on the operating system.</value>
      /// <remarks>Only the <see cref="System.Version.Major"/> and <see cref="System.Version.Minor"/> fields are used.</remarks>
      public static Version ServicePackVersion
      {
         get
         {
            if (_servicePackVersion == null)
               UpdateData();
            return _servicePackVersion;
         }
      }

      #endregion // ServicePackVersion

      #endregion // Properties

      #region Methods

      #region IsAtLeast

      /// <summary>Determines whether the operating system is of the specified version or later.</summary>
      /// <returns><see langword="true"/> if the operating system is of the specified <paramref name="version"/> or later; otherwise, <see langword="false"/>.</returns>      
      /// <param name="version">The lowest version for which to return true.</param>
      public static bool IsAtLeast(EnumOsName version)
      {
         return VersionName >= version;
      }

      /// <summary>Determines whether the operating system is of the specified version or later, allowing specification of a minimum service pack that must be installed on the lowest version.</summary>
      /// <returns><see langword="true"/> if the operating system matches the specified <paramref name="version"/> with the specified service pack, or if the operating system is of a later version; otherwise, <see langword="false"/>.</returns>      
      /// <param name="version">The minimum required version.</param>
      /// <param name="servicePackVersion">The major version of the service pack that must be installed on the minimum required version to return true. This can be 0 to indicate that no service pack is required.</param>
      public static bool IsAtLeast(EnumOsName version, int servicePackVersion)
      {
         return IsAtLeast(version) && ServicePackVersion.Major >= servicePackVersion;
      }

      #endregion // IsAtLeast
      
      #endregion // Methods


      #region Private

      private static void UpdateData()
      {
         NativeMethods.OsVersionInfoEx verInfo = new NativeMethods.OsVersionInfoEx();

         // Needed to prevent: System.Runtime.InteropServices.COMException:
         // The data area passed to a system call is too small. (Exception from HRESULT: 0x8007007A)
         verInfo.OSVersionInfoSize = Marshal.SizeOf(verInfo);

         NativeMethods.SystemInfo sysInfo = new NativeMethods.SystemInfo();
         NativeMethods.GetSystemInfo(ref sysInfo);

         if (!NativeMethods.GetVersionEx(ref verInfo))
            NativeError.ThrowException(Marshal.GetLastWin32Error());

         Debug.Assert(verInfo.MajorVersion == Environment.OSVersion.Version.Major);
         Debug.Assert(verInfo.MinorVersion == Environment.OSVersion.Version.Minor);
         Debug.Assert(verInfo.BuildNumber == Environment.OSVersion.Version.Build);

         _processorArchitecture = (EnumProcessorArchitecture)sysInfo.processorArchitecture;
         _servicePackVersion = new Version(verInfo.ServicePackMajor, verInfo.ServicePackMinor);
         _isServer = verInfo.ProductType == NativeMethods.VerNtDomainController || verInfo.ProductType == NativeMethods.VerNtServer;


         // http://msdn.microsoft.com/en-us/library/windows/desktop/ms724833%28v=vs.85%29.aspx

         // The following table summarizes the most recent operating system version numbers.
         //    Operating system	            Version number    Other
         // ================================================================================
         //    Windows 8.1                   6.3               OSVersionInfoEx.ProductType == VerNtWorkstation
         //    Windows Server 2012 R2        6.3               OSVersionInfoEx.ProductType != VerNtWorkstation
         //    Windows 8	                  6.2               OSVersionInfoEx.ProductType == VerNtWorkstation
         //    Windows Server 2012	         6.2               OSVersionInfoEx.ProductType != VerNtWorkstation
         //    Windows 7	                  6.1               OSVersionInfoEx.ProductType == VerNtWorkstation
         //    Windows Server 2008 R2	      6.1               OSVersionInfoEx.ProductType != VerNtWorkstation
         //    Windows Server 2008	         6.0               OSVersionInfoEx.ProductType != VerNtWorkstation  
         //    Windows Vista	               6.0               OSVersionInfoEx.ProductType == VerNtWorkstation
         //    Windows Server 2003 R2	      5.2               GetSystemMetrics(SM_SERVERR2) != 0
         //    Windows Home Server  	      5.2               OSVersionInfoEx.SuiteMask & VER_SUITE_WH_SERVER
         //    Windows Server 2003           5.2               GetSystemMetrics(SM_SERVERR2) == 0
         //    Windows XP 64-Bit Edition     5.2               (OSVersionInfoEx.ProductType == VerNtWorkstation) && (sysInfo.PaName == PaName.X64)
         //    Windows XP	                  5.1               Not applicable
         //    Windows 2000	               5.0               Not applicable


         if (verInfo.MajorVersion > 6)
            _enumOsName = EnumOsName.Later;

         else
            switch (verInfo.MajorVersion)
            {
                  #region Version 6

               case 6:
                  switch (verInfo.MinorVersion)
                  {
                        // Windows Vista or Windows Server 2008
                     case 0:
                        _enumOsName = (verInfo.ProductType == NativeMethods.VerNtWorkstation)
                           ? EnumOsName.WindowsVista
                           : EnumOsName.WindowsServer2008;
                        break;

                        // Windows 7 or Windows Server 2008 R2
                     case 1:
                        _enumOsName = (verInfo.ProductType == NativeMethods.VerNtWorkstation)
                           ? EnumOsName.Windows7
                           : EnumOsName.WindowsServer2008R2;
                        break;

                        // Windows 8 or Windows Server 2012
                     case 2:
                        _enumOsName = (verInfo.ProductType == NativeMethods.VerNtWorkstation)
                           ? EnumOsName.Windows8
                           : EnumOsName.WindowsServer2012;
                        break;

                        // Windows 8.1 or Windows Server 2012R2
                     case 3:
                        _enumOsName = (verInfo.ProductType == NativeMethods.VerNtWorkstation)
                           ? EnumOsName.Windows81
                           : EnumOsName.WindowsServer2012R2;
                        break;

                     default:
                        _enumOsName = EnumOsName.Later;
                        break;
                  }
                  break;

                  #endregion // Version 6

                  #region Version 5

               case 5:
                  switch (verInfo.MinorVersion)
                  {
                     case 0:
                        _enumOsName = EnumOsName.Windows2000;
                        break;

                     case 1:
                        _enumOsName = EnumOsName.WindowsXP;
                        break;

                     case 2:
                        _enumOsName = (verInfo.ProductType == NativeMethods.VerNtWorkstation && _processorArchitecture == EnumProcessorArchitecture.X64)
                           ? EnumOsName.WindowsXP
                           : (verInfo.ProductType != NativeMethods.VerNtWorkstation)
                              ? EnumOsName.WindowsServer2003
                              : EnumOsName.Later;
                        break;

                     default:
                        _enumOsName = EnumOsName.Later;
                        break;
                  }
                  break;

                  #endregion // Version 5

               default:
                  _enumOsName = EnumOsName.Earlier;
                  break;
            }
      }
      
      #region P/Invoke members / NativeMethods

      private static class NativeMethods
      {
         public const short VerNtWorkstation = 1;
         public const short VerNtDomainController = 2;
         public const short VerNtServer = 3;

         #region OsVersionInfoEx

         [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
         public struct OsVersionInfoEx
         {
            public int OSVersionInfoSize;
            public readonly int MajorVersion;
            public readonly int MinorVersion;
            public readonly int BuildNumber;
            public readonly int PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public readonly string CSDVersion;
            public readonly UInt16 ServicePackMajor;
            public readonly UInt16 ServicePackMinor;
            public readonly UInt16 SuiteMask;
            public readonly byte ProductType;
            public readonly byte Reserved;
         }

         #endregion // OsVersionInfoEx

         #region SystemInfo

         [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
         public struct SystemInfo
         {
            public readonly ushort processorArchitecture;
            private readonly ushort reserved;
            public readonly uint pageSize;
            public readonly IntPtr minimumApplicationAddress;
            public readonly IntPtr maximumApplicationAddress;
            public readonly IntPtr activeProcessorMask;
            public readonly uint numberOfProcessors;
            public readonly uint processorType;
            public readonly uint allocationGranularity;
            public readonly ushort processorLevel;
            public readonly ushort processorRevision;
         }

         #endregion SystemInfo

         #region GetVersionEx

         /// <summary>Retrieves information about the current operating system.</summary>
         /// <returns>
         /// If the function succeeds, the return value is a nonzero value.
         /// If the function fails, the return value is zero. To get extended error information, call GetLastError. The function fails if you specify an invalid value for the dwOSVersionInfoSize member of the OSVERSIONINFO or OSVERSIONINFOEX structure.
         /// </returns>
         /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
         [SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api")]
         [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetVersionExW")]
         [return: MarshalAs(UnmanagedType.Bool)]
         public static extern bool GetVersionEx([MarshalAs(UnmanagedType.Struct)] ref OsVersionInfoEx osvi);

         #endregion // GetVersionEx

         #region GetSystemInfo

         /// <summary>Retrieves information about the current system.</summary>
         /// <returns>This function does not return a value.</returns>
         /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
         [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
         public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SystemInfo lpSystemInfo);

         #endregion // GetSystemInfo

         #region IsWow64Process

         /// <summary>Determines whether the specified process is running under WOW64.</summary>
         /// <returns>
         /// If the function succeeds, the return value is a nonzero value.
         /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
         /// </returns>
         /// <remarks>Minimum supported client: Windows Vista, Windows XP with SP2 [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows Server 2008, Windows Server 2003 with SP1 [desktop apps only]</remarks>
         [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
         [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
         [return: MarshalAs(UnmanagedType.Bool)]
         public static extern bool IsWow64Process([In] IntPtr hProcess, [Out, MarshalAs(UnmanagedType.Bool)] out bool lpSystemInfo);

         #endregion // IsWow64Process
      }

      #endregion // P/Invoke members / NativeMethods

      #endregion // Private
   }
}
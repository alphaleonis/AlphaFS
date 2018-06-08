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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32
{
   /// <summary>[AlphaFS] Static class providing access to information about the operating system under which the assembly is executing.</summary>
   public static partial class OperatingSystem
   {
      #region Private Fields

      private static bool _isServer;
      private static bool? _isWow64Process;
      private static Version _osVersion;
      private static EnumOsName _enumOsName = EnumOsName.Later;
      private static EnumProcessorArchitecture _processorArchitecture;
      private static Version _servicePackVersion;

      #endregion // Private Fields


      #region Properties

      /// <summary>[AlphaFS] Gets a value indicating whether the operating system is a server operating system.</summary>
      /// <value><c>true</c> if the current operating system is a server operating system; otherwise, <c>false</c>.</value>
      public static bool IsServer
      {
         get
         {
            if (null == _servicePackVersion)
               UpdateData();

            return _isServer;
         }
      }

      
      /// <summary>[AlphaFS] Gets a value indicating whether the current process is running under WOW64.</summary>
      /// <value><c>true</c> if the current process is running under WOW64; otherwise, <c>false</c>.</value>
      public static bool IsWow64Process
      {
         get
         {
            if (null == _isWow64Process)
            {
               bool value;
               var processHandle = Process.GetCurrentProcess().Handle;

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


      /// <summary>[AlphaFS] Gets the numeric version of the operating system.</summary>            
      /// <value>The numeric version of the operating system.</value>
      public static Version OSVersion
      {
         get
         {
            if (null == _osVersion)
               UpdateData();

            return _osVersion;
         }
      }

      
      /// <summary>[AlphaFS] Gets the named version of the operating system.</summary>
      /// <value>The named version of the operating system.</value>
      public static EnumOsName VersionName
      {
         get
         {
            if (null == _servicePackVersion)
               UpdateData();

            return _enumOsName;
         }
      }

      
      /// <summary>[AlphaFS] Gets the processor architecture for which the operating system is targeted.</summary>
      /// <value>The processor architecture for which the operating system is targeted.</value>
      /// <remarks>If running under WOW64 this will return a 32-bit processor. Use <see cref="IsWow64Process"/> to determine if this is the case.</remarks>      
      public static EnumProcessorArchitecture ProcessorArchitecture
      {
         get
         {
            if (null == _servicePackVersion)
               UpdateData();

            return _processorArchitecture;
         }
      }

      
      /// <summary>[AlphaFS] Gets the version of the service pack currently installed on the operating system.</summary>
      /// <value>The version of the service pack currently installed on the operating system.</value>
      /// <remarks>Only the <see cref="System.Version.Major"/> and <see cref="System.Version.Minor"/> fields are used.</remarks>
      public static Version ServicePackVersion
      {
         get
         {
            if (null == _servicePackVersion)
               UpdateData();

            return _servicePackVersion;
         }
      }

      #endregion // Properties
      

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RtlGetVersion")]
      private static void UpdateData()
      {
         var verInfo = new NativeMethods.RTL_OSVERSIONINFOEXW();

         // Needed to prevent: System.Runtime.InteropServices.COMException:
         // The data area passed to a system call is too small. (Exception from HRESULT: 0x8007007A)
         verInfo.dwOSVersionInfoSize = Marshal.SizeOf(verInfo);

         var sysInfo = new NativeMethods.SYSTEM_INFO();
         NativeMethods.GetNativeSystemInfo(ref sysInfo);


         // RtlGetVersion returns STATUS_SUCCESS (0).
         var success = !NativeMethods.RtlGetVersion(ref verInfo);
         
         var lastError = Marshal.GetLastWin32Error();
         if (!success)
            throw new Win32Exception(lastError, "Function RtlGetVersion() failed to retrieve the operating system information.");


         _osVersion = new Version(verInfo.dwMajorVersion, verInfo.dwMinorVersion, verInfo.dwBuildNumber);

         _processorArchitecture = sysInfo.wProcessorArchitecture;
         _servicePackVersion = new Version(verInfo.wServicePackMajor, verInfo.wServicePackMinor);
         _isServer = verInfo.wProductType == NativeMethods.VER_NT_DOMAIN_CONTROLLER || verInfo.wProductType == NativeMethods.VER_NT_SERVER;


         // RtlGetVersion: https://msdn.microsoft.com/en-us/library/windows/hardware/ff561910%28v=vs.85%29.aspx
         // Operating System Version: https://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx

         // The following table summarizes the most recent operating system version numbers.
         //    Operating system	            Version number    Other
         // ================================================================================
         //    Windows 10                    10.0              OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
         //    Windows Server 2016           10.0              OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION
         //    Windows 8.1                   6.3               OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
         //    Windows Server 2012 R2        6.3               OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION
         //    Windows 8	                  6.2               OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
         //    Windows Server 2012	         6.2               OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION
         //    Windows 7	                  6.1               OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
         //    Windows Server 2008 R2	      6.1               OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION
         //    Windows Server 2008	         6.0               OSVERSIONINFOEX.wProductType != VER_NT_WORKSTATION  
         //    Windows Vista	               6.0               OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION
         //    Windows Server 2003 R2	      5.2               GetSystemMetrics(SM_SERVERR2) != 0
         //    Windows Server 2003           5.2               GetSystemMetrics(SM_SERVERR2) == 0
         //    Windows XP 64-Bit Edition     5.2               (OSVERSIONINFOEX.wProductType == VER_NT_WORKSTATION) && (sysInfo.PaName == PaName.X64)
         //    Windows XP	                  5.1               Not applicable
         //    Windows 2000	               5.0               Not applicable


         // 2017-01-07: 10 == The lastest MajorVersion of Windows.
         if (verInfo.dwMajorVersion > 10)
            _enumOsName = EnumOsName.Later;

         else
            switch (verInfo.dwMajorVersion)
            {
               #region Version 10

               case 10:

                  // Windows 10 or Windows Server 2016

                  _enumOsName = verInfo.wProductType == NativeMethods.VER_NT_WORKSTATION
                     ? EnumOsName.Windows10
                     : EnumOsName.WindowsServer2016;

                  break;
                  

               #endregion // Version 10


               #region Version 6

               case 6:
                  switch (verInfo.dwMinorVersion)
                  {
                     // Windows 8.1 or Windows Server 2012 R2
                     case 3:
                        _enumOsName = verInfo.wProductType == NativeMethods.VER_NT_WORKSTATION
                           ? EnumOsName.Windows81
                           : EnumOsName.WindowsServer2012R2;
                        break;


                     // Windows 8 or Windows Server 2012
                     case 2:
                        _enumOsName = verInfo.wProductType == NativeMethods.VER_NT_WORKSTATION
                           ? EnumOsName.Windows8
                           : EnumOsName.WindowsServer2012;
                        break;


                     // Windows 7 or Windows Server 2008 R2
                     case 1:
                        _enumOsName = verInfo.wProductType == NativeMethods.VER_NT_WORKSTATION
                           ? EnumOsName.Windows7
                           : EnumOsName.WindowsServer2008R2;
                        break;


                     // Windows Vista or Windows Server 2008
                     case 0:
                        _enumOsName = verInfo.wProductType == NativeMethods.VER_NT_WORKSTATION
                           ? EnumOsName.WindowsVista
                           : EnumOsName.WindowsServer2008;
                        break;
                        

                     default:
                        _enumOsName = EnumOsName.Later;
                        break;
                  }

                  break;

               #endregion // Version 6


               #region Version 5

               case 5:
                  switch (verInfo.dwMinorVersion)
                  {
                     case 2:
                        _enumOsName = verInfo.wProductType == NativeMethods.VER_NT_WORKSTATION && _processorArchitecture == EnumProcessorArchitecture.X64
                           ? EnumOsName.WindowsXP
                           : verInfo.wProductType != NativeMethods.VER_NT_WORKSTATION ? EnumOsName.WindowsServer2003 : EnumOsName.Later;
                        break;


                     case 1:
                        _enumOsName = EnumOsName.WindowsXP;
                        break;


                     case 0:
                        _enumOsName = EnumOsName.Windows2000;
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
   }
}

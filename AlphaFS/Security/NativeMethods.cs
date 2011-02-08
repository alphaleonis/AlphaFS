/* Copyright (c) 2008-2009 Peter Palotas
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
using System.Runtime.InteropServices;
using System.Text;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Security
{
    internal static class NativeMethods
    {
        #region Types

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES_HDR
        {
            public UInt32 PrivilegeCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public UInt32 PrivilegeCount;
            public LUID Luid;
            public UInt32 Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public UInt32 Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public UInt32 LowPart;
            public UInt32 HighPart;
        }


        [Flags]
        public enum SECURITY_INFORMATION : uint
        {
            OWNER_SECURITY_INFORMATION = 0x00000001,
            GROUP_SECURITY_INFORMATION = 0x00000002,
            DACL_SECURITY_INFORMATION = 0x00000004,
            SACL_SECURITY_INFORMATION = 0x00000008,
            LABEL_SECURITY_INFORMATION = 0x00000010,
            UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,
            UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,
            PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,
            PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000
        }

        public enum SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE = 0,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT,
            SE_REGISTRY_WOW64_32KEY
        }

        public enum SECURITY_DESCRIPTOR_CONTROL : ushort
        {
            SE_OWNER_DEFAULTED = (0x0001),
            SE_GROUP_DEFAULTED = (0x0002),
            SE_DACL_PRESENT = (0x0004),
            SE_DACL_DEFAULTED = (0x0008),
            SE_SACL_PRESENT = (0x0010),
            SE_SACL_DEFAULTED = (0x0020),
            SE_DACL_AUTO_INHERIT_REQ = (0x0100),
            SE_SACL_AUTO_INHERIT_REQ = (0x0200),
            SE_DACL_AUTO_INHERITED = (0x0400),
            SE_SACL_AUTO_INHERITED = (0x0800),
            SE_DACL_PROTECTED = (0x1000),
            SE_SACL_PROTECTED = (0x2000),
            SE_RM_CONTROL_VALID = (0x4000),
            SE_SELF_RELATIVE = (0x8000)
        }

        #endregion

        #region Constants

        public const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;
        public const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        public const UInt32 SE_PRIVILEGE_REMOVED = 0x00000004;
        public const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;

        public const string SE_SECURITY_NAME = "SeSecurityPrivilege";

        #endregion

        #region DllImport Methods

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, UInt32 BufferLength, out TOKEN_PRIVILEGES PreviousState, out UInt32 ReturnLength);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        [DllImport("advapi32.dll")]
        static internal extern uint GetSecurityInfo(SafeHandle handle, SE_OBJECT_TYPE ObjectType, SECURITY_INFORMATION SecurityInfo, out IntPtr pSidOwner, out IntPtr pSidGroup, out IntPtr pDacl, out IntPtr pSacl, out SafeLocalMemoryBufferHandle pSecurityDescriptor);

        [DllImport("advapi32.dll", SetLastError = true)]
        static internal extern uint SetSecurityInfo(SafeHandle handle, SE_OBJECT_TYPE ObjectType, SECURITY_INFORMATION SecurityInfo, IntPtr psidOwner, IntPtr psidGroup, IntPtr pDacl, IntPtr pSacl);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetSecurityDescriptorDacl(SafeHandle pSecurityDescriptor, [MarshalAs(UnmanagedType.Bool)] out bool lpbDaclPresent, out IntPtr pDacl, [MarshalAs(UnmanagedType.Bool)] out bool lpbDaclDefaulted);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetSecurityDescriptorSacl(SafeHandle pSecurityDescriptor, [MarshalAs(UnmanagedType.Bool)] out bool lpbSaclPresent, out IntPtr pSacl, [MarshalAs(UnmanagedType.Bool)] out bool lpbSaclDefaulted);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetSecurityDescriptorGroup(SafeHandle pSecurityDescriptor, out IntPtr pGroup, [MarshalAs(UnmanagedType.Bool)] out bool lpbGroupDefaulted);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetSecurityDescriptorControl(SafeHandle pSecurityDescriptor, out SECURITY_DESCRIPTOR_CONTROL pControl, out UInt32 lpdwRevision);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetSecurityDescriptorOwner(SafeHandle pSecurityDescriptor, out IntPtr pOwner, [MarshalAs(UnmanagedType.Bool)] out bool lpbOwnerDefaulted);

        [DllImport("advapi32.dll")]
        static internal extern UInt32 GetSecurityDescriptorLength(SafeHandle pSecurityDescriptor);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool IsValidSecurityDescriptor(SafeHandle pSecurityDescriptor);

        [DllImport("kernel32.dll", SetLastError = true)]
        static internal extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool LookupPrivilegeDisplayName(string lpSystemName, string lpName, [Out] StringBuilder lpDisplayName, ref UInt32 cchDisplayName, out UInt32 lpLanguageId);

        #endregion

        #region Internal Utility Methods

        internal static UInt64 LuidToLong(LUID luid)
        {
            ulong high = (((ulong)luid.HighPart) << 32);
            ulong low = (((ulong)luid.LowPart) & 0x00000000FFFFFFFF);
            return high | low;
        }

        internal static LUID LongToLuid(UInt64 lluid)
        {
            LUID luid = new LUID();
            luid.HighPart = (UInt32)(lluid >> 32);
            luid.LowPart = (UInt32)(lluid & 0xFFFFFFFF);
            return luid;
        }

        internal static void SetSecurityInfo(SafeHandle handle, NativeMethods.SE_OBJECT_TYPE objectType, NativeObjectSecurity fileSecurity)
        {
            byte[] managedDescriptor = fileSecurity.GetSecurityDescriptorBinaryForm();
            using (SafeGlobalMemoryBufferHandle hDescriptor = new SafeGlobalMemoryBufferHandle(managedDescriptor.Length))
            {
                hDescriptor.CopyFrom(managedDescriptor, 0, managedDescriptor.Length);


                NativeMethods.SECURITY_DESCRIPTOR_CONTROL control;
                uint revision;
                if (!NativeMethods.GetSecurityDescriptorControl(hDescriptor, out control, out revision))
                    NativeError.ThrowException();

                IntPtr pDacl;
                bool daclDefaulted, daclPresent;
                if (!NativeMethods.GetSecurityDescriptorDacl(hDescriptor, out daclPresent, out pDacl, out daclDefaulted))
                    NativeError.ThrowException();

                IntPtr pSacl;
                bool saclDefaulted, saclPresent;
                if (!NativeMethods.GetSecurityDescriptorSacl(hDescriptor, out saclPresent, out pSacl, out saclDefaulted))
                    NativeError.ThrowException();

                IntPtr pOwner;
                bool ownerDefaulted;
                if (!NativeMethods.GetSecurityDescriptorOwner(hDescriptor, out pOwner, out ownerDefaulted))
                    NativeError.ThrowException();

                IntPtr pGroup;
                bool GroupDefaulted;
                if (!NativeMethods.GetSecurityDescriptorGroup(hDescriptor, out pGroup, out GroupDefaulted))
                    NativeError.ThrowException();

                PrivilegeEnabler privilegeEnabler = null;
                try
                {

                    NativeMethods.SECURITY_INFORMATION info = 0;

                    if (daclPresent)
                    {
                        info |= NativeMethods.SECURITY_INFORMATION.DACL_SECURITY_INFORMATION;

                        if ((control & NativeMethods.SECURITY_DESCRIPTOR_CONTROL.SE_DACL_PROTECTED) != 0)
                            info |= NativeMethods.SECURITY_INFORMATION.PROTECTED_DACL_SECURITY_INFORMATION;
                        else
                            info |= NativeMethods.SECURITY_INFORMATION.UNPROTECTED_DACL_SECURITY_INFORMATION;
                    }

                    if (saclPresent)
                    {
                        info |= NativeMethods.SECURITY_INFORMATION.SACL_SECURITY_INFORMATION;
                        if ((control & NativeMethods.SECURITY_DESCRIPTOR_CONTROL.SE_SACL_PROTECTED) != 0)
                            info |= NativeMethods.SECURITY_INFORMATION.PROTECTED_SACL_SECURITY_INFORMATION;
                        else
                            info |= NativeMethods.SECURITY_INFORMATION.UNPROTECTED_SACL_SECURITY_INFORMATION;

                        privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
                    }

                    if (pOwner != IntPtr.Zero)
                        info |= NativeMethods.SECURITY_INFORMATION.OWNER_SECURITY_INFORMATION;

                    if (pGroup != IntPtr.Zero)
                        info |= NativeMethods.SECURITY_INFORMATION.GROUP_SECURITY_INFORMATION;

                    uint errorCode = NativeMethods.SetSecurityInfo(handle, objectType, info, pOwner, pGroup, pDacl, pSacl);

                    if (errorCode != 0)
                        NativeError.ThrowException((int)errorCode);
                }
                finally
                {
                    if (privilegeEnabler != null)
                        privilegeEnabler.Dispose();
                }
            }
        }
        #endregion
    }
}

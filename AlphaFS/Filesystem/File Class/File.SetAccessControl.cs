using Alphaleonis.Win32.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region SetAccessControl

      /// <summary>
      ///   Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> FileSecurity object to the specified file.
      /// </summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A  <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/>
      ///   parameter.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity)
      {
         SetAccessControlInternal(path, null, fileSecurity, AccessControlSections.All, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.
      /// </summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections)
      {
         SetAccessControlInternal(path, null, fileSecurity, includeSections, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> FileSecurity object to the specified
      ///   file.
      /// </summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A  <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/>
      ///   parameter.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, PathFormat pathFormat)
      {
         SetAccessControlInternal(path, null, fileSecurity, AccessControlSections.All, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified
      ///   directory.
      /// </summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections, PathFormat pathFormat)
      {
         SetAccessControlInternal(path, null, fileSecurity, includeSections, pathFormat);
      }

      #endregion // SetAccessControl

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method SetAccessControlInternal() applies access control list (ACL) entries described by a
      ///   <see cref="FileSecurity"/> FileSecurity object to the specified file.
      /// </summary>
      /// <remarks>Use either <paramref name="path"/> or <paramref name="handle"/>, not both.</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">
      ///   A file to add or remove access control list (ACL) entries from. This parameter This parameter may be <see langword="null"/>.
      /// </param>
      /// <param name="handle">
      ///   A handle to add or remove access control list (ACL) entries from. This parameter This parameter may be <see langword="null"/>.
      /// </param>
      /// <param name="objectSecurity">
      ///   A <see cref="DirectorySecurity"/> or <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described
      ///   by the <paramref name="path"/> parameter.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static void SetAccessControlInternal(string path, SafeHandle handle, ObjectSecurity objectSecurity, AccessControlSections includeSections, PathFormat pathFormat)
      {
         if (pathFormat == PathFormat.RelativePath)
            Path.CheckValidPath(path, true, true);

         if (objectSecurity == null)
            throw new ArgumentNullException("objectSecurity");

         byte[] managedDescriptor = objectSecurity.GetSecurityDescriptorBinaryForm();
         using (var safeBuffer = new SafeGlobalMemoryBufferHandle(managedDescriptor.Length))
         {
            string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars);

            safeBuffer.CopyFrom(managedDescriptor, 0, managedDescriptor.Length);

            SecurityDescriptorControl control;
            uint revision;
            if (!Security.NativeMethods.GetSecurityDescriptorControl(safeBuffer, out control, out revision))
               NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

            PrivilegeEnabler privilegeEnabler = null;
            try
            {
               var securityInfo = SecurityInformation.None;

               IntPtr pDacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Access) != 0)
               {
                  bool daclDefaulted, daclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorDacl(safeBuffer, out daclPresent, out pDacl, out daclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (daclPresent)
                  {
                     securityInfo |= SecurityInformation.Dacl;
                     securityInfo |= (control & SecurityDescriptorControl.DaclProtected) != 0
                        ? SecurityInformation.ProtectedDacl
                        : SecurityInformation.UnprotectedDacl;
                  }
               }

               IntPtr pSacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Audit) != 0)
               {
                  bool saclDefaulted, saclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorSacl(safeBuffer, out saclPresent, out pSacl, out saclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (saclPresent)
                  {
                     securityInfo |= SecurityInformation.Sacl;
                     securityInfo |= (control & SecurityDescriptorControl.SaclProtected) != 0
                        ? SecurityInformation.ProtectedSacl
                        : SecurityInformation.UnprotectedSacl;

                     privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
                  }
               }

               IntPtr pOwner = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Owner) != 0)
               {
                  bool ownerDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorOwner(safeBuffer, out pOwner, out ownerDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (pOwner != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Owner;
               }

               IntPtr pGroup = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Group) != 0)
               {
                  bool groupDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorGroup(safeBuffer, out pGroup, out groupDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (pGroup != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Group;
               }


               uint lastError;
               if (!Utils.IsNullOrWhiteSpace(pathLp))
               {
                  // SetNamedSecurityInfo()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

                  lastError = Security.NativeMethods.SetNamedSecurityInfo(pathLp, ObjectType.FileObject, securityInfo, pOwner, pGroup, pDacl, pSacl);
                  if (lastError != Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException(lastError, pathLp);
               }
               else if (NativeMethods.IsValidHandle(handle))
               {
                  lastError = Security.NativeMethods.SetSecurityInfo(handle, ObjectType.FileObject, securityInfo, pOwner, pGroup, pDacl, pSacl);
                  if (lastError != Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException((int)lastError);
               }
            }
            finally
            {
               if (privilegeEnabler != null)
                  privilegeEnabler.Dispose();
            }
         }
      }

      #endregion // SetAccessControlInternal
   }
}

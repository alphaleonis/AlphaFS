/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   partial class File
   {
      /// <summary>Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>      
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path)
      {
         return GetAccessControlCore<FileSecurity>(false, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.RelativePath);
      }

      /// <summary>Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>      
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         return GetAccessControlCore<FileSecurity>(false, path, includeSections, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>      
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, PathFormat pathFormat)
      {
         return GetAccessControlCore<FileSecurity>(false, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, pathFormat);
      }

      /// <summary>[AlphaFS] Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections, PathFormat pathFormat)
      {
         return GetAccessControlCore<FileSecurity>(false, path, includeSections, pathFormat);
      }


      /// <summary>[AlphaFS] Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file handle.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="handle"/> parameter.</returns>      
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="handle">A <see cref="SafeHandle"/> to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(SafeFileHandle handle)
      {
         return GetAccessControlHandleCore<FileSecurity>(false, false, handle, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, SecurityInformation.None);
      }

      /// <summary>[AlphaFS] Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file handle.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="handle"/> parameter.</returns>      
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="handle">A <see cref="SafeHandle"/> to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(SafeFileHandle handle, AccessControlSections includeSections)
      {
         return GetAccessControlHandleCore<FileSecurity>(false, false, handle, includeSections, SecurityInformation.None);
      }




      /// <summary>[AlphaFS] Gets an <see cref="ObjectSecurity"/> object for a particular file or directory.</summary>
      /// <returns>An <see cref="ObjectSecurity"/> object that encapsulates the access control rules for the file or directory described by the <paramref name="path"/> parameter.</returns>
      /// <exception cref="IOException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <typeparam name="T">Generic type parameter.</typeparam>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="path">The path to a file/directory containing a <see cref="FileSecurity"/>/<see cref="DirectorySecurity"/> object that describes the file's/directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing is controlled.")]
      [SecurityCritical]
      internal static T GetAccessControlCore<T>(bool isFolder, string path, AccessControlSections includeSections, PathFormat pathFormat)
      {
         SecurityInformation securityInfo = CreateSecurityInformation(includeSections);


         // We need the SE_SECURITY_NAME privilege enabled to be able to get the SACL descriptor.
         // So we enable it here for the remainder of this function.

         PrivilegeEnabler privilege = null;

         if ((includeSections & AccessControlSections.Audit) != 0)
            privilege = new PrivilegeEnabler(Privilege.Security);

         using (privilege)
         {
            IntPtr pSidOwner, pSidGroup, pDacl, pSacl;
            SafeGlobalMemoryBufferHandle pSecurityDescriptor;

            string pathLp = Path.GetExtendedLengthPathCore(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);


            // Get/SetNamedSecurityInfo does not work with a handle but with a path, hence does not honor the privileges.
            // It magically does since Windows Server 2012 / 8 but not in previous OS versions.

            uint lastError = Security.NativeMethods.GetNamedSecurityInfo(pathLp, ObjectType.FileObject, securityInfo,
               out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSecurityDescriptor);


            // When GetNamedSecurityInfo() fails with ACCESS_DENIED, try again using GetSecurityInfo().

            if (lastError == Win32Errors.ERROR_ACCESS_DENIED)
               using (SafeFileHandle handle = CreateFileCore(null, pathLp, ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, FileSystemRights.Read, FileShare.Read, false, PathFormat.LongFullPath))
                  return GetAccessControlHandleCore<T>(true, isFolder, handle, includeSections, securityInfo);

            return GetSecurityDescriptor<T>(lastError, isFolder, pathLp, pSecurityDescriptor);
         }
      }


      internal static T GetAccessControlHandleCore<T>(bool internalCall, bool isFolder, SafeFileHandle handle, AccessControlSections includeSections, SecurityInformation securityInfo)
      {
         if (!internalCall)
            securityInfo = CreateSecurityInformation(includeSections);


         // We need the SE_SECURITY_NAME privilege enabled to be able to get the SACL descriptor.
         // So we enable it here for the remainder of this function.
         
         PrivilegeEnabler privilege = null;

         if (!internalCall && (includeSections & AccessControlSections.Audit) != 0)
            privilege = new PrivilegeEnabler(Privilege.Security);
         
         using (privilege)
         {
            IntPtr pSidOwner, pSidGroup, pDacl, pSacl;
            SafeGlobalMemoryBufferHandle pSecurityDescriptor;

            uint lastError = Security.NativeMethods.GetSecurityInfo(handle, ObjectType.FileObject, securityInfo,
               out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSecurityDescriptor);

            return GetSecurityDescriptor<T>(lastError, isFolder, null, pSecurityDescriptor);
         }
      }


      private static SecurityInformation CreateSecurityInformation(AccessControlSections includeSections)
      {
         var securityInfo = SecurityInformation.None;


         if ((includeSections & AccessControlSections.Access) != 0)
            securityInfo |= SecurityInformation.Dacl;

         if ((includeSections & AccessControlSections.Audit) != 0)
            securityInfo |= SecurityInformation.Sacl;

         if ((includeSections & AccessControlSections.Group) != 0)
            securityInfo |= SecurityInformation.Group;

         if ((includeSections & AccessControlSections.Owner) != 0)
            securityInfo |= SecurityInformation.Owner;
         

         return securityInfo;
      }


      private static T GetSecurityDescriptor<T>(uint lastError, bool isFolder, string path, SafeGlobalMemoryBufferHandle securityDescriptor)
      {
         ObjectSecurity objectSecurity;

         using (securityDescriptor)
         {
            if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND || lastError == Win32Errors.ERROR_PATH_NOT_FOUND)
               lastError = isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND;


            // If the function fails, the return value is zero.
            if (lastError != Win32Errors.ERROR_SUCCESS)
            {
               if (!Utils.IsNullOrWhiteSpace(path))
                  NativeError.ThrowException(lastError, path);
               else
                  NativeError.ThrowException((int) lastError);
            }

            if (!NativeMethods.IsValidHandle(securityDescriptor, false))
               throw new IOException(Resources.Returned_Invalid_Security_Descriptor);


            uint length = Security.NativeMethods.GetSecurityDescriptorLength(securityDescriptor);

            // Seems not to work: Method .CopyTo: length > Capacity, so an Exception is thrown.
            //byte[] managedBuffer = new byte[length];
            //pSecurityDescriptor.CopyTo(managedBuffer, 0, (int) length);

            byte[] managedBuffer = securityDescriptor.ToByteArray(0, (int) length);

            objectSecurity = isFolder ? (ObjectSecurity) new DirectorySecurity() : new FileSecurity();
            objectSecurity.SetSecurityDescriptorBinaryForm(managedBuffer);
         }

         return (T) (object) objectSecurity;
      }
   }
}

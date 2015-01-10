using Alphaleonis.Win32.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Public Methods

      /// <summary>
      ///   Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL)
      ///   entries for a specified file.
      /// </summary>
      /// <param name="path">
      ///   The path to a file containing a <see cref="FileSecurity"/> object that describes the file's
      ///   access control list (ACL) information.
      /// </param>
      /// <returns>
      ///   A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file
      ///   described by the <paramref name="path"/> parameter.
      /// </returns>      
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL)
      ///   entries for a specified file.
      /// </summary>
      /// <param name="path">
      ///   The path to a file containing a <see cref="FileSecurity"/> object that describes the file's
      ///   access control list (ACL) information.
      /// </param>
      /// <param name="includeSections">
      ///   One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of
      ///   access control list (ACL) information to receive.
      /// </param>
      /// <returns>
      ///   A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file
      ///   described by the <paramref name="path"/> parameter.
      /// </returns>      
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, includeSections, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Gets a <see cref="FileSecurity"/> object that encapsulates the access control
      ///   list (ACL) entries for a specified file.
      /// </summary>
      /// <param name="path">
      ///   The path to a file containing a <see cref="FileSecurity"/> object that describes the file's
      ///   access control list (ACL) information.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file
      ///   described by the <paramref name="path"/> parameter.
      /// </returns>      
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, PathFormat pathFormat)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.
      /// </summary>
      /// <param name="path">
      ///   The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.
      /// </param>
      /// <param name="includeSections">
      ///   One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   receive.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the
      ///   <paramref name="path"/> parameter.
      /// </returns>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections, PathFormat pathFormat)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, includeSections, pathFormat);
      }

      #endregion

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method GetAccessControlInternal() to get an <see cref="ObjectSecurity"/> object for a particular file or
      ///   directory.
      /// </summary>
      /// <exception cref="IOException">Thrown when an IO failure occurred.</exception>
      /// <typeparam name="T">Generic type parameter.</typeparam>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="path">
      ///   The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's or file's access control
      ///   list (ACL) information.
      /// </param>
      /// <param name="includeSections">
      ///   One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   receive.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An <see cref="ObjectSecurity"/> object that encapsulates the access control rules for the file or directory described by the
      ///   <paramref name="path"/> parameter.
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException"/>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static T GetAccessControlInternal<T>(bool isFolder, string path, AccessControlSections includeSections, PathFormat pathFormat)
      {
         SecurityInformation securityInfo = 0;
         PrivilegeEnabler privilegeEnabler = null;

         if ((includeSections & AccessControlSections.Access) != 0)
            securityInfo |= SecurityInformation.Dacl;

         if ((includeSections & AccessControlSections.Group) != 0)
            securityInfo |= SecurityInformation.Group;

         if ((includeSections & AccessControlSections.Owner) != 0)
            securityInfo |= SecurityInformation.Owner;

         if ((includeSections & AccessControlSections.Audit) != 0)
         {
            // We need the SE_SECURITY_NAME privilege enabled to be able to get the
            // SACL descriptor. So we enable it here for the remainder of this function.
            privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
            securityInfo |= SecurityInformation.Sacl;
         }

         using (privilegeEnabler)
         {
            string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

            IntPtr pSidOwner, pSidGroup, pDacl, pSacl;
            SafeGlobalMemoryBufferHandle pSecurityDescriptor;
            ObjectSecurity objectSecurity;

            uint lastError = Security.NativeMethods.GetNamedSecurityInfo(pathLp, ObjectType.FileObject, securityInfo, out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSecurityDescriptor);

            try
            {
               if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND || lastError == Win32Errors.ERROR_PATH_NOT_FOUND)
                  lastError = (isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND);

               // If the function fails, the return value is zero.
               if (lastError != Win32Errors.ERROR_SUCCESS)
                  NativeError.ThrowException(lastError, pathLp);

               if (!NativeMethods.IsValidHandle(pSecurityDescriptor, false))
                  throw new IOException(Resources.InvalidSecurityDescriptorReturnedFromSystem);


               uint length = Security.NativeMethods.GetSecurityDescriptorLength(pSecurityDescriptor);

               // Seems not to work: Method .CopyTo: length > Capacity, so an Exception is thrown.
               //byte[] managedBuffer = new byte[length];
               //pSecurityDescriptor.CopyTo(managedBuffer, 0, (int) length);

               byte[] managedBuffer = pSecurityDescriptor.ToByteArray(0, (int)length);

               objectSecurity = (isFolder) ? (ObjectSecurity)new DirectorySecurity() : new FileSecurity();
               objectSecurity.SetSecurityDescriptorBinaryForm(managedBuffer);
            }
            finally
            {
               if (pSecurityDescriptor != null)
                  pSecurityDescriptor.Close();
            }

            return (T)(object)objectSecurity;
         }
      }

      #endregion // GetAccessControlInternal
   }
}

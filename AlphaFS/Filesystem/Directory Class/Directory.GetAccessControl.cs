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
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region GetAccessControl

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the specified directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.RelativePath);
      }

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory described by the <paramref name="path"/> parameter. </returns>
      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, includeSections, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the specified directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, PathFormat pathFormat)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, pathFormat);
      }

      /// <summary>[AlphaFS] Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory described by the <paramref name="path"/> parameter. </returns>
      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections, PathFormat pathFormat)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, includeSections, pathFormat);
      }

      #endregion

      #region HasInheritedPermissions

      /// <summary>[AlphaFS] Check if the directory has permission inheritance enabled.</summary>
      /// <returns><see langword="true"/> if permission inheritance is enabled, <see langword="false"/> if permission inheritance is disabled.</returns>
      /// <param name="path">The full path to the directory to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      public static bool HasInheritedPermissions(string path, PathFormat pathFormat)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         //DirectorySecurity acl = GetAccessControl(directoryPath);
         DirectorySecurity acl = File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, pathFormat);

         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }


      /// <summary>[AlphaFS] Check if the directory has permission inheritance enabled.</summary>
      /// <param name="path">The full path to the directory to check.</param>
      /// <returns><see langword="true"/> if permission inheritance is enabled, <see langword="false"/> if permission inheritance is disabled.</returns>
      public static bool HasInheritedPermissions(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         DirectorySecurity acl = File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.RelativePath);

         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #endregion // HasInheritedPermissions
   }
}

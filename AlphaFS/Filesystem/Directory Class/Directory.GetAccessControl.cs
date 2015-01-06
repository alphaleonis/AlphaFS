using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the specified directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.Relative);
      }

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular directory.</summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory described by the <paramref name="path"/> parameter. </returns>
      [SecurityCritical]
      public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, path, includeSections, PathFormat.Relative);
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
   }
}

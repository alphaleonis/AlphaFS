using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, AccessControlSections.All, PathFormat.Relative);
      }

      /// <summary>Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <remarks>Note that unlike <see cref="System.IO.File.SetAccessControl"/> this method does <b>not</b> automatically
      /// determine what parts of the specified <see cref="DirectorySecurity"/> instance has been modified. Instead, the
      /// parameter <paramref name="includeSections"/> is used to specify what entries from <paramref name="directorySecurity"/> to apply to <paramref name="path"/>.</remarks>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, AccessControlSections includeSections)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, includeSections, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, PathFormat pathFormat)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, AccessControlSections.All, pathFormat);
      }

      /// <summary>[AlphaFS] Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>Note that unlike <see cref="System.IO.File.SetAccessControl"/> this method does <b>not</b> automatically
      /// determine what parts of the specified <see cref="DirectorySecurity"/> instance has been modified. Instead, the
      /// parameter <paramref name="includeSections"/> is used to specify what entries from <paramref name="directorySecurity"/> to apply to <paramref name="path"/>.</remarks>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, AccessControlSections includeSections, PathFormat pathFormat)
      {
         File.SetAccessControlInternal(path, null, directorySecurity, includeSections, pathFormat);
      }
   }
}

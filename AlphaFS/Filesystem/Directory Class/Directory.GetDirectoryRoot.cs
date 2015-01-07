using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region GetDirectoryRoot

      /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
      /// <param name="path">The path of a file or directory.</param>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      [SecurityCritical]
      public static string GetDirectoryRoot(string path)
      {
         return GetDirectoryRootInternal(null, path, PathFormat.Relative);
      }

      /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static string GetDirectoryRoot(string path, PathFormat pathFormat)
      {
         return GetDirectoryRootInternal(null, path, pathFormat);
      }

      /// <summary>Returns the volume information, root information, or both for the specified path.</summary>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static string GetDirectoryRoot(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetDirectoryRootInternal(transaction, path, pathFormat);
      }

      /// <summary>[AlphaFS] Returns the volume information, root information, or both for the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path of a file or directory.</param>
      /// <returns>The volume information, root information, or both for the specified path, or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</returns>
      [SecurityCritical]
      public static string GetDirectoryRoot(KernelTransaction transaction, string path)
      {
         return GetDirectoryRootInternal(transaction, path, PathFormat.Relative);
      }

      #endregion // GetDirectoryRoot

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method GetDirectoryRootInternal() to return the volume information, root information, or both for the specified path.
      /// <returns>
      /// <para>Returns the volume information, root information, or both for the specified path,</para>
      /// <para> or <see langword="null"/> if <paramref name="path"/> path does not contain root directory information.</para>
      /// </returns>
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static string GetDirectoryRootInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.CheckInvalidPathChars);
         pathLp = Path.GetRegularPathInternal(pathLp, false, false, false, false);
         string rootPath = Path.GetPathRoot(pathLp, false);

         return Utils.IsNullOrWhiteSpace(rootPath) ? null : rootPath;
      }

      #endregion // GetDirectoryRootInternal
   }
}

using System;
using System.Globalization;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region AddDirectorySeparator

      /// <summary>
      ///   [AlphaFS] Adds a <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character to the string.
      /// </summary>
      /// <param name="path">
      ///   A text string to which the <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> is to be added.
      /// </param>
      /// <returns>
      ///   A text string with the <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character suffixed. The
      ///   function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.
      /// </returns>
      [SecurityCritical]
      public static string AddDirectorySeparator(string path)
      {
         return AddDirectorySeparator(path, false);
      }

      /// <summary>
      ///   [AlphaFS] Adds a <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character to the string.
      /// </summary>
      /// <param name="path">
      ///   A text string to which the <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> is to be added.
      /// </param>
      /// <param name="addAlternateSeparator">
      ///   if <see langword="true"/> the <see cref="AltDirectorySeparatorChar"/> character will be added instead.
      /// </param>
      /// <returns>
      ///   A text string with the <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character suffixed. The
      ///   function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.
      /// </returns>
      [SecurityCritical]
      public static string AddDirectorySeparator(string path, bool addAlternateSeparator)
      {
         if (path == null)
            return null;

         return addAlternateSeparator
            ? ((!path.EndsWith(AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
               ? path + AltDirectorySeparatorChar : path)
            : ((!path.EndsWith(DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
               ? path + DirectorySeparatorChar : path);
      }

      #endregion // AddDirectorySeparator

      #region RemoveDirectorySeparator

      /// <summary>[AlphaFS] Removes the <see cref="DirectorySeparatorChar"/> character from the string.</summary>
      /// <param name="path">A text string from which the <see cref="DirectorySeparatorChar"/> is to be removed.</param>
      /// <returns>
      ///   <para>Returns A text string where the suffixed <see cref="DirectorySeparatorChar"/> has been removed.</para>
      ///   <para>The function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.</para>
      /// </returns>
      [SecurityCritical]
      public static string RemoveDirectorySeparator(string path)
      {
         return path == null ? null : path.TrimEnd(DirectorySeparatorChar, AltDirectorySeparatorChar);
      }

      /// <summary>
      ///   [AlphaFS] Removes the <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character from the string.
      /// </summary>
      /// <param name="path">
      ///   A text string from which the <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> is to be removed.
      /// </param>
      /// <param name="removeAlternateSeparator">
      ///   If <see langword="true"/> the <see cref="AltDirectorySeparatorChar"/> character will be removed instead.
      /// </param>
      /// <returns>
      ///   A text string where the suffixed <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character has been
      ///   removed. The function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.
      /// </returns>
      [SecurityCritical]
      public static string RemoveDirectorySeparator(string path, bool removeAlternateSeparator)
      {
         return path == null
            ? null
            : path.TrimEnd(removeAlternateSeparator ? AltDirectorySeparatorChar : DirectorySeparatorChar);
      }

      #endregion // RemoveDirectorySeparator

      #region GetSuffixedDirectoryName

      /// <summary>
      ///   [AlphaFS] Returns the directory information for the specified <paramref name="path"/> with a trailing directory separator.
      /// </summary>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or <see langword="null"/> if
      ///   <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").
      /// </returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryName(string path)
      {
         return GetSuffixedDirectoryNameInternal(null, path);
      }

      /// <summary>
      ///   [AlphaFS] Returns the directory information for the specified <paramref name="path"/> with a trailing directory separator.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or <see langword="null"/> if
      ///   <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").
      /// </returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryName(KernelTransaction transaction, string path)
      {
         return GetSuffixedDirectoryNameInternal(transaction, path);
      }


      #endregion // GetSuffixedDirectoryName

      #region GetSuffixedDirectoryNameWithoutRoot

      /// <summary>
      ///   [AlphaFS] Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing
      ///   directory separator.
      /// </summary>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator, or
      ///   <see langword="null"/> if <paramref name="path"/> is <see langword="null"/>.
      /// </returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameWithoutRoot(string path)
      {
         return GetSuffixedDirectoryNameWithoutRootInternal(null, path);
      }

      /// <summary>
      ///   [AlphaFS] Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing
      ///   directory separator.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator, or
      ///   <see langword="null"/> if <paramref name="path"/> is <see langword="null"/>.
      /// </returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameWithoutRoot(KernelTransaction transaction, string path)
      {
         return GetSuffixedDirectoryNameWithoutRootInternal(transaction, path);
      }


      #endregion // GetSuffixedDirectoryNameWithoutRoot

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method GetSuffixedDirectoryNameInternal() to return the directory information for the specified
      ///   <paramref name="path"/> with a trailing directory separator.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or
      ///   <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\
      ///   ", "C:", or * "\\server\share").
      /// </returns>
      [SecurityCritical]
      private static string GetSuffixedDirectoryNameInternal(KernelTransaction transaction, string path)
      {
         DirectoryInfo di = Directory.GetParentInternal(transaction, path, PathFormat.Relative);
         return di != null && di.Parent != null && di.Name != null
            ? AddDirectorySeparator(CombineInternal(false, di.Parent.FullName, di.Name), false)
            : null;
      }

      /// <summary>
      ///   [AlphaFS] Unified method GetSuffixedDirectoryNameWithoutRootInternal() to return the directory information for the specified
      ///   <paramref name="path"/> with a trailing directory separator.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or
      ///   <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\
      ///   ", "C:", or * "\\server\share").
      /// </returns>
      [SecurityCritical]
      private static string GetSuffixedDirectoryNameWithoutRootInternal(KernelTransaction transaction, string path)
      {
         DirectoryInfo di = Directory.GetParentInternal(transaction, path, PathFormat.Relative);
         if (di == null || di.Parent == null)
            return null;

         DirectoryInfo tmp = di;
         string suffixedDirectoryNameWithoutRoot;

         do
         {
            suffixedDirectoryNameWithoutRoot = tmp.DisplayPath.Replace(di.Root.ToString(), string.Empty);

            if (tmp.Parent != null)
               tmp = di.Parent.Parent;

         } while (tmp != null && tmp.Root.Parent != null && tmp.Parent != null && !Utils.IsNullOrWhiteSpace(tmp.Parent.ToString()));

         return Utils.IsNullOrWhiteSpace(suffixedDirectoryNameWithoutRoot)
            ? null
            : AddDirectorySeparator(suffixedDirectoryNameWithoutRoot.TrimStart(DirectorySeparatorChar), false);
         // TrimStart() for network-drive, like: C$
      }

      #endregion
   }
}

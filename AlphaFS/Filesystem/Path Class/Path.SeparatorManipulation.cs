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
using System.Globalization;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region AddTrailingDirectorySeparator

      /// <summary>[AlphaFS] Adds a trailing <see cref="DirectorySeparatorChar"/> character to the string, when absent.</summary>
      /// <returns>A text string with a trailing <see cref="DirectorySeparatorChar"/> character. The function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.</returns>
      /// <param name="path">A text string to which the trailing <see cref="DirectorySeparatorChar"/> is to be added, when absent.</param>
      [SecurityCritical]
      public static string AddTrailingDirectorySeparator(string path)
      {
         return AddTrailingDirectorySeparator(path, false);
      }

      /// <summary>[AlphaFS] Adds a trailing <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character to the string, when absent.</summary>
      /// <returns>A text string with a trailing <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character. The function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.</returns>
      /// <param name="path">A text string to which the trailing <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> is to be added, when absent.</param>
      /// <param name="addAlternateSeparator">If <see langword="true"/> the <see cref="AltDirectorySeparatorChar"/> character will be added instead.</param>
      [SecurityCritical]
      public static string AddTrailingDirectorySeparator(string path, bool addAlternateSeparator)
      {
         return path == null
            ? null
            : (addAlternateSeparator
               ? ((!path.EndsWith(AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
                  ? path + AltDirectorySeparatorChar
                  : path)

               : ((!path.EndsWith(DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
                  ? path + DirectorySeparatorChar
                  : path));
      }

      #endregion // AddTrailingDirectorySeparator

      #region RemoveTrailingDirectorySeparator

      /// <summary>[AlphaFS] Removes the trailing <see cref="DirectorySeparatorChar"/> character from the string, when present.</summary>
      /// <returns>A text string where the trailing <see cref="DirectorySeparatorChar"/> character has been removed. The function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.</returns>
      /// <param name="path">A text string from which the trailing <see cref="DirectorySeparatorChar"/> is to be removed, when present.</param>
      [SecurityCritical]
      public static string RemoveTrailingDirectorySeparator(string path)
      {
         return path == null ? null : path.TrimEnd(DirectorySeparatorChar, AltDirectorySeparatorChar);
      }

      /// <summary>[AlphaFS] Removes the trailing <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character from the string, when present.</summary>
      /// <returns>A text string where the trailing <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> character has been removed. The function returns <see langword="null"/> when <paramref name="path"/> is <see langword="null"/>.</returns>
      /// <param name="path">A text string from which the trailing <see cref="DirectorySeparatorChar"/> or <see cref="AltDirectorySeparatorChar"/> is to be removed, when present.</param>
      /// <param name="removeAlternateSeparator">If <see langword="true"/> the trailing <see cref="AltDirectorySeparatorChar"/> character will be removed instead.</param>
      [SecurityCritical]
      public static string RemoveTrailingDirectorySeparator(string path, bool removeAlternateSeparator)
      {
         return path == null
            ? null
            : path.TrimEnd(removeAlternateSeparator ? AltDirectorySeparatorChar : DirectorySeparatorChar);
      }

      #endregion // RemoveTrailingDirectorySeparator

      #region GetSuffixedDirectoryName

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> with a trailing <see cref="DirectorySeparatorChar"/> character.</summary>
      /// <returns>
      ///   <para>The suffixed directory information for the specified <paramref name="path"/> with a trailing <see cref="DirectorySeparatorChar"/> character,</para>
      ///   <para>or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</para>
      /// </returns>
      /// <remarks>This method is similar to calling Path.GetDirectoryName() + Path.AddTrailingDirectorySeparator()</remarks>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetSuffixedDirectoryName(string path)
      {
         return GetSuffixedDirectoryNameCore(null, path);
      }

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> with a trailing <see cref="DirectorySeparatorChar"/> character.</summary>
      /// <returns>
      ///   <para>The suffixed directory information for the specified <paramref name="path"/> with a trailing <see cref="DirectorySeparatorChar"/> character,</para>
      ///   <para>or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</para>
      /// </returns>
      /// <remarks>This method is similar to calling Path.GetDirectoryName() + Path.AddTrailingDirectorySeparator()</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameTransacted(KernelTransaction transaction, string path)
      {
         return GetSuffixedDirectoryNameCore(transaction, path);
      }

      #endregion // GetSuffixedDirectoryName

      #region GetSuffixedDirectoryNameWithoutRoot

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing <see cref="DirectorySeparatorChar"/> character.</summary>
      /// <returns>
      ///   <para>The directory information for the specified <paramref name="path"/> without the root and with a trailing <see cref="DirectorySeparatorChar"/> character,</para>
      ///   <para>or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> is <see langword="null"/>.</para>
      /// </returns>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameWithoutRoot(string path)
      {
         return GetSuffixedDirectoryNameWithoutRootCore(null, path);
      }

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing <see cref="DirectorySeparatorChar"/> character.</summary>
      /// <returns>
      ///   <para>The directory information for the specified <paramref name="path"/> without the root and with a trailing <see cref="DirectorySeparatorChar"/> character,</para>
      ///   <para>or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> is <see langword="null"/>.</para>
      /// </returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameWithoutRootTransacted(KernelTransaction transaction, string path)
      {
         return GetSuffixedDirectoryNameWithoutRootCore(transaction, path);
      }
      
      #endregion // GetSuffixedDirectoryNameWithoutRoot

      #region Internal Methods

      /// <summary>Returns the directory information for the specified <paramref name="path"/> with a trailing <see cref="DirectorySeparatorChar"/> character.</summary>
      /// <returns>
      ///   <para>The suffixed directory information for the specified <paramref name="path"/> with a trailing <see cref="DirectorySeparatorChar"/> character,</para>
      ///   <para>or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</para>
      /// </returns>
      /// <remarks>This method is similar to calling Path.GetDirectoryName() + Path.AddTrailingDirectorySeparator()</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      private static string GetSuffixedDirectoryNameCore(KernelTransaction transaction, string path)
      {
         DirectoryInfo di = Directory.GetParentCore(transaction, path, PathFormat.RelativePath);

         return di != null && di.Parent != null && di.Name != null
            ? AddTrailingDirectorySeparator(CombineCore(false, di.Parent.FullName, di.Name), false)
            : null;
      }

      /// <summary>Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing <see cref="DirectorySeparatorChar"/> character.</summary>
      /// <returns>
      ///   <para>The directory information for the specified <paramref name="path"/> without the root and with a trailing <see cref="DirectorySeparatorChar"/> character,</para>
      ///   <para>or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> is <see langword="null"/>.</para>
      /// </returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      private static string GetSuffixedDirectoryNameWithoutRootCore(KernelTransaction transaction, string path)
      {
         DirectoryInfo di = Directory.GetParentCore(transaction, path, PathFormat.RelativePath);

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
            : AddTrailingDirectorySeparator(suffixedDirectoryNameWithoutRoot.TrimStart(DirectorySeparatorChar), false);
         // TrimStart() for network-drive, like: C$
      }

      #endregion // Internal Methods
   }
}

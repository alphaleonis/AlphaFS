/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region Non-Transactional

      /// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      [SecurityCritical]
      public static DirectoryInfo GetParent(string path)
      {
         return GetParentInternal(null, path, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DirectoryInfo GetParent(string path, PathFormat pathFormat)
      {
         return GetParentInternal(null, path, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      [SecurityCritical]
      public static DirectoryInfo GetParent(KernelTransaction transaction, string path)
      {
         return GetParentInternal(transaction, path, PathFormat.Relative);
      }

      /// <summary>Retrieves the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <returns>The parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DirectoryInfo GetParent(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetParentInternal(transaction, path, pathFormat);
      }

      #endregion // Transactional

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method GetParent() to retrieve the parent directory of the specified path, including both absolute and relative paths.</summary>
      /// <returns>Returns the parent directory, or <see langword="null"/> if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static DirectoryInfo GetParentInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.CheckInvalidPathChars);

         pathLp = Path.GetRegularPathInternal(pathLp, false, false, false, false);
         string dirName = Path.GetDirectoryName(pathLp, false);

         return Utils.IsNullOrWhiteSpace(dirName) ? null : new DirectoryInfo(transaction, dirName, PathFormat.Relative);
      }

      #endregion // Internal Methods
   }
}

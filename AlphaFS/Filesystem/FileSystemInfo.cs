/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides the base class for both FileInfo and DirectoryInfo objects.</summary>
   [SerializableAttribute]
   [ComVisibleAttribute(true)]
   public abstract class FileSystemInfo : MarshalByRefObject
   {
      #region Methods

      #region .NET

      #region Delete

      /// <summary>Deletes a file or directory.</summary>
      [SecurityCritical]
      public abstract void Delete();

      #endregion // Delete

      #region Refresh

      /// <summary>Refreshes the state of the object.</summary>
      /// <remarks>
      /// FileSystemInfo.Refresh() takes a snapshot of the file from the current file system.
      /// Refresh cannot correct the underlying file system even if the file system returns incorrect or outdated information.
      /// This can happen on platforms such as Windows 98.
      /// Calls must be made to Refresh() before attempting to get the attribute information, or the information will be outdated.
      /// </remarks>
      [SecurityCritical]
      protected void Refresh()
      {
         EntryInfo = GetFileSystemEntryInfoInternal(IsDirectory, Transaction, LongFullPath, true, true, true);
         _exists = _systemInfo != null;
      }

      #endregion // Refresh

      #region ToString

      /// <summary>Returns a string that represents the current object.</summary>
      /// <remarks>ToString is the major formatting method in the .NET Framework. It converts an object to its string representation so that it is suitable for display.</remarks>
      public override string ToString()
      {
         // "Alphaleonis.Win32.Filesystem.FileSystemInfo"
         return GetType().ToString();
      }

      #endregion // ToString

      #region Equality

      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; <c>false</c> otherwise.</returns>
      public override bool Equals(object obj)
      {
         if (obj == null || GetType() != obj.GetType())
            return false;

         FileSystemInfo other = obj as FileSystemInfo;

         return other != null && (other.Name != null &&
                                  (other.FullName.Equals(FullName, StringComparison.OrdinalIgnoreCase) &&
                                   other.Attributes.Equals(Attributes) &&
                                   other.CreationTimeUtc.Equals(CreationTimeUtc) &&
                                   other.LastWriteTimeUtc.Equals(LastWriteTimeUtc)));
      }

      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            int hash = !Utils.IsNullOrWhiteSpace(FullName) ? FullName.GetHashCode() : 17;

            if (!Utils.IsNullOrWhiteSpace(Name))
               hash = hash * 23 + Name.GetHashCode();

            hash = hash * 23 + Attributes.GetHashCode();
            hash = hash * 23 + CreationTimeUtc.GetHashCode();
            hash = hash * 23 + LastWriteTimeUtc.GetHashCode();

            return hash;
         }
      }
      
      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(FileSystemInfo left, FileSystemInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) ||
                !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }
      
      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(FileSystemInfo left, FileSystemInfo right)
      {
         return !(left == right);
      }

      #endregion  // Equality

      #endregion // .NET

      #region AlphaFS

      #region EnumerateFileSystemEntryInfos

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")][
      SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, bool continueOnException, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, bool continueOnException, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, SearchOption searchOption, bool continueOnException, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, searchOption, null, null, false, continueOnException, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, SearchOption searchOption)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, searchOption, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of <see cref="T:FileSystemEntryInfo"/> file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, searchOption, null, null, false, continueOnException, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, bool continueOnException, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, bool continueOnException, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption, bool continueOnException, bool isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, searchOption, null, null, false, continueOnException, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, searchOption, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of <see cref="T:FileSystemEntryInfo"/> file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [ComVisibleAttribute(false)]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, searchOption, null, null, false, continueOnException, false);
      }

      #endregion // Transacted

      #endregion // EnumerateFileSystemEntryInfos

      #region GetFileSystemEntryInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path, bool isFullPath)
      {
         return GetFileSystemEntryInfoInternal(false, null, path, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, string path, bool isFullPath)
      {
         return GetFileSystemEntryInfoInternal(isFolder, null, path, true, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path)
      {
         return GetFileSystemEntryInfoInternal(false, null, path, true, false, false);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, string path)
      {
         return GetFileSystemEntryInfoInternal(isFolder, null, path, true, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path, bool isFullPath)
      {
         return GetFileSystemEntryInfoInternal(false, transaction, path, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, KernelTransaction transaction, string path, bool isFullPath)
      {
         return GetFileSystemEntryInfoInternal(isFolder, transaction, path, true, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoInternal(false, transaction, path, true, false, false);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [ComVisibleAttribute(false)]
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoInternal(isFolder, transaction, path, true, false, false);
      }

      #endregion // Transacted

      #endregion // GetFileSystemEntryInfo


      #region VerifyObjectExists

      /// <summary>[AlphaFS] Performs a <see cref="T:Refresh()"/> and checks that the file system object (folder, file) exists. If the file system object is not found, a <see cref="T:DirectoryNotFoundException"/> or <see cref="T:FileNotFoundException"/> is thrown.</summary>
      /// <exception cref="DirectoryNotFoundException"></exception>
      /// <exception cref="FileNotFoundException"></exception>
      private void VerifyObjectExists()
      {
         Refresh();

         if (!_exists)
            NativeError.ThrowException(IsDirectory ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND, LongFullPath);
      }

      #endregion // VerifyObjectExists

      #region Reset

      /// <summary>[AlphaFS] Resets the state of the file system object to uninitialized.</summary>
      internal void Reset()
      {
         _exists = false;
         _systemInfo = null;
      }

      #endregion // Reset

      
      #region Unified Internals

      // These methods apply to files and directories.

      #region CreateFileInternal

      /// <summary>[AlphaFS] Unified method CreateFileInternal() to create or open a file, directory or I/O device.</summary>
      /// <param name="isFile"><c>null</c> indicates a device. <c>true</c> indicates a file object. <c>false</c> indicates a folder object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file or directory to create.</param>
      /// <param name="attributes">One of the <see cref="T:EFileAttributes"/> values that describes how to create or overwrite the file or directory.</param>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity"/> instance that determines the access control and audit security for the file or directory.</param>
      /// <param name="fileMode">A <see cref="T:FileMode"/> constant that determines how to open or create the file or directory.</param>
      /// <param name="fileSystemRights">A <see cref="T:FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file or directory.</param>
      /// <param name="fileShare">A <see cref="T:FileShare"/> constant that determines how the file or directory will be shared by processes.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <remarks>To obtain a directory handle using CreateFile, specify the FILE_FLAG_BACKUP_SEMANTICS flag as part of dwFlagsAndAttributes.</remarks>
      /// <remarks>The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.</remarks>
      /// <returns>A <see cref="T:SafeFileHandle"/> that provides read/write access to the file or directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object needs to be disposed by caller.")]
      [SecurityCritical]
      internal static SafeFileHandle CreateFileInternal(bool? isFile, KernelTransaction transaction, string path, EFileAttributes attributes, FileSecurity fileSecurity, FileMode fileMode, FileSystemRights fileSystemRights, FileShare fileShare, bool isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         PrivilegeEnabler privilegeEnabler = null;
         try
         {
            if (fileSecurity != null)
               fileSystemRights |= (FileSystemRights) 0x1000000;

            // AccessSystemSecurity = 0x1000000    AccessSystemAcl access type.
            // MaximumAllowed       = 0x2000000    MaximumAllowed access type.            
            if ((fileSystemRights & (FileSystemRights) 0x1000000) != 0 ||
                (fileSystemRights & (FileSystemRights) 0x2000000) != 0)
               privilegeEnabler = new PrivilegeEnabler(Privilege.Security);

            
            using (Security.NativeMethods.SecurityAttributes securityAttributes = new Security.NativeMethods.SecurityAttributes(fileSecurity))
            {
               // When isFile == null, we're working with a device.
               // When opening a VOLUME or removable media drive (for example, a floppy disk drive or flash memory thumb drive),
               // the path string should be the following form: "\\.\X:"
               // Do not use a trailing backslash (\), which indicates the root.

               string pathLp = isFullPath
                  ? Path.GetLongPathInternal(path, false, false, false, false)
                  : Path.GetFullPathInternal(transaction, path, true, false, false, (isFile == null || (bool)isFile));


               SafeFileHandle handle = transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // CreateFile() / CreateFileTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  ? NativeMethods.CreateFile(pathLp, fileSystemRights, fileShare, securityAttributes, fileMode, attributes, IntPtr.Zero)
                  : NativeMethods.CreateFileTransacted(pathLp, fileSystemRights, fileShare, securityAttributes, fileMode, attributes, IntPtr.Zero, transaction.SafeHandle, IntPtr.Zero, IntPtr.Zero);

               int lastError = Marshal.GetLastWin32Error();
               switch ((uint) lastError)
               {
                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                     if (pathLp.Equals(Path.GetPathRoot(pathLp), StringComparison.OrdinalIgnoreCase))
                        lastError = 5;
                     break;

                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                     lastError = (int) ((isFile == null || (bool) !isFile)
                        ? Win32Errors.ERROR_PATH_NOT_FOUND
                        : Win32Errors.ERROR_FILE_NOT_FOUND);
                     break;
               }

               if (handle.IsInvalid)
               {
                  handle.Close();
                  NativeError.ThrowException(lastError, pathLp);
               }

               return handle;
            }
         }
         finally
         {
            if (privilegeEnabler != null)
               privilegeEnabler.Dispose();
         }
      }

      #endregion // CreateFileInternal

      #region EncryptDecryptDirectoryInternal

      /// <summary>[AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a file or directory so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="encrypt"><c>true</c> encrypt, <c>false</c> decrypt.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void EncryptDecryptDirectoryInternal(string path, bool encrypt, bool isFullPath)
      {
         string pathLp = isFullPath ? path : Path.GetFullPathInternal(null, path, true, false, false, false);

         // Process folders and files.
         foreach (string fso in EnumerateFileSystemEntryInfoInternal<string>(null, pathLp, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, true, true, false, true))
            EncryptDecryptFileInternal(fso, encrypt, true);

         // Process the root folder, the given path.
         EncryptDecryptFileInternal(pathLp, encrypt, true);
      }

      #endregion // EncryptDecryptDirectoryInternal

      #region EncryptDecryptFileInternal

      /// <summary>[AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a file or directory so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a file to encrypt.</param>
      /// <param name="encrypt"><c>true</c> encrypt, <c>false</c> decrypt.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void EncryptDecryptFileInternal(string path, bool encrypt, bool isFullPath)
      {
         string pathLp = isFullPath ? path : Path.GetFullPathInternal(null, path, true, false, false, true);

         // EncryptFile() / DecryptFile()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!(encrypt
            ? NativeMethods.EncryptFile(pathLp)
            : NativeMethods.DecryptFile(pathLp, 0)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_ACCESS_DENIED:
                  string root = Path.GetPathRoot(pathLp, false);
                  if (!string.Equals("NTFS", new DriveInfo(root).DriveFormat, StringComparison.OrdinalIgnoreCase))
                     throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The drive does not support NTFS encryption: [{0}]", root));
                  break;

               default:
                  NativeError.ThrowException(lastError, pathLp);
                  break;
            }
         }
      }

      #endregion // EncryptDecryptFileInternal

      #region EnumerateFileSystemEntryInfoInternal

      /// <summary>[AlphaFS] Unified method EnumerateFileSystemEntryInfoInternal() to enumerate Non-/Transacted files/directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="getFolders">
      /// <c>true</c> folders will be returned.
      /// <c>false</c> files will be returned.
      /// <c>null</c> both folders and files will be returned.
      /// </param>
      /// <param name="getAsString">
      /// <c>true</c> returns the results as an enumerable <see cref="T:string"/> object.
      /// <c>false</c> returns the results as an enumerable <see cref="T:FileSystemInfo"/> object.
      /// <c>null</c> returns the results as an enumerable <see cref="T:FileSystemEntryInfo"/> object.
      /// </param>
      /// <param name="asLongPath"><c>true</c> returns the full path with long path prefix.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>
      /// If <paramref name="getAsString"/> is <c>null</c> an enumerable <see cref="T:FileSystemEntryInfo"/> collection that match <paramref name="searchPattern"/> and <paramref name="searchOption"/>.
      /// If <paramref name="getAsString"/> is <c>true</c> an enumerable <see cref="T:string"/> collection of the full pathnames that match searchPattern and searchOption.
      /// If <paramref name="getAsString"/> is <c>false</c>, an enumerable <see cref="T:FileSystemInfo"/> (<see cref="T:DirectoryInfo"/> / <see cref="T:FileInfo"/>) collection that match <paramref name="searchPattern"/> and <paramref name="searchOption"/>.
      /// </returns>
      [SecurityCritical]
      internal static IEnumerable<T> EnumerateFileSystemEntryInfoInternal<T>(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption, bool? getFolders, bool? getAsString, bool asLongPath, bool continueOnException, bool isFullPath)
      {
         foreach (FileSystemEntryInfo fsei in new FindFileSystemEntryInfo
         {
            IsFullPath = isFullPath,
            InputPath = path,
            AsLongPath = asLongPath,
            FileSystemObjectType = getFolders,
            SearchOption = searchOption,
            SearchPattern = searchPattern,
            Transaction = transaction,
            ContinueOnException = continueOnException

         }.Enumerate())
         {
               // Return FullPath property as string.
            if (getAsString != null && (bool) getAsString)
               yield return (T) (object) (asLongPath ? fsei.LongFullPath : fsei.FullPath);

            else
            {
               // Return FileSystemEntryInfo instance.
               if (getAsString == null)
               {
                  switch (getFolders)
                  {
                     case true:
                        if (fsei.IsDirectory)
                           yield return (T) (object) fsei;
                        break;

                     case false:
                        if (!fsei.IsDirectory)
                           yield return (T) (object) fsei;
                        break;

                     default:
                        yield return (T) (object) fsei;
                        break;
                  }
               }
               else
               {
                  // Return a specific instance of type: FileSystemInfo, DirectoryInfo or FileInfo.
                  // Bonus: the returned FileSystemEntryInfo instance is constructed from a Win32FindData data structure
                  // with properties already populated by the Win32 FindFirstFileEx() function.
                  // This means that the returned DirectoryInfo/FileInfo instance is already .Refresh()-ed.
                  // I call it: Cached LazyLoading.

                  switch (getFolders)
                  {
                        // true = return instance of type: DirectoryInfo.
                     case true:
                        yield return (T) (object) new DirectoryInfo(transaction, fsei.LongFullPath, true) {EntryInfo = fsei};
                        break;

                        // false = return instance of type: FileInfo.
                     case false:
                        yield return (T) (object) new FileInfo(transaction, fsei.LongFullPath, true) {EntryInfo = fsei};
                        break;

                     // null = return instances of type: DirectoryInfo or FileInfo.
                     default:
                        yield return (T)(object) (fsei.IsDirectory
                              ? (FileSystemInfo) new DirectoryInfo(transaction, fsei.LongFullPath, true) {EntryInfo = fsei}
                              : new FileInfo(transaction, fsei.LongFullPath, true) {EntryInfo = fsei});
                        break;
                  }
               }
            }
         }
      }

      #endregion // EnumerateFileSystemEntryInfoInternal

      #region ExistsInternal

      /// <summary>[AlphaFS] Unified method ExistsInternal() to determine whether the given path refers to an existing file or directory on disk.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>Note that this files may contain wildcards, such as '*'</remarks>
      /// <remarks>Trailing spaces are removed from the end of the path parameter before checking whether the directory exists.</remarks>
      /// <remarks>Trailing spaces are removed from the path parameter before determining if the file exists.</remarks>
      /// <remarks>Return value is <c>true</c> if the caller has the required permissions and path contains the name of an existing file; <c>false</c> otherwise.</remarks>
      /// <remarks>This method also returns <c>false</c> if path is a <c>null</c> reference, an invalid path, or a zero-length string.</remarks>
      /// <remarks>If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <c>false</c> regardless of the existence of path.</remarks>
      [SecurityCritical]
      internal static bool ExistsInternal(bool isFolder, KernelTransaction transaction, string path, bool isFullPath)
      {
         string pathLp = isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, true, false, false);

         // MSDN: Trailing spaces are removed from the end of the path parameter before checking whether the directory exists.
         // MSDN: Trailing spaces are removed from the path parameter before determining if the file exists.
         
         FileAttributes attrs = File.GetAttributesInternal(isFolder, transaction, pathLp, true, true, true);
         
         return attrs != (FileAttributes) (-1) && (isFolder
            ? (attrs & FileAttributes.Directory) == FileAttributes.Directory
            : (attrs & FileAttributes.Directory) != FileAttributes.Directory);
      }

      #endregion ExistsInternal

      #region GetAccessControlInternal

      /// <summary>[AlphaFS] Unified method GetAccessControlInternal() to get/set an <see cref="T:ObjectSecurity"/> for a particular file or directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="path">The path to a directory containing a <see cref="T:DirectorySecurity"/> object that describes the directory's or file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>An <see cref="T:ObjectSecurity"/> object that encapsulates the access control rules for the file or directory described by the <paramref name="path"/> parameter. </returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static T GetAccessControlInternal<T>(bool isFolder, string path, AccessControlSections includeSections, bool isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

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
            string pathLp = isFullPath ? path : Path.GetFullPathInternal(null, path, true, false, false, true);
            uint sizeRequired = 1024;

         startGetFileSecurity:

            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle((int)sizeRequired))
            {
               // GetFileSecurity()
               // Seems to perform better than GetNamedSecurityInfo() and doesn't require Administrator rights.
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               if (!Security.NativeMethods.GetFileSecurity(pathLp, securityInfo, safeBuffer, (uint)safeBuffer.Capacity, out sizeRequired))
               {
                  int lastError = Marshal.GetLastWin32Error();
                  switch ((uint)lastError)
                  {
                     case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                        safeBuffer.Dispose();
                        goto startGetFileSecurity;

                     case Win32Errors.ERROR_FILE_NOT_FOUND:
                     case Win32Errors.ERROR_PATH_NOT_FOUND:
                        lastError = (int)(isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND);
                        break;
                  }

                  // If the function fails, the return value is zero.
                  NativeError.ThrowException(lastError, path);
               }

               ObjectSecurity objectSecurity = (isFolder) ? (ObjectSecurity) new DirectorySecurity() : new FileSecurity();
               objectSecurity.SetSecurityDescriptorBinaryForm(safeBuffer.ToByteArray(0, safeBuffer.Capacity));

               return (T)(object)objectSecurity;
            }
         }
      }

      #endregion // GetAccessControlInternal
      
      #region GetCreationTimeInternal

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) or local time, of the specified file or directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <param name="getUtc"><c>true</c> gets the Coordinated Universal Time (UTC), <c>false</c> gets the local time.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file or directory. Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.</returns>
      [SecurityCritical]
      internal static DateTime GetCreationTimeInternal(bool isFolder, KernelTransaction transaction, string path, bool getUtc, bool isFullPath)
      {
         return (getUtc)
            ? DateTime.FromFileTimeUtc(File.GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).CreationTime)
            : DateTime.FromFileTime(File.GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).CreationTime);
      }

      #endregion // GetCreationTimeInternal

      #region GetFileSystemEntryInfoInternal

      /// <summary>[AlphaFS] Unified method GetFileSystemEntryInfoInternal() to get a FileSystemEntryInfo from a Non-/Transacted directory/file.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fallback"><c>true</c> enable fallback on function GetFileAttributesXxx() when function FindFirstFileXxx() fails. <c>false</c> disable fallback.</param>
      /// <param name="continueOnException">If <c>null</c>, function GetFileAttributesXxx() will be skipped in case function FindFirstFileXxx() fails. <c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory, or <c>null</c> on Exception when <paramref name="continueOnException"/> is <c>true</c>.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      internal static FileSystemEntryInfo GetFileSystemEntryInfoInternal(bool isFolder, KernelTransaction transaction, string path, bool fallback, bool continueOnException, bool isFullPath)
      {
         return new FindFileSystemEntryInfo
         {
            IsFullPath = isFullPath,
            IsFolder = isFolder,
            InputPath = path,
            Transaction = transaction,
            Fallback = fallback,
            ContinueOnException = continueOnException,

         }.Get();
      }

      #endregion // GetFileSystemEntryInfoInternal

      #region GetLastAccessTimeInternal

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) or local time, that the specified file or directory was last accessed.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain access date and time information.</param>
      /// <param name="getUtc"><c>true</c> gets the Coordinated Universal Time (UTC), <c>false</c> gets the local time.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last accessed. Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.</returns>
      [SecurityCritical]
      internal static DateTime GetLastAccessTimeInternal(bool isFolder, KernelTransaction transaction, string path, bool getUtc, bool isFullPath)
      {
         return (getUtc)
            ? DateTime.FromFileTimeUtc(File.GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastAccessTime)
            : DateTime.FromFileTime(File.GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastAccessTime);
      }

      #endregion // GetLastAccessTimeInternal

      #region GetLastWriteTimeUtcInternal

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) or local time, that the specified file or directory was last written to.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain write date and time information.</param>
      /// <param name="getUtc"><c>true</c> gets the Coordinated Universal Time (UTC), <c>false</c> gets the local time.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last written to. Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.</returns>
      [SecurityCritical]
      internal static DateTime GetLastWriteTimeInternal(bool isFolder, KernelTransaction transaction, string path, bool getUtc, bool isFullPath)
      {
         return (getUtc)
            ? DateTime.FromFileTimeUtc(File.GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastWriteTime)
            : DateTime.FromFileTime(File.GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastWriteTime);
      }

      #endregion // GetLastWriteTimeUtcInternal

      #region InitializeInternal

      /// <summary>[AlphaFS] Initializes the specified file name.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The full path and name of the file.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      internal void InitializeInternal(bool isFolder, KernelTransaction transaction, string path, bool isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(isFolder ? "path" : "filename");

         LongFullPath = isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, !isFolder);

         FullPath = Path.GetRegularPathInternal(LongFullPath, false, false, false, false);

         IsDirectory = isFolder;
         Transaction = transaction;

         OriginalPath = FullPath.Length == 2 && (FullPath[1] == Path.VolumeSeparatorChar)
            ? Path.CurrentDirectoryPrefix
            : path;

         DisplayPath = OriginalPath.Length != 2 || OriginalPath[1] != Path.VolumeSeparatorChar
            ? OriginalPath
            : Path.CurrentDirectoryPrefix;
      }

      #endregion // InitializeInternal

      #region SetAccessControlInternal

      /// <summary>[AlphaFS] Unified method SetAccessControlInternal() applies access control list (ACL) entries described by a <see cref="T:FileSecurity"/> FileSecurity object to the specified file.</summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from. This parameter This parameter may be <c>null</c>.</param>
      /// <param name="handle">A handle to add or remove access control list (ACL) entries from. This parameter This parameter may be <c>null</c>.</param>
      /// <param name="objectSecurity">A <see cref="T:DirectorySecurity"/> or <see cref="T:FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/> parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <remarks>Either use <paramref name="path"/> or <paramref name="handle"/>, not both.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void SetAccessControlInternal(string path, SafeHandle handle, ObjectSecurity objectSecurity, AccessControlSections includeSections, bool isFullPath)
      {
         if (objectSecurity == null)
            throw new ArgumentNullException("objectSecurity");


         byte[] managedDescriptor = objectSecurity.GetSecurityDescriptorBinaryForm();
         using (SafeGlobalMemoryBufferHandle hDescriptor = new SafeGlobalMemoryBufferHandle(managedDescriptor.Length))
         {
            hDescriptor.CopyFrom(managedDescriptor, 0, managedDescriptor.Length);

            SecurityDescriptorControl control;
            uint revision;
            if (!Security.NativeMethods.GetSecurityDescriptorControl(hDescriptor, out control, out revision))
               NativeError.ThrowException(Marshal.GetLastWin32Error());

            PrivilegeEnabler privilegeEnabler = null;
            try
            {
               SecurityInformation securityInfo = SecurityInformation.None;

               IntPtr pDacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Access) != 0)
               {
                  bool daclDefaulted, daclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorDacl(hDescriptor, out daclPresent, out pDacl, out daclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

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
                  if (!Security.NativeMethods.GetSecurityDescriptorSacl(hDescriptor, out saclPresent, out pSacl, out saclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

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
                  if (!Security.NativeMethods.GetSecurityDescriptorOwner(hDescriptor, out pOwner, out ownerDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  if (pOwner != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Owner;
               }

               IntPtr pGroup = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Group) != 0)
               {
                  bool groupDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorGroup(hDescriptor, out pGroup, out groupDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  if (pGroup != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Group;
               }


               uint lastError;
               if (!Utils.IsNullOrWhiteSpace(path))
               {
                  string pathLp = isFullPath ? path : Path.GetFullPathInternal(null, path, true, false, false, false);

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
                     NativeError.ThrowException(lastError);
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

      #region SetAttributesInternal

      /// <summary>[AlphaFS] Unified method SetAttributesInternal() to set the attributes for a Non-/Transacted file/directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file or directory whose attributes are to be set.</param>
      /// <param name="fileAttributes">The attributes to set for the file or directory. Note that all other values override <see cref="T:FileAttributes.Normal"/>.</param>
      /// <param name="continueOnNotExist"><c>true</c> does not throw an Exception when the file system object does not exist.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void SetAttributesInternal(bool isFolder, KernelTransaction transaction, string path, FileAttributes fileAttributes, bool continueOnNotExist, bool isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = isFullPath ? path : Path.GetFullPathInternal(transaction, path, true, false, false, false);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // SetFileAttributes()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.SetFileAttributes(pathLp, fileAttributes)
            : NativeMethods.SetFileAttributesTransacted(pathLp, fileAttributes, transaction.SafeHandle)))
         {
            if (continueOnNotExist)
               return;

            int lastError = Marshal.GetLastWin32Error();
            if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
                  lastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;

            NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // SetAttributesInternal

      #region SetFsoDateTimeInternal

      /// <summary>[AlphaFS] Unified method SetFsoDateTimeInternal() to set the date and time, in coordinated universal time (UTC), that the file or directory was created and/or last accessed and/or written to.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to set the date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="path"/> is already a full path and will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void SetFsoDateTimeInternal(bool isFolder, KernelTransaction transaction, string path, DateTime? creationTimeUtc, DateTime? lastAccessTimeUtc, DateTime? lastWriteTimeUtc, bool isFullPath)
      {
         using (SafeGlobalMemoryBufferHandle hCreationTime = SafeGlobalMemoryBufferHandle.CreateFromLong(creationTimeUtc.HasValue ? creationTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle hLastAccessTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastAccessTimeUtc.HasValue ? lastAccessTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle hLastWriteTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastWriteTimeUtc.HasValue ? lastWriteTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeFileHandle handle = CreateFileInternal(!isFolder, transaction, path, isFolder ? EFileAttributes.BackupSemantics : EFileAttributes.Normal, null, FileMode.Open, FileSystemRights.WriteAttributes, FileShare.Delete | FileShare.Write, isFullPath))
            if (!NativeMethods.SetFileTime(handle, hCreationTime, hLastAccessTime, hLastWriteTime))
               NativeError.ThrowException(path);
      }

      #endregion // SetFsoDateTimeInternal

      #region TransferTimestampsInternal

      /// <summary>[AlphaFS] Unified method TransferTimestampsInternal() to transfer the date and time stamps for the specified files and directories.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="source">The source path.</param>
      /// <param name="destination">The destination path.</param>
      /// <param name="isFullPath"><c>true</c> it is assumed that <paramref name="source"/> and <paramref name="destination"/> are already a full path and will be used as is.</param>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void TransferTimestampsInternal(bool isFolder, KernelTransaction transaction, string source, string destination, bool isFullPath)
      {
         FileSystemEntryInfo fsei = GetFileSystemEntryInfoInternal(isFolder, transaction, source, true, false, isFullPath);
         SetFsoDateTimeInternal(isFolder, transaction, destination, DateTime.FromFileTimeUtc(fsei.Win32FindData.CreationTime.ToLong()), DateTime.FromFileTimeUtc(fsei.Win32FindData.LastAccessTime.ToLong()), DateTime.FromFileTimeUtc(fsei.Win32FindData.LastWriteTime.ToLong()), isFullPath);
      }

      #endregion // TransferTimestampsInternal

      #endregion // Unified Internals

      #endregion // AlphaFS
      
      #endregion // Methods

      #region Properties

      #region .NET

      #region Attributes

      /// <summary>Gets or sets the attributes for the current file or directory.</summary>
      /// <returns><see cref="T:System.IO.FileAttributes"/> of the current <see cref="T:FileSystemInfo"/>.</returns>
      public FileAttributes Attributes
      {
         [SecurityCritical]
         get
         {
            if (EntryInfo == null)
               Refresh();

            return EntryInfo != null ? EntryInfo.Attributes : (FileAttributes)(-1);
         }

         [SecurityCritical]
         set
         {
            if (EntryInfo == null)
               VerifyObjectExists();

            SetAttributesInternal(IsDirectory, Transaction, FullPath, value, false, true);
            Reset();
         }
      }

      #endregion // Attributes

      #region CreationTime

      /// <summary>Gets or sets the creation time of the current file or directory.</summary>
      /// <returns>The creation date and time of the current <see cref="T:FileSystemInfo"/> object.</returns>
      /// <remarks>This value is expressed in local time.</remarks>
      public DateTime CreationTime
      {
         get { return CreationTimeUtc.ToLocalTime(); }
         set { CreationTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // CreationTime

      #region CreationTimeUtc

      /// <summary>Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.</summary>
      /// <returns>The creation date and time in UTC format of the current <see cref="T:FileSystemInfo"/> object.</returns>
      /// <remarks>This value is expressed in UTC time.</remarks>
      [ComVisible(false)]
      public DateTime CreationTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (EntryInfo == null)
               Refresh();

            return DateTime.FromFileTimeUtc(EntryInfo != null
               ? EntryInfo.Win32FindData.CreationTime.ToLong()
               : new NativeMethods.Win32FindData().CreationTime.ToLong());
         }

         set
         {
            if (EntryInfo == null)
               VerifyObjectExists();

            SetFsoDateTimeInternal(IsDirectory, Transaction, FullPath, value, null, null, false);
            Reset();
         }
      }

      #endregion // CreationTimeUtc

      #region Exists
      
      private bool _exists;

      /// <summary>Gets a value indicating whether the file or directory exists.</summary>
      /// <returns><c>true</c> if the file or directory exists; <c>false</c> otherwise.</returns>
      /// <remarks>The <see cref="T:Exists"/> property returns <c>false</c> if any error occurs while trying to determine if the specified file or directory exists. This can occur in situations that raise exceptions such as passing a directory- or file name with invalid characters or too many characters, a failing or missing disk, or if the caller does not have permission to read the file or directory.</remarks>
      public abstract bool Exists { get; }

      #endregion // Exists

      #region Extension

      /// <summary>Gets the extension part of the file.</summary>
      /// <returns>The <see cref="T:System.IO.FileSystemInfo"/> extension.</returns>
      public string Extension
      {
         get { return System.IO.Path.GetExtension(FullPath); }
      }

      #endregion // Extension

      #region FullName

      /// <summary>Gets the full path of the file or directory.</summary>
      /// <returns>The full path.</returns>
      public virtual string FullName
      {
         [SecurityCritical]
         get { return FullPath; }
      }

      #endregion // FullName

      #region LastAccessTime

      /// <summary>Gets or sets the time the current file or directory was last accessed.</summary>
      /// <returns>The time that the current file or directory was last accessed.</returns>
      /// <remarks>This value is expressed in local time.</remarks>
      /// <remarks>When first called, <see cref="T:FileSystemInfo"/> calls Refresh and returns the cached information on APIs to get attributes and so on. On subsequent calls, you must call Refresh to get the latest copy of the information. 
      /// If the file described in the <see cref="T:FileSystemInfo"/> object does not exist, this property will return 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time. 
      /// </remarks>
      public DateTime LastAccessTime
      {
         get { return LastAccessTimeUtc.ToLocalTime(); }
         set { LastAccessTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // LastAccessTime

      #region LastAccessTimeUtc

      /// <summary>Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.</summary>
      /// <returns>The UTC time that the current file or directory was last accessed.</returns>
      /// <remarks>This value is expressed in UTC time.</remarks>
      /// <remarks>When first called, <see cref="T:FileSystemInfo"/> calls Refresh and returns the cached information on APIs to get attributes and so on. On subsequent calls, you must call Refresh to get the latest copy of the information. 
      /// If the file described in the <see cref="T:FileSystemInfo"/> object does not exist, this property will return 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time. 
      /// </remarks>
      [ComVisible(false)]
      public DateTime LastAccessTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (EntryInfo == null)
               Refresh();

            return DateTime.FromFileTimeUtc(EntryInfo != null
               ? EntryInfo.Win32FindData.LastAccessTime.ToLong()
               : new NativeMethods.Win32FindData().LastAccessTime.ToLong());
         }

         set
         {
            if (EntryInfo == null)
               VerifyObjectExists();

            SetFsoDateTimeInternal(IsDirectory, Transaction, FullPath, null, value, null, false);
            Reset();
         }
      }

      #endregion // LastAccessTimeUtc

      #region LastWriteTime

      /// <summary>Gets or sets the time when the current file or directory was last written to.</summary>
      /// <returns>The time the current file was last written.</returns>
      /// <remarks>This value is expressed in local time.</remarks>
      public DateTime LastWriteTime
      {
         get { return LastWriteTimeUtc.ToLocalTime(); }
         set { LastWriteTimeUtc = value.ToUniversalTime(); }
      }

      #endregion // LastWriteTime

      #region LastWriteTimeUtc

      /// <summary>Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.</summary>
      /// <returns>The UTC time when the current file was last written to.</returns>
      /// <remarks>This value is expressed in UTC time.</remarks>
      [ComVisible(false)]
      public DateTime LastWriteTimeUtc
      {
         [SecurityCritical]
         get
         {
            if (EntryInfo == null)
               Refresh();

            return DateTime.FromFileTimeUtc(EntryInfo != null
               ? EntryInfo.Win32FindData.LastWriteTime.ToLong()
               : new NativeMethods.Win32FindData().LastWriteTime.ToLong());
         }

         set
         {
            if (EntryInfo == null)
               VerifyObjectExists();

            SetFsoDateTimeInternal(IsDirectory, Transaction, FullPath, null, null, value, false);
            Reset();
         }
      }

      #endregion // LastWriteTimeUtc

      #region Name

      /// <summary>For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.</summary>
      /// <returns>The name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension.</returns>
      public abstract string Name { get; }

      #endregion // Name

      #endregion // .NET

      #region AlphaFS

      #region EntryInfo

      private FileSystemEntryInfo _systemInfo;

      /// <summary>[AlphaFS] Gets the instance of the <see cref="T:FileSystemEntryInfo"/> class.</summary>
      public FileSystemEntryInfo EntryInfo
      {
         get
         {
            if (_systemInfo == null)
               Refresh();

            return _systemInfo;
         }

         private set { _systemInfo = value; }
      }

      #endregion // EntryInfo

      #region Transaction

      [NonSerialized]
      private KernelTransaction _transaction;

      /// <summary>[AlphaFS] Represents the KernelTransaction that was passed to the constructor.</summary>
      protected KernelTransaction Transaction
      {
         get { return _transaction; }
         set { _transaction = value; }
      }

      #endregion // Transaction

      #endregion // AlphaFS

      #endregion // Properties

      #region Fields

      #region .NET

      /// <summary>Represents the fully qualified path of the file or directory.</summary>
      /// <remarks>Classes derived from <see cref="T:FileSystemInfo"/> can use the FullPath field to determine the full path of the object being manipulated.</remarks>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string FullPath;

      /// <summary>The path originally specified by the user, whether relative or absolute.</summary>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string OriginalPath;

      #endregion // .NET

      #region AlphaFS

      /// <summary>Returns the path as a string.</summary>
      internal string DisplayPath;

      /// <summary>[AlphaFS] The initial "IsDirectory" indicator that was passed to the constructor.</summary>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected bool IsDirectory;

      /// <summary>Represents the fully qualified path with  of the file or directory.</summary>
      /// <remarks>The path is always in Unicode (LongPath) format.</remarks>
      [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
      protected string LongFullPath;

      #endregion // AlphaFS

      #endregion // Fields
   }
}
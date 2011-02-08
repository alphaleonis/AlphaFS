/* Copyright (c) 2008-2009 Peter Palotas
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using Alphaleonis.Win32.Security;
using SearchOption = System.IO.SearchOption;
using SecurityNativeMethods = Alphaleonis.Win32.Security.NativeMethods;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// Exposes static methods for creating, moving, and enumerating through directories and subdirectories. 
   /// </summary>
   /// <remarks>
   /// <para>As opposed to <see cref="System.IO.Directory"/> this class supports the use of extenden length unicode paths, such as 
   /// <c>\\?\Volume{c00fa7c5-63eb-11dd-b6ed-806e6f6e6963}\Program Files\Internet Explorer</c>. In addition, support for transacted file operation 
   /// using the kernel transaction manager is provided. (See also <see cref="KernelTransaction"/>).</para>
   /// <para>Note that no methods in this class perform any validation of the supplied paths. They are passed as is to the corresponding
   /// native kernel functions, meaning that invalid paths may result in exceptions of a type other than the expected for a certain operation.
   /// </para>
   /// </remarks>
   public static class Directory
   {
      #region CreateDirectory

      #region Non-transactional

      /// <overloads>
      /// <summary>Creates a new directory.</summary>
      /// </overloads>
      /// <summary>
      /// Creates a new directory. If the underlying file system supports security on files and directories, the function applies a default security descriptor to the new directory.
      /// </summary>
      /// <param name="pathName">The path of the directory to be created.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(string pathName)
      {
         CreateDirectory((string)null, pathName, (DirectorySecurity)null);
      }

      /// <summary>
      /// Creates a new directory. If the underlying file system supports security on files and directories, the function applies a security descriptor to the new directory.
      /// </summary>
      /// <param name="pathName">The path of the directory to be created.</param>
      /// <param name="security">The seurity descriptor to apply to the directory</param>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(string pathName, DirectorySecurity security)
      {
         CreateDirectory((string)null, pathName, security);
      }

      /// <summary>
      /// Creates a new directory with the attributes of a specified template directory. 
      /// If the underlying file system supports security on files and directories, the function 
      /// applies a default security descriptor to the new directory. The new directory retains 
      /// the other attributes of the specified template directory.
      /// </summary>
      /// <param name="templatePathName">The path of the directory to use as a template when creating the new directory. </param>
      /// <param name="newPathName">The path of the directory to be created. </param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(string templatePathName, string newPathName)
      {
         CreateDirectory(templatePathName, newPathName, null);
      }

      /// <summary>
      /// Creates a new directory with the attributes of a specified template directory. 
      /// If the underlying file system supports security on files and directories, the function 
      /// applies the specified security descriptor to the new directory. The new directory retains 
      /// the other attributes of the specified template directory.
      /// </summary>
      /// <param name="templatePathName">The path of the directory to use as a template when creating the new directory. </param>
      /// <param name="newPathName">The path of the directory to be created. </param>
      /// <param name="security">The seurity descriptor to apply to the directory.</param>
      /// <exception cref="AlreadyExistsException">The specified directory already exists.</exception>
      /// <exception cref="System.IO.DirectoryNotFoundException">One or more intermediate directories do not exist; this function will only create the final directory in the path.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(string templatePathName, string newPathName, DirectorySecurity security)
      {
         if (newPathName == null)
             throw new ArgumentNullException("newPathName");

         NativeMethods.SecurityAttributes lpSecurityAttributes = null;
         if (security != null)
         {
             lpSecurityAttributes = new NativeMethods.SecurityAttributes();
             SafeGlobalMemoryBufferHandle handle;
             lpSecurityAttributes.Initialize(out handle, security);
         }

         CreateDirectoryInternal(templatePathName, newPathName, lpSecurityAttributes);
      }

      /// <summary>
      /// Creates a new directory with the attributes of a specified template directory (if one is specified). 
      /// If the underlying file system supports security on files and directories, the function 
      /// applies the specified security descriptor to the new directory. The new directory retains 
      /// the other attributes of the specified template directory.
      /// </summary>
      /// <param name="templatePathName">The path of the directory to use as a template when creating the new directory. May be <see langword="null"/> to indicate 
      /// that no template should be used.</param>
      /// <param name="newPathName">The path of the directory to be created.</param>
      /// <param name="lpSecurityAttributes">The security descriptor to apply to the newly created directory. May be <see langword="null"/> in which case a default 
      /// security descriptor will be applied.</param>
      private static void CreateDirectoryInternal(string templatePathName, string newPathName, NativeMethods.SecurityAttributes lpSecurityAttributes)
      {
          bool status;

          if (templatePathName == null)
              status = NativeMethods.CreateDirectoryW(newPathName, lpSecurityAttributes);
          else
              status = NativeMethods.CreateDirectoryExW(templatePathName, newPathName, lpSecurityAttributes);

          if (!status)
          {
              switch ((uint)Marshal.GetLastWin32Error())
              {
                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                      if (!Exists(Path.GetDirectoryName(newPathName)) && Exists(Path.GetPathRoot(newPathName)))
                      {
                          CreateDirectoryInternal(templatePathName, Path.GetDirectoryName(newPathName), lpSecurityAttributes);
                          CreateDirectoryInternal(templatePathName, newPathName, lpSecurityAttributes);
                      }
                      else
                          NativeError.ThrowException(null, newPathName);
                      break;
                  case Win32Errors.ERROR_ALREADY_EXISTS:
                      //as stated in the MSDN article for Directory.CreateDirectory() method,
                      //it should throw exception only for existing files with the same name as requested directory
                      // http://msdn.microsoft.com/en-us/library/54a0at6s.aspx
                      if(File.Exists(newPathName))
                          NativeError.ThrowException(null, newPathName);
                      break;
                  default:
                      // throw exceptions for everything else
                      NativeError.ThrowException(null, newPathName);
                      break;
              }
          }
      }

      #endregion

      #region Transactional

      /// <summary>
      /// Creates a new directory as a transacted operation. 
      /// If the underlying file system supports security on files and directories, the function applies a 
      /// default security descriptor to the new directory. 
      /// </summary>
      /// <param name="transaction">The transaction to use.</param>
      /// <param name="newPathName">The path of the directory to be created.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(KernelTransaction transaction, string newPathName)
      {
         CreateDirectory(transaction, null, newPathName, null);
      }

      /// <summary>
      /// Creates a new directory as a transacted operation. 
      /// If the underlying file system supports security on files and directories, the function 
      /// applies a specified security descriptor to the new directory. 
      /// </summary>
      /// <param name="transaction">The transaction to use.</param>
      /// <param name="newPathName">The path of the directory to be created. </param>
      /// <param name="security">
      /// <para>
      ///     If <paramref name="security"/> is <see langword="null"/>, the directory gets a default security descriptor. 
      ///     The access control lists (ACL) in the default security descriptor for a directory are inherited from its parent directory.
      /// </para>
      /// <para>
      ///     The target file system must support security on files and directories for this parameter to have an effect. 
      ///     This is indicated when <see cref="Volume.GetVolumeInformation(string)"/> returns an object with <see cref="VolumeInfo.HasPersistentAccessControlLists"/> 
      ///     set to <c>true</c>.
      /// </para>
      /// </param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(KernelTransaction transaction, string newPathName, DirectorySecurity security)
      {
         CreateDirectory(transaction, null, newPathName, security);
      }

      /// <summary>
      /// Creates a new directory as a transacted operation, with the attributes of a specified template directory. 
      /// If the underlying file system supports security on files and directories, the function applies a default security descriptor to the new directory. 
      /// The new directory retains the other attributes of the specified template directory.
      /// </summary>
      /// <param name="templatePathName">
      /// <para>The path of the directory to use as a template when creating the new directory. This parameter can be <see langword="null"/>. </para>
      /// <para>The directory must reside on the local computer; otherwise, the an exception of type <see cref="UnsupportedRemoteTransactionException"/> is thrown.</para>
      /// </param>
      /// <param name="transaction">The transaction to use.</param>
      /// <param name="newPathName">The path of the directory to be created. </param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(KernelTransaction transaction, string templatePathName, string newPathName)
      {
         CreateDirectory(transaction, templatePathName, newPathName, null);
      }

      /// <summary>
      /// Creates a new directory as a transacted operation, with the attributes of a specified template directory. If the underlying file system supports security on files and directories, the function applies a specified security descriptor to the new directory. The new directory retains the other attributes of the specified template directory.
      /// </summary>
      /// <param name="templatePathName">
      /// <para>The path of the directory to use as a template when creating the new directory. This parameter can be <see langword="null"/>. </para>
      /// <para>The directory must reside on the local computer; otherwise, the an exception of type <see cref="UnsupportedRemoteTransactionException"/> is thrown.</para>
      /// </param>
      /// <param name="transaction">The transaction to use.</param>
      /// <param name="newPathName">The path of the directory to be created. </param>
      /// <param name="security">
      /// <para>
      ///     If <paramref name="security"/> is <see langword="null"/>, the directory gets a default security descriptor. 
      ///     The access control lists (ACL) in the default security descriptor for a directory are inherited from its parent directory.
      /// </para>
      /// <para>
      ///     The target file system must support security on files and directories for this parameter to have an effect. 
      ///     This is indicated when <see cref="Volume.GetVolumeInformation(string)"/> returns an object with <see cref="VolumeInfo.HasPersistentAccessControlLists"/> 
      ///     set to <c>true</c>.
      /// </para>
      /// </param>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void CreateDirectory(KernelTransaction transaction, string templatePathName, string newPathName, DirectorySecurity security)
      {
         if (newPathName == null)
			 throw new ArgumentNullException("newPathName");

         if (transaction == null)
            throw new ArgumentNullException("transaction");

         NativeMethods.SecurityAttributes securityAttributes = null;
         if (security != null)
         {
             securityAttributes = new NativeMethods.SecurityAttributes();
             SafeGlobalMemoryBufferHandle securityDescriptorBuffer;
             securityAttributes.Initialize(out securityDescriptorBuffer, security);
         }
          
         bool status = NativeMethods.CreateDirectoryTransactedW(templatePathName, newPathName, securityAttributes, transaction.SafeHandle);
         if (!status)
         {
             switch ((uint)Marshal.GetLastWin32Error())
             {
                 case Win32Errors.ERROR_PATH_NOT_FOUND:
                     if (!Exists(transaction, Path.GetDirectoryName(newPathName)) && Exists(transaction, Path.GetPathRoot(newPathName)))
                     {
                         CreateDirectory(transaction, templatePathName, Path.GetDirectoryName(newPathName), security);
                         CreateDirectory(transaction, templatePathName, newPathName, security);
                     }
                     else
                     {
                         NativeError.ThrowException(null, newPathName);
                     }
                     break;
                  case Win32Errors.ERROR_ALREADY_EXISTS:
                     //as stated in the MSDN article for Directory.CreateDirectory() method,
                     //it should throw exception only for existing files with the same name as requested directory
                     // http://msdn.microsoft.com/en-us/library/54a0at6s.aspx
                     if (File.Exists(transaction, newPathName))
                         NativeError.ThrowException(null, newPathName);
                      break;
                  default:
                      // throw exceptions for everything else
                      NativeError.ThrowException(null, newPathName);
                      break;
             }
         }
      }

      #endregion

      #endregion

      #region CountFiles

      /// <summary>
      /// Counts files in a given directory. Uses <see cref="GetProperties"/> method.
      /// It's way faster than using lots of memory through <see cref="System.IO.Directory.GetFiles(string)"/> method.
      /// </summary>
      /// <param name="directory">The directory path.</param>
      /// <param name="searchOption"><see cref="System.IO.SearchOption"/> The search option. Either top only directory, or subfolders too.</param>
      /// <param name="continueOnAccessErrors">if set to <c>true</c> skip on access errors resulted from ACLs protected directories or not accessible reparse points, otherwise a <see cref="System.UnauthorizedAccessException"/> will be thrown.</param>
      /// <returns>The counted number of files.</returns>
      public static long CountFiles(string directory, SearchOption searchOption, bool continueOnAccessErrors)
      {
         Dictionary<string, long> result = GetProperties(null, directory, searchOption, continueOnAccessErrors);
         return result["File"];
      }

      #endregion

      #region Delete

      /// <overloads>
      /// <summary>
      /// Deletes an existing directory.
      /// </summary>
      /// </overloads>
      /// <summary>
      /// Deletes an existing empty directory.
      /// </summary>
      /// <param name="path">The path of the directory to be removed. This path must specify an empty directory, and the calling process must have delete access to the directory.</param>
      /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/></exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Delete(string path)
      {
         Delete(path, false, false);
      }

      /// <summary>
      /// Deletes the specified directory and, if indicated, any subdirectories in the directory.
      /// </summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in path; otherwise, <c>false</c>.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Delete(string path, bool recursive)
      {
         Delete(path, recursive, false);
      }

      /// <summary>
      /// Deletes the specified directory and, if indicated, any subdirectories in the directory.
      /// </summary>
      /// <param name="directoryPath">The name of the directory to remove.</param>
      /// <param name="recursive"><c>true</c> to remove all subdirectories and files recursively; otherwise, <c>false</c> only the top level empty directory.</param>
      /// <param name="ignoreReadOnly">if set to <c>true</c> overrides read only attribute of files and directories.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Delete(string directoryPath, bool recursive, bool ignoreReadOnly)
      {
         if (directoryPath == null)
             throw new ArgumentNullException("directoryPath");

         if (!recursive)
         {
            if (!NativeMethods.RemoveDirectoryW(directoryPath))
            {
               if ((Marshal.GetLastWin32Error() == Win32Errors.ERROR_ACCESS_DENIED) && ignoreReadOnly)
               {
                  File.SetAttributes(directoryPath, FileAttributes.Normal);
                  Delete(directoryPath, false, ignoreReadOnly); // repeat the deletion again after removing ReadOnly flag
               }
               else
               {
                  NativeError.ThrowException(directoryPath, directoryPath);
               }
            }
         }
         else
         {
             foreach (FileSystemEntryInfo fsentry in GetFullFileSystemEntries(null, directoryPath, "*", SearchOption.TopDirectoryOnly, false, null, null))
             {
                 if (fsentry.IsDirectory)
                 {
                     Delete(fsentry.FullPath, true, ignoreReadOnly); // delete subdirectory entries
                 }
                 else
                     File.Delete(fsentry.FullPath, ignoreReadOnly);
             }

             Delete(directoryPath, false, ignoreReadOnly); // delete the directory itself after all sub entries had been deleted
         }
      }

      /// <summary>
      /// Deletes an existing empty directory as a transacted operation.
      /// </summary>
      /// <param name="pathName">
      /// <para>The path of the directory to be removed. </para>
      /// <para>The path must specify an empty directory, and the calling process must have delete access to the directory.</para>
      /// <para>The path of the directory to be removed. This path must specify an empty directory, and the calling process must have delete access to the directory.</para>
      /// <para>The directory must reside on the local computer; otherwise, the function throws <see cref="UnsupportedRemoteTransactionException"/>.</para>
      /// </param>
      /// <param name="transaction">The transaction to use</param>
      /// <exception cref="ArgumentNullException"><paramref name="pathName"/> or <paramref name="transaction"/> is <see langword="null"/></exception>
      /// <exception cref="UnsupportedRemoteTransactionException">The directory <paramref name="pathName"/> does not reside on the local computer.</exception>
      /// <exception cref="InvalidTransactionException">The transaction object is not valid for this operation.</exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Delete(KernelTransaction transaction, string pathName)
      {
         if (pathName == null)
            throw new ArgumentNullException("pathName");

         if (transaction == null)
            throw new ArgumentNullException("transaction");

         if (!NativeMethods.RemoveDirectoryTransactedW(pathName, transaction.SafeHandle))
         {
            NativeError.ThrowException(pathName, pathName);
         }
      }

      #endregion

      #region Exists

      /// <overloads>
      /// <summary>
      /// Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// </overloads>
      /// <summary>
      /// Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// <param name="path">The path to test.</param>
      /// <returns><c>true</c> if path refers to an existing directory; otherwise, <c>false</c>.</returns>
      /// <remarks>Possible performance improvement may be achieved by utilizing <c>FINDEX_SEARCH_OPS.FindExSearchLimitToDirectories</c>.</remarks>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static bool Exists(string path)
      {
          try
          {
              if (File.GetFileSystemEntryInfo(path).IsDirectory)
                  return true;
          }
          catch
          { }

          return false;
      }

      /// <summary>
      /// Determines whether the given path refers to an existing directory on disk as part of a transaction.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      /// <returns>
      /// 	<c>true</c> if path refers to an existing directory; otherwise, <c>false</c>.
      /// </returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static bool Exists(KernelTransaction transaction, string path)
      {
          try
          {
              if (File.GetFileSystemEntryInfo(transaction, path).IsDirectory)
                  return true;
          }
          catch
          { }

          return false;
      }


      #endregion

      #region GetAccessControl

      /// <overloads>
      /// <summary>
      /// Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for a specified directory.
      /// </summary>
      /// </overloads>
      /// <summary>
      /// Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the specified directory.
      /// </summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DirectorySecurity GetAccessControl(string path)
      {
         return GetAccessControl(path, AccessControlSections.Owner | AccessControlSections.Group | AccessControlSections.Access);
      }

      /// <summary>
      /// Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular directory.
      /// </summary>
      /// <param name="path">The path to a directory containing a <see cref="DirectorySecurity"/> object that describes the directory's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory described by the <paramref name="path"/> parameter. </returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         UInt32 securityInfo = 0;
         PrivilegeEnabler privilegeEnabler = null;

         try
         {
            if ((includeSections & AccessControlSections.Access) != 0)
               securityInfo |= NativeMethods.DACL_SECURITY_INFORMATION;

            if ((includeSections & AccessControlSections.Audit) != 0)
            {
               // We need the SE_SECURITY_NAME privilege enabled to be able to get the
               // SACL descriptor. So we enable it here for the reamined of this function.
               privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
               securityInfo |= NativeMethods.SACL_SECURITY_INFORMATION;
            }

            if ((includeSections & AccessControlSections.Group) != 0)
               securityInfo |= NativeMethods.GROUP_SECURITY_INFORMATION;

            if ((includeSections & AccessControlSections.Owner) != 0)
               securityInfo |= NativeMethods.OWNER_SECURITY_INFORMATION;

            uint sizeRequired;
            SafeGlobalMemoryBufferHandle buffer = new SafeGlobalMemoryBufferHandle(256);
            if (!NativeMethods.GetFileSecurity(path, securityInfo, buffer, (uint)buffer.Capacity, out sizeRequired))
            {
               int lastError = Marshal.GetLastWin32Error();

               if (sizeRequired > buffer.Capacity)
               {
                  // A larger buffer was required to store the descriptor, so we increase the size and try again.
                  buffer.Dispose();
                  buffer = new SafeGlobalMemoryBufferHandle((int)sizeRequired);
                  if (!NativeMethods.GetFileSecurity(path, securityInfo, buffer, (uint)buffer.Capacity, out sizeRequired))
                     NativeError.ThrowException(path, null);
               }
               else
               {
                  NativeError.ThrowException(lastError, path, null);
               }
            }
            DirectorySecurity ds = new DirectorySecurity();
            ds.SetSecurityDescriptorBinaryForm(buffer.ToByteArray(0, (int)sizeRequired));

            return ds;
         }
         finally
         {
            if (privilegeEnabler != null)
               privilegeEnabler.Dispose();
         }
      }

      #endregion

      #region Get Directory Times

      #region Non Transacted

      /// <overloads>
      /// <summary>
      /// Gets the creation date and time, in local time, of a directory.
      /// </summary>
      /// </overloads>
      /// <summary>
      /// Gets the creation date and time, in local time, of a directory.
      /// </summary>
      /// <param name="path">The path of the directory. </param>
      /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetCreationTime(string path)
      {
         return File.GetCreationTime(path);
      }

      /// <overloads>
      /// <summary>
      /// Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.
      /// </summary>
      /// </overloads>
      /// <summary>
      /// Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.
      /// </summary>
      /// <param name="path">The path of the directory. </param>
      /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetCreationTimeUtc(string path)
      {
         return File.GetCreationTimeUtc(path);
      }

      /// <overloads>
      /// Returns the date and time the specified file or directory was last accessed. 
      /// </overloads>
      /// <summary>
      /// Returns the date and time the specified file or directory was last accessed. 
      /// </summary>
      /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastAccessTime(string path)
      {
         return File.GetLastAccessTime(path);
      }

      /// <summary>
      /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed. 
      /// </summary>
      /// <param name="path">The path.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastAccessTimeUtc(string path)
      {
         return File.GetLastAccessTimeUtc(path);
      }

      /// <overloads>
      ///Returns the date and time the specified file or directory was last written to. 
      /// </overloads>
      /// <summary>
      ///Returns the date and time the specified file or directory was last written to. 
      /// </summary>
      /// <param name="path">The path.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastWriteTime(string path)
      {
         return File.GetLastWriteTime(path);
      }

      /// <overloads>
      /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.
      /// </overloads>
      /// <summary>
      /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.
      /// </summary>
      /// <param name="path">The file or directory for which to obtain write date and time information. </param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written. This value is expressed in UTC time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastWriteTimeUtc(string path)
      {
         return File.GetLastWriteTimeUtc(path);
      }

      #endregion

      #region Transacted

      /// <summary>
      /// Returns the creation date and time of the specified file or directory as part of a transaction.
      /// </summary>
      /// <param name="transaction">The transaction to use.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <returns>
      /// A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.
      /// </returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path)
      {
         return File.GetCreationTime(transaction, path);
      }

      /// <summary>
      /// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory as part of a transaction. 
      /// </summary>
      /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
      /// <param name="transaction">The transaction to use.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetCreationTimeUtc(transaction, path);
      }

      /// <summary>
      /// Returns the date and time the specified file or directory was last accessed as part of a transaction. 
      /// </summary>
      /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
      /// <param name="transaction">The transaction to use.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path)
      {
         return File.GetLastAccessTime(transaction, path);
      }

      /// <summary>
      /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed as part of a transaction. 
      /// </summary>
      /// <param name="path">The path.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction to use.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetLastAccessTimeUtc(transaction, path);
      }

      /// <summary>
      ///Returns the date and time the specified file or directory was last written to as part of a transaction. 
      /// </summary>
      /// <param name="path">The path.</param>
      /// <param name="transaction">The transaction to use.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path)
      {
         return File.GetLastWriteTime(transaction, path);
      }

      /// <summary>
      /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to as part of a transaction.
      /// </summary>
      /// <param name="path">The file or directory for which to obtain write date and time information. </param>
      /// <param name="transaction">The transaction to use.</param>
      /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written. This value is expressed in UTC time.</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetLastWriteTimeUtc(transaction, path);
      }

      #endregion

      #endregion

      #region Get/Set Current Directory

      /// <summary>
      /// Gets the current working directory of the application.
      /// </summary>
      /// <returns>A string containing the path of the current working directory.</returns>
      /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      public static string GetCurrentDirectory()
      {
         return System.IO.Directory.GetCurrentDirectory();
      }

      /// <summary>
      /// Sets the application's current working directory to the specified directory.
      /// </summary>
      /// <param name="path">The path to which the current working directory is set. </param>
      /// <exception cref="System.IO.IOException">An IO error occurred.</exception>
      /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.</exception>
      /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/></exception>
      /// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception>
      /// <exception cref="System.Security.SecurityException">The caller does not have the required permission to access unmanaged code.</exception>
      /// <exception cref="System.IO.FileNotFoundException">The specified path was not found.</exception>
      /// <exception cref="System.IO.DirectoryNotFoundException">The specified directory was not found.</exception>
      public static void SetCurrentDirectory(string path)
      {
          if (path.Length > 255)
              System.IO.Directory.SetCurrentDirectory(Path.GetShort83Path(Path.GetRegularPath(path)));
          else
              System.IO.Directory.SetCurrentDirectory(Path.GetRegularPath(path));
      }

      #endregion

      #region Set Access Control
      /// <summary>
      /// Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> DirectorySecurity object to the specified directory.
      /// </summary>
      /// <remarks>Note that unlike <see cref="System.IO.File.SetAccessControl"/> this method does <b>not</b> automatically
      /// determine what parts of the specified <see cref="DirectorySecurity"/> instance has been modified. Instead, the
      /// parameter <paramref name="includeSections"/> is used to specify what entries from <paramref name="directorySecurity"/> to 
      /// apply to <paramref name="path"/>.</remarks>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="directorySecurity">A  <see cref="DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the <paramref name="path"/> parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control 
      /// list (ACL) information to set.</param>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      public static void SetAccessControl(string path, DirectorySecurity directorySecurity, AccessControlSections includeSections)
      {

         if (path == null)
            throw new ArgumentNullException(path);

         SecurityNativeMethods.SECURITY_INFORMATION info = 0;
         if ((includeSections & AccessControlSections.Access) != 0)
            info |= SecurityNativeMethods.SECURITY_INFORMATION.DACL_SECURITY_INFORMATION;

         if ((includeSections & AccessControlSections.Audit) != 0)
            info |= SecurityNativeMethods.SECURITY_INFORMATION.SACL_SECURITY_INFORMATION;

         if ((includeSections & AccessControlSections.Group) != 0)
            info |= SecurityNativeMethods.SECURITY_INFORMATION.GROUP_SECURITY_INFORMATION;

         if ((includeSections & AccessControlSections.Owner) != 0)
            info |= SecurityNativeMethods.SECURITY_INFORMATION.OWNER_SECURITY_INFORMATION;

         byte[] d = directorySecurity.GetSecurityDescriptorBinaryForm();
         SafeGlobalMemoryBufferHandle descriptor = new SafeGlobalMemoryBufferHandle(d.Length);
         descriptor.CopyFrom(d, 0, d.Length);
         if (!NativeMethods.SetFileSecurityW(path, info, descriptor))
            NativeError.ThrowException(null, path);
      }

      #endregion

      #region Move

      /// <overloads>
      /// Moves a file or a directory and its contents to a new location.
      /// </overloads>
      /// <summary>
      /// Moves a file or a directory and its contents to a new location.
      /// </summary>
      /// <param name="sourceDirName">The path of the file or directory to move. </param>
      /// <param name="destDirName">The path to the new location for <paramref name="sourceDirName"/>. If <paramref name="sourceDirName"/> is a file, then <paramref name="destDirName"/> must also be a file name.</param>
      /// <remarks>For more options, see the Move methods of <see cref="File"/>.</remarks>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Move(string sourceDirName, string destDirName)
      {
         File.Move(sourceDirName, destDirName);
      }

      /// <summary>
      /// Moves a file or a directory and its contents to a new location as part of a transaction.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceDirName">The path of the file or directory to move.</param>
      /// <param name="destDirName">The path to the new location for <paramref name="sourceDirName"/>. If <paramref name="sourceDirName"/> is a file, then <paramref name="destDirName"/> must also be a file name.</param>
      /// <remarks>For more options, see the Move methods of <see cref="File"/>.</remarks>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Move(KernelTransaction transaction, string sourceDirName, string destDirName)
      {
         File.Move(transaction, sourceDirName, destDirName);
      }

      #endregion

      #region GetDirectories

      #region Non transacted

	  /// <summary>
	  /// Gets the names of subdirectories in the specified directory.
	  /// </summary>
	  /// <param name="path">The directory to search.</param>
	  /// <returns>
	  /// An array of type String containing the names of subdirectories in <paramref name="path"/>.
	  /// </returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetDirectories(string path)
	  {
		  return GetDirectories(path, "*");
	  }

	  /// <summary>
	  /// Gets an array of directories matching the specified search pattern from the current directory.
	  /// </summary>
	  /// <param name="path">The directory to search.</param>
	  /// <param name="searchPattern">The search string to match against the names of directories in <paramref ref="path" />. The parameter cannot
	  /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <returns>
	  /// A String array of directories matching the search pattern. Directory names include the full path.
	  /// </returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetDirectories(string path, string searchPattern)
	  {
		  return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
	  }

      /// <overloads>
      /// Gets an array of directories contained within a directory or drive.
      /// </overloads>
      /// <summary>
      /// Gets an array of directories matching the specified search pattern from the current directory, using a value to determine whether to search subdirectories.
      /// </summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref ref="path" />. The parameter cannot
      /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
      /// <param name="searchOption">One of the <see cref="System.IO.SearchOption"/> values that specifies whether the
      /// search operation should include all subdirectories or only the current directory.</param>
      /// <returns>
      /// A String array of directories matching the search pattern. Directory names include the full path.
      /// </returns>
      /// <remarks>This method may consume a lot of memory if a large tree of files, directories and subdirectories
      /// are searched. Consider using <see cref="FileSystemEntryEnumerator"/> instead if possible.</remarks>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
      {
		  return GetDirectoriesInternal(null, path, searchPattern, searchOption);
      }

      #endregion

      #region Transacted

	  /// <summary>
	  /// Gets the names of subdirectories in the specified directory. The search is performed as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory to search.</param>
	  /// <returns>
	  /// An array of type String containing the names of subdirectories in <paramref name="path"/>.
	  /// </returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetDirectories(KernelTransaction transaction, string path)
	  {
		  return GetDirectories(transaction, path, "*");
	  }

	  /// <summary>
	  /// Gets an array of directories matching the specified search pattern from the current directory.  The search is performed as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory to search.</param>
	  /// <param name="searchPattern">The search string to match against the names of directories in <paramref ref="path"/>. The parameter cannot
	  /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <returns>
	  /// A String array of directories matching the search pattern. Directory names include the full path.
	  /// </returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetDirectories(KernelTransaction transaction, string path, string searchPattern)
	  {
		  return GetDirectories(transaction, path, searchPattern, SearchOption.TopDirectoryOnly);
	  }

      /// <summary>
      /// Gets an array of directories matching the specified search pattern from the current directory,
      /// using a value to determine whether to search subdirectories. The search is performed as part of a transaction.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref ref="path"/>. The parameter cannot
      /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
      /// <param name="searchOption">One of the <see cref="System.IO.SearchOption"/> values that specifies whether the
      /// search operation should include all subdirectories or only the current directory.</param>
      /// <returns>
      /// A String array of directories matching the search pattern. Directory names include the full path.
      /// </returns>
      /// <remarks>This method may consume a lot of memory if a large tree of files, directories and subdirectories
	  /// are searched. Consider using <see cref="FileSystemEntryEnumerator"/> instead if possible.</remarks>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetDirectories(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
	  {
		  if(transaction == null)
		  	throw new ArgumentNullException("transaction");

      	return GetDirectoriesInternal(transaction, path, searchPattern, searchOption);
	  }

      #endregion

	  private static string[] GetDirectoriesInternal(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
	  {
		  List<string> entries = new List<string>(20);

		  foreach (FileSystemEntryInfo entry in GetFullFileSystemEntries(transaction, path, searchPattern, searchOption, true, null, null)) {
			  // even though we specified a search parameter to folders only
			  // we shoudl double check it anyways according to this article
			  // http://msdn.microsoft.com/en-us/library/aa364416%28v=VS.85%29.aspx
			  if (entry.IsDirectory)
				  entries.Add(entry.FullPath);
		  }

		  return entries.ToArray();
	  }

      #endregion

      #region GetFiles

      #region Non transacted

	  /// <summary>
	  /// Returns the names of files in the specified directory.
	  /// </summary>
	  /// <param name="path">The directory from which to retrieve the files.</param>
	  /// <returns>A String array of file names in the specified directory.</returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetFiles(string path)
	  {
		  return GetFiles(path, "*");
	  }

	  /// <summary>
	  /// Returns the names of files in the specified directory that match the specified search pattern.
	  /// </summary>
	  /// <param name="path">The directory to search. </param>
	  /// <param name="searchPattern">The search string to match against the names of files in path. The parameter cannot 
	  /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or 
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <returns>A String array containing the names of files in the specified directory that match the specified search pattern.</returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetFiles(string path, string searchPattern)
	  {
		  return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
	  }

      /// <overloads>
      /// Retrieves the names of files contained within a directory.
      /// </overloads>
      /// <summary>
      /// Returns the names of files in the specified directory that match the specified search pattern, using a 
      /// value to determine whether to search subdirectories.
      /// </summary>
      /// <param name="path">The directory to search. </param>
      /// <param name="searchPattern">The search string to match against the names of files in path. The parameter cannot 
      /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or 
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
      /// <param name="searchOption">One of the <see cref="System.IO.SearchOption"/> values that specifies whether the 
      /// search operation should include all subdirectories or only the current directory.</param>
      /// <returns>A String array containing the names of files in the specified 
      /// directory that match the specified search pattern. File names include the full path.</returns>
      /// <remarks>This method may consume a lot of memory if a large tree of files, directories and subdirectories
      /// are searched. Consider using <see cref="FileSystemEntryEnumerator"/> instead if possible.</remarks>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
      {
		  return GetFilesInternal(null, path, searchPattern, searchOption);
      }

      #endregion

      #region Transacted

	  /// <summary>
	  /// Returns the names of files in the specified directory. The search will be performed as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory from which to retrieve the files.</param>
	  /// <returns>
	  /// A String array of file names in the specified directory.
	  /// </returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetFiles(KernelTransaction transaction, string path)
	  {
		  return GetFiles(transaction, path, "*");
	  }

	  /// <summary>
	  /// Returns the names of files in the specified directory that match the specified search pattern. The search will be performed as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory to search.</param>
	  /// <param name="searchPattern">The search string to match against the names of files in path. The parameter cannot
	  /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <returns>
	  /// A String array containing the names of files in the specified directory that match the specified search pattern.
	  /// </returns>
	  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	  public static string[] GetFiles(KernelTransaction transaction, string path, string searchPattern)
	  {
		  return GetFiles(transaction, path, searchPattern, SearchOption.TopDirectoryOnly);
	  }

      /// <summary>
      /// Returns the names of files in the specified directory that match the specified search pattern, using a
      /// value to determine whether to search subdirectories. The search will be performed as part of a transaction.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of files in path. The parameter cannot
      /// end in two periods ("..") or contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="Path.GetInvalidPathChars"/>.</param>
      /// <param name="searchOption">One of the <see cref="System.IO.SearchOption"/> values that specifies whether the
      /// search operation should include all subdirectories or only the current directory.</param>
      /// <returns>
      /// A String array containing the names of files in the specified
      /// directory that match the specified search pattern. File names include the full path.
      /// </returns>
      /// <remarks>This method may consume a lot of memory if a large tree of files, directories and subdirectories
      /// are searched. Consider using <see cref="FileSystemEntryEnumerator"/> instead if possible.</remarks>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string[] GetFiles(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         if (transaction == null)
            throw new ArgumentNullException("transaction");

		 return GetFilesInternal(transaction, path, searchPattern, searchOption);
      }

      #endregion

	  private static string[] GetFilesInternal(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
	  {
		  List<string> entries = new List<string>(20);

          foreach (FileSystemEntryInfo entry in GetFullFileSystemEntries(transaction, path, searchPattern, searchOption, false, null, null))
          {
			  // if it is not directory add to the list
			  if (!entry.IsDirectory)
				  entries.Add(entry.FullPath);
		  }

		  return entries.ToArray();
	  }

      #endregion

      #region GetDirectoryRoot

      /// <summary>
      /// Returns the volume information, root information, or both for the specified path.
      /// </summary>
      /// <param name="path">The path of a file or directory. </param>
      /// <returns>A string containing the volume information, root information, or both for the specified path.</returns>
      public static string GetDirectoryRoot(string path)
      {
         return new PathInfo(new PathInfo(path).GetFullPath()).Root;
      }

      #endregion

      #region GetFileSystemEntries

	  #region Non-transacted
	  /// <summary>
	  /// Returns the names of all files and subdirectories in the specified directory.
	  /// </summary>
	  /// <param name="path">The directory for which file and subdirectory names are returned.</param>
	  /// <returns>
	  /// A String array containing the names of file system entries in the specified directory.
	  /// </returns>
	  public static string[] GetFileSystemEntries(string path)
	  {
		  return GetFileSystemEntries(path, "*");
	  }

      /// <summary>
      /// Returns an array of file system entries matching the specified search criteria.
      /// </summary>
      /// <param name="path">The directory for which file and subdirectory names are returned.</param>
      /// <param name="searchPattern">The search string to match against the names of files in path. 
      /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or 
      /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or 
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in 
      /// <see cref="Path.GetInvalidPathChars"/>.</param>
      /// <returns>
      /// A String array containing the names of file system entries matching the specified pattern 
      /// in the specified directory.
      /// </returns>
      public static string[] GetFileSystemEntries(string path, string searchPattern)
      {
         return GetFileSystemEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
	  }

	  /// <summary>
	  /// Returns an array of file system entries matching the specified search criteria.
	  /// </summary>
	  /// <param name="path">The directory for which file and subdirectory names are returned.</param>
	  /// <param name="searchPattern">The search string to match against the names of files in path.
	  /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or
	  /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in
	  /// <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <param name="searchOption">The search option.</param>
	  /// <returns>
	  /// A String array containing the names of file system entries matching the specified pattern
	  /// in the specified directory.
	  /// </returns>
	  public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
	  {
		  return GetFileSystemEntriesInternal(null, path, searchPattern, searchOption);
	  }

	  #endregion

	  #region Transacted

	  /// <summary>
	  /// Returns the names of all files and subdirectories in the specified directory.
	  /// The file system entries are retrieved as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory for which file and subdirectory names are returned.</param>
	  /// <returns>
	  /// A String array containing the names of file system entries in the specified directory.
	  /// </returns>
	  public static string[] GetFileSystemEntries(KernelTransaction transaction, string path)
	  {
		  return GetFileSystemEntries(transaction, path, "*");
	  }

	  /// <summary>
	  /// Returns the names of all files and subdirectories in the specified directory.
	  /// The file system entries are retrieved as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory for which file and subdirectory names are returned.</param>
	  /// <param name="searchPattern">The search pattern.</param>
	  /// <returns>
	  /// A String array containing the names of file system entries in the specified directory.
	  /// </returns>
	  public static string[] GetFileSystemEntries(KernelTransaction transaction, string path, string searchPattern)
	  {
		  return GetFileSystemEntries(transaction, path, searchPattern, SearchOption.TopDirectoryOnly);
	  }

	  /// <summary>
	  /// Returns an array of file system entries matching the specified search criteria.
	  /// The file system entries are retrieved as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="path">The directory for which file and subdirectory names are returned.</param>
	  /// <param name="searchPattern">The search string to match against the names of files in path.
	  /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or
	  /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in
	  /// <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <param name="searchOption">The search option.</param>
	  /// <returns>
	  /// A String array containing the names of file system entries matching the specified pattern
	  /// in the specified directory.
	  /// </returns>
      public static string[] GetFileSystemEntries(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
	  	if (transaction == null) {
	  		throw new ArgumentNullException("transaction");
	  	}

	  	return GetFileSystemEntriesInternal(transaction, path, searchPattern, searchOption);
      }

	  #endregion

	  private static string[] GetFileSystemEntriesInternal(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
	  {
		  List<string> entries = new List<string>(20);

		  foreach (FileSystemEntryInfo entry in GetFullFileSystemEntries(transaction, path, searchPattern, searchOption, false, null, null)) {
			  entries.Add(entry.FullPath);
		  }

		  return entries.ToArray();
	  }

	  #endregion

	  #region GetFullFileSystemEntries

 	  #region Non-transacted

      /// <overloads>
	  /// Enumerates file system entries contained in a specified directory as <see cref="FileSystemEntryInfo"/> instances.
      /// </overloads>
      /// <summary>
      /// Enumerates all file system entries as <see cref="FileSystemEntryInfo"/> instances 
	  /// in the specified <paramref name="directory"/>.
      /// </summary>
      /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
	  /// <param name="directory">The directory path containing the file system entries to enumerate.</param>
	  /// <returns>An enumerable containing the file system entries for the specified <paramref name="directory"/></returns>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(string directory)
      {
		  return GetFullFileSystemEntries(directory, "*");
      }

      /// <summary>
	  /// Enumerates the file system entries in the specified <paramref name="directory"/> matching 
      /// the specified <paramref name="searchPattern"/> as <see cref="FileSystemEntryInfo"/> instances.
      /// </summary>
      /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
	  /// <param name="directory">The directory or path, and the file name, which can include 
      /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
      /// <param name="searchPattern">The search string to match against the names of files in path.
      /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or
      /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in
      /// <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <returns>An enumerable containing the file system entries for the specified <paramref name="directory"/></returns>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(string directory, string searchPattern)
      {
		  return GetFullFileSystemEntries(directory, searchPattern, SearchOption.TopDirectoryOnly);
      }

	  /// <summary>
	  /// Enumerates all file system entries as <see cref="FileSystemEntryInfo"/> instances
	  /// in the specified <paramref name="directory"/>, optionally enumerating
	  /// directories only.
	  /// </summary>
	  /// <param name="directory">The directory or path, and the file name, which can include
	  /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
	  /// <param name="searchPattern">The search pattern.</param>
	  /// <param name="searchOption">The search option.</param>
	  /// <returns>
	  /// An enumerable containing the file system entries for the specified <paramref name="directory"/>
	  /// </returns>
	  /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(string directory, string searchPattern, SearchOption searchOption)
	  {
          return GetFullFileSystemEntries(directory, searchPattern, searchOption, false);
	  }

	  /// <summary>
	  /// Enumerates the file system entries in the specified <paramref name="directory"/> matching
	  /// the specified <paramref name="searchPattern"/> as <see cref="FileSystemEntryInfo"/> instances,
	  /// optionally enumerating directories only.
	  /// </summary>
	  /// <param name="directory">The directory or path, and the file name, which can include
	  /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
	  /// <param name="searchPattern">The search string to match against the names of files in path.
	  /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or
	  /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in
	  /// <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <param name="directoriesOnly">if set to <c>true</c> enumerate only directories.</param>
	  /// <param name="searchOption">The search options. <see cref="System.IO.SearchOption"/></param>
	  /// <returns>
	  /// An enumerable containing the file system entries for the specified <paramref name="directory"/>
	  /// </returns>
	  /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(string directory, string searchPattern, SearchOption searchOption, bool directoriesOnly)
      {
		  return GetFullFileSystemEntries(null, directory, searchPattern, searchOption, directoriesOnly, null, null);
      }

   	#endregion

      #region Transacted

	  /// <summary>
	  /// Enumerates all file system entries as <see cref="FileSystemEntryInfo"/> instances 
	  /// in the specified <paramref name="directory"/> as part of a transaction.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="directory">The directory or path containing the file system entries to enumerate.</param>
	  /// <returns>
	  /// An enumerable containing the file system entries for the specified <paramref name="directory"/>
	  /// </returns>
	  /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(KernelTransaction transaction, string directory)
      {
		  return GetFullFileSystemEntries(transaction, directory, "*");
      }

      /// <summary>
	  /// Enumerates the file system entries in the specified <paramref name="directory"/> matching 
      /// the specified <paramref name="searchPattern"/> as <see cref="FileSystemEntryInfo"/> instances as part 
      /// of a transaction.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
	  /// <param name="directory">The directory or path, and the file name, which can include
      /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
      /// <param name="searchPattern">The search string to match against the names of files in path.
      /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or
      /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
      /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in
      /// <see cref="Path.GetInvalidPathChars"/>.</param>
      /// <returns>
	  /// An enumerable containing the file system entries for the specified <paramref name="directory"/>
      /// </returns>
      /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(KernelTransaction transaction, string directory, string searchPattern)
      {
		  return GetFullFileSystemEntries(transaction, directory, searchPattern, SearchOption.TopDirectoryOnly);
      }

	  /// <summary>
	  /// Enumerates all file system entries as <see cref="FileSystemEntryInfo"/> instances
	  /// in the specified <paramref name="directory"/> as part of a transaction, optionally enumerating
	  /// directories only.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="directory">The directory path</param>
	  /// <param name="searchPattern">The search pattern.</param>
	  /// <param name="searchOption">The search option.</param>
	  /// <returns>
	  /// An enumerable containing the file system entries for the specified <paramref name="directory"/>
	  /// </returns>
	  /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(KernelTransaction transaction, string directory, string searchPattern, SearchOption searchOption)
      {
		  return GetFullFileSystemEntries(transaction, directory, searchPattern, searchOption, false);
      }

	  /// <summary>
	  /// Enumerates the file system entries in the specified <paramref name="directory"/> matching
	  /// the specified <paramref name="searchPattern"/> as <see cref="FileSystemEntryInfo"/> instances as part
	  /// of a transaction, optionally enumerating directories only.
	  /// </summary>
	  /// <param name="transaction">The transaction.</param>
	  /// <param name="directory">The directory or path, and the file name, which can include
	  /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
	  /// <param name="searchPattern">The search string to match against the names of files in path.
	  /// The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or
	  /// contain two periods ("..") followed by <see cref="Path.DirectorySeparatorChar"/> or
	  /// <see cref="Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in
	  /// <see cref="Path.GetInvalidPathChars"/>.</param>
	  /// <param name="directoriesOnly">if set to <c>true</c> enumerate only directories.</param>
	  /// <param name="searchOption">The search option. <see cref="System.IO.SearchOption"/></param>
	  /// <returns>
	  /// An enumerable containing the file system entries for the specified <paramref name="directory"/>
	  /// </returns>
	  /// <remarks>This is a convenience method for using the <see cref="FileSystemEntryEnumerator"/> for enumeration.</remarks>
      public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(KernelTransaction transaction, string directory, string searchPattern, SearchOption searchOption, bool directoriesOnly)
      {
		  if (transaction == null)
			  throw new ArgumentNullException("transaction");

	  	return GetFullFileSystemEntries(transaction, directory, searchPattern, searchOption, directoriesOnly, null, null);
      }

	  #endregion

      /// <summary>
      /// Enumerates the file system entries in the specified <paramref name="directory"/> matching entries
      /// with the <paramref name="searchPattern"/> as <see cref="FileSystemEntryInfo"/> instances. This method gives more control over
      /// the progress and exception handling.
      /// </summary>
      /// <param name="transaction">Optional kernel transaction object. Pass <c>NULL</c> for non-transacted call.</param>
      /// <param name="directory">The directory path where to start enumeration. Use <see cref="Path.GetLongPath"/> to get beyond 254 chars limit.</param>
      /// <param name="searchPattern">The search pattern prefix, wildcards, or a mixture.</param>
      /// <param name="searchOption">Whether to search files in the specified directory only, or all subdirectories recursively.</param>
      /// <param name="directoriesOnly">if set to <c>true</c> [directories only].</param>
      /// <param name="handler">Optional enumeration exception handler. You can subscribe to exceptions and make decisions about them on the fly.</param>
      /// <param name="suppressedExceptions">The list of exceptions to skip. Useful when dealing with protected directories.</param>
      /// <returns></returns>
	  public static IEnumerable<FileSystemEntryInfo> GetFullFileSystemEntries(
          KernelTransaction transaction,
          string directory,
          string searchPattern,
          SearchOption searchOption,
          bool directoriesOnly,
          DirectoryEnumerationExceptionHandler handler,
          IList<Type> suppressedExceptions)
	  {
		  if (directory == null)
			  throw new ArgumentNullException("directory");

		  if (searchPattern == null)
			  throw new ArgumentNullException("searchPattern");

		  // stack is used to perform safer than recursion in case of thousands nested directories if using with long path extension
		  // theoretical limit is millions of nested folders :)
		  Stack<string> dirs = new Stack<string>();
		  dirs.Push(directory);
          // making local copy
          List<Type> exceptionsToSkip = new List<Type>();
          if (suppressedExceptions != null)
              exceptionsToSkip.AddRange(suppressedExceptions);

		  while (dirs.Count > 0) {
			  string tmpDir = dirs.Pop();
                using (FileSystemEntryEnumerator entryEnumerator = new FileSystemEntryEnumerator(transaction, Path.Combine(tmpDir, searchPattern), directoriesOnly))
                  {
                    // the following block might look messy but it is becuase of multiple restrictions of yield statemens
                    // see http://msdn.microsoft.com/en-us/library/9k7k7cf0(VS.90).aspx?appId=Dev10IDEF1&l=EN-US&k=k(YIELD_CSHARPKEYWORD);k(TargetFrameworkMoniker-%22.NETFRAMEWORK&k=VERSION=V2.0%22);k(DevLang-CSHARP)&rd=true
                      for(;;)
                      {
                          try
                          {
                              if (!entryEnumerator.MoveNext())
                                  break;
                          }
                          catch (Exception e)
                          {
                              Type exType = e.GetType();
                              if (exceptionsToSkip.Exists(new Predicate<Type>(delegate(Type mytype) { return mytype.Equals(exType); })))
                                  break;
                              else if (handler != null)
                              {
                                  EnumerationExceptionDecision answer = handler(tmpDir, e);

                                  if (answer == EnumerationExceptionDecision.Suppress)
                                  {
                                      exceptionsToSkip.Add(exType);
                                  }
                                  else if (answer == EnumerationExceptionDecision.Retry)
                                  {
                                      continue; // let's try one more time :)
                                  }
                                  else if (answer == EnumerationExceptionDecision.Skip)
                                  {
                                      break; // advance to the next dir on the stack
                                  }
                              }
                              yield break; // stop and return from the method
                          }

                          string fullPath = Path.Combine(tmpDir, entryEnumerator.Current.FileName);
                          entryEnumerator.Current.FullPath = fullPath;
                          if (searchOption == SearchOption.AllDirectories)
                          {
                              if (entryEnumerator.Current.IsDirectory)
                              {
                                  dirs.Push(fullPath);
                              }
                          }
                          yield return entryEnumerator.Current;
                      }
                  }
		  }
	  }

      #endregion

      #region GetParent

      /// <summary>
      /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
      /// </summary>
      /// <param name="path">The path for which to retrieve the parent directory.</param>
      /// <returns>The parent directory or a <see langword="null"/> reference if the path is the root.</returns>
      public static string GetParent(string path)
      {
         PathInfo p = new PathInfo(path).Parent;
         return p == null ? null : p.Path;
      }

      #endregion

      #region GetProperties

	  /// <summary>
	  /// Gets the properties of the particular folder without following any symbolic links or mount points.
	  /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
	  /// Plus additional ones: Total, File, Size, Error
	  /// <para><b>Total:</b> is the total number of enumerated objects.</para>
	  /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
	  /// <para><b>Size:</b> is the total size of enumerated objects.</para>
	  /// <para><b>Error:</b> is the total number of errors encountered during request.</para>
	  /// </summary>
	  /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction. For a non transacted operation pass <c>NULL</c>.</param>
	  /// <param name="directory">The target directory.</param>
	  /// <param name="searchOption">The search option. Either top level or subfolders too.</param>
	  /// <param name="continueOnAccessErrors">if set to <c>true</c> continue on <see cref="System.UnauthorizedAccessException"/> errors.</param>
	  /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string directory, SearchOption searchOption, bool continueOnAccessErrors)
	  {
		  const string FILE = "File";
		  const string TOTAL = "Total";
		  const string ERROR = "Error";
		  const string SIZE = "Size";

		  Dictionary<string, long> props = new Dictionary<string, long>();

		  foreach (string name in Enum.GetNames(typeof(FileAttributes))) {
			  props.Add(name, 0);
		  }

		  long total = 0;
		  long errors = 0;
		  long size = 0;

		  Array attributes = Enum.GetValues(typeof(FileAttributes));

          foreach (FileSystemEntryInfo entry in GetFullFileSystemEntries(transaction, directory, "*", searchOption, false,
              delegate(string path, Exception e)
              {
                  errors++;
                  if (continueOnAccessErrors)
                      return EnumerationExceptionDecision.Skip;
                  else
                      return EnumerationExceptionDecision.Abort;
              },
              null))
          {
              total++;
              size += entry.FileSize;

              foreach (var attributeMarker in attributes)
              {
                  FileAttributes attribute = (FileAttributes)attributeMarker;

                  if ((entry.Attributes & attribute) != 0) // marker exists in flags
                  {
                      // marker is dir flag
                      if ((attribute & FileAttributes.Directory) != 0)
                      {
                          // skip on reparse points here to cleanly separate regular dirs from links
                          if (entry.IsReparsePoint)
                              continue;

                          // regular dir that will go to stack
                          props[FileAttributes.Directory.ToString()]++; // adding dir flag ++

                      }
                      else
                      {
                          props[attribute.ToString()]++;
                      }
                  }
              }
          }

          props.Add(FILE, 0);
          props.Add(SIZE, 0);
          props.Add(TOTAL, 0);
          props.Add(ERROR, 0);

          props[TOTAL] = total;
          props[SIZE] = size;
          props[ERROR] = errors;

          // adjusting regular files count
          props[FILE] = total - props[FileAttributes.Directory.ToString()] -
                         props[FileAttributes.ReparsePoint.ToString()];

          return props;
      }

      #endregion

      #region Encryption

      /// <summary>
      /// Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory. 
      /// </summary>
      /// <param name="directory">The name of the directory for which to enable encryption.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void EnableEncryption(string directory)
      {
         if (!NativeMethods.EncryptionDisable(directory, false))
            NativeError.ThrowException(directory, directory);
      }

      /// <summary>
      /// Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory. 
      /// </summary>
      /// <param name="directory">The name of the directory for which to disable encryption.</param>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void DisableEncryption(string directory)
      {
         if (!NativeMethods.EncryptionDisable(directory, true))
            NativeError.ThrowException(directory, directory);
      }

      #endregion

      #region GetFileIdBothDirectoryInfo

      private static readonly long fileNameOffset = Marshal.OffsetOf(typeof(NativeMethods.FILE_ID_BOTH_DIR_INFO), "FileName").ToInt64();

      /// <summary>
      /// Retrieves information about files in the directory specified by <paramref name="directoryPath"/>.
      /// </summary>
      /// <remarks>
      /// <para>
      /// No specific access rights is required to query this information. 
      /// </para>
      /// <para>
      /// File reference numbers, also called file IDs, are guaranteed to be unique only within a static file system. 
      /// They are not guaranteed to be unique over time, because file systems are free to reuse them. Nor are they guaranteed to remain constant. 
      /// For example, the FAT file system generates the file reference number for a file from the byte offset of the file's directory entry record 
      /// (DIRENT) on the disk. Defragmentation can change this byte offset. Thus a FAT file reference number can change over time.
      /// </para>
      /// <para>
      /// <b>Requires Windows Vista or Windows Server 2008 or later.</b>
      /// </para>
      /// </remarks>
      /// <param name="directoryPath">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      public static IEnumerable<FileIdBothDirectoryInfo> GetFileIdBothDirectoryInfo(string directoryPath)
      {
         return GetFileIdBothDirectoryInfo(directoryPath, FileShare.ReadWrite);
      }

      /// <summary>
	  /// Retrieves information about files in the directory specified by <paramref name="path"/> using the specified <paramref name="transaction"/>
      /// </summary>
      /// <remarks>
      /// <para>
      /// No specific access rights is required to query this information. 
      /// </para>
      /// <para>
      /// File reference numbers, also called file IDs, are guaranteed to be unique only within a static file system. 
      /// They are not guaranteed to be unique over time, because file systems are free to reuse them. Nor are they guaranteed to remain constant. 
      /// For example, the FAT file system generates the file reference number for a file from the byte offset of the file's directory entry record 
      /// (DIRENT) on the disk. Defragmentation can change this byte offset. Thus a FAT file reference number can change over time.
      /// </para>
      /// <para>
      /// <b>Requires Windows Vista or Windows Server 2008 or later.</b>
      /// </para>
      /// </remarks>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="transaction">The transaction to use for this operation.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      public static IEnumerable<FileIdBothDirectoryInfo> GetFileIdBothDirectoryInfo(KernelTransaction transaction, string path)
      {
         return GetFileIdBothDirectoryInfo(transaction, path, FileShare.ReadWrite);
      }

      /// <summary>
      /// Retrieves information about files in the directory specified by <paramref name="path"/> using the specified <paramref name="transaction"/> and
      /// share mode.
      /// </summary>
      /// <remarks>
      /// <para>
      /// No specific access rights is required to query this information. 
      /// </para>
      /// <para>
      /// File reference numbers, also called file IDs, are guaranteed to be unique only within a static file system. 
      /// They are not guaranteed to be unique over time, because file systems are free to reuse them. Nor are they guaranteed to remain constant. 
      /// For example, the FAT file system generates the file reference number for a file from the byte offset of the file's directory entry record 
      /// (DIRENT) on the disk. Defragmentation can change this byte offset. Thus a FAT file reference number can change over time.
      /// </para>
      /// <para>
      /// <b>Requires Windows Vista or Windows Server 2008 or later.</b>
      /// </para>
      /// </remarks>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="transaction">The transaction to use for this operation.</param>
      /// <param name="shareMode">The sharing mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      public static IEnumerable<FileIdBothDirectoryInfo> GetFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode)
      {
         using (SafeFileHandle handle = File.CreateInternal(transaction, path, FileMode.Open, FileSystemRights.ListDirectory, shareMode, FileOptions.BackupSemantics, null))
         {
            // We need to loop through the results yielding back each one to prevent the handle from going out of scope.
            foreach (var info in GetFileIdBothDirectoryInfo(handle))
               yield return info;
         }
      }

      /// <summary>
      /// Retrieves information about files in the directory specified by <paramref name="path"/> using the specified
      /// share mode.
      /// </summary>
      /// <remarks>
      /// <para>
      /// No specific access rights is required to query this information. 
      /// </para>
      /// <para>
      /// File reference numbers, also called file IDs, are guaranteed to be unique only within a static file system. 
      /// They are not guaranteed to be unique over time, because file systems are free to reuse them. Nor are they guaranteed to remain constant. 
      /// For example, the FAT file system generates the file reference number for a file from the byte offset of the file's directory entry record 
      /// (DIRENT) on the disk. Defragmentation can change this byte offset. Thus a FAT file reference number can change over time.
      /// </para>
      /// <para>
      /// <b>Requires Windows Vista or Windows Server 2008 or later.</b>
      /// </para>
      /// </remarks>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The sharing mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      public static IEnumerable<FileIdBothDirectoryInfo> GetFileIdBothDirectoryInfo(string path, FileShare shareMode)
      {
         using (SafeFileHandle handle = File.CreateInternal(path, FileMode.Open, FileSystemRights.ListDirectory, shareMode, FileOptions.BackupSemantics, null))
         {
            // We need to loop through the results yielding back each one to prevent the handle from going out of scope.
            foreach (var info in GetFileIdBothDirectoryInfo(handle))
               yield return info;
         }
      }

      /// <summary>
      /// Retrieves information about files in the directory handle specified.
      /// </summary>
      /// <remarks>
      /// <para>
      /// No specific access rights is required to query this information. 
      /// </para>
      /// <para>
      /// File reference numbers, also called file IDs, are guaranteed to be unique only within a static file system. 
      /// They are not guaranteed to be unique over time, because file systems are free to reuse them. Nor are they guaranteed to remain constant. 
      /// For example, the FAT file system generates the file reference number for a file from the byte offset of the file's directory entry record 
      /// (DIRENT) on the disk. Defragmentation can change this byte offset. Thus a FAT file reference number can change over time.
      /// </para>
      /// <para>
      /// <b>Requires Windows Vista or Windows Server 2008 or later.</b>
      /// </para>
      /// </remarks>
      /// <param name="fileHandle">An open handle to the directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    
      public static IEnumerable<FileIdBothDirectoryInfo> GetFileIdBothDirectoryInfo(SafeFileHandle fileHandle)
      {
         const int bufferSize = 16384;

         using (SafeGlobalMemoryBufferHandle hBuf = new SafeGlobalMemoryBufferHandle(bufferSize))
         {
            while (NativeMethods.GetFileInformationByHandleEx(fileHandle, NativeMethods.FileInformationClass.FileIdBothDirectoryInfo, hBuf, bufferSize))
            {
               IntPtr buf = hBuf.DangerousGetHandle();
               while (buf != IntPtr.Zero)
               {
                  NativeMethods.FILE_ID_BOTH_DIR_INFO fdi = (NativeMethods.FILE_ID_BOTH_DIR_INFO)Marshal.PtrToStructure(buf, typeof(NativeMethods.FILE_ID_BOTH_DIR_INFO));
                  string fileName = Marshal.PtrToStringUni(new IntPtr(fileNameOffset + buf.ToInt64()), fdi.FileNameLength / 2);

                  yield return new FileIdBothDirectoryInfo(fdi, fileName);

                  if (fdi.NextEntryOffset != 0)
                     buf = new IntPtr(buf.ToInt64() + fdi.NextEntryOffset);
                  else
                     buf = IntPtr.Zero;
               }
            }

            int lastErrorCode = Marshal.GetLastWin32Error();
            if (lastErrorCode == Win32Errors.ERROR_NO_MORE_FILES || lastErrorCode == Win32Errors.ERROR_SUCCESS)
               yield break;
            else
               Marshal.ThrowExceptionForHR(lastErrorCode);
         }

      }

      #endregion

   }
}

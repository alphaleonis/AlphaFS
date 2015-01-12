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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>
      ///   Creates a new directory.
      ///   <para>If the underlying file system supports security on files and directories,</para>
      ///   <para>the function applies a specified security descriptor to the new directory.</para>
      /// </summary>
      /// <remarks>
      ///   <para>Some file systems, such as the NTFS file system, support compression or encryption for individual files and
      ///   directories.</para>
      ///   <para>On volumes formatted for such a file system, a new directory inherits the compression and encryption attributes of its parent
      ///   directory.</para>
      ///   <para>An application can obtain a handle to a directory by calling <see cref="CreateFile"/> with the FILE_FLAG_BACKUP_SEMANTICS
      ///   flag set.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</para>
      /// </remarks>
      /// <param name="lpPathName">Full pathname of the file.</param>
      /// <param name="lpSecurityAttributes">The security attributes.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateDirectoryW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool CreateDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes);

      /// <summary>
      ///   Creates a new directory with the attributes of a specified template directory.
      ///   <para>If the underlying file system supports security on files and directories,</para>
      ///   <para>the function applies a specified security descriptor to the new directory.</para>
      ///   <para>The new directory retains the other attributes of the specified template directory.</para>
      /// </summary>
      /// <remarks>
      ///   <para>The CreateDirectoryEx function allows you to create directories that inherit stream information from other directories.</para>
      ///   <para>This function is useful, for example, when you are using Macintosh directories, which have a resource stream</para>
      ///   <para>that is needed to properly identify directory contents as an attribute.</para>
      ///   <para>Some file systems, such as the NTFS file system, support compression or encryption for individual files and
      ///   directories.</para>
      ///   <para>On volumes formatted for such a file system, a new directory inherits the compression and encryption attributes of its parent
      ///   directory.</para>
      ///   <para>You can obtain a handle to a directory by calling the <see cref="CreateFile"/> function with the FILE_FLAG_BACKUP_SEMANTICS
      ///   flag set.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      /// <param name="lpTemplateDirectory">Pathname of the template directory.</param>
      /// <param name="lpPathName">Full pathname of the file.</param>
      /// <param name="lpSecurityAttributes">The security attributes.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero (0). To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateDirectoryExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool CreateDirectoryEx([MarshalAs(UnmanagedType.LPWStr)] string lpTemplateDirectory, [MarshalAs(UnmanagedType.LPWStr)] string lpPathName, Security.NativeMethods.SecurityAttributes lpSecurityAttributes);

      /// <summary>
      ///   Creates a new directory as a transacted operation, with the attributes of a specified template directory.
      ///   <para>If the underlying file system supports security on files and directories,</para>
      ///   <para>the function applies a specified security descriptor to the new directory.</para>
      ///   <para>The new directory retains the other attributes of the specified template directory.</para>
      /// </summary>
      /// <remarks>
      ///   <para>The CreateDirectoryTransacted function allows you to create directories that inherit stream information from other
      ///   directories.</para>
      ///   <para>This function is useful, for example, when you are using Macintosh directories, which have a resource stream</para>
      ///   <para>that is needed to properly identify directory contents as an attribute.</para>
      ///   <para>Some file systems, such as the NTFS file system, support compression or encryption for individual files and
      ///   directories.</para>
      ///   <para>On volumes formatted for such a file system, a new directory inherits the compression and encryption attributes of its parent
      ///   directory.</para>
      ///   <para>You can obtain a handle to a directory by calling the <see cref="CreateFileTransacted"/> function with the
      ///   FILE_FLAG_BACKUP_SEMANTICS flag set.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      /// <param name="lpTemplateDirectory">Pathname of the template directory.</param>
      /// <param name="lpNewDirectory">Pathname of the new directory.</param>
      /// <param name="lpSecurityAttributes">The security attributes.</param>
      /// <param name="hTransaction">The transaction.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero (0). To get extended error information, call GetLastError.</para>
      ///   <para>This function fails with ERROR_EFS_NOT_ALLOWED_IN_TRANSACTION if you try to create a</para>
      ///   <para>child directory with a parent directory that has encryption disabled.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateDirectoryTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateDirectoryTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpTemplateDirectory, [MarshalAs(UnmanagedType.LPWStr)] string lpNewDirectory, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes, SafeHandle hTransaction);

      /// <summary>
      ///   Deletes an existing empty directory.
      /// </summary>
      /// <remarks>
      ///   <para>The RemoveDirectory function marks a directory for deletion on close.</para>
      ///   <para>Therefore, the directory is not removed until the last handle to the directory is closed.</para>
      ///   <para>RemoveDirectory removes a directory junction, even if the contents of the target are not empty;</para>
      ///   <para>the function removes directory junctions regardless of the state of the target object.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps | Windows Store apps]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps]</para>
      /// </remarks>
      /// <param name="lpPathName">Full pathname of the file.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RemoveDirectoryW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal extern static bool RemoveDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);

      /// <summary>
      ///   Deletes an existing empty directory as a transacted operation.
      /// </summary>
      /// <remarks>
      ///   <para>The RemoveDirectoryTransacted function marks a directory for deletion on close.</para>
      ///   <para>Therefore, the directory is not removed until the last handle to the directory is closed.</para>
      ///   <para>RemoveDirectory removes a directory junction, even if the contents of the target are not empty;</para>
      ///   <para>the function removes directory junctions regardless of the state of the target object.</para>
      ///   <para>Minimum supported client: Windows Vista [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2008 [desktop apps only]</para>
      /// </remarks>
      /// <param name="lpPathName">Full pathname of the file.</param>
      /// <param name="hTransaction">The transaction.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RemoveDirectoryTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool RemoveDirectoryTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpPathName, SafeHandle hTransaction);
   }
}

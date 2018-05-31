/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.IO;
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>[AlphaFS] Creates an NTFS directory junction(similar to CMD command: "MKLINK /J")
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      [SecurityCritical]
      public static void CreateJunction(string junctionPath, string directoryPath)
      {
         CreateJunctionCore(null, junctionPath, directoryPath, false, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J").
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CreateJunction(string junctionPath, string directoryPath, PathFormat pathFormat)
      {
         CreateJunctionCore(null, junctionPath, directoryPath, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      [SecurityCritical]
      public static void CreateJunction(string junctionPath, string directoryPath, bool overwrite)
      {
         CreateJunctionCore(null, junctionPath, directoryPath, overwrite, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CreateJunction(string junctionPath, string directoryPath, bool overwrite, PathFormat pathFormat)
      {
         CreateJunctionCore(null, junctionPath, directoryPath, overwrite, false, pathFormat);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="copyTargetTimestamps"><see langword="true"/> to copy the target date and time stamps to the directory junction.</param>
      [SecurityCritical]
      public static void CreateJunction(string junctionPath, string directoryPath, bool overwrite, bool copyTargetTimestamps)
      {
         CreateJunctionCore(null, junctionPath, directoryPath, overwrite, copyTargetTimestamps, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="copyTargetTimestamps"><see langword="true"/> to copy the target date and time stamps to the directory junction.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CreateJunction(string junctionPath, string directoryPath, bool overwrite, bool copyTargetTimestamps, PathFormat pathFormat)
      {
         CreateJunctionCore(null, junctionPath, directoryPath, overwrite, copyTargetTimestamps, pathFormat);
      }




      /// <summary>[AlphaFS] Creates an NTFS directory junction(similar to CMD command: "MKLINK /J")
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      [SecurityCritical]
      public static void CreateJunction(KernelTransaction transaction, string junctionPath, string directoryPath)
      {
         CreateJunctionCore(transaction, junctionPath, directoryPath, false, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J").
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CreateJunction(KernelTransaction transaction, string junctionPath, string directoryPath, PathFormat pathFormat)
      {
         CreateJunctionCore(transaction, junctionPath, directoryPath, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      [SecurityCritical]
      public static void CreateJunction(KernelTransaction transaction, string junctionPath, string directoryPath, bool overwrite)
      {
         CreateJunctionCore(transaction, junctionPath, directoryPath, overwrite, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CreateJunction(KernelTransaction transaction, string junctionPath, string directoryPath, bool overwrite, PathFormat pathFormat)
      {
         CreateJunctionCore(transaction, junctionPath, directoryPath, overwrite, false, pathFormat);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="copyTargetTimestamps"><see langword="true"/> to copy the target date and time stamps to the directory junction.</param>
      [SecurityCritical]
      public static void CreateJunction(KernelTransaction transaction, string junctionPath, string directoryPath, bool overwrite, bool copyTargetTimestamps)
      {
         CreateJunctionCore(transaction, junctionPath, directoryPath, overwrite, copyTargetTimestamps, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="copyTargetTimestamps"><see langword="true"/> to copy the target date and time stamps to the directory junction.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CreateJunction(KernelTransaction transaction, string junctionPath, string directoryPath, bool overwrite, bool copyTargetTimestamps, PathFormat pathFormat)
      {
         CreateJunctionCore(transaction, junctionPath, directoryPath, overwrite, copyTargetTimestamps, pathFormat);
      }




      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J"). Overwriting a junction point of the same name is allowed.
      /// <returns>Returns the long path to the directory junction.</returns>
      /// <remarks>
      /// <para>&#160;</para>
      /// <para>The directory must be empty and reside on a local volume.</para>
      /// <para>The directory date and time stamps from <paramref name="directoryPath"/> (the target) are copied to the directory junction.</para>
      /// <para></para>
      /// <para></para>
      /// <para>&#160;</para>
      /// <para>MSDN: A junction (also called a soft link) differs from a hard link in that the storage objects it references are separate directories,</para>
      /// <para>and a junction can link directories located on different local volumes on the same computer.</para>
      /// <para>Otherwise, junctions operate identically to hard links. Junctions are implemented through reparse points.</para>
      /// </remarks></summary>
      /// <exception cref="AlreadyExistsException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="junctionPath">The path of the junction point to create.</param>
      /// <param name="directoryPath">The path to the directory. If the directory does not exist it will be created.</param>
      /// <param name="overwrite"><see langword="true"/> to overwrite an existing junction point. The directory is removed and recreated.</param>
      /// <param name="copyTargetTimestamps"><see langword="true"/> to copy the target date and time stamps to the directory junction.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static string CreateJunctionCore(KernelTransaction transaction, string junctionPath, string directoryPath, bool overwrite, bool copyTargetTimestamps, PathFormat pathFormat)
      {
         if (pathFormat != PathFormat.LongFullPath)
         {
            Path.CheckSupportedPathFormat(directoryPath, true, true);
            Path.CheckSupportedPathFormat(junctionPath, true, true);

            directoryPath = Path.GetExtendedLengthPathCore(transaction, directoryPath, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);
            junctionPath = Path.GetExtendedLengthPathCore(transaction, junctionPath, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);

            pathFormat = PathFormat.LongFullPath;
         }


         // Directory Junction logic.


         // Check if drive letter is a mapped network drive.
         if (new DriveInfo(directoryPath).IsUnc)
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.Network_Path_Not_Allowed, directoryPath), "directoryPath");

         if (new DriveInfo(junctionPath).IsUnc)
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.Network_Path_Not_Allowed, junctionPath), "junctionPath");


         // Check for existing file.
         File.ThrowIOExceptionIfFsoExist(transaction, false, directoryPath, pathFormat);
         File.ThrowIOExceptionIfFsoExist(transaction, false, junctionPath, pathFormat);


         // Check for existing directory junction folder.
         if (File.ExistsCore(transaction, true, junctionPath, pathFormat))
         {
            if (overwrite)
            {
               DeleteDirectoryCore(transaction, null, junctionPath, true, true, true, pathFormat);
               CreateDirectoryCore(transaction, junctionPath, null, null, false, pathFormat);
            }

            else
            {
               // Ensure the folder is empty.
               if (!IsEmptyCore(transaction, junctionPath, pathFormat))
                  throw new DirectoryNotEmptyException(junctionPath, true);

               throw new AlreadyExistsException(junctionPath, true);
            }
         }


         // Create the folder and convert it to a directory junction.
         CreateDirectoryCore(transaction, junctionPath, null, null, false, pathFormat);

         using (var safeHandle = OpenDirectoryJunction(transaction, junctionPath, pathFormat))
            Device.CreateDirectoryJunction(safeHandle, directoryPath);


         // Copy the target date and time stamps to the directory junction.
         if (copyTargetTimestamps)
            File.CopyTimestampsCore(transaction, directoryPath, junctionPath, true, pathFormat);


         return junctionPath;
      }


      private static SafeFileHandle OpenDirectoryJunction(KernelTransaction transaction, string junctionPath, PathFormat pathFormat)
      {
         return File.CreateFileCore(transaction, junctionPath, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.OpenReparsePoint, null, FileMode.Open, FileSystemRights.WriteData, FileShare.ReadWrite, false, false, pathFormat);
      }
   }
}

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
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Defines the access rights to use when creating access and audit rules. 
	/// </summary>
	/// <remarks>
	///		This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.
	///	</remarks>	
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags]
    public enum FileSystemRights : uint
    {
        /// <summary>
        /// Specifies the right to open and copy folders or files as read-only. This right includes the 
        /// <see cref="ReadData"/> right, <see cref="ReadExtendedAttributes"/> right, <see cref="ReadAttributes"/> right, 
        /// and <see cref="ReadPermissions"/> right. 
        /// </summary>
        Read = NativeMethods.FILE_GENERIC_READ,
        /// <summary>
        /// Specifies the right to create folders and files, and to add or remove data from files. This right includes the 
        /// <see cref="WriteData"/> right, <see cref="AppendData"/> right, <see cref="WriteExtendedAttributes"/> right, and <see cref="WriteAttributes"/> right. 
        /// </summary>
        Write = NativeMethods.FILE_GENERIC_WRITE,
        /// <summary>
        /// Specifies the right to open and copy folders or files as read-only, and to run application files. 
        /// This right includes the <see cref="Read"/> right and the <see cref="ExecuteFile"/> right. 
        /// </summary>
        ReadAndExecute = NativeMethods.FILE_GENERIC_EXECUTE,
        /// <summary>
        /// Specifies the right to read, write, list folder contents, delete folders and files, and run application files. 
        /// This right includes the <see cref="ReadAndExecute"/> right, the <see cref="Write"/> right, and the <see cref="Delete"/> right. 
        /// </summary>
        Modify = FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.Delete,
        /// <summary>
        /// Specifies the right to open and copy a file or folder. This does not include the right to read file system attributes, extended file system attributes, or access and audit rules. 
        /// </summary>
        ReadData = NativeMethods.FILE_READ_DATA,
        /// <summary>
        /// Specifies the right to read the contents of a directory. 
        /// </summary>
        ListDirectory = NativeMethods.FILE_LIST_DIRECTORY,
        /// <summary>
        /// Specifies the right to open and write to a file or folder. This does not include the right to open and write file system attributes, extended file system attributes, or access and audit rules. 
        /// </summary>
        WriteData = NativeMethods.FILE_WRITE_DATA,
        /// <summary>
        /// Specifies the right to create a file. 
        /// </summary>
        CreateFiles = NativeMethods.FILE_ADD_FILE,
        /// <summary>
        /// Specifies the right to append data to the end of a file. 
        /// </summary>
        AppendData = NativeMethods.FILE_APPEND_DATA,
        /// <summary>
        /// Specifies the right to create a folder. 
        /// </summary>
        CreateDirectories = NativeMethods.FILE_ADD_SUBDIRECTORY,
        /// <summary>
        /// Specifies the right to open and copy extended file system attributes from a folder or file. For example, this value specifies the right to view author and content information. This does not include the right to read data, file system attributes, or access and audit rules. 
        /// </summary>
        ReadExtendedAttributes = NativeMethods.FILE_READ_EA,
        /// <summary>
        /// Specifies the right to open and write extended file system attributes to a folder or file. This does not include the ability to write data, attributes, or access and audit rules. 
        /// </summary>
        WriteExtendedAttributes = NativeMethods.FILE_WRITE_EA,
        /// <summary>
        /// Specifies the right to run an application file. 
        /// </summary>
        ExecuteFile = NativeMethods.FILE_EXECUTE,
        /// <summary>
        /// Specifies the right to list the contents of a folder and to run applications contained within that folder.
        /// </summary>
        Traverse = NativeMethods.FILE_TRAVERSE,
        /// <summary>
        /// Specifies the right to delete a folder and any files contained within that folder. 
        /// </summary>
        DeleteSubdirectoriesAndFiles = NativeMethods.FILE_DELETE_CHILD,
        /// <summary>
        /// Specifies the right to open and copy file system attributes from a folder or file. For example, this value specifies the right to view the file creation or modified date. This does not include the right to read data, extended file system attributes, or access and audit rules. 
        /// </summary>
        ReadAttributes = NativeMethods.FILE_READ_ATTRIBUTES,
        /// <summary>
        /// Specifies the right to open and write file system attributes to a folder or file. This does not include the ability to write data, extended attributes, or access and audit rules. 
        /// </summary>
        WriteAttributes = NativeMethods.FILE_WRITE_ATTRIBUTES,
        /// <summary>
        /// Specifies the right to delete a folder or file. 
        /// </summary>
        Delete = NativeMethods.DELETE,
        /// <summary>
        /// Specifies the right to open and copy access and audit rules from a folder or file. This does not include the right to read data, file system attributes, and extended file system attributes. 
        /// </summary>
        ReadPermissions = NativeMethods.READ_CONTROL,
        /// <summary>
        /// Specifies the right to change the security and audit rules associated with a file or folder. 
        /// </summary>
        ChangePermissions = NativeMethods.WRITE_DAC,
        /// <summary>
        /// Specifies the right to change the owner of a folder or file. Note that owners of a resource have full access to that resource. 
        /// </summary>
        TakeOwnership = NativeMethods.WRITE_OWNER,
        /// <summary>
        /// Specifies whether the application can wait for a file handle to synchronize with the completion of an I/O operation. 
        /// </summary>
        Synchronize = NativeMethods.SYNCHRONIZE,
        /// <summary>
        /// Specifies the right to exert full control over a folder or file, and to modify access control and audit rules. 
        /// This value represents the right to do anything with a file and is the combination of all rights in this enumeration except
        /// for <see cref="FileSystemRights.SystemSecurity"/>
        /// </summary>
        FullControl = NativeMethods.FILE_ALL_ACCESS,
        /// <summary>
        /// The <see cref="SystemSecurity"/> access right controls the ability to get or set the SACL in an object's security 
        /// descriptor. The system grants this access right only if the <see cref="Security.Privilege.Security"/> privilege is enabled in the 
        /// access token of the requesting thread (see <see cref="Security.PrivilegeEnabler"/>).
        /// </summary>
        SystemSecurity = NativeMethods.ACCESS_SYSTEM_SECURITY
    }
}

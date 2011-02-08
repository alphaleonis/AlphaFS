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

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// The file attributes of a file.
    /// </summary>
   [Flags]
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileAttributes : uint
    {
        /// <summary>
        /// No attributes set.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// The file or directory is an archive file or directory. Applications use this attribute to mark files for backup or removal.
        /// </summary>
        Archive = 0x20,

        /// <summary>
        /// <para>The file or directory is compressed.</para>
        /// <para>For a file, this means that all of the data in the file is compressed.</para>
        /// <para>For a directory, this means that compression is the default for newly created files and subdirectories.</para>
        /// </summary>
        Compressed = 0x800,

        /// <summary>
        /// Reserved; do not use.
        /// </summary>
        Device = 0x40,

        /// <summary>
        /// The handle identifies a directory.
        /// </summary>
        Directory = 0x10,

        /// <summary>
        /// <para>The file or directory is encrypted.</para>
        /// <para>For a file, this means that all data in the file is encrypted.</para>
        /// <para>For a directory, this means that encryption is the default for newly created files and subdirectories.</para>
        /// </summary>
        Encrypted = 0x4000,

        /// <summary>
        /// The file or directory is hidden. It is not included in an ordinary directory listing.
        /// </summary>
        Hidden = 0x02,

        /// <summary>
        /// The file or directory does not have other attributes set. This attribute is valid only when used alone.
        /// </summary>
        Normal = 0x80,

        /// <summary>
        /// The file is not to be indexed by the content indexing service.
        /// </summary>
        NotContentIndexed = 0x2000,

        /// <summary>
        /// <para>The file data is not available immediately.</para>
        /// <para>This attribute indicates that the file data is physically moved to offline storage.</para>
        /// <para>This attribute is used by Remote Storage, which is the hierarchical storage management software.</para>
        /// <para><b>Note</b> Applications should not arbitrarily change this attribute.</para>
        /// </summary>
        Offline = 0x1000,

        /// <summary>
        /// <para>The file or directory is read-only. </para>
        /// <para>For a file, applications can read the file, but cannot write to it or delete it.</para>
        /// <para>For a directory, applications cannot delete it.</para>
        /// </summary>
        ReadOnly = 0x01,

        /// <summary>
        /// The file or directory has an associated reparse point.
        /// </summary>
        ReparsePoint = 0x400,

        /// <summary>
        /// The file is a sparse file.
        /// </summary>
        SparseFile = 0x200,

        /// <summary>
        /// The file or directory is part of the operating system, or the operating system uses the file or directory exclusively.
        /// </summary>
        System = 0x04,

        /// <summary>
        /// <para>The file is being used for temporary storage.</para>
        /// <para>File systems attempt to keep all of the data in memory for quick access, rather than flushing it back to mass storage.</para>
        /// <para>An application should delete a temporary file as soon as it is not needed.</para>
        /// </summary>
        Temporary = 0x100,

        /// <summary>
        /// The file is a virtual file.
        /// </summary>
        Virtual = 0x10000,
    }
}
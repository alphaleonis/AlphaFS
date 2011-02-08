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
    /// Enumeration specifying the different reparse point tags.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags"), Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ReparsePointTag : uint
    {
        /// <summary>
        /// The entry is not a reparse point
        /// </summary>
        None = 0x00000000,
        /// <summary>
        /// Dfs
        /// </summary>
        Dfs = 0x8000000A,
        /// <summary>
        /// Dfsr
        /// </summary>
        Dfsr = 0x80000012,
        /// <summary>
        /// Hsm
        /// </summary>
        Hsm = 0xC0000004,
        /// <summary>
        /// Hsm2
        /// </summary>
        Hsm2 = 0x80000006,
        /// <summary>
        /// The entry is a mount point
        /// </summary>
        MountPoint = 0xA0000003,
        /// <summary>
        /// Sis
        /// </summary>
        Sis = 0x80000007,
        /// <summary>
        /// The entry is a symbolic link
        /// </summary>
        SymLink = 0xA000000C
    }
}

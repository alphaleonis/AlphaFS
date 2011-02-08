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
    /// <para>Contains constants for controlling the kind of access other FileStream objects can have to the same file.</para>
    /// <para>This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.</para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags]
    public enum FileShare : uint
    {
        /// <summary>
        /// Declines sharing of the current file. Any request to open the file (by this process or another process) will fail until the file is closed.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Allows subsequent opening of the file for reading. If this flag is not specified, any request to open the file for reading (by this process or another process) will fail until the file is closed. However, even if this flag is specified, additional permissions might still be needed to access the file.
        /// </summary>
        Read = NativeMethods.FILE_SHARE_READ,
        /// <summary>
        /// Allows subsequent opening of the file for writing. If this flag is not specified, any request to open the file for writing (by this process or another process) will fail until the file is closed. However, even if this flag is specified, additional permissions might still be needed to access the file.        
        /// </summary>
        Write = NativeMethods.FILE_SHARE_WRITE,
        /// <summary>
        /// Allows subsequent opening of the file for reading or writing. If this flag is not specified, any request to open the file for reading or writing (by this process or another process) will fail until the file is closed. However, even if this flag is specified, additional permissions might still be needed to access the file.
        /// </summary>
        ReadWrite = Read | Write,
        /// <summary>
        /// Allows subsequent deleting of a file.
        /// </summary>
        Delete = NativeMethods.FILE_SHARE_DELETE
    }
}

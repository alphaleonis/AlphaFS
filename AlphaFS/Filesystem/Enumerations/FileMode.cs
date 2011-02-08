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

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Specifies how the operating system should open a file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum FileMode : int
    {
        /// <summary>
        /// Specifies that the operating system should create a new file. If the file already exists, an exception is thrown.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        CreateNew = 1,
        /// <summary>
        /// Specifies that the operating system should create a new file. If the file already exists, it will be overwritten. 
        /// Create is equivalent to requesting that if the file does not exist, use <see cref="CreateNew"/>; otherwise, use <see cref="Truncate"/>.
        /// </summary>
        Create = 2,
        /// <summary>
        /// Specifies that the operating system should open an existing file. The ability to open the file is dependent on the value specified by <see cref="FileAccess"/>. A <see cref="System.IO.FileNotFoundException"/> is thrown if the file does not exist.
        /// </summary>
        Open = 3,
        /// <summary>
        /// Specifies that the operating system should open a file if it exists; otherwise, a new file should be created. 
        /// </summary>
        OpenOrCreate = 4,
        /// <summary>
        /// Specifies that the operating system should open an existing file. Once opened, the file should be truncated so that its size is zero bytes. Attempts to read from a file opened with <c>Truncate</c> cause an exception.
        /// </summary>
        Truncate = 5
    }
}

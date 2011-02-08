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
    /// Represents the encryption status of the specified file.
    /// </summary>
    public enum FileEncryptionStatus
    {
        /// <summary>
        /// The file can be encrypted.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Encryptable")]
        Encryptable         = 0x00,
        /// <summary>
        /// The file is encrypted.
        /// </summary>
        Encrypted           = 0x01,
        /// <summary>
        /// The file is a read-only file.
        /// </summary>
        ReadOnly            = 0x08,
        /// <summary>
        /// The file is a root directory. Root directories cannot be encrypted.
        /// </summary>
        RootDir             = 0x03,
        /// <summary>
        /// The file is a system file. System files cannot be encrypted.
        /// </summary>
        SystemFile          = 0x02,
        /// <summary>
        /// The file is a system directory. System directories cannot be encrypted.
        /// </summary>
        SystemDirectory     = 0x04,
        /// <summary>
        /// The file system does not support file encryption.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Filesystem")]
        NoFilesystemSupport = 0x06,
        /// <summary>
        /// The encryption status is unknown. The file may be encrypted.
        /// </summary>
        Unknown             = 0x05,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        UserDisallowed      = 0x07
    }
}

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
    /// Flags that specify how a file is to be copied. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags]
    public enum CopyOptions : uint
    {
        /// <summary>
        /// None of the other flags.
        /// </summary>
        None = 0x00,
        
        /// <summary>
        /// The copy operation fails immediately if the target file already exists.
        /// </summary>
        FailIfExists = 0x01,

        /// <summary>
        /// Progress of the copy is tracked in the target file in case the copy fails. 
        /// The failed copy can be restarted at a later time by specifying the same values for 
        /// existing file name and new file name as those used in the call that failed.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Restartable")]
        Restartable = 0x02,

        /// <summary>
        /// The file is copied and the original file is opened for write access.
        /// </summary>
        OpenSourceForWrite = 0x04,

        /// <summary>
        /// An attempt to copy an encrypted file will succeed even if the destination copy cannot be encrypted.        
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///    <term>Windows 2000:</term>
        ///    <description>This value is not supported</description>
        /// </list>
        /// </remarks>
        AllowDecryptedDestination = 0x08,

        /// <summary>
        /// If the source file is a symbolic link, the destination file is also a symbolic link pointing to the same file that the source symbolic link is pointing to. 
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///    <term>Windows Server 2003 and Windows XP/2000:</term>
        ///    <description>This value is not supported</description>
        /// </list>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Symlink")]
        CopySymlink = 0x800,
    }
}

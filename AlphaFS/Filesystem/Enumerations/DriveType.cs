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
    /// Specifies the type of a drive.
    /// </summary>
    public enum DriveType : int
    {
        /// <summary>
        /// The drive type cannot be determined.
        /// </summary>
        Unknown = (int)NativeMethods.DRIVE_UNKNOWN,
        /// <summary>
        /// The root path is invalid; for example, there is no volume is mounted at the path.
        /// </summary>
        NoRootDir = (int)NativeMethods.DRIVE_NO_ROOT_DIR,
        /// <summary>
        /// The drive has removable media; for example, a floppy drive, thumb drive, or flash card reader.
        /// </summary>
        Removable = (int)NativeMethods.DRIVE_REMOVABLE,
        /// <summary>
        /// The drive has fixed media; for example, a hard drive or flash drive.
        /// </summary>
        Fixed = (int)NativeMethods.DRIVE_FIXED,
        /// <summary>
        /// The drive is a remote (network) drive.
        /// </summary>
        Remote = (int)NativeMethods.DRIVE_REMOTE,
        /// <summary>
        /// The drive is a CD-ROM drive.
        /// </summary>
        CDRom = (int)NativeMethods.DRIVE_CDROM,
        /// <summary>
        /// The drive is a RAM disk.
        /// </summary>
        RamDisk = (int)NativeMethods.DRIVE_RAMDISK
    }
}

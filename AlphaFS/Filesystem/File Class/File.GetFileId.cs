/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
    public static partial class File
    {
        /// <summary>[AlphaFS] Retrieves a unique file identifier.</summary>                
        /// <remarks>File IDs are not guaranteed to be unique over time, because file systems are free to reuse them. In some cases, the file ID for a file can change over time.</remarks>
        [SecurityCritical]
        public static FileId GetFileId(string path)
        {
            using (var handle = File.CreateFileCore(null, path,
                ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, FileSystemRights.ReadData,
                FileShare.ReadWrite, true, PathFormat.RelativePath))
            {
                if (NativeMethods.IsAtLeastWindows8)
                {
                    //ReFS is supported                    
                    using (var safeBuffer = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(typeof(NativeMethods.FILE_ID_INFO))))
                    {
                        if (!NativeMethods.GetFileInformationByHandleEx(handle, NativeMethods.FileInfoByHandleClass.FileIdInfo, safeBuffer, (uint)safeBuffer.Capacity))                        
                            NativeError.ThrowException(Marshal.GetLastWin32Error());

                        NativeMethods.FILE_ID_INFO info = safeBuffer.PtrToStructure<NativeMethods.FILE_ID_INFO>(0);
                        return new FileId(info);
                    }
                }
                else
                {
                    //only NTFS is supported
                    NativeMethods.BY_HANDLE_FILE_INFORMATION info;

                    if (!NativeMethods.GetFileInformationByHandle(handle, out info))
                        NativeError.ThrowException(Marshal.GetLastWin32Error());

                    return new FileId(info);
                }
            }            
        }
    }
}

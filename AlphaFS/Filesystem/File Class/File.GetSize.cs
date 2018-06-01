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

using System.Collections.ObjectModel;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path)
      {
         return GetSizeCore(null, null, path, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path, PathFormat pathFormat)
      {
         return GetSizeCore(null, null, path, false, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path, bool sizeOfAllStreams)
      {
         return GetSizeCore(null, null, path, sizeOfAllStreams, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path, bool sizeOfAllStreams, PathFormat pathFormat)
      {
         return GetSizeCore(null, null, path, sizeOfAllStreams, pathFormat);
      }
      

      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path)
      {
         return GetSizeCore(transaction, null, path, false, PathFormat.RelativePath);
      }

      
      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetSizeCore(transaction, null, path, false, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, bool sizeOfAllStreams)
      {
         return GetSizeCore(transaction, null, path, sizeOfAllStreams, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, bool sizeOfAllStreams, PathFormat pathFormat)
      {
         return GetSizeCore(transaction, null, path, sizeOfAllStreams, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of the specified file.</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(SafeFileHandle handle)
      {
         return GetSizeCore(null, handle, null, false, PathFormat.LongFullPath);
      }




      /// <summary>Retrieves the size of the specified file.
      /// <remarks>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.
      /// </remarks>
      /// </summary>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      /// <exception/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static long GetSizeCore(KernelTransaction transaction, SafeFileHandle safeHandle, string path, bool sizeOfAllStreams, PathFormat pathFormat)
      {
         var pathLp = path;
         
         var callerHandle = null != safeHandle;
         if (!callerHandle)
         {
            pathLp = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

            safeHandle = CreateFileCore(transaction, pathLp, ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.ReadData, FileShare.Read, true, false, PathFormat.LongFullPath);
         }


         if (sizeOfAllStreams)
            return GetSizeAllStreamsCore(transaction, pathLp, pathFormat);


         long fileSize;

         try
         {
            var success = NativeMethods.GetFileSizeEx(safeHandle, out fileSize);

            var lastError = Marshal.GetLastWin32Error();

            if (!success && lastError != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(lastError, path);
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && null != safeHandle && !safeHandle.IsClosed)
               safeHandle.Close();
         }
         

         return fileSize;
      }


      /// <summary>Retrieves the size of all alternate datastreams.</summary>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      /// <exception/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static long GetSizeAllStreamsCore(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         var pathLp = pathFormat == PathFormat.LongFullPath ? path : Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

         var streamSizes = new Collection<long>();

         using (var buffer = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(typeof(NativeMethods.WIN32_FIND_STREAM_DATA))))
         using (var safeHandle = null == transaction

            // FindFirstStreamW() / FindFirstStreamTransactedW()
            // 2018-01-15: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.FindFirstStreamW(pathLp, NativeMethods.STREAM_INFO_LEVELS.FindStreamInfoStandard, buffer, 0)
            : NativeMethods.FindFirstStreamTransactedW(pathLp, NativeMethods.STREAM_INFO_LEVELS.FindStreamInfoStandard, buffer, 0, transaction.SafeHandle))
         {
            var lastError = Marshal.GetLastWin32Error();
            var reachedEOF = lastError == Win32Errors.ERROR_HANDLE_EOF;


            if (!NativeMethods.IsValidHandle(safeHandle, false))
            {
               if (!reachedEOF)
                  NativeError.ThrowException(lastError, pathLp);
            }


            while (true)
            {
               streamSizes.Add(buffer.PtrToStructure<NativeMethods.WIN32_FIND_STREAM_DATA>(0).StreamSize);

               var success = NativeMethods.FindNextStreamW(safeHandle, buffer);

               lastError = Marshal.GetLastWin32Error();
               if (!success)
               {
                  if (lastError == Win32Errors.ERROR_HANDLE_EOF)
                     break;

                  NativeError.ThrowException(lastError, pathLp);
               }
            }
         }


         return streamSizes.Sum();
      }
   }
}

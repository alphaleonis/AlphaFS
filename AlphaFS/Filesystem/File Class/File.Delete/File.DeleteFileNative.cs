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

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      [SecurityCritical]
      internal static bool DeleteFileNative(string pathLp, bool continueOnException, DeleteArguments deleteArguments, out int lastError)
      {

      startDeleteFile:

         var success = null == deleteArguments.Transaction || !NativeMethods.IsAtLeastWindowsVista

            // DeleteFile() / DeleteFileTransacted()
            // 2013-01-13: MSDN confirms LongPath usage.
            //
            // If the path points to a symbolic link, the symbolic link is deleted, not the target.

            ? NativeMethods.DeleteFile(pathLp)

            : NativeMethods.DeleteFileTransacted(pathLp, deleteArguments.Transaction.SafeHandle);


         lastError = Marshal.GetLastWin32Error();

         if (success)
         {
            if (lastError == Win32Errors.ERROR_NO_MORE_FILES)
               lastError = 0;
         }

         else
         {
            var attributes = deleteArguments.Attributes;


            switch ((uint) lastError)
            {
               case Win32Errors.ERROR_FILE_NOT_FOUND:
                  // MSDN: .NET 3.5+: If the file to be deleted does not exist, no exception is thrown.
                  success = true;
                  break;


               case Win32Errors.ERROR_PATH_NOT_FOUND:
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: The specified path is invalid (for example, it is on an unmapped drive).
                  if (!continueOnException)
                     NativeError.ThrowException(lastError, pathLp);
                  break;


               case Win32Errors.ERROR_SHARING_VIOLATION:
                  // MSDN: .NET 3.5+: IOException: The specified file is in use or there is an open handle on the file.
                  if (!continueOnException)
                     NativeError.ThrowException(lastError, pathLp);
                  break;


               case Win32Errors.ERROR_ACCESS_DENIED:

                  if (attributes == 0)
                  {
                     var attrs = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();

                     if (FillAttributeInfoCore(deleteArguments.Transaction, pathLp, ref attrs, false, true) == Win32Errors.NO_ERROR)

                        attributes = attrs.dwFileAttributes;
                  }


                  // MSDN: .NET 3.5+: UnauthorizedAccessException: Path is a directory.
                  if (IsDirectory(attributes))
                     throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, "({0}) {1}", lastError.ToString(CultureInfo.InvariantCulture), string.Format(CultureInfo.InvariantCulture, Resources.Target_File_Is_A_Directory, pathLp)));


                  if (IsReadOnlyOrHidden(attributes))
                  {
                     if (deleteArguments.IgnoreReadOnly)
                     {
                        // Reset attributes to Normal.
                        SetAttributesCore(deleteArguments.Transaction, false, pathLp, FileAttributes.Normal, PathFormat.LongFullPath);

                        goto startDeleteFile;
                     }


                     // MSDN: .NET 3.5+: UnauthorizedAccessException: Path specified a read-only file.
                     if (!continueOnException)
                        throw new FileReadOnlyException(pathLp, lastError);
                  }

                  
                  // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
                  if (attributes == 0)
                  {
                     if (!continueOnException)
                        NativeError.ThrowException(lastError, pathLp);
                  }

                  break;
            }

            // MSDN: .NET 3.5+: IOException:
            // The specified file is in use.
            // There is an open handle on the file, and the operating system is Windows XP or earlier.

            if (!success && !continueOnException)
               NativeError.ThrowException(lastError, IsDirectory(attributes), pathLp);
         }


         return success;
      }
   }
}

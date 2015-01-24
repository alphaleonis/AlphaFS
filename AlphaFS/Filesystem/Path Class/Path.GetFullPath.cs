/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>
      /// <para>GetFullPathName merges the name of the current drive and directory with a specified file name to determine the full path and file name of a specified file.</para>
      /// <para>It also calculates the address of the file name portion of the full path and file name.</para>
      /// <para>&#160;</para>
      /// <para>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</para>
      /// <para>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</para>
      /// <para>&#160;</para>
      /// <para>MSDN: Multithreaded applications and shared library code should not use the GetFullPathName function and</para>
      /// <para>should avoid using relative path names. The current directory state written by the SetCurrentDirectory function is stored as a global variable in each process,</para>
      /// <para>therefore multithreaded applications cannot reliably use this value without possible data corruption from other threads that may also be reading or setting this value.</para>
      /// <para>This limitation also applies to the SetCurrentDirectory and GetCurrentDirectory functions. The exception being when the application is guaranteed to be running in a single thread,</para>
      /// <para>for example parsing file names from the command line argument string in the main thread prior to creating any additional threads.</para>
      /// <para>Using relative path names in multithreaded applications or shared library code can yield unpredictable results and is not supported.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException">path contains a colon (":") that is not part of a volume identifier (for example, "c:\").</exception>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      [SecurityCritical]
      public static string GetFullPath(string path)
      {
         return GetFullPathTackleInternal(null, path);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>
      /// <para>GetFullPathName merges the name of the current drive and directory with a specified file name to determine the full path and file name of a specified file.</para>
      /// <para>It also calculates the address of the file name portion of the full path and file name.</para>
      /// <para>&#160;</para>
      /// <para>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</para>
      /// <para>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</para>
      /// <para>&#160;</para>
      /// <para>MSDN: Multithreaded applications and shared library code should not use the GetFullPathName function and</para>
      /// <para>should avoid using relative path names. The current directory state written by the SetCurrentDirectory function is stored as a global variable in each process,</para>
      /// <para>therefore multithreaded applications cannot reliably use this value without possible data corruption from other threads that may also be reading or setting this value.</para>
      /// <para>This limitation also applies to the SetCurrentDirectory and GetCurrentDirectory functions. The exception being when the application is guaranteed to be running in a single thread,</para>
      /// <para>for example parsing file names from the command line argument string in the main thread prior to creating any additional threads.</para>
      /// <para>Using relative path names in multithreaded applications or shared library code can yield unpredictable results and is not supported.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException">path contains a colon (":") that is not part of a volume identifier (for example, "c:\").</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path)
      {
         return GetFullPathTackleInternal(transaction, path);
      }

      #region Internal Methods

      /// <summary>Unified method GetFullPathInternal() to retrieve the absolute path for the specified <paramref name="path"/> string.</summary>
      /// <returns>Returns the fully qualified location of <paramref name="path"/>, such as "C:\MyFile.txt".</returns>
      /// <remarks>
      /// <para>GetFullPathName merges the name of the current drive and directory with a specified file name to determine the full path and file name of a specified file.</para>
      /// <para>It also calculates the address of the file name portion of the full path and file name.</para>
      /// <para>&#160;</para>
      /// <para>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</para>
      /// <para>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0".</para>
      /// <para>&#160;</para>
      /// <para>MSDN: Multithreaded applications and shared library code should not use the GetFullPathName function and</para>
      /// <para>should avoid using relative path names. The current directory state written by the SetCurrentDirectory function is stored as a global variable in each process,</para>
      /// <para>therefore multithreaded applications cannot reliably use this value without possible data corruption from other threads that may also be reading or setting this value.</para>
      /// <para>This limitation also applies to the SetCurrentDirectory and GetCurrentDirectory functions. The exception being when the application is guaranteed to be running in a single thread,</para>
      /// <para>for example parsing file names from the command line argument string in the main thread prior to creating any additional threads.</para>
      /// <para>Using relative path names in multithreaded applications or shared library code can yield unpredictable results and is not supported.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="options">Options for controlling the operation.</param>
      [SecurityCritical]
      internal static string GetFullPathInternal(KernelTransaction transaction, string path, GetFullPathOptions options)
      {
         if (path != null)
            if (path.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase))
               return path;
         
         if (options != GetFullPathOptions.None)
         {
            if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
            {
               bool checkAdditional = (options & GetFullPathOptions.CheckAdditional) != 0;

               CheckInvalidPathChars(path, checkAdditional);

               // Prevent duplicate checks.
               options &= ~GetFullPathOptions.CheckInvalidPathChars;

               if (checkAdditional)
                  options &= ~GetFullPathOptions.CheckAdditional;
            }

            // Do not remove trailing directory separator when path points to a drive like: "C:\"
            // Doing so makes path point to the current directory.

            if (path == null || path.Length <= 3 || (!path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase) && path[1] != VolumeSeparatorChar))
               options &= ~GetFullPathOptions.RemoveTrailingDirectorySeparator;
         }


         string pathLp = GetLongPathInternal(path, options);

         uint bufferSize = NativeMethods.MaxPathUnicode;
         

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            startGetFullPathName:

            var buffer = new StringBuilder((int)bufferSize);
            uint returnLength = (transaction == null || !NativeMethods.IsAtLeastWindowsVista

               // GetFullPathName() / GetFullPathNameTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.

               ? NativeMethods.GetFullPathName(pathLp, bufferSize, buffer, IntPtr.Zero)
               : NativeMethods.GetFullPathNameTransacted(pathLp, bufferSize, buffer, IntPtr.Zero, transaction.SafeHandle));

            if (returnLength != Win32Errors.NO_ERROR)
            {
               if (returnLength > bufferSize)
               {
                  bufferSize = returnLength;
                  goto startGetFullPathName;
               }
            }
            else
            {
               if ((options & GetFullPathOptions.ContinueOnNonExist) != 0)
                  return null;

               NativeError.ThrowException(pathLp);
            }

            return (options & GetFullPathOptions.AsLongPath) != 0
               ? GetLongPathInternal(buffer.ToString(), GetFullPathOptions.None)
               : GetRegularPathInternal(buffer.ToString(), GetFullPathOptions.None);
         }
      }

      private static string GetFullPathTackleInternal(KernelTransaction transaction, string path)
      {
         if (path != null)
         {
            if (path.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase))
               return path;

            // Tackle: Path.GetFullPath(@"\\\\.txt"), but exclude "." which is the current directory.
            bool isLongPath = path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase) ||
                              path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase);

            bool isUnc = !isLongPath && path.StartsWith(UncPrefix, StringComparison.OrdinalIgnoreCase);

            string tackle = GetRegularPathInternal(path, GetFullPathOptions.None).TrimStart(DirectorySeparatorChar, AltDirectorySeparatorChar);

            if (isUnc && (tackle.Length >= 2 && tackle[0] == CurrentDirectoryPrefixChar))
               throw new ArgumentException(Resources.UNCPathShouldMatchTheFormatServerShare);
         }

         CheckValidPath(path, true, true);

         return GetFullPathInternal(transaction, path, GetFullPathOptions.None);
      }

      #endregion // Internal Methods
   }
}

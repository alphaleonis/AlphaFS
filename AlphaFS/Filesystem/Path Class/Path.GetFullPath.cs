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

using System;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region .NET

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
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      [SecurityCritical]
      public static string GetFullPath(string path)
      {
         return GetFullPathTackleCore(null, path, GetFullPathOptions.None);
      }

      #endregion // .NET


      #region AlphaFS

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
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="options">Options for controlling the full path retrieval.</param>
      [SecurityCritical]
      public static string GetFullPath(string path, GetFullPathOptions options)
      {
         return GetFullPathTackleCore(null, path, options);
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
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      [SecurityCritical]
      public static string GetFullPathTransacted(KernelTransaction transaction, string path)
      {
         return GetFullPathTackleCore(transaction, path, GetFullPathOptions.None);
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
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="options">Options for controlling the full path retrieval.</param>
      [SecurityCritical]
      public static string GetFullPathTransacted(KernelTransaction transaction, string path, GetFullPathOptions options)
      {
         return GetFullPathTackleCore(transaction, path, options);
      }

      #endregion // AlphaFS

      #region Internal Methods

      /// <summary>Retrieves the absolute path for the specified <paramref name="path"/> string.</summary>
      /// <returns>The fully qualified location of <paramref name="path"/>, such as "C:\MyFile.txt".</returns>
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
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="options">Options for controlling the full path retrieval.</param>
      [SecurityCritical]
      internal static string GetFullPathCore(KernelTransaction transaction, string path, GetFullPathOptions options)
      {
         if (path != null)
            if (path.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) ||path.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase))
               return path;
         
         if (options != GetFullPathOptions.None)
         {
            if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
            {
               var checkAdditional = (options & GetFullPathOptions.CheckAdditional) != 0;

               CheckInvalidPathChars(path, checkAdditional, false);

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


         var pathLp = GetLongPathCore(path, options);
         uint bufferSize = NativeMethods.MaxPathUnicode;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            startGetFullPathName:

            var buffer = new StringBuilder((int)bufferSize);
            var returnLength = (transaction == null || !NativeMethods.IsAtLeastWindowsVista

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


            var finalFullPath = (options & GetFullPathOptions.AsLongPath) != 0
               ? GetLongPathCore(buffer.ToString(), GetFullPathOptions.None)
               : GetRegularPathCore(buffer.ToString(), GetFullPathOptions.None, false);


            finalFullPath = NormalizePath(finalFullPath, options);


            if ((options & GetFullPathOptions.KeepDotOrSpace) != 0)
            {
               if (pathLp.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                  finalFullPath += ".";

               var lastChar = pathLp[pathLp.Length - 1];
               if (char.IsWhiteSpace(lastChar))
                  finalFullPath += lastChar;
            }

            
            return finalFullPath;
         }
      }

      private static string GetFullPathTackleCore(KernelTransaction transaction, string path, GetFullPathOptions options)
      {
         if (path != null)
         {
            if (path.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) || path.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase))
               return path;

            CheckInvalidUncPath(path);
         }

         CheckSupportedPathFormat(path, true, true);

         return GetFullPathCore(transaction, path, options);
      }

      /// <summary>Applies the <seealso cref="GetFullPathOptions"/> to <paramref name="path"/></summary>
      /// <returns><paramref name="path"/> with applied <paramref name="options"/>.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="path"></param>
      /// <param name="options"></param>
      private static string ApplyFullPathOptions(string path, GetFullPathOptions options)
      {
         if ((options & GetFullPathOptions.TrimEnd) != 0)
            if ((options & GetFullPathOptions.KeepDotOrSpace) == 0)
               path = path.TrimEnd();

         if ((options & GetFullPathOptions.AddTrailingDirectorySeparator) != 0)
            path = AddTrailingDirectorySeparator(path, false);

         if ((options & GetFullPathOptions.RemoveTrailingDirectorySeparator) != 0)
            path = RemoveTrailingDirectorySeparator(path, false);

         if ((options & GetFullPathOptions.CheckInvalidPathChars) != 0)
            CheckInvalidPathChars(path, (options & GetFullPathOptions.CheckAdditional) != 0, false);


         // Trim leading whitespace.
         if ((options & GetFullPathOptions.KeepDotOrSpace) == 0)
            path = path.TrimStart();

         return path;
      }

      #endregion // Internal Methods
   }
}

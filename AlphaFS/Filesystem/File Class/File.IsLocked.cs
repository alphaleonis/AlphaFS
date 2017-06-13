/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class File
   {
      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="path">The file to check.</param>
      [SecurityCritical]
      public static bool IsLocked(string path)
      {
         return IsLockedCore(null, path, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static bool IsLocked(string path, PathFormat pathFormat)
      {
         return IsLockedCore(null, path, pathFormat);
      }


      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      [SecurityCritical]
      public static bool IsLockedTransacted(KernelTransaction transaction, string path)
      {
         return IsLockedCore(transaction, path, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static bool IsLockedTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return IsLockedCore(transaction, path, pathFormat);
      }




      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="filePath">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static bool IsLockedCore(KernelTransaction transaction, string filePath, PathFormat pathFormat)
      {
         try
         {
            // Use FileAccess.Read since FileAccess.ReadWrite always fails when file is read-only.
            using (OpenCore(transaction, filePath, FileMode.Open, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, null, null, pathFormat)) {}
         }
         catch (IOException ex)
         {
            var lastError = Marshal.GetHRForException(ex) & NativeMethods.OverflowExceptionBitShift;
            if (lastError == Win32Errors.ERROR_SHARING_VIOLATION || lastError == Win32Errors.ERROR_LOCK_VIOLATION)
               return true;

            throw;
         }
         catch (Exception ex)
         {
            NativeError.ThrowException(Marshal.GetHRForException(ex) & NativeMethods.OverflowExceptionBitShift, filePath);
         }

         return false;
      }




      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the files specified by <paramref name="filePath"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file specified by <paramref name="filePath"/>.</para>
      /// <para>Returns a list of processes locking the file specified by <paramref name="filePath"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="filePath">The path to the file.</param>
      public static Collection<Process> GetProcessForFileLock(string filePath)
      {
         return GetProcessForFileLockCore(null, new Collection<string>(new[] {filePath}), PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the files specified by <paramref name="filePath"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file specified by <paramref name="filePath"/>.</para>
      /// <para>Returns a list of processes locking the file specified by <paramref name="filePath"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="filePath">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      public static Collection<Process> GetProcessForFileLock(string filePath, PathFormat pathFormat)
      {
         return GetProcessForFileLockCore(null, new Collection<string>(new[] {filePath}), pathFormat);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the file(s) specified by <paramref name="filePaths"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// <para>Returns a list of processes locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="filePaths">A list with one or more file paths.</param>
      public static Collection<Process> GetProcessForFileLock(Collection<string> filePaths)
      {
         return GetProcessForFileLockCore(null, filePaths, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the file(s) specified by <paramref name="filePaths"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// <para>Returns a list of processes locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="filePaths">A list with one or more file paths.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      public static Collection<Process> GetProcessForFileLock(Collection<string> filePaths, PathFormat pathFormat)
      {
         return GetProcessForFileLockCore(null, filePaths, pathFormat);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the files specified by <paramref name="filePath"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file specified by <paramref name="filePath"/>.</para>
      /// <para>Returns a list of processes locking the file specified by <paramref name="filePath"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="filePath">The path to the file.</param>
      public static Collection<Process> GetProcessForFileLock(KernelTransaction transaction, string filePath)
      {
         return GetProcessForFileLockCore(transaction, new Collection<string>(new[] { filePath }), PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the files specified by <paramref name="filePath"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file specified by <paramref name="filePath"/>.</para>
      /// <para>Returns a list of processes locking the file specified by <paramref name="filePath"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="filePath">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      public static Collection<Process> GetProcessForFileLock(KernelTransaction transaction, string filePath, PathFormat pathFormat)
      {
         return GetProcessForFileLockCore(transaction, new Collection<string>(new[] { filePath }), pathFormat);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the file(s) specified by <paramref name="filePaths"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// <para>Returns a list of processes locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="filePaths">A list with one or more file paths.</param>
      public static Collection<Process> GetProcessForFileLock(KernelTransaction transaction, Collection<string> filePaths)
      {
         return GetProcessForFileLockCore(transaction, filePaths, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the file(s) specified by <paramref name="filePaths"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// <para>Returns a list of processes locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="filePaths">A list with one or more file paths.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      public static Collection<Process> GetProcessForFileLock(KernelTransaction transaction, Collection<string> filePaths, PathFormat pathFormat)
      {
         return GetProcessForFileLockCore(transaction, filePaths, pathFormat);
      }




      /// <summary>[AlphaFS] Gets a list of processes that have a lock on the file(s) specified by <paramref name="filePaths"/>.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns null when no processes found that are locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// <para>Returns a list of processes locking the file(s) specified by <paramref name="filePaths"/>.</para>
      /// </returns>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="InvalidOperationException"/>
      /// <param name="transaction"></param>
      /// <param name="filePaths">A list with one or more file paths.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.RmEndSession(System.UInt32)")]
      internal static Collection<Process> GetProcessForFileLockCore(KernelTransaction transaction, Collection<string> filePaths, PathFormat pathFormat)
      {
         if (null == filePaths)
            throw new ArgumentNullException("filePaths");

         if (filePaths.Count == 0)
            throw new ArgumentOutOfRangeException("filePaths", "No paths specified.");


         var isLongPath = pathFormat == PathFormat.LongFullPath;
         var allPaths = isLongPath ? new Collection<string>(filePaths) : new Collection<string>();

         if (!isLongPath)
            foreach (var path in filePaths)
               allPaths.Add(Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck));




         uint sessionHandle;
         var success = NativeMethods.RmStartSession(out sessionHandle, 0, Guid.NewGuid().ToString()) == Win32Errors.ERROR_SUCCESS;

         var lastError = Marshal.GetLastWin32Error();
         if (!success)
            NativeError.ThrowException(lastError);




         var processes = new Collection<Process>();

         try
         {
            // A snapshot count of all running processes.
            var processesFound = (uint) Process.GetProcesses().Length;
            uint lpdwRebootReasons = 0;


            success = NativeMethods.RmRegisterResources(sessionHandle, (uint) allPaths.Count, allPaths.ToArray(), 0, null, 0, null) == Win32Errors.ERROR_SUCCESS;

            lastError = Marshal.GetLastWin32Error();
            if (!success)
               NativeError.ThrowException(lastError);


         GetList:

            var processInfo = new NativeMethods.RM_PROCESS_INFO[processesFound];
            var processesTotal = processesFound;


            lastError = NativeMethods.RmGetList(sessionHandle, out processesFound, ref processesTotal, processInfo, ref lpdwRebootReasons);


            // There would b no more need for this because we already have a/the total number of running processes.
            if (lastError == Win32Errors.ERROR_MORE_DATA)
               goto GetList;


            if (lastError != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(lastError);


            for (var i = 0; i < processesTotal; i++)
            {
               try
               {
                  processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
               }

               // MSDN: The process specified by the processId parameter is not running. The identifier might be expired.
               catch (ArgumentException) {}
            }
         }
         finally
         {
            NativeMethods.RmEndSession(sessionHandle);
         }


         return processes.Count == 0 ? null : processes;
      }
   }
}

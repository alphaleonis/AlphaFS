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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class that retrieves file system entries (i.e. files and directories) using Win32 API FindFirst()/FindNext().</summary>
   [SerializableAttribute]
   internal sealed class FindFileSystemEntryInfo
   {
      private static readonly Regex WildcardMatchAll = new Regex(@"^(\*)+(\.\*+)+$", RegexOptions.IgnoreCase | RegexOptions.Compiled); // special case to recognize *.* or *.** etc
      private Regex _nameFilter;
      private string _searchPattern = Path.WildcardStarMatchAll;


      public FindFileSystemEntryInfo(bool isFolder, KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options, Type typeOfT, PathFormat pathFormat)
      {
         Transaction = transaction;

         OriginalInputPath = path;
         InputPath = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);
         IsRelativePath = !Path.IsPathRooted(OriginalInputPath, false);

         // .NET behaviour.
         SearchPattern = searchPattern.TrimEnd(Path.TrimEndChars);

         FileSystemObjectType = null;

         ContinueOnException = (options & DirectoryEnumerationOptions.ContinueOnException) != 0;

         AsLongPath = (options & DirectoryEnumerationOptions.AsLongPath) != 0;

         AsString = typeOfT == typeof(string);
         AsFileSystemInfo = !AsString && (typeOfT == typeof(FileSystemInfo) || typeOfT.BaseType == typeof(FileSystemInfo));

         FindExInfoLevel = NativeMethods.IsAtLeastWindows7 && (options & DirectoryEnumerationOptions.BasicSearch) != 0
            ? NativeMethods.FINDEX_INFO_LEVELS.Basic
            : NativeMethods.FINDEX_INFO_LEVELS.Standard;

         LargeCache = NativeMethods.IsAtLeastWindows7 && (options & DirectoryEnumerationOptions.LargeCache) != 0
            ? NativeMethods.FindExAdditionalFlags.LargeFetch
            : NativeMethods.FindExAdditionalFlags.None;

         IsDirectory = isFolder;

         if (IsDirectory)
         {
            // Need files or folders to enumerate.
            if ((options & DirectoryEnumerationOptions.FilesAndFolders) == 0)
               options |= DirectoryEnumerationOptions.FilesAndFolders;

            FileSystemObjectType = (options & DirectoryEnumerationOptions.FilesAndFolders) == DirectoryEnumerationOptions.FilesAndFolders
               ? (bool?) null
               : (options & DirectoryEnumerationOptions.Folders) != 0;

            Recursive = (options & DirectoryEnumerationOptions.Recursive) != 0;

            SkipReparsePoints = (options & DirectoryEnumerationOptions.SkipReparsePoints) != 0;
         }
      }




      private void ThrowPossibleException(uint lastError, string pathLp)
      {
         //Answer

         switch (lastError)
         {
            case Win32Errors.ERROR_NO_MORE_FILES:
               lastError = Win32Errors.NO_ERROR;
               break;

            case Win32Errors.ERROR_FILE_NOT_FOUND:
            case Win32Errors.ERROR_PATH_NOT_FOUND:
               // MSDN: .NET 3.5+: DirectoryNotFoundException: Path is invalid, such as referring to an unmapped drive.
               // Directory.Delete()

               lastError = IsDirectory ? (int) Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND;
               break;


            //case Win32Errors.ERROR_DIRECTORY:
            //   // MSDN: .NET 3.5+: IOException: path is a file name.
            //   // Directory.EnumerateDirectories()
            //   // Directory.EnumerateFiles()
            //   // Directory.EnumerateFileSystemEntries()
            //   // Directory.GetDirectories()
            //   // Directory.GetFiles()
            //   // Directory.GetFileSystemEntries()
            //   break;

            //case Win32Errors.ERROR_ACCESS_DENIED:
            //   // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
            //   break;
         }

         if (lastError != Win32Errors.NO_ERROR)
            NativeError.ThrowException(lastError, pathLp);
      }


      private SafeFindFileHandle FindFirstFile(string pathLp, out NativeMethods.WIN32_FIND_DATA win32FindData)
      {
         var searchOption = null != FileSystemObjectType && (bool)FileSystemObjectType
            ? NativeMethods.FINDEX_SEARCH_OPS.SearchLimitToDirectories
            : NativeMethods.FINDEX_SEARCH_OPS.SearchNameMatch;


         var handle = Transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // FindFirstFileEx() / FindFirstFileTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            // A trailing backslash is not allowed.
            ? NativeMethods.FindFirstFileEx(Path.RemoveTrailingDirectorySeparator(pathLp, false), FindExInfoLevel, out win32FindData, searchOption, IntPtr.Zero, LargeCache)
            : NativeMethods.FindFirstFileTransacted(Path.RemoveTrailingDirectorySeparator(pathLp, false), FindExInfoLevel, out win32FindData, searchOption, IntPtr.Zero, LargeCache, Transaction.SafeHandle);

         var lastError = Marshal.GetLastWin32Error();

         if (handle.IsInvalid)
         {
            handle.Close();
            handle = null;

            if (!ContinueOnException)
               ThrowPossibleException((uint)lastError, pathLp);
         }

         return handle;
      }


      private T NewFileSystemEntryType<T>(bool isFolder, NativeMethods.WIN32_FIND_DATA win32FindData, string fileName, string pathLp)
      {
         // Determine yield, e.g. don't return files when only folders are requested and vice versa.
         if (null != FileSystemObjectType && (!(bool) FileSystemObjectType || !isFolder) && (!(bool) !FileSystemObjectType || isFolder))
            return (T) (object) null;

         // Determine yield.
         if (null != fileName && !(_nameFilter == null || (_nameFilter != null && _nameFilter.IsMatch(fileName))))
            return (T) (object) null;


         var fullPathLp = (IsRelativePath ? OriginalInputPath + Path.DirectorySeparator : pathLp) + (!Utils.IsNullOrWhiteSpace(fileName) ? fileName : string.Empty);


         // Return object instance FullPath property as string, optionally in long path format.
         if (AsString)
            return (T) (object) (AsLongPath ? fullPathLp : Path.GetRegularPathCore(fullPathLp, GetFullPathOptions.None, false));


         // Make sure the requested file system object type is returned.
         // null = Return files and directories.
         // true = Return only directories.
         // false = Return only files.

         var fsei = new FileSystemEntryInfo(win32FindData) {FullPath = fullPathLp};

         return AsFileSystemInfo
            // Return object instance of type FileSystemInfo.
            ? (T) (object) (fsei.IsDirectory
               ? (FileSystemInfo)
               new DirectoryInfo(Transaction, fsei.LongFullPath, PathFormat.LongFullPath) {EntryInfo = fsei}
               : new FileInfo(Transaction, fsei.LongFullPath, PathFormat.LongFullPath) {EntryInfo = fsei})

            // Return object instance of type FileSystemEntryInfo.
            : (T) (object) fsei;
      }




      /// <summary>Get an enumerator that returns all of the file system objects that match the wildcards that are in any of the directories to be searched.</summary>
      /// <returns>An <see cref="IEnumerable{T}"/> instance: FileSystemEntryInfo, DirectoryInfo, FileInfo or string (full path).</returns>
      [SecurityCritical]
      public IEnumerable<T> Enumerate<T>()
      {
         // MSDN: Queue
         // Represents a first-in, first-out collection of objects.
         // The capacity of a Queue is the number of elements the Queue can hold.
         // As elements are added to a Queue, the capacity is automatically increased as required through reallocation. The capacity can be decreased by calling TrimToSize.
         // The growth factor is the number by which the current capacity is multiplied when a greater capacity is required. The growth factor is determined when the Queue is constructed.
         // The capacity of the Queue will always increase by a minimum value, regardless of the growth factor; a growth factor of 1.0 will not prevent the Queue from increasing in size.
         // If the size of the collection can be estimated, specifying the initial capacity eliminates the need to perform a number of resizing operations while adding elements to the Queue.
         // This constructor is an O(n) operation, where n is capacity.

         var dirs = new Queue<string>(1000);
         dirs.Enqueue(InputPath);

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (dirs.Count > 0)
            {
               // Removes the object at the beginning of your Queue.
               // The algorithmic complexity of this is O(1). It doesn't loop over elements.

               var path = Path.AddTrailingDirectorySeparator(dirs.Dequeue(), false);
               var pathLp = path + Path.WildcardStarMatchAll;
               NativeMethods.WIN32_FIND_DATA win32FindData;

               using (var handle = FindFirstFile(pathLp, out win32FindData))
               {
                  if (handle == null && ContinueOnException)
                     continue;

                  do
                  {
                     var fileName = win32FindData.cFileName;

                     // Skip entries "." and ".."
                     if (fileName.Equals(Path.CurrentDirectoryPrefix, StringComparison.OrdinalIgnoreCase) ||
                         fileName.Equals(Path.ParentDirectoryPrefix, StringComparison.OrdinalIgnoreCase))
                        continue;

                     // Skip reparse points here to cleanly separate regular directories from links.
                     if (SkipReparsePoints && (win32FindData.dwFileAttributes & FileAttributes.ReparsePoint) != 0)
                        continue;


                     // If object is a folder, add it to the queue for later traversal.
                     var isFolder = (win32FindData.dwFileAttributes & FileAttributes.Directory) != 0;

                     if (Recursive && (win32FindData.dwFileAttributes & FileAttributes.Directory) != 0)
                        dirs.Enqueue(path + fileName);


                     var res = NewFileSystemEntryType<T>(isFolder, win32FindData, fileName, path);
                     if (res == null)
                        continue;

                     yield return res;


                  } while (NativeMethods.FindNextFile(handle, out win32FindData));


                  var lastError = Marshal.GetLastWin32Error();

                  if (!ContinueOnException)
                     ThrowPossibleException((uint)lastError, pathLp);
               }
            }
      }


      /// <summary>Gets a specific file system object.</summary>
      /// <returns>
      /// <para>The return type is based on C# inference. Possible return types are:</para>
      /// <para> <see cref="string"/>- (full path), <see cref="FileSystemInfo"/>- (<see cref="DirectoryInfo"/> or <see cref="FileInfo"/>), <see cref="FileSystemEntryInfo"/> instance</para>
      /// <para>or null in case an Exception is raised and <see cref="ContinueOnException"/> is <see langword="true"/>.</para>
      /// </returns>
      [SecurityCritical]
      public T Get<T>()
      {
         NativeMethods.WIN32_FIND_DATA win32FindData;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         using (var handle = FindFirstFile(InputPath, out win32FindData))
            return handle == null
               ? (T)(object)null
               : NewFileSystemEntryType<T>((win32FindData.dwFileAttributes & FileAttributes.Directory) != 0, win32FindData, null, InputPath);
      }




      /// <summary>Gets or sets the ability to return the object as a <see cref="FileSystemInfo"/> instance.</summary>
      /// <value><see langword="true"/> returns the object as a <see cref="FileSystemInfo"/> instance.</value>
      public bool AsFileSystemInfo { get; internal set; }


      /// <summary>Gets or sets the ability to return the full path in long full path format.</summary>
      /// <value><see langword="true"/> returns the full path in long full path format, <see langword="false"/> returns the full path in regular path format.</value>
      public bool AsLongPath { get; internal set; }


      /// <summary>Gets or sets the ability to return the object instance as a <see cref="string"/>.</summary>
      /// <value><see langword="true"/> returns the full path of the object as a <see cref="string"/></value>
      public bool AsString { get; internal set; }


      /// <summary>Gets the value indicating which <see cref="NativeMethods.FINDEX_INFO_LEVELS"/> to use.</summary>
      public NativeMethods.FINDEX_INFO_LEVELS FindExInfoLevel { get; internal set; }


      /// <summary>Gets or sets the ability to skip on access errors.</summary>
      /// <value><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as ACLs protected directories or non-accessible reparse points.</value>
      public bool ContinueOnException { get; internal set; }


      /// <summary>Gets the file system object type.</summary>
      /// <value>
      /// <see langword="null"/> = Return files and directories.
      /// <see langword="true"/> = Return only directories.
      /// <see langword="false"/> = Return only files.
      /// </value>
      public bool? FileSystemObjectType { get; set; }


      /// <summary>Gets or sets if the path is an absolute or relative path.</summary>
      /// <value>Gets a value indicating whether the specified path string contains absolute or relative path information.</value>
      public bool IsRelativePath { get; set; }


      /// <summary>Gets or sets the initial path to the folder.</summary>
      /// <value>The initial path to the file or folder in long path format.</value>
      public string OriginalInputPath { get; internal set; }


      /// <summary>Gets or sets the path to the folder.</summary>
      /// <value>The path to the file or folder in long path format.</value>
      public string InputPath { get; internal set; }


      /// <summary>Gets or sets a value indicating which <see cref="NativeMethods.FINDEX_INFO_LEVELS"/> to use.</summary>
      /// <value><see langword="true"/> indicates a folder object, <see langword="false"/> indicates a file object.</value>
      public bool IsDirectory { get; internal set; }


      /// <summary>Gets the value indicating which <see cref="NativeMethods.FindExAdditionalFlags"/> to use.</summary>
      public NativeMethods.FindExAdditionalFlags LargeCache { get; internal set; }


      /// <summary>Specifies whether the search should include only the current directory or should include all subdirectories.</summary>
      /// <value><see langword="true"/> to all subdirectories.</value>
      public bool Recursive { get; internal set; }


      /// <summary>Search for file system object-name using a pattern.</summary>
      /// <value>The path which has wildcard characters, for example, an asterisk (<see cref="Path.WildcardStarMatchAll"/>) or a question mark (<see cref="Path.WildcardQuestion"/>).</value>
      [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
      public string SearchPattern
      {
         get { return _searchPattern; }

         internal set
         {
            if (null == value)
               throw new ArgumentNullException("SearchPattern");

            _searchPattern = value;

            _nameFilter = _searchPattern == Path.WildcardStarMatchAll || WildcardMatchAll.IsMatch(_searchPattern)
               ? null
               : new Regex(string.Format(CultureInfo.CurrentCulture, "^{0}$", Regex.Escape(_searchPattern).Replace(@"\*", ".*").Replace(@"\?", ".")), RegexOptions.IgnoreCase | RegexOptions.Compiled);
         }
      }


      /// <summary><see langword="true"/> skips ReparsePoints, <see langword="false"/> will follow ReparsePoints.</summary>
      public bool SkipReparsePoints { get; internal set; }


      /// <summary>Get or sets the KernelTransaction instance.</summary>
      /// <value>The transaction.</value>
      public KernelTransaction Transaction { get; internal set; }
   }
}

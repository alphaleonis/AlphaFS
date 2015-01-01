/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
      #region Methods

      #region FindFirstFile

      private SafeFindFileHandle FindFirstFile(string pathLp, out NativeMethods.Win32FindData win32FindData)
      {
         SafeFindFileHandle handle = Transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // FindFirstFileEx() / FindFirstFileTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            // A trailing backslash is not allowed.
            ? NativeMethods.FindFirstFileEx(Path.RemoveDirectorySeparator(pathLp, false), _basicSearch, out win32FindData, _limitSearchToDirs, IntPtr.Zero, _largeCache)
            : NativeMethods.FindFirstFileTransacted(Path.RemoveDirectorySeparator(pathLp, false), _basicSearch, out win32FindData, _limitSearchToDirs, IntPtr.Zero, _largeCache, Transaction.SafeHandle);

         if (handle.IsInvalid)
         {
            int lastError = Marshal.GetLastWin32Error();
            handle.Close();

            if (!ContinueOnException)
            {
               // Use path without any search filter.
               string path = Path.GetDirectoryName(pathLp, false);

               switch ((uint) lastError)
               {
                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                     // MSDN: .NET 3.5+: DirectoryNotFoundException: Path is invalid, such as referring to an unmapped drive.
                     if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND)
                        lastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;
                     NativeError.ThrowException(lastError, path);
                     break;

                  case Win32Errors.ERROR_DIRECTORY:
                     // MSDN: .NET 3.5+: IOException: path is a file name.
                     NativeError.ThrowException(lastError, path, true);
                     break;

                  case Win32Errors.ERROR_ACCESS_DENIED:
                     // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
                     NativeError.ThrowException(lastError, path);
                     break;
               }

               // MSDN: .NET 3.5+: IOException
               NativeError.ThrowException(lastError, path, true);
            }
         }

         return handle;
      }

      #endregion // FindFirstFile


      #region Enumerate

      /// <summary>Get an enumerator that returns all of the file system objects that match the wildcards that are in any of the directories to be searched.</summary>
      /// <returns>An <see cref="T:IEnumerable{T}"/> instance: FileSystemEntryInfo, DirectoryInfo, FileInfo or string (full- or long path).</returns>
      [SecurityCritical]
      public IEnumerable<T> Enumerate<T>()
      {
         bool? fileSystemObjectType = FileSystemObjectType;
         bool asFileSystemEntryInfo = !AsString && !AsFileSystemInfo;

         // MSDN: Queue
         // Represents a first-in, first-out collection of objects.
         // The capacity of a Queue is the number of elements the Queue can hold.
         // As elements are added to a Queue, the capacity is automatically increased as required through reallocation. The capacity can be decreased by calling TrimToSize.
         // The growth factor is the number by which the current capacity is multiplied when a greater capacity is required. The growth factor is determined when the Queue is constructed.
         // The capacity of the Queue will always increase by a minimum value, regardless of the growth factor; a growth factor of 1.0 will not prevent the Queue from increasing in size.
         // If the size of the collection can be estimated, specifying the initial capacity eliminates the need to perform a number of resizing operations while adding elements to the Queue.
         // This constructor is an O(n) operation, where n is capacity.

         Queue<string> dirs = new Queue<string>(1000);

         // Removes the object at the beginning of your Queue.
         // The algorithmic complexity of this is O(1). It doesn't loop over elements.
         dirs.Enqueue(InputPath);
         
         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         while (dirs.Count > 0)
         {
            string path = Path.AddDirectorySeparator(dirs.Dequeue(), false);
            string pathLp = path + Path.WildcardStarMatchAll;
            NativeMethods.Win32FindData win32FindData;

            using (SafeFindFileHandle handle = FindFirstFile(pathLp, out win32FindData))
            {
               if (handle.IsInvalid && ContinueOnException)
                  continue;

               do
               {
                  string fileName = win32FindData.FileName;

                  // Skip entries.
                  if (fileName.Equals(".", StringComparison.OrdinalIgnoreCase) || fileName.Equals("..", StringComparison.OrdinalIgnoreCase))
                     continue;

                  // Skip reparse points here to cleanly separate regular directories from links.
                  if (SkipReparsePoints && (win32FindData.FileAttributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                     continue;

                  
                  string fullPathLp = path + fileName;
                  bool isDirectory = (win32FindData.FileAttributes & FileAttributes.Directory) == FileAttributes.Directory;

                  // If object is a directory, add it to the queue for later traversal.
                  if (isDirectory && Recursive)
                     dirs.Enqueue(fullPathLp);


                  if (!(_nameFilter == null || (_nameFilter != null && _nameFilter.IsMatch(fileName))))
                     continue;


                  // Make sure the requested file system object type is returned.
                  // null = Return files and directories.
                  // true = Return only directories.
                  // false = Return only files.

                  if (fileSystemObjectType == null || ((bool) fileSystemObjectType && isDirectory || (bool) !fileSystemObjectType && !isDirectory))
                  {
                     FileSystemEntryInfo fsei = new FileSystemEntryInfo(win32FindData) {FullPath = fullPathLp};

                     yield return asFileSystemEntryInfo
                        ? (T) (object) fsei

                        : (T) (AsString
                           // Return object instance FullPath property as string, optionally in Unicode format.
                           ? (object) (AsLongPath ? fsei.LongFullPath : fsei.FullPath)

                           // Return object instance of type DirectoryInfo or FileInfo.
                           : (fsei.IsDirectory
                              ? (FileSystemInfo)
                                 new DirectoryInfo(Transaction, fsei.LongFullPath, null) {EntryInfo = fsei}
                              : new FileInfo(Transaction, fsei.LongFullPath, null) {EntryInfo = fsei}));
                  }

               } while (NativeMethods.FindNextFile(handle, out win32FindData));


               uint lastError = (uint) Marshal.GetLastWin32Error();
               switch (lastError)
               {
                  case Win32Errors.ERROR_NO_MORE_FILES:
                     lastError = Win32Errors.NO_ERROR;
                     break;

                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                     if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && IsFolder)
                        lastError = Win32Errors.ERROR_PATH_NOT_FOUND;
                     break;
               }

               if (!ContinueOnException && lastError != Win32Errors.NO_ERROR)
                  NativeError.ThrowException(lastError, InputPath);
            }
         }
      }

      #endregion // Enumerate

      #region Get

      /// <summary>Gets a specific file system object.</summary>
      /// <returns>An <see cref="T:IEnumerable{FileSystemEntryInfo}"/> instance.</returns>
      [SecurityCritical]
      public FileSystemEntryInfo Get()
      {
         NativeMethods.Win32FindData win32FindData;

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         using (SafeFindFileHandle handle = FindFirstFile(InputPath, out win32FindData))
         {
            return handle.IsInvalid ? null : new FileSystemEntryInfo(win32FindData) {FullPath = InputPath};
         }
      }

      #endregion // Get
      
      #endregion // Methods

      #region Properties

      #region AsFileSystemInfo

      /// <summary>Gets or sets the ability to return the object as a <see cref="T:FileSystemInfo"/> instance.</summary>
      /// <value><c>true</c> returns the object as a <see cref="T:FileSystemInfo"/> instance.</value>
      public bool AsFileSystemInfo { get; set; }

      #endregion // AsFileSystemInfo

      #region AsLongPath

      /// <summary>Gets or sets the ability to return the full path in Unicode format.</summary>
      /// <value><c>true</c> returns the full path in Unicode format, <c>false</c> returns the full path in regular path format.</value>
      public bool AsLongPath { get; set; }

      #endregion // AsLongPath

      #region AsString

      /// <summary>Gets or sets the ability to return the object instance as a <see cref="T:string"/>.</summary>
      /// <value><c>true</c> returns the full path of the object as a <see cref="T:string"/></value>
      public bool AsString { get; set; }

      #endregion // AsString

      #region BasicSearch

      private NativeMethods.FindExInfoLevels _basicSearch = NativeMethods.BasicSearch;

      /// <summary>Gets or sets a value indicating which <see cref="T:NativeMethods.FindExInfoLevels"/> to use.</summary>
      /// <value><c>true</c> uses <see cref="T:NativeMethods.FindExInfoLevels.Basic"/>, otherwise uses <see cref="T:NativeMethods.FindExInfoLevels.Standard"/></value>
      public bool BasicSearch
      {
         get { return _basicSearch == NativeMethods.FindExInfoLevels.Basic; }

         set
         {
            // Verify that BasicSearch is available on Operating System.
            _basicSearch = value && NativeMethods.IsAtLeastWindows7
               ? NativeMethods.FindExInfoLevels.Basic
               : NativeMethods.FindExInfoLevels.Standard;
         }
      }

      #endregion // BasicSearch

      #region ContinueOnException

      /// <summary>Gets or sets the ability to skip on access errors.</summary>
      /// <value><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</value>
      public bool ContinueOnException { get; set; }

      #endregion // ContinueOnException

      #region Fallback

      /// <summary>Gets or sets the ability to fallback on function GetFileAttributesXxx() when function FindFirstFileXxx() fails.</summary>
      /// <value><c>true</c> enable fallback, <c>false</c> disable fallback.</value>
      public bool Fallback { get; set; }

      #endregion // Fallback

      #region FileSystemObjectType

      private NativeMethods.FindExSearchOps _limitSearchToDirs = NativeMethods.FindExSearchOps.SearchNameMatch;
      private bool? _fileSystemObjectType;

      /// <summary>Gets the file system object type.</summary>
      /// <value>
      /// <c>null</c> = Return files and directories.
      /// <c>true</c> = Return only directories.
      /// <c>false</c> = Return only files.
      /// </value>
      public bool? FileSystemObjectType
      {
         get { return _fileSystemObjectType; }

         set
         {
            _fileSystemObjectType = value;

            _limitSearchToDirs = value != null && (bool) value
               ? NativeMethods.FindExSearchOps.SearchLimitToDirectories
               : NativeMethods.FindExSearchOps.SearchNameMatch;
         }
      }

      #endregion // FileSystemObjectType

      #region InputPath

      private string _inputPath;

      /// <summary>Gets or sets the path to the folder.</summary>
      /// <value>The path to the folder.</value>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      public string InputPath
      {
         get { return _inputPath; }

         set
         {
            _inputPath = IsFullPath == null
               ? value
               : (bool) IsFullPath
                  ? Path.GetLongPathInternal(value, false, false, false, false)
                  : Path.GetFullPathInternal(Transaction, value, true, false, false, true, false, true, true);
         }
      }

      #endregion // InputPath

      #region IsFolder

      /// <summary>Gets or sets a value indicating which <see cref="T:NativeMethods.FindExInfoLevels"/> to use.</summary>
      /// <value><c>null</c> indicates the file system object is unknown, <c>true</c> indicates a folder object, <c>false</c> indicates a file object.</value>
      public bool IsFolder { get; set; }

      #endregion // IsFolder

      #region IsFullPath

      /// <summary><c>true</c> uses the path as is.</summary>
      public bool? IsFullPath { get; set; }

      #endregion // IsFullPath

      #region LargeCache

      private NativeMethods.FindExAdditionalFlags _largeCache = NativeMethods.LargeCache;

      /// <summary>Gets or sets a value indicating which <see cref="T:NativeMethods.FindExAdditionalFlags"/> to use.</summary>
      /// <value><c>true</c> uses <see cref="T:NativeMethods.FindExAdditionalFlags.LargeFetch"/>, otherwise uses <see cref="T:NativeMethods.FindExAdditionalFlags.None"/></value>
      public bool LargeCache
      {
         get { return _largeCache == NativeMethods.FindExAdditionalFlags.LargeFetch; }

         set
         {
            // Verify that LargeCache is available on Operating System.
            _largeCache = value && NativeMethods.IsAtLeastWindows7
               ? NativeMethods.FindExAdditionalFlags.LargeFetch
               : NativeMethods.FindExAdditionalFlags.None;
         }
      }

      #endregion // Cache

      #region Recursive

      /// <summary>Specifies whether the search should include only the current directory or should include all subdirectories.</summary>
      /// <value><c>true</c> to all subdirectories.</value>
      public bool Recursive { get; set; }

      #endregion // Recursive

      #region SearchPattern

      private string _searchPattern = Path.WildcardStarMatchAll;
      private Regex _nameFilter;

      /// <summary>Search for file system object-name using a pattern.</summary>
      /// <value>The path which has wildcard characters, for example, an asterisk (<see cref="T:Path.WildcardStarMatchAll"/>) or a question mark (<see cref="T:Path.WildcardQuestion"/>).</value>
      public string SearchPattern
      {
         get { return _searchPattern; }

         set
         {
            if (value == null)
               throw new ArgumentNullException("value");

            if (!Utils.IsNullOrWhiteSpace(value))
               _searchPattern = value;

            _nameFilter = _searchPattern == Path.WildcardStarMatchAll
               ? null
               : new Regex("^" + Regex.Escape(_searchPattern).Replace(@"\.", ".").Replace(@"\*", ".*").Replace(@"\?", ".?") + "$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
         }
      }

      #endregion // SearchPattern

      #region SkipReparsePoints

      /// <summary><c>true</c> skips ReparsePoints, <c>false</c> will follow ReparsePoints.</summary>
      public bool SkipReparsePoints { get; set; }

      #endregion // SkipReparsePoints

      #region Transaction

      /// <summary>Get or sets the KernelTransaction instance.</summary>
      /// <value>The transaction.</value>
      public KernelTransaction Transaction { get; set; }

      #endregion // Transaction

      #endregion // Properties
   }
}
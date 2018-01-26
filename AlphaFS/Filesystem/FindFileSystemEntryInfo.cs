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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class that retrieves file system entries (i.e. files and directories) using Win32 API FindFirst()/FindNext().</summary>
   [Serializable]
   internal sealed class FindFileSystemEntryInfo
   {
      private static readonly Regex WildcardMatchAll = new Regex(@"^(\*)+(\.\*+)+$", RegexOptions.IgnoreCase | RegexOptions.Compiled); // special case to recognize *.* or *.** etc
      private Regex _nameFilter;
      private string _searchPattern = Path.WildcardStarMatchAll;


      /// <summary>Initializes a new instance of the <see cref="FindFileSystemEntryInfo"/> class.</summary>
      /// <param name="transaction">The NTFS Kernel transaction, if used.</param>
      /// <param name="isFolder">if set to <c>true</c> the path is a folder.</param>
      /// <param name="path">The path.</param>
      /// <param name="searchPattern">The wildcard search pattern.</param>
      /// <param name="options">The enumeration options.</param>
      /// <param name="customFilters">The custom filters.</param>
      /// <param name="pathFormat">The format of the path.</param>
      /// <param name="typeOfT">The type of objects to be retrieved.</param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      public FindFileSystemEntryInfo(KernelTransaction transaction, bool isFolder, string path, string searchPattern, DirectoryEnumerationOptions? options, DirectoryEnumerationFilters customFilters, PathFormat pathFormat, Type typeOfT)
      {
         if (null == options)
            throw new ArgumentNullException("options");


         Transaction = transaction;
         
         OriginalInputPath = path;

         InputPath = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);
         
         IsRelativePath = !Path.IsPathRooted(OriginalInputPath, false);

         SearchPattern = searchPattern.TrimEnd(Path.TrimEndChars); // .NET behaviour.

         FileSystemObjectType = null;
         
         ContinueOnException = (options & DirectoryEnumerationOptions.ContinueOnException) != 0;
         
         AsLongPath = (options & DirectoryEnumerationOptions.AsLongPath) != 0;
         
         AsString = typeOfT == typeof(string);
         AsFileSystemInfo = !AsString && (typeOfT == typeof(FileSystemInfo) || typeOfT.BaseType == typeof(FileSystemInfo));

         LargeCache = (options & DirectoryEnumerationOptions.LargeCache) != 0 ? NativeMethods.UseLargeCache : NativeMethods.FIND_FIRST_EX_FLAGS.NONE;

         FindExInfoLevel = (options & DirectoryEnumerationOptions.BasicSearch) != 0 ? NativeMethods.FindexInfoLevel : NativeMethods.FINDEX_INFO_LEVELS.Standard;


         if (null != customFilters)
         {
            Filter = customFilters.InclusionFilter;
            RecursionFilter = customFilters.RecursionFilter;
            ErrorHandler = customFilters.ErrorFilter;
         }


         if (isFolder)
         {
            IsDirectory = true;

            Recursive = (options & DirectoryEnumerationOptions.Recursive) != 0 || null != RecursionFilter;

            SkipReparsePoints = (options & DirectoryEnumerationOptions.SkipReparsePoints) != 0;


            // Need folders or files to enumerate.
            if ((options & DirectoryEnumerationOptions.FilesAndFolders) == 0)
               options |= DirectoryEnumerationOptions.FilesAndFolders;


            //FileSystemObjectType = (options & DirectoryEnumerationOptions.FilesAndFolders) == DirectoryEnumerationOptions.FilesAndFolders
            //   ? (bool?) null
            //   : (options & DirectoryEnumerationOptions.Folders) != 0;
         }

         else
         {
            options &= ~DirectoryEnumerationOptions.Folders; // Remove enumeration of folders.
            options |= DirectoryEnumerationOptions.Files;    // Add enumeration of files.


            //if ((options & DirectoryEnumerationOptions.Folders) == 0)
            //   options &= ~DirectoryEnumerationOptions.Folders;

            //if ((options & DirectoryEnumerationOptions.Files) == 0)
            //   options |= DirectoryEnumerationOptions.Files;
         }


         FileSystemObjectType = (options & DirectoryEnumerationOptions.FilesAndFolders) == DirectoryEnumerationOptions.FilesAndFolders
            
            // Folders and files (null).
            ? (bool?) null

            // Only folders (true) or only files (false).
            : (options & DirectoryEnumerationOptions.Folders) != 0;
      }


      private SafeFindFileHandle FindFirstFile(string pathLp, out NativeMethods.WIN32_FIND_DATA win32FindData, bool suppressException = false)
      {
         int lastError;
         var searchOption = null != FileSystemObjectType && (bool) FileSystemObjectType ? NativeMethods.FINDEX_SEARCH_OPS.SearchLimitToDirectories : NativeMethods.FINDEX_SEARCH_OPS.SearchNameMatch;

         var handle = FileSystemInfo.FindFirstFileNative(Transaction, pathLp, FindExInfoLevel, searchOption, LargeCache, out lastError, out win32FindData);


         if (!suppressException && !ContinueOnException)
         {
            if (null == handle)
            {
               switch ((uint) lastError)
               {
                  case Win32Errors.ERROR_FILE_NOT_FOUND: // FileNotFoundException.
                  case Win32Errors.ERROR_PATH_NOT_FOUND: // DirectoryNotFoundException.
                  case Win32Errors.ERROR_NOT_READY:      // DeviceNotReadyException: Floppy device or network drive not ready.

                     string drive;

                     var driveExists = File.ExistsDrive(Transaction, pathLp, out drive, false);

                     lastError = (int) (!driveExists ? Win32Errors.ERROR_NOT_READY : IsDirectory ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND);

                     NativeError.ThrowException(lastError, null, !driveExists ? drive : pathLp);

                     break;
               }


               ThrowPossibleException((uint) lastError, pathLp);
            }

            // When the handle is null and we are still here, it means the ErrorHandler is active,
            // preventing the Exception from being thrown.

            if (null != handle)
               VerifyInstanceType(win32FindData);
         }


         return handle;
      }
      
      
      private T NewFileSystemEntryType<T>(bool isFolder, NativeMethods.WIN32_FIND_DATA win32FindData, string fileName, string pathLp)
      {
         // Determine yield, e.g. don't return files when only folders are requested and vice versa.
         if (null != FileSystemObjectType && (!(bool) FileSystemObjectType || !isFolder) && (!(bool) !FileSystemObjectType || isFolder))
            return (T) (object) null;


         // Determine yield.
         if (null != fileName && !(_nameFilter == null || _nameFilter != null && _nameFilter.IsMatch(fileName)))
            return (T) (object) null;


         var fullPathLp = (IsRelativePath ? OriginalInputPath + Path.DirectorySeparator : pathLp) + fileName;

         var fsei = new FileSystemEntryInfo(win32FindData) {FullPath = fullPathLp};


         // Return object instance FullPath property as string, optionally in long path format.

         return AsString
            ? null == Filter || Filter(fsei)
               ? (T) (object) (AsLongPath ? fullPathLp : Path.GetRegularPathCore(fullPathLp, GetFullPathOptions.None, false))
               : (T) (object) null


            // Make sure the requested file system object type is returned.
            // null = Return files and directories.
            // true = Return only directories.
            // false = Return only files.

            : null != Filter && !Filter(fsei)
               ? (T) (object) null

               // Return object instance of type FileSystemInfo.

               : AsFileSystemInfo
                  ? (T) (object) (fsei.IsDirectory

                     ? (FileSystemInfo) new DirectoryInfo(Transaction, fsei.LongFullPath, PathFormat.LongFullPath) {EntryInfo = fsei}

                     : new FileInfo(Transaction, fsei.LongFullPath, PathFormat.LongFullPath) {EntryInfo = fsei})

                  // Return object instance of type FileSystemEntryInfo.

                  : (T) (object) fsei;
      }


      private void ThrowPossibleException(uint lastError, string pathLp)
      {
         switch (lastError)
         {
            case Win32Errors.ERROR_NO_MORE_FILES:
               lastError = Win32Errors.NO_ERROR;
               break;


            case Win32Errors.ERROR_FILE_NOT_FOUND:
            case Win32Errors.ERROR_PATH_NOT_FOUND:
               // MSDN: .NET 3.5+: DirectoryNotFoundException: Path is invalid, such as referring to an unmapped drive.
               // Directory.Delete()

               lastError = IsDirectory ? (int)Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND;
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
         {
            if (null == ErrorHandler || !ErrorHandler((int) lastError, new Win32Exception((int) lastError).Message, pathLp.TrimEnd(Path.WildcardStarMatchAllChar)))
               NativeError.ThrowException(lastError, pathLp);
         }
      }


      private void VerifyInstanceType(NativeMethods.WIN32_FIND_DATA win32FindData)
      {
         var isFolder = (win32FindData.dwFileAttributes & FileAttributes.Directory) != 0;

         if (IsDirectory)
         {
            if (!isFolder)
               throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "({0}) {1}", Win32Errors.ERROR_PATH_NOT_FOUND, string.Format(CultureInfo.InvariantCulture, Resources.Target_Directory_Is_A_File, InputPath)));
         }

         else if (isFolder)
            throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "({0}) {1}", Win32Errors.ERROR_FILE_NOT_FOUND, string.Format(CultureInfo.InvariantCulture, Resources.Target_File_Is_A_Directory, InputPath)));
      }




      /// <summary>Gets an enumerator that returns all of the file system objects that match both the wildcards that are in any of the directories to be searched and the custom predicate.</summary>
      /// <returns>An <see cref="IEnumerable{T}" /> instance: FileSystemEntryInfo, DirectoryInfo, FileInfo or string (full path).</returns>
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
                  // When the handle is null and we are still here, it means the ErrorHandler is active.
                  // We hit an inaccessible folder, so break and continue with the next one.
                  if (null == handle)
                     continue;

                  do
                  {
                     // Skip reparse points here to cleanly separate regular directories from links.
                     if (SkipReparsePoints && (win32FindData.dwFileAttributes & FileAttributes.ReparsePoint) != 0)
                        continue;


                     var fileName = win32FindData.cFileName;

                     var isFolder = (win32FindData.dwFileAttributes & FileAttributes.Directory) != 0;

                     // Skip entries "." and ".."
                     if (isFolder && (fileName.Equals(Path.CurrentDirectoryPrefix, StringComparison.Ordinal) || fileName.Equals(Path.ParentDirectoryPrefix, StringComparison.Ordinal)))
                        continue;


                     var res = NewFileSystemEntryType<T>(isFolder, win32FindData, fileName, path);


                     // If recursion is requested, add it to the queue for later traversal.

                     if (isFolder && Recursive)
                     {
                        // Is there anything we can take from res?
                        var fsei = null != res
                           ? AsString

                              ? new FileSystemEntryInfo(win32FindData) {FullPath = (string) (object) res}

                              : AsFileSystemInfo ? ((DirectoryInfo) (object) res).EntryInfo : (FileSystemEntryInfo) (object) res

                           
                           // No, create new instance.

                           : new FileSystemEntryInfo(win32FindData) {FullPath = (IsRelativePath ? OriginalInputPath + Path.DirectorySeparator : pathLp) + fileName};


                        if (null == RecursionFilter || RecursionFilter(fsei))
                           dirs.Enqueue(path + fileName);
                     }


                     if (null == res)
                        continue;

                     yield return res;
                     
                  } while (NativeMethods.FindNextFile(handle, out win32FindData));


                  var lastError = Marshal.GetLastWin32Error();

                  if (!ContinueOnException)
                     ThrowPossibleException((uint) lastError, pathLp);
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
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            NativeMethods.WIN32_FIND_DATA win32FindData;

            
            // Not explicitly set to be a folder.

            if (!IsDirectory)
            {
               using (var handle = FindFirstFile(InputPath, out win32FindData))

                  return null == handle

                     ? (T) (object) null

                     : NewFileSystemEntryType<T>((win32FindData.dwFileAttributes & FileAttributes.Directory) != 0, win32FindData, null, InputPath);

            }
            

            using (var handle = FindFirstFile(InputPath, out win32FindData, true))
            {
               if (null == handle)
               {
                  // InputPath might be a drive letter like: C:\, D:\
                  
                  var attrs = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();

                  var lastError = File.FillAttributeInfoCore(Transaction, Path.GetRegularPathCore(InputPath, GetFullPathOptions.None, false), ref attrs, false, true);
                  if (lastError != Win32Errors.NO_ERROR)
                  {
                     if (!ContinueOnException)
                     {
                        switch ((uint) lastError)
                        {
                           case Win32Errors.ERROR_FILE_NOT_FOUND: // FileNotFoundException.
                           case Win32Errors.ERROR_PATH_NOT_FOUND: // DirectoryNotFoundException.
                           case Win32Errors.ERROR_NOT_READY:      // DeviceNotReadyException: Floppy device or network drive not ready.
                           case Win32Errors.ERROR_BAD_NET_NAME:

                              string drive;

                              var driveExists = File.ExistsDrive(Transaction, InputPath, out drive, false);

                              lastError = (int) (!driveExists ? Win32Errors.ERROR_NOT_READY : IsDirectory ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND);

                              NativeError.ThrowException(lastError, null, !driveExists ? drive : InputPath);

                              break;
                        }

                        ThrowPossibleException((uint) lastError, InputPath);
                     }

                     return (T) (object) null;
                  }


                  win32FindData = new NativeMethods.WIN32_FIND_DATA
                  {
                     cFileName = Path.CurrentDirectoryPrefix,
                     dwFileAttributes = attrs.dwFileAttributes,
                     ftCreationTime = attrs.ftCreationTime,
                     ftLastAccessTime = attrs.ftLastAccessTime,
                     ftLastWriteTime = attrs.ftLastWriteTime,
                     nFileSizeHigh = attrs.nFileSizeHigh,
                     nFileSizeLow = attrs.nFileSizeLow
                  };
               }


               VerifyInstanceType(win32FindData);
            }


            return NewFileSystemEntryType<T>((win32FindData.dwFileAttributes & FileAttributes.Directory) != 0, win32FindData, null, InputPath);
         }
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


      /// <summary>Uses a larger buffer for directory queries, which can increase performance of the find operation.</summary>
      /// <remarks>This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
      public NativeMethods.FIND_FIRST_EX_FLAGS LargeCache { get; internal set; }


      /// <summary>The FindFirstFileEx function does not query the short file name, improving overall enumeration speed.</summary>
      /// <remarks>This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
      public NativeMethods.FINDEX_INFO_LEVELS FindExInfoLevel { get; internal set; }


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
               : new Regex(string.Format(CultureInfo.InvariantCulture, "^{0}$", Regex.Escape(_searchPattern).Replace(@"\*", ".*").Replace(@"\?", ".")), RegexOptions.IgnoreCase | RegexOptions.Compiled);
         }
      }


      /// <summary><see langword="true"/> skips ReparsePoints, <see langword="false"/> will follow ReparsePoints.</summary>
      public bool SkipReparsePoints { get; internal set; }


      /// <summary>Get or sets the KernelTransaction instance.</summary>
      /// <value>The transaction.</value>
      public KernelTransaction Transaction { get; internal set; }


      /// <summary>Gets or sets the custom filter.</summary>
      /// <value>The method determining if the object should be excluded from the output or not.</value>
      public Predicate<FileSystemEntryInfo> Filter { get; internal set; }


      /// <summary>Gets or sets the custom filter.</summary>
      /// <value>The method determining if the directory should be recursively traversed or not.</value>
      public Predicate<FileSystemEntryInfo> RecursionFilter { get; internal set; }


      /// <summary>Gets or sets the handler of errors that may occur.</summary>
      /// <value>The error handler method.</value>
      public ErrorHandler ErrorHandler { get; internal set; }
   }
}

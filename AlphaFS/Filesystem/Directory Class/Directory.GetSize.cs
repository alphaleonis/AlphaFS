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
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="path">The path to the directory.</param>
      [SecurityCritical]
      public static long GetSize(string path)
      {
         return GetSizeCore(null, path, false, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="path">The path to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long GetSize(string path, PathFormat pathFormat)
      {
         return GetSizeCore(null, path, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      [SecurityCritical]
      public static long GetSize(string path, bool sizeOfAllStreams)
      {
         return GetSizeCore(null, path, sizeOfAllStreams, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long GetSize(string path, bool sizeOfAllStreams, PathFormat pathFormat)
      {
         return GetSizeCore(null, path, sizeOfAllStreams, false, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="recursive"><c>true</c> to include subdirectories.</param>
      [SecurityCritical]
      public static long GetSize(string path, bool sizeOfAllStreams, bool recursive)
      {
         return GetSizeCore(null, path, sizeOfAllStreams, recursive, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="recursive"><c>true</c> to include subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long GetSize(string path, bool sizeOfAllStreams, bool recursive, PathFormat pathFormat)
      {
         return GetSizeCore(null, path, sizeOfAllStreams, recursive, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path)
      {
         return GetSizeCore(transaction, path, false, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetSizeCore(transaction, path, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, bool sizeOfAllStreams)
      {
         return GetSizeCore(transaction, path, sizeOfAllStreams, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, bool sizeOfAllStreams, PathFormat pathFormat)
      {
         return GetSizeCore(transaction, path, sizeOfAllStreams, false, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="recursive"><c>true</c> to include subdirectories.</param>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, bool sizeOfAllStreams, bool recursive)
      {
         return GetSizeCore(transaction, path, sizeOfAllStreams, recursive, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="recursive"><c>true</c> to include subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long GetSizeTransacted(KernelTransaction transaction, string path, bool sizeOfAllStreams, bool recursive, PathFormat pathFormat)
      {
         return GetSizeCore(transaction, path, sizeOfAllStreams, recursive, pathFormat);
      }




      /// <summary>Retrieves the size of all alternate data streams of the specified directory and it files.</summary>
      /// <returns>The size of all alternate data streams of the specified directory and its files.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the directory.</param>
      /// <param name="sizeOfAllStreams"><c>true</c> to retrieve the size of all alternate data streams, <c>false</c> to get the size of the first stream.</param>
      /// <param name="recursive"><c>true</c> to include subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static long GetSizeCore(KernelTransaction transaction, string path, bool sizeOfAllStreams, bool recursive, PathFormat pathFormat)
      {
         var streamSizes = new Collection<long>();

         var pathLp = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);


         var enumOptions = (recursive ? DirectoryEnumerationOptions.Recursive : DirectoryEnumerationOptions.None) | DirectoryEnumerationOptions.SkipReparsePoints;

         if (sizeOfAllStreams)
         {
            enumOptions |= DirectoryEnumerationOptions.FilesAndFolders;

            streamSizes.Add(File.FindAllStreamsCore(transaction, pathLp));
         }

         else
            enumOptions |= DirectoryEnumerationOptions.Files;


         foreach (var fsei in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, transaction, pathLp, Path.WildcardStarMatchAll, null, enumOptions, null, PathFormat.LongFullPath))
         {
            // Although tempting, AlphaFS does not use the fsei.FileSize property.
            //
            // https://blogs.msdn.microsoft.com/oldnewthing/20111226-00/?p=8813/
            // "The directory-enumeration functions report the last-updated metadata, which may not correspond to the actual metadata if the directory entry is stale. 


            streamSizes.Add(sizeOfAllStreams ? File.FindAllStreamsCore(transaction, fsei.LongFullPath) : File.GetSizeCore(null, transaction, fsei.LongFullPath, false, PathFormat.LongFullPath));
         }


         return streamSizes.Sum();
      }
   }
}

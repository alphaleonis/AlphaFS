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
using System.IO;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class with method arguments that are used by the Directory/File Delete methods.</summary>
   internal class DeleteArguments : BaseArguments
   {
      public DeleteArguments()
      {
      }


      internal DeleteArguments(FileSystemEntryInfo entryInfo)
      {
         if (null == entryInfo)
            throw new ArgumentNullException("entryInfo");

         EntryInfo = entryInfo;

         PathFormat = PathFormat.LongFullPath;

         TargetPath = entryInfo.FullPath;

         TargetPathLongPath = entryInfo.LongFullPath;

         Attributes = entryInfo.Attributes;

         IsDirectory = entryInfo.IsDirectory;
      }

      
      internal bool IsDirectory { get; set; }


      /// <summary>The path to the file or folder to delete.</summary>
      internal string TargetPath { get; set; }


      /// <summary>The path to the file or folder to delete in <see cref="PathFormat.LongFullPath"/> format.</summary>
      internal string TargetPathLongPath { get; set; }


      internal RetryArguments RetryArguments { get; set; }


      internal FileSystemEntryInfo EntryInfo { get; set; }


      internal FileAttributes Attributes { get; set; }


      internal bool IgnoreReadOnly { get; set; }
      

      internal bool Recursive { get; set; }


      internal bool ContinueOnNotFound { get; set; }
   }
}

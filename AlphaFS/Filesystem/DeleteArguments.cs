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
   /// <summary>
   /// 
   /// </summary>
   internal class DeleteArguments : BaseArguments
   {
      /// <summary>
      /// 
      /// </summary>
      public DeleteArguments()
      {
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="entryInfo"></param>
      public DeleteArguments(FileSystemEntryInfo entryInfo)
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




      /// <summary>
      /// 
      /// </summary>
      public bool IsDirectory { get; internal set; }


      /// <summary>The path to the file or folder to delete.</summary>
      public string TargetPath { get; internal set; }


      /// <summary>The path to the file or folder to delete in <see cref="PathFormat.LongFullPath"/> format.</summary>
      public string TargetPathLongPath { get; internal set; }

      
      /// <summary>
      /// 
      /// </summary>
      internal FileSystemEntryInfo EntryInfo { get; private set; }


      /// <summary>
      /// 
      /// </summary>
      public FileAttributes Attributes { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public bool IgnoreReadOnly { get; set; }
      

      /// <summary>
      /// 
      /// </summary>
      public bool Recursive { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public bool ContinueOnNotFound { get; set; }
   }
}

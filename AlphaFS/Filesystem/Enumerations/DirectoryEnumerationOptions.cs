/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>[AlphaFS] Directory enumeration options, flags that specify how a directory is to be enumerated.</summary>
   [Flags]
   public enum DirectoryEnumerationOptions
   {
      /// <summary>None, do not use.</summary>
      None = 0,

      /// <summary>Enumerate files only.</summary>
      Files = 1,

      /// <summary>Enumerate directories only.</summary>
      Folders = 2,

      /// <summary>Enumerate files and folders.</summary>
      FilesAndFolders = Files | Folders,

      /// <summary>Return file/folder full path in Unicode format, only valid when return type is <see cref="string"/>.</summary>
      AsLongPath = 4,

      /// <summary>Do not follow (skip) Reparse Points during enumeration.</summary>
      SkipReparsePoints = 8,

      /// <summary><para>Suppress any Exception that might be thrown a result from a failure,</para>
      /// <para>such as ACLs protected directories or non-accessible reparse points.</para> 
      /// </summary>
      ContinueOnException = 16,

      /// <summary>Specifies whether to search the current directory, or the current directory and all subdirectories.</summary>
      Recursive = 32
   }
}
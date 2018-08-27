﻿/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>[AlphaFS] Retrieves file information for the specified directory.</summary>
      /// <returns>Returns a <see cref="ByHandleFileInfo"/> instance containing the requested information.</returns>
      /// <remarks>Directory IDs are not guaranteed to be unique over time, because file systems are free to reuse them. In some cases, the directory ID for a directory can change over time.</remarks>
      /// <param name="path">The path to the directory.</param>
      [SecurityCritical]
      public static ByHandleFileInfo GetFileInfoByHandle(string path)
      {
         return File.GetFileInfoByHandleCore(null, true, path, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Retrieves file information for the specified directory.</summary>
      /// <returns>Returns a <see cref="ByHandleFileInfo"/> instance containing the requested information.</returns>
      /// <remarks>Directory IDs are not guaranteed to be unique over time, because file systems are free to reuse them. In some cases, the directory ID for a directory can change over time.</remarks>
      /// <param name="path">The path to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static ByHandleFileInfo GetFileInfoByHandle(string path, PathFormat pathFormat)
      {
         return File.GetFileInfoByHandleCore(null, true, path, pathFormat);
      }


      /// <summary>[AlphaFS] Retrieves file information for the specified <see cref="SafeFileHandle"/>.</summary>
      /// <returns>Returns a <see cref="ByHandleFileInfo"/> instance containing the requested information.</returns>
      /// <remarks>Directory IDs are not guaranteed to be unique over time, because file systems are free to reuse them. In some cases, the directory ID for a directory can change over time.</remarks>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the open file or directory from which to retrieve the information.</param>
      [SecurityCritical]
      public static ByHandleFileInfo GetFileInfoByHandle(SafeFileHandle handle)
      {
         return File.GetFileInfoByHandle(handle);
      }
   }
}

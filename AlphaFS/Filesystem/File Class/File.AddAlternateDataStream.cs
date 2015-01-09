/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region AddAlternateDataStream

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void AddAlternateDataStream(string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(false, null, path, name, contents, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>      
      [SecurityCritical]
      public static void AddAlternateDataStream(string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(false, null, path, name, contents, PathFormat.RelativePath);
      }

      #region Transactional

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void AddAlternateDataStream(KernelTransaction transaction, string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(false, transaction, path, name, contents, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>      
      [SecurityCritical]
      public static void AddAlternateDataStream(KernelTransaction transaction, string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(false, transaction, path, name, contents, PathFormat.RelativePath);
      }

      #endregion // Transactional

      #endregion // AddAlternateDataStream
   }
}

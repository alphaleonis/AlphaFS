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

using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region AlphaFS

      #region AddAlternateDataStream

      #region Non-Transactional

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      [SecurityCritical]
      public static void AddAlternateDataStream(string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(true, null, path, name, contents, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AddAlternateDataStream(string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(true, null, path, name, contents, pathFormat);
      }
      
      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      [SecurityCritical]
      public static void AddAlternateDataStream(KernelTransaction transaction, string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(true, transaction, path, name, contents, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AddAlternateDataStream(KernelTransaction transaction, string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddAlternateDataStreamInternal(true, transaction, path, name, contents, pathFormat);
      }

      #endregion // Transactional

      #endregion // AddAlternateDataStream
      
      #region EnumerateAlternateDataStreams

      #region Non-Transactional

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, null, null, path, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, null, null, path, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, null, null, path, null, streamType, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, null, null, path, null, streamType, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, null, handle, null, null, null, PathFormat.LongFullPath);
      }

      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, transaction, null, path, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, transaction, null, path, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, transaction, null, path, null, streamType, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateAlternateDataStreams(KernelTransaction transaction, string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateAlternateDataStreamsInternal(null, transaction, null, path, null, streamType, pathFormat);
      }
      
      #endregion // Transactional

      #endregion // EnumerateAlternateDataStreams

      #region GetAlternateDataStreamSize

      #region Non-Transactional

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(string path)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, null, path, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, null, path, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(string path, string name)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, null, path, name, StreamType.Data, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, null, path, name, StreamType.Data, pathFormat);
      }



      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, null, path, null, type, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, null, path, null, type, pathFormat);
      }

      

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(SafeFileHandle handle, string name)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, handle, null, name, StreamType.Data, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(SafeFileHandle handle, StreamType type)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, null, handle, null, null, type, PathFormat.LongFullPath);
      }

      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, transaction, null, path, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, transaction, null, path, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(KernelTransaction transaction, string path, string name)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, transaction, null, path, name, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, transaction, null, path, name, StreamType.Data, pathFormat);
      }



      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(KernelTransaction transaction, string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, transaction, null, path, null, type, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetAlternateDataStreamSize(KernelTransaction transaction, string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetAlternateDataStreamSizeInternal(true, transaction, null, path, null, type, pathFormat);
      }
      
      #endregion // Transactional

      #endregion GetAlternateDataStreamSize

      #region RemoveAlternateDataStream

      #region Non-Transactional

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(string path)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, null, path, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, null, path, null, pathFormat);
      }



      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, null, path, name, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, null, path, name, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, transaction, path, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, transaction, path, null, pathFormat);
      }



      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, transaction, path, name, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveAlternateDataStream(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveAlternateDataStreamInternal(true, transaction, path, name, pathFormat);
      }

      #endregion // Transactional

      #endregion // RemoveAlternateDataStream

      #endregion // AlphaFS
   }
}

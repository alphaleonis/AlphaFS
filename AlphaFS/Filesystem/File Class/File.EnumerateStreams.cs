using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, pathFormat);
      }


      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by
      ///   <paramref name="handle"/>.
      /// </summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, handle, null, null, null, PathFormat.ExtendedLength);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, PathFormat.Auto);
      }
   }
}

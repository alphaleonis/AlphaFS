using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Non-Transactional

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, pathFormat);
      }

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>      
      [SecurityCritical]
      public static void RemoveStream(string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, PathFormat.Auto);
      }
      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, pathFormat);
      }

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, PathFormat.Auto);
      }

      #endregion Transacted

   }
}

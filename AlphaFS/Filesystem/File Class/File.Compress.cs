using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Compress

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Compress(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, null, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>      
      [SecurityCritical]
      public static void Compress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, pathFormat);
      }


      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>      
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, PathFormat.RelativePath);
      }

      #endregion

      #region Decompress

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Decompress(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, null, path, false, pathFormat);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>      
      [SecurityCritical]
      public static void Decompress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, pathFormat);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>      
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, PathFormat.RelativePath);
      }

      #endregion
   }
}

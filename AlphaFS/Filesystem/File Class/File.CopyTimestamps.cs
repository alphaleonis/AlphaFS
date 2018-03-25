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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Obsolete

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [Obsolete("Use new method name: CopyTimestamps")]
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         CopyTimestamps(sourcePath, destinationPath, pathFormat);
      }

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>      
      [Obsolete("Use new method name: CopyTimestamps")]
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath)
      {
         CopyTimestamps(sourcePath, destinationPath, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [Obsolete("Use new method name: CopyTimestampsTransacted")]
      [SecurityCritical]
      public static void TransferTimestampsTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         CopyTimestampsTransacted(transaction, sourcePath, destinationPath, pathFormat);
      }

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>      
      [Obsolete("Use new method name: CopyTimestampsTransacted")]
      [SecurityCritical]
      public static void TransferTimestampsTransacted(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         CopyTimestampsTransacted(transaction, sourcePath, destinationPath, PathFormat.RelativePath);
      }

      #endregion // Obsolete




      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      [SecurityCritical]
      public static void CopyTimestamps(string sourcePath, string destinationPath)
      {
         CopyTimestampsCore(null, sourcePath, destinationPath, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTimestamps(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         CopyTimestampsCore(null, sourcePath, destinationPath, false, pathFormat);
      }


      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="modifyReparsePoint">If <see langword="true"/>, the date and time information will apply to the reparse point (symlink or junction) and not the file linked to. No effect if <paramref name="destinationPath"/> does not refer to a reparse point.</param>
      [SecurityCritical]
      public static void CopyTimestamps(string sourcePath, string destinationPath, bool modifyReparsePoint)
      {
         CopyTimestampsCore(null, sourcePath, destinationPath, modifyReparsePoint, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="modifyReparsePoint">If <see langword="true"/>, the date and time information will apply to the reparse point (symlink or junction) and not the file linked to. No effect if <paramref name="destinationPath"/> does not refer to a reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTimestamps(string sourcePath, string destinationPath, bool modifyReparsePoint, PathFormat pathFormat)
      {
         CopyTimestampsCore(null, sourcePath, destinationPath, modifyReparsePoint, pathFormat);
      }




      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      [SecurityCritical]
      public static void CopyTimestampsTransacted(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         CopyTimestampsCore(transaction, sourcePath, destinationPath, false, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTimestampsTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         CopyTimestampsCore(transaction, sourcePath, destinationPath, false, pathFormat);
      }


      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="modifyReparsePoint">If <see langword="true"/>, the date and time information will apply to the reparse point (symlink or junction) and not the file linked to. No effect if <paramref name="destinationPath"/> does not refer to a reparse point.</param>
      [SecurityCritical]
      public static void CopyTimestampsTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, bool modifyReparsePoint)
      {
         CopyTimestampsCore(transaction, sourcePath, destinationPath, modifyReparsePoint, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="modifyReparsePoint">If <see langword="true"/>, the date and time information will apply to the reparse point (symlink or junction) and not the file linked to. No effect if <paramref name="destinationPath"/> does not refer to a reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTimestampsTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, bool modifyReparsePoint, PathFormat pathFormat)
      {
         CopyTimestampsCore(transaction, sourcePath, destinationPath, modifyReparsePoint, pathFormat);
      }


      /// <summary>Transfer the date and time stamps for the specified files and directories.</summary>
      /// <remarks>
      ///   <para>This method does not change last access time for the source file.</para>
      ///   <para>This method uses BackupSemantics flag to get Timestamp changed for directories.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source path.</param>
      /// <param name="destinationPath">The destination path.</param>
      /// <param name="modifyReparsePoint">If <see langword="true"/>, the date and time information will apply to the reparse point (symlink or junction) and not the file or directory linked to. No effect if <paramref name="destinationPath"/> does not refer to a reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void CopyTimestampsCore(KernelTransaction transaction, string sourcePath, string destinationPath, bool modifyReparsePoint, PathFormat pathFormat)
      {
         var attrs = GetAttributesExCore<NativeMethods.WIN32_FILE_ATTRIBUTE_DATA>(transaction, sourcePath, pathFormat, true);

         SetFsoDateTimeCore(transaction, destinationPath, DateTime.FromFileTimeUtc(attrs.ftCreationTime),
            DateTime.FromFileTimeUtc(attrs.ftLastAccessTime), DateTime.FromFileTimeUtc(attrs.ftLastWriteTime), modifyReparsePoint, pathFormat);
      }
   }
}

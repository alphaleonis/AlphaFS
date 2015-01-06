using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Public Methods

      /// <summary>Determines whether the specified file exists.</summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/> parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to
      ///   determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters, a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      ///   <para>The Exists method should not be used for path validation,
      ///   this method merely checks if the file specified in path exists.</para>
      ///   <para>Passing an invalid path to Exists returns false.</para>
      ///   <para>Be aware that another process can potentially do something with the file in
      ///   between the time you call the Exists method and perform another operation on the file, such as Delete.</para>
      /// </remarks>
      /// <param name="path">The file to check.</param>
      /// <returns>
      ///   Returns <see langword="true"/> if the caller has the required permissions and
      ///   <paramref name="path"/> contains the name of an existing file; otherwise,
      ///   <see langword="false"/>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(string path)
      {
         return ExistsInternal(false, null, path, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Determines whether the specified file exists.</summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/> parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to
      ///   determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      ///   <para>The Exists method should not be used for path validation, this method merely checks
      ///   if the file specified in path exists.</para>
      ///   <para>Passing an invalid path to Exists returns false.</para>
      ///   <para>Be aware that another process can potentially do something with the file in
      ///   between the time you call the Exists method and perform another operation on the file, such
      ///   as Delete.</para>
      /// </remarks>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if the caller has the required permissions and
      ///   <paramref name="path"/> contains the name of an existing file; otherwise,
      ///   <see langword="false"/></para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(string path, PathFormat pathFormat)
      {
         return ExistsInternal(false, null, path, pathFormat);
      }

      #region Transacted

      /// <summary>
      ///   [AlphaFS] Determines whether the specified file exists.
      /// </summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/> parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to
      ///   determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      ///   <para>The Exists method should not be used for path validation,</para>
      ///   <para>this method merely checks if the file specified in path exists.</para>
      ///   <para>Passing an invalid path to Exists returns false.</para>
      ///   <para>Be aware that another process can potentially do something with the file in
      ///   between</para>
      ///   <para>the time you call the Exists method and perform another operation on the file, such
      ///   as Delete.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if the caller has the required permissions</para>
      ///   <para>and <paramref name="path"/> contains the name of an existing file; otherwise,
      ///   <see langword="false"/></para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path)
      {
         return ExistsInternal(false, transaction, path, PathFormat.RelativeOrFullPath);
      }

      /// <summary>
      ///   [AlphaFS] Determines whether the specified file exists.
      /// </summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/> parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to
      ///   determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      ///   <para>The Exists method should not be used for path validation,</para>
      ///   <para>this method merely checks if the file specified in path exists.</para>
      ///   <para>Passing an invalid path to Exists returns false.</para>
      ///   <para>Be aware that another process can potentially do something with the file in
      ///   between</para>
      ///   <para>the time you call the Exists method and perform another operation on the file, such
      ///   as Delete.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if the caller has the required permissions</para>
      ///   <para>and <paramref name="path"/> contains the name of an existing file; otherwise,
      ///   <see langword="false"/></para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return ExistsInternal(false, transaction, path, pathFormat);
      }

      #endregion // Transacted

      #endregion

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method ExistsInternal() to determine whether the specified file or directory exists.</summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the <paramref name="path"/> parameter before checking whether
      ///   the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine if the specified file
      ///   exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,
      ///   </para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      ///   <para>The Exists method should not be used for path validation,
      ///   this method merely checks if the file specified in path exists.</para>
      ///   <para>Passing an invalid path to Exists returns false.</para>
      ///   <para>Be aware that another process can potentially do something with the file in between
      ///   the time you call the Exists method and perform another operation on the file, such as Delete.</para>
      /// </remarks>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if the caller has the required permissions</para>
      ///   <para>and <paramref name="path"/> contains the name of an existing file or directory; otherwise, <see langword="false"/></para>
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      internal static bool ExistsInternal(bool isFolder, KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         // Will be caught later and be thrown as an ArgumentException or ArgumentNullException.
         // Let's take a shorter route, preventing an Exception from being thrown altogether.
         if (Utils.IsNullOrWhiteSpace(path))
            return false;


         // DriveInfo.IsReady() will fail.
         //
         //// After normalizing, check whether path ends in directory separator.
         //// Otherwise, FillAttributeInfoInternal removes it and we may return a false positive.
         //string pathRp = Path.GetRegularPathInternal(path, true, false, false, false);

         //if (pathRp.Length > 0 && Path.IsDVsc(pathRp[pathRp.Length - 1], false))
         //   return false;


         try
         {
            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.ContinueOnNonExist);

            var data = new NativeMethods.Win32FileAttributeData();
            int dataInitialised = FillAttributeInfoInternal(transaction, pathLp, ref data, false, true);

            return (dataInitialised == Win32Errors.ERROR_SUCCESS && data.FileAttributes != (FileAttributes)(-1) && (isFolder
               ? (data.FileAttributes & FileAttributes.Directory) == FileAttributes.Directory
               : (data.FileAttributes & FileAttributes.Directory) != FileAttributes.Directory));
         }
         catch
         {
            return false;
         }
      }

      #endregion ExistsInternal
   }
}

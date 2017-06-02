/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.Diagnostics.CodeAnalysis;
using System.IO;
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
         return ExistsCore(false, null, path, PathFormat.RelativePath);
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
         return ExistsCore(false, null, path, pathFormat);
      }

      #region Transactional

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
      public static bool ExistsTransacted(KernelTransaction transaction, string path)
      {
         return ExistsCore(false, transaction, path, PathFormat.RelativePath);
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
      public static bool ExistsTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return ExistsCore(false, transaction, path, pathFormat);
      }

      #endregion // Transacted

      #endregion

      #region Internal Methods

      /// <summary>Determines whether the specified file or directory exists.</summary>
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
      internal static bool ExistsCore(bool isFolder, KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         // Will be caught later and be thrown as an ArgumentException or ArgumentNullException.
         // Let's take a shorter route, preventing an Exception from being thrown altogether.
         if (Utils.IsNullOrWhiteSpace(path))
            return false;


         // Check for driveletter, such as: "C:"
         var pathRp = Path.GetRegularPathCore(path, GetFullPathOptions.None, false);

         if (pathRp.Length == 2 && Path.IsPathRooted(pathRp, false))
            path = pathRp;


         try
         {
            var pathLp = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.ContinueOnNonExist);

            var data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
            var dataInitialised = FillAttributeInfoCore(transaction, pathLp, ref data, false, true);

            return (dataInitialised == Win32Errors.ERROR_SUCCESS &&
                    data.dwFileAttributes != (FileAttributes) (-1) &&
                    (isFolder
                       ? (data.dwFileAttributes & FileAttributes.Directory) != 0
                       : (data.dwFileAttributes & FileAttributes.Directory) == 0));
         }
         catch
         {
            return false;
         }
      }

      #endregion // Internal Methods
   }
}

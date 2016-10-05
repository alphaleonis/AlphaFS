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

using System;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class DirectoryInfo
   {
      #region CopyTo

      // .NET: Directory class does not contain the Copy() method.
      // Mimic .NET File.Copy() methods.

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing directory by default.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="DirectoryInfo"/> instance if the directory was completely copied.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public DirectoryInfo CopyTo(string destinationPath)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing directory by default.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="DirectoryInfo"/> instance if the directory was completely copied.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public DirectoryInfo CopyTo(string destinationPath, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, pathFormat);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing directory to a new directory, allowing the overwriting of an existing directory, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>Returns a new directory, or an overwrite of an existing directory if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the directory exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public DirectoryInfo CopyTo(string destinationPath, CopyOptions copyOptions)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, copyOptions, null, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing directory to a new directory, allowing the overwriting of an existing directory, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>
      ///   <para>Returns a new directory, or an overwrite of an existing directory if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      ///   <para>If the directory exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public DirectoryInfo CopyTo(string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, copyOptions, null, null, null, out destinationPathLp, pathFormat);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing directory to a new directory, allowing the overwriting of an existing directory, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing directory to a new directory, allowing the overwriting of an existing directory, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyTo


      #region MoveTo

      #region .NET

      /// <summary>Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing directory by default.</para>
      ///   <para>This method does not work across disk volumes.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">
      ///   <para>The name and path to which to move this directory.</para>
      ///   <para>The destination cannot be another disk volume or a directory with the identical name.</para>
      ///   <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath)
      {
         string destinationPathLp;
         var copyMoveResult = CopyToMoveToCore(destinationPath, null, MoveOptions.None, null, null, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);

         return copyMoveResult;
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing directory by default.</para>
      ///   <para>This method does not work across disk volumes.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="DirectoryInfo"/> instance if the directory was completely moved.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">
      ///   <para>The name and path to which to move this directory.</para>
      ///   <para>The destination cannot be another disk volume or a directory with the identical name.</para>
      ///   <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public DirectoryInfo MoveTo(string destinationPath, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, null, MoveOptions.None, null, null, out destinationPathLp, pathFormat);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified.
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="DirectoryInfo"/> instance if the directory was completely moved.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">
      ///   <para>The name and path to which to move this directory.</para>
      ///   <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      ///   <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public DirectoryInfo MoveTo(string destinationPath, MoveOptions moveOptions)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, null, moveOptions, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified.
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="DirectoryInfo"/> instance if the directory was completely moved.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">
      ///   <para>The name and path to which to move this directory.</para>
      ///   <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      ///   <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public DirectoryInfo MoveTo(string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, null, moveOptions, null, null, out destinationPathLp, pathFormat);
         return new DirectoryInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">
      ///   <para>The name and path to which to move this directory.</para>
      ///   <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      ///   <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }



      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">
      ///   <para>The name and path to which to move this directory.</para>
      ///   <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      ///   <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // AlphaFS

      #endregion // MoveTo


      #region Internal Methods

      /// <summary>Copy/move a Non-/Transacted file or directory including its children to a new location,
      /// <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified, and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy or Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="longFullPath">Returns the retrieved long full path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      private CopyMoveResult CopyToMoveToCore(string destinationPath, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, out string longFullPath, PathFormat pathFormat)
      {
         var destinationPathLp = Path.GetExtendedLengthPathCore(null, destinationPath, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);
         longFullPath = destinationPathLp;

         // Returns false when CopyMoveProgressResult is PROGRESS_CANCEL or PROGRESS_STOP.
         return Directory.CopyMoveCore(Transaction, LongFullName, destinationPathLp, copyOptions, moveOptions, progressHandler, userProgressData, null, PathFormat.LongFullPath);
      }


      private void CopyToMoveToCoreRefresh(string destinationPath, string destinationPathLp)
      {
         LongFullName = destinationPathLp;
         FullPath = Path.GetRegularPathCore(destinationPathLp, GetFullPathOptions.None, false);

         OriginalPath = destinationPath;
         DisplayPath = Path.GetRegularPathCore(OriginalPath, GetFullPathOptions.None, false);

         // Flush any cached information about the directory.
         Reset();
      }

      #endregion // Internal Methods
   }
}

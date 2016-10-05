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
   partial class FileInfo
   {
      #region CopyTo

      #region .NET

      /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="overwrite"><see langword="true"/> to allow an existing file to be overwritten; otherwise, <see langword="false"/>.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool overwrite)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Copies an existing file to a new file, disallowing the overwriting of an existing file.
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }
      


      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file.
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="overwrite"><see langword="true"/> to allow an existing file to be overwritten; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool overwrite, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, copyOptions, null, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, copyOptions, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, preserveDates, copyOptions, null, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A new <see cref="FileInfo"/> instance with a fully qualified path.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, preserveDates, copyOptions, null, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, false, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, false, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo(string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // AlphaFS

      #endregion // CopyTo


      #region MoveTo

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath)
      {
         string destinationPathLp;
         var copyMoveResult = CopyToMoveToCore(destinationPath, false, null, MoveOptions.CopyAllowed, null, null, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);

         return copyMoveResult;
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <returns><para>Returns a new <see cref="FileInfo"/> instance with a fully qualified path when successfully moved,</para></returns>
      /// <remarks>
      ///   <para>Use this method to prevent overwriting of an existing file by default.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo MoveTo(string destinationPath, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, null, MoveOptions.CopyAllowed, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.
      /// <returns><para>Returns a new <see cref="FileInfo"/> instance with a fully qualified path when successfully moved,</para></returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public FileInfo MoveTo(string destinationPath, MoveOptions moveOptions)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, null, moveOptions, null, null, out destinationPathLp, PathFormat.RelativePath);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.
      /// <returns><para>Returns a new <see cref="FileInfo"/> instance with a fully qualified path when successfully moved,</para></returns>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public FileInfo MoveTo(string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         string destinationPathLp;
         CopyToMoveToCore(destinationPath, false, null, moveOptions, null, null, out destinationPathLp, pathFormat);
         return new FileInfo(Transaction, destinationPathLp, PathFormat.LongFullPath);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, false, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, PathFormat.RelativePath);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.
      /// <remarks>
      ///   <para>Use this method to allow or prevent overwriting of an existing file.</para>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="destinationPath">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo(string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         string destinationPathLp;
         var cmr = CopyToMoveToCore(destinationPath, false, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, pathFormat);
         CopyToMoveToCoreRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // AlphaFS

      #endregion // MoveTo


      #region Internal Methods

      /// <summary>Copy/move an existing file to a new file, allowing the overwriting of an existing file.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <param name="destinationPath"><para>A full path string to the destination directory</para></param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="copyOptions"><para>This parameter can be <see langword="null"/>. Use <see cref="CopyOptions"/> to specify how the file is to be copied.</para></param>
      /// <param name="moveOptions"><para>This parameter can be <see langword="null"/>. Use <see cref="MoveOptions"/> that specify how the file is to be moved.</para></param>
      /// <param name="progressHandler"><para>This parameter can be <see langword="null"/>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <see langword="null"/>. The argument to be passed to the callback function.</para></param>
      /// <param name="longFullPath">[out] Returns the retrieved long full path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SecurityCritical]
      private CopyMoveResult CopyToMoveToCore(string destinationPath, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, out string longFullPath, PathFormat pathFormat)
      {
         var destinationPathLp = Path.GetExtendedLengthPathCore(Transaction, destinationPath, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

         longFullPath = destinationPathLp;

         // Returns false when CopyMoveProgressResult is PROGRESS_CANCEL or PROGRESS_STOP.
         return File.CopyMoveCore(false, Transaction, LongFullName, destinationPathLp, preserveDates, copyOptions, moveOptions, progressHandler, userProgressData, null, PathFormat.LongFullPath);
      }


      private void CopyToMoveToCoreRefresh(string destinationPath, string destinationPathLp)
      {
         LongFullName = destinationPathLp;
         FullPath = Path.GetRegularPathCore(destinationPathLp, GetFullPathOptions.None, false);

         OriginalPath = destinationPath;
         DisplayPath = Path.GetRegularPathCore(OriginalPath, GetFullPathOptions.None, false);

         _name = Path.GetFileName(destinationPathLp, true);

         // Flush any cached information about the file.
         Reset();
      }

      #endregion // Internal Methods
   }
}

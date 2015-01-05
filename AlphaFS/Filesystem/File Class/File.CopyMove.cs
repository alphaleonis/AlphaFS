using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using FileStream = System.IO.FileStream;
using StreamReader = System.IO.StreamReader;
using StreamWriter = System.IO.StreamWriter;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Copy

      /// <summary>
      ///   Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this
      ///   method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an
      ///   exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">
      ///   The name of the destination file. This cannot be a directory or an existing file.
      /// </param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>            
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, CopyOptions.FailIfExists, null, null, null, PathFormat.Auto);
      }

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// </summary>
      /// <remarks>
      /// <para>The attributes of the original file are retained in the copied file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, bool overwrite)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// </summary>
      /// <remarks>
      /// <para>The attributes of the original file are retained in the copied file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, bool overwrite, PathFormat pathFormat)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// </summary>
      /// <remarks>
      /// <para>The attributes of the original file are retained in the copied file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destinationFileName, bool overwrite, PathFormat pathFormat)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
      /// </summary>
      /// <remarks>
      /// <para>The attributes of the original file are retained in the copied file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destinationFileName)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, CopyOptions.FailIfExists, null, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// </summary>
      /// <remarks>
      /// <para>The attributes of the original file are retained in the copied file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destinationFileName, bool overwrite)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, PathFormat.Auto);
      }


      #endregion // Transacted

      #endregion // Copy

      #region Move

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Move(string sourceFileName, string destinationFileName)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// </summary>
      /// <remarks>
      /// <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      /// <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      /// <para>You cannot use the Move method to overwrite an existing file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(string sourceFileName, string destinationFileName, PathFormat pathFormat)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// </summary>
      /// <remarks>
      /// <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      /// <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      /// <para>You cannot use the Move method to overwrite an existing file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>      
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destinationFileName, PathFormat pathFormat)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// </summary>
      /// <remarks>
      /// <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      /// <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      /// <para>You cannot use the Move method to overwrite an existing file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destinationFileName)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, PathFormat.Auto);
      }

      #endregion // Transacted


      #endregion // Move

      #region Copy1


      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Copy1(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified, and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been copied. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Copy1(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Copy1(string sourceFileName, string destinationFileName, CopyOptions copyOptions)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, copyOptions, null, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      ///
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Copy1(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified,.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been copied. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Copy1(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, PathFormat.Auto);
      }

      #region Transacted

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      ///
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Copy1(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be
      ///   specified, and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been copied. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Copy1(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Copy1(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, copyOptions, null, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be
      ///   specified.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Copy1(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be
      ///   specified, and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been copied. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Copy1(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // Copy1

      #region Move1


      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name and <see cref="MoveOptions"/>
      ///   options.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Move1(string sourceFileName, string destinationFileName, MoveOptions moveOptions, PathFormat pathFormat)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, null, moveOptions, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/>
      ///   options, and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been moved. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Move1(string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name and <see cref="MoveOptions"/>
      ///   options.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Move1(string sourceFileName, string destinationFileName, MoveOptions moveOptions)
      {
         CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, null, moveOptions, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/>
      ///   options, and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been moved. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Move1(string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(false, null, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, PathFormat.Auto);
      }

      #region Transacted

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name and <see cref="MoveOptions"/>
      ///   options.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Move1(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions, PathFormat pathFormat)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/>
      ///   options, and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been moved. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Move1(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name and <see cref="MoveOptions"/>
      ///   options.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static void Move1(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/>
      ///   options,.
      /// </summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an
      ///   <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains
      ///   <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable
      ///   behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions">
      ///   <see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been moved. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">.</exception>
      /// <exception cref="FileNotFoundException">.</exception>
      /// <exception cref="IOException">.</exception>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SecurityCritical]
      public static CopyMoveResult Move1(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // Move1

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method CopyMoveInternal() to copy/move a Non-/Transacted file or directory including its children to a new
      ///   location,
      ///   <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless
      ///   <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>This Move method works across disk volumes, and it does not throw an exception if the
      ///   source and destination are the same. </para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into
      ///   that directory, you get an IOException.</para>
      /// </remarks>
      /// <exception cref="NotSupportedException">.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="FileNotFoundException">Passed if the file was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="ArgumentException">
      ///   Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <param name="isFolder">
      ///   Specifies that <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are a file or directory.
      /// </param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The source directory path.</param>
      /// <param name="destinationFileName">The destination directory path.</param>
      /// <param name="preserveDates">
      ///   <see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise. This parameter is ignored for
      ///   move operations.
      /// </param>
      /// <param name="copyOptions">
      ///   <see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="moveOptions">
      ///   Flags that specify how the file or directory is to be moved. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="progressHandler">
      ///   A callback function that is called each time another portion of the file has been copied/moved. This parameter can be
      ///   <see langword="null"/>.
      /// </param>
      /// <param name="userProgressData">
      ///   The argument to be passed to the callback function. This parameter can be <see langword="null"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>      
      /// <exception cref="UnauthorizedAccessException">.</exception>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveInternal(bool isFolder, KernelTransaction transaction, string sourceFileName, string destinationFileName, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.Auto)
         {
            Path.CheckValidPath(sourceFileName, true, true);
            Path.CheckValidPath(destinationFileName, true, true);
         }
         else
         {
            // MSDN:. NET 3.5+: NotSupportedException: Path contains a colon character (:) that is not part of a drive label ("C:\").
            Path.CheckValidPath(sourceFileName, false, false);
            Path.CheckValidPath(destinationFileName, false, false);
         }

         string sourceFileNameLp = Path.GetExtendedLengthPathInternal(transaction, sourceFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);
         string destinationFileNameLp = Path.GetExtendedLengthPathInternal(transaction, destinationFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);


         // MSDN: If this flag is set to TRUE during the copy/move operation, the operation is canceled.
         // Otherwise, the copy/move operation will continue to completion.
         bool cancel = false;

         // Determine Copy or Move action.
         bool doCopy = copyOptions != null;
         bool doMove = !doCopy && moveOptions != null;

         if ((!doCopy && !doMove) || (doCopy && doMove))
            throw new NotSupportedException(Resources.UndeterminedCopyMoveAction);

         bool overwrite = doCopy
            ? (((CopyOptions)copyOptions & CopyOptions.FailIfExists) != CopyOptions.FailIfExists)
            : (((MoveOptions)moveOptions & MoveOptions.ReplaceExisting) == MoveOptions.ReplaceExisting);

         bool raiseException = progressHandler == null;

         // Setup callback function for progress notifications.
         NativeMethods.NativeCopyMoveProgressRoutine routine = (progressHandler != null)
            ? (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, hSourceFile, hDestinationFile, lpData)
               =>
               progressHandler(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, userProgressData)
            : (NativeMethods.NativeCopyMoveProgressRoutine)null;

         #endregion //Setup

      startCopyMove:

         uint lastError = Win32Errors.ERROR_SUCCESS;

         #region Win32 Copy/Move

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista
            ? doMove
            // MoveFileWithProgress() / MoveFileTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-04-15: MSDN confirms LongPath usage.

               // CopyFileEx() / CopyFileTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-04-15: MSDN confirms LongPath usage.

               ? NativeMethods.MoveFileWithProgress(sourceFileNameLp, destinationFileNameLp, routine, IntPtr.Zero, (MoveOptions)moveOptions)
               : NativeMethods.CopyFileEx(sourceFileNameLp, destinationFileNameLp, routine, IntPtr.Zero, out cancel, (CopyOptions)copyOptions)

            : doMove
               ? NativeMethods.MoveFileTransacted(sourceFileNameLp, destinationFileNameLp, routine, IntPtr.Zero, (MoveOptions)moveOptions, transaction.SafeHandle)
               : NativeMethods.CopyFileTransacted(sourceFileNameLp, destinationFileNameLp, routine, IntPtr.Zero, out cancel, (CopyOptions)copyOptions, transaction.SafeHandle)))
         {
            lastError = (uint)Marshal.GetLastWin32Error();

            if (lastError == Win32Errors.ERROR_REQUEST_ABORTED)
            {
               // If lpProgressRoutine returns PROGRESS_CANCEL due to the user canceling the operation,
               // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
               // In this case, the partially copied destination file is deleted.
               //
               // If lpProgressRoutine returns PROGRESS_STOP due to the user stopping the operation,
               // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
               // In this case, the partially copied destination file is left intact.

               cancel = true;
            }

            else if (raiseException)
            {
               #region Win32Errors

               switch (lastError)
               {
                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                     // File.Copy()
                     // File.Move()
                     // MSDN: .NET 3.5+: FileNotFoundException: sourceFileName was not found. 
                     NativeError.ThrowException(lastError, sourceFileNameLp);
                     break;

                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                     // File.Copy()
                     // File.Move()
                     // Directory.Move()
                     // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified in sourceFileName or destinationFileName is invalid (for example, it is on an unmapped drive).
                     NativeError.ThrowException(lastError, sourceFileNameLp);
                     break;

                  case Win32Errors.ERROR_FILE_EXISTS:
                     // File.Copy()
                     // Directory.Copy1()
                     NativeError.ThrowException(lastError, destinationFileNameLp, true);
                     break;

                  default:
                     // For a number of error codes (sharing violation, path not found, etc)
                     // we don't know if the problem was with the source or dest file.

                     // Check if destination directory already exists.
                     // Directory.Move()
                     // MSDN: .NET 3.5+: IOException: destDirName already exists. 
                     if (ExistsInternal(true, transaction, destinationFileNameLp, PathFormat.ExtendedLength))
                        NativeError.ThrowException(Win32Errors.ERROR_ALREADY_EXISTS, destinationFileNameLp, true);

                     if (doMove)
                     {
                        // Ensure that the source file or directory exists.
                        // Directory.Move()
                        // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified by sourceDirName is invalid (for example, it is on an unmapped drive). 
                        if (!ExistsInternal(isFolder, transaction, sourceFileNameLp, PathFormat.ExtendedLength))
                           NativeError.ThrowException(isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND, sourceFileNameLp);
                     }


                     // Try reading the source file.
                     string fileNameLp = destinationFileNameLp;

                     if (!isFolder)
                        using (SafeFileHandle safeHandle = CreateFileInternal(transaction, sourceFileNameLp, ExtendedFileAttributes.None, null, FileMode.Open, 0, FileShare.Read, false, PathFormat.ExtendedLength))
                           if (safeHandle.IsInvalid)
                              fileNameLp = sourceFileNameLp;


                     if (lastError == Win32Errors.ERROR_ACCESS_DENIED)
                     {
                        // File.Copy()
                        // File.Move()
                        // MSDN: .NET 3.5+: IOException: An I/O error has occurred.
                        //   Directory exists with the same name as the file.
                        if (!isFolder && ExistsInternal(true, transaction, destinationFileNameLp, PathFormat.ExtendedLength))
                           NativeError.ThrowException(lastError, string.Format(CultureInfo.CurrentCulture, Resources.DirectoryExistsWithSameNameSpecifiedByPath, destinationFileNameLp), true);

                        else
                        {
                           var data = new NativeMethods.Win32FileAttributeData();
                           FillAttributeInfoInternal(transaction, destinationFileNameLp, ref data, false, true);

                           if (data.FileAttributes != (FileAttributes)(-1))
                           {
                              if ((data.FileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                              {
                                 // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only, or recursive is false and path is not an empty directory.

                                 if (overwrite)
                                 {
                                    // Reset directory attributes.
                                    SetAttributesInternal(isFolder, transaction, destinationFileNameLp, FileAttributes.Normal, true, PathFormat.ExtendedLength);
                                    goto startCopyMove;
                                 }

                                 // MSDN: .NET 3.5+: UnauthorizedAccessException: destinationFileName is read-only.
                                 // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                                 // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.
                                 NativeError.ThrowException(Win32Errors.ERROR_FILE_READ_ONLY, destinationFileNameLp, true);
                              }

                              // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                              // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.
                              if ((data.FileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                                 NativeError.ThrowException(lastError, string.Format(CultureInfo.CurrentCulture, Resources.FileHidden, destinationFileNameLp), true);
                           }


                           // Observation: .NET 3.5+: For files: UnauthorizedAccessException: The caller does not have the required permission.
                           // Observation: .NET 3.5+: For directories: IOException: The caller does not have the required permission.
                           NativeError.ThrowException(lastError, destinationFileNameLp, isFolder);
                        }
                     }

                     // MSDN: .NET 3.5+: An I/O error has occurred. 
                     // File.Copy(): IOException: destinationFileName exists and overwrite is false.
                     // File.Move(): The destination file already exists or sourceFileName was not found.
                     NativeError.ThrowException(lastError, fileNameLp, true);
                     break;
               }

               #endregion // Win32Errors
            }
         }

         #endregion // Win32 Copy/Move

         #region Transfer Timestamps

         // Apply original Timestamps if requested.
         // MoveFileWithProgress() / MoveFileTransacted() automatically preserve Timestamps.
         // File.Copy()
         if (preserveDates && doCopy && lastError == Win32Errors.ERROR_SUCCESS)
         {
            // Currently preserveDates is only used with files.
            var data = new NativeMethods.Win32FileAttributeData();
            int dataInitialised = FillAttributeInfoInternal(transaction, sourceFileNameLp, ref data, false, true);

            if (dataInitialised == Win32Errors.ERROR_SUCCESS && data.FileAttributes != (FileAttributes)(-1))
               SetFsoDateTimeInternal(false, transaction, destinationFileNameLp,
                  DateTime.FromFileTimeUtc(data.CreationTime), DateTime.FromFileTimeUtc(data.LastAccessTime), DateTime.FromFileTimeUtc(data.LastWriteTime), PathFormat.ExtendedLength);
         }

         #endregion // Transfer Timestamps

         // The copy/move operation succeeded, failed or was canceled.
         return new CopyMoveResult(sourceFileNameLp, destinationFileNameLp, isFolder, doMove, cancel, (int)lastError);
      }

      #endregion // CopyMoveInternal
   }
}

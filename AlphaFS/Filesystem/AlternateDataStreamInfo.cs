/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class that retrieves the NTFS alternate data streams from a file or directory.</summary>
   [SerializableAttribute]
   public sealed class AlternateDataStreamInfo
   {
      #region Constructors

      /// <summary>Initializes a new instance of the <see cref="T:AlternateDataStreamInfo"/> class.</summary>
      /// <param name="path">The path to an existing file or directory.</param>
      public AlternateDataStreamInfo(string path) 
         : this(new NativeMethods.Win32StreamId(), path, null, null, null, false)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="T:AlternateDataStreamInfo"/> class.</summary>
      /// <param name="handle">A <see cref="T:SafeFileHandle"/> connected to the file or directory from which to retrieve the information.</param>
      public AlternateDataStreamInfo(SafeFileHandle handle)
         : this(new NativeMethods.Win32StreamId(), Path.GetFinalPathNameByHandleInternal(handle, FinalPathFormats.None), null, null, null, false)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="T:AlternateDataStreamInfo"/> class.</summary>
      /// <param name="stream">The <see cref="T:NativeMethods.Win32StreamId"/> stream ID.</param>
      /// <param name="path">The path to an existing file or directory.</param>
      /// <param name="name">The originalName of the stream.</param>
      /// <param name="originalName">The alternative data stream name originally specified by the user.</param>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      private AlternateDataStreamInfo(NativeMethods.Win32StreamId stream, string path, string name, string originalName, bool? isFolder, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         _isFullPath = isFullPath;

         OriginalName = originalName;
         Name = name ?? "";

         Attributes = stream.StreamAttributes;
         Type = stream.StreamType;
         Size = (long) stream.StreamSize;

         FullPath = path;

         if (isFolder == null)
         {
            FileAttributes attrs = File.GetAttributesInternal(false, null, LongFullPath, true, false, null);
            IsDirectory = (attrs & FileAttributes.Directory) == FileAttributes.Directory;
         }
         else
            IsDirectory = (bool) isFolder;
      }

      #endregion // Constructors

      #region Methods

      #region AddStream

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file or directory.</summary>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void AddStream(string name, string[] contents)
      {
         AddStreamInternal(false, null, LongFullPath, name, contents, null);
      }

      #endregion // AddStream

      #region EnumerateStreams

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file or directory.</summary>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file or directory.</returns>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams()
      {
         return EnumerateStreamsInternal(null, null, null, LongFullPath, null, null, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file or directory.</summary>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file or directory.</returns>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams(StreamType streamType)
      {
         return EnumerateStreamsInternal(null, null, null, LongFullPath, null, streamType, null);
      }

      #endregion // EnumerateStreams

      #region RemoveStreams

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file or directory.</summary>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream()
      {
         RemoveStreamInternal(null, null, LongFullPath, null, null);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file or directory.</summary>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream(string name)
      {
         RemoveStreamInternal(null, null, LongFullPath, name, null);
      }

      #endregion // RemoveStreams


      #region Unified Internals

      #region AddStreamInternal

      /// <summary>[AlphaFS] Unified method AddStreamInternal() to add an alternate data stream (NTFS ADS) to an existing file or directory.</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file or directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "StreamWriter() disposes of FileStream() object.")]
      [SecurityCritical]
      internal static void AddStreamInternal(bool isFolder, KernelTransaction transaction, string path, string name, IEnumerable<string> contents, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(path);

         if (Utils.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);

         if (name.Contains(Path.StreamSeparator))
            throw new ArgumentException(Resources.StreamNameWithColon);

         using (SafeFileHandle handle = File.CreateFileInternal(!isFolder, transaction, path + Path.StreamSeparator + name, isFolder ? EFileAttributes.BackupSemantics : EFileAttributes.Normal, null, FileMode.Create, FileSystemRights.Write, FileShare.ReadWrite, isFullPath))
         using (StreamWriter writer = new StreamWriter(new FileStream(handle, FileAccess.Write), NativeMethods.DefaultFileEncoding))
            foreach (string line in contents)
               writer.WriteLine(line);
      }

      #endregion // AddStreamInternal
      
      #region EnumerateStreamsInternal

      /// <summary>Unified method EnumerateStreamsInternal() to return an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances, associated with a file or directory.</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">A <see cref="T:SafeFileHandle"/> connected to the open file from which to retrieve the information. Use either <paramref name="safeHandle"/> or <paramref name="path"/>, not both.</param>
      /// <param name="path">The path to an existing file or directory. Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</param>
      /// <param name="originalName">The name of the stream to retrieve.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <returns>An <see cref="T:IEnumerable{AlternateDataStreamInfo}"/> collection of streams for the file or directory specified by path.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static IEnumerable<AlternateDataStreamInfo> EnumerateStreamsInternal(bool? isFolder, KernelTransaction transaction, SafeFileHandle safeHandle, string path, string originalName, StreamType? streamType, bool? isFullPath)
      {
         string pathLp = null;

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            pathLp = isFullPath == null
               ? path
               : (bool) isFullPath
                  ? Path.GetLongPathInternal(path, false, false, false, false)
                  : Path.GetFullPathInternal(transaction, path, true, false, false, true, false);

            if (isFolder == null)
            {
               FileAttributes attrs = File.GetAttributesInternal(false, transaction, pathLp, true, false, null);
               isFolder = (attrs & FileAttributes.Directory) == FileAttributes.Directory;
            }

            safeHandle = File.CreateFileInternal(null, transaction, pathLp, (bool) isFolder ? EFileAttributes.BackupSemantics : EFileAttributes.Normal, null, FileMode.Open, FileSystemRights.Read, FileShare.ReadWrite, null);
         }
         else
            NativeMethods.IsValidHandle(safeHandle);


         try
         {
            using (new PrivilegeEnabler(Privilege.Backup))
            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               Type typeWin32Stream = typeof(NativeMethods.Win32StreamId);
               uint sizeOfType = (uint)Marshal.SizeOf(typeWin32Stream);
               uint numberOfBytesRead;
               IntPtr context;

               bool doLoop = true;
               while (doLoop)
               {
                  if (!NativeMethods.BackupRead(safeHandle, safeBuffer, sizeOfType, out numberOfBytesRead, false, true, out context))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  doLoop = numberOfBytesRead == sizeOfType;
                  if (doLoop)
                  {
                     string streamName = null;
                     string streamSearchName = null;

                     
                     // CA2001:AvoidCallingProblematicMethods

                     IntPtr buffer = IntPtr.Zero;
                     bool successRef = false;
                     safeBuffer.DangerousAddRef(ref successRef);

                     // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                     if (successRef)
                        buffer = safeBuffer.DangerousGetHandle();

                     safeBuffer.DangerousRelease();

                     if (buffer == IntPtr.Zero)
                        NativeError.ThrowException(Resources.HandleDangerousRef);

                     // CA2001:AvoidCallingProblematicMethods


                     NativeMethods.Win32StreamId stream = NativeMethods.GetStructure<NativeMethods.Win32StreamId>(0, buffer);

                     if (streamType == null || stream.StreamType == streamType)
                     {
                        if (stream.StreamNameSize > 0)
                        {
                           if (!NativeMethods.BackupRead(safeHandle, safeBuffer, stream.StreamNameSize, out numberOfBytesRead, false, true, out context))
                              NativeError.ThrowException(Marshal.GetLastWin32Error());


                           // CA2001:AvoidCallingProblematicMethods

                           buffer = IntPtr.Zero;
                           successRef = false;
                           safeBuffer.DangerousAddRef(ref successRef);

                           // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                           if (successRef)
                              buffer = safeBuffer.DangerousGetHandle();

                           safeBuffer.DangerousRelease();

                           if (buffer == IntPtr.Zero)
                              NativeError.ThrowException(Resources.HandleDangerousRef);

                           // CA2001:AvoidCallingProblematicMethods


                           streamName = Marshal.PtrToStringUni(buffer, (int)numberOfBytesRead / 2);

                           // Returned stream name format: ":streamName:$DATA"
                           streamSearchName = streamName.TrimStart(Path.StreamSeparatorChar).Replace(Path.StreamSeparator + "$DATA", "");
                        }

                        if (originalName == null || (streamSearchName != null && streamSearchName.Equals(originalName, StringComparison.OrdinalIgnoreCase)))
                           yield return new AlternateDataStreamInfo(stream, pathLp ?? path, streamName, originalName ?? streamSearchName, isFolder, isFullPath);
                     }

                     uint lo, hi;
                     doLoop = !NativeMethods.BackupSeek(safeHandle, uint.MinValue, uint.MaxValue, out lo, out hi, out context);
                  }
               }

               // MSDN: To release the memory used by the data structure, call BackupRead with the bAbort parameter set to TRUE when the backup operation is complete.
               if (!NativeMethods.BackupRead(safeHandle, safeBuffer, 0, out numberOfBytesRead, true, false, out context))
                  NativeError.ThrowException(Marshal.GetLastWin32Error());
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // EnumerateStreamsInternal

      #region GetStreamSizeInternal

      /// <summary>Retrieves the actual number of bytes of disk storage used by all or a specific alternate data streams (NTFS ADS).</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="handle">A <see cref="T:SafeFileHandle"/> connected to the open file from which to retrieve the information. Use either <paramref name="handle"/> or <paramref name="path"/>, not both.</param>
      /// <param name="path">A path that describes a file. Use either <paramref name="path"/> or <paramref name="handle"/>, not both.</param>
      /// <param name="name">The name of the stream to retrieve or <c>null</c> to retrieve all streams.</param>
      /// <param name="streamType">The type of stream to retrieve or <c>null</c> to retrieve all streams.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public static long GetStreamSizeInternal(bool? isFolder, KernelTransaction transaction, SafeFileHandle handle, string path, string name, StreamType? streamType, bool? isFullPath)
      {
         IEnumerable<AlternateDataStreamInfo> streamSizes = EnumerateStreamsInternal(isFolder, transaction, handle, path, name, streamType, isFullPath);

         return streamType == null
            ? streamSizes.Sum(stream => stream.Size)
            : streamSizes.Where(stream => stream.Type == streamType).Sum(stream => stream.Size);
      }

      #endregion // GetStreamSizeInternal

      #region RemoveStreamInternal

      /// <summary>Unified method RemoveStreamInternal() to remove alternate data streams (NTFS ADS) from a file or directory.</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file or directory.</param>
      /// <param name="name">The name of the stream to remove. When <c>null</c> all ADS are removed.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void RemoveStreamInternal(bool? isFolder, KernelTransaction transaction, string path, string name, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(path);

         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, true, false);

         foreach (AlternateDataStreamInfo stream in EnumerateStreamsInternal(isFolder, transaction, null, pathLp, name, StreamType.AlternateData, isFullPath))
            File.DeleteFileInternal(transaction, string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}{1}$DATA", pathLp, Path.StreamSeparator, stream.OriginalName), false, null);
      }

      #endregion // RemoveStreamInternal

      #endregion // Unified Internals

      #endregion Methods

      #region Properties

      #region Attributes

      /// <summary>The attributes of the data to facilitate cross-operating system transfer.</summary>
      public StreamAttributes Attributes { get; private set; }

      #endregion // Attributes

      #region FullPath

      private readonly bool? _isFullPath = false;
      private string _fullPath;

      /// <summary>The source path of the stream.</summary>
      public string FullPath
      {
         get { return _fullPath; }
         set
         {
            LongFullPath = value;
            _fullPath = Path.GetRegularPathInternal(LongFullPath, false, false, false, false);
         }
      }

      #endregion // FullPath

      #region Type

      /// <summary>The type of the data in the stream.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
      public StreamType Type { get; private set; }

      #endregion // Type

      #region IsDirectory

      /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
      /// <value><c>true</c> if this instance represents a directory; otherwise, <c>false</c>.</value>
      public bool IsDirectory { get; private set; }

      #endregion // IsDirectory

      #region LongFullPath

      private string _longFullPath;

      /// <summary>The full path of the file system object in Unicode (LongPath) format.</summary>
      internal string LongFullPath
      {
         get { return _longFullPath; }
         private set
         {
            _longFullPath = _isFullPath == null ? value : Path.GetLongPathInternal(value, false, false, false, false);
         }
      }

      #endregion // LongFullPath

      #region Name

      /// <summary>The name of the alternative data stream.</summary>
      public string Name { get; private set; }

      #endregion // Name

      #region OriginalName

      /// <summary>The alternative data stream name originally specified by the user.</summary>
      internal string OriginalName { get; private set; }

      #endregion // OriginalName

      #region Size

      /// <summary>The size of the data in the substream, in bytes.</summary>
      public long Size { get; private set; }

      #endregion // Size

      #endregion // Properties
   }
}
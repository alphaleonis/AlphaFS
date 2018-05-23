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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class for CopyMoveResult that contains the results for the Copy or Move action.
   /// <remarks>Normally there is no need to manually instantiate and/or populate this class.</remarks>
   /// </summary>
   [SerializableAttribute]
   public sealed class CopyMoveResult
   {
      #region Private Fields

      [NonSerialized] internal readonly Stopwatch _stopwatch;

      #endregion // Private Fields
      

      #region Constructors

      /// <summary>Initializes a CopyMoveResult instance for the Copy or Move action.</summary>
      /// <param name="source">Indicates the full path to the source file or directory.</param>
      /// <param name="destination">Indicates the full path to the destination file or directory.</param>
      private CopyMoveResult(string source, string destination)
      {
         Source = source;
         Destination = destination;

         IsCopy = true;

         _stopwatch = Stopwatch.StartNew();
      }


      /// <summary>Initializes a CopyMoveResult instance for the Copy or Move action.
      /// <remarks>Normally there is no need to manually call this constructor.</remarks>
      /// </summary>
      /// <param name="source">Indicates the full path to the source file or directory.</param>
      /// <param name="destination">Indicates the full path to the destination file or directory.</param>
      /// <param name="isCopy">>When <see langword="true"/> the action is a Copy, Move otherwise.</param>
      /// <param name="isFolder">When <see langword="true"/> indicates the sources is a directory; file otherwise.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise. This parameter is ignored for move operations.</param>
      /// <param name="emulatedMove">When <see langword="true"/> indicates the Move action used a fallback of Copy + Delete actions.</param>
      public CopyMoveResult(string source, string destination, bool isCopy, bool isFolder, bool preserveDates, bool emulatedMove) : this(source, destination)
      {
         IsEmulatedMove = emulatedMove;

         IsCopy = isCopy;
         IsDirectory = isFolder;

         TimestampsCopied = preserveDates;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>Indicates the duration of the Copy or Move action.</summary>
      public TimeSpan Duration
      {
         get { return TimeSpan.FromMilliseconds(_stopwatch.Elapsed.TotalMilliseconds); }
      }
      

      /// <summary>Indicates the destination file or directory.</summary>
      public string Destination { get; private set; }
      

      /// <summary>The error code encountered during the Copy or Move action.</summary>
      /// <value>0 (zero) indicates success.</value>
      public int ErrorCode { get; internal set; }


      /// <summary>The error message from the <see cref="ErrorCode"/> that was encountered during the Copy or Move action.</summary>
      /// <value>A message describing the error.</value>
      [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
      public string ErrorMessage { get { return new Win32Exception(ErrorCode).Message; } }


      /// <summary>When <see langword="true"/> indicates that the Copy or Move action was canceled.</summary>
      /// <value><see langword="true"/> when the Copy/Move action was canceled. Otherwise <see langword="false"/>.</value>
      public bool IsCanceled { get; internal set; }


      /// <summary>When <see langword="true"/> the action was a Copy, Move otherwise.</summary>
      /// <value><see langword="true"/> when the action was a Copy. Otherwise a Move action was performed.</value>
      public bool IsCopy { get; private set; }


      /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
      /// <value><see langword="true"/> if this instance represents a directory; otherwise, <see langword="false"/>.</value>
      public bool IsDirectory { get; private set; }


      /// <summary>Indicates the Move action used a fallback of Copy + Delete actions.</summary>
      public bool IsEmulatedMove { get; private set; }


      /// <summary>Gets a value indicating whether this instance represents a file.</summary>
      /// <value><see langword="true"/> if this instance represents a file; otherwise, <see langword="false"/>.</value>
      public bool IsFile { get { return !IsDirectory; } }


      /// <summary>When <see langword="true"/> the action was a Move, Copy otherwise.</summary>
      /// <value><see langword="true"/> when the action was a Move. Otherwise a Copy action was performed.</value>
      public bool IsMove { get { return !IsCopy; } }


      /// <summary>Indicates the source file or directory.</summary>
      public string Source { get; private set; }


      /// <summary>Indicates that the source date and timestamps have been applied to the destination file system objects.</summary>
      public bool TimestampsCopied { get; private set; }


      /// <summary>The total number of bytes copied.</summary>
      public long TotalBytes { get; internal set; }


      /// <summary>The total number of bytes copied, formatted as a unit size.</summary>
      public string TotalBytesUnitSize
      {
         get { return Utils.UnitSizeToText(TotalBytes); }
      }


      /// <summary>The total number of files copied.</summary>
      public long TotalFiles { get; internal set; }


      /// <summary>The total number of folders copied.</summary>
      public long TotalFolders { get; internal set; }

      #endregion // Properties
   }
}

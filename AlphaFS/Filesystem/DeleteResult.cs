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
   /// <summary>Class for DeleteResult that contains the results for the delete action.</summary>
   /// <remarks>Normally there is no need to manually instantiate and/or populate this class.</remarks>
   [Serializable]
   public sealed class DeleteResult
   {
      #region Private Fields

      [NonSerialized] internal readonly Stopwatch Stopwatch;

      #endregion // Private Fields


      #region Constructors

      /// <summary>Initializes a DeleteResult instance for the delete action.</summary>
      private DeleteResult()
      {
      }


      internal DeleteResult(bool isFolder, string source)
      {
         IsDirectory = isFolder;

         Source = source;

         Retries = 0;

         Stopwatch = new Stopwatch();
      }

      #endregion // Constructors


      #region Properties

      /// <summary>Indicates the duration of the delete action.</summary>
      public TimeSpan Duration
      {
         get { return TimeSpan.FromMilliseconds(Stopwatch.Elapsed.TotalMilliseconds); }
      }
      

      /// <summary>The error code encountered during the delete action.</summary>
      /// <value>0 (zero) indicates success.</value>
      public int ErrorCode { get; internal set; }


      /// <summary>The error message from the <see cref="ErrorCode"/> that was encountered during the delete action.</summary>
      /// <value>A message describing the error.</value>
      [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
      public string ErrorMessage { get { return new Win32Exception(ErrorCode).Message; } }
      

      /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
      /// <value><c>true</c> if this instance represents a directory; otherwise, <c>false</c>.</value>
      internal bool IsDirectory { get; set; }
      

      /// <summary>Gets a value indicating whether this instance represents a file.</summary>
      /// <value><c>true</c> if this instance represents a file; otherwise, <c>false</c>.</value>
      public bool IsFile { get { return !IsDirectory; } }
      

      /// <summary>The total number of retry attempts.</summary>
      public long Retries { get; internal set; }


      /// <summary>Indicates the source file or directory.</summary>
      public string Source { get; private set; }


      /// <summary>The total number of bytes deleted.</summary>
      public long TotalBytes { get; internal set; }


      /// <summary>The total number of bytes deleted, formatted as a unit size.</summary>
      public string TotalBytesUnitSize
      {
         get { return Utils.UnitSizeToText(TotalBytes); }
      }


      /// <summary>The total number of files deleted.</summary>
      public long TotalFiles { get; internal set; }


      /// <summary>The total number of folders deleted.</summary>
      public long TotalFolders { get; internal set; }

      #endregion // Properties
   }
}

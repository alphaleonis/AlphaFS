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

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class for CopyMoveResult that contains the results for the Copy or Move action.</summary>
   public sealed class CopyMoveResult
   {
      /// <summary>Create a CopyMoveResult class instance for the Copy or Move action.</summary>
      /// <param name="source">Indicates the source file or directory.</param>
      /// <param name="destination">Indicates the destination file or directory.</param>
      /// <param name="isCanceled">Indicates if the action was canceled.</param>
      /// <param name="errorCode">The error code encountered during the Copy or Move action.</param>
      public CopyMoveResult(string source, string destination, bool isCanceled, int errorCode)
      {
         Source = source;
         Destination = destination;
         IsCanceled = isCanceled;
         ErrorCode = errorCode;
      }


      /// <summary>The error code encountered during the Copy or Move action.</summary>
      /// <value>0 (zero) indicates success.</value>
      public int ErrorCode { get; internal set; }


      /// <summary>When true indicates that the Copy or Move action was canceled.</summary>
      /// <value><see langword="true"/> when the Copy/Move action was canceled. Otherwise <see langword="false"/>.</value>
      public bool IsCanceled { get; internal set; }


      /// <summary>Indicates the source file or directory.</summary>
      public string Source { get; private set; }


      /// <summary>Indicates the destination file or directory.</summary>
      public string Destination { get; private set; }


      /// <summary>The total number of folders copied.</summary>
      public long TotalFolders { get; internal set; }


      /// <summary>The total number of files copied.</summary>
      public long TotalFiles { get; internal set; }


      /// <summary>The total number of bytes copied.</summary>
      public long TotalBytes { get; internal set; }
   }
}

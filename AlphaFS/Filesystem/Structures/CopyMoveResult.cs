/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.ComponentModel;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class for CopyMoveResult that contains the results for the Copy or Move action.</summary>
   public sealed class CopyMoveResult
   {
      #region ErrorCode

      /// <summary>
      ///   The error code encountered during the Copy or Move action.
      /// </summary>
      /// <value>0 (zero) indicates success.</value>
      public int ErrorCode { get; private set; }

      #endregion // ErrorCode

      #region ErrorMessage

      /// <summary>
      ///   The error message from the <see cref="ErrorCode"/> that was encountered during the Copy or Move action.
      /// </summary>
      /// <value>A message describing the error.</value>
      public string ErrorMessage { get; private set; }

      #endregion // ErrorMessage

      #region IsCanceled

      /// <summary>Indicates if the Copy or Move action was canceled.</summary>
      /// <value><see langword="true"/> when the Copy/Move action was canceled. Otherwise <see langword="false"/>.</value>
      public bool IsCanceled { get; private set; }

      #endregion // IsCanceled

      #region IsDirectory

      /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
      /// <value><see langword="true"/> if this instance represents a directory; otherwise, <see langword="false"/>.</value>
      public bool IsDirectory { get; private set; }

      #endregion // IsDirectory

      #region IsMove

      /// <summary>
      ///   Indicates if the action is a Copy or Move action.
      /// </summary>
      /// <value><see langword="true"/> when action is Move. Otherwise <see langword="false"/>.</value>
      public bool IsMove { get; private set; }

      #endregion // IsMove


      #region Source

      /// <summary>Indicates the source file or directory.</summary>
      public string Source { get; private set; }

      #endregion // Source

      #region Destination

      /// <summary>Indicates the destination file or directory.</summary>
      public string Destination { get; private set; }

      #endregion // Destination

      /// <summary>Create a CopyMoveResult class instance for the Copy or Move action.</summary>
      /// <param name="source">Indicates the source file or directory.</param>
      /// <param name="destination">Indicates the destination file or directory.</param>
      /// <param name="isDirectory">Indicates that the file system object is a directory.</param>
      /// <param name="isMove">Indicates if the action is a Copy or Move action.</param>
      /// <param name="isCanceled">The result of the Copy or Move action.</param>
      /// <param name="errorCode">The error code encountered during the Copy or Move action.</param>
      public CopyMoveResult(string source, string destination, bool isDirectory, bool isMove, bool isCanceled, int errorCode)
      {
         Source = source;
         Destination = destination;

         IsDirectory = isDirectory;

         IsMove = isMove;
         IsCanceled = isCanceled;

         ErrorCode = errorCode;
         ErrorMessage = new Win32Exception(errorCode).Message;
      }
   }
}
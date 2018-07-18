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
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>[AlphaFS] The operation could not be completed because the file is read-only.</summary>
   [Serializable]
   public class FileReadOnlyException : UnauthorizedAccessException
   {
      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      public FileReadOnlyException() : base(NativeError.ErrorMessage((int) Win32Errors.ERROR_FILE_READ_ONLY))
      {
      }


      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="path">The path to the file.</param>
      public FileReadOnlyException(string path) : base(NativeError.ErrorMessage((int) Win32Errors.ERROR_FILE_READ_ONLY, path))
      {
      }


      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="lastError">The Marshal.GetLastWin32Error() return code.</param>
      public FileReadOnlyException(string path, int lastError) : base(NativeError.ErrorMessage(lastError, path))
      {
      }


      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="innerException">The inner exception.</param>
      public FileReadOnlyException(string path, Exception innerException) : base(NativeError.ErrorMessage((int) Win32Errors.ERROR_FILE_READ_ONLY, path), innerException)
      {
      }


      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected FileReadOnlyException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}

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
using System.Globalization;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>The operation could not be completed because the file is read-only.</summary>
   [Serializable]
   public class FileReadOnlyException : UnauthorizedAccessException
   {
      /// <summary>Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      public FileReadOnlyException() : base(string.Format(CultureInfo.CurrentCulture, "({0}) The file is read-only.", Win32Errors.ERROR_FILE_READ_ONLY))
      {
      }

      /// <summary>Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="message">The message.</param>
      public FileReadOnlyException(string message) : base(string.Format(CultureInfo.CurrentCulture, "({0}) The file is read-only: [{1}]", Win32Errors.ERROR_FILE_READ_ONLY, message))
      {
      }

      /// <summary>Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public FileReadOnlyException(string message, Exception innerException) : base(string.Format(CultureInfo.CurrentCulture, "({0}) The file is read-only: [{1}]", Win32Errors.ERROR_FILE_READ_ONLY, message), innerException)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="FileReadOnlyException"/> class.</summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected FileReadOnlyException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}
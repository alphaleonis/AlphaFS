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
   /// <summary>The operation could not be completed because the directory is read-only.</summary>
   [Serializable]
   public class DirectoryReadOnlyException : System.IO.IOException
   {
      private static readonly int s_errorCode = Win32Errors.GetHrFromWin32Error(Win32Errors.ERROR_FILE_READ_ONLY);

      /// <summary>Initializes a new instance of the <see cref="DirectoryReadOnlyException"/> class.</summary>
      public DirectoryReadOnlyException() : base(string.Format(CultureInfo.CurrentCulture, "({0}) The directory is read-only", Win32Errors.ERROR_FILE_READ_ONLY), s_errorCode)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DirectoryReadOnlyException"/> class.</summary>
      /// <param name="message">The message.</param>
      public DirectoryReadOnlyException(string message) : base(string.Format(CultureInfo.CurrentCulture, "({0}) The directory is read-only: [{1}]", Win32Errors.ERROR_FILE_READ_ONLY, message), s_errorCode)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DirectoryReadOnlyException"/> class.</summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public DirectoryReadOnlyException(string message, Exception innerException) : base(string.Format(CultureInfo.CurrentCulture, "({0}) The directory is read-only: [{1}]", Win32Errors.ERROR_FILE_READ_ONLY, message), innerException)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DirectoryReadOnlyException"/> class.</summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected DirectoryReadOnlyException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}
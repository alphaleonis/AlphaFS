﻿/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   /// <summary>The exception that is thrown when an attempt perform an operation across difference devices when this is not supported.</summary>
   [Serializable]
   public class NotSameDeviceException : System.IO.IOException
   {
      private static readonly int s_errorCode = Win32Errors.GetHrFromWin32Error(Win32Errors.ERROR_NOT_SAME_DEVICE);

      /// <summary>Initializes a new instance of the <see cref="NotSameDeviceException"/> class.</summary>
      public NotSameDeviceException() : base(Resources.File_Or_Directory_Already_Exists, s_errorCode)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="NotSameDeviceException"/> class.</summary>
      /// <param name="message">The message.</param>
      public NotSameDeviceException(string message) : base(message, s_errorCode)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="NotSameDeviceException"/> class.</summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public NotSameDeviceException(string message, Exception innerException) : base(message, innerException)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="AlreadyExistsException"/> class.</summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected NotSameDeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}

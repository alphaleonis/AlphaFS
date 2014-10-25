/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
   /// <summary>The exception that is thrown when a pathname or filename is illegal.</summary>
   [SerializableAttribute]
   public class IllegalPathException : System.IO.IOException
   {
      /// <summary>Initializes a new instance of the <see cref="T:IllegalPathException"/> class.</summary>
      public IllegalPathException()
      {
      }

      /// <summary>Initializes a new instance of the <see cref="T:IllegalPathException"/> class.</summary>
      /// <param name="path">The path.</param>
      public IllegalPathException(string path)
         : base(string.Format(CultureInfo.CurrentCulture, Resources.IllegalPath, path))
      {
      }

      /// <summary>Initializes a new instance of the <see cref="T:IllegalPathException"/> class.</summary>
      /// <param name="path">The path.</param>
      /// <param name="innerException">The inner exception.</param>
      public IllegalPathException(string path, Exception innerException)
         : base(string.Format(CultureInfo.CurrentCulture, Resources.IllegalPath, path), innerException)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="T:IllegalPathException"/> class.</summary>
      /// <param name="info">The data for serializing or deserializing the object.</param>
      /// <param name="context">The source and destination for the object.</param>
      protected IllegalPathException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
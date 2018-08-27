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

using System.IO;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   partial class FileInfo
   {
      #region .NET

      /// <summary>Creates a <see cref="StreamWriter"/> that appends text to the file represented by this instance of the <see cref="FileInfo"/>.</summary>
      /// <returns>A new <see cref="StreamWriter"/></returns>
      [SecurityCritical]
      public StreamWriter AppendText()
      {
         return File.AppendTextCore(Transaction, LongFullName, NativeMethods.DefaultFileEncoding, PathFormat.LongFullPath);
      }


      /// <summary>Creates a <see cref="StreamWriter"/> that appends text to the file represented by this instance of the <see cref="FileInfo"/>.</summary>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <returns>Returns a new <see cref="StreamWriter"/></returns>
      [SecurityCritical]
      public StreamWriter AppendText(Encoding encoding)
      {
         return File.AppendTextCore(Transaction, LongFullName, encoding, PathFormat.LongFullPath);
      }

      #endregion // .NET
   }
}

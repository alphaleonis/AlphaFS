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

using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   partial class DirectoryInfo
   {
      #region .NET

      /// <summary>Creates a directory.</summary>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>

      [SecurityCritical]
      public void Create()
      {
         Directory.CreateDirectoryCore(Transaction, LongFullName, null, null, false, PathFormat.LongFullPath);
      }

      /// <summary>Creates a directory using a <see cref="System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity)
      {
         Directory.CreateDirectoryCore(Transaction, LongFullName, null, directorySecurity, false, PathFormat.LongFullPath);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a directory using a <see cref="System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(bool compress)
      {
         Directory.CreateDirectoryCore(Transaction, LongFullName, null, null, compress, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Creates a directory using a <see cref="System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <see langword="true"/> compresses the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>

      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity, bool compress)
      {
         Directory.CreateDirectoryCore(Transaction, LongFullName, null, directorySecurity, compress, PathFormat.LongFullPath);
      }

      #endregion // AlphaFS
   }
}

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

using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides a concrete implementation of SafeHandle supporting transactions.</summary>
   internal class SafeKernelTransactionHandle : SafeHandleMinusOneIsInvalid
   {
      /// <summary>Initializes a new instance of the <see cref="SafeKernelTransactionHandle"/> class.</summary>      
      public SafeKernelTransactionHandle() : base(true)
      {
      }


      /// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
      /// <returns>
      /// <c>true</c> if the handle is released successfully; otherwise, in the event of a catastrophic failure,
      /// <c>false</c>. In this case, it generates a ReleaseHandleFailed Managed Debugging Assistant.
      /// </returns>
      [SecurityCritical]
      protected override bool ReleaseHandle()
      {
         return NativeMethods.CloseHandle(handle);
      }
   }
}

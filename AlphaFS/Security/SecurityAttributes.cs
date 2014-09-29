/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Security
{
   internal static partial class NativeMethods
   {
      /// <summary>Class used to represent the SECURITY_ATTRIBUES native win32 structure. It provides initialization function from an <see cref="T:ObjectSecurity"/> object.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal sealed class SecurityAttributes : IDisposable
      {
         [MarshalAs(UnmanagedType.U4)]
         private int nLength;
                  
         private SafeHandle lpSecurityDescriptor;

         [MarshalAs(UnmanagedType.Bool)]
         private bool bInheritHandle;

         public SecurityAttributes(ObjectSecurity securityDescriptor)
         {
            SafeGlobalMemoryBufferHandle handle = ToUnmanagedSecurityAttributes(securityDescriptor);
            nLength = handle.Capacity;
            lpSecurityDescriptor = handle;
            bInheritHandle = false;
         }

         /// <summary>
         /// Marshals an ObjectSecurity instance to unmanaged memory.
         /// </summary>
         /// <param name="securityDescriptor">The security descriptor.</param>
         /// <returns>A safe handle containing the marshalled security descriptor.</returns>
         [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
         private static SafeGlobalMemoryBufferHandle ToUnmanagedSecurityAttributes(ObjectSecurity securityDescriptor)
         {
            if (securityDescriptor == null)
            {
               return new SafeGlobalMemoryBufferHandle();
            }
            else
            {
               byte[] src = securityDescriptor.GetSecurityDescriptorBinaryForm();
               SafeGlobalMemoryBufferHandle memoryHandle = new SafeGlobalMemoryBufferHandle(src.Length);
               try
               {
                  memoryHandle.CopyFrom(src, 0, src.Length);
                  return memoryHandle;
               }
               catch
               {
                  memoryHandle.Dispose();
                  throw;
               }
            }
         }

         //private uint nLength;

         public void Dispose()
         {
            if (lpSecurityDescriptor != null)
               lpSecurityDescriptor.Dispose();
         }
      }
   }
}
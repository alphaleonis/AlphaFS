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

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>The NETRESOURCE struct contains information about a network resource.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct NetResource
      {
         /// <summary>The scope of the enumeration. This member can be one of the following ResourceScope values.</summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceScope Scope;

         /// <summary>The type of resource. This member can be one of the following <see cref="T:ResourceType"/> values.</summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceType Type;

         /// <summary>The display options for the network object in a network browsing user interface. This member can be one of the following ResourceDisplayType values.</summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceDisplayType DisplayType;

         /// <summary>A set of bit flags describing how the resource can be used. This member can be one of the following ResourceUsage values.</summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceUsage Usage;

         /// <summary>If the <see cref="T:Scope"/> member is equal to ResourceScope.Connected or ResourceScope.Remembered.
         /// This member is a pointer to a null-terminated character string that specifies the name of a local device. 
         /// This member is NULL if the connection does not use a device.
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string LocalName;

         /// <summary>If the entry is a network resource, this member is a <see cref="T:string"/> that specifies the remote network name.
         /// If the entry is a current or persistent connection, <see cref="T:RemoteName"/> member points to the network name associated with the name pointed to by the <see cref="T:LocalName"/> member.
         /// The string can be <see cref="T:Alphaleonis.Win32.Filesystem.NativeMethods.MaxPath"/> characters in length, and it must follow the network provider's naming conventions.
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string RemoteName;

         /// <summary>A <see cref="T:string"/> that contains a comment supplied by the network provider.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string Comment;

         /// <summary>A <see cref="T:string"/> that contains the name of the provider that owns the resource. 
         /// This member can be <c>null</c> if the provider name is unknown. To retrieve the provider name, you can call the WNetGetProviderName function.
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string Provider;
      }
   }
}
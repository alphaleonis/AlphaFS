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

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>The NETRESOURCE struct contains information about a network resource.
      /// <para>&#160;</para>
      /// <para>The NETRESOURCE structure is returned during an enumeration of network resources.</para>
      /// <para>The NETRESOURCE structure is also specified when making or querying</para>
      /// <para>a network connection with calls to various Windows Networking functions.</para>
      /// </summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct NetResource
      {
         #region Scope

         /// <summary>The scope of the enumeration.
         /// <para>&#160;</para>
         /// <para>This member can be one of the following <see cref="ResourceScope"/> values.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceScope Scope;

         #endregion // Scope

         #region Type

         /// <summary>The type of resource.
         /// <para>&#160;</para>
         /// <para>This member can be one of the following <see cref="ResourceType"/> values.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceType Type;

         #endregion // Type

         #region DisplayType

         /// <summary>The display options for the network object in a network browsing user interface.
         /// <para>&#160;</para>
         /// <para>This member can be one of the following <see cref="ResourceDisplayType"/> values.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceDisplayType DisplayType;

         #endregion // DisplayType

         #region Usage

         /// <summary>A set of bit flags describing how the resource can be used.</summary>
         [MarshalAs(UnmanagedType.U4)] public ResourceUsage Usage;

         #endregion // Usage

         #region LocalName

         /// <summary>If the <see cref="Scope"/> member is equal to <see cref="ResourceScope.Connected"/> or <see cref="ResourceScope.Remembered"/>,
         /// <para>this member is a pointer to a <see langword="null"/>-terminated character string that specifies the name of a local device.</para>
         /// <para>This member is <see langword="null"/> if the connection does not use a device.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string LocalName;

         #endregion // LocalName

         #region RemoteName

         /// <summary>If the entry is a network resource, this member is a <see cref="string"/>
         /// <para>that specifies the remote network name.</para>
         /// <para>&#160;</para>
         /// <para>If the entry is a current or persistent connection, <see cref="RemoteName"/> member points to </para>
         /// <para>the network name associated with the name pointed to by the <see cref="LocalName"/> member.</para>
         /// <para>&#160;</para>
         /// <para>The <see cref="string"/> can be <see cref="Alphaleonis.Win32.Filesystem.NativeMethods.MaxPath"/> characters</para>
         /// <para>in length, and it must follow the network provider's naming conventions.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string RemoteName;

         #endregion // RemoteName

         #region Comment

         /// <summary>A <see cref="string"/> that contains a comment supplied by the network provider.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string Comment;

         #endregion // Comment

         #region Provider

         /// <summary>A <see cref="string"/> that contains the name of the provider that owns the resource. 
         /// <para>&#160;</para>
         /// <para>This member can be <see langword="null"/> if the provider name is unknown.</para>
         /// <para>To retrieve the provider name, you can call the WNetGetProviderName function.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public string Provider;

         #endregion // Provider
      }
   }
}
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
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains the name and type (domain-based or stand-alone) of a DFS namespace.</summary>
      /// <remarks>The DFS functions use the <see cref="DFS_INFO_300"/> structure to enumerate DFS namespaces hosted on a machine.</remarks>
      /// <remarks>Minimum supported client: Windows XP with SP1 [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct DFS_INFO_300
      {
         /// <summary>Value that specifies the type of the DFS namespace. This member can be one of the <see cref="DfsNamespaceFlavors"/> values.</summary>
         [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
         [MarshalAs(UnmanagedType.U4)] public readonly DfsNamespaceFlavors Flags;

         /// <summary>The name of a DFS namespace.
         /// This member can have one of the following two formats:
         /// The first format is: \ServerName\DfsName
         /// where ServerName is the name of the root target server that hosts the stand-alone DFS namespace and DfsName is the name of the DFS namespace.
         /// The second format is:
         /// \DomainName\DomDfsName
         /// where DomainName is the name of the domain that hosts the domain-based DFS namespace and DomDfsname is the name of the DFS namespace.
         /// </summary>
         [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string DfsName;
      }
   }
}

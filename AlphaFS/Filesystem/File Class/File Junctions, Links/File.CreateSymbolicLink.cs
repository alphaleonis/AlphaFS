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

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Obsolete

      /// <summary>[AlphaFS] Creates a symbolic link (similar to CMD command: "MKLINK") to a file.</summary>
      /// <remarks>See <see cref="Security.Privilege.CreateSymbolicLink"/> to run this method from an elevated state.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      [Obsolete("Methods with SymbolicLinkTarget parameter are obsolete.")]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkCore(null, symlinkFileName, targetFileName, targetType, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates a symbolic link (similar to CMD command: "MKLINK") to a file.</summary>
      /// <remarks>See <see cref="Security.Privilege.CreateSymbolicLink"/> to run this method from an elevated state.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      [Obsolete("Methods with SymbolicLinkTarget parameter are obsolete.")]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         CreateSymbolicLinkCore(null, symlinkFileName, targetFileName, targetType, pathFormat);
      }


      /// <summary>[AlphaFS] Creates a symbolic link (similar to CMD command: "MKLINK") to a file as a transacted operation.</summary>
      /// <remarks>See <see cref="Security.Privilege.CreateSymbolicLink"/> to run this method from an elevated state.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      [Obsolete("Methods with SymbolicLinkTarget parameter are obsolete.")]
      public static void CreateSymbolicLinkTransacted(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkCore(transaction, symlinkFileName, targetFileName, targetType, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates a symbolic link (similar to CMD command: "MKLINK") to a file as a transacted operation.</summary>
      /// <remarks>See <see cref="Security.Privilege.CreateSymbolicLink"/> to run this method from an elevated state.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      [Obsolete("Methods with SymbolicLinkTarget parameter are obsolete.")]
      public static void CreateSymbolicLinkTransacted(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         CreateSymbolicLinkCore(transaction, symlinkFileName, targetFileName, targetType, pathFormat);
      }

      #endregion // Obsolete


      /// <summary>[AlphaFS] Creates a symbolic link (similar to CMD command: "MKLINK") to a file.</summary>
      /// <remarks>
      ///   Symbolic links can point to a non-existent target.
      ///   When creating a symbolic link, the operating system does not check to see if the target exists.
      ///   Symbolic links are reparse points.
      ///   There is a maximum of 31 reparse points (and therefore symbolic links) allowed in a particular path.
      ///   See <see cref="Security.Privilege.CreateSymbolicLink"/> to run this method from an elevated state.
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName)
      {
         CreateSymbolicLinkCore(null, symlinkFileName, targetFileName, SymbolicLinkTarget.File, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Creates a symbolic link (similar to CMD command: "MKLINK") to a file.</summary>
      /// <remarks>
      ///   Symbolic links can point to a non-existent target.
      ///   When creating a symbolic link, the operating system does not check to see if the target exists.
      ///   Symbolic links are reparse points.
      ///   There is a maximum of 31 reparse points (and therefore symbolic links) allowed in a particular path.
      ///   See <see cref="Security.Privilege.CreateSymbolicLink"/> to run this method from an elevated state.
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, PathFormat pathFormat)
      {
         CreateSymbolicLinkCore(null, symlinkFileName, targetFileName, SymbolicLinkTarget.File, pathFormat);
      }
   }
}

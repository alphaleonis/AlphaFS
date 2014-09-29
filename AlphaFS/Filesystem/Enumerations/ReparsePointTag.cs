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

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Enumeration specifying the different reparse point tags.</summary>
      internal enum ReparsePointTag : uint
      {
         /// <summary>The entry is not a reparse point.</summary>
         None = 0,

         /// <summary>IO_REPARSE_TAG_DRIVER_EXTENDER (0x80000005) - Used by Home server drive extender.</summary>
         TagDriverExtender = 2147483653,

         /// <summary>IO_REPARSE_TAG_SIS (0x80000007) - Used by single-instance storage (SIS) filter driver.</summary>
         Sis = 2147483655,

         /// <summary>IO_REPARSE_TAG_DFS (0x8000000A) - Used by the DFS filter.</summary>
         Dfs = 2147483658,

         /// <summary>IO_REPARSE_TAG_DFSR (0x80000012) - Used by the DFS filter.</summary>
         Dfsr = 2147483666,

         /// <summary>IO_REPARSE_TAG_MOUNT_POINT (0xA0000003) - Used for mount point support.
         /// To determine if the reparse point is a mounted folder (and not some other form of reparse point), test whether the tag value equals the value IO_REPARSE_TAG_MOUNT_POINT.
         /// </summary>
         MountPoint = 2684354563,

         /// <summary>IO_REPARSE_TAG_SYMLINK (0xA000000C) - Used for symbolic link support.</summary>
         SymLink = 2684354572
      }
   }
}
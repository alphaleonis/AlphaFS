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

using System;
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about the shared resource.</summary>
   /// <remarks>
   /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
   /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
   /// </remarks>
   [Flags]
   public enum ShareResourceTypes
   {
      /// <summary>No specific resource type used.</summary>
      None = 0,

      /// <summary>SHI1005_FLAGS_DFS
      /// <para>The specified share is present in a DFS tree structure. This flag cannot be set with NetShareSetInfo.</para>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      Dfs = 1,

      /// <summary>SHI1005_FLAGS_DFS_ROOT
      /// <para>The specified share is the root volume in a DFS tree structure. This flag cannot be set with NetShareSetInfo.</para>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      DfsRoot = 2,

      /// <summary>SHI1005_FLAGS_RESTRICT_EXCLUSIVE_OPENS 
      /// <para>The specified share disallows exclusive file opens, where reads to an open file are disallowed.</para>
      /// </summary>
      RestrictExclusiveOpens = 256,

      /// <summary>SHI1005_FLAGS_FORCE_SHARED_DELETE
      /// <para>Shared files in the specified share can be forcibly deleted.</para>
      /// </summary>
      ForceSharedDelete = 512,

      /// <summary>SHI1005_FLAGS_ALLOW_NAMESPACE_CACHING
      /// <para>Clients are allowed to cache the namespace of the specified share.</para>
      /// </summary>
      AllowNamespaceCaching = 1024,

      /// <summary>SHI1005_FLAGS_ACCESS_BASED_DIRECTORY_ENUM
      /// <para>The server will filter directory entries based on the access permissions that the user on the client computer has for the server on which the files reside.</para>
      /// <para>Only files for which the user has read access and directories for which the user has FILE_LIST_DIRECTORY access will be returned. If the user has SeBackupPrivilege, all available information will be returned.</para>
      /// </summary>
      /// <remarks>This flag is supported only on servers running Windows Server 2003 with SP1 or later.</remarks>
      AccessBasedDirectoryEnum = 2048,

      /// <summary>SHI1005_FLAGS_FORCE_LEVELII_OPLOCK
      /// <para>Prevents exclusive caching modes that can cause delays for highly shared read-only data.</para>
      /// </summary>
      /// <remarks>This flag is supported only on servers running Windows Server 2008 R2 or later.</remarks>
      ForceLevel2OpLock = 4096,

      /// <summary>SHI1005_FLAGS_ENABLE_HASH</summary>
      /// <para>Enables server-side functionality needed for peer caching support.</para>
      /// <para>Clients on high-latency or low-bandwidth connections can use alternate methods to retrieve data from peers if available, instead of sending requests to the server.</para>
      /// <para>This is only supported on shares configured for manual caching (CSC_CACHE_MANUAL_REINT).</para>
      /// <remarks>This flag is supported only on servers running Windows Server 2008 R2 or later.</remarks>
      EnableHash = 8192,

      /// <summary>SHI1005_FLAGS_ENABLE_CA (0X4000) - Enables server-side functionality needed for peer caching support. Clients on high-latency or low-bandwidth connections can use alternate methods to retrieve data from peers if available, instead of sending requests to the server. This is only supported on shares configured for manual caching (CSC_CACHE_MANUAL_REINT).</summary>
      /// <remarks>Windows 7, Windows Server 2008 R2, Windows Vista, Windows Server 2008, and Windows Server 2003:  This flag is not supported.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ca")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ca")]
      EnableCa = 16384
   }
}
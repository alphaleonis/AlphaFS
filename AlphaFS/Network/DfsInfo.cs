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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about a Distributed File System (DFS) root or link. This class cannot be inherited.
   /// <para>This structure contains the name, status, GUID, time-out, number of targets, and information about each target of the root or link.</para>
   /// </summary>
   [SerializableAttribute]
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   public sealed class DfsInfo
   {
      #region Constructor

      /// <summary>Initializes a new instance of the <see cref="DfsInfo"/> class which acts as a wrapper for a DFS root or link target.</summary>
      public DfsInfo()
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DfsInfo"/> class, which acts as a wrapper for a DFS root or link target.</summary>
      /// <param name="structure">An initialized <see cref="NativeMethods.DFS_INFO_9"/> instance.</param>
      internal DfsInfo(NativeMethods.DFS_INFO_9 structure)
      {
         Comment = structure.Comment;
         EntryPath = structure.EntryPath;
         State = structure.State;
         Timeout = structure.Timeout;
         Guid = structure.Guid;
         MetadataSize = structure.MetadataSize;
         PropertyFlags = structure.PropertyFlags;
         SecurityDescriptor = structure.pSecurityDescriptor;

         if (structure.NumberOfStorages > 0)
         {
            var typeOfStruct = typeof (NativeMethods.DFS_STORAGE_INFO_1);
            var sizeOfStruct = Marshal.SizeOf(typeOfStruct);

            for (int i = 0; i < structure.NumberOfStorages; i++)
               _storageInfoCollection.Add(new DfsStorageInfo((NativeMethods.DFS_STORAGE_INFO_1) Marshal.PtrToStructure(new IntPtr(structure.Storage.ToInt64() + i*sizeOfStruct), typeOfStruct)));
         }
      }

      #endregion // Constructor

      #region Methods

      /// <summary>Returns the Universal Naming Convention (UNC) path of the DFS root or link.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return EntryPath;
      }

      #endregion // Methods

      #region Properties

      private DirectoryInfo _directoryInfo;

      /// <summary>The <see cref="DirectoryInfo"/> instance of the DFS root or link.</summary>
      public DirectoryInfo DirectoryInfo
      {
         get { return _directoryInfo ?? (_directoryInfo = new DirectoryInfo(null, EntryPath, PathFormat.FullPath)); }
      }

      /// <summary>The comment of the DFS root or link.</summary>
      public string Comment { get; internal set; }

      /// <summary>The Universal Naming Convention (UNC) path of the DFS root or link.</summary>
      public string EntryPath { get; internal set; }

      /// <summary>Specifies the GUID of the DFS root or link.</summary>
      public Guid Guid { get; internal set; }


      private readonly List<DfsStorageInfo> _storageInfoCollection = new List<DfsStorageInfo>();

      /// <summary>The collection of DFS targets of the DFS root or link.</summary>
      public IEnumerable<DfsStorageInfo> StorageInfoCollection
      {
         get { return _storageInfoCollection; }
      }

      /// <summary>An <see cref="DfsVolumeStates"/> enum that specifies a set of bit flags that describe the DFS root or link.</summary>
      public DfsVolumeStates State { get; internal set; }

      //DfsVolumeStates flavorBits = (structure3.State & (DfsVolumeStates) DfsNamespaceFlavors.All);
      //If (flavorBits == DFS_VOLUME_FLAVOR_STANDALONE)     // Namespace is stand-alone DFS.
      //else if (flavorBits == DFS_VOLUME_FLAVOR_AD_BLOB)   // Namespace is AD Blob.
      //else StateBits = (Flavor & DFS_VOLUME_STATES)        // Unknown flavor.
      // StateBits can be one of the following: 
      //  (DFS_VOLUME_STATE_OK, DFS_VOLUME_STATE_INCONSISTENT, 
      //   DFS_VOLUME_STATE_OFFLINE or DFS_VOLUME_STATE_ONLINE)
      //State = flavorBits | structure3.State;

      /// <summary>Specifies the time-out, in seconds, of the DFS root or link.</summary>
      public long Timeout { get; internal set; }

      /// <summary>Specifies a set of flags that describe specific properties of a DFS namespace, root, or link.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
      public DfsPropertyFlags PropertyFlags { get; internal set; }
      
      /// <summary>For domain-based DFS namespaces, this member specifies the size of the corresponding Active Directory data blob, in bytes.
      /// For stand-alone DFS namespaces, this field specifies the size of the metadata stored in the registry,
      /// including the key names and value names, in addition to the specific data items associated with them. This field is valid for DFS roots only.
      /// </summary>
      public long MetadataSize { get; internal set; }


      /// <summary>Pointer to a SECURITY_DESCRIPTOR structure that specifies a self-relative security descriptor to be associated with the DFS link's reparse point.
      /// This field is valid for DFS links only.
      /// </summary>
      public IntPtr SecurityDescriptor { get; internal set; }
      
      #endregion // Properties
   }
}

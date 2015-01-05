/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Globalization;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about Server Message Block (SMB) shares. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed class ShareInfo
   {
      #region Constructor
      
      #region ShareInfo

      /// <summary>Creates a <see cref="ShareInfo"/> instance.</summary>
      /// <param name="host">A host to retrieve shares from.</param>
      /// <param name="shareLevel">Possible structure levels: 503, 2,  1 and 1005.</param>
      /// <param name="shareInfo">A <see cref="NativeMethods.ShareInfo2"/> or <see cref="NativeMethods.ShareInfo503"/> instance.</param>
      internal ShareInfo(string host, int shareLevel, object shareInfo)
      {
         switch (shareLevel)
         {
            case 1005:
               NativeMethods.ShareInfo1005 s1005 = (NativeMethods.ShareInfo1005) shareInfo;
               ServerName = host ?? Environment.MachineName;
               ResourceType = s1005.ShareResourceType;
               break;

            case 503:
               NativeMethods.ShareInfo503 s503 = (NativeMethods.ShareInfo503) shareInfo;
               CurrentUses = s503.CurrentUses;
               MaxUses = s503.MaxUses;
               NetName = s503.NetName;
               Password = s503.Password;
               Path = Utils.IsNullOrWhiteSpace(s503.Path) ? null : s503.Path;
               Permissions = s503.Permissions;
               Remark = s503.Remark;
               ServerName = s503.ServerName == "*" ? host ?? Environment.MachineName : s503.ServerName;
               ShareType = s503.ShareType;
               SecurityDescriptor = s503.SecurityDescriptor;
               break;

            case 2:
               NativeMethods.ShareInfo2 s2 = (NativeMethods.ShareInfo2)shareInfo;
               CurrentUses = s2.CurrentUses;
               MaxUses = s2.MaxUses;
               NetName = s2.NetName;
               Password = s2.Password;
               Path = Utils.IsNullOrWhiteSpace(s2.Path) ? null : s2.Path;
               Permissions = s2.Permissions;
               Remark = s2.Remark;
               ServerName = host ?? Environment.MachineName;
               ShareType = s2.ShareType;
               break;

            case 1:
               NativeMethods.ShareInfo1 s1 = (NativeMethods.ShareInfo1)shareInfo;
               CurrentUses = 0;
               MaxUses = 0;
               NetName = s1.NetName;
               Password = null;
               Path = null;
               Permissions = AccessPermissions.None;
               Remark = s1.Remark;
               ServerName = host ?? Environment.MachineName;
               ShareType = s1.ShareType;
               break;
         }

         NetFullPath = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}{3}", Filesystem.Path.UncPrefix, ServerName, Filesystem.Path.DirectorySeparatorChar, NetName);
         ShareLevel = shareLevel;
      }

      #endregion // ShareInfo

      #endregion // Constructor

      #region Methods

      #region ToString

      /// <summary>Returns the full path to the share.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return NetFullPath;
      }

      #endregion // ToString

      #endregion // Methods

      #region Properties

      #region CurrentUses

      /// <summary>The number of current connections to the resource.</summary>
      public long CurrentUses { get; private set; }

      #endregion // CurrentUses

      #region DirectoryInfo

      private DirectoryInfo _directoryInfo;

      /// <summary>The <see cref="DirectoryInfo"/> instance associated with this share.</summary>
      public DirectoryInfo DirectoryInfo
      {
         get
         {
            // Do not use ?? expression here.
            if (_directoryInfo == null)
               _directoryInfo = new DirectoryInfo(null, NetFullPath, PathFormat.Standard);

            return _directoryInfo;
         }
      }

      #endregion // DirectoryInfo

      #region NetFullPath

      /// <summary>Returns the full UNC path to the share.</summary>
      public string NetFullPath { get; internal set; }

      #endregion // NetFullPath

      #region MaxUses

      /// <summary>The maximum number of concurrent connections that the shared resource can accommodate.</summary>
      /// <remarks>The number of connections is unlimited if the value specified in this member is –1.</remarks>
      public long MaxUses { get; private set; }

      #endregion // MaxUses

      #region NetName

      /// <summary>The name of a shared resource.</summary>
      public string NetName { get; private set; }

      #endregion // NetName

      #region Password

      /// <summary>The share's password (when the server is running with share-level security).</summary>
      public string Password { get; private set; }

      #endregion // Password

      #region Path

      /// <summary>The local path for the shared resource.</summary>
      /// <remarks>For disks, this member is the path being shared. For print queues, this member is the name of the print queue being shared.</remarks>
      public string Path { get; private set; }

      #endregion // Path

      #region Permissions

      /// <summary>The shared resource's permissions for servers running with share-level security.</summary>
      /// <remarks>Note that Windows does not support share-level security. This member is ignored on a server running user-level security.</remarks>
      public AccessPermissions Permissions { get; private set; }

      #endregion // Permissions

      #region Remark

      /// <summary>An optional comment about the shared resource.</summary>
      public string Remark { get; private set; }

      #endregion // Remark

      #region SecurityDescriptor

      /// <summary>Specifies the SECURITY_DESCRIPTOR associated with this share.</summary>
      public IntPtr SecurityDescriptor { get; private set; }

      #endregion // SecurityDescriptor

      #region ServerName

      /// <summary>A pointer to a string that specifies the DNS or NetBIOS name of the remote server on which the shared resource resides.</summary>
      /// <remarks>A value of "*" indicates no configured server name.</remarks>
      public string ServerName { get; private set; }

      #endregion // ServerName

      #region ShareType

      /// <summary>The type of share.</summary>
      public ShareType ShareType { get; set; }

      #endregion // ShareType

      #region ResourceType

      private ShareResourceTypes _shareResourceType;

      /// <summary>The type of share resource.</summary>
      public ShareResourceTypes ResourceType
      {
         get
         {
            if (_shareResourceType == ShareResourceTypes.None && !Utils.IsNullOrWhiteSpace(NetName))
               _shareResourceType = (Host.GetShareInfoInternal(1005, ServerName, NetName, true)).ResourceType;

            return _shareResourceType;
         }

         set { _shareResourceType = value; }
      }

      #endregion // ResourceType


      #region ShareLevel

      /// <summary>The structure level for the ShareInfo instance. Possible structure levels: 503, 2, 1 and 1005.</summary>
      public int ShareLevel { get; private set; }

      #endregion // ShareLevel
      
      #endregion // Properties
   }
}
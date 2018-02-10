/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about Server Message Block (SMB) shares. This class cannot be inherited.</summary>
   [Serializable]
   public sealed class SessionInfo
   {
      #region Constructor

      /// <summary>Creates a <see cref="SessionInfo"/> instance.</summary>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="sessionInfoLevel">One of the <see cref="SessionInfoLevel"/> options.</param>
      /// <param name="sessionInfo">A <see cref="NativeMethods.SESSION_INFO_0"/>, <see cref="NativeMethods.SESSION_INFO_1"/>, <see cref="NativeMethods.SESSION_INFO_2"/>, <see cref="NativeMethods.SESSION_INFO_10"/> or <see cref="NativeMethods.SESSION_INFO_502"/> instance.</param>
      internal SessionInfo(string hostName, SessionInfoLevel sessionInfoLevel, object sessionInfo)
      {
         hostName = hostName ?? Environment.MachineName;


         switch (sessionInfoLevel)
         {
            case SessionInfoLevel.Info502:
               var sesi502 = (NativeMethods.SESSION_INFO_502) sessionInfo;
               HostName = sesi502.sesi502_cname;
               break;

            case SessionInfoLevel.Info2:
               var sesi2 = (NativeMethods.SESSION_INFO_2) sessionInfo;
               HostName = sesi2.sesi2_cname;
               break;

            case SessionInfoLevel.Info1:
               var sesi1 = (NativeMethods.SESSION_INFO_1) sessionInfo;
               HostName = sesi1.sesi1_cname;
               break;

            case SessionInfoLevel.Info10:
               var sesi10 = (NativeMethods.SESSION_INFO_10) sessionInfo;
               HostName = sesi10.sesi10_cname;
               break;

            case SessionInfoLevel.Info0:
               var sesi0 = (NativeMethods.SESSION_INFO_0) sessionInfo;
               HostName = sesi0.sesi0_cname;
               break;
         }


         SessionInfoLevel = sessionInfoLevel;
      }

      #endregion // Constructor


      #region Methods

      /// <summary>Returns the host name of this session information.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return HostName;
      }

      #endregion // Methods


      #region Properties

      /// <summary>The number of current connections to the resource.</summary>
      public long CurrentUses { get; private set; }


      /// <summary>The maximum number of concurrent connections that the shared resource can accommodate.</summary>
      /// <remarks>The number of connections is unlimited if the value specified in this member is –1.</remarks>
      public long MaxUses { get; private set; }


      /// <summary>The name of a shared resource.</summary>
      public string NetName { get; private set; }


      /// <summary>The share's password (when the server is running with share-level security).</summary>
      public string Password { get; private set; }


      /// <summary>The local path for the shared resource.</summary>
      /// <remarks>For disks, this member is the path being shared. For print queues, this member is the name of the print queue being shared.</remarks>
      public string Path { get; private set; }


      /// <summary>The shared resource's permissions for servers running with share-level security.</summary>
      /// <remarks>Note that Windows does not support share-level security. This member is ignored on a server running user-level security.</remarks>
      public AccessPermissions Permissions { get; private set; }


      /// <summary>An optional comment about the shared resource.</summary>
      public string Remark { get; private set; }


      /// <summary>Specifies the SECURITY_DESCRIPTOR associated with this share.</summary>
      public IntPtr SecurityDescriptor { get; private set; }


      /// <summary>A pointer to a string that specifies the DNS or NetBIOS name of the remote server on which the shared resource resides.</summary>
      /// <remarks>A value of "*" indicates no configured server name.</remarks>
      public string HostName { get; private set; }


      /// <summary>The structure level for the SessionInfo instance.</summary>
      public SessionInfoLevel SessionInfoLevel { get; private set; }

      #endregion // Properties
   }
}

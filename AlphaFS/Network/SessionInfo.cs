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
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about Server Message Block (SMB) shares. This class cannot be inherited.</summary>
   [Serializable]
   public sealed class SessionInfo
   {
      #region Private Fields

      private string _netName;

      #endregion // Private Fields


      #region Constructor

      /// <summary>Creates a <see cref="SessionInfo"/> instance.</summary>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="sessionLevel">One of the <see cref="SessionInfoLevel"/> options.</param>
      /// <param name="structure">
      /// A <see cref="NativeMethods.SESSION_INFO_502"/>, <see cref="NativeMethods.SESSION_INFO_2"/>,
      /// <see cref="NativeMethods.SESSION_INFO_1"/>, <see cref="NativeMethods.SESSION_INFO_10"/> or <see cref="NativeMethods.SESSION_INFO_0"/> instance.
      /// </param>
      internal SessionInfo(string hostName, SessionInfoLevel sessionLevel, object structure)
      {
         var flags = 0;


         switch (sessionLevel)
         {
            case SessionInfoLevel.Info502:
               var sesi502 = (NativeMethods.SESSION_INFO_502) structure;
               NetName = sesi502.sesi502_cname;
               UserName = sesi502.sesi502_username;
               OpenedResources = (int) sesi502.sesi502_num_opens;
               ActiveTime = TimeSpan.FromSeconds((int) sesi502.sesi502_time);
               IdleTime = TimeSpan.FromSeconds((int) sesi502.sesi502_idle_time);
               ClientType = sesi502.sesi502_cltype_name;
               TransportType = sesi502.sesi502_transport;
               flags = (int) sesi502.sesi502_user_flags;
               break;


            case SessionInfoLevel.Info2:
               var sesi2 = (NativeMethods.SESSION_INFO_2) structure;
               NetName = sesi2.sesi2_cname;
               UserName = sesi2.sesi2_username;
               OpenedResources = (int) sesi2.sesi2_num_opens;
               ActiveTime = TimeSpan.FromSeconds((int) sesi2.sesi2_time);
               ClientType = sesi2.sesi2_cltype_name;
               flags = (int) sesi2.sesi2_user_flags;
               break;


            case SessionInfoLevel.Info1:
               var sesi1 = (NativeMethods.SESSION_INFO_1) structure;
               NetName = sesi1.sesi1_cname;
               UserName = sesi1.sesi1_username;
               OpenedResources = (int) sesi1.sesi1_num_opens;
               ActiveTime = TimeSpan.FromSeconds((int) sesi1.sesi1_time);
               IdleTime = TimeSpan.FromSeconds((int) sesi1.sesi1_idle_time);
               flags = (int) sesi1.sesi1_user_flags;
               break;


            case SessionInfoLevel.Info10:
               var sesi10 = (NativeMethods.SESSION_INFO_10) structure;
               NetName = sesi10.sesi10_cname;
               UserName = sesi10.sesi10_username;
               ActiveTime = TimeSpan.FromSeconds((int) sesi10.sesi10_time);
               IdleTime = TimeSpan.FromSeconds((int) sesi10.sesi10_idle_time);
               break;


            case SessionInfoLevel.Info0:
               var sesi0 = (NativeMethods.SESSION_INFO_0) structure;
               NetName = sesi0.sesi0_cname;
               break;
         }


         HostName = hostName;

         SessionLevel = sessionLevel;
         
         // SESS_GUEST = 1,
         // SESS_NOENCRYPTION = 2

         GuestSession = flags == 1;

         EncryptedSession = !GuestSession && flags != 2;
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

      /// <summary>The host name of this session information.</summary>
      public string HostName { get; private set; }


      /// <summary>The Computer name or IP address that established the session.</summary>
      public string NetName
      {
         get { return _netName; }

         set { _netName = null != value ? value.Replace(Path.LongPathUncPrefix, string.Empty).Replace(Path.UncPrefix, string.Empty).Trim('[', ']') : null; }
      }


      /// <summary>The name of the User who established the session.</summary>
      public string UserName { get; private set; }


      /// <summary>The number of files, devices, and pipes opened during the session.</summary>
      public int OpenedResources { get; private set; }


      /// <summary>The session active duration.</summary>
      public TimeSpan ActiveTime { get; private set; }


      /// <summary>The session idle duration.</summary>
      public TimeSpan IdleTime { get; private set; }


      ///// <summary>
      ///// <para>Specifies a value that describes how the user established the session. This member can be one of the following values:</para>
      ///// <para>SESS_GUEST: The user specified by the <see cref="UserName"/> member established the session using a guest account.</para>
      ///// <para>SESS_NOENCRYPTION: The user specified by the <see cref="UserName"/> member established the session without using password encryption.</para>
      ///// </summary>
      ////public int Flags { get; private set; }

      /// <summary>A value that describes how the User established the session.</summary>
      public bool GuestSession { get; private set; }

      /// <summary>A value that describes how the User established the session.</summary>
      public bool EncryptedSession { get; private set; }


      /// <summary>The type of client that established the session. Sessions from LAN Manager servers running UNIX also will appear as LAN Manager 2.0.</summary>
      public string ClientType { get; private set; }


      /// <summary>The name of the transport that the client is using to communicate with the server.</summary>
      public string TransportType { get; private set; }


      /// <summary>The structure level for the <see cref="SessionInfo"/> instance.</summary>
      public SessionInfoLevel SessionLevel { get; private set; }

      #endregion // Properties
   }
}

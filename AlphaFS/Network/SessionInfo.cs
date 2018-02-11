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
      /// <param name="sessionInfoLevel">One of the <see cref="SessionInfoLevel"/> options.</param>
      /// <param name="sessionInfo">A <see cref="NativeMethods.SESSION_INFO_0"/>, <see cref="NativeMethods.SESSION_INFO_1"/>, <see cref="NativeMethods.SESSION_INFO_2"/>, <see cref="NativeMethods.SESSION_INFO_10"/> or <see cref="NativeMethods.SESSION_INFO_502"/> instance.</param>
      internal SessionInfo(SessionInfoLevel sessionInfoLevel, object sessionInfo)
      {
         SessionInfoLevel = sessionInfoLevel;


         switch (sessionInfoLevel)
         {
            case SessionInfoLevel.Info502:
               var sesi502 = (NativeMethods.SESSION_INFO_502) sessionInfo;
               HostName = sesi502.sesi502_cname;
               UserName = sesi502.sesi502_username;
               OpenedResources = (int) sesi502.sesi502_num_opens;
               ActiveTime = (int) sesi502.sesi502_time;
               IdleTime = (int) sesi502.sesi502_idle_time;
               //Flags = (int) sesi502.sesi502_user_flags;
               ClientType = sesi502.sesi502_cltype_name;
               TransportType = sesi502.sesi502_transport;
               break;


            case SessionInfoLevel.Info2:
               var sesi2 = (NativeMethods.SESSION_INFO_2) sessionInfo;
               HostName = sesi2.sesi2_cname;
               UserName = sesi2.sesi2_username;
               OpenedResources = (int) sesi2.sesi2_num_opens;
               ActiveTime = (int) sesi2.sesi2_time;
               IdleTime = (int) sesi2.sesi2_idle_time;
               //Flags = (int) sesi2.sesi2_user_flags;
               ClientType = sesi2.sesi2_cltype_name;
               break;


            case SessionInfoLevel.Info1:
               var sesi1 = (NativeMethods.SESSION_INFO_1) sessionInfo;
               HostName = sesi1.sesi1_cname;
               UserName = sesi1.sesi1_username;
               OpenedResources = (int) sesi1.sesi1_num_opens;
               ActiveTime = (int) sesi1.sesi1_time;
               IdleTime = (int) sesi1.sesi1_idle_time;
               //Flags = (int) sesi1.sesi1_user_flags;
               break;


            case SessionInfoLevel.Info10:
               var sesi10 = (NativeMethods.SESSION_INFO_10) sessionInfo;
               HostName = sesi10.sesi10_cname;
               UserName = sesi10.sesi10_username;
               ActiveTime = (int) sesi10.sesi10_time;
               IdleTime = (int) sesi10.sesi10_idle_time;
               break;


            case SessionInfoLevel.Info0:
               var sesi0 = (NativeMethods.SESSION_INFO_0) sessionInfo;
               HostName = sesi0.sesi0_cname;
               break;
         }
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

      /// <summary>The name of the Computer that established the session..</summary>
      public string HostName { get; private set; }


      /// <summary>The name of the User who established the session.</summary>
      public string UserName { get; private set; }


      /// <summary>The number of files, devices, and pipes opened during the session.</summary>
      public int OpenedResources { get; private set; }


      /// <summary>The number of seconds the session has been active.</summary>
      public int ActiveTime { get; private set; }


      /// <summary>The number of seconds the session has been idle.</summary>
      public int IdleTime { get; private set; }


      ///// <summary>
      ///// <para>Specifies a value that describes how the user established the session. This member can be one of the following values:</para>
      ///// <para>SESS_GUEST: The user specified by the <see cref="UserName"/> member established the session using a guest account.</para>
      ///// <para>SESS_NOENCRYPTION: The user specified by the <see cref="UserName"/> member established the session without using password encryption.</para>
      ///// </summary>
      ////public int Flags { get; private set; }

      /// <summary>A value that describes how the User established the session.</summary>
      public bool IsGuest { get; private set; }

      /// <summary>A value that describes how the User established the session.</summary>
      public bool IsEncrypted { get; private set; }


      /// <summary>The type of client that established the session. Sessions from LAN Manager servers running UNIX also will appear as LAN Manager 2.0.</summary>
      public string ClientType { get; private set; }


      /// <summary>The name of the transport that the client is using to communicate with the server.</summary>
      public string TransportType { get; private set; }


      /// <summary>The structure level for the <see cref="SessionInfoLevel"/> instance.</summary>
      public SessionInfoLevel SessionInfoLevel { get; private set; }

      #endregion // Properties
   }
}

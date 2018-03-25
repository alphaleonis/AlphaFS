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
using System.Globalization;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains the identification number of a connection, number of open files, connection time, number of users on the connection, and the type of connection.</summary>
   [Serializable]
   public sealed class OpenConnectionInfo
   {
      #region Private Fields

      private string _netName;

      #endregion // Private Fields


      #region Constructor

      /// <summary>Create an OpenConnectionInfo instance.</summary>
      internal OpenConnectionInfo(string hostName, NativeMethods.CONNECTION_INFO_1 connectionInfo)
      {
         Host = hostName;
         HostName = hostName;
         Id = connectionInfo.coni1_id;
         ShareType = connectionInfo.coni1_type;
         TotalOpenFiles = connectionInfo.coni1_num_opens;
         TotalUsers = connectionInfo.coni1_num_users;
         ConnectedSeconds = connectionInfo.coni1_time;
         ConnectedTime = TimeSpan.FromSeconds(connectionInfo.coni1_time);
         UserName = connectionInfo.coni1_username;
         NetName = connectionInfo.oni1_netname;
      }

      #endregion // Constructor

      
      #region Methods

      /// <summary>Returns the full path to the share.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return Id.ToString(CultureInfo.InvariantCulture);
      }

      #endregion // Methods

      
      #region Properties

      /// <summary>The local or remote Host.</summary>
      [Obsolete("Use HostName")]
      public string Host { get; private set; }

      /// <summary>The host name of this connection information.</summary>
      public string HostName { get; private set; }

      /// <summary>Specifies a connection identification number.</summary>
      public long Id { get; private set; }

      /// <summary>The type of share.</summary>
      public ShareType ShareType { get; private set; }

      /// <summary>Specifies the number of files currently open as a result of the connection.</summary>
      public long TotalOpenFiles { get; private set; }

      /// <summary>Specifies the number of users on the connection.</summary>
      public long TotalUsers { get; private set; }

      /// <summary>Specifies the number of seconds that the connection has been established.</summary>
      [Obsolete("Use ConnectedTime property.")]
      public long ConnectedSeconds { get; private set; }

      /// <summary>Specifies duration that the connection has been established.</summary>
      public TimeSpan ConnectedTime { get; private set; }

      /// <summary>If the server sharing the resource is running with user-level security, the UserName member describes which user made the connection. If the server is running with share-level security, UserName describes which Computer (Computer name) made the connection.</summary>
      public string UserName { get; private set; }
      
      /// <summary>Specifies either the server's shared resource name or the Computer name or IP address of the client. The value of this member depends on which name was specified as the qualifier parameter to the function.</summary>
      public string NetName
      {
         get { return _netName; }

         set { _netName = null != value ? value.Replace(Path.LongPathUncPrefix, string.Empty).Replace(Path.UncPrefix, string.Empty).Trim('[', ']') : null; }
      }

      #endregion // Properties
   }
}

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
using System.Globalization;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains the identification number of a connection, number of open files, connection time, number of users on the connection, and the type of connection.</summary>
   [SerializableAttribute]
   public sealed class OpenConnectionInfo
   {
      #region Constructor

      /// <summary>Create a OpenConnectionInfo instance.</summary>
      internal OpenConnectionInfo(string host, NativeMethods.CONNECTION_INFO_1 connectionInfo)
      {
         Host = host;
         Id = connectionInfo.coni1_id;
         ShareType = connectionInfo.coni1_type;
         TotalOpenFiles = connectionInfo.coni1_num_opens;
         TotalUsers = connectionInfo.coni1_num_users;
         ConnectedSeconds = connectionInfo.coni1_time;
         UserName = connectionInfo.coni1_username;
         NetName = connectionInfo.oni1_netname.Replace(Path.LongPathUncPrefix, string.Empty).Replace(Path.UncPrefix, string.Empty);
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
      public string Host { get; private set; }

      /// <summary>Specifies a connection identification number.</summary>
      public long Id { get; private set; }

      /// <summary>The type of share.</summary>
      public ShareType ShareType { get; private set; }

      /// <summary>Specifies the number of files currently open as a result of the connection.</summary>
      public long TotalOpenFiles { get; private set; }

      /// <summary>Specifies the number of users on the connection.</summary>
      public long TotalUsers { get; private set; }

      /// <summary>Specifies the number of seconds that the connection has been established.</summary>
      public long ConnectedSeconds { get; private set; }

      /// <summary>If the server sharing the resource is running with user-level security, the UserName member describes which user made the connection. If the server is running with share-level security, coni1_username describes which computer (computername) made the connection.</summary>
      public string UserName { get; private set; }

      /// <summary>String that specifies either the share name of the server's shared resource or the computername of the client. The value of this member depends on which name was specified as the qualifier parameter to the NetConnectionEnum function.</summary>
      public string NetName { get; private set; }

      #endregion // Properties
   }
}

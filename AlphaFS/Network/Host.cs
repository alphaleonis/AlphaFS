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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using FileInfo3 = Alphaleonis.Win32.Network.NativeMethods.FileInfo3;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Provides static methods to retrieve network resource information from a local- or remote host.</summary>
   public static class Host
   {
      #region (Struct) ConnectDisconnectArguments
      
      /// <summary>ConnectDisconnectInternal() method has 11 parameters; use a structure for convenience.</summary>
      private struct ConnectDisconnectArguments
      {
         /// <summary>Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</summary>
         public IntPtr WinOwner;

         /// <summary>The name of a local device to be redirected, such as "F:". When <see cref="T:LocalName"/> is <c>null</c> or <c>string.empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</summary>
         public string LocalName;

         /// <summary>A network resource to connect to/disconnect from, for example: \\server or \\server\share</summary>
         public string RemoteName;

         /// <summary>A <see cref="T:NetworkCredential"/> instance. Use either this or the combination of <see cref="T:UserName"/> and <see cref="T:Password"/>.</summary>
         public NetworkCredential Credential;

         /// <summary>The user name for making the connection. If <see cref="T:UserName"/> is <c>null</c>, the function uses the default user name. (The user context for the process provides the default user name)</summary>
         public string UserName;

         /// <summary>The password to be used for making the network connection. If <see cref="T:Password"/> is <c>null</c>, the function uses the current default password associated with the user specified by <see cref="T:UserName"/>.</summary>
         public string Password;

         /// <summary><c>true</c> always pop-ups an authentication dialog box.</summary>
         public bool Prompt;

         /// <summary><c>true</c> successful network resource connections will be saved.</summary>
         public bool UpdateProfile;

         /// <summary>When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</summary>
         public bool SaveCredentials;

         /// <summary><c>true</c> indicates that the operation concerns a drive mapping.</summary>
         public bool IsDeviceMap;

         /// <summary><c>true</c> indicates that the operation needs to disconnect from the network resource, otherwise connect.</summary>
         public bool IsDisconnect;
      }

      #endregion // (Struct) ConnectDisconnectArguments

      #region (Class) DriveConnection

      /// <summary>Creates a temporary connection to a network resource. The function can redirect a local device to a network resource.</summary>
      [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
      public sealed class DriveConnection : IDisposable
      {
         #region Constructors

         /// <summary>Creates a temporary connection to a network resource. The function can redirect a local device to a network resource.</summary>
         /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
         public DriveConnection(string remoteName)
         {
            LocalName = ConnectDisconnectInternal(new ConnectDisconnectArguments
            {
               RemoteName = remoteName,
               IsDeviceMap = true
            });
         }

         /// <summary>Creates a temporary connection to a network resource. The function can redirect a local device to a network resource.</summary>
         /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
         /// <param name="userName">The user name for making the connection. If <paramref name="userName"/> is <c>null</c>, the function uses the default user name. (The user context for the process provides the default user name)</param>
         /// <param name="password">The password to be used for making the network connection. If <paramref name="password"/> is <c>null</c>, the function uses the current default password associated with the user specified by <paramref name="userName"/>.</param>
         /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
         public DriveConnection(string remoteName, string userName, string password, bool prompt)
         {
            LocalName = ConnectDisconnectInternal(new ConnectDisconnectArguments
            {
               RemoteName = remoteName,
               UserName = userName,
               Password = password,
               Prompt = prompt,
               IsDeviceMap = true
            });
         }

         /// <summary>Creates a temporary connection to a network resource. The function can redirect a local device to a network resource.</summary>
         /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
         /// <param name="credentials">An instance of <see cref="T:NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
         /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
         public DriveConnection(string remoteName, NetworkCredential credentials, bool prompt)
         {
            LocalName = ConnectDisconnectInternal(new ConnectDisconnectArguments
            {
               RemoteName = remoteName,
               Credential = credentials,
               Prompt = prompt,
               IsDeviceMap = true
            });
         }

         #endregion // Constructors

         #region Methods

         #region Dispose

         /// <summary>Releases all resources used by the <see cref="T:DriveConnection"/> class.</summary>
         public void Dispose()
         {
            if (!Utils.IsNullOrWhiteSpace(LocalName))
            {
               ConnectDisconnectInternal(new ConnectDisconnectArguments
               {
                  LocalName = LocalName,
                  Prompt = true,  // Use value of prompt variable for force value.
                  IsDeviceMap = true,
                  IsDisconnect = true
               });

               LocalName = null;
            }
         }

         #endregion // Dispose

         #region ToString

         /// <summary>Returns the last available drive letter used for this connection.</summary>
         public override string ToString()
         {
            return LocalName;
         }

         #endregion // ToString

         #endregion // Methods

         #region Properties

         #region LocalName

         /// <summary>The last available drive letter used for this connection.</summary>
         public string LocalName { get; private set; }

         #endregion // LocalName

         #endregion // Properties
      }

      #endregion // (Class) DriveConnection


      #region ConnectDrive

      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <param name="localName">The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <returns>If <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, returns the last available drive letter, <c>null</c> otherwise.</returns>
      [SecurityCritical]
      public static string ConnectDrive(string localName, string remoteName)
      {
         return ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            LocalName = localName,
            RemoteName = remoteName,
            IsDeviceMap = true
         });
      }

      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <param name="localName">The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="userName">The user name for making the connection. If <paramref name="userName"/> is <c>null</c>, the function uses the default user name. (The user context for the process provides the default user name)</param>
      /// <param name="password">The password to be used for making the network connection. If <paramref name="password"/> is <c>null</c>, the function uses the current default password associated with the user specified by <paramref name="userName"/>.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <returns>If <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, returns the last available drive letter, null otherwise.</returns>
      [SecurityCritical]
      public static string ConnectDrive(string localName, string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            LocalName = localName,
            RemoteName = remoteName,
            UserName = userName,
            Password =  password,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials,
            IsDeviceMap = true
         });
      }
      
      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <param name="localName">The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="credentials">An instance of <see cref="T:NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <returns>If <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, returns the last available drive letter, null otherwise.</returns>
      [SecurityCritical]
      public static string ConnectDrive(string localName, string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            LocalName = localName,
            RemoteName = remoteName,
            Credential = credentials,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials,
            IsDeviceMap = true
         });
      }

      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="localName">The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="userName">The user name for making the connection. If <paramref name="userName"/> is <c>null</c>, the function uses the default user name. (The user context for the process provides the default user name)</param>
      /// <param name="password">The password to be used for making the network connection. If <paramref name="password"/> is <c>null</c>, the function uses the current default password associated with the user specified by <paramref name="userName"/>.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <returns>If <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, returns the last available drive letter, null otherwise.</returns>
      [SecurityCritical]
      public static string ConnectDrive(IntPtr winOwner, string localName, string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            WinOwner = winOwner,
            LocalName = localName,
            RemoteName = remoteName,
            UserName = userName,
            Password = password,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials,
            IsDeviceMap = true
         });
      }

      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="localName">The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="credentials">An instance of <see cref="T:NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <returns>If <paramref name="localName"/> is <c>null</c> or <c>string.empty</c>, returns the last available drive letter, null otherwise.</returns>
      [SecurityCritical]
      public static string ConnectDrive(IntPtr winOwner, string localName, string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            WinOwner = winOwner,
            LocalName = localName,
            RemoteName = remoteName,
            Credential = credentials,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials,
            IsDeviceMap = true
         });
      }

      #endregion // ConnectDrive
      
      #region ConnectTo

      /// <summary>Creates a connection to a network resource.</summary>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void ConnectTo(string remoteName)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments {RemoteName = remoteName});
      }

      /// <summary>Creates a connection to a network resource.</summary>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share</param>
      /// <param name="userName">The user name for making the connection. If <paramref name="userName"/> is <c>null</c>, the function uses the default user name. (The user context for the process provides the default user name)</param>
      /// <param name="password">The password to be used for making the network connection. If <paramref name="password"/> is <c>null</c>, the function uses the current default password associated with the user specified by <paramref name="userName"/>.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void ConnectTo(string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            UserName = userName,
            Password = password,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials
         });
      }

      /// <summary>Creates a connection to a network resource.</summary>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share</param>
      /// <param name="credentials">An instance of <see cref="T:NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void ConnectTo(string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            Credential = credentials,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials
         });
      }

      /// <summary>Creates a connection to a network resource.</summary>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share</param>
      /// <param name="userName">The user name for making the connection. If <paramref name="userName"/> is <c>null</c>, the function uses the default user name. (The user context for the process provides the default user name)</param>
      /// <param name="password">The password to be used for making the network connection. If <paramref name="password"/> is <c>null</c>, the function uses the current default password associated with the user specified by <paramref name="userName"/>.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void ConnectTo(IntPtr winOwner, string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            WinOwner = winOwner,
            RemoteName = remoteName,
            UserName = userName,
            Password = password,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials
         });
      }

      /// <summary>Creates a connection to a network resource.</summary>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share</param>
      /// <param name="credentials">An instance of <see cref="T:NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
      /// <param name="prompt"><c>true</c> always pop-ups an authentication dialog box.</param>
      /// <param name="updateProfile"><c>true</c> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void ConnectTo(IntPtr winOwner, string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            WinOwner = winOwner,
            RemoteName = remoteName,
            Credential = credentials,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials
         });
      }

      #endregion // ConnectTo

      #region DisconnectDrive

      /// <summary>Cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <param name="localName">The name of a local device to be disconnected, such as "F:".</param>
      [SecurityCritical]
      public static void DisconnectDrive(string localName)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            LocalName = localName,
            IsDeviceMap = true,
            IsDisconnect = true
         });
      }

      /// <summary>Cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <param name="localName">The name of a local device to be disconnected, such as "F:".</param>
      /// <param name="force">Specifies whether the disconnection should occur if there are open files or jobs on the connection. If this parameter is <c>false</c>, the function fails if there are open files or jobs.</param>
      /// <param name="updateProfile"><c>true</c> successful removal of network resource connections will be saved.</param>
      [SecurityCritical]
      public static void DisconnectDrive(string localName, bool force, bool updateProfile)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            LocalName = localName,
            Prompt = force,
            UpdateProfile = updateProfile,
            IsDeviceMap = true,
            IsDisconnect = true
         });
      }

      #endregion // DisconnectDrive

      #region DisconnectFrom

      /// <summary>Cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <param name="remoteName">A network resource to disconnect from, for example: \\server or \\server\share</param>
      [SecurityCritical]
      public static void DisconnectFrom(string remoteName)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            IsDisconnect = true
         });
      }

      /// <summary>Cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <param name="remoteName">A network resource to disconnect from, for example: \\server or \\server\share</param>
      /// <param name="force">Specifies whether the disconnection should occur if there are open files or jobs on the connection. If this parameter is <c>false</c>, the function fails if there are open files or jobs.</param>
      /// <param name="updateProfile"><c>true</c> successful removal of network resource connections will be saved.</param>
      [SecurityCritical]
      public static void DisconnectFrom(string remoteName, bool force, bool updateProfile)
      {
         ConnectDisconnectInternal(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            Prompt = force,
            UpdateProfile = updateProfile,
            IsDisconnect = true
         });
      }

      #endregion // DisconnectFrom


      #region EnumerateDfsLinks

      /// <summary>Enumerates the DFS Links from a DFS namespace.</summary>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <returns>Returns <see cref="T:IEnumerable{DfsInfo}"/> of DFS namespaces.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static IEnumerable<DfsInfo> EnumerateDfsLinks(string dfsName)
      {
         if (Utils.IsNullOrWhiteSpace(dfsName))
            throw new ArgumentNullException("dfsName");

         FunctionData fd = new FunctionData();
         return EnumerateNetworkObjectInternal(fd, (NativeMethods.DfsInfo4 structure, SafeNetApiBuffer safeBuffer) =>
            new DfsInfo(structure),
            (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle1) =>
            {
               totalEntries = 0;
               return NativeMethods.NetDfsEnum(dfsName, 4, prefMaxLen, out safeBuffer, out entriesRead, out resumeHandle1);
            }, true);
      }

      #endregion // EnumerateDfsLinks

      #region EnumerateDfsRoot

      /// <summary>Enumerates the DFS namespaces from the local host.</summary>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> of DFS Root namespaces from the local host.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDfsRoot()
      {
         return EnumerateDfsRootInternal(null, false);
      }

      /// <summary>Enumerates the DFS namespaces from a remote host.</summary>
      /// <param name="host">The DNS or NetBIOS name of a remote host.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> of DFS Root namespaces from a remote host.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDfsRoot(string host, bool continueOnException)
      {
         return EnumerateDfsRootInternal(host, continueOnException);
      }

      #endregion // EnumerateDfsRoot

      #region EnumerateDomainDfsRoot

      /// <summary>Enumerates the DFS namespaces from the domain.</summary>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> of DFS Root namespaces from the domain.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDomainDfsRoot()
      {
         return EnumerateDomainDfsRootInternal(null, false);
      }

      /// <summary>Enumerates the DFS namespaces from a domain.</summary>
      /// <param name="domain">A domain name.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> of DFS Root namespaces from a domain.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDomainDfsRoot(string domain, bool continueOnException)
      {
         return EnumerateDomainDfsRootInternal(domain, continueOnException);
      }

      #endregion // EnumerateDomainDfsRoot

      #region EnumerateDrives

      /// <summary>Enumerates drives from the local host.</summary>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> drives from the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDrives()
      {
         return EnumerateDrivesInternal(null, false);
      }

      /// <summary>Enumerates local drives from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> drives from the specified host.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDrives(string host, bool continueOnException)
      {
         return EnumerateDrivesInternal(host, continueOnException);
      }

      #endregion // EnumerateDrives

      #region EnumerateOpenConnections

      /// <summary>Enumerates open connections from the local host.</summary>
      /// <returns>Returns <see cref="T:OpenConnectionInfo"/> connection information from the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<OpenConnectionInfo> EnumerateOpenConnections()
      {
         return EnumerateOpenConnectionsInternal(null, null, false);
      }

      /// <summary>Enumerates open connections from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:OpenConnectionInfo"/> connection information from the specified <paramref name="host"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<OpenConnectionInfo> EnumerateOpenConnections(string host, string share, bool continueOnException)
      {
         return EnumerateOpenConnectionsInternal(host, share, continueOnException);
      }

      #endregion // EnumerateOpenConnections

      #region EnumerateOpenResources

      /// <summary>Enumerates open resources from the local host.</summary>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> open resources from the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<OpenResourceInfo> EnumerateOpenResources()
      {
         return EnumerateOpenResourcesInternal(null, null, null, false);
      }

      /// <summary>Enumerates open resources from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      /// <param name="basePath">This parameter may be <c>null</c>. Enumerates only resources that have the value of the basepath parameter as a prefix. (A prefix is the portion of a path that comes before a backslash.)</param>
      /// <param name="typeName">This parameter may be <c>null</c>. The name of the user or the name of the connection; If <paramref name="typeName"/> does not begin with two backslashes ("\\") it indicates the name of the user. If <paramref name="typeName"/> begins with two backslashes ("\\") it indicates the name of the connection,</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> open resources from the specified <paramref name="host"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<OpenResourceInfo> EnumerateOpenResources(string host, string basePath, string typeName, bool continueOnException)
      {
         return EnumerateOpenResourcesInternal(host, basePath, typeName, continueOnException);
      }

      #endregion // EnumerateOpenResources

      #region EnumerateShares

      /// <summary>Enumerates Server Message Block (SMB) shares from the local host.</summary>
      /// <returns>Returns <see cref="T:IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares()
      {
         return EnumerateSharesInternal(null, false);
      }

      /// <summary>Enumerates Server Message Block (SMB) shares from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(string host, bool continueOnException)
      {
         return EnumerateSharesInternal(host, continueOnException);
      }
      
      #endregion // EnumerateShares

      #region GetDfsClientInformation

      /// <summary>Gets information about a DFS root or link from the cache maintained by the DFS client.</summary>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <returns>Returns a <see cref="T:DfsInfo"/> instance.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsClientInformation(string dfsName)
      {
         return GetDfsInformationInternal(true, dfsName, null, null);
      }

      /// <summary>Gets information about a DFS root or link from the cache maintained by the DFS client.</summary>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <param name="serverName">The name of the DFS root target or link target server.</param>
      /// <param name="shareName">The name of the share corresponding to the DFS root target or link target.</param>
      /// <returns>Returns a <see cref="T:DfsInfo"/> instance.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsClientInformation(string dfsName, string serverName, string shareName)
      {
         return GetDfsInformationInternal(true, dfsName, serverName, shareName);
      }

      #endregion // GetDfsClientInformation

      #region GetDfsInformation

      /// <summary>Gets information about a specified DFS root or link in a DFS namespace.</summary>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <returns>Returns a <see cref="T:DfsInfo"/> instance.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsInformation(string dfsName)
      {
         return GetDfsInformationInternal(false, dfsName, null, null);
      }

      #endregion // GetDfsInformation

      #region GetHostShareFromPath

      /// <summary>Gets the host and Server Message Block (SMB) share name for the given <paramref name="uncPath"/>.</summary>
      /// <param name="uncPath">The share in the format: \\host\share</param>
      /// <returns>string[0] = host, string[1] = share;</returns>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public static string[] GetHostShareFromPath(string uncPath)
      {
         if (Utils.IsNullOrWhiteSpace(uncPath))
            return null;

         // Get Host and Share.
         uncPath = uncPath.Replace(Path.LongPathUncPrefix, string.Empty);
         uncPath = uncPath.Replace(Path.UncPrefix, string.Empty);

         return uncPath.Split(Path.DirectorySeparatorChar);
      }

      #endregion // GetHostShareFromPath

      #region GetShareInformation

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <param name="uncPath">The share in the format: \\host\share</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>A <see cref="T:ShareInfo"/> class, or <c>null</c> on failure or when not available.</returns>
      [SecurityCritical]
      public static ShareInfo GetShareInformation(string uncPath, bool continueOnException)
      {
         string[] unc = GetHostShareFromPath(uncPath);
         return GetShareInformationInternal(503, unc[0], unc[1], continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <param name="structureLevel">The structure level for the ShareInfo instance. Possible structure levels: <see cref="T:NativeMethods.ShareInfo503">503</see>, <see cref="T:NativeMethods.ShareInfo2">2</see>, <see cref="T:NativeMethods.ShareInfo1">1</see> and <see cref="T:NativeMethods.ShareInfo1005">1005</see>.</param>
      /// <param name="uncPath">The share in the format: \\host\share</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>A <see cref="T:ShareInfo"/> class, or <c>null</c> on failure or when not available.</returns>
      [SecurityCritical]
      public static ShareInfo GetShareInformation(int structureLevel, string uncPath, bool continueOnException)
      {
         string[] unc = GetHostShareFromPath(uncPath);
         return GetShareInformationInternal(structureLevel, unc[0], unc[1], continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>A <see cref="T:ShareInfo"/> class, or <c>null</c> on failure or when not available.</returns>
      [SecurityCritical]
      public static ShareInfo GetShareInformation(string host, string share, bool continueOnException)
      {
         return GetShareInformationInternal(503, host, share, continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <param name="structureLevel">Possible structure levels: <see cref="T:NativeMethods.ShareInfo503">503</see>, <see cref="T:NativeMethods.ShareInfo2">2</see>,  <see cref="T:NativeMethods.ShareInfo1">1</see> and <see cref="T:NativeMethods.ShareInfo1005">1005</see>.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>A <see cref="T:ShareInfo"/> class, or <see cref="T:null"/> on failure or when not available.</returns>
      [SecurityCritical]
      public static ShareInfo GetShareInformation(int structureLevel, string host, string share, bool continueOnException)
      {
         return GetShareInformationInternal(structureLevel, host, share, continueOnException);
      }

      #endregion // GetShareInformation

      #region GetUncName

      /// <summary>Return the host name in UNC format, for example: \\hostname</summary>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetUncName()
      {
         return string.Format(CultureInfo.CurrentCulture, "{0}{1}", Path.UncPrefix, Environment.MachineName);
      }

      /// <summary>Return the host name in UNC format, for example: \\hostname</summary>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")][
      SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetUncName(string computerName)
      {
         return Utils.IsNullOrWhiteSpace(computerName)
            ? GetUncName()
            : (computerName.StartsWith(Path.UncPrefix, StringComparison.OrdinalIgnoreCase)
               ? computerName.Trim()
               : Path.UncPrefix + computerName.Trim());
      }

      #endregion // GetUncName
      

      #region Unified Internals

      #region ConnectDisconnectInternal

      /// <summary>Unified method ConnectDisconnectInternal() to connect to/disconnect from a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>If <see cref="T:ConnectDisconnectArguments.LocalName"/> is <c>null</c> or <c>string.empty</c>, returns the last available drive letter, null otherwise.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
      [SecurityCritical]
      private static string ConnectDisconnectInternal(ConnectDisconnectArguments arguments)
      {
         uint lastError;

         // Always remove backslash for local device.
         if (!Utils.IsNullOrWhiteSpace(arguments.LocalName))
            arguments.LocalName = Path.RemoveDirectorySeparator(arguments.LocalName, false).ToUpperInvariant();


         #region Disconnect

         if (arguments.IsDisconnect)
         {
            bool force = arguments.Prompt; // Use value of prompt variable for force value.
            string target = arguments.IsDeviceMap ? arguments.LocalName : arguments.RemoteName;

            if (Utils.IsNullOrWhiteSpace(target))
               throw new ArgumentNullException(arguments.IsDeviceMap ? "localName" : "remoteName");

            lastError = NativeMethods.WNetCancelConnection(target, arguments.UpdateProfile ? NativeMethods.Connect.UpdateProfile : NativeMethods.Connect.None, force);

            if (lastError != Win32Errors.NO_ERROR)
               NativeError.ThrowException(lastError, arguments.IsDeviceMap ? arguments.LocalName : arguments.RemoteName);

            return null;
         }

         #endregion // Disconnect

         #region Connect

         // arguments.LocalName is allowed to be null or empty.
         //if (Utils.IsNullOrWhiteSpace(arguments.LocalName) && !arguments.IsDeviceMap)
         //   throw new ArgumentNullException("localName");

         if (Utils.IsNullOrWhiteSpace(arguments.RemoteName) && !arguments.IsDeviceMap)
            throw new ArgumentNullException("remoteName");


         // When supplied, use data from NetworkCredential instance.
         if (arguments.Credential != null)
         {
            arguments.UserName = Utils.IsNullOrWhiteSpace(arguments.Credential.Domain)
               ? arguments.Credential.UserName
               : string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", arguments.Credential.Domain, arguments.Credential.UserName);

            arguments.Password = arguments.Credential.Password;
         }


         // Assemble Connect options.
         NativeMethods.Connect connect = NativeMethods.Connect.Interactive; // The operating system may interact.

         if (arguments.IsDeviceMap)
            connect = connect | NativeMethods.Connect.Redirect;

         if (arguments.Prompt)
            connect = connect | NativeMethods.Connect.Prompt;

         if (arguments.UpdateProfile)
            connect = connect | NativeMethods.Connect.UpdateProfile;

         if (arguments.SaveCredentials)
            connect = connect | NativeMethods.Connect.CmdSaveCred;


         // Initialize structure.
         NativeMethods.NetResource resource = new NativeMethods.NetResource
         {
            LocalName = arguments.LocalName,
            RemoteName = arguments.RemoteName,
            Type = NativeMethods.ResourceType.Disk
         };

         // Three characters for: "X:\0" (Drive X: with null terminator)
         uint bufferSize = 3;
         StringBuilder buffer;

         do
         {
            buffer = new StringBuilder((int) bufferSize);

            uint result;
            lastError = NativeMethods.WNetUseConnection(arguments.WinOwner, ref resource, arguments.Password, arguments.UserName, connect, buffer, out bufferSize, out result);
            
            switch (lastError)
            {
               case Win32Errors.NO_ERROR:
                  break;

               case Win32Errors.ERROR_MORE_DATA:
                  // MSDN, lpBufferSize: If the call fails because the buffer is not large enough,
                  // the function returns the required buffer size in this location.
                  //
                  // Windows 8 x64: bufferSize remains unchanged.

                  bufferSize = bufferSize*2;
                  break;

                  // Non-existent host.
               case Win32Errors.ERROR_BAD_NETPATH:
                  lastError = Win32Errors.ERROR_BAD_NET_NAME; // Throw more appropriate error.
                  break;
            }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);


         if (lastError != Win32Errors.NO_ERROR)
            NativeError.ThrowException(lastError);

         return arguments.IsDeviceMap ? buffer.ToString() : null;

         #endregion // Connect
      }

      #endregion // ConnectDisconnectInternal

      #region EnumerateDfsRootInternal

      /// <summary>Unified method EnumerateDfsRootInternal() to enumerate the DFS namespaces from a remote host.</summary>
      /// <param name="host">The DNS or NetBIOS name of a remote host.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> of DFS Root namespaces from a remote host.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      private static IEnumerable<string> EnumerateDfsRootInternal(string host, bool continueOnException)
      {
         // The UNC prefix: \\ is not required and always removed.
         host = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, true, false, false, false).Replace(Path.UncPrefix, string.Empty);

         FunctionData fd = new FunctionData();
         return EnumerateNetworkObjectInternal(fd, (NativeMethods.DfsInfo300 structure, SafeNetApiBuffer safeBuffer) =>
            new DfsInfo { EntryPath = structure.DfsName },
            (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               totalEntries = 0;
               return NativeMethods.NetDfsEnum(host, 300, prefMaxLen, out safeBuffer, out entriesRead, out resumeHandle);
            }, continueOnException).Select(dfs => dfs.EntryPath);
      }

      #endregion // EnumerateDfsRootInternal

      #region EnumerateDomainDfsRootInternal

      /// <summary>Unified method EnumerateDomainDfsRootInternal() to enumerate the DFS namespaces from a domain.</summary>
      /// <param name="domain">A domain name.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> of DFS Root namespaces from a domain.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      private static IEnumerable<string> EnumerateDomainDfsRootInternal(string domain, bool continueOnException)
      {
         // The UNC prefix: \\ is not required and always removed.
         domain = Utils.IsNullOrWhiteSpace(domain) ? NativeMethods.ComputerDomain : Path.GetRegularPathInternal(domain, true, false, false, false).Replace(Path.UncPrefix, string.Empty);

         FunctionData fd = new FunctionData();
         return EnumerateNetworkObjectInternal(fd, (NativeMethods.DfsInfo200 structure, SafeNetApiBuffer safeBuffer) =>
            new DfsInfo { EntryPath = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}{3}", Path.UncPrefix, NativeMethods.ComputerDomain, Path.DirectorySeparatorChar, structure.FtDfsName) },
            (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               totalEntries = 0;
               return NativeMethods.NetDfsEnum(domain, 200, prefMaxLen, out safeBuffer, out entriesRead, out resumeHandle);
            }, continueOnException).Select(dfs => dfs.EntryPath);
      }

      #endregion // EnumerateDomainDfsRootInternal
      
      #region EnumerateDrivesInternal

      /// <summary>Unified method EnumerateDrivesInternal() to enumerate local drives from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> drives from the specified host.</returns>
      [SecurityCritical]
      private static IEnumerable<string> EnumerateDrivesInternal(string host, bool continueOnException)
      {
         // The UNC prefix: \\ is not required and always removed.
         host = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, true, false, false, false).Replace(Path.UncPrefix, string.Empty);

         FunctionData fd = new FunctionData {EnumType = 1};
         return EnumerateNetworkObjectInternal(fd, (string structure, SafeNetApiBuffer safeBuffer) =>
            structure, (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resume) =>
               NativeMethods.NetServerDiskEnum(host, 0, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resume),
            continueOnException);
      }

      #endregion // EnumerateDrivesInternal

      #region EnumerateOpenConnectionsInternal

      /// <summary>Unified method EnumerateOpenConnectionsInternal() to enumerate open connections from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:OpenConnectionInfo"/> connection information from the specified <paramref name="host"/>.</returns>
      [SecurityCritical]
      private static IEnumerable<OpenConnectionInfo> EnumerateOpenConnectionsInternal(string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            throw new ArgumentNullException("share");

         // The UNC prefix: \\ is not required and always removed.
         host = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, true, false, false, false).Replace(Path.UncPrefix, "");

         FunctionData fd = new FunctionData {ExtraData1 = share};
         return EnumerateNetworkObjectInternal(fd, (NativeMethods.ConnectionInfo1 structure, SafeNetApiBuffer safeBuffer) =>
               new OpenConnectionInfo(host, structure), (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
               NativeMethods.NetConnectionEnum(host, functionData.ExtraData1, 1, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle),
            continueOnException);
      }

      #endregion // EnumerateOpenConnectionsInternal

      #region EnumerateOpenResourcesInternal

      /// <summary>>Unified method EnumerateOpenResourcesInternal() to enumerate open resources from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <c>null</c> refers to the local host.</param>
      /// <param name="basePath">This parameter may be <c>null</c>. Enumerates only resources that have the value of the basepath parameter as a prefix. (A prefix is the portion of a path that comes before a backslash.)</param>
      /// <param name="typeName">This parameter may be <c>null</c>. The name of the user or the name of the connection; If <paramref name="typeName"/> does not begin with two backslashes ("\\") it indicates the name of the user. If <paramref name="typeName"/> begins with two backslashes ("\\") it indicates the name of the connection,</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{String}"/> open resources from the specified <paramref name="host"/>.</returns>
      [SecurityCritical]
      private static IEnumerable<OpenResourceInfo> EnumerateOpenResourcesInternal(string host, string basePath, string typeName, bool continueOnException)
      {
         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         host = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, true, false, false, false).Replace(Path.UncPrefix, string.Empty);

         basePath = Utils.IsNullOrWhiteSpace(basePath)
            ? null
            : Path.GetRegularPathInternal(basePath, true, false, false, false);

         typeName = Utils.IsNullOrWhiteSpace(typeName) ? null : typeName;

         FunctionData fd = new FunctionData { ExtraData1 = basePath, ExtraData2 = typeName };
         return EnumerateNetworkObjectInternal(fd, (FileInfo3 structure, SafeNetApiBuffer safeBuffer) =>
            new OpenResourceInfo(host, structure), (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
               NativeMethods.NetFileEnum(host, fd.ExtraData1, fd.ExtraData2, 3, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle),
            continueOnException);
      }

      #endregion // EnumerateOpenResourcesInternal

      #region EnumerateNetworkObjectInternal

      private delegate TClass CreateNetworkObjectTDelegate<out TClass, in TNative>(TNative structure, SafeNetApiBuffer safeBuffer);
      private delegate uint EnumerateNetworkObjectDelegate(FunctionData functionData, out SafeNetApiBuffer netApiBuffer, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      /// <summary>Structure is used to pass additional data to the Win32 function.</summary>
      private struct FunctionData
      {
         public int EnumType;
         public string ExtraData1;
         public string ExtraData2;
      }

      [SecurityCritical]
      private static IEnumerable<TStruct> EnumerateNetworkObjectInternal<TStruct, TNative>(FunctionData functionData, CreateNetworkObjectTDelegate<TStruct, TNative> createTStruct, EnumerateNetworkObjectDelegate enumerateNetworkObject, bool continueOnException)
      {
         uint lastError;
         Type objectType;
         int objectSize;
         bool isString;

         switch (functionData.EnumType)
         {
            // Logical Drives
            case 1:
               objectType = typeof(IntPtr);
               isString = true;
               objectSize = Marshal.SizeOf(objectType) + 2;
               break;

            default:
               objectType = typeof(TNative);
               isString = objectType == typeof(string);
               objectSize = isString ? 0 : Marshal.SizeOf(objectType);
               break;
         }

         do
         {
            uint entriesRead;
            uint totalEntries;
            uint resumeHandle;
            SafeNetApiBuffer safeBuffer;

            lastError = enumerateNetworkObject(functionData, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle);

            using (safeBuffer)
               switch (lastError)
               {
                  case Win32Errors.NERR_Success:
                  case Win32Errors.ERROR_MORE_DATA:
                     if (entriesRead > 0)
                     {
                        // CA2001:AvoidCallingProblematicMethods

                        IntPtr buffer = IntPtr.Zero;
                        bool successRef = false;
                        safeBuffer.DangerousAddRef(ref successRef);

                        // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                        if (successRef)
                           buffer = safeBuffer.DangerousGetHandle();

                        safeBuffer.DangerousRelease();

                        if (buffer == IntPtr.Zero)
                           NativeError.ThrowException(Resources.HandleDangerousRef);

                        // CA2001:AvoidCallingProblematicMethods


                        for (long i = 0, itemOffset = buffer.ToInt64(); i < entriesRead; i++, itemOffset += objectSize)
                           yield return (TStruct) (isString
                              ? Marshal.PtrToStringUni(new IntPtr(itemOffset))
                              : (object) createTStruct((TNative) Marshal.PtrToStructure(new IntPtr(itemOffset), objectType), safeBuffer));
                     }
                     break;

                  // Non-existent host.
                  case Win32Errors.ERROR_BAD_NETPATH:
                     lastError = Win32Errors.ERROR_BAD_NET_NAME; // Throw more appropriate error.
                     break;

                  // Observed when ShareInfo503 is requested, but not supported/possible.
                  case Win32Errors.RPC_X_BAD_STUB_DATA:
                     yield break;
               }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);

         if (lastError != Win32Errors.NO_ERROR && !continueOnException)
            NativeError.ThrowException(lastError);
      }

      #endregion // EnumerateNetworkObjectInternal

      #region EnumerateSharesInternal

      /// <summary>Unified method EnumerateSharesInternal() to enumerate Server Message Block (SMB) shares from the specified host.</summary>
      /// <param name="host">The DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>Returns <see cref="T:IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      [SecurityCritical]
      private static IEnumerable<ShareInfo> EnumerateSharesInternal(string host, bool continueOnException)
      {
         // The UNC prefix: \\ is not required and always removed.
         host = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, true, false, false, false).Replace(Path.UncPrefix, string.Empty);

         FunctionData fd = new FunctionData();
         bool hasItems = false;

         foreach (ShareInfo si in EnumerateNetworkObjectInternal(fd, (NativeMethods.ShareInfo503 structure, SafeNetApiBuffer safeBuffer) =>
               new ShareInfo(host, 503, structure), (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
               NativeMethods.NetShareEnum(host, 503, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle),
            continueOnException))
         {
            yield return si;
            hasItems = true;
         }

         // ShareInfo503 is requested, but not supported/possible.
         // Try again with ShareInfo2 structure.
         if (!hasItems)
            foreach (ShareInfo si in EnumerateNetworkObjectInternal(fd, (NativeMethods.ShareInfo2 structure, SafeNetApiBuffer safeBuffer) =>
                        new ShareInfo(host, 2, structure), (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
                        NativeMethods.NetShareEnum(host, 2, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle),
                     continueOnException))
            {
               yield return si;
               hasItems = true;
            }

         // ShareInfo2 is requested, but not supported/possible.
         // Try again with ShareInfo1 structure.
         if (!hasItems)
            foreach (ShareInfo si in EnumerateNetworkObjectInternal(fd, (NativeMethods.ShareInfo1 structure, SafeNetApiBuffer safeBuffer) =>
                        new ShareInfo(host, 1, structure), (FunctionData functionData, out SafeNetApiBuffer safeBuffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
                        NativeMethods.NetShareEnum(host, 1, out safeBuffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle),
                     continueOnException))
            {
               yield return si;
            }
      }
      
      #endregion // EnumerateSharesInternal

      #region GetDfsInformationInternal

      /// <summary>Retrieves information about a specified DFS root or link in a DFS namespace.</summary>
      /// <param name="getFromClient">
      /// <c>true</c> retrieves information about a Distributed File System (DFS) root or link from the cache maintained by the DFS client.
      /// When <c>false</c> retrieves information about a specified Distributed File System (DFS) root or link in a DFS namespace.
      ///</param>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <param name="serverName">The name of the DFS root target or link target server. If <paramref name="getFromClient"/> is <c>false</c>, this parameter is always <c>null</c>.</param>
      /// <param name="shareName">The name of the share corresponding to the DFS root target or link target. If <paramref name="getFromClient"/> is <c>false</c>, this parameter is always <c>null</c>.</param>
      /// <returns>Returns <see cref="T:IEnumerable{DfsInfo}"/></returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      private static DfsInfo GetDfsInformationInternal(bool getFromClient, string dfsName, string serverName, string shareName)
      {
         if (Utils.IsNullOrWhiteSpace(dfsName))
            throw new ArgumentNullException("dfsName");

         serverName = Utils.IsNullOrWhiteSpace(serverName) ? null : serverName;
         shareName = Utils.IsNullOrWhiteSpace(shareName) ? null : shareName;

         SafeNetApiBuffer safeBuffer;
         uint lastError = getFromClient
            ? NativeMethods.NetDfsGetClientInfo(dfsName, serverName, shareName, 4, out safeBuffer) // Max level: 4 (DFS_INFO_4)
            : NativeMethods.NetDfsGetInfo(dfsName, null, null, 4, out safeBuffer);                 // MSDN: These parameters are currently ignored and should be null.

         using (safeBuffer)
         {
            if (Filesystem.NativeMethods.IsValidHandle(safeBuffer, (int) lastError))
            {
               // CA2001:AvoidCallingProblematicMethods

               IntPtr buffer = IntPtr.Zero;
               bool successRef = false;
               safeBuffer.DangerousAddRef(ref successRef);

               // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
               if (successRef)
                  buffer = safeBuffer.DangerousGetHandle();

               safeBuffer.DangerousRelease();

               if (buffer == IntPtr.Zero)
                  NativeError.ThrowException(Resources.HandleDangerousRef);

               // CA2001:AvoidCallingProblematicMethods


               return new DfsInfo(Filesystem.NativeMethods.GetStructure<NativeMethods.DfsInfo4>(0, buffer));
            }

            if (lastError != Win32Errors.NERR_Success)
               NativeError.ThrowException(lastError);
         }

         return null;
      }

      #endregion // GetDfsInformationInternal

      #region GetRemoteNameInfoInternal

      /// <summary>This method uses <see cref="T:NativeMethods.RemoteNameInfo"/> level to retieve full REMOTE_NAME_INFO structure.</summary>
      /// <param name="path">The local path with drive name.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>A <see cref="T:NativeMethods.RemoteNameInfo"/> structure.</returns>
      /// <remarks>AlphaFS regards network drives created using SUBST.EXE as invalid: http://alphafs.codeplex.com/discussions/316583</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SecurityCritical]
      internal static NativeMethods.RemoteNameInfo GetRemoteNameInfoInternal(string path, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         path = Path.GetRegularPathInternal(path, true, false, false, false); 

         // If path already is a network share path, we fill the RemoteNameInfo structure ourselves.
         if (Path.IsUncPath(path, false))
            return new NativeMethods.RemoteNameInfo
            {
               UniversalName = Path.AddDirectorySeparator(path, false),
               ConnectionName = Path.RemoveDirectorySeparator(path, false),
               RemainingPath = Path.DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)
            };


         // Start with a large buffer to prevent multiple calls.
         uint bufferSize = 1024;
         uint lastError;

         do
         {
            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle((int)bufferSize))
            {
               // Structure: UNIVERSAL_NAME_INFO_LEVEL = 1 (not used in AlphaFS).
               // Structure: REMOTE_NAME_INFO_LEVEL    = 2
               lastError = NativeMethods.WNetGetUniversalName(path, 2, safeBuffer, out bufferSize);

               switch (lastError)
               {
                  case Win32Errors.NO_ERROR:

                     // CA2001:AvoidCallingProblematicMethods

                     IntPtr buffer = IntPtr.Zero;
                     bool successRef = false;
                     safeBuffer.DangerousAddRef(ref successRef);

                     // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                     if (successRef)
                        buffer = safeBuffer.DangerousGetHandle();

                     safeBuffer.DangerousRelease();

                     if (buffer == IntPtr.Zero)
                        NativeError.ThrowException(Resources.HandleDangerousRef);

                     // CA2001:AvoidCallingProblematicMethods


                     return Filesystem.NativeMethods.GetStructure<NativeMethods.RemoteNameInfo>(0, buffer);

                  case Win32Errors.ERROR_MORE_DATA:
                     //bufferSize = Received the required buffer size.
                     break;
               }
            }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);

         if (!continueOnException && lastError != Win32Errors.NO_ERROR)
            NativeError.ThrowException(lastError, path);

         // Return an empty structure (all fields set to null).
         return new NativeMethods.RemoteNameInfo();
      }

      #endregion // GetRemoteNameInfoInternal

      #region GetShareInformationInternal

      /// <summary>Unified method GetShareInformationInternal() to get the <see cref="T:ShareInfo"/> structure of a Server Message Block (SMB) share.</summary>
      /// <param name="structureLevel">Possible structure levels: <see cref="T:NativeMethods.ShareInfo503">503</see>, <see cref="T:NativeMethods.ShareInfo2">2</see>,  <see cref="T:NativeMethods.ShareInfo1">1</see> and <see cref="T:NativeMethods.ShareInfo1005">1005</see>.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      /// <returns>A <see cref="T:ShareInfo"/> class, or <see cref="T:null"/> on failure or when not available.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SecurityCritical]
      internal static ShareInfo GetShareInformationInternal(int structureLevel, string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            return null;

         // The UNC prefix: \\ is not required and always removed.
         host = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, true, false, false, false).Replace(Path.UncPrefix, string.Empty);
         
         bool fallback = false;
         
         startNetShareGetInfo:

         SafeNetApiBuffer safeBuffer;
         uint lastError = NativeMethods.NetShareGetInfo(host, share, (uint)structureLevel, out safeBuffer);

         using (safeBuffer)
            switch (lastError)
            {
               case Win32Errors.NERR_Success:

                  // CA2001:AvoidCallingProblematicMethods

                  IntPtr buffer = IntPtr.Zero;
                  bool successRef = false;
                  safeBuffer.DangerousAddRef(ref successRef);

                  // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                  if (successRef)
                     buffer = safeBuffer.DangerousGetHandle();

                  safeBuffer.DangerousRelease();

                  if (buffer == IntPtr.Zero)
                     NativeError.ThrowException(Resources.HandleDangerousRef);

                  // CA2001:AvoidCallingProblematicMethods


                  switch (structureLevel)
                  {
                     case 1005:
                        return new ShareInfo(host, structureLevel, Filesystem.NativeMethods.GetStructure<NativeMethods.ShareInfo1005>(0, buffer))
                        {
                           NetFullPath = Path.CombineInternal(false, Path.UncPrefix + host, share)
                        };

                     case 503:
                        return new ShareInfo(host, structureLevel, Filesystem.NativeMethods.GetStructure<NativeMethods.ShareInfo503>(0, buffer));

                     case 2:
                        return new ShareInfo(host, structureLevel, Filesystem.NativeMethods.GetStructure<NativeMethods.ShareInfo2>(0, buffer));

                     case 1:
                        return new ShareInfo(host, structureLevel, Filesystem.NativeMethods.GetStructure<NativeMethods.ShareInfo1>(0, buffer));
                  }
                  break;
               

               case Win32Errors.ERROR_ACCESS_DENIED:

                  // Observed when ShareInfo503 is requested, but not supported/possible.
                  // Fall back on ShareInfo2 structure and try again.
               case Win32Errors.RPC_X_BAD_STUB_DATA:
                  if (!fallback && structureLevel != 2)
                  {
                     safeBuffer.Close();
                     structureLevel = 2;
                     fallback = true;
                     goto startNetShareGetInfo;
                  }
                  break;

               default:
                  if (!continueOnException)
                     NativeError.ThrowException(lastError, host, share);
                  break;
            }

         return null;
      }

      #endregion // GetShareInformationInternal
      
      #endregion // Unified Internals
   }
}
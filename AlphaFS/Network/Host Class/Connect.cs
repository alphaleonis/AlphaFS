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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Network
{
   partial class Host
   {
      #region ConnectDrive

      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>If <paramref name="localName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, <see langword="null"/> otherwise.</returns>
      /// <param name="localName">
      ///   The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <see langword="null"/> or
      ///   <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.
      /// </param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      [SecurityCritical]
      public static string ConnectDrive(string localName, string remoteName)
      {
         return ConnectDisconnectCore(new ConnectDisconnectArguments
         {
            LocalName = localName,
            RemoteName = remoteName,
            IsDeviceMap = true
         });
      }

      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>If <paramref name="localName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, null otherwise.</returns>
      /// <param name="localName">
      ///   The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <see langword="null"/> or
      ///   <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.
      /// </param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="userName">
      ///   The user name for making the connection. If <paramref name="userName"/> is <see langword="null"/>, the function uses the default
      ///   user name. (The user context for the process provides the default user name)
      /// </param>
      /// <param name="password">
      ///   The password to be used for making the network connection. If <paramref name="password"/> is <see langword="null"/>, the function
      ///   uses the current default password associated with the user specified by <paramref name="userName"/>.
      /// </param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">
      ///   When the operating system prompts for a credential, the credential should be saved by the credential manager when true.
      /// </param>
      [SecurityCritical]
      public static string ConnectDrive(string localName, string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <returns>If <paramref name="localName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, null otherwise.</returns>
      /// <param name="localName">
      ///   The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <see langword="null"/> or
      ///   <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.
      /// </param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="credentials">
      ///   An instance of <see cref="NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic,
      ///   digest, NTLM, and Kerberos authentication.
      /// </param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">
      ///   When the operating system prompts for a credential, the credential should be saved by the credential manager when true.
      /// </param>
      [SecurityCritical]
      public static string ConnectDrive(string localName, string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <returns>If <paramref name="localName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, null otherwise.</returns>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="localName">
      ///   The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <see langword="null"/> or
      ///   <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.
      /// </param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="userName">
      ///   The user name for making the connection. If <paramref name="userName"/> is <see langword="null"/>, the function uses the default
      ///   user name. (The user context for the process provides the default user name)
      /// </param>
      /// <param name="password">
      ///   The password to be used for making the network connection. If <paramref name="password"/> is <see langword="null"/>, the function
      ///   uses the current default password associated with the user specified by <paramref name="userName"/>.
      /// </param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">
      ///   When the operating system prompts for a credential, the credential should be saved by the credential manager when true.
      /// </param>
      [SecurityCritical]
      public static string ConnectDrive(IntPtr winOwner, string localName, string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <returns>If <paramref name="localName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, null otherwise.</returns>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="localName">
      ///   The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <see langword="null"/> or
      ///   <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.
      /// </param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to MAX_PATH characters in length.</param>
      /// <param name="credentials">
      ///   An instance of <see cref="NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic,
      ///   digest, NTLM, and Kerberos authentication.
      /// </param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">
      ///   When the operating system prompts for a credential, the credential should be saved by the credential manager when true.
      /// </param>
      [SecurityCritical]
      public static string ConnectDrive(IntPtr winOwner, string localName, string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         return ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <exception cref="NetworkInformationException"/>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share.</param>
      [SecurityCritical]
      public static void ConnectTo(string remoteName)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments { RemoteName = remoteName });
      }

      /// <summary>Creates a connection to a network resource.</summary>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share.</param>
      /// <param name="userName">
      ///   The user name for making the connection. If <paramref name="userName"/> is <see langword="null"/>, the function uses the default
      ///   user name. (The user context for the process provides the default user name)
      /// </param>
      /// <param name="password">
      ///   The password to be used for making the network connection. If <paramref name="password"/> is <see langword="null"/>, the function
      ///   uses the current default password associated with the user specified by <paramref name="userName"/>.
      /// </param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">
      ///   When the operating system prompts for a credential, the credential should be saved by the credential manager when true.
      /// </param>
      [SecurityCritical]
      public static void ConnectTo(string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share.</param>
      /// <param name="credentials">An instance of <see cref="NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      ///
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static void ConnectTo(string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            Credential = credentials,
            Prompt = prompt,
            UpdateProfile = updateProfile,
            SaveCredentials = saveCredentials
         });
      }

      /// <summary>Creates a connection to a network resource.</summary>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share.</param>
      /// <param name="userName">
      ///   The user name for making the connection. If <paramref name="userName"/> is <see langword="null"/>, the function uses the default
      ///   user name. (The user context for the process provides the default user name)
      /// </param>
      /// <param name="password">
      ///   The password to be used for making the network connection. If <paramref name="password"/> is <see langword="null"/>, the function
      ///   uses the current default password associated with the user specified by <paramref name="userName"/>.
      /// </param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      [SecurityCritical]
      public static void ConnectTo(IntPtr winOwner, string remoteName, string userName, string password, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <exception cref="NetworkInformationException"/>
      /// <param name="winOwner">Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</param>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share.</param>
      /// <param name="credentials">An instance of <see cref="NetworkCredential"/> which provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</param>
      /// <param name="prompt"><see langword="true"/> always pops-up an authentication dialog box.</param>
      /// <param name="updateProfile"><see langword="true"/> successful network resource connections will be saved.</param>
      /// <param name="saveCredentials">When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</param>
      [SecurityCritical]
      public static void ConnectTo(IntPtr winOwner, string remoteName, NetworkCredential credentials, bool prompt, bool updateProfile, bool saveCredentials)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
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
         ConnectDisconnectCore(new ConnectDisconnectArguments
         {
            LocalName = localName,
            IsDeviceMap = true,
            IsDisconnect = true
         });
      }

      /// <summary>Cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <param name="localName">The name of a local device to be disconnected, such as "F:".</param>
      /// <param name="force">
      ///   Specifies whether the disconnection should occur if there are open files or jobs on the connection.
      ///   If this parameter is <see langword="false"/>, the function fails if there are open files or jobs.
      /// </param>
      /// <param name="updateProfile"><see langword="true"/> successful removal of network resource connections will be saved.</param>
      [SecurityCritical]
      public static void DisconnectDrive(string localName, bool force, bool updateProfile)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
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
      /// <param name="remoteName">A network resource to disconnect from, for example: \\server or \\server\share.</param>
      [SecurityCritical]
      public static void DisconnectFrom(string remoteName)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            IsDisconnect = true
         });
      }

      /// <summary>Cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <param name="remoteName">A network resource to disconnect from, for example: \\server or \\server\share.</param>
      /// <param name="force">
      ///   Specifies whether the disconnection should occur if there are open files or jobs on the connection.
      ///   If this parameter is <see langword="false"/>, the function fails if there are open files or jobs.
      /// </param>
      /// <param name="updateProfile"><see langword="true"/> successful removal of network resource connections will be saved.</param>
      [SecurityCritical]
      public static void DisconnectFrom(string remoteName, bool force, bool updateProfile)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments
         {
            RemoteName = remoteName,
            Prompt = force,
            UpdateProfile = updateProfile,
            IsDisconnect = true
         });
      }

      #endregion // DisconnectFrom


      #region Internal Methods

      /// <summary>Connects to/disconnects from a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>If <see cref="ConnectDisconnectArguments.LocalName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, null otherwise.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="arguments">The <see cref="ConnectDisconnectArguments"/>.</param>
      [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
      [SecurityCritical]
      internal static string ConnectDisconnectCore(ConnectDisconnectArguments arguments)
      {
         uint lastError;

         // Always remove backslash for local device.
         if (!Utils.IsNullOrWhiteSpace(arguments.LocalName))
            arguments.LocalName = Path.RemoveTrailingDirectorySeparator(arguments.LocalName, false).ToUpperInvariant();


         #region Disconnect

         if (arguments.IsDisconnect)
         {
            bool force = arguments.Prompt; // Use value of prompt variable for force value.
            string target = arguments.IsDeviceMap ? arguments.LocalName : arguments.RemoteName;

            if (Utils.IsNullOrWhiteSpace(target))
               throw new ArgumentNullException(arguments.IsDeviceMap ? "localName" : "remoteName");

            lastError = NativeMethods.WNetCancelConnection(target, arguments.UpdateProfile ? NativeMethods.Connect.UpdateProfile : NativeMethods.Connect.None, force);

            if (lastError != Win32Errors.NO_ERROR)
               throw new NetworkInformationException((int)lastError);

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


         // Assemble Connect arguments.
         var connect = NativeMethods.Connect.None;

         if (arguments.IsDeviceMap)
            connect = connect | NativeMethods.Connect.Redirect;

         if (arguments.Prompt)
            connect = connect | NativeMethods.Connect.Prompt | NativeMethods.Connect.Interactive;

         if (arguments.UpdateProfile)
            connect = connect | NativeMethods.Connect.UpdateProfile;

         if (arguments.SaveCredentials)
            connect = connect | NativeMethods.Connect.SaveCredentialManager;


         // Initialize structure.
         var resource = new NativeMethods.NETRESOURCE
         {
            lpLocalName = arguments.LocalName,
            lpRemoteName = arguments.RemoteName,
            dwType = NativeMethods.ResourceType.Disk
         };

         // Three characters for: "X:\0" (Drive X: with null terminator)
         uint bufferSize = 3;
         StringBuilder buffer;

         do
         {
            buffer = new StringBuilder((int)bufferSize);

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

                  bufferSize = bufferSize * 2;
                  break;
            }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);


         if (lastError != Win32Errors.NO_ERROR)
            throw new NetworkInformationException((int)lastError);

         return arguments.IsDeviceMap ? buffer.ToString() : null;

         #endregion // Connect
      }

      #endregion // Internal Methods
   }
}

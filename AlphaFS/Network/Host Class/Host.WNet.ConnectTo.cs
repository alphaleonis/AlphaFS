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
using System.Net;
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Creates a connection to a network resource.</summary>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="remoteName">A network resource to connect to, for example: \\server or \\server\share.</param>
      [SecurityCritical]
      public static void ConnectTo(string remoteName)
      {
         ConnectDisconnectCore(new ConnectDisconnectArguments {RemoteName = remoteName});
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
   }
}

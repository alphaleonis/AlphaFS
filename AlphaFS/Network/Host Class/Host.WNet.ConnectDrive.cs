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
using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>If <paramref name="localName"/> is <see langword="null"/> or <c>string.Empty</c>, returns the last available drive letter, <see langword="null"/> otherwise.</returns>
      /// <param name="localName">
      ///   The name of a local device to be redirected, such as "F:". When <paramref name="localName"/> is <see langword="null"/> or
      ///   <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.
      /// </param>
      /// <param name="remoteName">The network resource to connect to. The string can be up to <see cref="Filesystem.NativeMethods.MaxPath"/> characters in length.</param>
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
      /// <param name="remoteName">The network resource to connect to. The string can be up to <see cref="Filesystem.NativeMethods.MaxPath"/> characters in length.</param>
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
      /// <param name="remoteName">The network resource to connect to. The string can be up to <see cref="Filesystem.NativeMethods.MaxPath"/> characters in length.</param>
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
      /// <param name="remoteName">The network resource to connect to. The string can be up to <see cref="Filesystem.NativeMethods.MaxPath"/> characters in length.</param>
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
      /// <param name="remoteName">The network resource to connect to. The string can be up to <see cref="Filesystem.NativeMethods.MaxPath"/> characters in length.</param>
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
   }
}

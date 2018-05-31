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

using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
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
   }
}

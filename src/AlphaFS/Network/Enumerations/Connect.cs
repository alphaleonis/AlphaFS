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

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Used by function WNetUseConnection(); Set of bit flags describing the connection. This parameter can be any combination of the following values.</summary>
      [Flags]
      internal enum Connect
      {
         /// <summary>No Connect options are used.</summary>
         None = 0,

         /// <summary>This flag instructs the operating system to store the network resource connection. If this bit flag is set, the operating system automatically attempts to restore the connection when the user logs on. The system remembers only successful connections that redirect local devices. It does not remember connections that are unsuccessful or deviceless connections.</summary>
         UpdateProfile = 1,

         /// <summary>If this flag is set, the operating system may interact with the user for authentication purposes.</summary>
         Interactive = 8,

         /// <summary>This flag instructs the system not to use any default settings for user names or passwords without offering the user the opportunity to supply an alternative. This flag is ignored unless <see cref="Interactive"/> is also set.</summary>
         Prompt = 16,

         /// <summary>This flag forces the redirection of a local device when making the connection.</summary>
         Redirect = 128,

         ///// <summary>If this flag is set, the connection was made using a local device redirection. If the lpAccessName parameter points to a buffer, the local device name is copied to the buffer.</summary>
         //LocalDrive = 256,

         // <summary>If this flag is set, the operating system prompts the user for authentication using the command line instead of a graphical user interface (GUI). This flag is ignored unless <see cref="Interactive"/> is also set.</summary>
         //CommandLine = 2048,

         /// <summary>If this flag is set, and the operating system prompts for a credential, the credential should be saved by the credential manager. If the credential manager is disabled for the caller's logon session, or if the network provider does not support saving credentials, this flag is ignored. This flag is also ignored unless you set the "CommandLine" flag.</summary>
         SaveCredentialManager = 4096
      }
   }
}
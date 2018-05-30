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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Provides static methods to retrieve network resource information from a local- or remote host.</summary>
   public static partial class Host
   {
      private static readonly NetworkListManager Manager = new NetworkListManager();

      internal delegate uint EnumerateNetworkObjectDelegate(FunctionData functionData, out SafeGlobalMemoryBufferHandle netApiBuffer, [MarshalAs(UnmanagedType.I4)] int prefMaxLen,
         [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);


      /// <summary>Structure is used to pass additional data to the Win32 function.</summary>
      internal struct FunctionData
      {
         public int EnumType;
         public string ExtraData1;
         public string ExtraData2;
      }


      internal struct ConnectDisconnectArguments
      {
         /// <summary>Handle to a window that the provider of network resources can use as an owner window for dialog boxes.</summary>
         public IntPtr WinOwner;

         /// <summary>The name of a local device to be redirected, such as "F:". When <see cref="LocalName"/> is <see langword="null"/> or <c>string.Empty</c>, the last available drive letter will be used. Letters are assigned beginning with Z:, then Y: and so on.</summary>
         public string LocalName;

         /// <summary>A network resource to connect to/disconnect from, for example: \\server or \\server\share. The string can be up to <see cref="Filesystem.NativeMethods.MaxPath"/> characters in length.</summary>
         public string RemoteName;

         /// <summary>A <see cref="NetworkCredential"/> instance. Use either this or the combination of <see cref="UserName"/> and <see cref="Password"/>.</summary>
         public NetworkCredential Credential;

         /// <summary>The user name for making the connection. If <see cref="UserName"/> is <see langword="null"/>, the function uses the default user name. (The user context for the process provides the default user name)</summary>
         public string UserName;

         /// <summary>The password to be used for making the network connection. If <see cref="Password"/> is <see langword="null"/>, the function uses the current default password associated with the user specified by <see cref="UserName"/>.</summary>
         public string Password;

         /// <summary><see langword="true"/> always pops-up an authentication dialog box.</summary>
         public bool Prompt;

         /// <summary><see langword="true"/> successful network resource connections will be saved.</summary>
         public bool UpdateProfile;

         /// <summary>When the operating system prompts for a credential, the credential should be saved by the credential manager when true.</summary>
         public bool SaveCredentials;

         /// <summary><see langword="true"/> indicates that the operation concerns a drive mapping.</summary>
         public bool IsDeviceMap;

         /// <summary><see langword="true"/> indicates that the operation needs to disconnect from the network resource, otherwise connect.</summary>
         public bool IsDisconnect;
      }


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

         // Always remove backslash.
         if (!Utils.IsNullOrWhiteSpace(arguments.LocalName))
            arguments.LocalName = Path.RemoveTrailingDirectorySeparator(arguments.LocalName).ToUpperInvariant();

         
         if (!Utils.IsNullOrWhiteSpace(arguments.RemoteName))
         {
            if (!arguments.RemoteName.StartsWith(Path.UncPrefix, StringComparison.Ordinal))
               arguments.RemoteName = Path.UncPrefix + arguments.RemoteName;


            // Always remove backslash.
            if (!Utils.IsNullOrWhiteSpace(arguments.RemoteName))
               arguments.RemoteName = Path.RemoveTrailingDirectorySeparator(arguments.RemoteName);
         }

         
         // Disconnect

         if (arguments.IsDisconnect)
         {
            var force = arguments.Prompt; // Use value of prompt variable for force value.
            var target = arguments.IsDeviceMap ? arguments.LocalName : arguments.RemoteName;

            if (Utils.IsNullOrWhiteSpace(target))
               throw new ArgumentNullException(arguments.IsDeviceMap ? "localName" : "remoteName");


            lastError = NativeMethods.WNetCancelConnection(target, arguments.UpdateProfile ? NativeMethods.Connect.UpdateProfile : NativeMethods.Connect.None, force);

            if (lastError != Win32Errors.NO_ERROR)
               throw new NetworkInformationException((int) lastError);

            return null;
         }

         
         // Connect

         // arguments.LocalName is allowed to be null or empty.

         if (Utils.IsNullOrWhiteSpace(arguments.RemoteName) && !arguments.IsDeviceMap)
            throw new ArgumentNullException("arguments.RemoteName");
         

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

                  bufferSize = bufferSize * 2;
                  break;
            }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);


         if (lastError != Win32Errors.NO_ERROR)
            throw new NetworkInformationException((int) lastError);


         return arguments.IsDeviceMap ? buffer.ToString() : null;
      }
      

      [SecurityCritical]
      internal static IEnumerable<TStruct> EnumerateNetworkObjectCore<TStruct, TNative>(FunctionData functionData, Func<TNative, SafeGlobalMemoryBufferHandle, TStruct> createTStruct, EnumerateNetworkObjectDelegate enumerateNetworkObject, bool continueOnException)
      {
         int objectSize;
         bool isString;

         switch (functionData.EnumType)
         {
            // Logical Drives
            case 1:
               isString = true;
               objectSize = 6; // Should always be 6.
               break;

            default:
               var objectType = typeof(TNative);
               isString = objectType == typeof(string);
               objectSize = isString ? 0 : Marshal.SizeOf(objectType);
               break;
         }


         uint lastError;
         do
         {
            uint entriesRead;
            uint totalEntries;
            uint resumeHandle;
            SafeGlobalMemoryBufferHandle buffer;

            lastError = enumerateNetworkObject(functionData, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle);

            using (buffer)
               switch (lastError)
               {
                  case Win32Errors.NERR_Success:
                  case Win32Errors.ERROR_MORE_DATA:
                     if (entriesRead > 0)
                     {
                        for (int i = 0, itemOffset = 0; i < entriesRead; i++, itemOffset += objectSize)

                           yield return (TStruct) (isString ? buffer.PtrToStringUni(itemOffset, 2) : (object) createTStruct(buffer.PtrToStructure<TNative>(itemOffset), buffer));
                     }
                     break;

                  case Win32Errors.ERROR_BAD_NETPATH:
                     break;

                  // Observed when SHARE_INFO_503 is requested but not supported/possible.
                  case Win32Errors.RPC_X_BAD_STUB_DATA:
                  case Win32Errors.ERROR_NOT_SUPPORTED:
                     yield break;
               }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);

         if (lastError != Win32Errors.NO_ERROR && !continueOnException)
            throw new NetworkInformationException((int) lastError);
      }


      /// <summary>This method uses <see cref="NativeMethods.REMOTE_NAME_INFO"/> level to retrieve full REMOTE_NAME_INFO structure.</summary>
      /// <returns>A <see cref="NativeMethods.REMOTE_NAME_INFO"/> structure.</returns>
      /// <remarks>AlphaFS regards network drives created using SUBST.EXE as invalid.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="PathTooLongException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="path">The local path with drive name.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      internal static NativeMethods.REMOTE_NAME_INFO GetRemoteNameInfoCore(string path, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");


         path = Path.GetRegularPathCore(path, GetFullPathOptions.CheckInvalidPathChars, false);


         uint lastError;
         uint bufferSize = 1024;

         do
         {
            using (var buffer = new SafeGlobalMemoryBufferHandle((int) bufferSize))
            {
               // Structure: UNIVERSAL_NAME_INFO_LEVEL = 1 (not used in AlphaFS).
               // Structure: REMOTE_NAME_INFO_LEVEL    = 2

               lastError = NativeMethods.WNetGetUniversalName(path, 2, buffer, out bufferSize);

               if (lastError == Win32Errors.NO_ERROR)
                  return buffer.PtrToStructure<NativeMethods.REMOTE_NAME_INFO>(0);
            }

         } while (lastError == Win32Errors.ERROR_MORE_DATA);


         if (lastError != Win32Errors.NO_ERROR && !continueOnException)
            throw new NetworkInformationException((int) lastError);


         // Return an empty structure (all fields set to null).
         return new NativeMethods.REMOTE_NAME_INFO();
      }
   }
}

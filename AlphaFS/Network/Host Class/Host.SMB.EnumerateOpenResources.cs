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
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Enumerates open resources from the local host.</summary>
      /// <returns><see cref="IEnumerable{OpenResourceInfo}"/> open resources from the local host.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static IEnumerable<OpenResourceInfo> EnumerateOpenResources()
      {
         return EnumerateOpenResourcesCore(null, null, null, false);
      }

      
      /// <summary>Enumerates open resources from the specified host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> open resources from the specified <paramref name="host"/>.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="basePath">
      ///   This parameter may be <see langword="null"/>. Enumerates only resources that have the value of the basepath parameter as a prefix.
      ///   (A prefix is the portion of a path that comes before a backslash.)
      /// </param>
      /// <param name="typeName">
      ///   This parameter may be <see langword="null"/>. The name of the user or the name of the connection; If <paramref name="typeName"/>
      ///   does not begin with two backslashes ("\\") it indicates the name of the user. If <paramref name="typeName"/> begins with two
      ///   backslashes ("\\") it indicates the name of the connection.
      /// </param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static IEnumerable<OpenResourceInfo> EnumerateOpenResources(string host, string basePath, string typeName, bool continueOnException)
      {
         return EnumerateOpenResourcesCore(host, basePath, typeName, continueOnException);
      }




      /// <summary>>Enumerates open resources from the specified host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> open resources from the specified <paramref name="host"/>.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="basePath">
      ///   This parameter may be <see langword="null"/>. Enumerates only resources that have the value of the basepath parameter as a prefix.
      ///   (A prefix is the portion of a path that comes before a backslash.)
      /// </param>
      /// <param name="typeName">
      ///   This parameter may be <see langword="null"/>. The name of the user or the name of the connection; If <paramref name="typeName"/>
      ///   does not begin with two backslashes ("\\") it indicates the name of the user. If <paramref name="typeName"/> begins with two
      ///   backslashes ("\\") it indicates the name of the connection.
      /// </param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      internal static IEnumerable<OpenResourceInfo> EnumerateOpenResourcesCore(string host, string basePath, string typeName, bool continueOnException)
      {
         basePath = Utils.IsNullOrWhiteSpace(basePath) ? null : Path.GetRegularPathCore(basePath, GetFullPathOptions.CheckInvalidPathChars, false);
         typeName = Utils.IsNullOrWhiteSpace(typeName) ? null : typeName;


         var fd = new FunctionData {ExtraData1 = basePath, ExtraData2 = typeName};

         return EnumerateNetworkObjectCore(fd, (NativeMethods.FILE_INFO_3 structure, SafeGlobalMemoryBufferHandle buffer) => new OpenResourceInfo(host, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               // When host == null, the local computer is used.
               // However, the resulting Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               var stripUnc = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

               return NativeMethods.NetFileEnum(stripUnc, fd.ExtraData1, fd.ExtraData2, 3, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle);

            }, continueOnException);
      }
   }
}

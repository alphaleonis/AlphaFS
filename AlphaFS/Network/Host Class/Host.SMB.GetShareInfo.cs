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
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(string uncPath, bool continueOnException)
      {
         var unc = GetHostShareFromPath(uncPath);
         return GetShareInfoCore(ShareInfoLevel.Info503, unc[0], unc[1], continueOnException);
      }

      
      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(ShareInfoLevel shareLevel, string uncPath, bool continueOnException)
      {
         var unc = GetHostShareFromPath(uncPath);
         return GetShareInfoCore(shareLevel, unc[0], unc[1], continueOnException);
      }

      
      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="host">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(string host, string share, bool continueOnException)
      {
         return GetShareInfoCore(ShareInfoLevel.Info503, host, share, continueOnException);
      }

      
      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(ShareInfoLevel shareLevel, string host, string share, bool continueOnException)
      {
         return GetShareInfoCore(shareLevel, host, share, continueOnException);
      }




      /// <summary>Gets the <see cref="ShareInfo"/> structure of a Server Message Block (SMB) share.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      internal static ShareInfo GetShareInfoCore(ShareInfoLevel shareLevel, string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            return null;


         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         var stripUnc = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

         var fallback = false;


         startNetShareGetInfo:

         SafeGlobalMemoryBufferHandle safeBuffer;

         uint structureLevel = Convert.ToUInt16(shareLevel, CultureInfo.InvariantCulture);
         var lastError = NativeMethods.NetShareGetInfo(stripUnc, share, structureLevel, out safeBuffer);

         using (safeBuffer)
         {
            switch (lastError)
            {
               case Win32Errors.NERR_Success:
                  switch (shareLevel)
                  {
                     case ShareInfoLevel.Info1005:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_1005>(0))
                        {
                           NetFullPath = Path.CombineCore(false, Path.UncPrefix + stripUnc, share)
                        };


                     case ShareInfoLevel.Info503:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_503>(0));


                     case ShareInfoLevel.Info2:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_2>(0));


                     case ShareInfoLevel.Info1:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_1>(0));
                  }

                  break;


               // Observed when SHARE_INFO_503 is requested, but not supported/possible.
               // Fall back on SHARE_INFO_2 structure and try again.
               case Win32Errors.RPC_X_BAD_STUB_DATA:


               case Win32Errors.ERROR_ACCESS_DENIED:
                  if (!fallback && shareLevel != ShareInfoLevel.Info2)
                  {
                     shareLevel = ShareInfoLevel.Info2;
                     fallback = true;
                     goto startNetShareGetInfo;
                  }

                  break;


               default:
                  if (!continueOnException)
                     throw new NetworkInformationException((int) lastError);

                  break;
            }


            return null;
         }
      }
   }
}

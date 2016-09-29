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

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains the identification number and other pertinent information about files, devices, and pipes. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed class OpenResourceInfo
   {
      #region Constructor

      /// <summary>Create a OpenResourceInfo instance.</summary>
      internal OpenResourceInfo(string host, NativeMethods.FILE_INFO_3 fileInfo)
      {
         Host = host;
         Id = fileInfo.fi3_id;
         Permissions = fileInfo.fi3_permissions;
         TotalLocks = fileInfo.fi3_num_locks;
         PathName = fileInfo.fi3_pathname.Replace(@"\\", @"\");
         UserName = fileInfo.fi3_username;
      }

      #endregion // Constructor

      #region Methods

      /// <summary>Forces the open resource to close.</summary>
      /// <remarks>You should this method with caution because it does not write data cached on the client system to the file before closing the file.</remarks>
      public void Close()
      {
         uint lastError = NativeMethods.NetFileClose(Host, (uint) Id);
         if (lastError != Win32Errors.NERR_Success && lastError != Win32Errors.NERR_FileIdNotFound)
            NativeError.ThrowException(lastError, Host, PathName);
      }

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

      /// <summary>The identification number assigned to the resource when it is opened.</summary>
      public long Id { get; private set; }

      /// <summary>The path of the opened resource.</summary>
      public string PathName { get; private set; }

      /// <summary>The access permissions associated with the opening application. This member can be one or more of the following <see cref="AccessPermissions"/> values.</summary>
      public AccessPermissions Permissions { get; private set; }

      /// <summary>The number of file locks on the file, device, or pipe.</summary>
      public long TotalLocks { get; private set; }

      /// <summary>Specifies which user (on servers that have user-level security) or which computer (on servers that have share-level security) opened the resource.</summary>
      public string UserName { get; private set; }

      #endregion // Properties
   }
}

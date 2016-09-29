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

namespace Alphaleonis.Win32.Network
{
   /// <summary>A set of bit flags that describe the permissions for the shared resource's on servers running with share-level security.</summary>
   /// <remarks>Note that Windows does not support share-level security. This member is ignored on a server running user-level security.</remarks>
   [Flags]
   public enum AccessPermissions
   {
      /// <summary>No permissions.</summary>
      None = 0,

      /// <summary>ACCESS_READ
      /// <para>Permission to read data from a resource and, by default, to execute the resource.</para>
      /// </summary>
      Read = 1,

      /// <summary>ACCESS_WRITE
      /// <para>Permission to write data to the resource.</para>
      /// </summary>
      Write = 2,

      /// <summary>ACCESS_CREATE
      /// <para>Permission to create an instance of the resource (such as a file); data can be written to the resource as the resource is created.</para>
      /// </summary>
      Create = 4,

      /// <summary>ACCESS_EXEC
      /// <para>Permission to execute the resource.</para>
      /// </summary>
      Execute = 8,

      /// <summary>ACCESS_DELETE
      /// <para>Permission to delete the resource.</para>
      /// </summary>
      Delete = 16,

      /// <summary>ACCESS_ATRIB
      /// <para>Permission to modify the resource's attributes, such as the date and time when a file was last modified.</para>
      /// </summary>
      Attributes = 32,

      /// <summary>ACCESS_PERM
      /// <para>Permission to modify the permissions (read, write, create, execute, and delete) assigned to a resource for a user or application.</para>
      /// </summary>
      Permissions = 64,

      /// <summary>ACCESS_ALL
      /// <para>Permission to read, write, create, execute, and delete resources, and to modify their attributes and permissions.</para>
      /// </summary>
      All = 32768
   }
}
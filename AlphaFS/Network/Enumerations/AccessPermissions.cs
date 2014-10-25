/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
   /// <summary>A set of bit flags that describe the permissions for the shared resource's
   /// <para>on servers running with share-level security.</para>
   /// </summary>
   /// <remarks>
   /// <para>Note that Windows does not support share-level security.</para>
   /// <para>This member is ignored on a server running user-level security.</para>
   /// </remarks>
   [Flags]
   public enum AccessPermissions
   {
      /// <summary>No permissions.</summary>
      None = 0,

      /// <summary>Permission to read data from a resource and, by default, to execute the resource.
      /// <para>Win32: ACCESS_READ = 0x00000001</para>
      /// </summary>
      Read = 1,

      /// <summary>Permission to write data to the resource.
      /// <para>Win32: ACCESS_WRITE = 0x00000002</para>
      /// </summary>
      Write = 2,

      /// <summary>Permission to create an instance of the resource (such as a file);
      /// <para>data can be written to the resource as the resource is created.</para>
      /// <para>Win32: ACCESS_CREATE = 0x00000004</para>
      /// </summary>
      Create = 4,

      /// <summary>Permission to execute the resource.
      /// <para>Win32: ACCESS_EXEC = 0x00000008</para>
      /// </summary>
      Execute = 8,

      /// <summary>Permission to delete the resource.
      /// <para>Win32: ACCESS_DELETE = 0x00000010</para>
      /// </summary>
      Delete = 16,

      /// <summary>Permission to modify the resource's attributes,
      /// <para>such as the date and time when a file was last modified.</para>
      /// <para>Win32: ACCESS_ATRIB = 0x00000020</para>
      /// </summary>
      Attributes = 32,

      /// <summary>Permission to modify the permissions (read, write, create, execute, and delete)
      /// <para>assigned to a resource for a user or application.</para>
      /// <para>Win32: ACCESS_PERM = 0x00000040</para>
      /// </summary>
      Permissions = 64,

      /// <summary>Permission to read, write, create, execute, and delete resources,
      /// <para>and to modify their attributes and permissions.</para>
      /// <para>Win32: ACCESS_ALL = 0x00008000</para>
      /// </summary>
      All = 32768
   }
}
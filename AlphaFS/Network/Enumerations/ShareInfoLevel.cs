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

namespace Alphaleonis.Win32.Network
{
   /// <summary>The <see cref="ShareInfo"/> information level.</summary>
   public enum ShareInfoLevel
   {
      /// <summary>No specific information level used.</summary>
      None = 0,

      /// <summary>Contains information about the shared resource, including the name and type of the resource, and a comment associated with the resource.</summary>
      Info1 = 1,

      /// <summary>Contains information about the shared resource, including the name, type, and permissions of the resource, comments associated with the resource,
      /// the maximum number of concurrent connections, the number of current connections, the local path for the resource, and a password for the current connection.
      /// </summary>
      Info2 = 2,

      /// <summary>Contains information about the shared resource, including the server name, name of the resource, type, and permissions,
      /// the number of connections, and other pertinent information.
      /// </summary>
      Info503 = 503,

      /// <summary>Contains information about the shared resource.</summary>
      Info1005 = 1005
   }
}
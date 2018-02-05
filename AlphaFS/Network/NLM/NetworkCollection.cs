/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.Collections;
using System.Collections.Generic;

namespace Alphaleonis.Win32.Network
{
   /// <summary>An enumerable collection of <see cref="Network"/> objects.</summary>
   public class NetworkCollection : IEnumerable<Network>
   {
      private readonly IEnumerable _networkEnumerable;


      internal NetworkCollection(IEnumerable networkEnumerable)
      {
         _networkEnumerable = networkEnumerable;
      }


      /// <summary>Returns the strongly typed enumerator for this collection.</summary>
      /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/>  object.</returns>
      public IEnumerator<Network> GetEnumerator()
      {
         foreach (INetwork network in _networkEnumerable)

            yield return new Network(network);
      }


      /// <summary>Returns the enumerator for this collection.</summary>
      ///<returns>An <see cref="System.Collections.IEnumerator"/> object.</returns> 
      IEnumerator IEnumerable.GetEnumerator()
      {
         foreach (INetwork network in _networkEnumerable)

            yield return new Network(network);
      }
   }
}
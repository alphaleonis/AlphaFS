/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Linq;

namespace Alphaleonis.Win32.Filesystem
{

   /// <summary>Information about an alternate data stream.</summary>   
   public struct AlternateDataStreamInfo
   {
      #region Private Fields

      private readonly string m_name;
      private readonly long m_size;

      #endregion

      #region Construction

      /// <summary>Constructor.</summary>
      /// <param name="name">The name of the stream. May be empty for the default stream.</param>
      /// <param name="size">The size of the stream.</param>
      public AlternateDataStreamInfo(string name, long size)
      {
         m_name = name ?? String.Empty;
         m_size = size;
      }

      #endregion

      #region Public Properties

      public string Name
      {
         get
         {
            return m_name;
         }
      }

      public long Size
      {
         get
         {
            return m_size;
         }
      }

      #endregion

      #region Public Methods

      /// <summary>Returns the hash code for this instance.</summary>
      /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
      public override int GetHashCode()
      {
         return m_name.GetHashCode();
      }

      /// <summary>Indicates whether this instance and a specified object are equal.</summary>
      /// <param name="obj">The object to compare with the current instance.</param>
      /// <returns>
      ///   true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
      /// </returns>
      public override bool Equals(object obj)
      {
         if (obj is AlternateDataStreamInfo)
         {
            AlternateDataStreamInfo other = (AlternateDataStreamInfo)obj;
            return Name.Equals(other.Name) && Size.Equals(other.Size);
         }

         return false;
      }

      /// <summary>Equality operator.</summary>
      /// <param name="first">The first operand.</param>
      /// <param name="second">The second operand.</param>
      /// <returns>The result of the operation.</returns>
      public static bool operator ==(AlternateDataStreamInfo first, AlternateDataStreamInfo second)
      {
         return first.Equals(second);
      }

      /// <summary>Inequality operator.</summary>
      /// <param name="first">The first operand.</param>
      /// <param name="second">The second operand.</param>
      /// <returns>The result of the operation.</returns>
      public static bool operator !=(AlternateDataStreamInfo first, AlternateDataStreamInfo second)
      {
         return !first.Equals(second);
      }

      #endregion
   }
}
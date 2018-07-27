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
using System.Threading;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Struct with retry arguments that are used by several methods.</summary>
   public class RetryArguments : IDisposable
   {
      #region Constructors

      /// <summary>Intializes a RetryArguments instance.</summary>
      public RetryArguments(int retry)
      {
         Retry = retry;

         RetryTimeout = TimeSpan.FromSeconds(10);

         WaitHandle = null;
      }
      

      /// <summary>Intializes a RetryArguments instance.</summary>
      public RetryArguments(int retry, TimeSpan retryTimeout) : this(retry)
      {
         RetryTimeout = retryTimeout;
      }


      /// <summary>Intializes a RetryArguments instance.</summary>
      public RetryArguments(int retry, WaitHandle waitHandle) : this(retry)
      {
         if (null == waitHandle)
            throw new ArgumentNullException("waitHandle");

         WaitHandle = waitHandle;
      }


      /// <summary>Intializes a RetryArguments instance.</summary>
      public RetryArguments(int retry, TimeSpan retryTimeout, WaitHandle waitHandle) : this(retry, retryTimeout)
      {
         if (null == waitHandle)
            throw new ArgumentNullException("waitHandle");

         WaitHandle = waitHandle;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>The number of retries, excluding the first (default) attempt. Default is <c>0</c>.</summary>
      public int Retry { get; private set; }


      /// <summary>A <see cref="TimeSpan"/> wait time between retries. Default is <c>10</c> seconds.</summary>
      public TimeSpan RetryTimeout { get; private set; }


      /// <summary>Gets or sets the <see cref="WaitHandle"/> to abort the retry action.</summary>
      public WaitHandle WaitHandle { get; private set; }

      #endregion // Properties


      #region Disposable Members

      /// <summary>Class deconstructor.</summary>
      ~RetryArguments()
      {
         Dispose();
      }


      /// <summary>Disposes the <see cref="WaitHandle"/> member, when used.</summary>
      public void Dispose()
      {
         if (null != WaitHandle)
         {
            WaitHandle.Dispose();
            WaitHandle = null;
         }

         GC.SuppressFinalize(this);
      }
      
      #endregion // Disposable Members
   }
}

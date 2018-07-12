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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public static class UnitTestAssert
   {
      public static void IsElevatedProcess()
      {
         if (!Alphaleonis.Win32.Security.ProcessContext.IsElevatedProcess)
            Inconclusive("This unit test must be run elevated.");
      }

      
      public static void Inconclusive(string errorMessage)
      {
         Console.WriteLine("{0}{1}{0}", Environment.NewLine, errorMessage);

         Assert.Inconclusive(errorMessage);

         //throw new AssertFailedException(errorMessage);
      }


      public static void InconclusiveBecauseResourcesAreUnavailable()
      {
         Inconclusive("No resources are available for this unit test.");
      }


      public static void InconclusiveBecauseFileNotFound(string fullPath)
      {
         Inconclusive("The file system object was not found: " + fullPath);
      }


      public static void ThrowsException<T>(Action action, string findString = null)
      {
         Exception exception = null;
         var message = string.Empty;
         var expectedException = typeof(T);

         try
         {
            action();
         }
         catch (Exception ex)
         {
            exception = ex;
            message = ex.Message;
         }


         var gotException = null != exception && exception.GetType() == expectedException;


         if (null != exception)
            Console.WriteLine("\n\t[{0}]{1} {2}: {3}", MethodBase.GetCurrentMethod().Name,

               gotException ? string.Empty : " Caught unexpected",
               gotException ? expectedException.Name : exception.GetType().Name, message.Trim());


         Assert.IsTrue(gotException, "The {0} is not caught, but is expected to.", expectedException.Name);


         if (gotException && !Alphaleonis.Utils.IsNullOrWhiteSpace(findString))
            Assert.IsTrue(message.Contains(findString), "The findString is not found in the exception message, but is expected to.");
      }


      public static bool TestException<T>(Action action)
      {
         Exception exception = null;
         var message = string.Empty;
         var expectedException = typeof(T);

         try
         {
            action();
         }
         catch (Exception ex)
         {
            exception = ex;
            message = ex.Message;
         }


         var gotException = null != exception && exception.GetType() == expectedException;


         if (null != exception)
            Console.WriteLine("\n\t[{0}]{1} {2}: {3}", MethodBase.GetCurrentMethod().Name,

               gotException ? string.Empty : " Caught unexpected",
               gotException ? expectedException.Name : exception.GetType().Name, message.Trim());


         return gotException;
      }
   }
}

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
   public static class ExceptionAssert
   {
      public static void AlreadyExistsException(Action action, string findString = null)
      {
         TestException(action, typeof(Alphaleonis.Win32.Filesystem.AlreadyExistsException), findString);
      }


      public static void ArgumentException(Action action, string findString = null)
      {
         TestException(action, typeof(ArgumentException), findString);
      }

      
      public static void DeviceNotReadyException(Action action, string findString = null)
      {
         TestException(action, typeof(Alphaleonis.Win32.Filesystem.DeviceNotReadyException), findString);
      }


      public static void DirectoryNotEmptyException(Action action, string findString = null)
      {
         TestException(action, typeof(Alphaleonis.Win32.Filesystem.DirectoryNotEmptyException), findString);
      }


      public static void DirectoryNotFoundException(Action action, string findString = null)
      {
         TestException(action, typeof(System.IO.DirectoryNotFoundException), findString);
      }


      public static void DirectoryReadOnlyException(Action action, string findString = null)
      {
         TestException(action, typeof(Alphaleonis.Win32.Filesystem.DirectoryReadOnlyException), findString);
      }
      

      public static void FileNotFoundException(Action action, string findString = null)
      {
         TestException(action, typeof(System.IO.FileNotFoundException), findString);
      }


      public static void FileReadOnlyException(Action action, string findString = null)
      {
         TestException(action, typeof(Alphaleonis.Win32.Filesystem.FileReadOnlyException), findString);
      }


      public static void IOException(Action action, string findString = null)
      {
         TestException(action, typeof(System.IO.IOException), findString);
      }


      public static void NotSupportedException(Action action, string findString = null)
      {
         TestException(action, typeof(NotSupportedException), findString);
      }


      public static void PathTooLongException(Action action, string findString = null)
      {
         TestException(action, typeof(System.IO.PathTooLongException), findString);
      }


      public static void UnauthorizedAccessException(Action action, string findString = null)
      {
         TestException(action, typeof(UnauthorizedAccessException), findString);
      }
      

      private static void TestException(Action action, Type expectedException, string findString = null)
      {
         Exception exception = null;
         var message = string.Empty;

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
   }
}

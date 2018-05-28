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
      public static void AlreadyExistsException(Exception exception)
      {
         IsExpected(exception, typeof(Alphaleonis.Win32.Filesystem.AlreadyExistsException));
      }


      public static void ArgumentException(Exception exception)
      {
         IsExpected(exception, typeof(ArgumentException));
      }


      public static void DeviceNotReadyException(Exception exception)
      {
         IsExpected(exception, typeof(Alphaleonis.Win32.Filesystem.DeviceNotReadyException));
      }
      

      public static void DirectoryNotEmptyException(Exception exception)
      {
         IsExpected(exception, typeof(Alphaleonis.Win32.Filesystem.DirectoryNotEmptyException));
      }


      public static void DirectoryNotFoundException(Exception exception)
      {
         IsExpected(exception, typeof(System.IO.DirectoryNotFoundException));
      }


      public static void DirectoryReadOnlyException(Exception exception)
      {
         IsExpected(exception, typeof(Alphaleonis.Win32.Filesystem.DirectoryReadOnlyException));
      }

      
      public static void FileNotFoundException(Exception exception)
      {
         IsExpected(exception, typeof(System.IO.FileNotFoundException));
      }

      
      public static void FileReadOnlyException(Exception exception)
      {
         IsExpected(exception, typeof(Alphaleonis.Win32.Filesystem.FileReadOnlyException));
      }


      public static void IOException(Exception exception)
      {
         IsExpected(exception, typeof(System.IO.IOException));
      }


      public static void NotSupportedException(Exception exception)
      {
         IsExpected(exception, typeof(NotSupportedException));
      }
      

      public static void PathTooLongException(Exception exception)
      {
         IsExpected(exception, typeof(System.IO.PathTooLongException));
      }
      

      public static void UnauthorizedAccessException(Exception exception)
      {
         IsExpected(exception, typeof(UnauthorizedAccessException));
      }
      



      public static void IsExpected(Exception exception, Type expectedException)
      {
         var gotException = null != exception && exception.GetType() == expectedException;


         Console.WriteLine("\n\t[{0}]{1} {2}: {3}", MethodBase.GetCurrentMethod().Name,

            gotException ? string.Empty : " Caught unexpected", gotException ? expectedException.Name : exception.GetType().Name, exception.Message);


         Assert.IsTrue(gotException, "The {0} is not caught, but is expected to.", expectedException.Name);
      }
   }
}

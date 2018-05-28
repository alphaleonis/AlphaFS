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
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public static class UnitTestAssert
   {
      public static void IsElevated()
      {
         if (!Alphaleonis.Win32.Security.ProcessContext.IsElevatedProcess)
            SetInconclusive("This unit test must be run elevated.");
      }


      public static void SetInconclusive(string errorMessage)
      {
         Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0}{1}{0}", Environment.NewLine, errorMessage));

         Assert.Inconclusive(errorMessage);

         //throw new AssertFailedException(errorMessage);
      }


      public static void SetInconclusiveBecauseEnumerationIsEmpty()
      {
         SetInconclusive("The enumeration returned an empty collection, which is not expected.");
      }


      public static void SetInconclusiveBecauseFileNotFound(string fullPath)
      {
         SetInconclusive("The file system object was not found: " + fullPath);
      }
   }
}

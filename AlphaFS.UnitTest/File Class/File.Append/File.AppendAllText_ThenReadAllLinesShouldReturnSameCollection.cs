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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class File_AppendTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_AppendAllText_ThenReadAllLinesShouldReturnSameCollection_LocalAndNetwork_Succes()
      {
         File_AppendAllText_ThenReadAllLinesShouldReturnSameCollection(false);
         File_AppendAllText_ThenReadAllLinesShouldReturnSameCollection(true);
      }


      private void File_AppendAllText_ThenReadAllLinesShouldReturnSameCollection(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;

            Console.WriteLine("Input File Path: [{0}]", file);

            var sample = new[] {UnitTestConstants.StreamArrayContent[0], UnitTestConstants.StreamArrayContent[1]};


            Alphaleonis.Win32.Filesystem.File.AppendAllText(file, sample[0] + Environment.NewLine);
            Alphaleonis.Win32.Filesystem.File.AppendAllText(file, sample[1] + Environment.NewLine);


            var collection = System.IO.File.ReadAllLines(file).ToArray();

            for (int i = 0, l = collection.Length; i < l; i++)
               Console.WriteLine("\n\t" + collection[i]);


            CollectionAssert.AreEquivalent(sample, collection);
         }

         Console.WriteLine();
      }
   }
}

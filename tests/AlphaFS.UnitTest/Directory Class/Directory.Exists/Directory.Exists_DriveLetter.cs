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

namespace AlphaFS.UnitTest
{
   public partial class ExistsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Exists_DriveLetter_LocalAndNetwork_Success()
      {
         Directory_Exists_DriveLetter(false);
         Directory_Exists_DriveLetter(true);
      }


      private void Directory_Exists_DriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         // Test Driveletter according to System.IO
         var driveSysIO = UnitTestConstants.SysDrive;
         if (isNetwork)
            driveSysIO = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveSysIO);


         // C:
         var sysIOshouldBe = true;
         var inputDrive = driveSysIO;
         var existSysIO = System.IO.Directory.Exists(inputDrive);
         var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);

         Console.WriteLine("System.IO/AlphaFS (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);

         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");




         // C:\
         sysIOshouldBe = true;
         inputDrive = driveSysIO + @"\";
         existSysIO = System.IO.Directory.Exists(inputDrive);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);

         Console.WriteLine("\nSystem.IO   (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);
         Console.WriteLine("AlphaFS     (should be {0}):\t[{1}]\t\tdrive= {2}", true.ToString().ToUpperInvariant(), existAlpha, inputDrive);

         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");




         // \\?\C:
         sysIOshouldBe = false;
         inputDrive = isNetwork ? Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + driveSysIO.TrimStart('\\') : Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + driveSysIO;
         existSysIO = System.IO.Directory.Exists(inputDrive);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);

         Console.WriteLine("\nSystem.IO   (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);
         Console.WriteLine("AlphaFS     (should be {0}):\t[{1}]\t\tdrive= {2}", true.ToString().ToUpperInvariant(), existAlpha, inputDrive);

         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.IsTrue(existAlpha);




         // \\?\C:\
         sysIOshouldBe = false;
         inputDrive = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + driveSysIO.TrimStart('\\') : Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + driveSysIO) + @"\";
         existSysIO = System.IO.Directory.Exists(inputDrive);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);

         Console.WriteLine("\nSystem.IO   (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);
         Console.WriteLine("AlphaFS     (should be {0}):\t[{1}]\t\tdrive= {2}", true.ToString().ToUpperInvariant(), existAlpha, inputDrive);
         
         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.IsTrue(existAlpha);


         Console.WriteLine();
      }
   }
}

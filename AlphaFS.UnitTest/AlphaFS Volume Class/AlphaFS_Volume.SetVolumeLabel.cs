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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_SetVolumeLabel_Local_Success()
      {
         UnitTestAssert.IsElevatedProcess();
         UnitTestConstants.PrintUnitTestHeader(false);
         
         const string newLabel = "ÂĽpĥæƑŞ ŠëtVőlümèĻāßƩl() Ťest";
         const string template = "System Drive: [{0}]\tCurrent Label: [{1}]\n";
         var drive = UnitTestConstants.SysDrive;


         #region Get Label

         var originalLabel = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);
         Console.WriteLine(template, drive, originalLabel);

         Assert.IsTrue(originalLabel.Equals(Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive)));

         #endregion // Get Label


         #region Set Label

         var isLabelSet = false;
         var currentLabel = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);
         try
         {
            Alphaleonis.Win32.Filesystem.Volume.SetVolumeLabel(drive, newLabel);
            isLabelSet = true;

            Console.WriteLine(template, drive, newLabel);
            Console.WriteLine("Set label.");
            Assert.IsTrue(!currentLabel.Equals(newLabel));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(isLabelSet);

         #endregion // Set Label


         #region Remove Label

         var isLabelRemoved = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Volume.DeleteVolumeLabel(drive);
            isLabelRemoved = true;

            currentLabel = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);

            Console.WriteLine(template, drive, currentLabel);
            Console.WriteLine("Removed label.");
            Assert.IsTrue(currentLabel.Equals(string.Empty));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(isLabelRemoved);

         #endregion // Remove Label


         #region Restore Label

         isLabelSet = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Volume.SetVolumeLabel(drive, originalLabel);
            isLabelSet = true;

            currentLabel = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);

            Console.WriteLine(template, drive, currentLabel);
            Console.WriteLine("Restored label.");
            Assert.IsTrue(currentLabel.Equals(originalLabel));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(isLabelSet);

         #endregion // Restore Label
      }
   }
}

/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>Containts static variables, used by unit tests.</summary>
   public static class UnitTestConstants
   {
      #region Fields

      public static readonly string Local = @"LOCAL";
      public static readonly string Network = @"NETWORK";

      public static readonly string LocalHost = Environment.MachineName;
      public static readonly string LocalHostShare = Environment.SystemDirectory;

      public static readonly string SysDrive = Environment.GetEnvironmentVariable("SystemDrive");
      public static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      public static readonly string SysRoot32 = Path.Combine(SysRoot, "System32");
      public static readonly string AppData = Environment.GetEnvironmentVariable("AppData");
      public static readonly string NotepadExe = Path.Combine(SysRoot32, "notepad.exe");

      public const string TextTrue = "IsTrue";
      public const string TextFalse = "IsFalse";
      public const string TenNumbers = "0123456789";
      public const string TextHelloWorld = "Hëllõ Wørld!";
      public const string TextGoodByeWorld = "GóödByé Wôrld!";
      public const string TextAppend = "GóödByé Wôrld!";
      public const string TextUnicode = "ÛņïÇòdè; ǖŤƑ";

      #region InputPaths

      public static readonly string[] InputPaths =
      {
         @".",
         @".zip",
         
         SysDrive + @"\\test.txt",
         SysDrive + @"\/test.txt",

         Path.DirectorySeparator,
         Path.DirectorySeparator + @"Program Files\Microsoft Office",
         
         Path.GlobalRootPrefix + @"device\harddisk0\partition1\",
         Path.VolumePrefix + @"{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\Program Files\notepad.exe",

         @"Program Files\Microsoft Office",
         SysDrive[0].ToString(CultureInfo.InvariantCulture),
         SysDrive,
         SysDrive + @"\",
         SysDrive + @"\a",
         SysDrive + @"\a\",
         SysDrive + @"\a\b",
         SysDrive + @"\a\b\",
         SysDrive + @"\a\b\c",
         SysDrive + @"\a\b\c\",
         SysDrive + @"\a\b\c\f",
         SysDrive + @"\a\b\c\f.",
         SysDrive + @"\a\b\c\f.t",
         SysDrive + @"\a\b\c\f.tx",
         SysDrive + @"\a\b\c\f.txt",

         Path.LongPathPrefix + @"Program Files\Microsoft Office",
         Path.LongPathPrefix + SysDrive[0].ToString(CultureInfo.InvariantCulture),
         Path.LongPathPrefix + SysDrive,
         Path.LongPathPrefix + SysDrive + @"\",
         Path.LongPathPrefix + SysDrive + @"\a",
         Path.LongPathPrefix + SysDrive + @"\a\",
         Path.LongPathPrefix + SysDrive + @"\a\b",
         Path.LongPathPrefix + SysDrive + @"\a\b\",
         Path.LongPathPrefix + SysDrive + @"\a\b\c",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.t",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.tx",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.txt",

         Path.UncPrefix + LocalHost + @"\Share",
         Path.UncPrefix + LocalHost + @"\Share\",
         Path.UncPrefix + LocalHost + @"\Share\d",
         Path.UncPrefix + LocalHost + @"\Share\d1",
         Path.UncPrefix + LocalHost + @"\Share\d1\",
         Path.UncPrefix + LocalHost + @"\Share\d1\d",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\f",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\fi",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\fil",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\file",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.e",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.ex",
         Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.ext",

         Path.LongPathUncPrefix + LocalHost + @"\Share",
         Path.LongPathUncPrefix + LocalHost + @"\Share\",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\f",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\fi",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\fil",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.e",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.ex",
         Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.ext"
      };

      #endregion // InputPaths

      #endregion // Fields

      #region Methods

      #region CreateDirectoriesAndFiles

      public static void CreateDirectoriesAndFiles(string rootPath, int max, bool recurse)
      {
         for (int i = 0; i < max; i++)
         {
            string file = Path.Combine(rootPath, Path.GetRandomFileName());
            string dir = file + "-" + i + "-dir";
            file = file + "-" + i + "-file";

            Directory.CreateDirectory(dir);

            // Some directories will remain empty.
            if (i % 2 != 0)
            {
               File.WriteAllText(file, UnitTestConstants.TextHelloWorld);
               File.WriteAllText(Path.Combine(dir, Path.GetFileName(file)), TextGoodByeWorld);
            }
         }

         if (recurse)
         {
            foreach (string dir in Directory.EnumerateDirectories(rootPath))
               CreateDirectoriesAndFiles(dir, max, false);
         }
      }

      #endregion // CreateDirectoriesAndFiles

      #region StopWatcher

      private static Stopwatch _stopWatcher;

      public static string StopWatcher(bool start = false)
      {
         if (_stopWatcher == null)
            _stopWatcher = new Stopwatch();

         if (start)
         {
            _stopWatcher.Restart();
            return null;
         }

         _stopWatcher.Stop();
         long ms = _stopWatcher.ElapsedMilliseconds;
         TimeSpan elapsed = _stopWatcher.Elapsed;

         return string.Format(CultureInfo.CurrentCulture, "*Duration: [{0}] ms. ({1})", ms, elapsed);
      }

      #endregion // StopWatcher

      #region Reporter

      public static string Reporter(bool onlyTime = false)
      {
         var lastError = new Win32Exception();

         UnitTestConstants.StopWatcher();

         return onlyTime
            ? string.Format(CultureInfo.CurrentCulture, "\t\t{0}", UnitTestConstants.StopWatcher())
            : string.Format(CultureInfo.CurrentCulture, "\t{0} [{1}: {2}]", UnitTestConstants.StopWatcher(), lastError.NativeErrorCode, lastError.Message);
      }

      #endregion // Reporter

      #region IsAdmin

      public static bool IsAdmin()
      {
         bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

         if (!isAdmin)
            Console.WriteLine("\nThis Unit Test must be run as Administrator.\n");

         return isAdmin;
      }

      #endregion // IsAdmin

      #region Dump

      /// <summary>Shows the Object's available Properties and Values.</summary>
      public static bool Dump(object obj, int width = -35, bool indent = false)
      {
         int cnt = 0;
         const string nulll = "\t\tnull";
         string template = "\t{0}#{1:000}\t{2, " + width + "} = [{3}]";

         if (obj == null)
         {
            Console.WriteLine(nulll);
            return false;
         }

         Console.WriteLine("\n\t{0}Instance: [{1}]\n", indent ? "\t" : "", obj.GetType().FullName);

         bool loopOk = false;
         foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj).Sort().Cast<PropertyDescriptor>().Where(descriptor => descriptor != null))
         {
            string propValue;
            try
            {
               object value = descriptor.GetValue(obj);
               propValue = (value == null) ? "null" : value.ToString();

               loopOk = true;
            }
            catch (Exception ex)
            {
               // Please do tell, oneliner preferably.
               propValue = ex.Message.Replace(Environment.NewLine, "  ");
            }

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }

         return loopOk;
      }

      #endregion //Dump

      #region StringToByteArray

      public static byte[] StringToByteArray(string str, params Encoding[] encoding)
      {
         Encoding encode = encoding != null && encoding.Any() ? encoding[0] : new UTF8Encoding(true, true);
         return encode.GetBytes(str);
      }

      #endregion // StringToByteArray

      #endregion // Methods
   }
}

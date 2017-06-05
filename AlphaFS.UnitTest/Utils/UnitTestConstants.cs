/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace AlphaFS.UnitTest
{
   /// <summary>Containts static variables, used by unit tests.</summary>
   public static class UnitTestConstants
   {
      #region Fields

      public const string Local = @"LOCAL";
      public const string Network = @"NETWORK";

      public const string EMspace = "\u3000";

      public static readonly string LocalHost = Environment.MachineName;
      public static readonly string LocalHostShare = Environment.SystemDirectory;

      public static readonly string SysDrive = Environment.GetEnvironmentVariable("SystemDrive");
      public static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      public static readonly string SysRoot32 = System.IO.Path.Combine(SysRoot, "System32");
      public static readonly string AppData = Environment.GetEnvironmentVariable("AppData");
      public static readonly string NotepadExe = System.IO.Path.Combine(SysRoot32, "notepad.exe");

      public const string TextTrue = "IsTrue";
      public const string TextFalse = "IsFalse";
      public const string TenNumbers = "0123456789";
      public const string TextHelloWorld = "Hëllõ Wørld!";
      public const string TextGoodbyeWorld = "Góödbyé Wôrld!";
      public const string TextUnicode = "ÛņïÇòdè; ǖŤƑ";

      private static Stopwatch _stopWatcher;

      private static readonly string RandomName = System.IO.Path.GetRandomFileName();
      public static readonly string MyStream = "ӍƔŞtrëƛɱ-" + RandomName;
      public static readonly string MyStream2 = "myStreamTWO-" + RandomName;
      public static readonly string[] AllStreams = {MyStream, MyStream2};
      public static readonly string StreamStringContent = "(1) Computer: [" + LocalHost + "]" + "\tHello there, " + Environment.UserName;
      public static readonly string[] StreamArrayContent =
      {
         "(1) The quick brown fox jumps over the lazy dog.",
         "(2) Albert Einstein: \"Science is a wonderful thing if one does not have to earn one's living at it.\"",
         "(3) " + TextHelloWorld + " " + TextUnicode
      };


      #region InputPaths

      public static readonly string[] InputPaths =
      {
         @".",
         @".zip",
         
         SysDrive + @"\\test.txt",
         SysDrive + @"\/test.txt",

         System.IO.Path.DirectorySeparatorChar.ToString(),
         System.IO.Path.DirectorySeparatorChar + @"Program Files\Microsoft Office",

         Alphaleonis.Win32.Filesystem.Path.GlobalRootPrefix + @"device\harddisk0\partition1\",
         Alphaleonis.Win32.Filesystem.Path.VolumePrefix + @"{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\Program Files\notepad.exe",

         "dir1/dir2/dir3/",

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

         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + @"Program Files\Microsoft Office",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive[0].ToString(CultureInfo.InvariantCulture),
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive,
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c\",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c\f",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c\f.",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c\f.t",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c\f.tx",
         Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + SysDrive + @"\a\b\c\f.txt",

         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\f",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\fi",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\fil",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\file",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.e",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.ex",
         Alphaleonis.Win32.Filesystem.Path.UncPrefix + LocalHost + @"\Share\d1\d2\file.ext",

         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\f",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\fi",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\fil",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.e",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.ex",
         Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + LocalHost + @"\Share\d1\d2\file.ext"
      };

      #endregion // InputPaths

      #endregion // Fields

      #region Methods

      // A high "max" increases the change of path too long.
      public static void CreateDirectoriesAndFiles(string rootPath, int max, bool recurse)
      {
         for (var i = 0; i < max; i++)
         {
            var file = System.IO.Path.Combine(rootPath, System.IO.Path.GetRandomFileName());
            var dir = file + "-" + i + "-dir";
            file = file + "-" + i + "-file";

            System.IO.Directory.CreateDirectory(dir);

            // Some directories will remain empty.
            if (i % 2 != 0)
            {
               //System.IO.File.WriteAllText(file, TextHelloWorld);
               CreateFile(dir);

               System.IO.File.WriteAllText(System.IO.Path.Combine(dir, System.IO.Path.GetFileName(file)), TextGoodbyeWorld);
            }
         }

         if (recurse)
         {
            foreach (var dir in System.IO.Directory.EnumerateDirectories(rootPath))
               CreateDirectoriesAndFiles(dir, max, false);
         }
      }


      public static System.IO.FileInfo CreateFile(string rootFolder, int fileLength = 0)
      {
         var file = System.IO.Path.Combine(rootFolder, System.IO.Path.GetRandomFileName());

         using (var fs = System.IO.File.Create(file))
         {
            if (fileLength <= 0)
               fileLength = new Random().Next(1, 10485760);

            fs.SetLength(fileLength);
         }

         return new System.IO.FileInfo(file);
      }


      public static void FolderDenyPermission(bool create, string tempPath)
      {
         var user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');

         var dirInfo = new System.IO.DirectoryInfo(tempPath);
         System.Security.AccessControl.DirectorySecurity dirSecurity;

         // ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
         // ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
         // ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
         // ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
         // ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
         // ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝

         var rule = new System.Security.AccessControl.FileSystemAccessRule(user,
            System.Security.AccessControl.FileSystemRights.FullControl,
            System.Security.AccessControl.InheritanceFlags.ContainerInherit |
            System.Security.AccessControl.InheritanceFlags.ObjectInherit,
            System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Deny);

         if (create)
         {
            dirInfo.Create();

            // Set DENY for current user.
            dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.AddAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);
         }
         else
         {
            // Remove DENY for current user.
            dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.RemoveAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);
         }
      }


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
         var ms = _stopWatcher.ElapsedMilliseconds;
         var elapsed = _stopWatcher.Elapsed;

         return string.Format(CultureInfo.CurrentCulture, "*Duration: [{0}] ms. ({1})", ms, elapsed);
      }


      public static string Reporter(bool onlyTime = false)
      {
         var lastError = new Win32Exception();

         StopWatcher();

         return onlyTime
            ? string.Format(CultureInfo.CurrentCulture, "\t\t{0}", StopWatcher())
            : string.Format(CultureInfo.CurrentCulture, "\t{0} [{1}: {2}]", StopWatcher(), lastError.NativeErrorCode, lastError.Message);
      }

      
      public static bool IsAdmin()
      {
         var isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

         if (!isAdmin)
            Console.WriteLine("\nThis Unit Test must be run as Administrator.\n");

         return isAdmin;
      }


      /// <summary>Shows the Object's available Properties and Values.</summary>
      public static bool Dump(object obj, int width = -35, bool indent = false)
      {
         var cnt = 0;
         const string nulll = "\t\tnull";
         var template = "\t{0}#{1:000}\t{2, " + width + "} = [{3}]";

         if (obj == null)
         {
            Console.WriteLine(nulll);
            return false;
         }

         Console.WriteLine("\n\t{0}Instance: [{1}]\n", indent ? "\t" : "", obj.GetType().FullName);

         var loopOk = false;
         foreach (var descriptor in TypeDescriptor.GetProperties(obj).Sort().Cast<PropertyDescriptor>().Where(descriptor => descriptor != null))
         {
            string propValue;
            try
            {
               var value = descriptor.GetValue(obj);
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


      public static byte[] StringToByteArray(string str, params Encoding[] encoding)
      {
         var encode = encoding != null && encoding.Any() ? encoding[0] : new UTF8Encoding(true, true);
         return encode.GetBytes(str);
      }


      public static void TestAccessRules(System.Security.AccessControl.ObjectSecurity sysIO, System.Security.AccessControl.ObjectSecurity alphaFS)
      {
         Console.WriteLine();


         Console.WriteLine("\tSystem.IO .AccessRightType: [{0}]", sysIO.AccessRightType);
         Console.WriteLine("\tAlphaFS   .AccessRightType: [{0}]", alphaFS.AccessRightType);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AccessRightType, alphaFS.AccessRightType);


         Console.WriteLine("\tSystem.IO .AccessRuleType: [{0}]", sysIO.AccessRuleType);
         Console.WriteLine("\tAlphaFS   .AccessRuleType: [{0}]", alphaFS.AccessRuleType);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AccessRuleType, alphaFS.AccessRuleType);


         Console.WriteLine("\tSystem.IO .AuditRuleType: [{0}]", sysIO.AuditRuleType);
         Console.WriteLine("\tAlphaFS   .AuditRuleType: [{0}]", alphaFS.AuditRuleType);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AuditRuleType, alphaFS.AuditRuleType);




         Console.WriteLine("\tSystem.IO .AreAccessRulesProtected: [{0}]", sysIO.AreAccessRulesProtected);
         Console.WriteLine("\tAlphaFS   .AreAccessRulesProtected: [{0}]", alphaFS.AreAccessRulesProtected);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAccessRulesProtected, alphaFS.AreAccessRulesProtected);


         Console.WriteLine("\tSystem.IO .AreAuditRulesProtected: [{0}]", sysIO.AreAuditRulesProtected);
         Console.WriteLine("\tAlphaFS   .AreAuditRulesProtected: [{0}]", alphaFS.AreAuditRulesProtected);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAuditRulesProtected, alphaFS.AreAuditRulesProtected);


         Console.WriteLine("\tSystem.IO .AreAccessRulesCanonical: [{0}]", sysIO.AreAccessRulesCanonical);
         Console.WriteLine("\tAlphaFS   .AreAccessRulesCanonical: [{0}]", alphaFS.AreAccessRulesCanonical);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAccessRulesCanonical, alphaFS.AreAccessRulesCanonical);


         Console.WriteLine("\tSystem.IO .AreAuditRulesCanonical: [{0}]", sysIO.AreAuditRulesCanonical);
         Console.WriteLine("\tAlphaFS   .AreAuditRulesCanonical: [{0}]", alphaFS.AreAuditRulesCanonical);
         Console.WriteLine();
         Assert.AreEqual(sysIO.AreAuditRulesCanonical, alphaFS.AreAuditRulesCanonical);
      }


      public static void PrintUnitTestHeader(bool isNetwork)
      {
         Console.WriteLine("\n=== TEST {0} ===", isNetwork ? Network : Local);
      }

      #endregion // Methods
   }
}

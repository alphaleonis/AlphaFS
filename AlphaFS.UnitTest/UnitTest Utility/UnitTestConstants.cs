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

using System;

namespace AlphaFS.UnitTest
{
   /// <summary>Containts static variables, used by unit tests.</summary>
   public static partial class UnitTestConstants
   {
#if NET35
      public const string EMspace = "\u3000";
#endif

      /// <summary>The User temp directory. For example "C:\Users\john\AppData\Local\Temp".</summary>
      public static readonly string TempFolder = Environment.GetEnvironmentVariable("Temp");

      
      /// <summary>The Computer Windows directory. For example "C:\Windows".</summary>
      public static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");

      
      /// <summary>The Computer System32 directory. For example "C:\Windows\System32".</summary>
      public static readonly string SysRoot32 = System.IO.Path.Combine(SysRoot, "System32");

      
      /// <summary>The User's app data directory. For example "C:\Users\john\AppData\Roaming".</summary>
      public static readonly string AppData = Environment.GetEnvironmentVariable("AppData");


      public static readonly string NotepadExe = System.IO.Path.Combine(SysRoot32, "notepad.exe");


      public const string TextTrue = "IsTrue";
      public const string TextFalse = "IsFalse";
      public const string TenNumbers = "0123456789";
      public const string TextHelloWorld = "Hëllõ Wørld!";
      public const string TextGoodbyeWorld = "Góödbyé Wôrld!";
      public const string TextUnicode = "ÛņïÇòdè; ǖŤƑ";

      public static readonly string MyStream = "ӍƔŞtrëƛɱ-" + GetRandomFileNameWithDiacriticCharacters();

      public static readonly string MyStream2 = "myStreamTWO-" + GetRandomFileNameWithDiacriticCharacters();

      public static readonly string[] AllStreams = {MyStream, MyStream2};

      public static readonly string StreamStringContent = "(1) Computer: [" + Environment.MachineName + "]" + "\tHello there, " + Environment.UserName;

      public static readonly string[] StreamArrayContent =
      {
         "(1) Nikolai Tesla: \"Today's scientists have substituted mathematics for experiments, and they wander off through equation after equation, and eventually build a structure which has no relation to reality.\"",
         "(2) The quick brown fox jumps over the lazy dog.",
         "(3) " + TextHelloWorld + " " + TextUnicode
      };
   }
}

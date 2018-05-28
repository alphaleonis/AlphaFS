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
using System.Diagnostics;

namespace AlphaFS.UnitTest
{
   /// <summary>Containts static variables, used by unit tests.</summary>
   public static partial class UnitTestConstants
   {
      #region Fields

      public const string Local = @"LOCAL";
      public const string Network = @"NETWORK";

#if NET35
      public const string EMspace = "\u3000";
#endif

      /// <summary>The path to the temporary folder, ending with a backslash.</summary>
      public static readonly string TempPath = System.IO.Path.GetTempPath();

      public static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      public static readonly string SysRoot32 = Environment.SystemDirectory;
      public static readonly string AppData = Environment.GetEnvironmentVariable("AppData");
      public static readonly string NotepadExe = System.IO.Path.Combine(SysRoot32, "notepad.exe");

      public const string TextTrue = "IsTrue";
      public const string TextFalse = "IsFalse";
      public const string TenNumbers = "0123456789";
      public const string TextHelloWorld = "Hëllõ Wørld!";
      public const string TextGoodbyeWorld = "Góödbyé Wôrld!";
      public const string TextUnicode = "ÛņïÇòdè; ǖŤƑ";

      #endregion // Fields
   }
}

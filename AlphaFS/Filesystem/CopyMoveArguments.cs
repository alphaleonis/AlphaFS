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

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Class with method arguments that are used by the Directory/File Copy and Move methods.</summary>

   internal class CopyMoveArguments : BaseArguments
   {
      internal string SourcePath { get; set; }


      internal string DestinationPath { get; set; }
      

      internal CopyOptions? CopyOptions { get; set; }


      internal MoveOptions? MoveOptions { get; set; }
      

      internal string SourcePathLp { get; set; }


      internal string DestinationPathLp { get; set; }


      internal bool CopyTimestamps { get; set; }


      internal bool IsCopy { get; set; }


      /// <summary>A Move action fallback using Copy + Delete.</summary>
      internal bool EmulateMove { get; set; }


      /// <summary>A file/folder will be deleted or renamed on Computer startup.</summary>
      internal bool DelayUntilReboot { get; set; }


      internal bool DeleteOnStartup { get; set; }


      internal CopyMoveProgressRoutine ProgressHandler { get; set; }


      internal object UserProgressData { get; set; }


      internal NativeMethods.NativeCopyMoveProgressRoutine Routine { get; set; }
   }
}

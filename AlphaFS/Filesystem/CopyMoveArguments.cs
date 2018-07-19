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
   /// <summary>
   /// 
   /// </summary>

   internal class CopyMoveArguments : BaseArguments
   {
      /// <summary>
      /// 
      /// </summary>
      public string SourcePath { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public string DestinationPath { get; set; }
      

      /// <summary>
      /// 
      /// </summary>
      public CopyOptions? CopyOptions { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public MoveOptions? MoveOptions { get; set; }
      

      /// <summary>
      /// 
      /// </summary>
      public string SourcePathLp { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public string DestinationPathLp { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public bool CopyTimestamps { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public bool IsCopy { get; set; }


      /// <summary>A Move action fallback using Copy + Delete.</summary>
      public bool EmulateMove { get; set; }


      /// <summary>A file/folder will be deleted or renamed on Computer startup.</summary>
      public bool DelayUntilReboot { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public bool DeleteOnStartup { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public CopyMoveProgressRoutine ProgressHandler { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public object UserProgressData { get; set; }


      /// <summary>
      /// 
      /// </summary>
      public NativeMethods.NativeCopyMoveProgressRoutine Routine { get; set; }
   }
}

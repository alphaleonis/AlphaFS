/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
   /// <summary>Used by CopyFileXxx and MoveFileXxx.
   /// <para>The <see cref="T:CopyMoveProgressRoutine"/> function should return one of the following values.</para>
   /// </summary>
   public enum CopyMoveProgressResult 
   {
      /// <summary>(0) PROGRESS_CONTINUE - Continue the copy operation.</summary>
      Continue = 0,

      /// <summary>(1) PROGRESS_CANCEL - Cancel the copy operation and delete the destination file.</summary>
      Cancel = 1,

      /// <summary>(2) PROGRESS_STOP - Stop the copy operation. It can be restarted at a later time.</summary>
      Stop = 2,

      /// <summary>(3) PROGRESS_QUIET - Continue the copy operation, but stop invoking <see cref="T:CopyMoveProgressRoutine"/> to report progress.</summary>
      Quiet = 3
   }
}
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
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      /// <summary>Gets the current working directory of the application.</summary>
      /// <returns>The path of the current working directory without a trailing directory separator.</returns>
      /// <remarks>
      ///   MSDN: Multithreaded applications and shared library code should not use the GetCurrentDirectory function and should avoid using relative path names.
      ///   The current directory state written by the SetCurrentDirectory function is stored as a global variable in each process,
      ///   therefore multithreaded applications cannot reliably use this value without possible data corruption from other threads that may also be reading or setting this value.
      ///   <para>This limitation also applies to the SetCurrentDirectory and GetFullPathName functions. The exception being when the application is guaranteed to be running in a single thread,
      ///   for example parsing file names from the command line argument string in the main thread prior to creating any additional threads.</para>
      ///   <para>Using relative path names in multithreaded applications or shared library code can yield unpredictable results and is not supported.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetCurrentDirectory()
      {
         return System.IO.Directory.GetCurrentDirectory();
      }

      /// <summary>Sets the application's current working directory to the specified directory.</summary>
      /// <param name="path">The path to which the current working directory is set.</param>
      /// <remarks>
      ///   MSDN: Multithreaded applications and shared library code should not use the GetCurrentDirectory function and should avoid using relative path names.
      ///   The current directory state written by the SetCurrentDirectory function is stored as a global variable in each process,
      ///   therefore multithreaded applications cannot reliably use this value without possible data corruption from other threads that may also be reading or setting this value.
      ///   <para>This limitation also applies to the SetCurrentDirectory and GetFullPathName functions. The exception being when the application is guaranteed to be running in a single thread,
      ///   for example parsing file names from the command line argument string in the main thread prior to creating any additional threads.</para>
      ///   <para>Using relative path names in multithreaded applications or shared library code can yield unpredictable results and is not supported.</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public static void SetCurrentDirectory(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathRp = Path.GetRegularPathCore(path, GetFullPathOptions.CheckInvalidPathChars, false);

         // System.IO SetCurrentDirectory() does not handle long paths.
         System.IO.Directory.SetCurrentDirectory(path.Length > 255 ? Path.GetShort83Path(pathRp) : pathRp);
      }
   }
}

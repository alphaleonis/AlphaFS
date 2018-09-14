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
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace AlphaFS.UnitTest
{
   #region CreateFile Types
   [Flags]
   enum EFileAccess : uint
   {
      //
      // Standart Section
      //

      AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
      MaximumAllowed = 0x2000000,     // MaximumAllowed access type

      Delete = 0x10000,
      ReadControl = 0x20000,
      WriteDAC = 0x40000,
      WriteOwner = 0x80000,
      Synchronize = 0x100000,

      StandardRightsRequired = 0xF0000,
      StandardRightsRead = ReadControl,
      StandardRightsWrite = ReadControl,
      StandardRightsExecute = ReadControl,
      StandardRightsAll = 0x1F0000,
      SpecificRightsAll = 0xFFFF,

      FILE_READ_DATA = 0x0001,        // file & pipe
      FILE_LIST_DIRECTORY = 0x0001,       // directory
      FILE_WRITE_DATA = 0x0002,       // file & pipe
      FILE_ADD_FILE = 0x0002,         // directory
      FILE_APPEND_DATA = 0x0004,      // file
      FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
      FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
      FILE_READ_EA = 0x0008,          // file & directory
      FILE_WRITE_EA = 0x0010,         // file & directory
      FILE_EXECUTE = 0x0020,          // file
      FILE_TRAVERSE = 0x0020,         // directory
      FILE_DELETE_CHILD = 0x0040,     // directory
      FILE_READ_ATTRIBUTES = 0x0080,      // all
      FILE_WRITE_ATTRIBUTES = 0x0100,     // all

      //
      // Generic Section
      //

      GenericRead = 0x80000000,
      GenericWrite = 0x40000000,
      GenericExecute = 0x20000000,
      GenericAll = 0x10000000,

      SPECIFIC_RIGHTS_ALL = 0x00FFFF,
      FILE_ALL_ACCESS =
      StandardRightsRequired |
      Synchronize |
      0x1FF,

      FILE_GENERIC_READ =
      StandardRightsRead |
      FILE_READ_DATA |
      FILE_READ_ATTRIBUTES |
      FILE_READ_EA |
      Synchronize,

      FILE_GENERIC_WRITE =
      StandardRightsWrite |
      FILE_WRITE_DATA |
      FILE_WRITE_ATTRIBUTES |
      FILE_WRITE_EA |
      FILE_APPEND_DATA |
      Synchronize,

      FILE_GENERIC_EXECUTE =
      StandardRightsExecute |
        FILE_READ_ATTRIBUTES |
        FILE_EXECUTE |
        Synchronize
   }

   [Flags]
   public enum EFileShare : uint
   {
      /// <summary>
      /// 
      /// </summary>
      None = 0x00000000,
      /// <summary>
      /// Enables subsequent open operations on an object to request read access. 
      /// Otherwise, other processes cannot open the object if they request read access. 
      /// If this flag is not specified, but the object has been opened for read access, the function fails.
      /// </summary>
      Read = 0x00000001,
      /// <summary>
      /// Enables subsequent open operations on an object to request write access. 
      /// Otherwise, other processes cannot open the object if they request write access. 
      /// If this flag is not specified, but the object has been opened for write access, the function fails.
      /// </summary>
      Write = 0x00000002,
      /// <summary>
      /// Enables subsequent open operations on an object to request delete access. 
      /// Otherwise, other processes cannot open the object if they request delete access.
      /// If this flag is not specified, but the object has been opened for delete access, the function fails.
      /// </summary>
      Delete = 0x00000004
   }

   public enum ECreationDisposition : uint
   {
      /// <summary>
      /// Creates a new file. The function fails if a specified file exists.
      /// </summary>
      New = 1,
      /// <summary>
      /// Creates a new file, always. 
      /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes, 
      /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
      /// </summary>
      CreateAlways = 2,
      /// <summary>
      /// Opens a file. The function fails if the file does not exist. 
      /// </summary>
      OpenExisting = 3,
      /// <summary>
      /// Opens a file, always. 
      /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
      /// </summary>
      OpenAlways = 4,
      /// <summary>
      /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
      /// The calling process must open the file with the GENERIC_WRITE access right. 
      /// </summary>
      TruncateExisting = 5
   }

   [Flags]
   public enum EFileAttributes : uint
   {
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000
   }
#endregion

   public partial class ExistsTest
   {
      #region CreateFile
      [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
      public static extern SafeFileHandle CreateFile(
         string lpFileName,
         uint dwDesiredAccess,
         uint dwShareMode,
         IntPtr SecurityAttributes,
         uint dwCreationDisposition,
         uint dwFlagsAndAttributes,
         IntPtr hTemplateFile
      );
      #endregion

      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_Exists_AfterDeletion_LocalAndNetwork_Success()
      {
         Directory_Exists_AfterDeletion(false);
         Directory_Exists_AfterDeletion(true);
      }


      private void Directory_Exists_AfterDeletion(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);

            //This simulates the behaviour of having a directory open in Windows Explorer and the deleting the directory
            //To obtain a handle to an existing directory, call the CreateFile function with the FILE_FLAG_BACKUP_SEMANTICS flag. https://docs.microsoft.com/en-us/windows/desktop/fileio/obtaining-a-handle-to-a-directory
            var ptr = CreateFile(folder.FullName, (uint)EFileAccess.GenericRead, (uint)EFileShare.Delete, IntPtr.Zero, (uint)ECreationDisposition.OpenAlways, (uint)EFileAttributes.BackupSemantics, IntPtr.Zero);

            folder.Delete();

            var existsSysIO = System.IO.Directory.Exists(folder.FullName);
            var existsAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(folder.FullName);

            ptr.Close();

            Assert.AreEqual(existsSysIO, existsAlpha);
         }

         Console.WriteLine();
      }
   }
}

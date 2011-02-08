using System;
using System.Collections.Generic;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// Contains information about files in the specified directory.    
   /// </summary>
   /// <seealso cref="Directory.GetFileIdBothDirectoryInfo(string,FileShare)"/>
   public class FileIdBothDirectoryInfo
   {
      internal FileIdBothDirectoryInfo(NativeMethods.FILE_ID_BOTH_DIR_INFO fibdi, string fileName)
      {
         System.Diagnostics.Debug.Assert(fileName != null, "fileName not expected to be null.");
         FileIndex = fibdi.FileIndex;
         CreationTime = fibdi.CreationTime.AsDateTime();
         LastAccessTime = fibdi.LastAccessTime.AsDateTime();
         LastWriteTime = fibdi.LastAccessTime.AsDateTime();
         ChangeTime = fibdi.ChangeTime.AsDateTime();
         EndOfFile = fibdi.EndOfFile;
         AllocationSize = fibdi.AllocationSize;
         FileAttributes = (FileAttributes)fibdi.FileAttributes;
         ExtendedAttributesSize = fibdi.EaSize;
         
         // ShortNameLength is the number of bytes in the short name, since we have a unicode string we must divide that by 2.
         ShortName = new string(fibdi.ShortName, 0, fibdi.ShortNameLength / 2);
         
         FileId = fibdi.FileId;
         FileName = fileName;
      }

      /// <summary>
      /// The byte offset of the file within the parent directory. 
      /// This member is undefined for file systems, such as NTFS, in which the position of a file within the parent directory is not fixed 
      /// and can be changed at any time to maintain sort order.
      /// </summary>
      public int FileIndex { get; set; }

      /// <summary>
      /// The time that the file was created.
      /// </summary>
      public DateTime CreationTime { get; set; }

      /// <summary>
      /// The time that the file was last accessed.
      /// </summary>
      public DateTime LastAccessTime { get; set; }

      /// <summary>
      /// The time that the file was last written to.
      /// </summary>
      public DateTime LastWriteTime { get; set; }

      /// <summary>
      /// The time that the file was last changed.
      /// </summary>
      public DateTime ChangeTime { get; set; }

      /// <summary>
      /// The absolute new end-of-file position as a byte offset from the start of the file to the end of the file. 
      /// Because this value is zero-based, it actually refers to the first free byte in the file. In other words, <b>EndOfFile</b> is the offset to 
      /// the byte that immediately follows the last valid byte in the file.
      /// </summary>
      public long EndOfFile { get; set; }

      /// <summary>
      /// The number of bytes that are allocated for the file. This value is usually a multiple of the sector or cluster size of the underlying physical device.
      /// </summary>
      public long AllocationSize { get; set; }

      /// <summary>
      /// The file attributes.
      /// </summary>
      public FileAttributes FileAttributes { get; set; }

      /// <summary>
      /// The size of the extended attributes for the file.
      /// </summary>
      public int ExtendedAttributesSize { get; set; }

      /// <summary>
      /// The short 8.3 file naming convention (for example, FILENAME.TXT) name of the file.
      /// </summary>
      public string ShortName { get; set; }

      /// <summary>
      /// The file ID.
      /// </summary>
      public long FileId { get; set; }

      /// <summary>
      /// The name of the file.
      /// </summary>
      public string FileName { get; set; }
   }
}

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
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wpd")]
   [Serializable]
   public sealed class WpdDirectoryInfo : WpdFileSystemInfo
   {
      #region Fields

      private string _name;

      #endregion // Fields


      #region Constructors

      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/> class on the specified path.</summary>
      /// <param name="fullName">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/>.</param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      public WpdDirectoryInfo(string fullName) : this(null, Path.GetFileName(Path.RemoveTrailingDirectorySeparator(fullName, false), false), fullName)
      {
      }


      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/> class on the specified path.</summary>
      /// <param name="objectId">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/>.</param>
      /// <param name="name"></param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      private WpdDirectoryInfo(string objectId, string name) : this(objectId, name, null)
      {
      }


      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/> class on the specified path.</summary>
      /// <param name="objectId">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/>.</param>
      /// <param name="name"></param>
      /// <param name="fullName"></param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      internal WpdDirectoryInfo(string objectId, string name, string fullName)
      {
         InitializeCore(true, objectId, fullName);

         _name = name;
      }

      
      /// <summary>[AlphaFS] Special internal implementation.</summary>
      /// <param name="fullName">The full path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.WpdDirectoryInfo"/>.</param>
      /// <param name="junk1">Not used.</param>
      /// <param name="junk2">Not used.</param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk1")]
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk2")]
      private WpdDirectoryInfo(string fullName, bool junk1, bool junk2)
      {
         IsDirectory = true;

         OriginalPath = Path.GetFileName(fullName, true);

         FullPath = fullName;

         DisplayPath = GetDisplayName(OriginalPath);
      }

      #endregion // Constructors
      

      #region Properties

      #region .NET

      /// <summary>Gets a value indicating whether the directory exists.</summary>
      /// <returns><c>true</c> if the directory exists; otherwise, <c>false</c>.</returns>
      public override bool Exists
      {
         //get { return (EntryInfo != null && EntryInfo.IsDirectory); }
         get { return true; }
      }


      /// <summary>Gets the name of this <see cref="T:WpdDirectoryInfo"/> instance.</summary>
      /// <returns>The directory name.</returns>
      /// <remarks>Returns only the name of the directory, such as "Bin". To get the full path, such as "c:\public\Bin", use the FullName property.</remarks>
      public override string Name
      {
         get { return _name; }
      }


      /// <summary>Gets the parent directory of a specified subdirectory.</summary>
      /// <returns>The parent directory, or <c>null</c> if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      public WpdDirectoryInfo Parent
      {
         get
         {
            var path = FullPath;

            if (path.Length > 3)
               path = Path.RemoveTrailingDirectorySeparator(FullPath, false);

            var dirName = Path.GetDirectoryName(path);

            return null != dirName ? new WpdDirectoryInfo(dirName, true, true) : null;
         }
      }


      /// <summary>Gets the root portion of the directory.</summary>
      /// <returns>An object that represents the root of the directory.</returns>
      public WpdDirectoryInfo Root
      {
         get
         {
            var root = Path.GetPathRoot(FullPath, false);

            if (Utils.IsNullOrWhiteSpace(root))
               root = Device.NativeMethods.WPD_DEVICE_OBJECT_ID;
            
            return new WpdDirectoryInfo(root, FullPath);
         }
      }

      #endregion // .NET

      #endregion // Properties


      #region Methods

      #region .NET

      /// <summary>Deletes this <see cref="T:PortableDeviceDirectoryInfo"/> if it is empty.</summary>
      [SecurityCritical]
      public override void Delete()
      {
         //Directory.DeleteDirectoryInternal(EntryInfo, null, null, false, false, true, false, null);
      }


      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public new void Refresh()
      {
         base.Refresh();
      }


      /// <summary>Returns the original path that was passed by the user.</summary>
      public override string ToString()
      {
         return DisplayPath;
      }

      #endregion // .NET


      #region AlphaFS

      private static string GetDisplayName(string path)
      {
         return path.Length != 2 || path[1] != Path.VolumeSeparatorChar ? path : Path.CurrentDirectoryPrefix;
      }

      #endregion // AlphaFS

      #endregion // Methods
   }
}
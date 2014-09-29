/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   #region AlphaFS

   /// <summary>Provides access to a file system object, using Shell32.</summary>
   public static class Shell32
   {
      #region Enum / Struct

      #region AssociationAttributes

      /// <summary>Provides information for the IQueryAssociations interface methods, used by Shell32.</summary>
      [Flags]
      internal enum AssociationAttributes
      {
         /// <summary>None.</summary>
         None = 0,

         /// <summary>Instructs not to map CLSID values to ProgID values.</summary>
         InitNoRemapClsid = 1,

         /// <summary>Identifies the value of the supplied file parameter (3rd parameter of function GetFileAssociation()) as an executable file name.</summary>
         /// <remarks>If this flag is not set, the root key will be set to the ProgID associated with the .exe key instead of the executable file's ProgID.</remarks>
         InitByExeName = 2,

         /// <summary>Specifies that when an IQueryAssociation method does not find the requested value under the root key, it should attempt to retrieve the comparable value from the * subkey.</summary>
         InitDefaultToStar = 4,

         /// <summary>Specifies that when an IQueryAssociation method does not find the requested value under the root key, it should attempt to retrieve the comparable value from the Folder subkey.</summary>
         InitDefaultToFolder = 8,

         /// <summary>Specifies that only HKEY_CLASSES_ROOT should be searched, and that HKEY_CURRENT_USER should be ignored.</summary>
         NoUserSettings = 16,

         /// <summary>Specifies that the return string should not be truncated. Instead, return an error value and the required size for the complete string.</summary>
         NoTruncate = 32,

         /// <summary>
         /// Instructs IQueryAssociations methods to verify that data is accurate.
         /// This setting allows IQueryAssociations methods to read data from the user's hard disk for verification.
         /// For example, they can check the friendly name in the registry against the one stored in the .exe file.
         /// </summary>
         /// <remarks>Setting this flag typically reduces the efficiency of the method.</remarks>
         Verify = 64,

         /// <summary>
         /// Instructs IQueryAssociations methods to ignore Rundll.exe and return information about its target.
         /// Typically IQueryAssociations methods return information about the first .exe or .dll in a command string.
         /// If a command uses Rundll.exe, setting this flag tells the method to ignore Rundll.exe and return information about its target.
         /// </summary>
         RemapRunDll = 128,

         /// <summary>Instructs IQueryAssociations methods not to fix errors in the registry, such as the friendly name of a function not matching the one found in the .exe file.</summary>
         NoFixUps = 256,

         /// <summary>Specifies that the BaseClass value should be ignored.</summary>
         IgnoreBaseClass = 512,

         /// <summary>Specifies that the "Unknown" ProgID should be ignored; instead, fail.</summary>
         /// <remarks>Introduced in Windows 7.</remarks>
         InitIgnoreUnknown = 1024,

         /// <summary>Specifies that the supplied ProgID should be mapped using the system defaults, rather than the current user defaults.</summary>
         /// <remarks>Introduced in Windows 8.</remarks>
         InitFixedProgId = 2048,

         /// <summary>Specifies that the value is a protocol, and should be mapped using the current user defaults.</summary>
         /// <remarks>Introduced in Windows 8.</remarks>
         IsProtocol = 4096
      }

      #endregion // AssociationAttributes

      #region AssociationData

      //internal enum AssociationData
      //{
      //   MsiDescriptor = 1,
      //   NoActivateHandler = 2 ,
      //   QueryClassStore = 3,
      //   HasPerUserAssoc = 4,
      //   EditFlags = 5,
      //   Value = 6
      //}

      #endregion // AssociationData

      #region AssociationKey

      //internal enum AssociationKey
      //{
      //   ShellExecClass = 1,
      //   App = 2,
      //   Class = 3,
      //   BaseClass = 4
      //}

      #endregion // AssociationKey

      #region AssociationString

      /// <summary>ASSOCSTR enumeration - Used by the AssocQueryString() function to define the type of string that is to be returned.</summary>
      internal enum AssociationString
      {
         /// <summary>None.</summary>
         None = 0,

         /// <summary>A command string associated with a Shell verb.</summary>
         Command = 1,

         /// <summary>
         /// An executable from a Shell verb command string.
         /// For example, this string is found as the (Default) value for a subkey such as HKEY_CLASSES_ROOT\ApplicationName\shell\Open\command.
         /// If the command uses Rundll.exe, set the <see cref="T:AssociationAttributes.RemapRunDll"/> flag in the attributes parameter of IQueryAssociations::GetString to retrieve the target executable.
         /// </summary>
         Executable = 2,

         /// <summary>The friendly name of a document type.</summary>
         FriendlyDocName = 3,

         /// <summary>The friendly name of an executable file.</summary>
         FriendlyAppName = 4,

         /// <summary>Ignore the information associated with the open subkey.</summary>
         NoOpen = 5,

         /// <summary>Look under the ShellNew subkey.</summary>
         ShellNewValue = 6,

         /// <summary>A template for DDE commands.</summary>
         DdeCommand = 7,

         /// <summary>The DDE command to use to create a process.</summary>
         DdeIfExec = 8,

         /// <summary>The application name in a DDE broadcast.</summary>
         DdeApplication = 9,

         /// <summary>The topic name in a DDE broadcast.</summary>
         DdeTopic = 10,

         /// <summary>
         /// Corresponds to the InfoTip registry value.
         /// Returns an info tip for an item, or list of properties in the form of an IPropertyDescriptionList from which to create an info tip, such as when hovering the cursor over a file name.
         /// The list of properties can be parsed with PSGetPropertyDescriptionListFromString.
         /// </summary>
         InfoTip = 11,

         /// <summary>
         /// Corresponds to the QuickTip registry value. This is the same as <see cref="T:InfoTip"/>, except that it always returns a list of property names in the form of an IPropertyDescriptionList.
         /// The difference between this value and <see cref="T:InfoTip"/> is that this returns properties that are safe for any scenario that causes slow property retrieval, such as offline or slow networks.
         /// Some of the properties returned from <see cref="T:InfoTip"/> might not be appropriate for slow property retrieval scenarios.
         /// The list of properties can be parsed with PSGetPropertyDescriptionListFromString.
         /// </summary>
         QuickTip = 12,

         /// <summary>
         /// Corresponds to the TileInfo registry value. Contains a list of properties to be displayed for a particular file type in a Windows Explorer window that is in tile view.
         /// This is the same as <see cref="T:InfoTip"/>, but, like <see cref="T:QuickTip"/>, it also returns a list of property names in the form of an IPropertyDescriptionList.
         /// The list of properties can be parsed with PSGetPropertyDescriptionListFromString.
         /// </summary>
         TileInfo = 13,

         /// <summary>
         /// Describes a general type of MIME file association, such as image and bmp,
         /// so that applications can make general assumptions about a specific file type.
         /// </summary>
         ContentType = 14,

         /// <summary>
         /// Returns the path to the icon resources to use by default for this association.
         /// Positive numbers indicate an index into the dll's resource table, while negative numbers indicate a resource ID.
         /// An example of the syntax for the resource is "c:\myfolder\myfile.dll,-1".
         /// </summary>
         DefaultIcon = 15,

         /// <summary>
         /// For an object that has a Shell extension associated with it,
         /// you can use this to retrieve the CLSID of that Shell extension object by passing a string representation
         /// of the IID of the interface you want to retrieve as the pwszExtra parameter of IQueryAssociations::GetString.
         /// For example, if you want to retrieve a handler that implements the IExtractImage interface,
         /// you would specify "{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}", which is the IID of IExtractImage.
         /// </summary>
         ShellExtension = 16,

         /// <summary>
         /// For a verb invoked through COM and the IDropTarget interface, you can use this flag to retrieve the IDropTarget object's CLSID.
         /// This CLSID is registered in the DropTarget subkey.
         /// The verb is specified in the supplied file parameter in the call to IQueryAssociations::GetString.
         /// </summary>
         DropTarget = 17,

         /// <summary>
         /// For a verb invoked through COM and the IExecuteCommand interface, you can use this flag to retrieve the IExecuteCommand object's CLSID.
         /// This CLSID is registered in the verb's command subkey as the DelegateExecute entry.
         /// The verb is specified in the supplied file parameter in the call to IQueryAssociations::GetString.
         /// </summary>
         DelegateExecute = 18,

         /// <summary>(No description available on MSDN)</summary>
         /// <remarks>Introduced in Windows 8.</remarks>
         SupportedUriProtocols = 19,

         /// <summary>The maximum defined <see cref="T:AssociationString"/> value, used for validation purposes.</summary>
         Max = 20
      }

      #endregion // AssociationString
      
      #region FileAttributes

      /// <summary>Shell32 FileAttributes structure, used to retrieve the different types of a file system object.</summary>
      [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
      [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
      [Flags]
      public enum FileAttributes
      {
         /// <summary>0x000000000 - Get file system object large icon.</summary>
         /// <remarks>The <see cref="T:Icon"/> flag must also be set.</remarks>
         LargeIcon = 0,

         /// <summary>0x000000001 - Get file system object small icon.</summary>
         /// <remarks>The <see cref="T:Icon"/> flag must also be set.</remarks>
         SmallIcon = 1,

         /// <summary>0x000000002 - Get file system object open icon.</summary>
         /// <remarks>A container object displays an open icon to indicate that the container is open.</remarks>
         /// <remarks>The <see cref="T:Icon"/> and/or <see cref="T:SysIconIndex"/> flag must also be set.</remarks>
         OpenIcon = 2,

         /// <summary>0x000000004 - Get file system object Shell-sized icon.</summary>
         /// <remarks>If this attribute is not specified the function sizes the icon according to the system metric values.</remarks>
         ShellIconSize = 4,

         /// <summary>0x000000008 - Get file system object by its PIDL.</summary>
         /// <remarks>Indicate that the given file contains the address of an ITEMIDLIST structure rather than a path name.</remarks>
         [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pidl")]
         Pidl = 8,

         /// <summary>0x000000010 - Indicates that the given file should not be accessed. Rather, it should act as if the given file exists and use the supplied attributes.</summary>
         /// <remarks>This flag cannot be combined with the <see cref="T:Attributes"/>, <see cref="T:ExeType"/> or <see cref="T:Pidl"/> attributes.</remarks>
         UseFileAttributes = 16,

         /// <summary>0x000000020 - Apply the appropriate overlays to the file's icon.</summary>
         /// <remarks>The <see cref="T:Icon"/> flag must also be set.</remarks>
         AddOverlays = 32,

         /// <summary>0x000000040 - Returns the index of the overlay icon.</summary>
         /// <remarks>The value of the overlay index is returned in the upper eight bits of the iIcon member of the structure specified by psfi.</remarks>
         OverlayIndex = 64,

         /// <summary>0x000000100 - Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the <see cref="T:FileInfo.IconHandle"/> member of the structure, and the index is copied to the <see cref="T:FileInfo.IconIndex"/> member.</summary>
         Icon = 256,

         /// <summary>0x000000200 - Retrieve the display name for the file. The name is copied to the <see cref="T:FileInfo.DisplayName"/> member of the structure.</summary>
         /// <remarks>The returned display name uses the long file name, if there is one, rather than the 8.3 form of the file name.</remarks>
         DisplayName = 512,

         /// <summary>0x000000400 - Retrieve the string that describes the file's type.</summary>
         TypeName = 1024,

         /// <summary>0x000000800 - Retrieve the item attributes. The attributes are copied to the <see cref="T:FileInfo.Attributes"/> member of the structure.</summary>
         /// <remarks>Will touch every file, degrading performance.</remarks>
         Attributes = 2048,

         /// <summary>0x000001000 - Retrieve the name of the file that contains the icon representing the file specified by pszPath. The name of the file containing the icon is copied to the <see cref="T:FileInfo.DisplayName"/> member of the structure.  The icon's index is copied to that structure's <see cref="T:FileInfo.IconIndex"/> member.</summary>
         IconLocation = 4096,

         /// <summary>0x000002000 - Retrieve the type of the executable file if pszPath identifies an executable file.</summary>
         /// <remarks>This flag cannot be specified with any other attributes.</remarks>
         ExeType = 8192,

         /// <summary>0x000004000 - Retrieve the index of a system image list icon.</summary>
         SysIconIndex = 16384,

         /// <summary>0x000008000 - Add the link overlay to the file's icon.</summary>
         /// <remarks>The <see cref="T:Icon"/> flag must also be set.</remarks>
         LinkOverlay = 32768,

         /// <summary>0x000010000 - Blend the file's icon with the system highlight color.</summary>
         Selected = 65536,

         /// <summary>0x000020000 - Modify <see cref="T:Attributes"/> to indicate that <see cref="T:FileInfo.Attributes"/> contains specific attributes that are desired.</summary>
         /// <remarks>This flag cannot be specified with the <see cref="T:Icon"/> attribute. Will touch every file, degrading performance.</remarks>
         AttributesSpecified = 131072
      }

      #endregion // FileAttributes

      #region FileInfo
      
      /// <summary>SHFILEINFO structure, contains information about a file system object.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Sh")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sh")]
      [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
      [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct FileInfo
      {
         /// <summary>A handle to the icon that represents the file.</summary>
         /// <remarks>Caller is responsible for destroying this handle with DestroyIcon() when no longer needed.</remarks>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
         public readonly IntPtr IconHandle;

         /// <summary>The index of the icon image within the system image list.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
         public int IconIndex;

         /// <summary>An array of values that indicates the attributes of the file object.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
         public readonly GetAttributesOf Attributes;

         /// <summary>The name of the file as it appears in the Windows Shell, or the path and file name of the file that contains the icon representing the file.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.MaxPath)]
         public string DisplayName;

         /// <summary>The type of file.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
         public string TypeName;
      }

      #endregion // FileInfo

      #region GetAttributesOf

      /// <summary>SFGAO - Attributes that can be retrieved from a file system object.</summary>
      [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Sh")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sh")]
      [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
      [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
      [Flags]
      public enum GetAttributesOf : uint
      {
         /// <summary>0x00000000 - None.</summary>
         None = 0,

         /// <summary>0x00000001 - The specified items can be copied.</summary>
         CanCopy = 1,

         /// <summary>0x00000002 - The specified items can be moved.</summary>
         CanMove = 2,

         /// <summary>0x00000004 - Shortcuts can be created for the specified items.</summary>
         CanLink = 4,

         /// <summary>0x00000008 - The specified items can be bound to an IStorage object through IShellFolder::BindToObject. For more information about namespace manipulation capabilities, see IStorage.</summary>
         Storage = 8,

         /// <summary>0x00000010 - The specified items can be renamed. Note that this value is essentially a suggestion; not all namespace clients allow items to be renamed. However, those that do must have this attribute set.</summary>
         CanRename = 16,

         /// <summary>0x00000020 - The specified items can be deleted.</summary>
         CanDelete = 32,

         /// <summary>0x00000040 - The specified items have property sheets.</summary>
         HasPropSheet = 64,

         /// <summary>0x00000100 - The specified items are drop targets.</summary>
         DropTarget = 256,

         /// <summary>0x00001000 - The specified items are system items.</summary>
         ///  <remarks>Windows 7 and later.</remarks>
         System = 4096,

         /// <summary>0x00002000 - The specified items are encrypted and might require special presentation.</summary>
         Encrypted = 8192,

         /// <summary>0x00004000 - Accessing the item (through IStream or other storage interfaces) is expected to be a slow operation.</summary>
         IsSlow = 16384,

         /// <summary>0x00008000 - The specified items are shown as dimmed and unavailable to the user.</summary>
         Ghosted = 32768,

         /// <summary>0x00010000 - The specified items are shortcuts.</summary>
         Link = 65536,

         /// <summary>0x00020000 - The specified objects are shared.</summary>
         Share = 131072,

         /// <summary>0x00040000 - The specified items are read-only. In the case of folders, this means that new items cannot be created in those folders.</summary>
         ReadOnly = 262144,

         /// <summary>0x00080000 - The item is hidden and should not be displayed unless the Show hidden files and folders option is enabled in Folder Settings.</summary>
         Hidden = 524288,

         /// <summary>0x00100000 - The items are nonenumerated items and should be hidden. They are not returned through an enumerator such as that created by the IShellFolder::EnumObjects method.</summary>
         NonEnumerated = 1048576,

         /// <summary>0x00200000 - The items contain new content, as defined by the particular application.</summary>
         NewContent = 2097152,

         /// <summary>0x00400000 - Indicates that the item has a stream associated with it.</summary>
         Stream = 4194304,

         /// <summary>0x00800000 - Children of this item are accessible through IStream or IStorage.</summary>
         StorageAncestor = 8388608,

         /// <summary>0x01000000 - When specified as input, instructs the folder to validate that the items contained in a folder or Shell item array exist.</summary>
         Validate = 16777216,

         /// <summary>0x02000000 - The specified items are on removable media or are themselves removable devices.</summary>
         Removable = 33554432,

         /// <summary>0x04000000 - The specified items are compressed.</summary>
         Compressed = 67108864,

         /// <summary>0x08000000 - The specified items can be hosted inside a web browser or Windows Explorer frame.</summary>
         Browsable = 134217728,

         /// <summary>0x10000000 - The specified folders are either file system folders or contain at least one descendant (child, grandchild, or later) that is a file system folder.</summary>
         FileSysAncestor = 268435456,

         /// <summary>0x20000000 - The specified items are folders.</summary>
         Folder = 536870912,

         /// <summary>0x40000000 - The specified folders or files are part of the file system (that is, they are files, directories, or root directories).</summary>
         FileSystem = 1073741824,

         /// <summary>0x80000000 - The specified folders have subfolders.</summary>
         [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubFolder")]
         HasSubFolder = 2147483648
      }

      #endregion // GetAttributesOf

      #region UrlType

      /// <summary>Used by method UrlIs() to define a URL type.</summary>
      internal enum UrlType
      {
         /// <summary>Is the URL valid?</summary>
         IsUrl = 0,

         /// <summary>Is the URL opaque?</summary>
         IsOpaque = 1,

         /// <summary>Is the URL a URL that is not typically tracked in navigation history?</summary>
         IsNoHistory = 2,

         /// <summary>Is the URL a file URL?</summary>
         IsFileUrl = 3,

         /// <summary>Attempt to determine a valid scheme for the URL.</summary>
         IsAppliable = 4,

         /// <summary>Does the URL string end with a directory?</summary>
         IsDirectory = 5,

         /// <summary>Does the URL have an appended query string?</summary>
         IsHasquery = 6
      }

      #endregion // UrlType

      #endregion // Enum / Struct

      #region Methods

      #region GetFileAssociation

      /// <summary>Gets the file or protocol that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related string from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileAssociation(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.Verify, AssociationString.Executable);
      }

      #endregion // GetFileAssociation
      
      #region GetFileContentType

      /// <summary>Gets the content-type that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related content-type from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileContentType(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.Verify, AssociationString.ContentType);
      }

      #endregion // GetFileContentType

      #region GetFileDefaultIcon

      /// <summary>Gets the default icon that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related default icon from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileDefaultIcon(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.Verify, AssociationString.DefaultIcon);
      }

      #endregion // GetFileDefaultIcon
      
      #region GetFileFriendlyAppName

      /// <summary>Gets the friendly application name that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related friendly application name from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileFriendlyAppName(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.InitByExeName, AssociationString.FriendlyAppName);
      }

      #endregion // GetFileFriendlyAppName

      #region GetFileFriendlyDocName

      /// <summary>Gets the friendly document name that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related friendly document name from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileFriendlyDocName(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.Verify, AssociationString.FriendlyDocName);
      }

      #endregion // GetFileFriendlyDocName

      #region GetFileIcon

      /// <summary>Gets an <see cref="T:IntPtr"/> handle to the Shell icon that represents the file.</summary>
      /// <param name="filePath">The path to the file system object which should not exceed <see cref="T:NativeMethods.MaxPath"/> in length. Both absolute and relative paths are valid.</param>
      /// <param name="iconAttributes">Icon size <see cref="T:Shell32.FileAttributes.SmallIcon"/> or <see cref="T:Shell32.FileAttributes.LargeIcon"/>. Can also be combined with <see cref="T:Shell32.FileAttributes.AddOverlays"/> and others.</param>
      /// <returns>An <see cref="T:IntPtr"/> handle to the Shell icon that represents the file, or IntPtr.Zero on failure.</returns>
      /// <remarks>Caller is responsible for destroying this handle with DestroyIcon() when no longer needed.</remarks>
      [SecurityCritical]
      public static IntPtr GetFileIcon(string filePath, FileAttributes iconAttributes)
      {
         if (Utils.IsNullOrWhiteSpace(filePath))
            return IntPtr.Zero;

         FileInfo fileInfo = GetFileInformationInternal(filePath, System.IO.FileAttributes.Normal, FileAttributes.Icon | iconAttributes, true);
         return fileInfo.IconHandle == IntPtr.Zero ? IntPtr.Zero : fileInfo.IconHandle;
      }

      #endregion // GetFileIcon

      #region GetFileInformation

      /// <summary>Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.</summary>
      /// <param name="filePath">The path to the file system object which should not exceed <see cref="T:NativeMethods.MaxPath"/> in length. Both absolute and relative paths are valid.</param>
      /// <param name="attributes">A <see cref="T:System.IO.FileAttributes"/> attribute.</param>
      /// <param name="fileAttributes">One ore more <see cref="T:FileAttributes"/> attributes.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>A <see cref="T:FileInfo"/> struct instance.</returns>
      /// <remarks>You should call this function from a background thread. Failure to do so could cause the UI to stop responding.</remarks>
      /// <remarks>LongPath is not supported.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileInfo GetFileInformation(string filePath, System.IO.FileAttributes attributes, FileAttributes fileAttributes, bool continueOnException)
      {
         return GetFileInformationInternal(filePath, attributes, fileAttributes, continueOnException);
      }

      #endregion // GetFileInformation

      #region GetShell32Information

      /// <summary></summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>A <see cref="T:Shell32Info"/> class instance.</returns>
      [SecurityCritical]
      public static Shell32Info GetShell32Information(string path)
      {
         return new Shell32Info(path);
      }

      #endregion // GetShell32Information

      #region GetFileOpenWithAppName

      /// <summary>Gets the "Open With" command that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related "Open With" command from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileOpenWithAppName(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.Verify, AssociationString.FriendlyAppName);
      }

      #endregion // GetFileOpenWithAppName

      #region GetFileVerbCommand

      /// <summary>Gets the Shell command that is associated with <paramref name="path"/> from the registry.</summary>
      /// <param name="path">A path to the file.</param>
      /// <returns>Returns the associated file- or protocol-related Shell command from the registry or <c>string.Empty</c> if no association can be found.</returns>
      [SecurityCritical]
      public static string GetFileVerbCommand(string path)
      {
         return GetFileAssociationInternal(path, AssociationAttributes.Verify, AssociationString.Command);
      }

      #endregion // GetFileVerbCommand

      #region PathCreateFromUrl

      /// <summary>Converts a file URL to a Microsoft MS-DOS path.</summary>
      /// <param name="urlPath">The file URL.</param>
      /// <returns>
      /// <para>The Microsoft MS-DOS path. If no path can be created, <c>string.Empty</c> is returned.</para>
      /// <para>If <paramref name="urlPath"/> is <c>null</c>, <c>null</c> will also be returned.</para>
      /// </returns>
      [SecurityCritical]
      internal static string PathCreateFromUrl(string urlPath)
      {
         if (urlPath == null)
            return null;

         StringBuilder buffer = new StringBuilder(NativeMethods.MaxPathUnicode);
         uint bufferSize = (uint) buffer.Capacity;

         uint lastError = NativeMethods.PathCreateFromUrl(urlPath, buffer, ref bufferSize, 0);

         // Don't throw exception, but return string.Empty;
         return lastError == Win32Errors.S_OK ? buffer.ToString() : string.Empty;
      }

      #endregion // PathCreateFromUrl

      #region PathCreateFromUrlAlloc

      /// <summary>Creates a path from a file URL.</summary>
      /// <param name="urlPath">The URL.</param>
      /// <returns>
      /// <para>The file path. If no path can be created, <c>string.Empty</c> is returned.</para>
      /// <para>If <paramref name="urlPath"/> is <c>null</c>, <c>null</c> will also be returned.</para>
      /// </returns>
      [SecurityCritical]
      internal static string PathCreateFromUrlAlloc(string urlPath)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         if (urlPath == null)
            return null;

         StringBuilder buffer;
         uint lastError = NativeMethods.PathCreateFromUrlAlloc(urlPath, out buffer, 0);

         // Don't throw exception, but return string.Empty;
         return lastError == Win32Errors.S_OK ? buffer.ToString() : string.Empty;
      }

      #endregion // PathCreateFromUrlAlloc

      #region PathFileExists

      /// <summary>Determines whether a path to a file system object such as a file or folder is valid.</summary>
      /// <param name="path">The full path of maximum length <see cref="T:NativeMethods.MaxPath"/> to the object to verify.</param>
      /// <returns><c>true</c> if the file exists; <c>false</c> otherwise</returns>
      [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastError")]
      [SecurityCritical]
      public static bool PathFileExists(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            return false;

         // PathFileExists()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         return NativeMethods.PathFileExists(Path.GetFullPathInternal(null, path, true, false, false, false));
      }

      #endregion // PathFileExists
      
      #region UrlIs

      /// <summary>Tests whether a URL is a specified type.</summary>
      /// <param name="url">The URL.</param>
      /// <param name="urlType"></param>
      /// <returns>
      /// For all but one of the URL types, UrlIs returns <c>true</c> if the URL is the specified type, or <c>false</c> otherwise.
      /// If UrlIs is set to <see cref="T:UrlType.IsAppliable"/>, UrlIs will attempt to determine the URL scheme.
      /// If the function is able to determine a scheme, it returns <c>true</c>, or <c>false</c> otherwise.
      /// </returns>
      [SecurityCritical]
      internal static bool UrlIs(string url, UrlType urlType)
      {
         return NativeMethods.UrlIs(url, urlType);
      }

      #endregion // UrlIs

      #region UrlCreateFromPath

      /// <summary>Converts a Microsoft MS-DOS path to a canonicalized URL.</summary>
      /// <param name="path">The full MS-DOS path of maximum length <see cref="T:NativeMethods.MaxPath"/>.</param>
      /// <returns>
      /// <para>The URL. If no URL can be created <c>string.Empty</c> is returned.</para>
      /// <para>If <paramref name="path"/> is <c>null</c>, <c>null</c> will also be returned.</para>
      /// </returns>
      [SecurityCritical]
      internal static string UrlCreateFromPath(string path)
      {
         if (path == null)
            return null;

         // UrlCreateFromPath does not support extended paths.
         string pathRp = Path.GetRegularPathInternal(path, true, false, false, false);

         StringBuilder buffer = new StringBuilder(NativeMethods.MaxPathUnicode);
         uint bufferSize = (uint) buffer.Capacity;

         uint lastError = NativeMethods.UrlCreateFromPath(pathRp, buffer, ref bufferSize, 0);

         // Don't throw exception, but return null;
         string url = buffer.ToString();
         if (Utils.IsNullOrWhiteSpace(url))
            url = string.Empty;

         return lastError == Win32Errors.S_OK ? url : string.Empty;
      }

      #endregion // UrlCreateFromPath

      #region UrlIsFileUrl

      /// <summary>Tests a URL to determine if it is a file URL.</summary>
      /// <param name="url">The URL.</param>
      /// <returns>Returns <c>true</c> if the URL is a file URL, or <c>false</c> otherwise.</returns>
      [SecurityCritical]
      internal static bool UrlIsFileUrl(string url)
      {
         return NativeMethods.UrlIs(url, UrlType.IsFileUrl);
      }

      #endregion // UrlIsFileUrl

      #region UrlIsNoHistory

      /// <summary>Returns whether a URL is a URL that browsers typically do not include in navigation history.</summary>
      /// <param name="url">The URL.</param>
      /// <returns>Returns <c>true</c> if the URL is a URL that is not included in navigation history, or <c>false</c> otherwise.</returns>
      [SecurityCritical]
      internal static bool UrlIsNoHistory(string url)
      {
         return NativeMethods.UrlIs(url, UrlType.IsNoHistory);
      }

      #endregion // UrlIsNoHistory

      #region UrlIsOpaque

      /// <summary>Returns whether a URL is opaque.</summary>
      /// <param name="url">The URL.</param>
      /// <returns>Returns <c>true</c> if the URL is opaque, or <c>false</c> otherwise.</returns>
      [SecurityCritical]
      internal static bool UrlIsOpaque(string url)
      {
         return NativeMethods.UrlIs(url, UrlType.IsOpaque);
      }

      #endregion // UrlIsOpaque


      #region Unified Internals

      #region GetFileAssociationInternal

      /// <summary>Unified method GetFileAssociationInternal() to search for and retrieves a file or protocol association-related string from the registry.</summary>
      /// <param name="path">A path to a file.</param>
      /// <param name="attributes">One or more <see cref="T:AssociationAttributes"/> attributes. Only one "InitXXX" attribute can be used.</param>
      /// <param name="associationType">A <see cref="T:AssociationString"/> attribute.</param>
      /// <returns>Returns the associated file- or protocol-related string from the registry or <c>string.Empty</c> if no association can be found.</returns>
      /// <exception cref="ArgumentNullException"></exception>
      [SecurityCritical]
      private static string GetFileAssociationInternal(string path, AssociationAttributes attributes, AssociationString associationType)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         attributes = attributes | AssociationAttributes.NoTruncate | AssociationAttributes.RemapRunDll;

         uint bufferSize = NativeMethods.MaxPath;
         StringBuilder buffer;
         uint retVal;

         do
         {
            buffer = new StringBuilder((int)bufferSize);

            // AssocQueryString()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-02-05: MSDN does not confirm LongPath usage but a Unicode version of this function exists.
            // However, the function fails when using LongPath notation.

            retVal = NativeMethods.AssocQueryString(attributes, associationType, path, null, buffer, out bufferSize);

            // No Exception is thrown, just return empty string on error.

            //switch (retVal)
            //{
            //   // 0x80070483: No application is associated with the specified file for this operation.
            //   case 2147943555:
            //   case Win32Errors.E_POINTER:
            //   case Win32Errors.S_OK:
            //      break;

            //   default:
            //      NativeError.ThrowException(retVal);
            //      break;
            //}

         } while (retVal == Win32Errors.E_POINTER);

         return buffer.ToString();
      }

      #endregion // GetFileAssociationInternal

      #region GetFileInformationInternal

      /// <summary>Unified method GetFileInformationInternal() to retrieve information about an object in the file system, such as a file, folder, directory, or drive root.</summary>
      /// <param name="path">The path to the file system object which should not exceed <see cref="T:NativeMethods.MaxPath"/> in length. Both absolute and relative paths are valid.</param>
      /// <param name="attributes">A <see cref="T:System.IO.FileAttributes"/> attribute.</param>
      /// <param name="fileAttributes">A <see cref="T:FileAttributes"/> attribute.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>A <see cref="T:FileInfo"/> struct instance.</returns>
      /// <remarks>You should call this function from a background thread. Failure to do so could cause the UI to stop responding.</remarks>
      /// <remarks>LongPaths not supported.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static FileInfo GetFileInformationInternal(string path, System.IO.FileAttributes attributes, FileAttributes fileAttributes, bool continueOnException)
      {
         // Prevent possible crash.
         FileInfo fileInfo = new FileInfo
         {
            DisplayName = string.Empty,
            TypeName = string.Empty,
            IconIndex = 0
         };

         if (!Utils.IsNullOrWhiteSpace(path))
         {
            // ShGetFileInfo()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.
            // However, the function fails when using LongPath notation.

            IntPtr shGetFileInfo = NativeMethods.ShGetFileInfo(Path.GetRegularPathInternal(path, true, false, false, false), attributes, out fileInfo, (uint)Marshal.SizeOf(fileInfo), fileAttributes);

            if (shGetFileInfo == IntPtr.Zero && !continueOnException)
               NativeError.ThrowException(Marshal.GetLastWin32Error(), path);
         }

         return fileInfo;
      }

      #endregion // GetFileInformationInternal

      #endregion // Unified Internals

      #endregion // Methods
   }

   #endregion // AlphaFS
}
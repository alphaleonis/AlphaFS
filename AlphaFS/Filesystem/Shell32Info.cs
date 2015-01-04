/* Copyright 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.IO;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Contains Shell32 information about a file.</summary>
   [SerializableAttribute]
   [SecurityCritical]
   public sealed class Shell32Info
   {
      #region Constructors

      /// <summary>Initializes a Shell32Info instance.
      /// <remarks>Shell32 is limited to MAX_PATH length.</remarks>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      /// </summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      public Shell32Info(string fileName) : this(fileName, false)
      {
      }

      /// <summary>Initializes a Shell32Info instance.
      /// <remarks>Shell32 is limited to MAX_PATH length.</remarks>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      /// </summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="isFullPath">
      /// <para><see langword="true"/> <paramref name="fileName"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><see langword="false"/> <paramref name="fileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><see langword="null"/> <paramref name="fileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      public Shell32Info(string fileName, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException("fileName");

         // Shell32 is limited to MAX_PATH length.
         // Get a full path of regular format.

         FullPath = isFullPath == null
            ? fileName
            : (bool) isFullPath
               ? Path.GetRegularPathInternal(fileName, false, false, true, false)
               : Path.GetFullPathInternal(null, fileName, false, false, false, true, false, true, true);

         Initialize();
      }

      #endregion // Constructors

      #region Methods

      #region GetIcon

      /// <summary>Gets an <see cref="IntPtr"/> handle to the Shell icon that represents the file.</summary>
      /// <param name="iconAttributes">Icon size <see cref="Shell32.FileAttributes.SmallIcon"/> or <see cref="Shell32.FileAttributes.LargeIcon"/>. Can also be combined with <see cref="Shell32.FileAttributes.AddOverlays"/> and others.</param>
      /// <returns>An <see cref="IntPtr"/> handle to the Shell icon that represents the file.</returns>
      /// <remarks>Caller is responsible for destroying this handle with DestroyIcon() when no longer needed.</remarks>
      [SecurityCritical]
      public IntPtr GetIcon(Shell32.FileAttributes iconAttributes)
      {
         return Shell32.GetFileIcon(FullPath, iconAttributes);
      }

      #endregion // GetIcon

      #region GetVerbCommand

      /// <summary>Gets the Shell command association from the registry.</summary>
      /// <param name="shellVerb">The shell verb.</param>
      /// <returns>
      ///   Returns the associated file- or protocol-related Shell command from the registry or <c>string.Empty</c> if no association can be
      ///   found.
      /// </returns>
      [SecurityCritical]
      public string GetVerbCommand(string shellVerb)
      {
         return GetString(_iQaNone, Shell32.AssociationString.Command, shellVerb);
      }

      #endregion // GetVerbCommand

      #region GetString

      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private static string GetString(NativeMethods.IQueryAssociations iQa, Shell32.AssociationString assocString, string shellVerb)
      {
         // GetString() throws Exceptions.
         try
         {
            // Use a large buffer to prevent calling this function twice.
            int size = NativeMethods.DefaultFileBufferSize;
            StringBuilder buffer = new StringBuilder(size);

            iQa.GetString(Shell32.AssociationAttributes.NoTruncate | Shell32.AssociationAttributes.RemapRunDll, assocString, shellVerb, buffer, out size);

            return buffer.ToString();
         }
         catch
         {
            return string.Empty;
         }
      }

      #endregion // GetString

      #region Initialize

      private NativeMethods.IQueryAssociations _iQaNone;    // Retrieve info from Shell.
      private NativeMethods.IQueryAssociations _iQaByExe;   // Retrieve info from exe file.

      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private void Initialize()
      {
         if (Initialized)
            return;

         Guid iidIQueryAssociations = new Guid(NativeMethods.QueryAssociationsGuid);

         if (NativeMethods.AssocCreate(NativeMethods.ClsidQueryAssociations, ref iidIQueryAssociations, out _iQaNone) == Win32Errors.S_OK)
         {
            try
            {
               _iQaNone.Init(Shell32.AssociationAttributes.None, FullPath, IntPtr.Zero, IntPtr.Zero);

               if (NativeMethods.AssocCreate(NativeMethods.ClsidQueryAssociations, ref iidIQueryAssociations, out _iQaByExe) == Win32Errors.S_OK)
               {
                  _iQaByExe.Init(Shell32.AssociationAttributes.InitByExeName, FullPath, IntPtr.Zero, IntPtr.Zero);

                  Initialized = true;
               }
            }
            catch
            {
            }
         }
      }

      #endregion // Initialize

      #region Refresh

      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public void Refresh()
      {
         Association = Command = ContentType = DdeApplication = DefaultIcon = FriendlyAppName = FriendlyDocName = OpenWithAppName = null;
         Attributes = Shell32.GetAttributesOf.None;
         Initialized = false;
         Initialize();
      }

      #endregion // Refresh

      #region ToString

      /// <summary>Returns the path as a string.</summary>
      /// <returns>The path.</returns>      
      public override string ToString()
      {
         return FullPath;
      }

      #endregion // ToString

      #endregion // Methods

      #region Properties

      #region Association

      private string _association;

      /// <summary>Gets the Shell file or protocol association from the registry.</summary>
      public string Association
      {
         get
         {
            if (_association == null)
               _association = GetString(_iQaNone, Shell32.AssociationString.Executable, null);

            return _association;
         }

         private set { _association = value; }
      }

      #endregion // Association

      #region Attributes
      
      private Shell32.GetAttributesOf _attributes;

      /// <summary>The attributes of the file object.</summary>
      public Shell32.GetAttributesOf Attributes
      {
         get
         {
            if (_attributes == Shell32.GetAttributesOf.None)
            {
               Shell32.FileInfo fileInfo = Shell32.GetFileInfoInternal(FullPath, FileAttributes.Normal, Shell32.FileAttributes.Attributes, false, true);
               _attributes = fileInfo.Attributes;
            }

            return _attributes;
         }

         private set { _attributes = value; }
      }

      #endregion // Attributes

      #region Command

      private string _command;

      /// <summary>Gets the Shell command association from the registry.</summary>
      public string Command
      {
         get
         {
            if (_command == null)
               _command = GetString(_iQaNone, Shell32.AssociationString.Command, null);

            return _command;
         }

         private set { _command = value; }
      }

      #endregion // Command

      #region ContentType

      private string _contentType;

      /// <summary>Gets the Shell command association from the registry.</summary>
      public string ContentType
      {
         get
         {
            if (_contentType == null)
               _contentType = GetString(_iQaNone, Shell32.AssociationString.ContentType, null);

            return _contentType;
         }

         private set { _contentType = value; }
      }

      #endregion // ContentType

      #region DdeApplication

      private string _ddeApplication;

      /// <summary>Gets the Shell DDE association from the registry.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dde")]
      public string DdeApplication
      {
         get
         {
            if (_ddeApplication == null)
               _ddeApplication = GetString(_iQaNone, Shell32.AssociationString.DdeApplication, null);

            return _ddeApplication;
         }

         private set { _ddeApplication = value; }
      }

      #endregion // DdeApplication

      #region DefaultIcon

      private string _defaultIcon;

      /// <summary>Gets the Shell default icon association from the registry.</summary>
      public string DefaultIcon
      {
         get
         {
            if (_defaultIcon == null)
               _defaultIcon = GetString(_iQaNone, Shell32.AssociationString.DefaultIcon, null);

            return _defaultIcon;
         }

         private set { _defaultIcon = value; }
      }

      #endregion // DefaultIcon

      #region FullPath

      /// <summary>Represents the fully qualified path of the file.</summary>
      public string FullPath { get; private set; }

      #endregion // FullPath

      #region FriendlyAppName

      private string _friendlyAppName;

      /// <summary>Gets the Shell friendly application name association from the registry.</summary>
      public string FriendlyAppName
      {
         get
         {
            if (_friendlyAppName == null)
               _friendlyAppName = GetString(_iQaByExe, Shell32.AssociationString.FriendlyAppName, null);

            return _friendlyAppName;
         }

         private set { _friendlyAppName = value; }
      }

      #endregion // FriendlyAppName

      #region FriendlyDocName

      private string _friendlyDocName;

      /// <summary>Gets the Shell friendly document name association from the registry.</summary>
      public string FriendlyDocName
      {
         get
         {
            if (_friendlyDocName == null)
               _friendlyDocName = GetString(_iQaNone, Shell32.AssociationString.FriendlyDocName, null);

            return _friendlyDocName;
         }

         private set { _friendlyDocName = value; }
      }

      #endregion // FriendlyDocName
      
      #region Initialized

      /// <summary>Reflects the initialization state of the instance.</summary>
      internal bool Initialized { get; set; }

      #endregion // Initialized

      #region OpenWithAppName

      private string _openWithAppName;

      /// <summary>Gets the Shell "Open With" command association from the registry.</summary>
      public string OpenWithAppName
      {
         get
         {
            if (_openWithAppName == null)
               _openWithAppName = GetString(_iQaNone, Shell32.AssociationString.FriendlyAppName, null);

            return _openWithAppName;
         }

         private set { _openWithAppName = value; }
      }

      #endregion // OpenWithAppName
      
      #endregion // Properties
   }
}
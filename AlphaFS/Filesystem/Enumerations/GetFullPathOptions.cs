using Alphaleonis.Win32.Network;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>A bitfield of flags for specifying options for various internal operation that converts path to full paths.</summary>
   [Flags]
   internal enum GetFullPathOptions
   {
      /// <summary>
      /// No special options applies.
      /// </summary>
      None = 0,

      /// <summary>
      /// Remove any trailing whitespace from the path.
      /// </summary>
      TrimEnd = 1,

      /// <summary>
      /// Add a trailing directory separator to the path (if one does not already exist).
      /// </summary>
      AddTrailingDirectorySeparator = 2,

      /// <summary>
      /// Remove the trailing directory separator from the path (if one exists).
      /// </summary>
      RemoveTrailingDirectorySeparator = 4,

      /// <summary>
      /// Prevents and exception from being thrown if a filesystem object does not exist.
      /// </summary>
      ContinueOnNonExist = 8,

      /// <summary>
      /// Check that the path contains only valid path-characters.
      /// </summary>
      CheckInvalidPathChars = 16,

      /// <summary>
      /// Also check for wildcard (? and *) characters.
      /// </summary>
      CheckAdditional = 32,
   }
}

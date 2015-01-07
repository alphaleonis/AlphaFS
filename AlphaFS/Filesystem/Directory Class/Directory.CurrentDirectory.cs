using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>Gets the current working directory of the application.</summary>
      /// <returns>The path of the current working directory without a trailing directory separator.</returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetCurrentDirectory()
      {
         return System.IO.Directory.GetCurrentDirectory();
      }

      /// <summary>Sets the application's current working directory to the specified directory.</summary>
      /// <param name="path">The path to which the current working directory is set.</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public static void SetCurrentDirectory(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathRp = Path.GetRegularPathInternal(path, false, false, false, true);

         // System.IO SetCurrentDirectory() does not handle long paths.
         System.IO.Directory.SetCurrentDirectory(path.Length > 255 ? Path.GetShort83Path(pathRp) : pathRp);
      }
   }
}

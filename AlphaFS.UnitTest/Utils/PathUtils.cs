using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaFS.UnitTest
{
   static class PathUtils
   {
      public static string AsUncPath(string localPath)
      {
         localPath = Path.GetFullPath(localPath);
         if (!Path.IsLocalPath(localPath))
            throw new ArgumentException("Path is not a local path.");
         return "\\\\" + Environment.MachineName + "\\" + localPath.First() + "$\\" + localPath.Substring(Path.GetPathRoot(localPath).Length);
      }
   }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlphaFS.UnitTest
{
   /// <summary>Used to create a temporary directory that will be deleted once this instance is disposed.</summary>
   sealed class TemporaryDirectory : IDisposable
   {
      private readonly DirectoryInfo m_directory;

      public TemporaryDirectory(string prefix)
      {
         if (String.IsNullOrEmpty(prefix))
            prefix = "AlphaFS";

         string root = Path.GetTempPath();
         do
         {
            m_directory = new DirectoryInfo(Path.Combine(root, prefix + "-" + Guid.NewGuid().ToString("N").Substring(0, 6)));
         }
         while (m_directory.Exists);

         m_directory.Create();
      }

      ~TemporaryDirectory()
      {
         Dispose(false);
      }

      public void Dispose()
      {
         GC.SuppressFinalize(this);
         Dispose(true);
      }

      private void Dispose(bool isDisposing)
      {
         try
         {
            m_directory.Delete(true);
         }
         catch (Exception ex)
         {
            Console.WriteLine("Failed to delete directory {0}: {1}", m_directory, ex.Message);
         }
      }

      public DirectoryInfo Directory
      {
         get
         {
            return m_directory;
         }
      }
   }
}

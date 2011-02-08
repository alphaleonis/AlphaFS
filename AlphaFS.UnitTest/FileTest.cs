using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Win32;
using System.Collections.Generic;

namespace AlphaFS.UnitTest
{
    /// <summary>
    ///This is a test class for FileTest and is intended
    ///to contain all FileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Non transacted

        /// <summary>
        ///A test for GetAttributes
        ///</summary>
        [TestMethod()]
        public void GetAttributesFromLockedSystemPagingFiles()
        {

            RegistryKey memManagementKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management");
            if (memManagementKey != null)
            {
                List<string> files = new List<string>();

                object value = memManagementKey.GetValue("PagingFiles");

                switch (memManagementKey.GetValueKind("PagingFiles"))
                {
                    case RegistryValueKind.Binary:
                        break;
                    case RegistryValueKind.DWord:
                        break;
                    case RegistryValueKind.ExpandString:
                        break;
                    case RegistryValueKind.MultiString:
                        files.AddRange((string[])value);
                        break;
                    case RegistryValueKind.None:
                        break;
                    case RegistryValueKind.QWord:
                        break;
                    case RegistryValueKind.String:
                        files.Add((string)value);
                        break;
                    case RegistryValueKind.Unknown:
                        break;
                    default:
                        break;
                }

                foreach (string file in files)
                {
                    // sanitize locations from ending size parameters
                    string path = file.Substring(0, file.LastIndexOf(' '));
                    path = path.Substring(0, path.LastIndexOf(' '));

                    if (File.Exists(path))
                    {
                        FileAttributes expected = FileAttributes.System | FileAttributes.Archive | FileAttributes.Hidden; // TODO: Initialize to an appropriate value
                        FileAttributes actual = File.GetAttributes(path);
                        Assert.AreEqual(expected, actual);
                        Assert.AreEqual(System.IO.File.GetCreationTimeUtc(path), File.GetCreationTimeUtc(path));
                    }
                }
            }
        }

        /// <summary>
        /// Setting and getting attributes from temporary file
        /// </summary>
        [TestMethod()]
        public void GetSetAttributesFromRegularFile()
        {
            string tempFile = Path.GetLongPath(Path.GetTempFileName());
            FileAttributes expected = FileAttributes.Archive | FileAttributes.Temporary | FileAttributes.Offline;
            File.SetAttributes(tempFile, expected);
            FileAttributes actual = File.GetAttributes(tempFile);
            File.Delete(tempFile);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Comparing attributes with .NET implementation
        /// </summary>
        [TestMethod()]
        public void GetAttributesFromRootDrives()
        {
            string[] localDrives = System.IO.Directory.GetLogicalDrives();

            foreach (string drive in localDrives)
            {
                try
                {
                    System.IO.FileAttributes netAttributes = System.IO.File.GetAttributes(drive);

                    FileAttributes alphaAttributes = File.GetAttributes(drive);

                    Assert.AreEqual(netAttributes.ToString(), alphaAttributes.ToString());
                }
                catch
                {
                    // do nothing for drives that are not initialized
                }
            }
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            string path = string.Empty; // TODO: Initialize to an appropriate value
            bool overrideReadOnly = false; // TODO: Initialize to an appropriate value
            File.Delete(path, overrideReadOnly);
            Assert.IsFalse(File.Exists(path));
        }

        #endregion

        #region Transacted

        #endregion
    }
}

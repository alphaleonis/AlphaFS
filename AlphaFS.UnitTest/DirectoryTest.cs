using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.AccessControl;

namespace AlphaFS.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for DirectoryTest and is intended
    ///to contain all DirectoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DirectoryTest
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
         
        //You can use the following additional attributes as you write your tests:
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }
        
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


        ///// <summary>
        /////A test for CreateDirectory
        /////</summary>
        //[TestMethod()]
        //public void CreateDirectoryTransactedTest()
        //{
        //    KernelTransaction transaction = null; // TODO: Initialize to an appropriate value
        //    string templatePathName = string.Empty; // TODO: Initialize to an appropriate value
        //    string newPathName = string.Empty; // TODO: Initialize to an appropriate value
        //    DirectorySecurity security = null; // TODO: Initialize to an appropriate value
        //    Directory.CreateDirectory(transaction, templatePathName, newPathName, security);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}


        /// <summary>
        ///A test for CreateDirectory
        ///</summary>
        [TestMethod()]
        public void CreateDirectoryNonTransactedTest()
        {
            string templatePathName = Path.GetTempPath(); // TODO: Initialize to an appropriate value
            string newPathName = Path.GetLongPath(Path.Combine(Path.GetTempPath(), TestContext.TestName)); // TODO: Initialize to an appropriate value
            string subDirName = Path.GetLongPath(Path.Combine(newPathName, @"one level\two levels\three levels\four levels"));
            DirectorySecurity security = new DirectorySecurity(templatePathName, AccessControlSections.Access); // TODO: Initialize to an appropriate value
            Directory.CreateDirectory(templatePathName, subDirName, security);
            Directory.CreateDirectory(templatePathName, newPathName, security);
            System.IO.Directory.CreateDirectory(Path.GetRegularPath(newPathName));
            Assert.IsTrue(Directory.Exists(subDirName));
            Directory.Delete(newPathName, true, true);
            Assert.IsFalse(Directory.Exists(newPathName));
        }
    }
}

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for PathTest and is intended
    ///to contain all PathTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PathTest
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


        /// <summary>
        ///A test for HasExtension
        ///</summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
        public void PathHasExtensionTest()
        {
            string input = TestContext.DataRow["Input"] as string;
            bool actual = Path.HasExtension(input);
            bool expected = bool.Parse(TestContext.DataRow["HasExtension"] as string);
            Assert.AreEqual(expected, actual, String.Format("HasExtension of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
        }

        /// <summary>
        ///A test for GetFileNameWithoutExtension
        ///</summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
        public void PathGetFileNameWithoutExtensionTest()
        {
            string input = TestContext.DataRow["Input"] as string;
            string actual = Path.GetFileNameWithoutExtension(input);
            string expected = TestContext.DataRow["FileNameWithoutExtension"] as string;
            Assert.AreEqual(expected, actual, String.Format("FileNameWithoutExtension of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));

        }

        /// <summary>
        ///A test for GetFileName
        ///</summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
        public void PathGetFileNameTest()
        {
            string input = TestContext.DataRow["Input"] as string;
            string actual = Path.GetFileName(input);
            string expected = TestContext.DataRow["FileName"] as string;
            Assert.AreEqual(expected, actual, String.Format("FileName of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
        }

        /// <summary>
        ///A test for GetExtension
        ///</summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
        public void PathGetExtensionTest()
        {
            string input = TestContext.DataRow["Input"] as string;
            string actual = Path.GetExtension(input);
            string expected = TestContext.DataRow["Extension"] as string;
            Assert.AreEqual(expected, actual, String.Format("Extension of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
        }
    }
}

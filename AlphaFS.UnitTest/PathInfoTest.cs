using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alphaleonis.Win32.Filesystem;

namespace AlphaFS.UnitTest
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class PathInfoTest
	{
		#region Setup

		public PathInfoTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

	    /// <summary>
	    ///Gets or sets the test context which provides
	    ///information about and functionality for the current test run.
	    ///</summary>
	    public TestContext TestContext { get; set; }

	    [TestInitialize]
		public void Init()
		{
			
		}

		#endregion

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoRootExtractionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).Root;
			string expected = TestContext.DataRow["Root"] as string;
			Assert.AreEqual(expected, actual, String.Format("Root of {0} was incorrect", input));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoDirectoryNameExtractionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).DirectoryName;
			string expected = TestContext.DataRow["DirectoryName"] as string;
			Assert.AreEqual(expected, actual, String.Format("DirectoryName of {0} was incorrect", input));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoDirectoryNameWithoutRootExtractionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).DirectoryNameWithoutRoot;
			string expected = TestContext.DataRow["DirectoryNameWithoutRoot"] as string;
			Assert.AreEqual(expected, actual, String.Format("DirectoryNameWithoutRoot of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoFilenameExtractionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).FileName;
			string expected = TestContext.DataRow["FileName"] as string;
			Assert.AreEqual(expected, actual, String.Format("FileName of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoFilenameWithoutExtensionExtractionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).FileNameWithoutExtension;
			string expected = TestContext.DataRow["FileNameWithoutExtension"] as string;
			Assert.AreEqual(expected, actual, String.Format("FileNameWithoutExtension of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoExtensionExtractionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).Extension;
			string expected = TestContext.DataRow["Extension"] as string;
			Assert.AreEqual(expected, actual, String.Format("Extension of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoGetFullPathTest()
		{
			string originalPath = System.IO.Directory.GetCurrentDirectory();

			try
			{
				System.IO.Directory.SetCurrentDirectory("C:\\");

				string input = TestContext.DataRow["Input"] as string;
				string actual = new PathInfo(input).GetFullPath();
				string expected = TestContext.DataRow["FullPath"] as string;
				Assert.AreEqual(expected, actual, String.Format("FullPath of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
			}
			finally
			{
				System.IO.Directory.SetCurrentDirectory(originalPath);
			}
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoGetLongFullPathTest()
		{
			string originalPath = System.IO.Directory.GetCurrentDirectory();

			try
			{
				System.IO.Directory.SetCurrentDirectory("C:\\");

				string input = TestContext.DataRow["Input"] as string;
				string actual = new PathInfo(input).GetLongPath();
				string expected = TestContext.DataRow["LongFullPath"] as string;
				Assert.AreEqual(expected, actual, String.Format("GetLongFullPath of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
			}
			finally
			{
				System.IO.Directory.SetCurrentDirectory(originalPath);
			}
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoIsRootedTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			bool actual = new PathInfo(input).IsRooted;
			bool expected = bool.Parse(TestContext.DataRow["IsRooted"] as string);
			Assert.AreEqual(expected, actual, String.Format("IsRooted of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoHasExtensionTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			bool actual = new PathInfo(input).HasExtension;
			bool expected = bool.Parse(TestContext.DataRow["HasExtension"] as string);
			Assert.AreEqual(expected, actual, String.Format("HasExtension of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}
	
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoHasFileNameTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			bool actual = new PathInfo(input).HasFileName;
			bool expected = bool.Parse(TestContext.DataRow["HasFileName"] as string);
			Assert.AreEqual(expected, actual, String.Format("HasFileName of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\PathTestData.xml", "Row", DataAccessMethod.Sequential), DeploymentItem("AlphaFS.UnitTest\\PathTestData.xml"), TestMethod]
		public void PathInfoParentTest()
		{
			string input = TestContext.DataRow["Input"] as string;
			string actual = new PathInfo(input).Parent.Path;
			string expected = TestContext.DataRow["Parent"] as string;
			Assert.AreEqual(expected, actual, String.Format("Parent of {0} was incorrect. Expected \"{1}\", but found \"{2}\".", input, expected, actual));
		}

	}
}

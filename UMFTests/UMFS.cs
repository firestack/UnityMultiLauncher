using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UMFTests
{
	[TestClass]
	public class UMFS
	{
		[TestMethod]
		public void TestFileLocations()
		{
			var a = UnityMultiFramework.Unity.Projects.ProjectLocations.ToList();
			
		}
	}
}

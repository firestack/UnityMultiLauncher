using System;

namespace UnityMultiLauncher
{
	public static class ExtentionMethods
	{
		public static string GetLocation(this System.Environment.SpecialFolder folder)
		{
			return Environment.GetFolderPath(folder);
		}
	}
}

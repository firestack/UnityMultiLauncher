using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

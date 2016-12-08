using System;

namespace UnityMultiLauncher
{
	public static class ExtentionMethods
	{
		public static string GetLocation(this System.Environment.SpecialFolder folder)
		{
			return Environment.GetFolderPath(folder);
		}

		public static string Format(this string baseString, params object[] objs)
		{
			return string.Format(baseString, objs);
		}

		public static string FormatExceptionData(this Exception exc)
		{
			// Extra Data
			if(exc.Data.Count == 0)
			{
				return "";
			}
			var ED = "Extra Data: \n";
			foreach (System.Collections.DictionaryEntry dp in exc.Data)
			{
				if(dp.Value != null)
				{
					var tmp = dp.Value.ToString();
					ED += "{0,-20}:\n{1}".Format(dp.Key, tmp);
				}
				else
				{
					ED += "\tKey: {0}".Format(dp.Key);
				}
				ED += Environment.NewLine;
			}

			return ED;
		}

		public static string FormatExceptionDataRecursive(this Exception exc)
		{
			var s = exc.FormatExceptionData();
			if(exc.InnerException != null)
			{
				s += "-- Inner Exception --";
				s += Environment.NewLine;
				s += exc.InnerException.FormatExceptionData();
			}
			return s;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnityMultiLauncher
{
	public static class Util
	{
		private static Regex versionExp = new Regex(@"(\d+).(\d+).(\d+)f(\d+)", RegexOptions.Compiled);

		public static string UnityProjectSettings(Uri project, string key)
		{
			var filename = System.IO.Path.Combine(project.LocalPath, @"ProjectSettings/ProjectSettings.asset");
			var data = System.IO.File.ReadAllBytes(filename);
			var filestring = Encoding.UTF8.GetString(data);
			if(filestring.StartsWith(@"%YAML 1.1"))
			{
				// File is not binary
				//lines = Encoding.ASCII.GetString(data);
			}
			else
			{
				filestring = Encoding.ASCII.GetString(data);
				//File is binary				
			}
			
			//var compat = lines.Where(line => line.Trim(' ').StartsWith(key)).ToList();
			//return compat.Count > 0? compat[0].Split(':')?[1] : "";
			return "";

		}

		public static Tuple<int, int, int, int> UnityProjectVersion(Uri project)
		{
			var filename = System.IO.Path.Combine(project.LocalPath, @"ProjectSettings /ProjectVersion.txt");
			var data = System.IO.File.ReadAllText(filename, Encoding.UTF8);
			var match = versionExp.Match(data);


			return Tuple.Create(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value), Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value));
		}

		public static Uri GetUnityExecutableFromVersion(Tuple<int, int, int, int> version)
		{
			foreach (var exe in ProgramConfig.conf.unityExeLocations)
			{
				var a = FileVersionInfo.GetVersionInfo(exe.LocalPath);
				if(a.ProductMajorPart == version.Item1 && a.ProductMinorPart == version.Item2 && a.ProductBuildPart == version.Item3)
				{
					return exe;
				}
			}
			return null;
		}

		public static Tuple<int, int, int, int> GetUnityVersionFromExecutable(Uri exec)
		{
			var versionInfo = FileVersionInfo.GetVersionInfo(exec.LocalPath);
			return Tuple.Create(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, 0);
		}
	}
}

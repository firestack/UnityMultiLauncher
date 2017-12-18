using System;
using System.Linq;

namespace UnityMultiFramework
{
	public static partial class Unity
	{
		public static ExecutableAccessor Executables = new ExecutableAccessor();

		public static ProjectAccessor Projects = new ProjectAccessor();

		public static void LoadSettings(Settings settings)
		{
			if(settings == null) { return; }
			Executables.Locations = settings.Locations;
		}

		public static Settings GetSettings()
		{
			return new Settings()
			{
				Locations = Executables.ToList()
			};
		}
	}
}
//	public static class Util
//	{
//		//private static Regex versionExp = new Regex(@"(\d+)\.(\d+)\.(\d+)([A-z]+)(\d+)", RegexOptions.Compiled);



//		public static string UnityProjectSettings(Uri project, string key)
//		{
//			var filename = System.IO.Path.Combine(project.LocalPath, @"ProjectSettings/ProjectSettings.asset");
//			var data = System.IO.File.ReadAllBytes(filename);
//			var filestring = Encoding.UTF8.GetString(data);
//			if (filestring.StartsWith(@"%YAML 1.1"))
//			{
//				// File is not binary
//				//lines = Encoding.ASCII.GetString(data);
//			}
//			else
//			{
//				filestring = Encoding.ASCII.GetString(data);
//				//File is binary				
//			}

//			//var compat = lines.Where(line => line.Trim(' ').StartsWith(key)).ToList();
//			//return compat.Count > 0? compat[0].Split(':')?[1] : "";
//			return "";

//		}



//		public static Uri GetUnityExecutableFromVersion(Version version)
//		{
//			//return ProgramConfig.conf.ValidUnityExeLocations
//			//	.Select(loc => (loc, GetUnityVersionFromExecutable(loc)))
//			//	.Where(exe => version.Major == exe.Item2.Major && version.Minor == exe.Item2.Minor && version.Build == exe.Item2.Build)
//			//	.Aggregate((Uri null, Version null), (working, next) => working.Item2.Revision > next.Item2.Revision ? working : next).Item1;

//			foreach (var exe in ProgramConfig.conf.ValidUnityExeLocations)
//			{
//				var a = GetUnityVersionFromExecutable(exe);
//				if (version.Major == a.Major && version.Minor == a.Minor && version.Build == a.Build)
//				{
//					return exe;
//				}
//			}
//			return null;
//		}

//		public static Version GetUnityVersionFromExecutable(Uri exec)
//		{
//			var versionInfo = FileVersionInfo.GetVersionInfo(exec.LocalPath);
//			return new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, 0);
//		}

//		public static void DumpUnityVersionInfo(Uri exec)
//		{
//			var versionInfo = FileVersionInfo.GetVersionInfo(exec.LocalPath);
//			System.Threading.Tasks.Task.Run(() => Newtonsoft.Json.JsonConvert.SerializeObject(versionInfo)).ContinueWith((input) => {
//				System.IO.File.WriteAllText(versionInfo.ProductName + "_" + versionInfo.ProductVersion + ".json", input.Result);

//			})

//;
//		}
//	}

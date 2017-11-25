using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace UnityMultiFramework
{
	public static partial class Unity
	{
		public static ProjectAccessor Projects = new ProjectAccessor();

		[System.Diagnostics.DebuggerDisplay("Name = {Name}, Version = {ProjectVersionString}")]
		public class Project : IVersionable, IEquatable<Project>
		{
			//m_EditorVersion: 2017.1.0f3
			private static Regex versionExp = new Regex(@"^m_EditorVersion: (\d+\.\d+\.\d+([A-z]{1,4})\d+)", RegexOptions.Compiled);

			public Project(Uri location)
			{
				Location = location;
				LoadVersionInfo();
				Name = Uri.UnescapeDataString(Location.Segments.Last());
			}

			public string Name { get; private set; }

			public Uri Location { get; private set; }

			public string ProjectVersionString { get; private set; }

			public Version Version { get; private set; }

			public string VersionType { get; private set; }

			private void LoadVersionInfo()
			{
				var filename = Path.Combine(Location.LocalPath, @"ProjectSettings\ProjectVersion.txt");
				if (File.Exists(filename))
				{
					var filedata = File.ReadAllText(filename, Encoding.UTF8);
					var matches = versionExp.Match(filedata);
					try
					{
						ProjectVersionString = matches.Groups[1].Value;
						VersionType = matches.Groups[2].Value;
						Version = Version.Parse(ProjectVersionString.Replace(VersionType, "."));
					}
					catch (Exception E)
					{
						E.Data["ProjectText"] = filedata.ToString();
						E.Data["Matches"] = matches.ToString();
						throw;
					}
				}
			}

			#region Interfaces
			
			public int CompareTo(IVersionable other)
			{
				return this.Version.CompareTo(other.Version);
			}

			public bool Equals(IVersionable other)
			{
				return Version == other.Version && VersionType == other.VersionType;
			}

			public bool Equals(Project other)
			{
				return Location == other.Location;
			}
			#endregion
		}

		public class ProjectAccessor : IEnumerable<Project>
		{
			private const string UNITYREGKEY = @"SOFTWARE\Unity Technologies\Unity Editor 5.x\";

			internal ProjectAccessor() { }

			private List<Uri> ExtraLocations = new List<Uri>();

			public IEnumerable<Project> ProjectLocations
				=> ExistingLocations.Select(uri => new Project(uri));

			public IEnumerable<Uri> ExistingLocations
				=> RegKeyToUri().Concat(ExtraLocations).Where(uri => Directory.Exists(uri.LocalPath) && Directory.Exists(Path.Combine(uri.LocalPath, "Assets")));

			public void AddLocations(params Uri[] uris)
				=> ExtraLocations.AddRange(uris);

			public IEnumerable<Uri> RegKeyToUri()
			{
				RegistryKey UnityRegKey = Registry
					.CurrentUser
					.OpenSubKey(UNITYREGKEY, false);

				return UnityRegKey?
					.GetValueNames()
					.Where(key => key.StartsWith("RecentlyUsedProjectPaths"))
					.Select(key =>
						new Uri(Encoding.UTF8.GetString(UnityRegKey.GetValue(key) as byte[]).TrimEnd((char)0))
					);
			}

			#region IEnumerable<Project> 

			public IEnumerator<Project> GetEnumerator() => ProjectLocations.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => ProjectLocations.GetEnumerator();

			#endregion
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

	}
}

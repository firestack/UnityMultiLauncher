using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace UnityMultiFramework
{

	[System.Diagnostics.DebuggerDisplay("Name = {Name}, Version = {ProjectVersionString}")]
	public class Project : IVersionable, IEquatable<Project>, ILaunchable
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

		public void Launch(params string[] args)
			=> Launch(Unity.Executables.ClosestExecutables(this).First(), args);

		public void Launch(Executable exe = null, params string[] args)
		{
			var argsList = args.ToList();
			argsList.Insert(0, $@"-projectpath ""{Location.LocalPath}""");

			exe.Launch(argsList.ToArray());
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
}

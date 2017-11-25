using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityMultiFramework
{
	public static partial class Unity
	{
		public class Executable : IVersionable, IEquatable<Executable>
		{
			private const string WILDCARD = "*";

			private Executable() { }

			public Executable(Uri Location)
			{
				this.Location = Location;
				VersionType = WILDCARD;

				var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(Location.LocalPath);
				Version = new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart, 0);
			}

			public Uri Location { get; private set; }

			public Version Version { get; private set; }

			public string VersionType { get; private set; }

			public void ChangeMinorVersion(int v)
			{
				Version = new Version(Version.Major, Version.Minor, Version.Build, v);
			}

			public void ChangeVersionType(string vT)
			{
				VersionType = vT;
			}

			public int CompareTo(IVersionable other)
			{
				return Version.CompareTo(other);
			}

			public bool Equals(IVersionable other)
			{
				return Version == other.Version
						&&
					(VersionType == WILDCARD ||
						other.VersionType == WILDCARD)
							? true : (VersionType == other.VersionType);
			}

			public bool Equals(Executable other)
			{
				return Location == other.Location;
			}
		}

		public static ExecutableAccessor Executables = new ExecutableAccessor();
		public class ExecutableAccessor
		{
			internal ExecutableAccessor() { }

			public List<Executable> Locations = new List<Executable>();

			public void AddIn(Uri baseLocation)
				=> Locations.AddRange(FindIn(baseLocation));

			private IEnumerable<Executable> FindIn(Uri baseLocation)
				=> System.IO.Directory.EnumerateFiles(baseLocation.LocalPath, "Unity.Exe", System.IO.SearchOption.AllDirectories).Select(loc => new Executable(new Uri(loc)));
		}
	}
}

using System;

namespace UnityMultiFramework
{
	public class Executable : IVersionable, IEquatable<Executable>, ILaunchable
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

		public string VersionType { get; set; }

		public int MinorVersion { set { Version = new Version(Version.Major, Version.Minor, Version.Build, value); } }

		public void Launch(params string[] args)
			=> System.Diagnostics.Process.Start(Location.LocalPath, string.Join(" ", args));

		public int CompareTo(IVersionable other)
			=> Version.CompareTo(other);

		public bool Equals(IVersionable other)
			=> FuzzyEquals(other) && (VersionType == WILDCARD || other.VersionType == WILDCARD) ? true : (VersionType == other.VersionType);

		public bool FuzzyEquals(IVersionable other)
		{
			return (Version.Major == other.Version.Major) && (Version.Minor == other.Version.Minor) && (Version.Build == other.Version.Build); 
		}

		public int FuzzyCompareTo(IVersionable other)
		{
			if ( !FuzzyEquals(other) ) { return CompareTo(other); }
			if( VersionType != other.VersionType ) { return VersionType.CompareTo(other.VersionType); }
			return Version.Revision - other.Version.Revision;
		}

		public bool Equals(Executable other)
			=> Location == other.Location;
	}
}

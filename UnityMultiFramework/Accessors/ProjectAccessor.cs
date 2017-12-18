using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace UnityMultiFramework
{

		public class ProjectAccessor : IEnumerable<Project>
		{
			private const string UNITYREGKEY = @"SOFTWARE\Unity Technologies\Unity Editor 5.x\";

			internal ProjectAccessor() { }

			private List<Uri> ExtraLocations = new List<Uri>();

			public IEnumerable<Project> ProjectLocations
				=> ExistingLocations
					.Select(uri => new Project(uri));

			public IEnumerable<Uri> ExistingLocations
				=> ExtraLocations
					.Concat((RegKeyToUri() ?? Enumerable.Empty<Uri>()))
					.Distinct()
					.Where(uri => 
						Directory.Exists(uri.LocalPath) && 
						Directory.Exists(Path.Combine(uri.LocalPath, "Assets"))
					);

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityMultiFramework
{
	public class ExecutableAccessor : IEnumerable<Executable>
	{
		internal ExecutableAccessor() { }

		public List<Executable> Locations = new List<Executable>();

		public void AddIn(Uri baseLocation)
			=> Locations.AddRange(FindIn(baseLocation));

		private IEnumerable<Executable> FindIn(Uri baseLocation)
			=> System.IO.Directory
				.EnumerateFiles(baseLocation.LocalPath, "Unity.exe", System.IO.SearchOption.AllDirectories)
				.Select(loc => new Executable(new Uri(loc)));

		public IEnumerable<Executable> ClosestExecutables(IVersionable ver) 
			=> Locations.Where(loc => loc.FuzzyEquals(ver)).OrderBy(exe => Math.Abs(exe.FuzzyCompareTo(ver)));

		public IEnumerator<Executable> GetEnumerator()
		{
			return ((IEnumerable<Executable>)Locations).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Executable>)Locations).GetEnumerator();
		}
	}

}

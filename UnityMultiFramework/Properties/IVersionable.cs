using System;

namespace UnityMultiFramework
{
	public interface IVersionable : IComparable<IVersionable>, IEquatable<IVersionable>
	{
		Version Version { get; }
		string VersionType { get; }
	}
}
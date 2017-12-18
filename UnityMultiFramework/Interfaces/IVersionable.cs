using System;

namespace UnityMultiFramework
{
	public interface IVersionable : IComparable<IVersionable>, IEquatable<IVersionable>
	{
		Version Version { get; }
		string VersionType { get; }

		//bool operator ==(IVersionable lhs, IVersionable rhs);
		//bool operator !=(IVersionable lhs, IVersionable rhs);
	}
}
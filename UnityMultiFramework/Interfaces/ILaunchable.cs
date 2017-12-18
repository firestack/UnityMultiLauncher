using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityMultiFramework
{
	public interface ILaunchable
	{
		void Launch(params string[] args);
	}
}

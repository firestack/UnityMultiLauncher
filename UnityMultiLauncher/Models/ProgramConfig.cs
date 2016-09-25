using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnityMultiLauncher
{
	public class ProgramConfig : Utility.ConfigLoader.Config
	{
		protected static ProgramConfig _conf;
		public new static ProgramConfig conf
		{
			get
			{
				if(_conf == null)
				{
					try
					{
						_conf = Utility.ConfigLoader.LoadConfig<ProgramConfig>(defaultFilename);
					}
					catch (Exception)
					{
						_conf = new ProgramConfig { filename = defaultFilename };
						_conf.Save();
					}
				}
				return _conf;
			}
		}

		public static string defaultFilename { get { return string.Format("unitymultilauncher.{0}.cfg.json", Environment.UserName); } }

		
		public HashSet<Uri> unityExeLocations = new HashSet<Uri>();

		public List<Uri> unityProjectLocations = new List<Uri>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MahApps.Metro;

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
						_conf.Save();
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

		[JsonIgnore]
		public Tuple<AppTheme, Accent> appStyle
		{
			get
			{
				return Tuple.Create(
					ThemeManager.AppThemes.First(theme => theme.Name == appTheme), 
					ThemeManager.Accents.First(accent => accent.Name == appAccent)
				);
			}
			set
			{
				appTheme = value.Item1.Name;
				appAccent = value.Item2.Name;
			}
		}

		public string appTheme = "BaseLight";

		public string appAccent = "Blue";

		public HashSet<Uri> unityExeLocations = new HashSet<Uri>();

		public bool ShouldUseUnitySubVersion = false;
	}
}

using System;
using Newtonsoft.Json;
using System.IO;

namespace Utility
{
	public static class ConfigLoader
	{
		[Serializable]
		public abstract class Config
		{
			public static Config conf { get; set; }

			public virtual void Save()
			{
				ConfigLoader.SaveConfig(filename, this);
			}

			public string filename = "";
		}

		public static object LoadConfig(string relPath) 
		{
			return JsonConvert.DeserializeObject(File.ReadAllText(relPath));
		}

		public static T LoadConfig<T>(string relPath) where T : Config
		{
			return JsonConvert.DeserializeObject<T>(File.ReadAllText(relPath));
		}

		public static void SaveConfig(string relPath, object obj)
		{
			File.WriteAllText(relPath, JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { }));
		}
	}
}

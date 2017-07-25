using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;

namespace UnityMultiLauncher.Controls.Converters
{
	public class UnityUriToUnityVersion : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is IEnumerable<Uri>)
			{
				var convertedUris = new List<string>();
				foreach(var a in value as IEnumerable<Uri>)
				{
					convertedUris.Add('v' + ConvertInternal(a));

				}
				return convertedUris;
			}
			if (value is Uri)
			{
				return 'v' + ConvertInternal(value as Uri);
			}
			return "";
		}

		public string ConvertInternal(Uri unity)
		{
			var versionInfo = Util.GetUnityVersionFromExecutable(unity);
			return $"{versionInfo.Major}.{versionInfo.Minor}.{versionInfo.Build}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}
	}
}


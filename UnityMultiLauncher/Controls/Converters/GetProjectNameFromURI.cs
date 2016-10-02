using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;

namespace UnityMultiLauncher.Controls.Converters
{ 
	class GetProjectNameFromURI : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is Uri)
			{
				var project = value as Uri;
				return project.LocalPath.Substring(project.LocalPath.LastIndexOf('\\') + 1);
			}
			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

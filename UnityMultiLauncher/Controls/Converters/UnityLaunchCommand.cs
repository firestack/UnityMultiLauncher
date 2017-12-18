using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;

namespace UnityMultiLauncher.Controls.Converters
{
	public class ILaunchableCommand : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var a = (UnityMultiFramework.ILaunchable)value;
			return new ViewModels.Utils.ViewCommand((extra) => a.Launch());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}

using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;

namespace UnityMultiLauncher.Controls.Converters
{
	public class NumberToGoldenRatio : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (IsNumber(value))
			{
				var d = System.Convert.ToDouble(value);
				return d * 1.618;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}

		public static bool IsNumber(object value)
		{
			return value is sbyte
					|| value is byte
					|| value is short
					|| value is ushort
					|| value is int
					|| value is uint
					|| value is long
					|| value is ulong
					|| value is float
					|| value is double
					|| value is decimal;
		}
	}
}

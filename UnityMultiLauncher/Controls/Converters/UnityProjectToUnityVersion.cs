﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;

namespace UnityMultiLauncher.Controls.Converters
{
	class UnityProjectToUnityVersion : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var unity = value as Uri;
			var file = System.IO.File.Open(System.IO.Path.Combine(unity.AbsolutePath, @"ProjectSettings/ProjectSettings.asset"), System.IO.FileMode.Open);
			throw new NotImplementedException();

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
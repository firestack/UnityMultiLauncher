﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro;

namespace UnityMultiLauncher
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{

			// get the current app style (theme and accent) from the application
			// you can then use the current theme and custom accent instead set a new theme
			Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);

			// now set the Green accent and dark theme
			ThemeManager.ChangeAppStyle(Application.Current,
										ThemeManager.GetAccent(ProgramConfig.conf.AccentColor),
										ThemeManager.GetAppTheme("BaseLight")); // or appStyle.Item1

			base.OnStartup(e);
		}
	}
}

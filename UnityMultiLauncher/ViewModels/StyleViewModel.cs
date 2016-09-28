using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro;
using UnityMultiLauncher.ViewModels.Utils;

namespace UnityMultiLauncher.ViewModels
{
	class StyleViewModel : ViewModel
	{
		public IEnumerable<string> appTheme
		{
			get
			{
				return ThemeManager.AppThemes.Select(theme => theme.Name);
			}
		}

		public IEnumerable<string> appAccent
		{
			get
			{
				return ThemeManager.Accents.Select(accent => accent.Name);
			}
		}

		public Tuple<AppTheme, Accent> cTheme
		{
			get
			{
				return ThemeManager.DetectAppStyle(Application.Current);
			}
			set
			{
				ThemeManager.ChangeAppStyle(Application.Current, value.Item2, value.Item1);
				//ThemeManager.GetAccent(ProgramConfig.conf.AccentColor), ThemeManager.GetAppTheme("BaseLight")
				ProgramConfig.conf.appStyle = value;
				ProgramConfig.conf.Save();
			}
		}

		public int appThemeSelected
		{
			get
			{
				return appTheme.ToList().IndexOf(cTheme.Item1.Name);
			}
			set
			{
				cTheme = Tuple.Create(
					ThemeManager.GetAppTheme(appTheme.ToList()[value]),
					ThemeManager.GetAccent(appAccent.ToList()[appAccentSelected])
				);
			}
		}

		public int appAccentSelected
		{
			get
			{
				return appAccent.ToList().IndexOf(cTheme.Item2.Name);
			}
			set
			{
				cTheme = Tuple.Create(
					ThemeManager.GetAppTheme(appTheme.ToList()[appThemeSelected]), 
					ThemeManager.GetAccent(appAccent.ToList()[value])
				);
			}
		}
	}
}

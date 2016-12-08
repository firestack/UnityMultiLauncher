using System.Windows;
using System;
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
			// now set the Green accent and dark theme
			ThemeManager.ChangeAppStyle(Application.Current, ProgramConfig.conf.appStyle.Item2, ProgramConfig.conf.appStyle.Item1);

			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

			base.OnStartup(e);
		}

		private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			System.TimeSpan timeDifference = DateTime.UtcNow -
				new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			long unixEpochTime = System.Convert.ToInt64(timeDifference.TotalSeconds);

			var filePath = @"Crashes/log." + unixEpochTime.ToString() + ".txt";

			System.IO.FileInfo file = new System.IO.FileInfo(filePath);
			file.Directory.Create(); System.IO.File.WriteAllText(file.FullName, e.Exception.ToString());

			var eWin = new Views.ErrorWindow();
			
			eWin.EVMP.EVMException = e.Exception;
			eWin.EVMP.FileLocation = file;

			eWin.metroWindow.ShowDialog();
		}
	}
}

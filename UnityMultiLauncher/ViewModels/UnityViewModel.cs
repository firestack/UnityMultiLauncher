using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityMultiLauncher.ViewModels.Utils;
using Microsoft.Win32;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace UnityMultiLauncher.ViewModels
{
	public class UnityViewModel : ViewModel
	{
		protected void LaunchUnityVersion(Uri unityExe)
		{

			System.Diagnostics.Process.Start(unityExe.LocalPath);
		}

		protected void LaunchProject(Uri projectLocation)
		{
			var exec = Util.GetUnityExecutableFromVersion(Util.UnityProjectVersion(projectLocation));
			if (exec != null)
			{
				System.Diagnostics.Process.Start(exec.LocalPath, string.Format("-projectpath {0}", projectLocation.LocalPath));
			}
			else
			{
				var a = Util.UnityProjectVersion(projectLocation);
				MainWindow.cwin.ShowMessageAsync("Unity Not Found", string.Format("The Unity Version For This Project Is Not Installed \n({0})", string.Format("Unity {0}.{1}.{2}", a.Item1, a.Item2, a.Item3)));
			}
		}

		protected void AddUnityVersion(object param)
		{
			
			var a = new OpenFileDialog();
			//a.Filter = "exe";
			if ((bool)a.ShowDialog())
			{
				ProgramConfig.conf.unityExeLocations.Add(new Uri(a.FileName));
				unityLocations.Add(new Uri(a.FileName));
				ProgramConfig.conf.Save();
				UpdateProperty(nameof(unityLocations));
			}
			
		}

		public ObservableCollection<Uri> unityLocations
		{
			get
			{
				return GetProperty() as ObservableCollection<Uri>?? SetProperty(new ObservableCollection<Uri>(ProgramConfig.conf.unityExeLocations));
			}
		}

		public List<Tuple<Uri, string, string>> unityProjectLocations
		{
			get
			{
				var unityKeys = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Unity Technologies\Unity Editor 5.x\", false);
				var matchingKeys = unityKeys.GetValueNames().Where(key => key.Contains("RecentlyUsedProjectPaths"));
				var pos = matchingKeys.Select(key => new Uri(Encoding.UTF8.GetString(unityKeys.GetValue(key) as byte[]).TrimEnd((char)0)));
				ProgramConfig.conf.unityProjectLocations.AddRange(pos);
				ProgramConfig.conf.unityProjectLocations = ProgramConfig.conf.unityProjectLocations.Distinct().ToList();
				ProgramConfig.conf.Save();

				var ret = new List<Tuple<Uri, string, string>>();
				foreach (var project in ProgramConfig.conf.unityProjectLocations)
				{
					var a = Util.UnityProjectVersion(project);
					ret.Add(Tuple.Create(project, project.LocalPath.Substring(project.LocalPath.LastIndexOf('\\')+1), string.Format("{0}.{1}.{2}f{3}",a.Item1, a.Item2, a.Item3, a.Item4)));
				}

				return ret;
			}
		}

		public ViewCommand launchUnity
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => LaunchUnityVersion(param as Uri)));
			}
		}

		public ViewCommand launchUnityProject
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => LaunchProject(param as Uri)));
			}
		}

		public ViewCommand addUnityVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => AddUnityVersion(param)));
			}
		}
	}

	public class unityLaunchers : ViewModel
	{

	}
}

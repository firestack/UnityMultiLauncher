﻿using System;
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
			UpdateProperty(nameof(unityProjectLocations));
		}

		protected void AddUnityVersion(object param)
		{

			var a = new OpenFileDialog();
			a.Filter = "Executable Files (.exe)|*.exe|All Files (*.*)|*.*";
			//a.Filter = "exe";
			if ((bool)a.ShowDialog())
			{
				ProgramConfig.conf.unityExeLocations.Add(new Uri(a.FileName));
				unityLocations.Add(new Uri(a.FileName));
				UpdateProperty(nameof(unityLocations));
				ProgramConfig.conf.Save();
			}

		}

		protected void RemoveUnityVersion(object param)
		{
			ProgramConfig.conf.unityExeLocations.Remove(param as Uri);
			unityLocations.Remove(param as Uri);
			ProgramConfig.conf.Save();
			UpdateProperty(nameof(unityLocations));
		}

		protected void LaunchProject(Uri projectLocation, Uri unityExe = null)
		{
			var projectVersion = Util.UnityProjectVersion(projectLocation);
			var exec = unityExe == null ? Util.GetUnityExecutableFromVersion(projectVersion) : unityExe;
			if (exec != null)
			{
				System.Diagnostics.Process.Start(exec.LocalPath, string.Format("-projectpath {0}", projectLocation.LocalPath));
				UpdateProperty(nameof(unityProjectLocations));
			}
			else
			{
				MainWindow.cwin.ShowMessageAsync(
					"Unity Not Found", 
					string.Format("The Unity Version For This Project Is Not Installed \n({0})", 
						string.Format("Unity {0}.{1}.{2}", projectVersion.Item1, projectVersion.Item2, projectVersion.Item3))
				);
			}
			if(unityExe == null)
			{
				selectedVersion = null;
				selectedProject = null;
			}
		}

		protected void DuplicateUnityProject(Uri param)
		{
			ProgramConfig.conf.unityExeLocations.Remove(param);
			unityLocations.Remove(param);
			ProgramConfig.conf.Save();
			UpdateProperty(nameof(unityLocations));
		}

		protected void SelectUnityVersionDialog(Uri project)
		{
			var cd = MainWindow.cwin.TryFindResource("CustomLaunchDialog");
			MainWindow.cwin.ShowMetroDialogAsync(cd as CustomDialog);
			selectedProject = project;
			selectedVersion = Util.GetUnityExecutableFromVersion(Util.UnityProjectVersion(project));
		}

		public ObservableCollection<Uri> unityLocations
		{
			get
			{
				return GetProperty() as ObservableCollection<Uri> ?? SetProperty(new ObservableCollection<Uri>(ProgramConfig.conf.unityExeLocations));
			}
		}

		public IEnumerable<Tuple<Uri, string, string>> unityProjectLocations
		{
			get
			{

#if EXAMPLE_VIEW
				var rng = new Random();
				foreach(var idx in Enumerable.Range(0,10))
				{
					var testUri = new Uri(@"A:\UnityProjects\UnityProject" + idx);

					var rngExeVersion = Util.GetUnityVersionFromExecutable(unityLocations[rng.Next(0, unityLocations.Count-1)]);

					yield return Tuple.Create(testUri, testUri.LocalPath.Substring(testUri.LocalPath.LastIndexOf('\\') + 1), string.Format("{0}.{1}.{2}f{3}", rngExeVersion.Item1, rngExeVersion.Item2, rngExeVersion.Item3, rngExeVersion.Item4));
				}			
#else

				var unityKeys = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Unity Technologies\Unity Editor 5.x\", false);
				var matchingKeys = unityKeys.GetValueNames().Where(key => key.Contains("RecentlyUsedProjectPaths"));
				var pos = matchingKeys.Select(key => new Uri(Encoding.UTF8.GetString(unityKeys.GetValue(key) as byte[]).TrimEnd((char)0)));
				foreach (var project in pos)
				{
					var a = Util.UnityProjectVersion(project);
					yield return Tuple.Create(project, project.LocalPath.Substring(project.LocalPath.LastIndexOf('\\') + 1), string.Format("{0}.{1}.{2}f{3}", a.Item1, a.Item2, a.Item3, a.Item4));
				}
#endif

			}
		}

		public Uri selectedProject
		{
			get
			{
				return GetProperty() as Uri;
			}
			set
			{
				SetProperty(value);
			}
		}

		public Uri selectedVersion
		{
			get
			{
				return GetProperty() as Uri;
			}
			set
			{
				SetProperty(value);
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

		public ViewCommand launchUnityProjectWithVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => LaunchProject(selectedProject, selectedVersion)));
			}
		}

		public ViewCommand addUnityVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => AddUnityVersion(param)));
			}
		}

		public ViewCommand removeUnityVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => RemoveUnityVersion(param)));
			}
		}

		public ViewCommand duplicateUnityProject
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => DuplicateUnityProject(param as Uri)));
			}
		}

		public ViewCommand unitySelectVersionDialog
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => SelectUnityVersionDialog(param as Uri)));
			}
		}
	}
}
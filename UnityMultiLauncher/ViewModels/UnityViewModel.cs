using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityMultiLauncher.ViewModels.Utils;
using Microsoft.Win32;
using System.Windows.Forms;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace UnityMultiLauncher.ViewModels
{
	public class UnityViewModel : ViewModel
	{
		protected List<Uri> extraProjectLoctions = new List<Uri>();
		#region Methods
		protected void LaunchUnityVersion(Uri unityExe, params string[] args)
		{

			System.Diagnostics.Process.Start(unityExe.LocalPath, string.Join(" ", args));
			UpdateProperty(nameof(UnityProjectLocations));
		}

		protected void SearchUnityVersions()
		{
			var a = new System.Windows.Forms.FolderBrowserDialog();
			if (a.ShowDialog() == DialogResult.OK)
			{
				var path = new System.IO.DirectoryInfo(a.SelectedPath);

				var stopwatch = System.Diagnostics.Stopwatch.StartNew();

				foreach (var location in path.GetFiles("Unity.exe", System.IO.SearchOption.AllDirectories))
				{
					AddUnityExe(location.FullName);
				}

				stopwatch.Stop();


			}
		}

		protected void AddUnityExe(string location)
		{
			ProgramConfig.conf.unityExeLocations.Add(new Uri(location));
			UnityLocations.Add(new Uri(location));
			UpdateProperty(nameof(UnityLocations));
			ProgramConfig.conf.Save();
		}

		protected void AddUnityVersion(object param)
		{
			//FileSelectDialog();
			//return;
			var a = new System.Windows.Forms.OpenFileDialog()
			{
				Filter = "Programs (.exe)|*.exe|All Files (*.*)|*.*"
			};
			//a.Filter = "exe";
			if (a.ShowDialog() == DialogResult.OK)
			{
				AddUnityExe(a.FileName);
			}

		}

		protected void RemoveUnityVersion(object param)
		{
			ProgramConfig.conf.ValidUnityExeLocations.Remove(param as Uri);
			UnityLocations.Remove(param as Uri);
			ProgramConfig.conf.Save();
			UpdateProperty(nameof(UnityLocations));
		}

		protected void LaunchProject(Uri projectLocation, Uri unityExe = null)
		{
			var projectVersion = Util.UnityProjectVersion(projectLocation);
			if (unityExe == null)
			{
				unityExe = Util.GetUnityExecutableFromVersion(projectVersion.version);
			}
			if (unityExe != null)
			{
				LaunchUnityVersion(unityExe, "-projectpath", $"\"{projectLocation.LocalPath}\"");
				//System.Diagnostics.Process.Start(unityExe.LocalPath, $"-projectpath \"{}\"" );
				UpdateProperty(nameof(UnityProjectLocations));
			}
			else
			{
				var dialogSettings = new MetroDialogSettings { AnimateHide = false };
				MainWindow.cwin.ShowMessageAsync(
					"Unity Not Found",
					$"The Unity Version For This Project Is Not Installed \n({$"Unity {projectVersion.version.Major}.{projectVersion.version.Minor}.{projectVersion.version.Build}"})",
					MessageDialogStyle.Affirmative,
					dialogSettings
				).ContinueWith(
					// HACK: This makes the screen oddly flash (This could either be the thread switch or the animate show affecting it oddly
					(a) => System.Windows.Application.Current.Dispatcher.Invoke(() => SelectUnityVersionDialog(projectLocation, new MetroDialogSettings { AnimateShow = false }))
				);
			}
			if (unityExe == null)
			{
				SelectedVersion = null;
				SelectedProject = null;
			}
		}

		protected void AddUnityProject()
		{
			var a = new FolderBrowserDialog();

			if (a.ShowDialog() == DialogResult.OK)
			{
				extraProjectLoctions.Add(new Uri(a.SelectedPath));
				UpdateProperty(nameof(UnityProjectLocations));
			}
		}

		protected void DuplicateUnityProject(Uri param)
		{
			ProgramConfig.conf.ValidUnityExeLocations.Remove(param);
			UnityLocations.Remove(param);
			ProgramConfig.conf.Save();
			UpdateProperty(nameof(UnityLocations));
		}

		protected void SelectUnityVersionDialog(Uri project, MetroDialogSettings dialogSettings = null)
		{
			var cd = MainWindow.cwin.TryFindResource("CustomLaunchDialog");
			MainWindow.cwin.ShowMetroDialogAsync(cd as CustomDialog, dialogSettings);
			SelectedProject = project;
			SelectedVersion = Util.GetUnityExecutableFromVersion(Util.UnityProjectVersion(project).version);
		}

		protected void FileSelectDialog()
		{
			var cd = MainWindow.cwin.TryFindResource("FileBrowser");
			MainWindow.cwin.ShowMetroDialogAsync(cd as CustomDialog);

		}

		public void DumpUnityExeInfoFunc(object uri)
		{
			Util.DumpUnityVersionInfo(uri as Uri);
		}

		public void OpenUnityProjectLocation(object param)
		{
			System.Diagnostics.Process.Start((param as Uri).LocalPath);
		}
		#endregion
		#region Properties
		public ObservableCollection<Uri> UnityLocations
		{
			get
			{
				return GetProperty() as ObservableCollection<Uri> ?? SetProperty(new ObservableCollection<Uri>(ProgramConfig.conf.ValidUnityExeLocations));
			}
		}

		public System.ComponentModel.ICollectionView UnityLocationsSorted
		{
			get
			{
				ListCollectionView prop = GetProperty() as ListCollectionView;
				if (prop == null)
				{
					var p = CollectionViewSource.GetDefaultView((IList<Uri>)UnityLocations) as ListCollectionView;
					prop = SetProperty(p as ListCollectionView);
					prop.CustomSort = Comparer<Uri>.Create(
						(A, B) => Util.GetUnityVersionFromExecutable(B).CompareTo(Util.GetUnityVersionFromExecutable(A))
					);
					prop.IsLiveSorting = true;
				}

				return prop;

			}
		}

		public IEnumerable<Tuple<Uri, string, string>> UnityProjectLocations
		{
			get
			{
#if EXAMPLE_VIEW
				var rng = new Random();
				foreach(var idx in Enumerable.Range(0,10))
				{
					var testUri = new Uri(@"A:\UnityProjects\UnityProject" + idx);

					var rngExeVersion = Util.GetUnityVersionFromExecutable(unityLocations[rng.Next(0, unityLocations.Count-1)]);

					yield return Tuple.Create(testUri, testUri.LocalPath.Substring(testUri.LocalPath.LastIndexOf('\\') + 1), $"{rngExeVersion.Major}.{rngExeVersion.Minor}.{rngExeVersion.Build}f{rngExeVersion.Revision}");
				}			
#else
				var unityKeys = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Unity Technologies\Unity Editor 5.x\", false);
				if (unityKeys == null)
				{
					yield break;
				}
				var matchingKeys = unityKeys.GetValueNames().Where(key => key.Contains("RecentlyUsedProjectPaths"));

				var pos = matchingKeys.Select(key => new Uri(Encoding.UTF8.GetString(unityKeys.GetValue(key) as byte[]).TrimEnd((char)0)));
				foreach (var project in extraProjectLoctions.Concat(pos))
				{
					if (System.IO.Directory.Exists(project.LocalPath) && System.IO.Directory.Exists(System.IO.Path.Combine(project.LocalPath, "Assets")))
					{
						var projectVersion = Util.UnityProjectVersion(project);
						if (projectVersion.version != null)
							yield return Tuple.Create(
								project,
								project.LocalPath.Substring(project.LocalPath.LastIndexOf('\\') + 1),
								$"{projectVersion.version.Major}.{projectVersion.version.Minor}.{projectVersion.version.Build}{projectVersion.buildType}{projectVersion.version.Revision}"
							);
					}
				}
#endif

			}
		}

		public Uri SelectedProject
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

		public Uri SelectedVersion
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

		public bool SupportUnitySubversion { get; set; }

		public bool UseUnitySubVersion
		{
			get
			{
				return ProgramConfig.conf.ShouldUseUnitySubVersion;
			}
			set
			{
				ProgramConfig.conf.ShouldUseUnitySubVersion = value;
				ProgramConfig.conf.Save();
				UpdateProperty();
			}
		} 
		#endregion
		#region Commands
		public ViewCommand CmdLaunchUnity
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => LaunchUnityVersion(param as Uri)));
			}
		}

		public ViewCommand CmdLaunchUnityProject
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => LaunchProject(param as Uri)));
			}
		}

		public ViewCommand CmdAddUnityProject
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => AddUnityProject()));
			}
		}

		public ViewCommand CmdLaunchUnityProjectWithVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => { LaunchProject(SelectedProject, SelectedVersion); UtilViewModel.HideDialogFunc(param); }));
			}
		}

		public ViewCommand CmdAddUnityVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => SearchUnityVersions()));
			}
		}

		public ViewCommand CmdRemoveUnityVersion
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => RemoveUnityVersion(param)));
			}
		}

		public ViewCommand CmdDuplicateUnityProject
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => DuplicateUnityProject(param as Uri)));
			}
		}

		public ViewCommand CmdUnitySelectVersionDialog
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => SelectUnityVersionDialog(param as Uri)));
			}
		}

		public ViewCommand CmdDumpUnityExeInfo
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(DumpUnityExeInfoFunc));
			}
		}

		public ViewCommand CmdCreateTemporaryProject
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(obj => LaunchUnityVersion((Uri)obj, "-temporary")));
			}
		}

		public ViewCommand CmdOpenUnityProjectLocaiton
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(OpenUnityProjectLocation));
			}
		} 
		#endregion
	}
}

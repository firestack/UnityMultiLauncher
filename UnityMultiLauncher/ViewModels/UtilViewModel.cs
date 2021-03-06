﻿using UnityMultiLauncher.ViewModels.Utils;
using MahApps.Metro.Controls.Dialogs;

namespace UnityMultiLauncher.ViewModels
{
	public class UtilViewModel : Utils.ViewModel
	{
		public static void ToggleFlyout(object param)
		{
			if (param is MahApps.Metro.Controls.Flyout)
			{
				var fl = (param as MahApps.Metro.Controls.Flyout);
				fl.IsOpen = !fl.IsOpen;
			}

		}

		public static void OpenDialogFunc(object cd)
		{
			MainWindow.cwin.ShowMetroDialogAsync(cd as CustomDialog);
		}

		public static void HideDialogFunc(object cd)
		{
			MainWindow.cwin.HideMetroDialogAsync(cd as CustomDialog);
		}

		public Utils.ViewCommand OpenFlyout
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(ToggleFlyout));
			}
		}

		public Utils.ViewCommand OpenDialog
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(OpenDialogFunc));
			}
		}

		public ViewCommand HideDialog
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(HideDialogFunc));
			}
		}

#if DEBUG
		public ViewCommand TestDialog
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(param => MainWindow.cwin.ShowMessageAsync("Test", "Testing")));
			}
		}
#endif
	}
}

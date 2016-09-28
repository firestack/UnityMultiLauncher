using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityMultiLauncher.ViewModels.Utils;

namespace UnityMultiLauncher.ViewModels
{
	public class UtilViewModel : Utils.ViewModel
	{
		protected void ToggleFlyout(object param)
		{
			if (param is MahApps.Metro.Controls.Flyout)
			{
				var fl = (param as MahApps.Metro.Controls.Flyout);
				fl.IsOpen = !fl.IsOpen;
			}

		}


		public Utils.ViewCommand OpenFlyout
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(ToggleFlyout));
			}
		}
	}
}

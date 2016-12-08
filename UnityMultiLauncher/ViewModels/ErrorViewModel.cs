using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityMultiLauncher.ViewModels
{
	using Utils;
	public class ErrorViewModel : ViewModel
	{
		public System.Exception EVMException
		{
			get
			{
				return GetProperty() as Exception;
			}
			set
			{
				SetProperty(value);
				UpdateProperty(nameof(ExceptionString));
			}
		}

		public string ExceptionString
		{
			get
			{


				return EVMException.FormatExceptionDataRecursive() + Environment.NewLine + EVMException.ToString() ;
			}
		}

		public System.IO.FileInfo FileLocation
		{
			get
			{
				return GetProperty() as System.IO.FileInfo;
			}
			set
			{
				SetProperty(value);
			}
		}	

		public void OpenGithubFunc(object obj)
		{
			
			
			var url = "https://github.com/firestack/UnityMultiLauncher/issues/new?"+
				"title=Automatic Error Report" +
				"&body=<Insert Info Here>\n```"+ExceptionString+"```";
			var uri = new Uri(url, UriKind.Absolute);
			System.Diagnostics.Process.Start(uri.AbsoluteUri);
		}

		public ViewCommand OpenGithub
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(OpenGithubFunc));
			}
		}

		public void ExceptionToClipboardFunc(object obj)
		{
			System.Windows.Clipboard.SetText(ExceptionString);

		}

		public ViewCommand ExceptionToClipboard
		{
			get
			{
				return GetProperty() as ViewCommand ?? SetProperty(new ViewCommand(ExceptionToClipboardFunc));
			}
		}


	}
}

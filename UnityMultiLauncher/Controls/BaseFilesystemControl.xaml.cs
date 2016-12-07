using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace UnityMultiLauncher.Controls
{
	/// <summary>
	/// Interaction logic for BaseFilesystemControl.xaml
	/// </summary>
	public partial class BaseFilesystemControl : UserControl
	{
		public static readonly DependencyProperty ButtonArray = ButtonArrayPropertyKey.DependencyProperty;

		private static readonly DependencyPropertyKey ButtonArrayPropertyKey =
		DependencyProperty.RegisterReadOnly(
			"Buttons",
			typeof(ObservableCollection<string>),
			typeof(BaseFilesystemControl),
			new FrameworkPropertyMetadata(new ObservableCollection<string>())
		);

		public ObservableCollection<string> Buttons { get { return (ObservableCollection<string>)GetValue(ButtonArray); } set { SetValue(ButtonArray, value); } }

		public static readonly DependencyProperty DirectoryRoot = DependencyProperty.Register("dirRoot", typeof(Uri), typeof(BaseFilesystemControl), new PropertyMetadata(new Uri(@"C:\")));

		public Uri dirRoot
		{
			get
			{
				return (Uri)GetValue(DirectoryRoot);
			}
			set
			{
				SetValue(DirectoryRoot, value);
			}
		}

		public BaseFilesystemControl()
		{
			InitializeComponent();
		}
	}
}

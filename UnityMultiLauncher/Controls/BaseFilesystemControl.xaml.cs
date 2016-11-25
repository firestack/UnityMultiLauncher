using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

using System;
using MahApps.Metro.Controls;

namespace UnityMultiLauncher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public static MainWindow cwin;

		public MainWindow()
		{
			cwin = this;
			InitializeComponent();
			
			//var a = Environment.SpecialFolder.MyComputer.GetLocation();
		}
	}
}

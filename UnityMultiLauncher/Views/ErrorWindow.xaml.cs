﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;


namespace UnityMultiLauncher.Views
{
	/// <summary>
	/// Interaction logic for ErrorWindow.xaml
	/// </summary>
	public partial class ErrorWindow : MetroWindow
	{
		public ViewModels.ErrorViewModel EVMP;

		public ErrorWindow()
		{
			InitializeComponent();
			EVMP = FindResource("EVM") as ViewModels.ErrorViewModel;
			
		}
	}
}

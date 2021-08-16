using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BE;
using System.Threading;

namespace PL
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		BL.IBL bl = BL.BL_Factory.GetBL();
		public MainWindow()
		{
			bl.StartThreads();
			InitializeComponent();
		}

		private void GuestButton_Click(object sender, RoutedEventArgs e)
		{
			GuestWindow gust = new GuestWindow();
			this.Close();
			gust.Show();
		}

		private void HostButton_Click(object sender, RoutedEventArgs e)
		{
			HostWindow host = new HostWindow();
			this.Close();
			host.Show();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e)
		{
			AdminWindow admin = new AdminWindow();
			this.Close();
			admin.Show();
		}
	}
}

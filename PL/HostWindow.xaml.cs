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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using BE;
using BL;

namespace PL
{
	/// <summary>
	/// Interaction logic for HostWindow.xaml
	/// </summary>
	//pass data to other window
	

	public partial class HostWindow : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		private Host host = null;
		public Host GetHost { get => host; }

		public HostWindow()
		{
			InitializeComponent();
			
		}
		
		private void Register_Click(object sender, RoutedEventArgs e)
		{
			NewHost host = new NewHost();
			this.Close();
			host.Show();
		}
		private static readonly Regex _regex = new Regex("[^0-9]+");
		private void Login_Click(object sender, RoutedEventArgs e)
		{
			
			//check if email is blank
			if (Email.Text.Length == 0)
			{
				MessageBox.Show("Enter an email.");
				Email.Focus();
				return;
			}
			//check if password is blank
			if (PasswordBox.Password.Length == 0)
			{
				MessageBox.Show("Enter a password.");
				PasswordBox.Focus();
				return;
			}
			if (_regex.IsMatch(PasswordBox.Password))
			{
				//password is hosting unit id
				MessageBox.Show("Wrong password or email");
				Email.Clear();
				PasswordBox.Clear();
				return;
			}
			//check if info is valid 
			if (bl.LoginHost(Email.Text, int.Parse(PasswordBox.Password)))
			{
				host = bl.GetHostData(Email.Text, int.Parse(PasswordBox.Password));
				PersonalPage login = new PersonalPage(host);
				this.Close();
				login.Show();
			}			

		}
		
	}
}

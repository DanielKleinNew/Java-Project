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
	/// Interaction logic for NewHost.xaml
	/// </summary
	/// private GuestRequest guestRequest = null;

	public partial class NewHost : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		private Host host = null;
		public Host GetHost { get => host; }
		private BankBranch bankbranch = null;
		public BankBranch GetBankBranch { get => bankbranch; }

		public NewHost()
		{
			InitializeComponent();
			host = new Host();
			bankbranch = new BankBranch();
			Clearance.SelectedIndex = 1;
			Clearance.ItemsSource = Enum.GetValues(typeof(BE.CollectionClearance));
			
		}

		private void Sumit_Click(object sender, RoutedEventArgs e)
		{
			if (bl.CheckValidEmail(Email.Text) == false)
			{
				Email.Clear();
				Email.Focus();
				return;
			}
			//add info into temp obj and try to see if its legal using BL logic.
			host.HostPrivateName = FirstName.Text;
			host.HostFamilyName = LastName.Text;
			host.HostEmail = Email.Text;
			bankbranch.BankName = BankName.Text;
			bankbranch.BranchAddress = Bankaddress.Text;
			bankbranch.BranchCity = BranchCity.Text;
			//parse string into int
			host.HostPhoneNumber = int.Parse(Phone.Text);
			bankbranch.BankNumber = int.Parse(BankId.Text);
			bankbranch.BranchNumber = int.Parse(BranchNumber.Text);
			host.HostBankAccount = int.Parse(BankNumber.Text);
			//Enum.Parse Method parse sting into enum
			host.HostCollectionClearance = (BE.CollectionClearance)Enum.Parse(typeof(BE.CollectionClearance), Clearance.SelectedItem.ToString());
			//test for legal bank branch
			if (!bl.CheckBankInfo(bankbranch))
			{
				MessageBox.Show("Bank info does not match known Bank Branches");
				return;
			}
			else
			{
				host.HostBankBranch = bankbranch;
				try
				{
					bl.AddHost(host);
				}
				catch (Exception)
				{

					throw;
				}
				HostWindow hostWindow = new HostWindow();
				this.Close();
				hostWindow.Show();
			}
			
		}
		//handle only numbers
		private static readonly Regex _regex = new Regex("[^0-9]+");
		//clear everything
		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			Clearance.SelectedIndex = 1;
			FirstName.Clear();
			LastName.Clear();
			Phone.Clear();
			Email.Clear();
			BankId.Clear();
			BranchNumber.Clear();
			BranchCity.Clear();
			Bankaddress.Clear();
			BankName.Clear();
			BankNumber.Clear();
		}

		private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}

		private void BankNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}

		private void BankId_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}

		private void BranchNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}
	}
	
}

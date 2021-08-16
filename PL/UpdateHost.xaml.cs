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
	/// Interaction logic for UpdateHost.xaml
	/// </summary>
	public partial class UpdateHost : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		private Host host = null;
		public Host GetHost { get => host; }
		private BankBranch bankbranch = null;
		public BankBranch GetBankBranch { get => bankbranch; }

		public UpdateHost(Host data)
		{
			
			InitializeComponent();

			//set up the save data
			host = data;
			bankbranch = host.HostBankBranch;
			//FirstName.Text = host.HostPrivateName;
			//LastName.Text = host.HostFamilyName;
			//trying out using Data Context could be use to clean up a lot of code by binding he values directly to the text box
			this.DataContext = host;
			Email.Text = host.HostEmail;
			BankName.Text = bankbranch.BankName;
			Bankaddress.Text = bankbranch.BranchAddress;
			BranchCity.Text = bankbranch.BranchCity;
			Phone.Text = host.HostPhoneNumber.ToString();
			BankId.Text = bankbranch.BankNumber.ToString();
			BranchNumber.Text = bankbranch.BranchNumber.ToString();
			BankNumber.Text = host.HostBankAccount.ToString();
			Clearance.ItemsSource = Enum.GetValues(typeof(BE.CollectionClearance));
			Clearance.SelectedIndex = Clearance.Items.IndexOf(host.HostCollectionClearance);
		}
		private void Sumit_Click(object sender, RoutedEventArgs e)
		{
			
			//test for legal bank branch
			if (!bl.CheckBankInfo(bankbranch))
			{
				MessageBox.Show("Bank info does not match known Bank Branches");
				return;
			}
			if(host.HostCollectionClearance == CollectionClearance.Yes && Clearance.SelectedItem.ToString() == "NO")
			{
				//check for open orders
				if(bl.CheckBillingStatus(host))
				{
					MessageBox.Show("You can not change collection clearance because of open order");
					Clearance.SelectedIndex = Clearance.Items.IndexOf(host.HostCollectionClearance);
					return;

				}
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

			host.HostBankBranch = bankbranch;
			try
			{
				bl.HostUpdate(host);
			}
			catch (Exception)
			{

				throw;
			}
			this.Close();


		}
		//handle only numbers
		private static readonly Regex _regex = new Regex("[^0-9]+");
		//clear everything
		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			bankbranch = host.HostBankBranch;
			FirstName.Text = host.HostPrivateName;
			LastName.Text = host.HostFamilyName;
			Email.Text = host.HostEmail;
			BankName.Text = bankbranch.BankName;
			Bankaddress.Text = bankbranch.BranchAddress;
			BranchCity.Text = bankbranch.BranchCity;
			Phone.Text = host.HostPhoneNumber.ToString();
			BankId.Text = bankbranch.BankNumber.ToString();
			BranchNumber.Text = bankbranch.BranchNumber.ToString();
			BankNumber.Text = host.HostBankAccount.ToString();
			Clearance.SelectedIndex = Clearance.Items.IndexOf(host.HostCollectionClearance);

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

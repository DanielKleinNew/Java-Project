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
using BL;
using BE;
using System.ComponentModel;

namespace PL
{
	/// <summary>
	/// Interaction logic for GuestWindow.xaml
	/// </summary>
	public partial class GuestWindow : Window 
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		private GuestRequest guestRequest = null;
		public GuestRequest GetGuestRequest { get => guestRequest; }

		public GuestWindow()
		{
			InitializeComponent();
			guestRequest = new GuestRequest();

			RegistrationDate.Text = DateTime.Now.ToString();
			Area.SelectedIndex = 0;
			Type.SelectedIndex = 3;
			Pool.SelectedIndex = 2;
			Jacuzzi.SelectedIndex = 2;
			Garden.SelectedIndex = 2;
			Children.SelectedIndex = 2;
			Area.ItemsSource = Enum.GetValues(typeof(BE.Areas));
			Type.ItemsSource = Enum.GetValues(typeof(BE.Types));
			Pool.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Jacuzzi.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Garden.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Children.ItemsSource = Enum.GetValues(typeof(BE.Decision));


		}

		//apply the information and past it to the BL to check if the info is legal
		private void Apply_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(AdultsNum.Text) ||
				string.IsNullOrWhiteSpace(ChildrenNum.Text) ||
				string.IsNullOrWhiteSpace(FirstName.Text) ||
				string.IsNullOrWhiteSpace(LastName.Text) ||
				string.IsNullOrWhiteSpace(Email.Text)||
				EntryDate.SelectedDate == null ||
				ExitDate.SelectedDate == null)
			{
				MessageBox.Show("Missing Information");
				return;
			}
			if(int.Parse(AdultsNum.Text) < 0 )
			{
				MessageBox.Show("Missing Information");
				AdultsNum.Clear();
				AdultsNum.Focus();
				return;
			}
			if (int.Parse(ChildrenNum.Text) < 0)
			{
				MessageBox.Show("Missing Information");
				ChildrenNum.Clear();
				ChildrenNum.Focus();
				return;
			}
			if(bl.CheckValidEmail(Email.Text) == false)
			{
				Email.Clear();
				Email.Focus();
				return;
			}
			else
			{
				try
				{
					//add info into temp obj and try to see if its legal using BL logic.
					guestRequest.GuestPrivateName = FirstName.Text;
					guestRequest.GuestFamilyName = LastName.Text;
					guestRequest.GuestMailAddress = Email.Text;
					guestRequest.RequestStatus = Statuses.NotAddressed;
					guestRequest.RequestEntryDate = DateTime.Parse(EntryDate.Text);
					guestRequest.RequestReleaseDate = DateTime.Parse(ExitDate.Text);
					//Enum.Parse Method parse sting into enum
					guestRequest.RequestArea = (BE.Areas)Enum.Parse(typeof(BE.Areas), Area.SelectedItem.ToString());
					guestRequest.RequestType = (BE.Types)Enum.Parse(typeof(BE.Types), Type.SelectedItem.ToString());
					//parse string into int
					guestRequest.RequestAdult = int.Parse(AdultsNum.Text);
					guestRequest.RequestChildren = int.Parse(ChildrenNum.Text);
					//Parse the rest of the Enum
					guestRequest.RequestPool = (BE.Decision)Enum.Parse(typeof(BE.Decision), Pool.SelectedItem.ToString());
					guestRequest.RequestJacuzzi = (BE.Decision)Enum.Parse(typeof(BE.Decision), Jacuzzi.SelectedItem.ToString());
					guestRequest.RequestGarden = (BE.Decision)Enum.Parse(typeof(BE.Decision), Garden.SelectedItem.ToString());
					guestRequest.RequestChildAttraction = (BE.Decision)Enum.Parse(typeof(BE.Decision), Children.SelectedItem.ToString());

					bl.AddCustomer(guestRequest);

				}
				catch (Exception ex)
				{

					throw ex;
				}
				
			}
			this.Close();
			MessageBox.Show("Thank you for your request");

		}
		//reset all the option on the page
		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			Area.SelectedIndex = 0;
			Type.SelectedIndex = 3;
			Pool.SelectedIndex = 2;
			Jacuzzi.SelectedIndex = 2;
			Garden.SelectedIndex = 2;
			Children.SelectedIndex = 2;
			AdultsNum.Clear();
			ChildrenNum.Clear();
			FirstName.Clear();
			LastName.Clear();
			Email.Clear();
			EntryDate.SelectedDate = null;
			EntryDate.DisplayDate = DateTime.Today;
			ExitDate.SelectedDate = null;
			ExitDate.SelectedDate = DateTime.Today;
		}

		//https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf/17715402
		//How do I get a TextBox to only accept numeric input in WPF?
		private static readonly Regex _regex = new Regex("[^0-9]+");
		//check that the input for adults is a number
		private void AdultsNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}
		private void ChildrenNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}


	}

}

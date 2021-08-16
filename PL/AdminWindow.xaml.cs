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
using BE;
using BL;
using System.Text.RegularExpressions;
namespace PL
{
	/// <summary>
	/// Interaction logic for AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		List<HostingUnit> hosting_list = new List<HostingUnit>();
		List<Order> order_list = new List<Order>();
		List<GuestRequest> request_list = new List<GuestRequest>();
		List<Host> host_list = new List<Host>();
		public AdminWindow()
		{
			InitializeComponent();
			hosting_list = bl.GetListUnits();
			HostingBox.ItemsSource = hosting_list;
			HostingBox.Items.Refresh();
			order_list = bl.GetListOrders();
			OrderBox.ItemsSource = order_list;
			OrderBox.Items.Refresh();
			request_list = bl.GetListCustomers();
			RequestBox.ItemsSource = request_list;
			RequestBox.Items.Refresh();
			host_list = bl.GetListHost();
			HostBox.ItemsSource = host_list;
			HostBox.Items.Refresh();
			commissionText.Text = BE.Configuration.st_commission.ToString();
			expirationText.Text = BE.Configuration.st_expire_days.ToString();
		}

		private void OrderBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (OrderBox.SelectedItem != null)
			{
				Host host = null;
				host = bl.GetHost((Order)OrderBox.SelectedItem);
				if(host == null)
				{
					MessageBox.Show("Host does not exist anymore");
				}
				ShowOrder showOrder = new ShowOrder((Order)OrderBox.SelectedItem, host);
				showOrder.Show();

			}
		}

		private void RequestBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (RequestBox.SelectedItem != null)
			{
				ShowRequest showRequest = new ShowRequest((GuestRequest)RequestBox.SelectedItem);
				showRequest.Show();
			}
		}

		private void HostingBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (HostingBox.SelectedItem != null)
			{

				ShowUnit showUnit = new ShowUnit((HostingUnit)HostingBox.SelectedItem);
				showUnit.Show();
			}
		}
		private void HostBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (HostBox.SelectedItem != null)
			{

				PersonalPage showHost = new PersonalPage((Host)HostBox.SelectedItem);
				showHost.Show();
			}
		}


		private void RefreshOrder_Click(object sender, RoutedEventArgs e)
		{
			order_list = bl.GetListOrders();
			OrderBox.ItemsSource = order_list;
			OrderBox.Items.Refresh();
			
		}

	
		private void RefreshUnit_Click(object sender, RoutedEventArgs e)
		{
			hosting_list = bl.GetListUnits();
			HostingBox.ItemsSource = hosting_list;
			HostingBox.Items.Refresh();
		}

		
		private void RefreshHost_Click(object sender, RoutedEventArgs e)
		{
			host_list = bl.GetListHost();
			HostBox.ItemsSource = host_list;
			HostBox.Items.Refresh();
		}

	

		private void RefreshRequest_Click(object sender, RoutedEventArgs e)
		{
			request_list = bl.GetListCustomers();
			RequestBox.ItemsSource = request_list;
			RequestBox.Items.Refresh();
			
		}

		private void GroupByAreaUnit_Click(object sender, RoutedEventArgs e)
		{ 
			
			List<HostingUnit> tempList = new List<HostingUnit>();
			IEnumerable<IGrouping<Areas, HostingUnit>> groups = bl.GroupUnitByArea();
			IEnumerable<HostingUnit> smths = groups.SelectMany(group => group);
			HostingBox.ItemsSource = smths;
			HostingBox.Items.Refresh();
			
		}

		private void GroupByAreaRequest_Click(object sender, RoutedEventArgs e)
		{
			List<GuestRequest> tempList = new List<GuestRequest>();
			IEnumerable<IGrouping<Areas, GuestRequest>> groups = bl.GroupRequestsByArea();
			IEnumerable<GuestRequest> smths = groups.SelectMany(group => group);
			RequestBox.ItemsSource = smths;
			RequestBox.Items.Refresh();
			
		}

		private void GroupByPeepsRequest_Click(object sender, RoutedEventArgs e)
		{
			List<GuestRequest> tempList = new List<GuestRequest>();
			IEnumerable<IGrouping<int, GuestRequest>> groups = bl.GroupRequestByAccupants();
			IEnumerable<GuestRequest> smths = groups.SelectMany(group => group);
			RequestBox.ItemsSource = smths;
			RequestBox.Items.Refresh();
		}

		private void GroupByPeepsUnit_Click(object sender, RoutedEventArgs e)
		{
			List<HostingUnit> tempList = new List<HostingUnit>();
			IEnumerable<IGrouping<int, HostingUnit>> groups = bl.GroupUnitByAccupants();
			IEnumerable<HostingUnit> smths = groups.SelectMany(group => group);
			HostingBox.ItemsSource = smths;
			HostingBox.Items.Refresh();
		}

		private void UnitStatClosed_Click(object sender, RoutedEventArgs e)
		{
			if (HostingBox.SelectedItem == null)
			{
				MessageBox.Show("Please select Hosting unit to see stats");
			}
			else
			{
				int temp = 0;
				temp = bl.UnitStatClosed((HostingUnit)HostingBox.SelectedItem);
				MessageBox.Show("Number of orders closed by this unit are " + temp.ToString());
			}
		}

		private void UnitStatSent_Click(object sender, RoutedEventArgs e)
		{
			if (HostingBox.SelectedItem == null)
			{
				MessageBox.Show("Please select Hosting unit to see stats");
			}
			else
			{
				int temp = 0;
				temp = bl.UnitStatOrders((HostingUnit)HostingBox.SelectedItem);
				MessageBox.Show("Number of orders sent through this unit are " + temp.ToString());
			}
		
		}

		private void RequestStat_Click(object sender, RoutedEventArgs e)
		{
			if (RequestBox.SelectedItem == null)
			{
				MessageBox.Show("Please select Request unit to see stats");
			}
			else
			{
				int temp = 0;
				temp = bl.RequestStat((GuestRequest)RequestBox.SelectedItem);
				MessageBox.Show("Number of orders sent to this request are " + temp.ToString());
		}
			
		}

		private void Month1_Click_1(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(1).ToString());
		}

		private void Month2_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(2).ToString());
		}

		private void Month3_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(3).ToString());
		}

		private void Month4_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(4).ToString());
		}

		private void Month5_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(5).ToString());
		}

		private void Month6_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(6).ToString());
		}

		private void Month7_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(7).ToString());
		}

		private void Month8_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(8).ToString());
		}

		private void Month9_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(9).ToString());
		}

		private void Month10_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(10).ToString());
		}

		private void Month11_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(11).ToString());
		}

		private void Month12_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Total Commission for this month are " + bl.GetCommission(12).ToString());
		}

		private void OrderSent_Click(object sender, RoutedEventArgs e)
		{
			if(int.Parse(OrderDays.Text) < 0)
			{
				MessageBox.Show("must be a positive number");
				OrderDays.Text = "";
				OrderDays.Focus();
			}
			else
			{
				order_list = bl.OrderFunction(int.Parse(OrderDays.Text));
				OrderBox.ItemsSource = order_list;
				OrderBox.Items.Refresh();
			}
		}

		private void UpdateCommission_Click(object sender, RoutedEventArgs e)
		{
			if(commissionUpdate.Text == "")
			{
				MessageBox.Show("please enter a number");
				commissionUpdate.Focus();
			}
			else if(Convert.ToInt32(commissionUpdate.Text) < 0)
			{
				MessageBox.Show("please enter a positive number");
				commissionUpdate.Text = "";
				commissionUpdate.Focus();
			}
			else
			{
				//update
				try
				{
					bl.ConfigurationUpdate(Convert.ToDouble(commissionUpdate.Text), BE.Configuration.st_expire_days);
					commissionUpdate.Text = "";
					commissionText.Text = BE.Configuration.st_commission.ToString();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return;
				}
				
			}
			

		}

		private void UpdateDate_Click(object sender, RoutedEventArgs e)
		{

			if (expirationUpdate.Text == "")
			{
				MessageBox.Show("please enter a number");
				expirationUpdate.Focus();
			}
			else if (Convert.ToInt32(expirationUpdate.Text) < 0)
			{
				MessageBox.Show("please enter a positive number");
				expirationUpdate.Text = "";
				expirationUpdate.Focus();
			}
			else
			{
				//update
				try
				{
					bl.ConfigurationUpdate(BE.Configuration.st_commission, Convert.ToInt32(expirationUpdate.Text));
					expirationUpdate.Text = "";
					expirationText.Text = BE.Configuration.st_expire_days.ToString();
				}
				catch (Exception ex)
				{

					MessageBox.Show(ex.ToString());
					return;
				}
				
			}
			

		}

		//handle only numbers
		private static readonly Regex _regex = new Regex("[^0-9]+");
		private static readonly Regex decimal_regex = new Regex("[^0-9]+");
		private void AdultsNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}

		private void commissionUpdate_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = decimal_regex.IsMatch(e.Text);
		}

		private void expirationUpdate_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}

		private void OrderDays_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}


	}
}

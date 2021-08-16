using BE;
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
using System.Collections.ObjectModel;
using BL;
using System.ComponentModel;

namespace PL
{
	/// <summary>
	/// Interaction logic for PersonalPage.xaml
	/// </summary>

	public partial class PersonalPage : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		//The ListBox control
		//https://www.wpf-tutorial.com/list-controls/listbox-control/
		List<HostingUnit> hosting_list = new List<HostingUnit>();
		List<Order> order_list = new List<Order>();
		List<GuestRequest> request_list = new List<GuestRequest>();
		//data about the hosting unit
		private HostingUnit hostingUnit = null;
		public HostingUnit GetHostingUnit { get => hostingUnit; }
		//data about the host
		private Host host = null;
		public Host GetHost { get => host; }
		private GuestRequest request = null;
		public GuestRequest GetRequest { get => request; }
		private Order order = null;
		public Order GetOrder { get => order; }



		public PersonalPage(Host data)
		{
			InitializeComponent();
			host = data;


			//set up all the list boxes
			hosting_list = bl.GetHostListUnits(host.HostKey);
			HostingBox.ItemsSource = hosting_list;
			HostingBox.Items.Refresh();
			CalcFirstDay.Text = default(DateTime).ToString();
			CalcSecondDay.Text = default(DateTime).ToString();


			AreaRequest.ItemsSource = Enum.GetValues(typeof(BE.Areas));
			TypeRequest.ItemsSource = Enum.GetValues(typeof(BE.Types));
			PoolRequest.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			JacuzziRequest.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			GardenRequest.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			AttractionRequest.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			EntryDateRequest.SelectedDate = null;
			EntryDateRequest.SelectedDate = DateTime.Today;
			ExitDateRequest.SelectedDate = null;
			ExitDateRequest.SelectedDate = DateTime.Today;
			StatusRequest.ItemsSource = Enum.GetValues(typeof(BE.Statuses));


		}
	

		//updated hosted unit that is selected 
		private void UpdateHostingUnit_Click(object sender, RoutedEventArgs e)
		{
			if (HostingBox.SelectedItem != null)
			{
				hostingUnit = ((HostingUnit)HostingBox.SelectedItem);
				UpdateHostingUnit update = new UpdateHostingUnit(hostingUnit);
				update.ShowDialog();
			}
			else
			{
				MessageBox.Show("No Item Selected or Available");
			}
		}


		//add hosting unit
		private void AddHostingUnit_Click(object sender, RoutedEventArgs e)
		{

			NewHostingUnit newUnit = new NewHostingUnit(host);
			newUnit.ShowDialog();

			hosting_list = bl.GetHostListUnits(host.HostKey);
			HostingBox.ItemsSource = hosting_list;
			HostingBox.Items.Refresh();

		}

		//delete selected hosting unit
		private void DeleteHostingUnit_Click(object sender, RoutedEventArgs e)
		{
			//https://stackoverflow.com/questions/25620518/wpf-c-sharp-how-to-get-selected-value-of-listbox
			//WPF: C# How to get selected value of Listbox?
			if (HostingBox.SelectedIndex >= 0)
			{

				//get the date of inside the ListBox
				dynamic obj = HostingBox.SelectedItem as dynamic;
				int unitId = obj.HostingUnitKey;

				hostingUnit = hosting_list.Find(item => item.HostingUnitKey == unitId);

				try
				{
					//Delete the item
					if (bl.DeleteHostingUnit(hostingUnit) == true)
					{
						MessageBox.Show("Hosting Unit Was Deleted");
						//get updated list and apply it to list box source.
						hosting_list = bl.GetListUnits();
						HostingBox.ItemsSource = hosting_list;
						HostingBox.Items.Refresh();
					}
					else
					{
						MessageBox.Show("Not Possible To Delete the Hosting Unit");
					}
				}
				catch (Exception)
				{

					throw;
				}

			}
			else
			{
				MessageBox.Show("No Item Selected or Available");
			}
		}
		//search for all hosting unit
		private void RefreshUnit_Click(object sender, RoutedEventArgs e)
		{
			//set up all the list boxes
			SearchCheckBox.IsChecked = false;
			PickDatesUnit.Clear();
			PickUnit.SelectedDate = null;
			PickUnit.SelectedDate = DateTime.Today;			
			hosting_list = bl.GetHostListUnits(host.HostKey);
			HostingBox.ItemsSource = hosting_list;
		}

		//Search for hosting unit by available days
		private void RefreshUnit2_Click(object sender, RoutedEventArgs e)
		{

			hosting_list = bl.GetHostListUnits(host.HostKey);
			HostingBox.ItemsSource = hosting_list;
			HostingBox.Items.Refresh();

		}
		private void UpdateHost_Click(object sender, RoutedEventArgs e)
		{
			UpdateHost add = new UpdateHost(host);
			add.ShowDialog();
		}

		private void OrderBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (OrderBox.SelectedItem != null)
			{
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

		private void ResetRequest_Click_1(object sender, RoutedEventArgs e)
		{
			AreaRequest.SelectedIndex = 0;
			TypeRequest.SelectedIndex = 3;
			PoolRequest.SelectedIndex = 1;
			JacuzziRequest.SelectedIndex = 1;
			GardenRequest.SelectedIndex = 1;
			AttractionRequest.SelectedIndex = 1;
			AdultRequest.Clear();
			ChildrenRequest.Clear();
			EntryDateRequest.SelectedDate = null;
			EntryDateRequest.SelectedDate = DateTime.Today;
			ExitDateRequest.SelectedDate = null;
			ExitDateRequest.SelectedDate = DateTime.Today;

		}

		private void RefreshRequest_Click_1(object sender, RoutedEventArgs e)
		{
			//check for the request that fir the amount of days and date
			if (RequestAll.IsChecked == true)
			{
				request_list = bl.FilterGuestRequests(delegate (GuestRequest item)
				{
					return item.RequestStatus.ToString() == StatusRequest.Text &&
					  item.RequestArea.ToString() == AreaRequest.Text && item.RequestType.ToString() == TypeRequest.Text && item.RequestJacuzzi.ToString() == JacuzziRequest.Text &&
					  item.RequestGarden.ToString() == GardenRequest.Text && item.RequestChildAttraction.ToString() == AttractionRequest.Text &&
					  item.RequestPool.ToString() == PoolRequest.Text && item.RequestPool.ToString() == PoolRequest.Text && item.RequestAdult  <= Int32.Parse(AdultRequest.Text) &&
					  item.RequestChildren <= Int32.Parse(ChildrenRequest.Text) && item.RequestEntryDate >= DateTime.Parse(EntryDateRequest.Text) && item.RequestReleaseDate <= DateTime.Parse(ExitDateRequest.Text);
				});
				RequestBox.ItemsSource = request_list;
				RequestBox.Items.Refresh();
				return;
			}
			request_list = bl.GetListOpenCustomers();
			RequestBox.ItemsSource = request_list;
			RequestBox.Items.Refresh();

		}

		//add new order
		private void AddRequest_Click(object sender, RoutedEventArgs e)
		{
			

			if (RequestBox.SelectedItem != null && HostingBox.SelectedItem != null)
			{
				//create order
				try					
				{
					bl.MakeOrder((HostingUnit)HostingBox.SelectedItem, (GuestRequest)RequestBox.SelectedItem);
					MessageBox.Show("Order has been made and will appear in Order list box");
					order_list = bl.GetHostOrders(host.HostKey);
					OrderBox.ItemsSource = order_list;
					OrderBox.Items.Refresh();


				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return;
				}
				
			}
			else if (RequestBox.SelectedItem == null && HostingBox.SelectedItem != null)
			{
				MessageBox.Show("Please select a Request by clicking on a request in the list below");
				RequestBox.Focus();
			}
			else if (RequestBox.SelectedItem != null && HostingBox.SelectedItem == null)
			{
				MessageBox.Show("Please select a Hosting Unit by click on a unit in the hosting unit list");
				HostingBox.Focus();
			}
			else
			{
				MessageBox.Show("Please select a Unit and a Request");
				RequestBox.Focus();
			}

		}

		private void OrderStatusUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (OrderBox.SelectedItem != null)
			{ 
				//update status to closed
				try
				{
					bl.ChangeOrderStatus((Order)OrderBox.SelectedItem);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return;
					
				}
			}
			order_list = bl.GetHostOrders(host.HostKey);
			OrderBox.ItemsSource = order_list;
			OrderBox.Items.Refresh();
			hosting_list = bl.GetHostListUnits(host.HostKey);
			HostingBox.ItemsSource = hosting_list;
			HostingBox.Items.Refresh();
			request_list = bl.GetListOpenCustomers();
			RequestBox.ItemsSource = request_list;
			RequestBox.Items.Refresh();
		}

		private void RefreshOrder_Click(object sender, RoutedEventArgs e)
		{
		
			order_list = bl.GetHostOrders(host.HostKey);
			OrderBox.ItemsSource = order_list;
			OrderBox.Items.Refresh();
		
		}

	

		private void CalcTotalDay_Click(object sender, RoutedEventArgs e)
		{
			if (DateTime.Parse(CalcFirstDay.Text) == default(DateTime))
			{
				CalcFirstDay.Text = default(DateTime).ToString();
				CalcSecondDay.Text = default(DateTime).ToString();
				CalcFirstDay.Focus();
				MessageBox.Show("Only the second day can be left as default");
				return;
			}
			
			TotalDay.Text = bl.CheckNumberDay(DateTime.Parse(CalcFirstDay.Text), DateTime.Parse(CalcSecondDay.Text)).ToString();
		}

		
	}
}
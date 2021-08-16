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
using BL;

namespace PL
{
	/// <summary>
	/// Interaction logic for ShowOrder.xaml
	/// </summary>
	
	public partial class ShowOrder : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		//data about the hosting unit
		
		List<GuestRequest> request_list = new List<GuestRequest>();
		private GuestRequest request = null;
		public GuestRequest GetRequest { get => request; }
		private HostingUnit unit = null;
		public HostingUnit GetUnit { get => unit; }
		public ShowOrder(Order data, Host host)
		{
			InitializeComponent();
			//unit = bl.GetUnitData(host.HostKey);
			unit = bl.GetUnitOfOrder(data, host.HostKey);
			request = bl.GetRequestData(data.OrderRequestKey);

			UnitKey.Text = unit.HostingUnitKey.ToString();
			RequestKey.Text = request.GuestRequestKey.ToString();
			EntryDate.Text = request.RequestEntryDate.ToString();
			ExitDate.Text = request.RequestReleaseDate.ToString();
			AdultsNum.Text = request.RequestAdult.ToString();
			ChildrenNum.Text = request.RequestChildren.ToString();
			MaxAdultsNum.Text = unit.UnitAdult.ToString();
			MaxChildrenNum.Text = unit.UnitChildren.ToString();

			Area.ItemsSource = Enum.GetValues(typeof(BE.Areas));
			Type.ItemsSource = Enum.GetValues(typeof(BE.Types));
			Pool.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Jacuzzi.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Garden.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Children.ItemsSource = Enum.GetValues(typeof(BE.Decision));
			Status.ItemsSource = Enum.GetValues(typeof(BE.Statuses));
			Area.SelectedIndex = Area.Items.IndexOf(request.RequestArea);
			Type.SelectedIndex = Type.Items.IndexOf(request.RequestType);
			Pool.SelectedIndex = Pool.Items.IndexOf(request.RequestPool);
			Jacuzzi.SelectedIndex = Jacuzzi.Items.IndexOf(request.RequestJacuzzi);
			Garden.SelectedIndex = Garden.Items.IndexOf(request.RequestGarden);
			Children.SelectedIndex = Children.Items.IndexOf(request.RequestChildAttraction);
			AdultsNum.Text = request.RequestAdult.ToString();
			ChildrenNum.Text = request.RequestChildren.ToString();
			ExitDate.Text = request.RequestReleaseDate.ToString();
			EntryDate.Text = request.RequestEntryDate.ToString();
			Status.SelectedIndex = Status.Items.IndexOf(data.OrderStatus);
		}
	}
}

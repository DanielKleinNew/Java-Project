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

namespace PL
{
	/// <summary>
	/// Interaction logic for ShowRequest.xaml
	/// </summary>
	public partial class ShowRequest : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		//data about the hosting unit
		private GuestRequest request = null;
		public GuestRequest GetRequest { get => request; }
		public ShowRequest(GuestRequest data)
		{
			InitializeComponent();
			request = data;
			RegistrationDate.Text = request.RegistrationDate.ToString();
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
			Status.SelectedIndex = Status.Items.IndexOf(request.RequestStatus);
		}
	}
}

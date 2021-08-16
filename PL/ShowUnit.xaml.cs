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
	/// Interaction logic for SowUnit.xaml
	/// </summary>
	public partial class ShowUnit : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		//data about the hosting unit
		private HostingUnit hostingUnit = null;
		public HostingUnit GetHostingUnit { get => hostingUnit; }
		public ShowUnit(HostingUnit data)
		{
			InitializeComponent();
			hostingUnit = data;
			//get previous save data
			Location.Text = hostingUnit.Location;
			ChildrenNum.Text = (hostingUnit.UnitChildren).ToString();
			AdultsNum.Text = (hostingUnit.UnitAdult).ToString();
			Price.Text = (hostingUnit.UnitPrice).ToString();
			Name.Text = hostingUnit.HostingUnitName;
			//set the enum source 
			Area.ItemsSource = Enum.GetValues(typeof(BE.Areas));
			Type.ItemsSource = Enum.GetValues(typeof(BE.Types));
			Pool.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Jacuzzi.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Garden.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Children.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Area.SelectedIndex = Area.Items.IndexOf(hostingUnit.UnitArea);
			Type.SelectedIndex = Type.Items.IndexOf(hostingUnit.UnitType);
			Pool.SelectedIndex = Pool.Items.IndexOf(hostingUnit.UnitPool);
			Jacuzzi.SelectedIndex = Jacuzzi.Items.IndexOf(hostingUnit.UnitJacuzzi);
			Garden.SelectedIndex = Garden.Items.IndexOf(hostingUnit.UnitGarden);
			Children.SelectedIndex = Children.Items.IndexOf(hostingUnit.UnitChildAttraction);

		}
	}
}

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
	/// Interaction logic for NewHostingUnit.xaml
	/// </summary>
	

	public partial class NewHostingUnit : Window
	{
		BL.IBL bl = BL.BL_Factory.GetBL();
		private HostingUnit hostingUnit = null;
		public HostingUnit GetHostingUnit { get => hostingUnit; }
		//data about the host
		private Host host = null;
		public Host GetHost { get => host; }

		public NewHostingUnit(Host data)
		{
			InitializeComponent();
			hostingUnit = new HostingUnit();
			host = data;
			
			//get passed info of the host from other window
			FirstName.Text = host.HostPrivateName;
			LastName.Text = host.HostFamilyName;
			Area.SelectedIndex = 0;
			Type.SelectedIndex = 3;
			Pool.SelectedIndex = 1;
			Jacuzzi.SelectedIndex = 1;
			Garden.SelectedIndex = 1;
			Children.SelectedIndex = 1;
			Area.ItemsSource = Enum.GetValues(typeof(BE.Areas));
			Type.ItemsSource = Enum.GetValues(typeof(BE.Types));
			Pool.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Jacuzzi.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Garden.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));
			Children.ItemsSource = Enum.GetValues(typeof(BE.DecisionUnit));



		}
		
		//handle only numbers
		private static readonly Regex _regex = new Regex("[^0-9]+");
		private void AdultsNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}

		//check if the input for children is a number
		private void ChildrenNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}
		//check that there are numbers in the price box
		private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = _regex.IsMatch(e.Text);
		}
		//Apply_Click
		private void Apply_Click(object sender, RoutedEventArgs e)
		{
			if (int.Parse(AdultsNum.Text) < 0)
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
			if (float.Parse(Price.Text) <= 0)
			{
				MessageBox.Show("Missing Information");
				Price.Clear();
				Price.Focus();
				return;
			}
			try
			{
				//add info into temp obj and try to see if its legal using BL logic.
				hostingUnit.UnitPrice = float.Parse(Price.Text);
				hostingUnit.Location = Location.Text;
				hostingUnit.UnitArea = (BE.Areas)Enum.Parse(typeof(BE.Areas), Area.SelectedItem.ToString());
				hostingUnit.UnitType = (BE.Types)Enum.Parse(typeof(BE.Types), Type.SelectedItem.ToString());
				//parse string into int
				hostingUnit.UnitAdult = int.Parse(AdultsNum.Text);
				hostingUnit.UnitChildren = int.Parse(ChildrenNum.Text);
				//Parse the rest of the Enum
				hostingUnit.UnitPool = (BE.DecisionUnit)Enum.Parse(typeof(BE.DecisionUnit), Pool.SelectedItem.ToString());
				hostingUnit.UnitJacuzzi = (BE.DecisionUnit)Enum.Parse(typeof(BE.DecisionUnit), Jacuzzi.SelectedItem.ToString());
				hostingUnit.UnitGarden = (BE.DecisionUnit)Enum.Parse(typeof(BE.DecisionUnit), Garden.SelectedItem.ToString());
				hostingUnit.UnitChildAttraction = (BE.DecisionUnit)Enum.Parse(typeof(BE.DecisionUnit), Children.SelectedItem.ToString());
				hostingUnit.UnitOwner = host;
				hostingUnit.HostingUnitName = Name.Text;
				bl.AddHostingUnit(hostingUnit);
			}
			catch (Exception)
			{

				throw;
			}
			this.Close();
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			Area.SelectedIndex = 0;
			Type.SelectedIndex = 3;
			Pool.SelectedIndex = 2;
			Jacuzzi.SelectedIndex = 2;
			Garden.SelectedIndex = 2;
			Children.SelectedIndex = 2;
			Location.Clear();
			AdultsNum.Clear();
			ChildrenNum.Clear();
			Price.Clear();
			Name.Clear();
		}

		
	}
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FleetClients.Controls
{
	/// <summary>
	/// Interaction logic for FleetTemplateControl.xaml
	/// </summary>
	public partial class FleetTemplateControl : UserControl
	{
		public FleetTemplateControl()
		{
			InitializeComponent();
		}

		private void PopulateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				IFleetManagerClient client = DataContext as IFleetManagerClient;
				FleetTemplate fleetTemplate = FindResource("fleetTemplate") as FleetTemplate;

				fleetTemplate.Populate(client);

			}
			catch (Exception ex)
			{

			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			FleetTemplate fleetTemplate = FindResource("fleetTemplate") as FleetTemplate;

			fleetTemplate.Add(new AGVTemplate());
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			FleetTemplate fleetTemplate = FindResource("fleetTemplate") as FleetTemplate;
			fleetTemplate.Clear();
		}
	}
}

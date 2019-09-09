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
using BaseClients;
using System.Windows.Shapes;

namespace FleetClients.DemoApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			HandleInit();
		}

		private void HandleInit()
		{
			IFleetManagerClient client = FleetClients.ClientFactory.CreateTcpFleetManagerClient(new EndpointSettings(System.Net.IPAddress.Loopback));

			FleetManagerClientControlWindow window = new FleetManagerClientControlWindow()
			{
				DataContext = client
			};

			window.Show();

            this.DataContext = client;

            Close();
		}
	}
}

using BaseClients;
using System.Windows;

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
		}

		private void HandleClose()
		{
			Application.Current.Shutdown();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			HandleClose();
		}

		private void FmcControlButton_Click(object sender, RoutedEventArgs e)
		{
			using (IFleetManagerClient client = FleetClients.ClientFactory.CreateTcpFleetManagerClient(new EndpointSettings(System.Net.IPAddress.Loopback)))
			{
				FleetManagerClientControlWindow window = new FleetManagerClientControlWindow()
				{
					DataContext = client
				};

				window.ShowDialog();
			}		
		}
	}
}

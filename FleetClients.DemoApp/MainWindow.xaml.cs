using BaseClients;
using System.Net;
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
			IPAddress ipAddress = ipV4Control.ToIPAddress();

			if (ipAddress == null)
			{
				MessageBox.Show("IPv4 Address is invalid", "Invalid IP Address", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			using (IFleetManagerClient client = FleetClients.ClientFactory.CreateTcpFleetManagerClient(new EndpointSettings(ipAddress)))
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

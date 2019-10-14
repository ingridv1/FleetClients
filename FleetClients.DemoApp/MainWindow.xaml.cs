using BaseClients;
using System.Net;
using System.Windows;
using Markdig;

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

			string html = Markdown.ToHtml(Properties.Resources.MainWindowDescription);
			webBrowser.NavigateToString(html);
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

		private void FtControlButton_Click(object sender, RoutedEventArgs e)
		{
			IPAddress ipAddress = ipV4Control.ToIPAddress();

			if (ipAddress == null)
			{
				MessageBox.Show("IPv4 Address is invalid", "Invalid IP Address", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			using (IFleetManagerClient client = FleetClients.ClientFactory.CreateTcpFleetManagerClient(new EndpointSettings(ipAddress)))
			{
				FleetTemplateControlWindow window = new FleetTemplateControlWindow()
				{
					DataContext = client
				};

				window.ShowDialog();
			}
		}
	}
}

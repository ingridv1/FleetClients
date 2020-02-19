using System.Windows;

namespace FleetClients.DemoApp
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Bootstrapper.Activate();
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
	}
}

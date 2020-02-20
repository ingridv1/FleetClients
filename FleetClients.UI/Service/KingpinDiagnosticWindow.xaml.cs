using System.Windows;

namespace FleetClients.UI.Service
{
	public partial class KingpinDiagnosticWindow : Window
	{
		public KingpinDiagnosticWindow()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
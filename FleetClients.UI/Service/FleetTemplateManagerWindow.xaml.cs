using System.Windows;

namespace FleetClients.UI.Service
{
	public partial class FleetTemplateManagerWindow : Window
	{
		public FleetTemplateManagerWindow()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}

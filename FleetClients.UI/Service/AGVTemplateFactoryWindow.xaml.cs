using System.Windows;

namespace FleetClients.UI.Service
{
	public partial class AGVTemplateFactoryWindow : Window
	{
		public AGVTemplateFactoryWindow()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
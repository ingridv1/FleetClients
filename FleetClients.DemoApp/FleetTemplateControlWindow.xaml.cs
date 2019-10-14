using Markdig;
using System;
using System.Linq;
using System.Windows;

namespace FleetClients.DemoApp
{
	/// <summary>
	/// Interaction logic for FleetTemplateControlWindow.xaml
	/// </summary>
	public partial class FleetTemplateControlWindow : Window
	{
		public FleetTemplateControlWindow()
		{
			InitializeComponent();

			string html = Markdown.ToHtml(Properties.Resources.FleetTemplateControlDescription);
			webBrowser.NavigateToString(html);

			fleetTemplateControl.DataContext = new FleetTemplate();
		}

		private void PopulateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				IFleetManagerClient client = DataContext as IFleetManagerClient;
				FleetTemplate fleetTemplate = fleetTemplateControl.DataContext as FleetTemplate;

				foreach(AGVTemplate agvTemplate in fleetTemplate.AGVTemplates.ToList())
				{
					client.TryCreateVirtualVehicle(agvTemplate.GetIPV4Address(), agvTemplate.ToPoseData(), out bool result);
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}

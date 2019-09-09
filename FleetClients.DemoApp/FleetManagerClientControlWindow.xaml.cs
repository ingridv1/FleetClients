using System.Windows;
using Markdig;

namespace FleetClients.DemoApp
{
	public partial class FleetManagerClientControlWindow : Window
	{
		public FleetManagerClientControlWindow()
		{
			InitializeComponent();

			string html = Markdown.ToHtml(Properties.Resources.FleetManagerClientControlDescription);
			webBrowser.NavigateToString(html);
		}
	}
}
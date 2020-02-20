using Markdig;
using System.Windows;
using System.Windows.Controls;

namespace FleetClients.DemoApp.Service
{
	public partial class FleetTemplateManagerTutorialWindow : Window
	{
		public FleetTemplateManagerTutorialWindow()
		{
			InitializeComponent();
		}

		private void webBrowser_Loaded(object sender, RoutedEventArgs e)
		{
			string html = Markdown.ToHtml(Properties.Resources.FleetTemplateControlDescription);
			WebBrowser webBrowser = (WebBrowser)sender;
			webBrowser.NavigateToString(html);
		}
	}
}

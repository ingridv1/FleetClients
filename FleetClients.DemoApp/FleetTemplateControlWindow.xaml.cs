using Markdig;
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
		}
	}
}

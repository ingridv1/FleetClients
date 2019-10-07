using System.Windows;
using System.Windows.Controls;

namespace FleetClients.Controls
{
	public partial class AGVTemplateControl : UserControl
	{
		public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent("Delete", RoutingStrategy.Bubble,
			typeof(RoutedEventHandler), typeof(AGVTemplateControl));

		public event RoutedEventHandler Delete
		{
			add { AddHandler(DeleteEvent, value); }
			remove { RemoveHandler(DeleteEvent, value); }
		}

		public AGVTemplateControl()
		{
			InitializeComponent();
		}

		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
			=> RaiseEvent(new RoutedEventArgs(AGVTemplateControl.DeleteEvent));
	}
}
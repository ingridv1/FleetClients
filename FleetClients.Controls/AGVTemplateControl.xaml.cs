using FleetClients.FleetManagerServiceReference;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

		private void IpV4TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ipV4TextBox.Background = IPAddress.TryParse(ipV4TextBox.Text, out IPAddress ipAddress) ? Brushes.White : Brushes.Crimson;
		}

		private void PoseDataTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			poseDataTextBox.Background = PoseDataFactory.TryParseString(poseDataTextBox.Text, out PoseData poseData) ? Brushes.White : Brushes.Crimson;
		}
	}
}
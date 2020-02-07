using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FleetClients.Controls
{
	public partial class FleetTemplateControl : UserControl
	{
		public FleetTemplateControl()
		{
			InitializeComponent();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			FleetTemplate fleetTemplate = DataContext as FleetTemplate;
			fleetTemplate.Add(new AGVTemplate());
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			FleetTemplate fleetTemplate = DataContext as FleetTemplate;
			fleetTemplate.Clear();
		}

		private void AGVTemplateControl_Delete(object sender, RoutedEventArgs e)
		{
			try
			{
				AGVTemplate agvTemplate = listBox.SelectedItem as AGVTemplate;

				FleetTemplate fleetTemplate = DataContext as FleetTemplate;
				fleetTemplate.Remove(agvTemplate);
			}
			catch (Exception ex)			
			{
			}
		}
	}
}

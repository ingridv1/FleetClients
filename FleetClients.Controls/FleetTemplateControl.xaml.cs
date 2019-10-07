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

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				FleetTemplate fleetTemplate = DataContext as FleetTemplate;
				SaveFileDialog dialog = DialogFactory.GetSaveJsonDialog();

				if (dialog.ShowDialog() == true) fleetTemplate.ToFile(dialog.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Failed to save fleet template", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog dialog = DialogFactory.GetOpenJsonDialog();

				if (dialog.ShowDialog() == true)
				{
					FleetTemplate parsedTemplate = JsonFactory.FleetTemplateFromFile(dialog.FileName);

					if (parsedTemplate != null)
					{
						DataContext = parsedTemplate;
						UpdateLayout();
					}
				}
			}
			catch (Exception ex)
			{
			}
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

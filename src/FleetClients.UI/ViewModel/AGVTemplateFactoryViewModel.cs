using GACore;
using GACore.Command;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace FleetClients.UI.ViewModel
{
	public class AGVTemplateFactoryViewModel : AbstractViewModel<FleetTemplateManager>
	{
		private string ipV4string = "192.168.0.1";

		public string IPV4string
		{
			get { return ipV4string; }
			set
			{
				ipV4string = value;
				OnNotifyPropertyChanged();
			}
		}

		private string poseString = "0,0,0";

		public string PoseString
		{
			get { return poseString; }
			set
			{
				poseString = value;
				OnNotifyPropertyChanged();
			}
		}

		public AGVTemplateFactoryViewModel()
		{
			HandleLoadCommands();
		}

		private void HandleLoadCommands()
		{
			CreateCommand = new CustomCommand(CreateCommandClick, CanCreateCommandClick);
		}

		private bool CanCreateCommandClick(object obj) => true;

		private void CreateCommandClick(object obj)
		{
			try
			{
				if (!IPAddress.TryParse(ipV4string, out IPAddress ipV4parsed))
				{
					MessageBox.Show("IP address mst be in v4 format eg: 192.168.0.1", "IP address invalid", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (!PoseDataFactory.TryParseString(poseString, out _))
				{
					MessageBox.Show("Pose must be in the format: xx,yy,hh", "Pose string invalid", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				AGVTemplate template = new AGVTemplate()
				{
					IPV4String = ipV4parsed.ToString(),
					PoseDataString = poseString
				};

				Model.FleetTemplate.Add(template);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		public ICommand CreateCommand { get; set; }

		protected override void HandleModelUpdate(FleetTemplateManager oldValue, FleetTemplateManager newValue)
		{
			base.HandleModelUpdate(oldValue, newValue);
		}
	}
}
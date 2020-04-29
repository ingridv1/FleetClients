using FleetClients.Core;
using GACore;

namespace FleetClients.UI.ViewModel
{
	public class AGVTemplateViewModel : AbstractViewModel<AGVTemplate>
	{
		private string poseDataString = string.Empty;

		public string PoseDataString
		{
			get { return poseDataString; }
			private set
			{
				if (poseDataString != value)
				{
					poseDataString = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		private string ipV4String = string.Empty;

		public string IPV4String
		{
			get { return ipV4String; }
			private set
			{
				if (ipV4String != value)
				{
					ipV4String = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		public AGVTemplateViewModel()
		{
		}

		protected override void HandleModelUpdate(AGVTemplate oldValue, AGVTemplate newValue)
		{
			IPV4String = (newValue != null) ? newValue.IPV4String : string.Empty;
			PoseDataString = (newValue != null) ? newValue.PoseDataString : string.Empty;

			base.HandleModelUpdate(oldValue, newValue);
		}
	}
}
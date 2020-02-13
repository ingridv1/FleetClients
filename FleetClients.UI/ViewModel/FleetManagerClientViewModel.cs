using GACore;
using FleetClients;

namespace FleetClients.UI.ViewModel
{
	public class FleetManagerClientViewModel : AbstractViewModel<IFleetManagerClient>
	{
		public FleetManagerClientViewModel()
		{
		}

		private string endpointString = string.Empty;

		public string EndpointString
		{
			get { return endpointString; }
			private set
			{
				if (endpointString != value)
				{
					endpointString = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		protected override void HandleModelUpdate(IFleetManagerClient oldValue, IFleetManagerClient newValue)
		{
			EndpointString = (newValue != null) ? newValue.EndpointAddress.ToString() : string.Empty;
		}
	}
}

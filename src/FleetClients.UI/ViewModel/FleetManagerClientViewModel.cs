using GACore;
using GACore.Controls;
using System.Linq;

namespace FleetClients.UI.ViewModel
{
	public class FleetManagerClientViewModel : AbstractViewModel<IFleetManagerClient>
	{
		public FleetManagerClientViewModel()
		{      
			// 2020 and still doing this since 2010...
			BrushDictionaries.DynamicLimiterStatusBrushCollectionDictionary.First();
		}

		private bool isConnected = false;

		public bool IsConnected
		{
			get { return isConnected; }
			private set
			{
				if (isConnected != value)
				{
					isConnected = value;
					OnNotifyPropertyChanged();
				}
			}
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
			if (oldValue != null) oldValue.Connected -= Model_Connected;
			if (oldValue != null) oldValue.Disconnected -= Model_Disconnected;

			if (newValue != null) newValue.Connected += Model_Connected;
			if (newValue != null) newValue.Disconnected += Model_Disconnected;

			EndpointString = (newValue != null) ? newValue.EndpointAddress.ToString() : string.Empty;
		}

		private void Model_Disconnected(System.DateTime obj)
		{
			IsConnected = false;
		}

		private void Model_Connected(System.DateTime obj)
		{
			IsConnected = true;
		}
	}
}
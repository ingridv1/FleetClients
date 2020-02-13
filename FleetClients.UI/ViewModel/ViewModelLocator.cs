namespace FleetClients.UI.ViewModel
{
	public static class ViewModelLocator
	{
		public static FleetTemplateManagerViewModel FleetTemplateManagerViewModel { get; } = new FleetTemplateManagerViewModel();

		public static FleetTemplateViewModel FleetTemplateViewModel { get; } = new FleetTemplateViewModel();

		public static FleetManagerClientViewModel FleetManagerClientViewModel { get; } = new FleetManagerClientViewModel();

		public static KingpinStatesViewModel KingpinStatesViewModel { get; } = new KingpinStatesViewModel();

		public static AGVTemplateFactoryViewModel AGVTemplateFactoryViewModel { get; } = new AGVTemplateFactoryViewModel();

		public static void UpdateFleetManagerClientViewModels(IFleetManagerClient client)
		{
			FleetManagerClientViewModel.Model = client;
			KingpinStatesViewModel.Model = client;
		}

		public static void UpdateFleetTemplateManagerViewModels(FleetTemplateManager manager)
		{
			FleetTemplateManagerViewModel.Model = manager;
			FleetTemplateViewModel.Model = manager?.FleetTemplate;
		}

	}
}

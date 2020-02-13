namespace FleetClients.UI.ViewModel
{
	public static class ViewModelLocator
	{
		public static FleetTemplateManagerViewModel FleetTemplateManagerViewModel { get; } = new FleetTemplateManagerViewModel();

		public static FleetTemplateViewModel FleetTemplateViewModel { get; } = new FleetTemplateViewModel();

		public static FleetManagerClientViewModel FleetManagerClientViewModel { get; } = new FleetManagerClientViewModel();

		public static AGVTemplateFactoryViewModel AGVTemplateFactoryViewModel { get; } = new AGVTemplateFactoryViewModel();

		public static void UpdateFleetTemplateManagerViewModels(FleetTemplateManager manager)
		{
			FleetTemplateManagerViewModel.Model = manager;
			FleetTemplateViewModel.Model = manager?.FleetTemplate;
		}

	}
}

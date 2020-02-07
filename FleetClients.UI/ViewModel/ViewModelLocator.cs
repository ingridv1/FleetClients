namespace FleetClients.UI.ViewModel
{
	public static class ViewModelLocator
	{
		public static FleetTemplateManagerViewModel FleetTemplateManagerViewModel { get; } = new FleetTemplateManagerViewModel();

		public static FleetTemplateViewModel FleetTemplateViewModel { get; } = new FleetTemplateViewModel();
	}
}

using System;

namespace FleetClients.Core
{
	public class FleetTemplateManager
	{
		public FleetTemplate FleetTemplate { get; set; } = new FleetTemplate();

		public IFleetManagerClient FleetManagerClient { get; } = null;

		public FleetTemplateManager(IFleetManagerClient client)
		{
			if (client == null) throw new ArgumentNullException("client");

			FleetManagerClient = client;
		}

		public void Populate() => FleetTemplate.Populate(FleetManagerClient);
	}
}
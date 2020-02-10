using System;

namespace FleetClients
{
	public class FleetTemplateManager
	{
		public FleetTemplate FleetTemplate { get; set; } = new FleetTemplate();

		private IFleetManagerClient fleetManagerClient = null;

		public FleetTemplateManager(IFleetManagerClient client)
		{
			if (client == null) throw new ArgumentNullException("client");

			fleetManagerClient = client;
		}

		public void Populate() => FleetTemplate.Populate(fleetManagerClient);
	}
}

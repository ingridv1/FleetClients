using System;

namespace FleetClients.Core
{
    public class FleetTemplateManager
    {
        public FleetTemplate FleetTemplate { get; set; } = new FleetTemplate();

        public IFleetManagerClient FleetManagerClient { get; } = null;

        public FleetTemplateManager(IFleetManagerClient client)
        {
            FleetManagerClient = client ?? throw new ArgumentNullException("client");
        }

        public void Populate() => FleetTemplate.Populate(FleetManagerClient);
    }
}
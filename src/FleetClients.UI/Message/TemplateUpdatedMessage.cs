using FleetClients.Core;

namespace FleetClients.UI.Message
{
	public class TemplateUpdatedMessage
	{
		public FleetTemplate FleetTemplate { get; } = null;

		public TemplateUpdatedMessage(FleetTemplate fleetTemplate)
		{
			FleetTemplate = fleetTemplate;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

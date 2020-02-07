using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients.UI.ViewModel
{
	public static class ViewModelLocator
	{
		public static FleetTemplateManagerViewModel FleetTemplateManagerViewModel { get; } = new FleetTemplateManagerViewModel();
	}
}

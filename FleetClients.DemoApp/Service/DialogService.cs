using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FleetClients.DemoApp.Service
{
	public static class DialogService
	{
		public static Window CreateFleetTemplateManagerTutorialWindow()
		{
			FleetTemplateManagerTutorialWindow window = new FleetTemplateManagerTutorialWindow();
			return window;
		}
	}
}

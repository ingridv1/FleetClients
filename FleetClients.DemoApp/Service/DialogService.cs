using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uiVM = FleetClients.UI.ViewModel;

namespace FleetClients.DemoApp.Service
{
	public static class DialogService
	{
		public static Window CreateFleetTemplateManagerTutorialWindow(FleetTemplateManager manager)
		{
			uiVM.ViewModelLocator.UpdateFleetTemplateManagerViewModels(manager);

			FleetTemplateManagerTutorialWindow window = new FleetTemplateManagerTutorialWindow();
			return window;
		}

		public static Window CreateFleetManagerTutorialWindow()
		{
			FleetManagerTutorialWindow window = new FleetManagerTutorialWindow();
			return window;
		}
	}
}

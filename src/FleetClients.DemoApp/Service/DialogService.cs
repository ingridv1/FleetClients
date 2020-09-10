using FleetClients.Core;
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

        public static Window CreateFleetClientTutorialWindow(IFleetManagerClient client)
        {
            uiVM.ViewModelLocator.UpdateFleetManagerClientViewModels(client);

            FleetManagerClientTutorialWindow window = new FleetManagerClientTutorialWindow();
            return window;
        }
    }
}
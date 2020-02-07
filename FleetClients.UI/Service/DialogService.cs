using GACore;
using GACore.NLog;
using NLog;
using System.Windows;

namespace FleetClients.UI.Service
{
	public static class DialogService
	{
		private static Logger logger { get; } = LoggerFactory.GetStandardLogger(StandardLogger.ViewModel);

		public static Window CreateFleetTemplateManagerWindow(FleetTemplateManager manager)
		{
			logger.Debug("[DialogService] CreateFleetTemplateManagerWindow()");

			ViewModel.ViewModelLocator.FleetTemplateManagerViewModel.Model = manager;

			FleetTemplateManagerWindow window = new FleetTemplateManagerWindow();
			return window;
		}
	}
}

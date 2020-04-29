using FleetClients.UI.ViewModel;
using GACore;
using GACore.NLog;
using NLog;
using FleetClients.Core;
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
			ViewModel.ViewModelLocator.FleetTemplateViewModel.Model = manager?.FleetTemplate;

			FleetTemplateManagerWindow window = new FleetTemplateManagerWindow();
			return window;
		}

		public static Window CreateAGVTemplateFactoryWindow(FleetTemplateManager manager)
		{
			logger.Debug("[DialogService] CreateAGVTemplateFactoryWindow()");

			AGVTemplateFactoryWindow window = new AGVTemplateFactoryWindow();

			ViewModel.ViewModelLocator.AGVTemplateFactoryViewModel.Model = manager;

			return window;
		}

		public static Window CreateKingpinDiagnosticWindow(KingpinStateMailboxViewModel ksmViewModel)
		{
			logger.Debug("[DialogService] CreateKingpinDiagnosticWindow()");

			KingpinDiagnosticWindow window = new KingpinDiagnosticWindow()
			{
				DataContext = ksmViewModel
			};

			return window;
		}
	}
}
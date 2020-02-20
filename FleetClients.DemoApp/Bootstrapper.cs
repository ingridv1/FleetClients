using FleetClients.DemoApp.Model;
using System.Net;

namespace FleetClients.DemoApp
{
	internal static class Bootstrapper
	{
		public static void Activate()
		{
			TutorialModel tutorialModel = new TutorialModel();
			ViewModel.ViewModelLocator.TutorialViewModel.Model = tutorialModel;

			GACore.UI.ViewModel.ViewModelLocator.IPAddressViewModel.IPAddress = IPAddress.Loopback;
		}
	}
}
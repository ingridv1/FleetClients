using FleetClients.DemoApp.Model;

namespace FleetClients.DemoApp
{
	internal static class Bootstrapper
	{
		public static void Activate()
		{
			TutorialModel tutorialModel = new TutorialModel();
			ViewModel.ViewModelLocator.TutorialViewModel.Model = tutorialModel;
		}
	}
}

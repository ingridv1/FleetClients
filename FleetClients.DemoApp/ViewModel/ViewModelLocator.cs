using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACore;
using FleetClients.DemoApp.Model;

namespace FleetClients.DemoApp.ViewModel
{
	public static class ViewModelLocator
	{
		public static TutorialViewModel TutorialViewModel { get; } = new TutorialViewModel();
	}
}

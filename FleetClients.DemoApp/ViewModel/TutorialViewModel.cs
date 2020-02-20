using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using FleetClients.DemoApp.Model;
using System.Net;
using GACore;
using GACore.Command;
using Markdig;

namespace FleetClients.DemoApp.ViewModel
{
	public class TutorialViewModel : AbstractViewModel<TutorialModel>
	{
		internal TutorialViewModel()
		{
			HandleLoadCommands();
		}

		protected override void HandleModelUpdate(TutorialModel oldValue, TutorialModel newValue)
		{
			base.HandleModelUpdate(oldValue, newValue);
		}

		public ICommand TutorialCommand { get; set; }

		public ICommand RequestNavigateCommand { get; set; }

		private void HandleLoadCommands()
		{
			RequestNavigateCommand = new CustomCommand(RequestNavigate, CanRequestNavigate);
			TutorialCommand = new CustomCommand(TutorialClick, CanTutorialClick);
		}

		private bool CanTutorialClick(object obj) => true;

		private void HandleShowTemplateManager()
		{
			IPAddress ipAddress = GACore.UI.ViewModel.ViewModelLocator.IPAddressViewModel.IPAddress;
			FleetTemplateManager manager = Model.CreateFleetTemplateManager(ipAddress);

			Service.DialogService.CreateFleetTemplateManagerTutorialWindow(manager)
				.ShowDialog();
		}

		private void HandleShowFleetManager()
		{
			IPAddress ipAddress = GACore.UI.ViewModel.ViewModelLocator.IPAddressViewModel.IPAddress;
			IFleetManagerClient client = Model.CreateFleetManagerClient(ipAddress);

			Service.DialogService.CreateFleetClientTutorialWindow(client)
				.ShowDialog();
		}

		private void HandleOption(TutorialCommandOption option)
		{
			switch(option)
			{
				case TutorialCommandOption.ShowFleetManager:
					{
						HandleShowFleetManager();
						break;
					}

				case TutorialCommandOption.ShowTemplateManager:
					{
						HandleShowTemplateManager();
						break;
					}

				default:
					throw new NotImplementedException();
			}
		}

		private void TutorialClick(object obj)
		{
			try
			{
				TutorialCommandOption option = (TutorialCommandOption)obj;
				HandleOption(option);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		private bool CanRequestNavigate(object obj) => true;

		private void RequestNavigate(object obj)
		{
			try
			{
				string html = Markdown.ToHtml(Properties.Resources.MainWindowDescription);

				WebBrowser webBrowser = (WebBrowser)obj;
				webBrowser.NavigateToString(html);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}
	}
}

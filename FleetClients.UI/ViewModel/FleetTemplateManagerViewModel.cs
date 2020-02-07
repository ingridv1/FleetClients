using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACore;
using GACore.Architecture;
using FleetClients;
using System.Windows.Input;
using GACore.Command;
using Microsoft.Win32;
using FleetClients.UI.Message;
using GACore.Utility;

namespace FleetClients.UI.ViewModel
{
	public class FleetTemplateManagerViewModel : AbstractViewModel<FleetTemplateManager>
	{
		internal FleetTemplateManagerViewModel()
		{
			HandleLoadCommands();
		}

		public ICommand FTMOptionCommand { get; set; }

		private void HandleLoadCommands()
		{
			FTMOptionCommand = new CustomCommand(FleetTemplateOptionClick, CanFleetTemplateOptionClick);
		}

		private void HandleLoad()
		{
			OpenFileDialog dialog = DialogFactory.GetOpenJsonDialog();

			if (dialog.ShowDialog() == true)
			{
				FleetTemplate parsedTemplate = JsonFactory.FleetTemplateFromFile(dialog.FileName);

				if (parsedTemplate != null)
				{
					if (Model != null) Model.FleetTemplate = parsedTemplate;

					TemplateUpdatedMessage message = new TemplateUpdatedMessage(parsedTemplate);
					Messenger.Default.Send(message);
				}
			}
		}

		private void HandleFleetTemplateManagerOption(FleetTemplateManagerOption option)
		{
			switch(option)
			{
				case FleetTemplateManagerOption.Load:
					{
						HandleLoad();
						return;
					}

				default:
					throw new NotImplementedException();
			}
		}

		private bool CanFleetTemplateOptionClick(object obj) => true;

		private void FleetTemplateOptionClick(object obj)
		{
			try
			{
				FleetTemplateManagerOption option = (FleetTemplateManagerOption)obj;
				HandleFleetTemplateManagerOption(option);
			}
			catch (Exception ex)
			{
			}
		}
	}
}

using FleetClients.UI.Message;
using GACore;
using GACore.Command;
using GACore.Utility;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace FleetClients.UI.ViewModel
{
	public class FleetTemplateManagerViewModel : AbstractViewModel<FleetTemplateManager>
	{
		internal FleetTemplateManagerViewModel()
		{
			HandleLoadCommands();
		}

		private string endpointString = string.Empty;

		public string EndpointString
		{
			get { return endpointString; }
			private set
			{
				if (endpointString != value)
				{
					endpointString = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		protected override void HandleModelUpdate(FleetTemplateManager oldValue, FleetTemplateManager newValue)
		{
			EndpointString = (newValue != null) ? newValue.FleetManagerClient.EndpointAddress.ToString() : string.Empty;
		}

		public ICommand FTMOptionCommand { get; set; }

		private void HandleLoadCommands()
		{
			FTMOptionCommand = new CustomCommand(FleetTemplateOptionClick, CanFleetTemplateOptionClick);
		}

		private void HandleSave()
		{
			try
			{
				SaveFileDialog dialog = DialogFactory.GetSaveJsonDialog();
				if (dialog.ShowDialog() == true) Model.FleetTemplate.ToFile(dialog.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Failed to save fleet template", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void HandleClear()
		{
			if (Model != null) Model.FleetTemplate.Clear();
		}

		private void HandleAdd()
		{
			Window window = Service.DialogService.CreateAGVTemplateFactoryWindow(Model);
			window.ShowDialog();
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
			switch (option)
			{
				case FleetTemplateManagerOption.Add:
					{
						HandleAdd();
						return;
					}

				case FleetTemplateManagerOption.Clear:
					{
						HandleClear();
						return;
					}

				case FleetTemplateManagerOption.Load:
					{
						HandleLoad();
						return;
					}

				case FleetTemplateManagerOption.Save:
					{
						HandleSave();
						return;
					}

				case FleetTemplateManagerOption.Populate:
					{
						Model.Populate();
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
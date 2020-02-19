using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GACore;
using GACore.Architecture;
using GACore.Command;
using NLog;

namespace FleetClients.UI.ViewModel
{
	public class KingpinStatesViewModel : AbstractCollectionViewModel<IFleetManagerClient, KingpinStateMailboxViewModel, KingpinStateMailbox>
	{

		public KingpinStatesViewModel()			
		{
			HandleLoadCommands();
		}

		public ICommand MouseDoubleClickCommand { get; set; }

		public ICommand SelectionChangedCommand { get; set; }

		private void HandleLoadCommands()
		{
			MouseDoubleClickCommand = new CustomCommand(MouseDoubleClick, CanMouseDoubleClick);
			SelectionChangedCommand = new CustomCommand(SelectionChanged, CanSelectionChanged);
		}

		private bool CanSelectionChanged(object obj) => true;

		private void SelectionChanged(object obj)
		{
			try
			{
				KingpinStateMailboxViewModel ksmViewModel = (KingpinStateMailboxViewModel)obj;
				ViewModelLocator.SelectedKingpinViewModel.Model = ksmViewModel.Model;
			}
			catch(Exception ex)
			{
				ViewModelLocator.SelectedKingpinViewModel.Model = null;
				Logger.Error(ex);
			}
		}

		private void MouseDoubleClick(object obj)
		{
			try
			{
				KingpinStateMailboxViewModel ksmViewModel = (KingpinStateMailboxViewModel)obj;

				Service.DialogService.CreateKingpinDiagnosticWindow(ksmViewModel)
					.Show();
			}
			catch(Exception ex)
			{
				Logger.Error(ex);
			}
		}

		private bool CanMouseDoubleClick(object obj) => true;

		public override KingpinStateMailboxViewModel GetViewModelForModel(KingpinStateMailbox model)
		{
			try
			{
				return ViewModels.FirstOrDefault(e => e.KingpinState.IPAddress.Equals(model.Key));			
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return null;
			}
		}
	}
}

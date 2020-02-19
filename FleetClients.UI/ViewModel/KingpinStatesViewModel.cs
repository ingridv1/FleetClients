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

		public ICommand SelectedKingpinCommand { get; set; }

		private void HandleLoadCommands()
		{
			SelectedKingpinCommand = new CustomCommand(SelectedKingpin, CanSelectedKingpin);
		}

		private void SelectedKingpin(object obj)
		{
			try
			{
				KingpinStateMailboxViewModel ksmViewModel = (KingpinStateMailboxViewModel)obj;
			}
			catch(Exception ex)
			{
				Logger.Error(ex);
			}
		}

		private bool CanSelectedKingpin(object obj) => true;

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

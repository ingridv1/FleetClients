using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACore;
using GACore.Architecture;

namespace FleetClients.UI.ViewModel
{
	public class KingpinStatesViewModel : AbstractCollectionViewModel<IFleetManagerClient, KingpinStateMailboxViewModel, KingpinStateMailbox>
	{
		public override KingpinStateMailboxViewModel GetViewModelForModel(KingpinStateMailbox model)
		{
			throw new NotImplementedException();
		}
	}
}

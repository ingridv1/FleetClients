using GACore;
using GACore.Architecture;

namespace FleetClients.UI.ViewModel
{
	public class KingpinStateMailboxViewModel : AbstractViewModel<KingpinStateMailbox>
	{
		private IKingpinState kingpinState = null;

		public IKingpinState KingpinState
		{
			get { return kingpinState; }
			private set
			{
				if (kingpinState != value)
				{
					kingpinState = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		protected override void HandleModelUpdate(KingpinStateMailbox oldValue, KingpinStateMailbox newValue)
		{
			if (oldValue != null) oldValue.Updated -= Model_Updated;
			if (newValue != null) newValue.Updated += Model_Updated;

			KingpinState = (newValue != null) ? newValue.Mail : null;

			base.HandleModelUpdate(oldValue, newValue);
		}

		private void Model_Updated()
		{
			KingpinState = Model?.Mail;
		}
	}
}
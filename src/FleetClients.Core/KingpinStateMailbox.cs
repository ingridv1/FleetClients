using GACore;
using GACore.Architecture;
using System.Net;

namespace FleetClients.Core
{
	public class KingpinStateMailbox : GenericMailbox<IPAddress, IKingpinState>
	{
		public KingpinStateMailbox(IPAddress ipAddress, IKingpinState kingpingState)
			: base(ipAddress, kingpingState)
		{
		}
	}
}
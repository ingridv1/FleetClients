using GACore;
using GACore.Architecture;
using System.Net;

namespace FleetClients.Core
{
	/// <summary>
	/// Wraps GenericMailbox and provides a container tightly coupling an IP Address and an IKingpinState
	/// </summary>
	public class KingpinStateMailbox : GenericMailbox<IPAddress, IKingpinState>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ipAddress">Kingpin hardware IP address</param>
		/// <param name="kingpinState">Current IKingpinState</param>
		public KingpinStateMailbox(IPAddress ipAddress, IKingpinState kingpinState)
			: base(ipAddress, kingpinState)
		{
		}
	}
}
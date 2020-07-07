using BaseClients;
using BaseClients.Core;
using System;
using System.Net;

namespace FleetClients.Core
{
	/// <summary>
	/// Factory class for creating IFleetManagerClient instances
	/// </summary>
	public static class ClientFactory
	{
		/// <summary>
		/// Creates a new Tcp based IFleetManagerClient instance
		/// </summary>
		/// <param name="endpointSettings">Endpoint settings specifying port and ip address to use</param>
		/// <returns>Tcp based IFleetManagerClient instance</returns>
		public static IFleetManagerClient CreateTcpFleetManagerClient(EndpointSettings endpointSettings)
		{
			return new FleetManagerClient(endpointSettings.TcpFleetManagerService());
		}

		/// <summary>
		/// Create a new Tcp based IFleetManagerClient instance
		/// </summary>
		/// <param name="ipAddress">Fleet Manager IP address</param>
		/// <param name="tcpPort">TCP port to use (default 41917)</param>
		/// <returns>Tcp based IFleetManagerClient instance</returns>
		public static IFleetManagerClient CreateTcpFleetManagerClient(IPAddress ipAddress, ushort tcpPort = 41917)
		{
			if (ipAddress == null) throw new ArgumentNullException("ipAddress");

			EndpointSettings endpointSettings = new EndpointSettings(ipAddress, 41916, tcpPort);
			return CreateTcpFleetManagerClient(endpointSettings);
		}
	}
}
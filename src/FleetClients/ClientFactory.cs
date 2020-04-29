using BaseClients;
using System.Net;

namespace FleetClients
{
	public static class ClientFactory
	{
		public static IFleetManagerClient CreateTcpFleetManagerClient(EndpointSettings endpointSettings)
		{
			return new FleetManagerClient(endpointSettings.TcpFleetManagerService());
		}

		public static IFleetManagerClient CreateTcpFleetManagerClient(IPAddress ipAddress, ushort tcpPort = 41917)
		{
			EndpointSettings endpointSettings = new EndpointSettings(ipAddress, 41916, tcpPort);
			return CreateTcpFleetManagerClient(endpointSettings);
		}
	}
}
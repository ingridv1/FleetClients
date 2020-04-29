using BaseClients;
using System;

namespace FleetClients.Core
{
	public static class EndpointSettings_ExtensionMethods
	{
		public static Uri TcpFleetManagerService(this EndpointSettings portSettings)
		{
			return new Uri(portSettings.ToTcpBase(), "fleetManager.svc");
		}
	}
}
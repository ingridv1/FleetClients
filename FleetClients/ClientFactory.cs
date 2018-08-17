using BaseClients;

namespace FleetClients
{
    public static class ClientFactory
    {
        public static IFleetManagerClient CreateTcpFleetManagerClient(EndpointSettings portSettings)
        {
            return new FleetManagerClient(portSettings.TcpFleetManagerService());
        }
    }
}

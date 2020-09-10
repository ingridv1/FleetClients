using FleetClients.Core;
using System.Net;

namespace FleetClients.DemoApp.Model
{
    public class TutorialModel
    {
        public TutorialModel()
        {
        }

        public IFleetManagerClient CreateFleetManagerClient(IPAddress ipAddress)
        {
            return ClientFactory.CreateTcpFleetManagerClient(ipAddress);
        }

        public FleetTemplateManager CreateFleetTemplateManager(IPAddress ipAddress)
        {
            IFleetManagerClient client = CreateFleetManagerClient(ipAddress);
            return new FleetTemplateManager(client);
        }
    }
}
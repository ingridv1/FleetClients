using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using System.Net;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("remove", HelpText = "Remove an AGV from the fleet")]
    public class RemoveOptions : AbstractConsoleOption<IFleetManagerClient>
    {
        [Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
        public string IPv4String { get; set; }

        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IPAddress ipAddress = IPAddress.Parse(IPv4String);

            return client.RemoveVehicle(ipAddress);
        }
    }
}
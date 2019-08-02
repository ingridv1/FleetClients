using CommandLine;
using System;
using BaseClients;
using System.Net;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("Remove", HelpText = "Remove an AGV from the fleet")]
	public class RemoveOptions
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		public int Remove(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);

			client.TryRemoveVehicle(ipAddress, out bool success);

			return 0;
		}
	}
}

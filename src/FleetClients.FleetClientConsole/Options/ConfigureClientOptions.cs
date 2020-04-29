using CommandLine;
using System.Net;
using FleetClients.Core;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("configure", HelpText = "Configure Fleet Manager Client")]
	public class ConfigureClientOptions
	{
		[Option('i', "IPv4String", Required = false, Default = "127.0.0.1", HelpText = "Scheduler IPv4 Address")]
		public string IPv4String { get; set; }

		[Option('t', "TcpPort", Required = false, Default = (ushort)41917, HelpText = "TCP port of scheduler")]
		public ushort TcpPort { get; set; }

		public IFleetManagerClient CreateTcpFleetManagerClient()
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);
			return ClientFactory.CreateTcpFleetManagerClient(ipAddress, TcpPort);
		}
	}
}
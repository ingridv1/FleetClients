using BaseClients;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("reset", HelpText = "Reset a kingpin module")]
	public class ResetKingpinOption : AbstractOption<IFleetManagerClient>
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);

			ServiceOperationResult result = client.TryResetKingpin(ipAddress, out bool output);

			Console.WriteLine("ResetKingpin:{0}", output ? "Success" : "Failed");
			return result;
		}
	}
}

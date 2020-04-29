using BaseClients;
using CommandLine;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using System;
using System.Net;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("setkingpinstate", HelpText = "Sets a kingpins state")]
	public class SetKingpinStateOption : AbstractConsoleOption<IFleetManagerClient>
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		[Option('v', "VehicleControllerState", Required = true, Default = "Enabled", HelpText = "Enabled, Disabled")]
		public string ControllerState { get; set; }

		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);

			VehicleControllerState controllerstate = (VehicleControllerState)Enum.Parse(typeof(VehicleControllerState), ControllerState, true);

			ServiceOperationResult result = client.TrySetKingpinState(ipAddress, controllerstate, out bool success);

			Console.WriteLine("SetFleetState:{0}", success ? "Success" : "Failed");
			return result;
		}
	}
}
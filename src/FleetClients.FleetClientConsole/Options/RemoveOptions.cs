﻿using BaseClients;
using CommandLine;
using System.Net;
using FleetClients.Core;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("remove", HelpText = "Remove an AGV from the fleet")]
	public class RemoveOptions : AbstractConsoleOption<IFleetManagerClient>
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);

			return client.TryRemoveVehicle(ipAddress, out bool success);
		}
	}
}
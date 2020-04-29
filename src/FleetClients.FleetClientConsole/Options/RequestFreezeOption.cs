using BaseClients;
using CommandLine;
using FleetClients.Core;
using System;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("freeze", HelpText = "Freezes fleet movement")]
	public class RequestFreezeOption : AbstractConsoleOption<IFleetManagerClient>
	{
		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			ServiceOperationResult result = client.TryRequestFreeze(out bool output);

			Console.WriteLine("RequestFreeze:{0}", output ? "Success" : "Failed");
			return result;
		}
	}
}
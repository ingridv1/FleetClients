using BaseClients;
using CommandLine;
using System;
using FleetClients.Core;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("unfreeze", HelpText = "Unfreezes fleet movement")]
	public class RequestUnfreezeOption : AbstractConsoleOption<IFleetManagerClient>
	{
		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			ServiceOperationResult result = client.TryRequestUnfreeze(out bool output);

			Console.WriteLine("RequestUnfreeze:{0}", output ? "Success" : "Failed");
			return result;
		}
	}
}
using BaseClients;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using BaseClients;
using CommandLine;
using GACore;
using System;
using FleetClients.Core;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("semver", HelpText = "Get semantic version of service")]
	public class GetSemVerOptions : AbstractConsoleOption<IFleetManagerClient>
	{
		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			ServiceOperationResult result = client.TryGetSemVer(out SemVerData semVerData);

			if (result.IsSuccessfull)
			{
				Console.WriteLine("v{0}.{1}.{2}", semVerData.Major, semVerData.Minor, semVerData.Patch);
			}

			return result;
		}
	}
}
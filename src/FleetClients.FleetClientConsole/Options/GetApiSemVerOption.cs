using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using GAAPICommon.Core.Dtos;
using System;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("apisemver", HelpText = "Get semantic version of service api")]
    public class GetSemVerOptions : AbstractConsoleOption<IFleetManagerClient>
    {
        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IServiceCallResult<SemVerDto> result = client.GetAPISemVer();

            if (result.ServiceCode == 0)
                Console.WriteLine("v{0}.{1}.{2}", result.Value.Major, result.Value.Minor, result.Value.Patch);

            return result;
        }
    }
}
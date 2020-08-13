using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using System;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("unfreeze", HelpText = "Unfreezes fleet movement")]
    public class RequestUnfreezeOption : AbstractConsoleOption<IFleetManagerClient>
    {
        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IServiceCallResult result = client.RequestUnfreeze();

            Console.WriteLine("RequestUnfreeze:{0}", result.ServiceCode == 0 ? "Success" : "Failed");
            return result;
        }
    }
}
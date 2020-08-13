using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using System;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("freeze", HelpText = "Freezes fleet movement")]
    public class RequestFreezeOption : AbstractConsoleOption<IFleetManagerClient>
    {
        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IServiceCallResult result = client.RequestFreeze();

            Console.WriteLine("RequestFreeze:{0}", result.ServiceCode == 0 ? "Success" : "Failed");
            return result;
        }
    }
}
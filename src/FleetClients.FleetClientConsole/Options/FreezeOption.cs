using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using GACore.Architecture;
using System;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("freeze", HelpText = "Freezes fleet movement")]
    public class FreezeOption : AbstractConsoleOption<IFleetManagerClient>
    {
        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IServiceCallResult result = client.SetFrozenState(FrozenState.Frozen);

            Console.WriteLine("RequestFreeze:{0}", result.ServiceCode == 0 ? "Success" : "Failed");
            return result;
        }
    }
}
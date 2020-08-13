using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using System;
using System.Net;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("reset", HelpText = "Reset a kingpin module")]
    public class ResetKingpinOption : AbstractConsoleOption<IFleetManagerClient>
    {
        [Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
        public string IPv4String { get; set; }

        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IPAddress ipAddress = IPAddress.Parse(IPv4String);

            IServiceCallResult result = client.ResetKingpin(ipAddress);

            Console.WriteLine("ResetKingpin:{0}", result.ServiceCode == 0 ? "Success" : "Failed");
            return result;
        }
    }
}
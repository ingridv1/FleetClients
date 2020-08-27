using CommandLine;
using FleetClients.Core;
using FleetClients.FleetClientConsole.Options;
using GAAPICommon.Core;
using System;

namespace FleetClients.FleetClientConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = @"Fleet Client Console";

            IFleetManagerClient client = null;

            Parser.Default.ParseArguments<ConfigureClientOptions>(args)
                .WithParsed<ConfigureClientOptions>(o =>
                {
                    client = o.CreateTcpFleetManagerClient();
                }
                )
                .WithNotParsed<ConfigureClientOptions>(o =>
                {
                    Environment.Exit(-1);
                });

            while (true)
            {
                Console.Write("fc>");
                args = Console.ReadLine().Split();

                Parser.Default.ParseArguments
                    <CreateVirtualVehicleOptions,
                     GetKingpinDescriptionOptions,
                     RemoveOptions,
                     FreezeOption,
                     UnfreezeOption,
                     SetFleetStateOption,
                     SetKingpinStateOption,
                     SetPoseOptions,
                     GetSemVerOptions
                     >
                     (args)
                    .MapResult(
                        (CreateVirtualVehicleOptions opts) => opts.ExecuteOption(client),
                        (GetKingpinDescriptionOptions opts) => opts.ExecuteOption(client),
                        (RemoveOptions opts) => opts.ExecuteOption(client),
                        (FreezeOption opts) => opts.ExecuteOption(client),
                        (UnfreezeOption opts) => opts.ExecuteOption(client),
                        (SetFleetStateOption opts) => opts.ExecuteOption(client),
                        (SetKingpinStateOption opts) => opts.ExecuteOption(client),
                        (SetPoseOptions opts) => opts.ExecuteOption(client),
                        (GetSemVerOptions opts) => opts.ExecuteOption(client),
                        errs => ServiceCallResultFactory.FromClientException(new Exception("Operation failed"))
                        );
            }
        }
    }
}
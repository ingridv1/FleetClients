using BaseClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using FleetClients.FleetClientConsole.Options;
using FleetClients;

namespace FleetClients.FleetClientConsole
{
	class Program
	{
		static void Main(string[] args)
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
				Parser.Default.ParseArguments<CreateVirtualVehicleOptions, GetKingpinDescriptionOptions, RemoveOptions, ResetKingpinOption, SetPoseOptions>(Console.ReadLine().Split())
					.MapResult(
						(CreateVirtualVehicleOptions opts) => opts.ExecuteOption(client),
						(GetKingpinDescriptionOptions opts) => opts.ExecuteOption(client),
						(RemoveOptions opts) => opts.ExecuteOption(client),
						(ResetKingpinOption opts) => opts.ExecuteOption(client),
						(SetPoseOptions opts) => opts.ExecuteOption(client),
						errs => ServiceOperationResult.FromClientException(new Exception("Operation failed"))
						);
			}		
		}
	}
}

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
				Parser.Default.ParseArguments<CreateVirtualVehicleOptions, RemoveOptions, SetPoseOptions>(Console.ReadLine().Split())
					.MapResult(		
						(CreateVirtualVehicleOptions opts) => opts.CreateVirtualVehicle(client),
						(RemoveOptions opts) => opts.Remove(client),
						(SetPoseOptions opts) => opts.SetPose(client),
						errs => ServiceOperationResult.FromClientException(new Exception("foo")));
			}		
		}
	}
}

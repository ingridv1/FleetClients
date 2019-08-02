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

		private static int Moo(ConfigureClientOptions opts)
		{
			return -1;
		}

		[Verb("Set", HelpText ="Set pose of an AGV in the fleet")]
		public class SetPoseOptions
		{

		}



		private static int RunSetPoseAndReturnExitCode(SetPoseOptions opts)
		{
			return 0;
		}

		private static int RunRemoveAndReturnExitCode(RemoveOptions opts)
		{
			return 0;
		}

		static void Main(string[] args)
		{
			Console.Title = @"Fleet Client Console";

			IFleetManagerClient client = null;		

			while (true)
			{
				Console.WriteLine("reroll");

				string foo = Console.ReadLine();



				Parser.Default.ParseArguments<ConfigureClientOptions, RemoveOptions, SetPoseOptions>(foo.Split())
					.MapResult(
						(ConfigureClientOptions opts) =>
						{
							client = opts.CreateTcpFleetManagerClient();
							return 0;
						},
						(RemoveOptions opts) => opts.Remove(client),
						(SetPoseOptions opts) => RunSetPoseAndReturnExitCode(opts),
						errs => 1); ;


			}
			/*

			IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(new EndpointSettings(IPAddress.Loopback));

			Console.WriteLine("Foo");

			bool continueFlag = true;

			IPAddress ipAddress = IPAddress.Parse("192.168.4.69");

			while (continueFlag)
			{
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.C:
						{
							client.TryCreateVirtualVehicle(ipAddress, PoseDataFactory.ZeroPose, out bool success);
							break;
						}

					case ConsoleKey.R:
						{
							client.TryRemoveVehicle(ipAddress, out bool success);
							break;
						}

					case ConsoleKey.S:
						{
							break;
						}

					case ConsoleKey.Q:
						{
							continueFlag = false;
							break;
						}

					default:
						break;
				}
			}



			*/
			Console.WriteLine("Press <any> key to quit");
			Console.ReadKey(true);

		
		}
	}
}

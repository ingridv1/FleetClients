using BaseClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients.FleetClientConsole
{
	class Program
	{
		private static void CreateVirtualVehicle(IFleetManagerClient client)

		static void Main(string[] args)
		{
			Console.Title = @"Fleet Client Console";

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




			Console.WriteLine("Press <any> key to quit");
			Console.ReadKey(true);

			client.Dispose();
		}
	}
}

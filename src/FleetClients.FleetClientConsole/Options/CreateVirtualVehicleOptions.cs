using BaseClients;
using CommandLine;
using FleetClients.Core.FleetManagerServiceReference;
using System.Net;
using FleetClients.Core;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("create", HelpText = "Create a virtual vehicle")]
	public class CreateVirtualVehicleOptions : AbstractConsoleOption<IFleetManagerClient>
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		[Option('p', "PoseString", Required = false, Default = "", HelpText = "Pose")]
		public string PoseString { get; set; }

		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);
			PoseDataFactory.TryParseString(PoseString, out PoseData poseData);

			return client.TryCreateVirtualVehicle(ipAddress, poseData ?? PoseDataFactory.NaNPose, out bool success);
		}
	}
}
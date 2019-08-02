using BaseClients;
using CommandLine;
using FleetClients.FleetManagerServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("setpose", HelpText = "Set AGV pose")]
	public class SetPoseOptions
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		[Option('p', "PoseString", Required = false, Default = "", HelpText = "Pose")]
		public string PoseString { get; set; }

		public ServiceOperationResult SetPose(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);
			PoseDataFactory.TryParseString(PoseString, out PoseData poseData);

			return client.TrySetPose(ipAddress, poseData ?? PoseDataFactory.NaNPose, out bool success);
		}
	}
}

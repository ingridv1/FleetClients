using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Architecture;
using GAAPICommon.Core.Dtos;
using System.Net;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("setpose", HelpText = "Set AGV pose")]
    public class SetPoseOptions : AbstractConsoleOption<IFleetManagerClient>
    {
        [Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
        public string IPv4String { get; set; }

        [Option('p', "PoseString", Required = false, Default = "", HelpText = "Pose")]
        public string PoseString { get; set; }

        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IPAddress ipAddress = IPAddress.Parse(IPv4String);
            PoseDtoFactory.TryParseString(PoseString, out PoseDto poseData);

            return client.SetPose(ipAddress, poseData ?? PoseDtoFactory.NaNPose);
        }
    }
}
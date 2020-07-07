using BaseClients;
using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Architecture;
using System;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("setfleetstate", HelpText = "Sets fleet state")]
	public class SetFleetStateOption : AbstractConsoleOption<IFleetManagerClient>
	{
		[Option('v', "VehicleControllerState", Required = true, Default = "Enabled", HelpText = "Enabled, Disabled")]
		public string ControllerState { get; set; }

		protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
		{
			VehicleControllerState controllerstate = (VehicleControllerState)Enum.Parse(typeof(VehicleControllerState), ControllerState, true);

			IServiceCallResult result = client.SetFleetState(controllerstate);

			Console.WriteLine("SetFleetState:{0}", result.ServiceCode == 0 ? "Success" : "Failed");
			return result;
		}
	}
}
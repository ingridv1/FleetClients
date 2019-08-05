using BaseClients;
using CommandLine;
using FleetClients.FleetManagerServiceReference;
using System;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("setfleetstate", HelpText = "Sets fleet state")]
	public class SetFleetStateOption : AbstractConsoleOption<IFleetManagerClient>
	{
		[Option('v', "VehicleControllerState", Required = true, Default = "Enabled", HelpText = "Enabled, Disabled")]
		public string ControllerState { get; set; }

		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			VehicleControllerState controllerstate = (VehicleControllerState) Enum.Parse(typeof(VehicleControllerState), ControllerState, true);

			ServiceOperationResult result = client.TrySetFleetState(controllerstate, out bool success);

			Console.WriteLine("SetFleetState:{0}", success ? "Success" : "Failed");
			return result;
		}
	}
}

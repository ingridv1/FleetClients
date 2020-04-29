using BaseClients;
using FleetClients.Core.FleetManagerServiceReference;
using GACore;
using GACore.Architecture;
using System.Net;
using System.Xml.Linq;

namespace FleetClients.Core
{
	/// <summary>
	/// For interacting with the fleet manager remotely.
	/// </summary>
	public interface IFleetManagerClient : ICallbackClient, IModelCollection<KingpinStateMailbox>
	{
		FleetState FleetState { get; }

		ServiceOperationResult TryCommitEx2Waypoints(IPAddress ipAddress, int instructionId, byte[] ex2Waypoints, out bool success);

		ServiceOperationResult TryCreateVirtualVehicle(IPAddress ipAddress, PoseData pose, out bool success);

		ServiceOperationResult TryGetKingpinDescription(IPAddress ipAddress, out XDocument xDocument);

		ServiceOperationResult TryRemoveVehicle(IPAddress ipAddress, out bool success);

		ServiceOperationResult TryRequestFreeze(out bool success);

		ServiceOperationResult TryRequestUnfreeze(out bool success);

		ServiceOperationResult TryResetKingpin(IPAddress ipAddress, out bool success);

		ServiceOperationResult TrySetFleetState(VehicleControllerState controllerState, out bool success);

		ServiceOperationResult TrySetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState, out bool success);

		ServiceOperationResult TrySetPose(IPAddress ipAddress, PoseData pose, out bool success);

		ServiceOperationResult TryGetSemVer(out SemVerData semVerData);
	}
}
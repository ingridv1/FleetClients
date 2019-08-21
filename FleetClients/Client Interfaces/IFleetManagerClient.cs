using BaseClients;
using FleetClients.FleetManagerServiceReference;
using GACore;
using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Linq;

namespace FleetClients
{
	public interface IFleetManagerClient : ICallbackClient
	{
		FleetState FleetState { get; }

        ReadOnlyObservableCollection<KingpinStateMailbox> KingpinStateMailboxes { get; }

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
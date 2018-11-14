using BaseClients;
using FleetClients.FleetManagerServiceReference;
using System.Net;
using System.Xml.Linq;

namespace FleetClients
{
    public interface IFleetManagerClient : ICallbackClient
    { 
        ServiceOperationResult TryRequestFreeze(out bool success);

    	ServiceOperationResult TryGetKingpinDescription(IPAddress ipAddress, out XDocument xDocument);

    	ServiceOperationResult TryCommitExtendedWaypoints(IPAddress ipAddress, int instructionId, BaseMovementType baseMovementType, byte[] extendedWaypoints, out bool success);

    	ServiceOperationResult TryRequestUnfreeze(out bool success);

    	ServiceOperationResult TryCreateVirtualVehicle(IPAddress ipAddress, PoseData pose, out bool success);

    	ServiceOperationResult TryRemoveVehicle(IPAddress ipAddress, out bool success);

    	ServiceOperationResult TrySetPose(IPAddress ipAddress, PoseData pose, out bool success);

        FleetState FleetState { get; }

    	ServiceOperationResult TryResetKingpin(IPAddress ipAddress, out bool success);

		ServiceOperationResult TrySetFleetState(VehicleControllerState controllerState, out bool success);

		ServiceOperationResult TrySetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState, out bool success);
    }
}

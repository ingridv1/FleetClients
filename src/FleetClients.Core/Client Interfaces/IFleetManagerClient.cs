using BaseClients.Architecture;
using FleetClients.Core.FleetManagerServiceReference;

using GAAPICommon.Architecture;
using GAAPICommon.Core.Dtos;
using GACore.Architecture;
using System;
using System.Net;
using System.Xml.Linq;

namespace FleetClients.Core
{
    /// <summary>
    /// For interacting with the fleet manager remotely.
    /// </summary>
    public interface IFleetManagerClient : ICallbackClient, IModelCollection<KingpinStateMailbox>
    {
        FleetStateDto FleetState { get; }

        event Action<FleetStateDto> FleetStateUpdated;

        IServiceCallResult CreateVirtualVehicle(IPAddress ipAddress, PoseDto pose);

        IServiceCallResult<XElement> GetKingpinDescription(IPAddress ipAddress);

        IServiceCallResult RemoveVehicle(IPAddress ipAddress);

        IServiceCallResult SetFrozenState(FrozenState frozenState);

        IServiceCallResult SetFleetState(VehicleControllerState controllerStates);

        IServiceCallResult SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState);

        IServiceCallResult SetPose(IPAddress ipAddress, PoseDto pose);

        IServiceCallResult<SemVerDto> GetAPISemVer();
    }
}
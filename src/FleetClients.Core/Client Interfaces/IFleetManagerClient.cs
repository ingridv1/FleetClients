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
    /// For interacting with the fleet manager service remotely.
    /// </summary>
    public interface IFleetManagerClient : ICallbackClient, IModelCollection<KingpinStateMailbox>
    {
        /// <summary>
        /// The current state of the fleet.
        /// </summary>
        FleetStateDto FleetState { get; }

        /// <summary>
        /// Fired whenever the FleetState property is updated.
        /// </summary>
        event Action<FleetStateDto> FleetStateUpdated;

        /// <summary>
        /// Creates a new virtual vehicle.
        /// </summary>
        /// <param name="ipAddress">IPv4 address of the vehicle to be created.</param>
        /// <param name="pose">The initialization pose.</param>
        /// <returns>Successful service call result on creation.</returns>
        IServiceCallResult CreateVirtualVehicle(IPAddress ipAddress, PoseDto pose);

        /// <summary>
        /// Returns the xml description of the target Kingpin.
        /// </summary>
        /// <param name="ipAddress">IPv4 address of the target Kingpin</param>
        /// <returns>XElement defining the Kingpins operation.</returns>
        IServiceCallResult<XElement> GetKingpinDescription(IPAddress ipAddress);

        /// <summary>
        /// Removes a vehicle from the fleet manager
        /// </summary>
        /// <param name="ipAddress">IPv4 address of the target.</param>
        /// <returns>Successful service call result on removal.</returns>
        IServiceCallResult RemoveVehicle(IPAddress ipAddress);

        /// <summary>
        /// Sets the frozen state of the fleet manager, allowing global vehicle movement to be enabled / disabled and then returned to its previous state.
        /// </summary>
        /// <param name="frozenState">Desired frozen state.</param>
        /// <returns>Successful service call result on success.</returns>
        IServiceCallResult SetFrozenState(FrozenState frozenState);

        /// <summary>
        /// Sets the fleet state (enabling or disabling vehicles).
        /// </summary>
        /// <param name="controllerStates">Desired controller state.</param>
        /// <returns>Successful service call result on success.</returns>
        IServiceCallResult SetFleetState(VehicleControllerState controllerStates);

        /// <summary>
        /// Sets an individual Kingpins controller state.
        /// </summary>
        /// <param name="ipAddress">IPv4 address of target vehicle.</param>
        /// <param name="controllerState">Desired controller state.</param>
        /// <returns>Successful service call result on success.</returns>
        IServiceCallResult SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState);

        /// <summary>
        /// Sets the pose of a vehicle.
        /// </summary>
        /// <param name="ipAddress">IPv4 address of target vehicle.</param>
        /// <param name="pose">Desired pose (position + orientation).</param>
        /// <returns>Successful service call result on success.</returns>
        IServiceCallResult SetPose(IPAddress ipAddress, PoseDto pose);

        /// <summary>
        /// Gets the API version of the server interface.
        /// </summary>
        /// <returns>A SemVerDto object representing the semantic version of the server interface.</returns>
        IServiceCallResult<SemVerDto> GetAPISemVer();
    }
}
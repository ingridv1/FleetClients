using BaseClients;
using FleetClients.Core.FleetManagerServiceReference;
using GACore;
using GACore.Architecture;
using System.Net;
using GAAPICommon.Core.Dtos;
using System.Xml.Linq;
using GAAPICommon.Architecture;
using BaseClients.Architecture;

namespace FleetClients.Core
{
	/// <summary>
	/// For interacting with the fleet manager remotely.
	/// </summary>
	public interface IFleetManagerClient : ICallbackClient, IModelCollection<KingpinStateMailbox>
	{
		FleetState FleetState { get; }

		IServiceCallResult CreateVirtualVehicle(IPAddress ipAddress, PoseData pose);

		IServiceCallResult<XElement> GetKingpinDescription(IPAddress ipAddress);

		IServiceCallResult RemoveVehicle(IPAddress ipAddress);

		IServiceCallResult RequestFreeze();

		IServiceCallResult RequestUnfreeze();

		IServiceCallResult ResetKingpin(IPAddress ipAddress);

		IServiceCallResult SetFleetState(VehicleControllerState controllerStates);

		IServiceCallResult SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState);

		IServiceCallResult SetPose(IPAddress ipAddress, PoseData pose);

		IServiceCallResult<SemVerDto> GetAPISemVer();
	}
}
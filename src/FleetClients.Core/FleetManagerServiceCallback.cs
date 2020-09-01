using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Core.Dtos;
using System;

namespace FleetClients.Core
{
    public class FleetManagerServiceCallback : IFleetManagerService_PublicAPI_v2_0Callback
    {
        public FleetManagerServiceCallback()
        {
        }

        public event Action<FleetStateDto> FleetStateUpdate;

        public void OnCallback(FleetStateDto fleetState)
        {
            Action<FleetStateDto> handlers = FleetStateUpdate;

            if (handlers != null)
            {
                foreach (Action<FleetStateDto> handler in handlers.GetInvocationList())
                {
                    handler.BeginInvoke(fleetState, null, null);
                }
            }
        }
    }
}
using BaseClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Architecture;
using GAAPICommon.Core;
using GAAPICommon.Core.Dtos;
using GACore.Architecture;
using GACore.Extensions;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Xml.Linq;

namespace FleetClients.Core
{
    internal class FleetManagerClient : AbstractCallbackClient<IFleetManagerService_PublicAPI_v2_0>, IFleetManagerClient
    {
        private readonly FleetManagerServiceCallback callback = new FleetManagerServiceCallback();

        private readonly List<KingpinStateMailbox> kingpinStateMailboxes = new List<KingpinStateMailbox>();

        private FleetState fleetState = null;

        private bool isDisposed = false;

        /// <summary>
        /// Creates a new FleetManagerClient
        /// </summary>
        /// <param name="netTcpUri">net.tcp address of the job builder service</param>
        public FleetManagerClient(Uri netTcpUri, TimeSpan heartbeat = default)
            : base(netTcpUri, heartbeat)
        {
            callback.FleetStateUpdate += Callback_FleetStateUpdate;
        }

        ~FleetManagerClient()
        {
            Dispose(false);
        }

        public event Action<KingpinStateMailbox> Added;

        public event Action<KingpinStateMailbox> Removed;

        public FleetState FleetState
        {
            get { return fleetState; }
            set
            {
                if (fleetState == null || value.Tick.IsCurrentByteTickLarger(fleetState.Tick))
                    fleetState = value;
            }
        }

        public IServiceCallResult CreateVirtualVehicle(IPAddress ipAddress, PoseDto pose)
        {
            Logger.Trace("CreateVirtualVehicle");
            return HandleAPICall(e => e.CreateVirtualVehicle(ipAddress, pose));
        }

        public IServiceCallResult<SemVerDto> GetAPISemVer()
        {
            Logger.Trace("GetAPISemVer");
            return HandleAPICall<SemVerDto>(e => e.GetAPISemVer());
        }          

        public IServiceCallResult<XElement> GetKingpinDescription(IPAddress ipAddress)
        {
            Logger.Trace("GetKingpinDescription");
            return HandleAPICall<XElement>(e => e.GetKingpinDescription(ipAddress));
        }

        public IEnumerable<KingpinStateMailbox> GetModels() => kingpinStateMailboxes.ToList();

        public IServiceCallResult RemoveVehicle(IPAddress ipAddress)
        {
            Logger.Trace("RemoveVehicle");
            return HandleAPICall(e => e.RemoveVehicle(ipAddress));
        }

        public IServiceCallResult RequestFreeze()
        {
            Logger.Trace("RequestFreeze");
            return HandleAPICall(e => e.RequestFreeze());
        }

        public IServiceCallResult RequestUnfreeze()
        {
            Logger.Trace("RequestUnfreeze");
            return HandleAPICall(e => e.RequestUnfreeze());
        }

        public IServiceCallResult ResetKingpin(IPAddress ipAddress)
        {
            Logger.Trace("ResetKingpin");
            return HandleAPICall(e => e.ResetKingpin(ipAddress));
        }

        public IServiceCallResult SetFleetState(VehicleControllerState controllerState)
        {
            Logger.Trace("SetFleetState");
            return HandleAPICall(e => e.SetFleetState(controllerState));
        }

        public IServiceCallResult SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState)
        {
            Logger.Trace("SetKingpinState");
            return HandleAPICall(e => e.SetKingpinState(ipAddress, controllerState));
        }

        public IServiceCallResult SetPose(IPAddress ipAddress, PoseDto pose)
        {
            Logger.Trace("SetPose");
            return HandleAPICall(e => e.SetPose(ipAddress, pose));
        }

        protected override void Dispose(bool isDisposing)
        {
            Logger.Debug($"Dispose isDisposing:{isDisposing}");

            if (isDisposed)
                return;

            if (isDisposing)
                callback.FleetStateUpdate -= Callback_FleetStateUpdate;

            base.Dispose(isDisposing);

            isDisposed = true;
        }
  
        protected override void SetInstanceContext()
        {
            context = new InstanceContext(this.callback);
        }

        private void Callback_FleetStateUpdate(FleetState fleetState)
        {
            FleetState = fleetState;

            lock (kingpinStateMailboxes)
            {
                foreach (IKingpinState kingpinState in fleetState.KingpinStates)
                {
                    KingpinStateMailbox mailbox = kingpinStateMailboxes.FirstOrDefault(e => e.Key.Equals(kingpinState.IPAddress));

                    if (mailbox != null)
                        mailbox.Update(kingpinState);
                    else
                    {
                        KingpinStateMailbox mailBox = new KingpinStateMailbox(kingpinState.IPAddress, kingpinState);

                        kingpinStateMailboxes.Add(mailBox);
                        OnAdded(mailBox);
                    }
                }

                IEnumerable<IPAddress> activeIP = fleetState.KingpinStates.Select(e => e.IPAddress);
                IEnumerable<IPAddress> deadIP = kingpinStateMailboxes.Select(e => e.Key).Except(activeIP).ToList();

                foreach (IPAddress ipAddress in deadIP)
                {
                    KingpinStateMailbox deadMailBox = kingpinStateMailboxes.First(e => e.Key == ipAddress);
                    kingpinStateMailboxes.Remove(deadMailBox);

                    OnRemoved(deadMailBox);
                }
            }
        }

        private void OnAdded(KingpinStateMailbox kingpinStateMailbox)
        {
            Action<KingpinStateMailbox> handlers = Added;

            handlers?
                .GetInvocationList()
                .Cast<Action<KingpinStateMailbox>>()
                .ForEach(e => e.BeginInvoke(kingpinStateMailbox, null, null));
        }

        public event Action<FleetState> FleetStateUpdated
        {
            add { callback.FleetStateUpdate += value; }
            remove { callback.FleetStateUpdate -= value; }            
        }

        private void OnRemoved(KingpinStateMailbox kingpinStateMailbox)
        {
            Action<KingpinStateMailbox> handlers = Removed;

            handlers?
                .GetInvocationList()
                .Cast<Action<KingpinStateMailbox>>()
                .ForEach(e => e.BeginInvoke(kingpinStateMailbox, null, null));
        }

        protected override void HandleSubscriptionHeartbeat(IFleetManagerService_PublicAPI_v2_0 channel, Guid key)
        {
            channel.SubscriptionHeartbeat(key);
        }
    }
}
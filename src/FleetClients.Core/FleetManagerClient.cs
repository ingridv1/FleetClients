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
using System.ServiceModel;
using System.Xml.Linq;

namespace FleetClients.Core
{
    internal class FleetManagerClient : AbstractCallbackClient<IFleetManagerService_PublicAPI_v2_0>, IFleetManagerClient
    {
        private readonly FleetManagerServiceCallback callback = new FleetManagerServiceCallback();

        private bool isDisposed = false;

        private readonly List<KingpinStateMailbox> kingpinStateMailboxes = new List<KingpinStateMailbox>();

        private readonly Queue<byte[]> buffer = new Queue<byte[]>();

        /// <summary>
        /// Creates a new FleetManagerClient
        /// </summary>
        /// <param name="netTcpUri">net.tcp address of the job builder service</param>
        public FleetManagerClient(Uri netTcpUri, TimeSpan heartbeat = default)
            : base(netTcpUri)
        {
            Heartbeat = heartbeat < TimeSpan.FromMilliseconds(1000) ? TimeSpan.FromMilliseconds(1000) : heartbeat;
            callback.FleetStateUpdate += Callback_FleetStateUpdate;
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

        public TimeSpan Heartbeat { get; private set; }

        ~FleetManagerClient()
        {
            Dispose(false);
        }

        public IServiceCallResult<XElement> GetKingpinDescription(IPAddress ipAddress)
        {
            Logger.Trace("GetKingpinDescription()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto<XElement> result = channel.GetKingpinDescription(ipAddress);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory<XElement>.FromClientException(ex);
            }
        }

        protected override void HeartbeatThread()
        {
            Logger.Trace("HeartbeatThread()");

            ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory();
            IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();

            bool? exceptionCaught;

            while (!Terminate)
            {
                exceptionCaught = null;

                try
                {
                    Logger.Trace("SubscriptionHeartbeat({0})", Key);
                    channel.SubscriptionHeartbeat(Key);
                    IsConnected = true;
                    exceptionCaught = false;
                }
                catch (EndpointNotFoundException)
                {
                    Logger.Warn("HeartbeatThread - EndpointNotFoundException. Is the server running?");
                    exceptionCaught = true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    exceptionCaught = true;
                }

                if (exceptionCaught == true)
                {
                    channelFactory.Abort();
                    IsConnected = false;

                    channelFactory = CreateChannelFactory(); // Create a new channel as this one is dead
                    channel = channelFactory.CreateChannel();
                }

                heartbeatReset.WaitOne(Heartbeat);
            }

            Logger.Trace("HeartbeatThread exit");
        }

        protected override void SetInstanceContext()
        {
            context = new InstanceContext(this.callback);
        }

        private FleetState fleetState = null;

        public event Action<KingpinStateMailbox> Added;

        public event Action<KingpinStateMailbox> Removed;

        private void OnRemoved(KingpinStateMailbox kingpinStateMailbox)
        {
            Action<KingpinStateMailbox> handlers = Removed;

            handlers?
                .GetInvocationList()
                .Cast<Action<KingpinStateMailbox>>()
                .ForEach(e => e.BeginInvoke(kingpinStateMailbox, null, null));
        }

        private void OnAdded(KingpinStateMailbox kingpinStateMailbox)
        {
            Action<KingpinStateMailbox> handlers = Added;

            handlers?
                .GetInvocationList()
                .Cast<Action<KingpinStateMailbox>>()
                .ForEach(e => e.BeginInvoke(kingpinStateMailbox, null, null));
        }

        public FleetState FleetState
        {
            get { return fleetState; }
            set
            {
                if (fleetState == null || value.Tick.IsCurrentByteTickLarger(fleetState.Tick))
                    fleetState = value;
            }
        }

        public IServiceCallResult RequestFreeze()
        {
            Logger.Trace("RequestFreeze()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.RequestFreeze();
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        public IServiceCallResult<SemVerDto> GetAPISemVer()
        {
            Logger.Trace("GetSemVer()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto<SemVerDto> result = channel.GetAPISemVer();
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory<SemVerDto>.FromClientException(ex);
            }
        }

        public IServiceCallResult RequestUnfreeze()
        {
            Logger.Trace("TryRequestUnfreeze()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.RequestUnfreeze();
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory<XElement>.FromClientException(ex);
            }
        }

        public IServiceCallResult CreateVirtualVehicle(IPAddress ipAddress, PoseData pose)
        {
            Logger.Trace("TryCreateVirtualVehicle()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.CreateVirtualVehicle(ipAddress, pose);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        public IServiceCallResult RemoveVehicle(IPAddress ipAddress)
        {
            Logger.Trace("RemoveVehicle()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.RemoveVehicle(ipAddress);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        public IServiceCallResult SetPose(IPAddress ipAddress, PoseData pose)
        {
            Logger.Trace("SetPose()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.SetPose(ipAddress, pose);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        public IServiceCallResult ResetKingpin(IPAddress ipAddress)
        {
            Logger.Trace("ResetKingpin()");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.ResetKingpin(ipAddress);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        public IServiceCallResult SetFleetState(VehicleControllerState controllerState)
        {
            Logger.Trace("SetFleetState");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.SetFleetState(controllerState);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        public IServiceCallResult SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState)
        {
            Logger.Trace("SetKingpinState");

            try
            {
                using (ChannelFactory<IFleetManagerService_PublicAPI_v2_0> channelFactory = CreateChannelFactory())
                {
                    IFleetManagerService_PublicAPI_v2_0 channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = channel.SetKingpinState(ipAddress, controllerState);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ServiceCallResultFactory.FromClientException(ex);
            }
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

        public IEnumerable<KingpinStateMailbox> GetModels() => kingpinStateMailboxes.ToList();
    }
}
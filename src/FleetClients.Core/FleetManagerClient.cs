using BaseClients;
using BaseClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Architecture;
using GAAPICommon.Core;
using GAAPICommon.Core.Dtos;
using GACore;
using GACore.Architecture;
using GACore.Extensions;
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

		private TimeSpan heartbeat;

		private readonly Queue<byte[]> buffer = new Queue<byte[]>();

		/// <summary>
		/// Creates a new FleetManagerClient
		/// </summary>
		/// <param name="netTcpUri">net.tcp address of the job builder service</param>
		public FleetManagerClient(Uri netTcpUri, TimeSpan heartbeat = default(TimeSpan))
			: base(netTcpUri)
		{
			this.heartbeat = heartbeat < TimeSpan.FromMilliseconds(1000) ? TimeSpan.FromMilliseconds(1000) : heartbeat;
			this.callback.FleetStateUpdate += Callback_FleetStateUpdate;
		}

		private readonly object lockObject = new object();

		private void Callback_FleetStateUpdate(FleetState fleetState)
		{
			FleetState = fleetState;

			lock (kingpinStateMailboxes)
			{
				foreach (IKingpinState kingpinState in fleetState.KingpinStates)
				{
					KingpinStateMailbox mailbox = kingpinStateMailboxes.FirstOrDefault(e => e.Key.Equals(kingpinState.IPAddress));

					if (mailbox != null)
					{
						mailbox.Update(kingpinState);
					}
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

		public TimeSpan Heartbeat => heartbeat;

		~FleetManagerClient()
		{
			Dispose(false);
		}

		protected override void HeartbeatThread()
		{
			Logger.Debug("HeartbeatThread()");

			ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory();
			IFleetManagerService fleetManagerService = channelFactory.CreateChannel();

			bool? exceptionCaught;

			while (!Terminate)
			{
				exceptionCaught = null;

				try
				{
					Logger.Trace("SubscriptionHeartbeat({0})", Key);
					fleetManagerService.SubscriptionHeartbeat(Key);
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
					fleetManagerService = channelFactory.CreateChannel();
				}

				heartbeatReset.WaitOne(Heartbeat);
			}

			Logger.Debug("HeartbeatThread exit");
		}

		protected override void SetInstanceContext()
		{
			this.context = new InstanceContext(this.callback);
		}

		private FleetState fleetState = null;

		public event Action<KingpinStateMailbox> Added;

		public event Action<KingpinStateMailbox> Removed;

		private void OnRemoved(KingpinStateMailbox kingpinStateMailbox)
		{
			if (Removed != null)
			{
				foreach (Action<KingpinStateMailbox> handler in Removed.GetInvocationList())
				{
					handler.BeginInvoke(kingpinStateMailbox, null, null);
				}
			}
		}

		private void OnAdded(KingpinStateMailbox kingpinStateMailbox)
		{
			if (Added != null)
			{
				foreach (Action<KingpinStateMailbox> handler in Added.GetInvocationList())
				{
					handler.BeginInvoke(kingpinStateMailbox, null, null);
				}
			}
		}

		public FleetState FleetState
		{
			get { return fleetState; }
			set
			{
				if (fleetState == null || value.Tick.IsCurrentByteTickLarger(fleetState.Tick))
				{
					fleetState = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		public IServiceCallResult RequestFreeze()
		{
			Logger.Info("RequestFreeze()");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}

		public ServiceOperationResult TryGetSemVer(out SemVerData semVerData)
		{
			Logger.Info("TryGetSemVer()");

			try
			{
				var result = GetSemVer();
				semVerData = result.Item1;

				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				semVerData = null;
				return HandleClientException(ex);
			}
		}

		public IServiceCallResult<XElement> TryGetKingpinDescription(IPAddress ipAddress)
		{
			Logger.Info("GetKingpinDescription()");

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
				return ServiceCallResultFactory<XElement>.FromClientException(ex);
			}
		}


		public IServiceCallResult RequestUnfreeze()
		{
			Logger.Info("TryRequestUnfreeze()");

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
				return ServiceCallResultFactory<XElement>.FromClientException(ex); 
			}
		}



		public IServiceCallResult CreateVirtualVehicle(IPAddress ipAddress, PoseData pose)
		{
			Logger.Info("TryCreateVirtualVehicle()");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}

		public IServiceCallResult RemoveVehicle(IPAddress ipAddress)
		{
			Logger.Info("RemoveVehicle()");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}

		public IServiceCallResult SetPose(IPAddress ipAddress, PoseData pose)
		{
			Logger.Info("SetPose()");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}

		public IServiceCallResult ResetKingpin(IPAddress ipAddress)
		{
			Logger.Info("ResetKingpin()");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}

		public IServiceCallResult SetFleetState(VehicleControllerState controllerState)
		{
			Logger.Info("SetFleetState");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}

		public IServiceCallResult SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState)
		{
			Logger.Info("SetKingpinState");

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
				return ServiceCallResultFactory.FromClientException(ex);
			}
		}




		protected override void Dispose(bool isDisposing)
		{
			Logger.Debug("Dispose({0})", isDisposing);

			if (isDisposed) return;

			if (isDisposing)
			{
				this.callback.FleetStateUpdate -= Callback_FleetStateUpdate;
			}

			base.Dispose(isDisposing);

			isDisposed = true;
		}

		private Tuple<SemVerData, ServiceCallData> GetSemVer()
		{
			Logger.Debug("GetSemVerData()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<SemVerData, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.GetSemVer();
				channelFactory.Close();
			}

			return result;
		}




		private Tuple<bool, ServiceCallData> CommitEx2Waypoints(IPAddress ipAddress, int instructionId, byte[] ex2Waypoints)
		{
			Logger.Debug("CommitExtendedWaypoints");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.CommitEx2Waypoints(ipAddress, instructionId, ex2Waypoints);
				channelFactory.Close();
			}

			return result;
		}







		public IEnumerable<KingpinStateMailbox> GetModels() => kingpinStateMailboxes.ToList();
	}
}
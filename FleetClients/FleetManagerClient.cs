using BaseClients;
using FleetClients.FleetManagerServiceReference;
using GACore;
using GACore.Architecture;
using GACore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Xml.Linq;

namespace FleetClients
{
	internal class FleetManagerClient : AbstractCallbackClient<IFleetManagerService>, IFleetManagerClient
	{
		private FleetManagerServiceCallback callback = new FleetManagerServiceCallback();

		private bool isDisposed = false;

		private List<KingpinStateMailbox> kingpinStateMailboxes = new List<KingpinStateMailbox>();

		private TimeSpan heartbeat;

		private Queue<byte[]> buffer = new Queue<byte[]>();

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

		/// <summary>
		/// Requests fleet manager freeze
		/// </summary>
		/// <param name="success">True if frozen</param>
		/// <returns>ServiceOperationResult</returns>
		public ServiceOperationResult TryRequestFreeze(out bool success)
		{
			Logger.Info("TryRequestFreeze()");

			try
			{
				var result = RequestFreeze();
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
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

		/// <summary>
		/// Get xDocument of kingpin description
		/// </summary>
		/// <param name="ipAddress">IPAddress</param>
		/// <param name="xDocument">kingpin.xml xDocument</param>
		/// <returns>ServiceOperationResult</returns>
		public ServiceOperationResult TryGetKingpinDescription(IPAddress ipAddress, out XDocument xDocument)
		{
			Logger.Info("TryGetKingpinDescription()");

			try
			{
				var result = GetKingpinDescription(ipAddress);
				XElement xElement = result.Item1;
				xDocument = new XDocument(xElement);

				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				xDocument = null;
				return HandleClientException(ex);
			}
		}

		/// <summary>
		/// Commit extended waypoints to a kingpin
		/// </summary>
		/// <returns>ServiceOperationResult</returns>
		public ServiceOperationResult TryCommitEx2Waypoints(IPAddress ipAddress, int instructionId, byte[] ex2Waypoints, out bool success)
		{
			Logger.Info("TryCommitExtendedWaypoints()");

			try
			{
				Tuple<bool, ServiceCallData> result = CommitEx2Waypoints(ipAddress, instructionId, ex2Waypoints);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		/// <summary>
		/// Requests fleet manager unfreeze
		/// </summary>
		/// <param name="success">True if unfrozen</param>
		/// <returns>ServiceOperationResult</returns>
		public ServiceOperationResult TryRequestUnfreeze(out bool success)
		{
			Logger.Info("TryRequestUnfreeze()");

			try
			{
				var result = RequestUnfreeze();
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		public ServiceOperationResult TryCreateVirtualVehicle(IPAddress ipAddress, PoseData pose, out bool success)
		{
			Logger.Info("TryCreateVirtualVehicle()");

			try
			{
				var result = CreateVirtualVehicle(ipAddress, pose);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		public ServiceOperationResult TryRemoveVehicle(IPAddress ipAddress, out bool success)
		{
			Logger.Info("TryRemoveVehicle()");

			try
			{
				var result = RemoveVehicle(ipAddress);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		public ServiceOperationResult TrySetPose(IPAddress ipAddress, PoseData pose, out bool success)
		{
			Logger.Info("TrySetPose()");

			try
			{
				var result = SetPose(ipAddress, pose);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		public ServiceOperationResult TryResetKingpin(IPAddress ipAddress, out bool success)
		{
			Logger.Info("TryResetKingpin()");

			try
			{
				var result = ResetKingpin(ipAddress);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		public ServiceOperationResult TrySetFleetState(VehicleControllerState controllerState, out bool success)
		{
			Logger.Info("TrySetFleetState");

			try
			{
				var result = SetFleetState(controllerState);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
			}
		}

		public ServiceOperationResult TrySetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState, out bool success)
		{
			Logger.Info("TrySetKingpinState");

			try
			{
				var result = SetKingpinState(ipAddress, controllerState);
				success = result.Item1;
				return ServiceOperationResultFactory.FromFleetManagerServiceCallData(result.Item2);
			}
			catch (Exception ex)
			{
				success = false;
				return HandleClientException(ex);
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

		private Tuple<XElement, ServiceCallData> GetKingpinDescription(IPAddress ipAddress)
		{
			Logger.Debug("GetKingpinDescription()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<XElement, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.GetKingpinDescription(ipAddress);
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> RequestFreeze()
		{
			Logger.Debug("RequestFreeze()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.RequestFreeze();
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

		private Tuple<bool, ServiceCallData> RequestUnfreeze()
		{
			Logger.Debug("RequestUnfreeze()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.RequestUnfreeze();
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> CreateVirtualVehicle(IPAddress ipAddress, PoseData pose)
		{
			Logger.Debug("CreateVirtualVehicle()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.CreateVirtualVehicle(ipAddress, pose);
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> RemoveVehicle(IPAddress ipAddress)
		{
			Logger.Debug("RemoveVehicle()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.RemoveVehicle(ipAddress);
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> SetPose(IPAddress ipAddress, PoseData pose)
		{
			Logger.Debug("SetPose()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.SetPose(ipAddress, pose);
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> ResetKingpin(IPAddress ipAddress)
		{
			Logger.Debug("ResetKingpin()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.ResetKingpin(ipAddress);
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> SetFleetState(VehicleControllerState controllerState)
		{
			Logger.Debug("SetFleetState()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.SetFleetState(controllerState);
				channelFactory.Close();
			}

			return result;
		}

		private Tuple<bool, ServiceCallData> SetKingpinState(IPAddress ipAddress, VehicleControllerState controllerState)
		{
			Logger.Debug("SetKingpinState()");

			if (isDisposed) throw new ObjectDisposedException("FleetManagerClient");

			Tuple<bool, ServiceCallData> result;

			using (ChannelFactory<IFleetManagerService> channelFactory = CreateChannelFactory())
			{
				IFleetManagerService channel = channelFactory.CreateChannel();
				result = channel.SetKingpinState(ipAddress, controllerState);
				channelFactory.Close();
			}

			return result;
		}

		public IEnumerable<KingpinStateMailbox> GetModels() => kingpinStateMailboxes.ToList();
	}
}
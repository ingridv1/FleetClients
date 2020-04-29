using BaseClients;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using NUnit.Framework;
using System.Net;
using System.Xml.Linq;

namespace FleetClients.Test
{
	[TestFixture]
	[Category("Fleet")]
	public class TFleetManagerClient_ClientExceptionNull
	{
		private IFleetManagerClient FleetManagerClient;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			FleetManagerClient = ClientFactory.CreateTcpFleetManagerClient(new EndpointSettings(IPAddress.Loopback));
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryRequestFreeze_ClientExceptionNull()
		{
			bool success;
			ServiceOperationResult result = FleetManagerClient.TryRequestFreeze(out success);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryGetKingpinDescription_ClientExceptionNull()
		{
			XDocument description;
			ServiceOperationResult result = FleetManagerClient.TryGetKingpinDescription(IPAddress.Loopback, out description);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryCommitExtendedWaypoints_ClientExceptionNull()
		{
			byte[] waypoints = { };
			bool success;
			ServiceOperationResult result = FleetManagerClient.TryCommitEx2Waypoints(IPAddress.Loopback, 1, waypoints, out success);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryRequestUnfreeze_ClientExceptionNull()
		{
			bool success;
			ServiceOperationResult result = FleetManagerClient.TryRequestUnfreeze(out success);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryCreateVirtualVehicle_ClientExceptionNull()
		{
			bool success;
			ServiceOperationResult result = FleetManagerClient.TryCreateVirtualVehicle(IPAddress.Loopback, new PoseData(), out success);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryRemoveVehicle_ClientExceptionNull()
		{
			bool success;
			ServiceOperationResult result = FleetManagerClient.TryRemoveVehicle(IPAddress.Loopback, out success);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TrySetPose_ClientExceptionNull()
		{
			bool success;
			ServiceOperationResult result = FleetManagerClient.TrySetPose(IPAddress.Loopback, new PoseData(), out success);

			Assert.IsNull(result.ClientException);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryResetKingpin_ClientExceptionNull()
		{
			bool success;
			ServiceOperationResult result = FleetManagerClient.TryResetKingpin(IPAddress.Loopback, out success);

			Assert.IsNull(result.ClientException);
		}
	}
}
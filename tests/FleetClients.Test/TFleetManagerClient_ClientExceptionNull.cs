using BaseClients.Core;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using NUnit.Framework;
using System.Net;

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
			var result = FleetManagerClient.RequestFreeze();
			Assert.IsNotNull(result.ExceptionMessage);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryGetKingpinDescription_ClientExceptionNull()
		{
			var result = FleetManagerClient.GetKingpinDescription(IPAddress.Loopback);
			Assert.IsNotNull(result.ExceptionMessage);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryRequestUnfreeze_ClientExceptionNull()
		{
			var result = FleetManagerClient.RequestUnfreeze();
			Assert.IsNotNull(result.ExceptionMessage);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryCreateVirtualVehicle_ClientExceptionNull()
		{
			var result = FleetManagerClient.CreateVirtualVehicle(IPAddress.Loopback, new PoseData());
			Assert.IsNotNull(result.ExceptionMessage);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryRemoveVehicle_ClientExceptionNull()
		{
			var result = FleetManagerClient.RemoveVehicle(IPAddress.Loopback);
			Assert.IsNotNull(result.ExceptionMessage);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TrySetPose_ClientExceptionNull()
		{
			var result = FleetManagerClient.SetPose(IPAddress.Loopback, new PoseData());
			Assert.IsNotNull(result.ExceptionMessage);
		}

		[Test]
		[Category("ClientExceptionNull")]
		public void TryResetKingpin_ClientExceptionNull()
		{
			var result = FleetManagerClient.ResetKingpin(IPAddress.Loopback);
			Assert.IsNotNull(result.ExceptionMessage);
		}
	}
}
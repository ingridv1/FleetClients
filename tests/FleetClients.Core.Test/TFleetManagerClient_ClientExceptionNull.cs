using BaseClients.Core;
using FleetClients.Core;
using GAAPICommon.Architecture;
using GAAPICommon.Core.Dtos;
using NUnit.Framework;
using System.Net;

namespace FleetClients.Core.Test
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

        [TestCase(FrozenState.Frozen)]
        [TestCase(FrozenState.Unfrozen)]
        [Category("ClientExceptionNull")]
        public void SetFrozenState_ClientExceptionNull(FrozenState frozenState)
        {
            var result = FleetManagerClient.SetFrozenState(frozenState);
            Assert.IsNotNull(result.ExceptionMessage);
        }

        [Test]
        [Category("ClientExceptionNull")]
        public void GetKingpinDescription_ClientExceptionNull()
        {
            var result = FleetManagerClient.GetKingpinDescription(IPAddress.Loopback);
            Assert.IsNotNull(result.ExceptionMessage);
        }

        [Test]
        [Category("ClientExceptionNull")]
        public void CreateVirtualVehicle_ClientExceptionNull()
        {
            var result = FleetManagerClient.CreateVirtualVehicle(IPAddress.Loopback, new PoseDto());
            Assert.IsNotNull(result.ExceptionMessage);
        }

        [Test]
        [Category("ClientExceptionNull")]
        public void RemoveVehicle_ClientExceptionNull()
        {
            var result = FleetManagerClient.RemoveVehicle(IPAddress.Loopback);
            Assert.IsNotNull(result.ExceptionMessage);
        }

        [Test]
        [Category("ClientExceptionNull")]
        public void SetPose_ClientExceptionNull()
        {
            var result = FleetManagerClient.SetPose(IPAddress.Loopback, new PoseDto());
            Assert.IsNotNull(result.ExceptionMessage);
        }
    }
}
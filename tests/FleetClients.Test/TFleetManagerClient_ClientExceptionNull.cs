using BaseClients.Core;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using GACore.Architecture;
using GAAPICommon.Architecture;
using NUnit.Framework;
using System.Net;
using GAAPICommon.Core.Dtos;

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
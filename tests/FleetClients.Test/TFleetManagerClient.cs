using BaseClients.Core;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Architecture;
using GAAPICommon.Core.Dtos;
using GACore.Architecture;
using NUnit.Framework;
using System.Net;

namespace FleetClients.Test
{
    /// <summary>
    /// Requires a server to be running on local host to be successful
    /// </summary>
    [TestFixture]
    [Category("FleetManagerClient")]
    public class TFleetManagerClient
    {
        private EndpointSettings settings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            settings = new EndpointSettings(ipAddress);
        }

        [Test]
        public void Unfreeze()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.SetFrozenState(FrozenState.Unfrozen);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void Freeze()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.SetFrozenState(FrozenState.Frozen);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        [TestCase("192.66.1.99")]
        public void TestVirtual(string ipV4String)
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            PoseDto poseDto = new PoseDto()
            {
                X = 0,
                Y = 0,
                Heading = 0
            };

            var result = client.CreateVirtualVehicle(IPAddress.Parse(ipV4String), poseDto);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void EnableVirtual()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.SetKingpinState(IPAddress.Parse("192.0.2.0"), VehicleControllerState.Enabled);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void DisableVirtual()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.SetKingpinState(IPAddress.Parse("192.0.2.0"), VehicleControllerState.Disabled);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void CreateVirtual()
        {
            PoseDto pose = new PoseDto()
            {
                X = -3,
                Y = -2,
                Heading = 0
            };

            IPAddress ipAddress = IPAddress.Parse("192.0.2.5");
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.CreateVirtualVehicle(ipAddress, pose);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void RemoveAGV()
        {
            IPAddress ipAddress = IPAddress.Parse("192.0.2.5");
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.RemoveVehicle(ipAddress);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        [TestCase(0, 0, 0)]
        public void SetPose(double x, double y, double heading)
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);
            PoseDto poseDto = new PoseDto()
            {
                X = x,
                Y = y,
                Heading = heading
            };

            var result = client.SetPose(IPAddress.Parse("192.0.2.5"), poseDto);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void EnableFleet()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.SetFleetState(VehicleControllerState.Enabled);
            Assert.AreEqual(0, result.ServiceCode);
        }

        [Test]
        public void DisableFleet()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            var result = client.SetFleetState(VehicleControllerState.Disabled);
            Assert.AreEqual(0, result.ServiceCode);
        }
    }
}
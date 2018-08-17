﻿using System.Net;
using BaseClients;
using FleetClients.FleetManagerServiceReference;
using NUnit.Framework;

namespace FleetClients.Test
{
    /// <summary>
    /// Requires a server to be running on local host to be succesfull
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

            bool success;
            client.TryRequestUnfreeze(out success);

            Assert.IsTrue(success);
        }        

        [Test]
        public void Freeze()
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            bool success;
            client.TryRequestFreeze(out success);

            Assert.IsTrue(success);
        }

        [Test]
        [TestCase("192.66.1.99")]
        public void TestVirtual(string ipV4String)
        {
            IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(settings);

            PoseData poseData = new PoseData()
            {
                X = 0,
                Y = 0,
                Heading = 0
            };

            bool success;
            client.TryCreateVirtualVehicle(IPAddress.Parse(ipV4String), poseData, out success);

            Assert.IsTrue(success);
        }
    }
}

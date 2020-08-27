using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetClients.Core;
using FleetClients.Core.FleetManagerServiceReference;
using BaseClients.Core;
using System.Net;
using FleetClients.Core.Client_Interfaces;
using GAAPICommon.Architecture;

namespace Tutorial_01
{
    /// <summary>
    /// A simple tutorial that shows how to create a virtual vehicle and reposition it:
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Here we create an endpoint settings object that defines where the fleet manager service is currently running
            // For this demo we are assuming it is running on localhost, using the default TCP port of 41917.
            EndpointSettings endpointSettings = new EndpointSettings(IPAddress.Loopback, 41916, 41917);

            // Now we create a fleet manager client using the client factory;
            IFleetManagerClient fleetManagerClient = ClientFactory.CreateTcpFleetManagerClient(endpointSettings);


            Console.WriteLine("Press <any> key to create a virtual vehicle 192.168.0.1 at 0,0,0");
            Console.ReadKey(true);

            IPAddress virtualVehicle = IPAddress.Parse("192.168.0.1");
            IServiceCallResult result = fleetManagerClient.CreateVirtualVehicle(virtualVehicle, 0, 0, 0);
            if (!result.IsSuccessful())
                throw new Exception(result.ToString());

            // Now we can update the vehicles pose
            Console.WriteLine("Press <any> key to set the pose of the vehicle to 1,1,1.57");
            Console.ReadKey(true);

            result = fleetManagerClient.SetPose(virtualVehicle, 1, 1, 1.57);
            if (!result.IsSuccessful())
                throw new Exception(result.ToString());

            Console.WriteLine("Press <any> key to quit");
            Console.ReadKey(true);

            // The fleet manager client has its own thread which must be disposed.
            fleetManagerClient.Dispose();
        }
    }
}

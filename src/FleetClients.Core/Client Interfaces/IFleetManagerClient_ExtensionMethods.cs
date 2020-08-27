using FleetClients.Core.FleetManagerServiceReference;
using GAAPICommon.Architecture;
using System;
using System.Net;

namespace FleetClients.Core.Client_Interfaces
{
    public static class IFleetManagerClient_ExtensionMethods
    {
        public static IServiceCallResult CreateVirtualVehicle(this IFleetManagerClient client, IPAddress ipAddress, double x, double y, double heading)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");

            PoseDto poseDto = new PoseDto()
            {
                X = x,
                Y = y,
                Heading = heading
            };

            return client.CreateVirtualVehicle(ipAddress, poseDto);
        }

        public static IServiceCallResult SetPose(this IFleetManagerClient client, IPAddress ipAddress, double x, double y, double heading)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");

            PoseDto poseDto = new PoseDto()
            {
                X = x,
                Y = y,
                Heading = heading
            };

            return client.SetPose(ipAddress, poseDto);
        }
    }
}

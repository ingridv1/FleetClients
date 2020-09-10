using GAAPICommon.Architecture;
using GAAPICommon.Core.Dtos;
using System;
using System.Net;

namespace FleetClients.Core.Client_Interfaces
{
    /// <summary>
    /// Extension methods for the fleet manager client.
    /// </summary>
    public static class IFleetManagerClient_ExtensionMethods
    {
        /// <summary>
        /// Creates a new virtual vehicle.
        /// </summary>
        /// <param name="client">The fleet manager client to use.</param>
        /// <param name="ipAddress">IPv4 address of the vehicle to be created.</param>
        /// <param name="x">X position in meters.</param>
        /// <param name="y">Y position in meters.</param>
        /// <param name="heading">Heading in radians.</param>
        /// <returns>Successful service call result on creation.</returns>
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

        /// <summary>
        /// Sets the pose of a vehicle.
        /// </summary>
        /// <param name="client">The fleet manager client to use.</param>
        /// <param name="ipAddress">IPv4 address of target vehicle.</param>
        /// <param name="x">X position in meters.</param>
        /// <param name="y">Y position in meters.</param>
        /// <param name="heading">Heading in radians.</param>
        /// <returns>Successful service call result on success.</returns>
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
using GAAPICommon.Core.Dtos;
using System.Net;
using System.Runtime.Serialization;
using SN = System.Net;

namespace FleetClients.Core
{
    /// <summary>
    /// Lightweight object to couple an ipAddress and a pose as a template for a virtual vehicle.
    /// </summary>
    [DataContract]
    public class AGVTemplate
    {
        /// <summary>
        /// IP address in ipV4 format e.g. 192.168.0.100
        /// </summary>
        [DataMember]
        public string IPV4String { get; set; } = "192.168.0.100";

        /// <summary>
        /// Pose in x,y,heading format e.g. 0,0,0
        /// </summary>
        [DataMember]
        public string PoseDataString { get; set; } = "0,0,0";

        /// <summary>
        /// Gets IPv4 address
        /// </summary>
        /// <returns>IPAddress</returns>
        public IPAddress GetIPV4Address()
            => (SN.IPAddress.TryParse(IPV4String, out IPAddress parsed)) ? parsed : null;

        /// <summary>
        /// Converts PoseDataString to PoseData.
        /// </summary>
        /// <returns>Parsed PoseData</returns>
        public PoseDto ToPoseDto()
        {
            PoseDtoFactory.TryParseString(PoseDataString, out PoseDto poseDto);
            return poseDto;
        }

        /// <summary>
        /// Provides a short summary in the form IPAddress, Pose.
        /// </summary>
        /// <returns>Summary string</returns>
        public string ToSummaryString() => string.Format("IPAddress:{0} Pose:{1}", IPV4String, PoseDataString);

        public override string ToString() => ToSummaryString();
    }
}
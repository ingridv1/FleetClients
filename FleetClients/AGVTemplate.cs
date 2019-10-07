using FleetClients.FleetManagerServiceReference;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SN = System.Net;

namespace FleetClients
{
	/// <summary>
	/// Lightweight object to couple an ipAddress and a pose.
	/// </summary>
	[DataContract]
	public class AGVTemplate : INotifyPropertyChanged
	{
		private IPAddress ipAddress = null;

		private string poseDataString = string.Empty;

		[DataMember]
		public string IPV4String
		{
			get { return ipAddress != null ? ipAddress.MapToIPv4().ToString() : string.Empty; }
			set
			{
				if (SN.IPAddress.TryParse(value, out IPAddress parsed))
				{
					if (ipAddress != parsed)
					{
						ipAddress = parsed;
						OnNotifyPropertyChanged();
					}
				}	
			}
		}

		public IPAddress GetIPV4Address() => ipAddress;

		[DataMember]
		public string PoseDataString
		{
			get { return poseDataString; }
			set
			{
				if (poseDataString != value)
				{
					poseDataString = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		public PoseData ToPoseData()
		{
			PoseDataFactory.TryParseString(PoseDataString, out PoseData poseData);
			return poseData;
		}		
		
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public string ToSummaryString() => string.Format("IPAddress:{0} Pose:{1}", IPV4String, PoseDataString);

		public override string ToString() => ToSummaryString();
	}
}

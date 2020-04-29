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
		private string ipV4string = "192.168.0.100";

		private string poseDataString = "0,0,0";

		[DataMember]
		public string IPV4String
		{
			get { return ipV4string; }
			set
			{
				if (value == null) value = string.Empty;

				if (ipV4string != value)
				{
					ipV4string = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		public IPAddress GetIPV4Address()
			=> (SN.IPAddress.TryParse(IPV4String, out IPAddress parsed)) ? parsed : null;

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
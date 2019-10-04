using FleetClients.FleetManagerServiceReference;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

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
		public IPAddress IPAddress
		{
			get { return IPAddress; }
			set
			{
				if (ipAddress != value)
				{
					ipAddress = value;
					OnNotifyPropertyChanged();
				}
			}
		}

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
	}
}

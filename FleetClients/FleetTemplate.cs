using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace FleetClients
{
	[DataContract]
	public class FleetTemplate
	{
		private readonly object lockObject = new object();

		private ObservableCollection<AGVTemplate> agvTemplates = new ObservableCollection<AGVTemplate>();

		private ReadOnlyObservableCollection<AGVTemplate> readonlyTemplates;

		public FleetTemplate()
		{
			readonlyTemplates = new ReadOnlyObservableCollection<AGVTemplate>(agvTemplates);
		}

		public void Populate(IFleetManagerClient fleetManagerClient)
		{
			if (fleetManagerClient == null) throw new ArgumentNullException("fleetManagerClient");

			lock (lockObject)
			{
				foreach(AGVTemplate agvTemplate in AGVTemplates.ToList())
				{
					fleetManagerClient.TryCreateVirtualVehicle(agvTemplate.GetIPV4Address(), agvTemplate.ToPoseData(), out bool success);
				}
			}
		}

		public void Clear()
		{
			lock (lockObject) agvTemplates.Clear();
		}

		[DataMember]
		public IEnumerable<AGVTemplate> AGVTemplates
		{
			get { return agvTemplates.ToList(); }
			set
			{
				agvTemplates.Clear();

				foreach (AGVTemplate agvTemplate in value)
				{
					Add(agvTemplate);
				}
			}
		}


		public ReadOnlyObservableCollection<AGVTemplate> AGVTemplatesOC => readonlyTemplates;

		public void Add(AGVTemplate agvTemplate)
		{
			if (agvTemplate == null) throw new ArgumentNullException("agvTemplate");

			lock (agvTemplates)
			{
				agvTemplates.Add(agvTemplate);
			}
		}
	}
}
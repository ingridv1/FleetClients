using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GACore;
using GACore.Architecture;
using System.Runtime.Serialization;
using MoreLinq;

namespace FleetClients
{
	[DataContract]
	public class FleetTemplate : IModelCollection<AGVTemplate>
	{
		private readonly object lockObject = new object();

		private List<AGVTemplate> agvTemplates = new List<AGVTemplate>();

		public event Action<AGVTemplate> Added;

		public event Action<AGVTemplate> Removed;

		private void OnRemoved(AGVTemplate agvTemplate)
		{
			if (Removed != null)
			{
				foreach (Action<AGVTemplate> handler in Removed.GetInvocationList())
				{
					handler.BeginInvoke(agvTemplate, null, null);
				}
			}
		}

		private void OnAdded(AGVTemplate agvTemplate)
		{
			if (Added != null)
			{
				foreach(Action<AGVTemplate> handler in Added.GetInvocationList())
				{
					handler.BeginInvoke(agvTemplate, null, null);
				}
			}
		}

		public FleetTemplate()
		{
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
			lock (lockObject)
			{
				while (agvTemplates.Any())
				{
					AGVTemplate first = agvTemplates.First();
					agvTemplates.RemoveAt(0);
					OnRemoved(first);
				}	

			}
		}

		public void Remove(AGVTemplate agvTemplate)
		{
			lock (lockObject)
			{
				agvTemplates.Remove(agvTemplate);
				OnRemoved(agvTemplate);
			}
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

		public void Add(AGVTemplate agvTemplate)
		{
			if (agvTemplate == null) throw new ArgumentNullException("agvTemplate");

			lock (agvTemplates)
			{
				agvTemplates.Add(agvTemplate);
				OnAdded(agvTemplate);
			}
		}

		public IEnumerable<AGVTemplate> GetModels() => agvTemplates.ToList();
	}
}
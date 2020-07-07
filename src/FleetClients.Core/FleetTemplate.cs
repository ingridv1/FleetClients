using GACore.Architecture;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FleetClients.Core
{
	/// <summary>
	/// A template for a virtual vehicle fleet. Used to quickly populate the fleet manager with pre-fabricated fleets of AGVs at set poses.
	/// </summary>
	[DataContract]
	public class FleetTemplate : IModelCollection<AGVTemplate>
	{
		private readonly object lockObject = new object();

		private readonly List<AGVTemplate> agvTemplates = new List<AGVTemplate>();

		/// <summary>
		/// Occurs whenever an AGV template is added to the fleet template.
		/// </summary>
		public event Action<AGVTemplate> Added;

		/// <summary>
		/// Occurs whenever an AGV template is removed from the fleet template.
		/// </summary>
		public event Action<AGVTemplate> Removed;

		private void OnRemoved(AGVTemplate agvTemplate)
		{
			Action<AGVTemplate> handlers = Removed;

			handlers?
				.GetInvocationList()
				.Cast<Action<AGVTemplate>>()
				.ForEach(e => e.BeginInvoke(agvTemplate, null, null));
		}

		private void OnAdded(AGVTemplate agvTemplate)
		{
			Action<AGVTemplate> handlers = Added;

			handlers?
				.GetInvocationList()
				.Cast<Action<AGVTemplate>>()
				.ForEach(e => e.BeginInvoke(agvTemplate, null, null));
		}

		public FleetTemplate()
		{
		}

		/// <summary>
		/// Attempts to create virtual vehicles in the fleet manager for every AGV template in the collection. 
		/// </summary>
		/// <param name="fleetManagerClient">Fleet manager client to use</param>
		public void Populate(IFleetManagerClient fleetManagerClient)
		{
			if (fleetManagerClient == null) throw new ArgumentNullException("fleetManagerClient");

			lock (lockObject)
			{
				foreach (AGVTemplate agvTemplate in AGVTemplates.ToList())
				{
					_ = fleetManagerClient.CreateVirtualVehicle(agvTemplate.GetIPV4Address(), agvTemplate.ToPoseData());
				}
			}
		}

		/// <summary>
		/// Removes all AGV templates from the fleet template.
		/// </summary>
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

		/// <summary>
		/// Removes an AGV template from the fleet template
		/// </summary>
		/// <param name="agvTemplate">AGV template to remove</param>
		public void Remove(AGVTemplate agvTemplate)
		{
			lock (lockObject)
			{
				agvTemplates.Remove(agvTemplate);
				OnRemoved(agvTemplate);
			}
		}

		/// <summary>
		/// Enumerable collection of AGV templates
		/// </summary>
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

		/// <summary>
		/// Adds a new AGV template to the fleet template.
		/// </summary>
		/// <param name="agvTemplate">AGV template to add</param>
		public void Add(AGVTemplate agvTemplate)
		{
			if (agvTemplate == null) throw new ArgumentNullException("agvTemplate");

			lock (agvTemplates)
			{
				agvTemplates.Add(agvTemplate);
				OnAdded(agvTemplate);
			}
		}

		/// <summary>
		/// Returns all AGV templates in the fleet template.
		/// </summary>
		/// <returns>Enumerable of AGV templates</returns>
		public IEnumerable<AGVTemplate> GetModels() => agvTemplates.ToList();
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetClients.JsonConverters;
using Newtonsoft.Json;

namespace FleetClients
{
	public static class JsonFactory
	{

		private static JsonSerializerSettings GetJsonSerializerSettings()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Converters.Add(new IEnumerableAGVTemplateConverter());
			settings.Formatting = Formatting.Indented;

			return settings;
		}


		public static FleetTemplate FleetTemplateFromFile(string filePath)
		{
			using (StreamReader file = File.OpenText(filePath))
			{
				JsonSerializer serializer = JsonSerializer.Create(GetJsonSerializerSettings());
				return (FleetTemplate)serializer.Deserialize(file, typeof(FleetTemplate));
			}
		}


		public static string ToJson(this FleetTemplate fleetTemplate)
		{
			if (fleetTemplate == null) throw new ArgumentNullException("fleetTemplate");

			JsonSerializerSettings settings = GetJsonSerializerSettings();
			return JsonConvert.SerializeObject(fleetTemplate, settings);
		}
	}
}

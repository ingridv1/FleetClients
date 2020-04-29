using FleetClients.Core.JsonConverters;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FleetClients.Core
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

		public static void ToFile(this FleetTemplate fleetTemplate, string filePath)
		{
			string json = fleetTemplate.ToJson();
			File.WriteAllText(filePath, json);
		}

		public static string ToJson(this FleetTemplate fleetTemplate)
		{
			if (fleetTemplate == null) throw new ArgumentNullException("fleetTemplate");

			JsonSerializerSettings settings = GetJsonSerializerSettings();
			return JsonConvert.SerializeObject(fleetTemplate, settings);
		}
	}
}
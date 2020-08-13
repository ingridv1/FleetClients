using FleetClients.Core.JsonConverters;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FleetClients.Core
{
    /// <summary>
    /// Factory class for json maniupulating json.
    /// </summary>
    public static class JsonFactory
    {
        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new IEnumerableAGVTemplateConverter());
            settings.Formatting = Formatting.Indented;

            return settings;
        }

        /// <summary>
        /// Deserializes a fleet template .json file.
        /// </summary>
        /// <param name="filePath">Path to *.json</param>
        /// <returns>Parsed FleetTemplate</returns>
        public static FleetTemplate FleetTemplateFromFile(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = JsonSerializer.Create(GetJsonSerializerSettings());
                return (FleetTemplate)serializer.Deserialize(file, typeof(FleetTemplate));
            }
        }

        /// <summary>
        /// Serializes a fleet template to .json.
        /// </summary>
        /// <param name="fleetTemplate">Fleet Template to be serialized</param>
        /// <param name="filePath">Output file path</param>
        public static void ToFile(this FleetTemplate fleetTemplate, string filePath)
        {
            if (fleetTemplate == null) throw new ArgumentNullException("fleetTemplate");

            if (string.IsNullOrEmpty(filePath)) throw new ArgumentOutOfRangeException("filePath");

            string json = fleetTemplate.ToJson();
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Converts fleet template to json string.
        /// </summary>
        /// <param name="fleetTemplate">Fleet template to be serialized</param>
        /// <returns>Json string</returns>
        public static string ToJson(this FleetTemplate fleetTemplate)
        {
            if (fleetTemplate == null) throw new ArgumentNullException("fleetTemplate");

            JsonSerializerSettings settings = GetJsonSerializerSettings();
            return JsonConvert.SerializeObject(fleetTemplate, settings);
        }
    }
}
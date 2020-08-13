using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FleetClients.Core.JsonConverters
{
    public class IEnumerableAGVTemplateConverter : JsonConverter<List<AGVTemplate>>
    {
        public override bool CanWrite => false;

        public override List<AGVTemplate> ReadJson(JsonReader reader, Type objectType, List<AGVTemplate> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<AGVTemplate> agvTemplates = new List<AGVTemplate>();

            foreach (JObject jObject in JArray.Load(reader))
            {
                AGVTemplate agvTemplate = jObject.ToObject<AGVTemplate>();
                agvTemplates.Add(agvTemplate);
            }

            return agvTemplates;
        }

        public override void WriteJson(JsonWriter writer, List<AGVTemplate> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
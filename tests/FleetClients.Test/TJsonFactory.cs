using FleetClients.Core;
using NUnit.Framework;
using System.IO;

namespace FleetClients.Test
{
    [TestFixture]
    [Category("FleetTemplate")]
    public class TJsonFactory
    {
        [Test]
        public void Serialize_Deserialize()
        {
            FleetTemplate fleetTemplate = new FleetTemplate();

            fleetTemplate.Add(new AGVTemplate() { IPV4String = "192.168.0.1", PoseDataString = "0,0,90" });
            fleetTemplate.Add(new AGVTemplate() { IPV4String = "192.168.0.2", PoseDataString = "10,0,90" });

            string json = fleetTemplate.ToJson();
            Assert.IsNotNull(json);

            string filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, json);

            FleetTemplate fleetTemplateLoaded = JsonFactory.FleetTemplateFromFile(filePath);

            Assert.IsNotNull(fleetTemplateLoaded);
            CollectionAssert.IsNotEmpty(fleetTemplateLoaded.AGVTemplates);
        }
    }
}
using BaseClients.Core;
using CommandLine;
using FleetClients.Core;
using GAAPICommon.Architecture;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace FleetClients.FleetClientConsole.Options
{
    [Verb("getdescription", HelpText = "Gets Kingpin description")]
    public class GetKingpinDescriptionOptions : AbstractConsoleOption<IFleetManagerClient>
    {
        [Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
        public string IPv4String { get; set; }

        protected override IServiceCallResult HandleExecution(IFleetManagerClient client)
        {
            IPAddress ipAddress = IPAddress.Parse(IPv4String);

            IServiceCallResult<XElement> result = client.GetKingpinDescription(ipAddress);

            if (result.ServiceCode == 0)
            {
                XDocument xDocument = new XDocument(result.Value);
                string fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".xml";
                xDocument.Save(fileName);

                Process.Start(fileName);
            }

            return result;
        }
    }
}
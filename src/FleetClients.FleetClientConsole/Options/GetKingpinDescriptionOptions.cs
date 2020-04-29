using BaseClients;
using CommandLine;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml.Linq;
using FleetClients.Core;

namespace FleetClients.FleetClientConsole.Options
{
	[Verb("getdescription", HelpText = "Gets Kingpin description")]
	public class GetKingpinDescriptionOptions : AbstractConsoleOption<IFleetManagerClient>
	{
		[Option('i', "IPv4String", Required = true, Default = "192.168.0.1", HelpText = "IPv4 Address")]
		public string IPv4String { get; set; }

		protected override ServiceOperationResult HandleExecution(IFleetManagerClient client)
		{
			IPAddress ipAddress = IPAddress.Parse(IPv4String);

			ServiceOperationResult result = client.TryGetKingpinDescription(ipAddress, out XDocument xDocument);

			if (result.IsSuccessfull && xDocument != null)
			{
				string fileName = Path.GetTempPath() + Guid.NewGuid().ToString() + ".xml";
				xDocument.Save(fileName);

				Process.Start(fileName);
			}

			return result;
		}
	}
}
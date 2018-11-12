using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetClients;
using System.Net;
using FleetClients.FleetManagerServiceReference;

namespace FleetClients.DevConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            int tcpPort = 41917;    

            using (IFleetManagerClient client = ClientFactory.CreateTcpFleetManagerClient(ipAddress, tcpPort))
            {
                client.PropertyChanged += Client_PropertyChanged;

                Console.WriteLine("Press <any> key to quit");
                Console.ReadKey(true);
            }             
        }

        private static void HandleFleetState(FleetState fleetState)
        {

        }

        private static void Client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IFleetManagerClient client = sender as IFleetManagerClient;

            switch (e.PropertyName)
            {
                case "FleetState":
                    {
                        HandleFleetState(client.FleetState);
                        break;
                    }                

                default:
                    {
                        return;
                    }
            }
        }
    }
}

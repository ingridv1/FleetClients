using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net;
using FleetClients.FleetManagerServiceReference;
using GACore.Architecture;

namespace FleetClients
{
    public class Postmaster
    {
        private ObservableCollection<Mailbox<IPAddress, IKingpinState>> mailboxes = new ObservableCollection<Mailbox<IPAddress, IKingpinState>>();

        private ReadOnlyObservableCollection<Mailbox<IPAddress, IKingpinState>> readonlyMailboxes;

        public Postmaster()
        {
            readonlyMailboxes = new ReadOnlyObservableCollection<Mailbox<IPAddress, IKingpinState>>(mailboxes);
        }

        public ReadOnlyObservableCollection<Mailbox<IPAddress, IKingpinState>> Mailboxes => readonlyMailboxes;

        public void Process()
        {

        }
    }
}

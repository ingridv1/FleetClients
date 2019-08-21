using FleetClients.FleetManagerServiceReference;
using GACore.Architecture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients
{
    public class KingpinStateMailbox : INotifyPropertyChanged
    {
        private IKingpinState kingpinState;

        private readonly IPAddress ipAddress;

        public IKingpinState KingpinState
        {
            get { return kingpinState; }

            private set
            {
                if (kingpinState != value)
                {
                    kingpinState = value;
                    OnNotifyPropertyChanged();
                }
            }
        }

        public void Update(IKingpinState kingpinState)
        {
            this.KingpinState = kingpinState;
        }

        public IPAddress IPAddress => ipAddress;

        public KingpinStateMailbox(IKingpinState kingpinState)
        {
            if (kingpinState == null) throw new ArgumentNullException("kingpinState");

            if (kingpinState.IPAddress == null) throw new ArgumentNullException("kingpinState.IPAddress");

            this.kingpinState = kingpinState;
            this.ipAddress = kingpinState.IPAddress;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

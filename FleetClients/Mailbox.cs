using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients
{
    public class Mailbox<T,U>
    {
        private readonly T key;

        private readonly U mail;

        public Mailbox(T key, U mail)
        {
            this.key = key;
            this.mail = mail;
        }

        public T Key => key;

        public U Mail => mail;              

    }
}

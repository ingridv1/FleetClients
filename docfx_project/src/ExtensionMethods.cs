using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients
{
    public static class ExtensionMethods
    {
        public static bool IsCurrentByteTickLarger(this byte current, byte other)
        {
            return (current < other && (other - current) > 128) || (current > other && (current - other) < 128);
        }
    }
}

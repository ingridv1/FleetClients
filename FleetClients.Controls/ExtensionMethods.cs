using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetClients.Controls
{
    public static class ExtensionMethods
    {
#warning GACORE 
        public static double RadToDeg(this double value) => (value * 180.0d) / Math.PI;            
    }
}

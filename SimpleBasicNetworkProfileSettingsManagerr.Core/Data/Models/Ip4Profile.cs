using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBasicNetworkProfileSettingsManager.Kernel.Data.Models
{
    public class Ip4Profile
    {
        public string ConnectionName { get; set; }
        public bool Static{ get; set; }
        public string IPAddress { get; set; }
        public string Mask { get; set; }
        public string GateWay { get; set; }
        public string PrimaryDns { get; set; }
        public string SecondaryDns { get; set; }



    }
}

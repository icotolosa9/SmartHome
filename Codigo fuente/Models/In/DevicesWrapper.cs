using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class DevicesWrapper
    {
        [JsonProperty("dispositivos")]
        public List<ImportedDevice> Dispositivos { get; set; }
    }
}

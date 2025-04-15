using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class CreateSmartLampRequest
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public List<string> Photographs { get; set; }

        public SmartLamp ToEntity()
        {
            return new SmartLamp(Name, ModelNumber, Description, Photographs);
        }
    }
}

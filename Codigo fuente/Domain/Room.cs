using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<HomeDevice> Devices { get; set; }
        public Guid HomeId { get; set; } 
        public Home Home { get; set; } 
    }
}

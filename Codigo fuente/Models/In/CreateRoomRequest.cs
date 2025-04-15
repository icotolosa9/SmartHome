using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class CreateRoomRequest
    {
        public string Name { get; set; }
        public Guid HomeId { get; set; }
    }
}

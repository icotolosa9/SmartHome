using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid HomeId { get; set; }
    }
}

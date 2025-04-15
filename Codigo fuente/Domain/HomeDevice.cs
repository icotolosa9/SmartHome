using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class HomeDevice
    {
        public string Name { get; set; }
        public Guid HardwareId { get; set; }
        public Guid HomeId { get; set; }
        public Guid DeviceId { get; set; }
        public bool Connected { get; set; } = true;
        public bool IsOpenOrOn { get; set; } = false;
        public Device Device { get; set; }
        public Room? Room { get; set; }
        public Guid? RoomId { get; set; }
    }
}

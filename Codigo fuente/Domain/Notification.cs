using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Event { get; set; } 
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public Guid HardwareId { get; set; }
        public string DeviceType { get; set; }

        public Notification() { }
        public Notification(string action, Guid hardwareId, string deviceType)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            Event = action;
            HardwareId = hardwareId;
            DeviceType = deviceType;
            IsRead = false;
        }
    }
}

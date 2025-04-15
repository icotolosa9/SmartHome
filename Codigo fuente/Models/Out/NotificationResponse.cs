using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public Guid HardwareId { get; set; }
        public string DeviceType { get; set; }
    }
}

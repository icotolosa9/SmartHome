using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class ListNotificationsRequest
    {
        public string? DeviceType { get; set; } = null;
        public DateTime? CreationDate { get; set; } = null;
        public bool? Read { get; set; } = null;
    }
}
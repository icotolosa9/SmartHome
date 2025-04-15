using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class ListDevicesRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; } = null;
        public string? Model { get; set; } = null;
        public string? Company { get; set; } = null;
        public string? DeviceType { get; set; } = null;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class DeviceDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string MainPicture { get; set; }
        public string Company { get; set; }
        public bool Connected { get; set; }
        public bool? OpenOrOn { get; set; }
        public string? RoomName { get; set; }
        public string DeviceType { get; set; }
        public bool? SupportsMotionDetection { get; set; }
        public bool? SupportsPersonDetection { get; set; }
    }
}

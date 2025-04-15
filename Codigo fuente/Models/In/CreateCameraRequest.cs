using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class CreateCameraRequest
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public bool IndoorUse { get; set; }
        public bool OutdoorUse { get; set; }
        public string Description { get; set; }
        public List<string> Photographs { get; set; }
        public bool MotionDetection { get; set; }
        public bool PersonDetection { get; set; }

        public Camera ToEntity()
        {
            return new Camera(Name, ModelNumber, Description, Photographs, IndoorUse, OutdoorUse, MotionDetection, PersonDetection);
        }
    }
}
    

     
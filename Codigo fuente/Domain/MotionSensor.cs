using System;
using System.Collections.Generic;

namespace Domain
{
    public class MotionSensor : Device
    {
        public MotionSensor(string name, string modelNumber, string description, List<string> photos)
            : base(name, modelNumber, description, photos)
        {
            DeviceType = "Motion Sensor";
        }

        public string DetectMotion()
        {
            return "Se detectó movimiento.";
        }
    }
}

using System;
using System.Collections.Generic;

namespace Domain
{
    public class SmartLamp : Device
    {
        public SmartLamp(string name, string modelNumber, string description, List<string> photos)
            : base(name, modelNumber, description, photos)
        {
            DeviceType = "Smart Lamp";
        }

        public string LightsOn()
        {
            return "Se prendió la lámpara.";
        }

        public string LightsOff()
        {
            return "Se apagó la lámpara.";
        }
    }
}

using System;
using System.Collections.Generic;

namespace Domain
{
    public class WindowSensor : Device
    {
        public bool IsOpen { get; set; }

        public WindowSensor(string name, string modelNumber, string description, List<string> photos)
            : base(name, modelNumber, description, photos)
        {
            IsOpen = false;
            DeviceType = "Window Sensor";
        }

        public string OpenWindow()
        {
            return "La ventana se abrió.";
        }

        public string CloseWindow()
        {
            return "La ventana se cerró.";
        }
    }
}

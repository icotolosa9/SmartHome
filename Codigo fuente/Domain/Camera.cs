using System;
using System.Collections.Generic;

namespace Domain
{
    public class Camera : Device
    {
        public bool ForIndoorUse { get; set; }
        public bool ForOutdoorUse { get; set; }
        public bool SupportsMotionDetection { get; set; }
        public bool SupportsPersonDetection { get; set; }

        public Camera(string name, string modelNumber, string description, List<string> photos,
                      bool forIndoorUse, bool forOutdoorUse, bool supportsMotionDetection, bool supportsPersonDetection)
            : base(name, modelNumber, description, photos)
        {
            ForIndoorUse = forIndoorUse;
            ForOutdoorUse = forOutdoorUse;
            SupportsMotionDetection = supportsMotionDetection;
            SupportsPersonDetection = supportsPersonDetection;
            DeviceType = "Camera";
        }

        public void Validate()
        {
            base.Validate();

            if (!ForIndoorUse && !ForOutdoorUse)
            {
                throw new ArgumentException("El dispositivo debe ser para uso interior, exterior o ambos.");
            }
        }

        public string MotionDetection()
        {
            if (!SupportsMotionDetection)
            {
                throw new InvalidOperationException("El dispositivo no soporta detección de movimiento.");
            }
            else
            {
                return "Se detectó movimiento.";
            }
        }

        public string UnknownPersonDetection()
        {
            if (!SupportsPersonDetection)
            {
                throw new InvalidOperationException("El dispositivo no soporta detección de personas.");
            }
            else
            {
                return "Se detectó una persona desconocida.";
            }
        }
        public string PersonDetection(string email)
        {
            if (!SupportsPersonDetection)
            {
                throw new InvalidOperationException("El dispositivo no soporta detección de personas.");
            }
            else
            {
                return $"Se detectó a la persona con el mail: {email}.";
            }
        }
    }
}

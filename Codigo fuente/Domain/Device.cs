using System;
using System.Collections.Generic;

namespace Domain
{
    public abstract class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public bool IsOnline { get; set; }
        public string DeviceType { get; protected set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public Device(string name, string modelNumber, string description, List<string> photos)
        {
            Id = Guid.NewGuid();
            Name = name;
            ModelNumber = modelNumber;
            Description = description;
            Photos = photos;
            IsOnline = false;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("El nombre del dispositivo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(ModelNumber))
            {
                throw new ArgumentException("El número de modelo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new ArgumentException("La descripción es obligatoria.");
            }

            if (Photos == null || Photos.Count == 0)
            {
                throw new ArgumentException("Debe proporcionar al menos una fotografía del dispositivo.");
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Device device &&
                   (Name == device.Name && ModelNumber == device.ModelNumber);
        }
    }
}
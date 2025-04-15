using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class CreateSmartLampResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public bool IndoorUse { get; set; }
        public bool OutdoorUse { get; set; }
        public string Description { get; set; }
        public List<string> Photographs { get; set; }
        public bool MotionDetection { get; set; }
        public bool PersonDetection { get; set; }
        public Guid CompanyId { get; set; }

        public CreateSmartLampResponse() { }

        public CreateSmartLampResponse(SmartLamp smartLamp)
        {
            Id = smartLamp.Id;
            Name = smartLamp.Name;
            ModelNumber = smartLamp.ModelNumber;
            Description = smartLamp.Description;
            Photographs = smartLamp.Photos;
            CompanyId = smartLamp.CompanyId;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateSmartLampResponse response &&
                   (Name == response.Name && ModelNumber == response.ModelNumber);
        }
    }
}

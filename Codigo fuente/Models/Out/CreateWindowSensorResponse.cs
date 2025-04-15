using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class CreateWindowSensorResponse
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

        public CreateWindowSensorResponse() { }

        public CreateWindowSensorResponse(WindowSensor windowSensor)
        {
            Id = windowSensor.Id;
            Name = windowSensor.Name;
            ModelNumber = windowSensor.ModelNumber;
            Description = windowSensor.Description;
            Photographs = windowSensor.Photos;
            CompanyId = windowSensor.CompanyId;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateWindowSensorResponse response &&
                   (Name == response.Name && ModelNumber == response.ModelNumber);
        }
    }
}

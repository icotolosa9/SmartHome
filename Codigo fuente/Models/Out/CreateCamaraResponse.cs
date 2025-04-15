using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class CreateCameraResponse
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
        public Guid CompanyId {  get; set; }

        public CreateCameraResponse() { }

        public CreateCameraResponse(Camera camera)
        {
            Id = camera.Id;
            Name = camera.Name;
            ModelNumber = camera.ModelNumber;
            IndoorUse = camera.ForIndoorUse;
            OutdoorUse = camera.ForOutdoorUse;
            Description = camera.Description;
            Photographs = camera.Photos;
            MotionDetection = camera.SupportsMotionDetection;
            PersonDetection = camera.SupportsPersonDetection;
            CompanyId = camera.CompanyId;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateCameraResponse response &&
                   (Name == response.Name && ModelNumber == response.ModelNumber);
        }
    }
}

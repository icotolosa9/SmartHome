using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class ImportedDevice
    {
        public Guid Id { get; set; }           
        public string Tipo { get; set; }          
        public string Nombre { get; set; }       
        public string Modelo { get; set; }        
        public List<Photo> Fotos { get; set; }    
        public bool? ForIndoorUse { get; set; }   
        public bool? ForOutdoorUse { get; set; } 
        public bool? MovementDetection { get; set; } 
        public bool? PersonDetection { get; set; }

    }

    public class Photo
    {
        public string Path { get; set; }
        public bool EsPrincipal { get; set; }
    }

}

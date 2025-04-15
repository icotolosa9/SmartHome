using IImporter;
using Models.In;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONImporters
{
    public class JSONImporter : ImporterInterface
    {
        public string GetName()
        {
            return "JSON Importer";
        }

        public List<ImportedDevice> ImportDevice()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Importers", "devices-to-import.json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("El archivo JSON no se encontró en la ruta especificada.");
            }

            string jsonData = File.ReadAllText(path);

            DevicesWrapper devicesWrapper = JsonConvert.DeserializeObject<DevicesWrapper>(jsonData);

            return new List<ImportedDevice>(devicesWrapper?.Dispositivos);
        }
    }
}
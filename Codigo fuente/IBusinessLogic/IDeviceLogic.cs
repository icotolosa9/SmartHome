using Domain;
using IImporter;
using Models.In;
using Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic
{
    public interface IDeviceLogic
    {
        Camera CreateCamera(Camera camera, string emailCompanyOwner);
        WindowSensor CreateWindowSensor(WindowSensor windowSensor, string emailCompanyOwner);
        MotionSensor CreateMotionSensor(MotionSensor motionSensor, string emailCompanyOwner);
        SmartLamp CreateSmartLamp(SmartLamp smartLamp, string emailCompanyOwner);
        PagedResult<DeviceDto> ListDevices(ListDevicesRequest request);
        List<string> GetSupportedDeviceTypes();
        List<ImporterInterface> GetAllImporters();
        void ImportDevices(List<ImportedDevice> importedDevices, string emailCompanyOwner);
    }
}


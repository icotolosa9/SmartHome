using Domain;
using Models.In;
using Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IDeviceRepository
    {
        Device? GetDeviceByNameAndModel(string name, string model);
        Device Save(Device device);
        PagedResult<Device> GetPagedDevices(ListDevicesRequest request);
        List<string> GetSupportedDeviceTypes();
        Device? GetDeviceById(Guid deviceId);
    }
}

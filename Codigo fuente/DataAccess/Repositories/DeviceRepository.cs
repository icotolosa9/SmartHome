using DataAccess.Context;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Models.In;
using Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DeviceRepository: IDeviceRepository
    {
        private readonly SmartHomeContext _smartHomeContext;

        public DeviceRepository(SmartHomeContext smartHomeContext)
        {
            _smartHomeContext = smartHomeContext;
        }

        public Device Save(Device device)
        {
            _smartHomeContext.Devices.Add(device);
            _smartHomeContext.SaveChanges();
            return device;
        }

        public Device? GetDeviceByNameAndModel(string name, string model)
        {
            return _smartHomeContext.Devices.FirstOrDefault(d => d.Name == name && d.ModelNumber == model);
        }

        public PagedResult<Device> GetPagedDevices(ListDevicesRequest request)
        {
            IQueryable<Device> query = _smartHomeContext.Devices
                    .Include(d => d.Company);

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(d => d.Name == request.Name);
            }

            if (!string.IsNullOrEmpty(request.Model))
            {
                query = query.Where(d => d.ModelNumber == request.Model);
            }

            if (!string.IsNullOrEmpty(request.Company))
            {
                query = query.Where(d => d.Company.Name == request.Company);
            }

            if (!string.IsNullOrEmpty(request.DeviceType))
            {
                query = query.Where(d => d.DeviceType == request.DeviceType);
            }

            int totalCount = query.Count();

            List<Device> devices = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<Device>(devices, totalCount, request.PageNumber, request.PageSize);
        }

        public List<string> GetSupportedDeviceTypes()
        {
            return new List<string>
            {
                nameof(Camera), 
                nameof(WindowSensor),
                nameof(SmartLamp),
                nameof(MotionSensor)
            };
        }

        public Device? GetDeviceById(Guid deviceId)
        {
            return _smartHomeContext.Devices.FirstOrDefault(d => d.Id == deviceId);
        }
    }
}

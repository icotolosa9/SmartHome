using DataAccess.Context;
using DataAccess.Migrations;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Models.In;
using Models.Out;

namespace DataAccess.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly SmartHomeContext _smartHomeContext;

        public HomeRepository(SmartHomeContext smartHomeContext)
        {
            _smartHomeContext = smartHomeContext;
        }

        public Home CreateHome(Home home)
        {
            _smartHomeContext.Homes.Add(home);
            _smartHomeContext.SaveChanges();
            return home;
        }

        public bool AddMemberToHome(Home home, AddHomeMemberRequest newHomeMembers)
        {
            foreach (AddHomeMemberDto memberDto in newHomeMembers.Members)
            {
                User? homeOwner = _smartHomeContext.Users.FirstOrDefault(u => u.Email == memberDto.UserEmail);

                if (homeOwner != null && !home.Members.Contains(homeOwner))
                {
                    home.Members.Add(homeOwner);
                    homeOwner.Homes.Add(home);
                    string permissionString = string.Join(", ", memberDto.Permissions);
                    home.MemberPermissions.Add(new HomeOwnerPermission
                    {
                        HomeId = home.Id,
                        HomeOwnerId = homeOwner.Id,
                        Permission = permissionString
                    });
                }
            }
            _smartHomeContext.SaveChanges();
            return true;
        }

        public Home GetHomeById(Guid id)
        {
            Home? result = _smartHomeContext.Homes
                .Include(h => h.Members)
                .Include(h => h.MemberPermissions)
                .FirstOrDefault(h => h.Id == id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public List<HomeMemberDto> GetHomeMembers(Guid homeId)
        {
            List<HomeMemberDto> members = _smartHomeContext.HomeOwnerPermissions
                .Where(hp => hp.HomeId == homeId)
                .Include(hp => hp.HomeOwner)
                .Select(hp => new HomeMemberDto
                {
                    FirstName = hp.HomeOwner.FirstName,
                    LastName = hp.HomeOwner.LastName,
                    Email = hp.HomeOwner.Email,
                    Permissions = _smartHomeContext.HomeOwnerPermissions
                        .Where(p => p.HomeOwnerId == hp.HomeOwnerId && p.HomeId == homeId)
                        .Select(p => p.Permission)
                        .ToList(),
                    IsNotificationsOn = hp.IsNotificationEnabled
                })
                .ToList();
            return members;
        }

        public void AssociateDeviceToHome(Home home, HomeDevice request)
        {
            home.HomeDevices.Add(request);
            _smartHomeContext.HomeDevices.Add(request);
            _smartHomeContext.SaveChanges(); 
        }

        public List<DeviceDto> GetDevicesByHomeId(Guid homeId, string? roomName = null)
        {
            List<HomeDevice> homeDevices = _smartHomeContext.HomeDevices
                .Include(hd => hd.Device).ThenInclude(d => d.Company)
                .Include(hd => hd.Room)
                .Where(hd => hd.HomeId == homeId && (string.IsNullOrEmpty(roomName) || (hd.Room != null && hd.Room.Name == roomName)))
                .ToList(); 

            List<DeviceDto> result = homeDevices.Select(hd => new DeviceDto
            {
                Id = hd.HardwareId,
                Name = hd.Name,
                Model = hd.Device.ModelNumber,
                MainPicture = hd.Device.Photos != null && hd.Device.Photos.Any() ? hd.Device.Photos.FirstOrDefault() : "default.jpg",
                Connected = hd.Connected,
                OpenOrOn = (hd.Device.DeviceType == "Window Sensor" || hd.Device.DeviceType == "Smart Lamp") ? hd.IsOpenOrOn : null,
                RoomName = hd.Room?.Name,
                Company = hd.Device.Company.Name,
                DeviceType = hd.Device.DeviceType,
                SupportsMotionDetection = hd.Device is Camera camera && camera.SupportsMotionDetection,
                SupportsPersonDetection = hd.Device is Camera camera2 && camera2.SupportsPersonDetection
            }).ToList();
            return result;
        }

        public string GetHomeOwnerPermissions(Guid homeId, Guid userId)
        {
            return _smartHomeContext.HomeOwnerPermissions
                .FirstOrDefault(hp => hp.HomeId == homeId && hp.HomeOwnerId == userId)?.Permission ?? "";
        }

        public void Save()
        {
            _smartHomeContext.SaveChanges(); 
        }

        public HomeDevice? GetHomeDeviceById(Guid hardwareId)
        {
            return _smartHomeContext.HomeDevices.FirstOrDefault(hd => hd.HardwareId == hardwareId);
        }

        public void UpdateHomeDevice(HomeDevice homeDevice)
        {
            _smartHomeContext.HomeDevices.Update(homeDevice);
            _smartHomeContext.SaveChanges();
        }

        public void UpdateHomeName(Home home)
        {
            Home existingHome = _smartHomeContext.Homes.Find(home.Id);
            existingHome.Name = home.Name;
            _smartHomeContext.SaveChanges();  
        }

        public void AddRoom(Room room)
        {
            _smartHomeContext.Room.Add(room);
            _smartHomeContext.SaveChanges();
        }

        public Room GetRoomByName(Guid homeId, string roomName)
        {
            return _smartHomeContext.Room.FirstOrDefault(r => r.HomeId == homeId && r.Name == roomName);
        }
        public List<Home> GetHomesByOwner(Guid userId)
        {
            return _smartHomeContext.Homes.Where(h => h.HomeOwnerId == userId).ToList();
        }

    }
}
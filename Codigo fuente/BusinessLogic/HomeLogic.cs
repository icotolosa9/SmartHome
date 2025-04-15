using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Models.In;
using Models.Out;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessLogic
{
    public class HomeLogic : IHomeLogic
    {
        private IHomeRepository _homeRepository;
        private IUserRepository _userRepository;
        private IDeviceRepository _deviceRepository;

        public HomeLogic(IHomeRepository homeRepository, IUserRepository userRepository, IDeviceRepository deviceRepository)
        {
            _homeRepository = homeRepository;
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
        }

        public Home CreateHome(Home home, string ownerEmail)
        {
            User user = _userRepository.GetUserByEmail(ownerEmail);

            bool homeExists = user.Homes.Any(h => h.Name == home.Name);
            if (homeExists)
            {
                throw new DuplicateNameException("Ya existe un hogar con el mismo nombre para este propietario.");
            }
                        
            home.HomeOwnerId = user.Id;
            home.Owner = user;
            
            _homeRepository.CreateHome(home);
            user.Homes.Add(home);
            _userRepository.Update(user);

            return home;
        }

        public bool AddMemberToHome(Guid homeId, AddHomeMemberRequest newHomeMembers, string ownerEmail)
        {

            Home home = _homeRepository.GetHomeById(homeId);

            if (home == null)
            {
                throw new HomeNotFoundException();
            }

            User owner = _userRepository.GetUserByEmail(ownerEmail);
            if (home.HomeOwnerId != owner.Id)
            {
                throw new UnauthorizedOwnerException();
            }

            List<HomeMemberDto> existingMembers = _homeRepository.GetHomeMembers(home.Id);
            var newHomeMembersCount = newHomeMembers.Members.Count;
            if (existingMembers.Count + newHomeMembersCount >= home.Capacity) 
            {
                throw new HomeCapacityExceededException();
            }

            foreach (AddHomeMemberDto memberDto in newHomeMembers.Members)
            {
                User? homeOwner = _userRepository.GetUserByEmail(memberDto.UserEmail);
                if (homeOwner == null)
                {
                    throw new NotFoundException();
                }
                if (existingMembers != null && existingMembers.Any(m => m.Email == homeOwner.Email))
                {
                    throw new MemberAlreadyExistsException($"Usuario {homeOwner.Email} ya pertenece a este hogar.");
                }
            }

            _homeRepository.AddMemberToHome(home, newHomeMembers);
            return true;
        }

        public List<HomeMemberDto> GetHomeMembers(Guid homeId, string ownerEmail)
        {
            User owner = _userRepository.GetUserByEmail(ownerEmail);
            Home home = _homeRepository.GetHomeById(homeId);
            if (home == null)
            {
                throw new HomeNotFoundException();
            }
            if (home.HomeOwnerId != owner.Id)
            {
                throw new UnauthorizedOwnerException();
            }
            return _homeRepository.GetHomeMembers(homeId);
        }

        public Home GetHomeById(Guid homeId)
        {
            return _homeRepository.GetHomeById(homeId);
        }

        public void AssociateDeviceToHome(Guid homeId, AssociateDeviceRequest request, string ownerEmail)
        {
            Home home = _homeRepository.GetHomeById(homeId);
            if (home == null)
            {
                throw new HomeNotFoundException();
            }

            User owner = _userRepository.GetUserByEmail(ownerEmail);

            if (home.HomeOwnerId != owner.Id)
            {
                var memberPermission = home.MemberPermissions.FirstOrDefault(mp => mp.HomeOwnerId == owner.Id);

                if (memberPermission == null || !HasPermission(memberPermission.Permission, "AddDevice"))
                {
                    throw new InvalidOperationException("No tienes permisos para asociar dispositivos al hogar.");
                }
            }

            Device device = _deviceRepository.GetDeviceByNameAndModel(request.DeviceName, request.DeviceModel);
            if (device == null)
            {
                throw new NotFoundDevice(); 
            }

            HomeDevice homeDevice = new HomeDevice
            {
                HomeId = homeId,
                DeviceId = device.Id,
                HardwareId = Guid.NewGuid(),
                Connected = request.Connected,
                Device = device,
                Name = device.Name
            };
            _homeRepository.AssociateDeviceToHome(home, homeDevice);
        }

        public List<DeviceDto> GetHomeDevices(Guid homeId, Guid userId, string? roomName = null)
        {
            Home home = _homeRepository.GetHomeById(homeId);
            if (home.HomeOwnerId != userId)
            {
                string permissions = _homeRepository.GetHomeOwnerPermissions(homeId, userId);
                if (!HasPermission(permissions, "listDevices"))
                {
                    throw new InvalidOperationException("No tienes permiso para ver los dispositivos.");
                }
            }
            return _homeRepository.GetDevicesByHomeId(homeId, roomName);
            
        }

        public bool UpdateMemberNotifications(Guid homeId, Guid ownerId, List<MemberNotificationUpdateDto> updates)
        {
            bool result = true;
            Home home = _homeRepository.GetHomeById(homeId); 

            if (home == null || home.HomeOwnerId != ownerId)
            {
                result = false;  
            }

            foreach (MemberNotificationUpdateDto update in updates)
            {
                User homeOwner = _userRepository.GetUserByEmail(update.HomeOwnerEmail);
                HomeOwnerPermission memberPermission = home.MemberPermissions
                    .FirstOrDefault(mp => mp.HomeOwnerId == homeOwner.Id);

                if (memberPermission != null)
                {
                    memberPermission.IsNotificationEnabled = update.IsNotificationEnabled;
                }
            }

            _homeRepository.Save(); 
            return result;
        }

        public void SetStatusHomeDevice(Guid hardwareId, bool status, Guid userId)
        {
            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }
            User owner = _userRepository.GetUserById(userId);

            if (!owner.Homes.Any(h => h.Id == homeDevice.HomeId))
            {
                throw new UnauthorizedOwnerException();
            }

            homeDevice.Connected = status;
            _homeRepository.UpdateHomeDevice(homeDevice);
        }

        public void UpdateHomeName(Guid homeId, UpdateHomeNameRequest request, string ownerEmail)
        {
            Home home = _homeRepository.GetHomeById(homeId);
            if (home == null)
            {
                throw new HomeNotFoundException();
            }

            User owner = _userRepository.GetUserByEmail(ownerEmail);
            if (home.HomeOwnerId != owner.Id)
            {
                throw new UnauthorizedOwnerException();
            }

            bool duplicateNameExists = owner.Homes.Any(h => h.Name == request.NewName && h.Id != home.Id);
            if (duplicateNameExists)
            {
                throw new InvalidOperationException("Ya existe un hogar con el mismo nombre para este propietario.");
            }

            home.Name = request.NewName;
            _homeRepository.UpdateHomeName(home);
        }

        public RoomDto CreateRoom(CreateRoomRequest request, string ownerEmail)
        {
            Home home = _homeRepository.GetHomeById(request.HomeId);
            if (home == null)
            {
                throw new HomeNotFoundException();
            }

            User owner = _userRepository.GetUserByEmail(ownerEmail);
            if (home.HomeOwnerId != owner.Id)
            {
                throw new UnauthorizedOwnerException();
            }

            if (_homeRepository.GetRoomByName(request.HomeId, request.Name) != null)
            {
                throw new DuplicateNameException("Ya existe un cuarto con este nombre en el hogar.");
            }

            Room room = new Room
            {
                Name = request.Name,
                HomeId = request.HomeId,
                Home = home
            };

            _homeRepository.AddRoom(room);
            return new RoomDto { Id = room.Id, Name = room.Name, HomeId = room.HomeId };
        }

        public void AssignDeviceToRoom(Guid homeId, AssignDeviceToRoomRequest request, string userEmail)
        {
            Home home = _homeRepository.GetHomeById(homeId);
            if (home == null)
            {
                throw new HomeNotFoundException();
            }

            User user = _userRepository.GetUserByEmail(userEmail);

            if (home.HomeOwnerId != user.Id)
            {
                var memberPermission = home.MemberPermissions.FirstOrDefault(mp => mp.HomeOwnerId == user.Id);

                if (memberPermission == null || !HasPermission(memberPermission.Permission, "AssignDevice"))
                {
                    throw new InvalidOperationException("No tienes permisos para asignar este dispositivo a un cuarto.");
                }
            }

            Room room = _homeRepository.GetRoomByName(homeId, request.RoomName);
            if (room == null)
            {
                throw new RoomNotFoundException(); 
            }

            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(request.HardwareId);
            if (homeDevice == null || homeDevice.HomeId != homeId)
            {
                throw new NotFoundDevice();
            }

            homeDevice.RoomId = room.Id;
            _homeRepository.UpdateHomeDevice(homeDevice);
        }

        public bool HasPermission(string permissions, string requiredPermission)
        {
            var permissionList = permissions.Split(',').Select(p => p.Trim()).ToList();
            return permissionList.Contains(requiredPermission);
        }

        public void UpdateDeviceName(Guid hardwareId, string newName, string userEmail)
        {
            var homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }

            Home home = _homeRepository.GetHomeById(homeDevice.HomeId);
            if (home == null)
            {
                throw new HomeNotFoundException();
            }

            var user = _userRepository.GetUserByEmail(userEmail);

            if (home.HomeOwnerId != user.Id)
            {
                var memberPermission = home.MemberPermissions.FirstOrDefault(mp => mp.HomeOwnerId == user.Id);
                if (memberPermission == null || !HasPermission(memberPermission.Permission, "RenameDevice"))
                {
                    throw new InvalidOperationException("No tienes permisos para renombrar este dispositivo.");
                }
            }

            homeDevice.Name = newName;
            _homeRepository.UpdateHomeDevice(homeDevice);
        }

        public List<HomeDto> GetHomesByUser(Guid userId)
        {
            return _userRepository.GetHomesByUser(userId);
        }

        public List<HomeDto> GetHomesByOwner(Guid userId)
        {
            var homes = _homeRepository.GetHomesByOwner(userId); 

            return homes.Select(home => new HomeDto
            {
                Id = home.Id,
                Name = home.Name,
                Address = home.Address,
                Capacity = home.Capacity
            }).ToList(); 
        }

    }
}

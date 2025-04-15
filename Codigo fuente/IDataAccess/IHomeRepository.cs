using Domain;
using Models.In;
using Models.Out;

namespace IDataAccess
{
    public interface IHomeRepository
    {
        Home CreateHome(Home home);
        bool AddMemberToHome(Home home, AddHomeMemberRequest newHomeMember);
        Home GetHomeById(Guid homeId);
        List<HomeMemberDto> GetHomeMembers(Guid homeId);
        void AssociateDeviceToHome(Home home, HomeDevice request);
        public List<DeviceDto> GetDevicesByHomeId(Guid homeId, string? roomName = null);
        string GetHomeOwnerPermissions(Guid homeId, Guid userId);
        void Save();
        HomeDevice GetHomeDeviceById(Guid hardwareId);
        void UpdateHomeDevice(HomeDevice homeDevice);
        void UpdateHomeName(Home home);
        void AddRoom(Room room);
        Room GetRoomByName(Guid homeId, string roomName);
        List<Home> GetHomesByOwner(Guid userId);
    }
}
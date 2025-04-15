using Domain;
using Models.In;
using Models.Out;

namespace IBusinessLogic
{
    public interface IHomeLogic
    {
        Home CreateHome(Home home, string emailOwner);
        bool AddMemberToHome(Guid homeId, AddHomeMemberRequest addHomeMemberRequest, string emailOwner);
        List<HomeMemberDto> GetHomeMembers(Guid homeId, string emailOwner);
        Home GetHomeById(Guid homeId);
        public List<DeviceDto> GetHomeDevices(Guid homeId, Guid userId, string? roomName = null);
        void AssociateDeviceToHome(Guid homeId, AssociateDeviceRequest request, string emailOwner);
        bool UpdateMemberNotifications(Guid homeId, Guid ownerId, List<MemberNotificationUpdateDto> updates);
        void SetStatusHomeDevice(Guid hardwareId, bool status, Guid userId);
        void UpdateHomeName(Guid homeId, UpdateHomeNameRequest request, string ownerEmail);
        RoomDto CreateRoom(CreateRoomRequest request, string ownerEmail);
        void AssignDeviceToRoom(Guid homeId, AssignDeviceToRoomRequest request, string email);
        void UpdateDeviceName(Guid hardwareId, string newName, string email);
        List<HomeDto> GetHomesByUser(Guid userId);
        List<HomeDto> GetHomesByOwner(Guid userId);
    }
}

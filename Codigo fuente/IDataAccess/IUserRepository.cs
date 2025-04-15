using Domain;
using Models.In;
using Models.Out;

namespace IDataAccess
{
    public interface IUserRepository
    {
        User Save(User user);
        List<User> GetAll();
        User? GetUserById(Guid Id);
        User? GetUserByEmail(string email);
        User Delete(Guid id);
        PagedResult<User> GetPagedUsers(ListAccountsRequest request);
        User Update(User user);
        List<Notification> GetNotificationsByUser(string email);
        List<HomeDto> GetHomesByUser(Guid userId);
        void SaveNotification(User member, Notification notification);
        void MarkNotificationAsRead(Guid notificationId, string email);
    }
}

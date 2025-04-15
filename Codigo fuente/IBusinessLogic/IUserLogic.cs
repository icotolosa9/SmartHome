using Domain;
using Models.In;
using Models.Out;

namespace IBusinessLogic
{
    public interface IUserLogic
    {
        Admin CreateAdmin(Admin admin);
        CompanyOwner CreateCompanyOwner(CompanyOwner companyOwner);
        HomeOwner CreateHomeOwner(HomeOwner homeOwner);
        bool DeleteAdmin(Guid id);
        PagedResult<UserDto> ListAccounts(ListAccountsRequest request);
        User GetCurrentUser(Guid token);
        Guid CreateSession(Session session);
        User AuthenticateUser(string email, string password);
        List<NotificationResponse> GetNotificationsByUser(ListNotificationsRequest request, string email);
        void MarkNotificationAsRead(Guid notificationId, string email);
    }
}

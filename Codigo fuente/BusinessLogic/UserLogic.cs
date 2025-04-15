using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Models.In;
using Models.Out;

namespace BusinessLogic
{
    public class UserLogic: IUserLogic
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        public UserLogic(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository; 
        }

        public Admin CreateAdmin(Admin admin) 
        {
            if (_userRepository.GetUserByEmail(admin.Email) != null)
            {
                throw new UserAlreadyExistsException();
            }
            _userRepository.Save(admin);
            return admin;
        }

        public CompanyOwner CreateCompanyOwner(CompanyOwner companyOwner)
        {
            if (_userRepository.GetUserByEmail(companyOwner.Email) != null)
            {
                throw new UserAlreadyExistsException();
            }
            _userRepository.Save(companyOwner);
            return companyOwner;
        }

        public HomeOwner CreateHomeOwner(HomeOwner homeOwner)
        {
            if (_userRepository.GetUserByEmail(homeOwner.Email) != null)
            {
                throw new UserAlreadyExistsException();
            }
            _userRepository.Save(homeOwner);
            return homeOwner;
        }

        public bool DeleteAdmin(Guid id)
        {
            User deletedUser = _userRepository.Delete(id);

            if (deletedUser == null)
            {
                throw new AdminNotExistException();
            }

            if (deletedUser.Homes.Any())
            {
                throw new AdminWithHomesException();
            }

            return true; 
        }
        public PagedResult<UserDto> ListAccounts(ListAccountsRequest request)
        {
            PagedResult<User> pagedUsers = _userRepository.GetPagedUsers(request);

            List<UserDto> userDtos = pagedUsers.Results.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role,
                CreationDate = u.CreationDate
            }).ToList();

            return new PagedResult<UserDto>(
                userDtos,
                pagedUsers.TotalCount,
                pagedUsers.PageNumber,
                pagedUsers.PageSize
            );
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = _userRepository.GetUserByEmail(email);
            if (user != null && user.Password == password)
            {
                return user;
            }
            throw new NotFoundException();
        }

        public Guid CreateSession(Session session)
        {
            _sessionRepository.Save(session);
            return session.Token; 
        }

        public User GetCurrentUser(Guid token)
        {
            var session = _sessionRepository.GetByToken(token);
            if (session == null)
            {
                return null;
            }
            return _userRepository.GetUserByEmail(session.UserEmail);
        }

        public List<NotificationResponse> GetNotificationsByUser(ListNotificationsRequest request, string email)
        {
            var notifications = _userRepository.GetNotificationsByUser(email);

            if (!string.IsNullOrEmpty(request.DeviceType))
            {
                notifications = notifications.Where(n => n.DeviceType == request.DeviceType).ToList();
            }

            if (request.CreationDate.HasValue)
            {
                notifications = notifications.Where(n => n.Date.Date == request.CreationDate.Value.Date).ToList();
            }

            if (request.Read.HasValue)
            {
                notifications = notifications.Where(n => n.IsRead == request.Read.Value).ToList();
            }

            return notifications.Select(n => new NotificationResponse
            {
                Id = n.Id,
                Event = n.Event,
                Date = n.Date,
                IsRead = n.IsRead,
                HardwareId = n.HardwareId,
                DeviceType = n.DeviceType
            }).ToList();
        }
        
        public void MarkNotificationAsRead(Guid notificationId, string email)
        {
            _userRepository.MarkNotificationAsRead(notificationId, email);
        }
    }
}

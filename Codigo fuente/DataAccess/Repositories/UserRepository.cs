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
    public class UserRepository: IUserRepository
    {
        private readonly SmartHomeContext _smartHomeContext;

        public UserRepository(SmartHomeContext smartHomeContext)
        {
            _smartHomeContext = smartHomeContext;
        }

        public User Save(User user)
        {
            _smartHomeContext.Users.Add(user);
            _smartHomeContext.SaveChanges();
            return user;
        }

        public User? GetUserById(Guid Id)
        {
            return _smartHomeContext.Users.FirstOrDefault(user => user.Id == Id);
        }

        public User? GetUserByEmail(string email)
        {
            return _smartHomeContext.Users
                .Include(h => h.Homes)
                .Include(n => n.Notifications)
                .FirstOrDefault(user => user.Email == email);
        }

        public List<User> GetAll()
        {
            return _smartHomeContext.Users.ToList();
        }

        public User? Delete(Guid id)
        {
            var userToDelete = _smartHomeContext.Users.SingleOrDefault(u => u.Id == id);

            if (userToDelete != null)
            {
                _smartHomeContext.Users.Remove(userToDelete);
                _smartHomeContext.SaveChanges();
            }

            return userToDelete;
        }

        public PagedResult<User> GetPagedUsers(ListAccountsRequest request)
        {
            IQueryable<User> query = _smartHomeContext.Users;

            if (!string.IsNullOrEmpty(request.Role))
            {
                query = query.Where(u => u.Role == request.Role);
            }

            if (!string.IsNullOrEmpty(request.FullName))
            {
                var fullName = request.FullName.ToLower();
                query = query.Where(u => (u.FirstName + " " + u.LastName).ToLower().Contains(fullName));
            }

            int totalCount = query.Count();

            List<User> users = query
                .Skip((request.PageNumber - 1) * request.PageSize) 
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<User>(users, totalCount, request.PageNumber, request.PageSize);
        }

        public User Update(User user)
        {
            _smartHomeContext.Users.Update(user);
            _smartHomeContext.SaveChanges();
            return user;
        }
        public List<Notification> GetNotificationsByUser(string email)
        {
            {
                var user = _smartHomeContext.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return new List<Notification>(); 
                }

                return user.Notifications.OrderByDescending(n => n.Date).ToList(); 
            }
        }

        public List<HomeDto> GetHomesByUser(Guid userId)
        {
            User user = _smartHomeContext.Users
                .Include(u => u.Homes)
                .First(u => u.Id == userId);

            return user.Homes.Select(h => new HomeDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                Capacity = h.Capacity
            }).ToList();
        }

        public void SaveNotification(User member, Notification notification)
        {
            _smartHomeContext.Notifications.Add(notification);
            _smartHomeContext.Users.Update(member); //necesario??
            _smartHomeContext.SaveChanges();
        }

        public void MarkNotificationAsRead(Guid notificationId, string email)
        {
            
            var notification = _smartHomeContext.Notifications.FirstOrDefault(n => n.Id == notificationId);

            notification.IsRead = true;

            _smartHomeContext.SaveChanges();  
        }
    }
}

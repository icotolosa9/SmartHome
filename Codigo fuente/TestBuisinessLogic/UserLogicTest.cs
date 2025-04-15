using BusinessLogic;
using Domain;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Models.In;
using Models.Out;
using Moq;

namespace TestBuisinessLogic
{
    [TestClass]
    public class UserLogicTest
    {
        [TestMethod]
        public void CreateAdmin_ReturnsCreatedAdmin()
        {
            // Arrange
            Admin admin = new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!");
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetUserByEmail(admin.Email)).Returns((User)null); 
            userRepositoryMock.Setup(repo => repo.Save(admin)).Returns(admin);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            User createdUser = userLogic.CreateAdmin(admin);

            // Assert
            Assert.AreEqual(admin, createdUser);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAlreadyExistsException))]
        public void CreateAdmin_ExistingEmail_Exception()
        {
            // Arrange
            Admin admin = new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!");
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetUserByEmail(admin.Email)).Returns(admin);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            userLogic.CreateAdmin(admin);
        }

        [TestMethod]
        public void CreateCompanyOwner_ReturnsCreatedCompanyOwner()
        {
            // Arrange
            CompanyOwner companyOwner = new CompanyOwner("Juan", "Perez", "juan.perez@gmail.com", "password123!");
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetUserByEmail(companyOwner.Email)).Returns((User)null);
            userRepositoryMock.Setup(repo => repo.Save(companyOwner)).Returns(companyOwner);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            User createdUser = userLogic.CreateCompanyOwner(companyOwner);

            // Assert
            Assert.AreEqual(companyOwner, createdUser);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAlreadyExistsException))]
        public void CreateCompanyOwner_ExistingEmail_Exception()
        {
            // Arrange
            CompanyOwner companyOwner = new CompanyOwner("Juan", "Perez", "juan.perez@gmail.com", "password123!");
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetUserByEmail(companyOwner.Email)).Returns(companyOwner);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            userLogic.CreateCompanyOwner(companyOwner);
        }

        [TestMethod]
        public void DeleteAdmin_AdminExists_ReturnsTrue()
        {
            // Arrange
            Guid adminId = Guid.NewGuid();
            User adminUser = new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!") { Id = adminId };
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.Delete(adminId)).Returns(adminUser);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            bool result = userLogic.DeleteAdmin(adminId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(AdminNotExistException))]
        public void DeleteAdmin_AdminDoesNotExist_ThrowsException()
        {
            // Arrange
            Guid adminId = Guid.NewGuid();

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.Delete(adminId)).Returns((User)null);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            userLogic.DeleteAdmin(adminId);
        }


        [TestMethod]
        public void ListAccounts_ReturnsPagedResult()
        {
            // Arrange
            List<User> users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            };

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetPagedUsers(It.IsAny<ListAccountsRequest>()))
                              .Returns(new PagedResult<User>(users, users.Count, 1, users.Count));

            ListAccountsRequest request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 2
            };

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            PagedResult<UserDto> result = userLogic.ListAccounts(request);

            // Assert
            Assert.AreEqual(2, result.Results.Count());
        }

        [TestMethod]
        public void ListAccounts_WithRoleFilter_ReturnsFilteredResults()
        {
            // Arrange
            List<User> users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            };

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetPagedUsers(It.IsAny<ListAccountsRequest>()))
                              .Returns(new PagedResult<User>(new List<User> { users[0] }, 1, 1, 1));

            ListAccountsRequest request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Role = "admin"
            };

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            PagedResult<UserDto> result = userLogic.ListAccounts(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
        }

        [TestMethod]
        public void ListAccounts_WithFullNameFilter_ReturnsFilteredResults()
        {
            // Arrange
            List<User> users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            };

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetPagedUsers(It.IsAny<ListAccountsRequest>()))
                              .Returns(new PagedResult<User>(new List<User> { users[0] }, 1, 1, 1));

            ListAccountsRequest request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 10,
                FullName = "Juan Perez"
            };

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            PagedResult<UserDto> result = userLogic.ListAccounts(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
        }

        [TestMethod]
        public void ListAccounts_WithNoResults_ReturnsEmptyPagedResult()
        {
            // Arrange
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetPagedUsers(It.IsAny<ListAccountsRequest>()))
                              .Returns(new PagedResult<User>(new List<User>(), 0, 1, 10));

            ListAccountsRequest request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            PagedResult<UserDto> result = userLogic.ListAccounts(request);

            // Assert
            Assert.AreEqual(0, result.Results.Count());
        }

        [TestMethod]
        public void AuthenticateUser_ReturnsAuthenticatedUser()
        {
            // Arrange
            string email = "juan.perez@gmail.com";
            string password = "password123!";
            Admin admin = new Admin("Juan", "Perez", email, password);

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).Returns(admin);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            User authenticatedUser = userLogic.AuthenticateUser(email, password);

            // Assert
            Assert.AreEqual(admin, authenticatedUser);
        }

        [TestMethod]
        public void GetCurrentUser_ReturnsUserFromSession()
        {
            // Arrange
            Guid token = Guid.NewGuid();
            string email = "juan.perez@gmail.com";
            Admin admin = new Admin("Juan", "Perez", email, "password123!");
            Session session = new Session { Token = token, UserEmail = email };

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            Mock<ISessionRepository> sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            sessionRepositoryMock.Setup(repo => repo.GetByToken(token)).Returns(session);
            userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).Returns(admin);

            UserLogic userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            User currentUser = userLogic.GetCurrentUser(token);

            // Assert
            Assert.AreEqual(admin, currentUser);
        }

        [TestMethod]
        public void GetNotificationsByUser_ReturnsFilteredNotifications()
        {
            // Arrange
            var hardwareId1 = Guid.NewGuid();
            var hardwareId2 = Guid.NewGuid();
            var hardwareId3 = Guid.NewGuid();
            var email = "juan.perez@gmail.com";
            var request = new ListNotificationsRequest
            {
                DeviceType = "Window Sensor", 
                CreationDate = new DateTime(2024, 10, 29),
                Read = false
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Event = "Event1",
                    Date = new DateTime(2024, 10, 29),
                    IsRead = false,
                    HardwareId = hardwareId1,
                    DeviceType = "Window Sensor" 
                },
                new Notification
                {
                    Event = "Event2",
                    Date = new DateTime(2024, 10, 30),
                    IsRead = true,
                    HardwareId = hardwareId2,
                    DeviceType = "Window Sensor" 
                },
                new Notification
                {
                    Event = "Event3",
                    Date = new DateTime(2024, 10, 29),
                    IsRead = false,
                    HardwareId = hardwareId3,
                    DeviceType = "Window Sensor" 
                }
            };

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            var sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetNotificationsByUser(email)).Returns(notifications);

            var userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            var result = userLogic.GetNotificationsByUser(request, email);

            // Assert
            Assert.AreEqual(2, result.Count); 
            Assert.IsTrue(result.All(n => n.DeviceType == request.DeviceType)); 
            Assert.IsTrue(result.All(n => n.Date.Date == request.CreationDate.Value.Date)); 
            Assert.IsTrue(result.All(n => n.IsRead == request.Read.Value)); 
        }

        [TestMethod]
        public void GetNotificationsByUser_NoMatchingNotifications_ReturnsEmptyList()
        {
            // Arrange
            var email = "juan.perez@gmail.com";
            var request = new ListNotificationsRequest
            {
                DeviceType = "Window Sensor",
                CreationDate = new DateTime(2024, 10, 30),
                Read = true
            };

            var notifications = new List<Notification>(); 

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            var sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

            userRepositoryMock.Setup(repo => repo.GetNotificationsByUser(email)).Returns(notifications);

            var userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            var result = userLogic.GetNotificationsByUser(request, email);

            // Assert
            Assert.AreEqual(0, result.Count); 
        }

        [TestMethod]
        public void MarkNotificationAsRead_Ok()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var email = "juan.perez@gmail.com";
            var user = new HomeOwner("Juan", "Perez", email, "password123!", "photo") { Id = Guid.NewGuid() };

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            var sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(ur => ur.MarkNotificationAsRead(notificationId, email));  

            var userLogic = new UserLogic(userRepositoryMock.Object, sessionRepositoryMock.Object);

            // Act
            userLogic.MarkNotificationAsRead(notificationId, email);

            // Assert
            userRepositoryMock.VerifyAll();  
        }
    }
}
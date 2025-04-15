using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using IDataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.In;
using Models.Out;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataAccess
{
    [TestClass]
    public class UserRepositoryTest
    {
        private SqliteConnection _connection;
        private SmartHomeContext _context;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var contextOptions = new DbContextOptionsBuilder<SmartHomeContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new SmartHomeContext(contextOptions);
            _context.Database.EnsureCreated();

            _userRepository = new UserRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _connection.Close();
        }

        [TestMethod]
        public void SaveUserMockTest()
        {
            // Arrange
            Admin expectedUser = new Admin("Hector", "Gomez", "hector@gmail.com", "password123!");
            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            userRepository.Save(expectedUser);

            // Assert
            mockDbSet.Verify(dbSet => dbSet.Add(It.IsAny<User>()), Times.Once);
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetAllUsersMockTest()
        {
            // Arrange
            var users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            }.AsQueryable();

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            List<User> result = userRepository.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Juan", result[0].FirstName);
        }

        [TestMethod]
        public void DeleteAdminMockTest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToDelete = new Admin("Hector", "Gomez", "hector@gmail.com", "password123!") { Id = userId };
            var users = new List<User> { userToDelete }.AsQueryable();

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            User result = userRepository.Delete(userId);

            // Assert
            mockDbSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Once);
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
            Assert.AreEqual(userToDelete, result);
        }

        [TestMethod]
        public void GetPagedUsersMockTest()
        {
            // Arrange
            var users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            }.AsQueryable();

            var request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 1,
                Role = "admin",
                FullName = null
            };

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            PagedResult<User> result = userRepository.GetPagedUsers(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual("Juan", result.Results.First().FirstName);
        }

        [TestMethod]
        public void GetPagedUsers_WithRoleFilter_ReturnsFilteredResults()
        {
            // Arrange
            var users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            }.AsQueryable();

            var request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 1,
                Role = "admin"
            };

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            PagedResult<User> result = userRepository.GetPagedUsers(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual("Juan", result.Results.First().FirstName);
        }

        [TestMethod]
        public void GetPagedUsers_WithFullNameFilter_ReturnsFilteredResults()
        {
            // Arrange
            var users = new List<User>
            {
                new Admin("Juan", "Perez", "juan.perez@gmail.com", "password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "password123!")
            }.AsQueryable();

            var request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 1,
                FullName = "Juan Perez"
            };

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            PagedResult<User> result = userRepository.GetPagedUsers(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual("Juan", result.Results.First().FirstName);
        }

        [TestMethod]
        public void GetPagedUsers_WithNoResults_ReturnsEmptyPagedResult()
        {
            // Arrange
            var users = new List<User>().AsQueryable();

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            var request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            PagedResult<User> result = userRepository.GetPagedUsers(request);

            // Assert
            Assert.AreEqual(0, result.Results.Count());
            Assert.AreEqual(0, result.TotalCount);
        }

        [TestMethod]
        public void GetUserByEmail_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToFind = new Admin("Juan", "Perez", "juan@gmail.com", "Password123!") { Id = userId };

            var users = new List<User>
            {
                userToFind,
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "Password123!")
            }.AsQueryable();

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            User result = userRepository.GetUserByEmail("juan@gmail.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userToFind, result);
        }

        [TestMethod]
        public void GetUserByEmail_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var users = new List<User>
            {
                new Admin("Juan", "Perez", "juan@gmail.com", "Password123!"),
                new CompanyOwner("Maria", "Garcia", "maria.garcia@gmail.com", "Password123!")
            }.AsQueryable();

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            User result = userRepository.GetUserByEmail("notfound@gmail.com");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateUserMockTest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToUpdate = new Admin("Hector", "Gomez", "hector@gmail.com", "password123!") { Id = userId };
            var updatedUser = new Admin("Hector", "Gomez", "hector.updated@gmail.com", "newpassword123!") { Id = userId };

            var users = new List<User> { userToUpdate }.AsQueryable();

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(ctx => ctx.Users).Returns(mockDbSet.Object);

            IUserRepository userRepository = new UserRepository(mockContext.Object);

            // Act
            User result = userRepository.Update(updatedUser);

            // Assert
            mockDbSet.Verify(dbSet => dbSet.Update(It.IsAny<User>()), Times.Once);
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
            Assert.AreEqual(updatedUser.Email, result.Email);
            Assert.AreEqual(updatedUser.FirstName, result.FirstName);
            Assert.AreEqual(updatedUser.LastName, result.LastName);
        }


        [TestMethod]
        public void GetNotificationsByUser_UserExistsWithNotifications_ReturnsNotifications()
        {
            // Arrange
            var userEmail = "juan.perez@gmail.com";
            var homeOwner = new HomeOwner("Juan", "Perez", userEmail, "password123!", "photo")
            {
                Notifications = new List<Notification>
                {
                    new Notification("Device Online", Guid.NewGuid(), "Sensor"),
                    new Notification("Device Offline", Guid.NewGuid(), "Camera")
                }
            };

            var users = new List<User> { homeOwner }.AsQueryable(); 

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var mockContext = new Mock<SmartHomeContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetNotificationsByUser(userEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(n => n.IsRead == false));
        }

        [TestMethod]
        public void GetNotificationsByUser_ShouldReturnEmptyList_WhenUserDoesNotExist()
        {
            // Arrange
            string email = "nonexistent@example.com";

            var mockSet = new Mock<DbSet<User>>(); 
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new List<User>().AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(new List<User>().AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(new List<User>().AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(new List<User>().GetEnumerator());


            var mockContext = new Mock<SmartHomeContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object); 

            // Act
            var result = _userRepository.GetNotificationsByUser(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetHomesByUser_ValidUserId_ReturnsHomesSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var homes = new List<Home>
            {
                new Home { Id = Guid.NewGuid(), Name = "Home 1", Address = "Address 1", Capacity = 4 },
                new Home { Id = Guid.NewGuid(), Name = "Home 2", Address = "Address 2", Capacity = 3 }
            };

            var homeOwner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "photo")
            {
                Id = userId,
                Homes = homes
            };

            var mockContext = new Mock<SmartHomeContext>();

            mockContext.Setup(c => c.Users).ReturnsDbSet(new List<HomeOwner> { homeOwner });

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetHomesByUser(userId);

            // Assert
            Assert.AreEqual(2, result.Count); 
            Assert.AreEqual("Home 1", result[0].Name);
            Assert.AreEqual("Home 2", result[1].Name);
        }

        [TestMethod]
        public void MarkNotificationAsRead_ValidId_NotificationMarkedAsRead()
        {
            {
                // Arrange
                var notificationId = Guid.NewGuid();
                var email = "test@example.com";
                var mockContext = new Mock<SmartHomeContext>();
                var mockSet = new Mock<DbSet<Notification>>();

                var notification = new Notification { Id = notificationId, IsRead = false };
                var data = new List<Notification> { notification }.AsQueryable();

                mockSet.As<IQueryable<Notification>>().Setup(m => m.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<Notification>>().Setup(m => m.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<Notification>>().Setup(m => m.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<Notification>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

                mockContext.Setup(c => c.Notifications).Returns(mockSet.Object);

                var userRepository = new UserRepository(mockContext.Object);

                // Act
                userRepository.MarkNotificationAsRead(notificationId, email);

                // Assert
                Assert.IsTrue(notification.IsRead);
                mockContext.Verify(m => m.SaveChanges(), Times.Once);
            }
        }
    }
}

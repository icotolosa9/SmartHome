using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using IDataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Models.In;
using Models.Out;
using Moq;

namespace TestDataAccess
{
    [TestClass]
    public class HomeRepositoryTest
    {
        private SqliteConnection _connection;
        private SmartHomeContext _context;
        private HomeRepository _homeRepository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            DbContextOptions<SmartHomeContext> contextOptions = new DbContextOptionsBuilder<SmartHomeContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new SmartHomeContext(contextOptions);
            _context.Database.EnsureCreated();

            _homeRepository = new HomeRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _connection.Close();
        }

        [TestMethod]
        public void CreateHomeOkTest()
        {
            Home expectedHome = new Home("My Home", "Address", "(1,1)", 5);

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            Mock<DbSet<Home>> mockDbSet = new Mock<DbSet<Home>>();
            mockContext.Setup(ctx => ctx.Homes).Returns(mockDbSet.Object);

            IHomeRepository homeRepository = new HomeRepository(mockContext.Object);

            homeRepository.CreateHome(expectedHome);

            mockDbSet.Verify(dbSet => dbSet.Add(It.IsAny<Home>()), Times.Once);
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddMemberToHomeOkTest()
        {
            HomeOwner owner = new HomeOwner("John", "Doe", "owner@mail.com", "password123!", "photo.com");
            Home home = new Home
            {
                Address = "123 Main St",
                Location = "(1,1)",
                Capacity = 5,
                HomeOwnerId = owner.Id,
                Owner = owner
            };

            _context.Homes.Add(home);
            _context.SaveChanges();

            HomeOwner newMember = new HomeOwner("John", "Doe", "member@mail.com", "password123!", "photo.com");

            _context.Users.Add(newMember);
            _context.SaveChanges(); 

            AddHomeMemberRequest newHomeMemberRequest = new AddHomeMemberRequest
            {
                Members = new List<AddHomeMemberDto>
                {
                    new AddHomeMemberDto { UserEmail = "member@mail.com", Permissions = new List<string> { "Admin", "User" } }
                }
            };

            bool result = _homeRepository.AddMemberToHome(home, newHomeMemberRequest);

            Home updatedHome = _context.Homes.Include(h => h.Members).FirstOrDefault(h => h.Id == home.Id);
            Assert.AreEqual("member@mail.com", updatedHome.Members.First().Email); 
        }

        [TestMethod]
        public void AddMemberToHome_ReturnsFalse_WhenCapacityExceeded()
        {
            Home home = new Home("My Home", "Address", "(1,1)", 2);
            home.Members.Add(new HomeOwner("Member1", "LastName", "member1@mail.com", "password!", "photoUrl"));
            home.Members.Add(new HomeOwner("Member2", "LastName", "member2@mail.com", "password!", "photoUrl"));

            Mock<IHomeRepository> repositoryMock = new Mock<IHomeRepository>();
            repositoryMock.Setup(repo => repo.GetHomeById(home.Id)).Returns(home);

            AddHomeMemberRequest addMemberRequest = new AddHomeMemberRequest
            {
                Members = new List<AddHomeMemberDto>
                {
                    new AddHomeMemberDto { UserEmail = "new@mail.com", Permissions = new List<string> { "User" } }
                }
            };

            bool result = repositoryMock.Object.AddMemberToHome(home, addMemberRequest);
            Assert.IsFalse(result, "Debería devolver falso porque se ha excedido la capacidad del hogar.");
        }

        [TestMethod]
        public void GetHomeMembers_ReturnsCorrectMembers()
        {
            HomeOwner homeOwner = new HomeOwner("John", "Doe", "owner@mail.com", "password123!", "photo.com");
            Home home = new Home
            {
                Address = "123 Main St",
                Location = "(1,1)",
                Capacity = 5,
                HomeOwnerId = homeOwner.Id,
                Owner = homeOwner
            };
            HomeOwnerPermission permission = new HomeOwnerPermission
            {
                HomeId = home.Id,
                HomeOwnerId = homeOwner.Id,
                Permission = "Admin",
                HomeOwner = homeOwner,
                Home = home
            };

            _context.Users.Add(homeOwner);
            _context.Homes.Add(home);
            _context.HomeOwnerPermissions.Add(permission);
            _context.SaveChanges();

            List<HomeMemberDto> result = _homeRepository.GetHomeMembers(home.Id);

            Assert.AreEqual(homeOwner.Email, result[0].Email);
            Assert.AreEqual(permission.Permission, result[0].Permissions[0]);
        }

        [TestMethod]
        public void GetHomeMembers_ShouldReturnIsNotificationsOnStatus()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            Guid homeOwnerId = Guid.NewGuid();

            HomeOwner homeOwner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "photo.com");

            var homeOwnerPermission = new HomeOwnerPermission
            {
                HomeId = homeId,
                HomeOwnerId = homeOwnerId,
                HomeOwner = homeOwner,
                Permission = "View",
                IsNotificationEnabled = true 
            };

            var permissions = new List<HomeOwnerPermission> { homeOwnerPermission };

            var homeOwnerPermissionsMock = new Mock<DbSet<HomeOwnerPermission>>();
            homeOwnerPermissionsMock.As<IQueryable<HomeOwnerPermission>>().Setup(m => m.Provider).Returns(permissions.AsQueryable().Provider);
            homeOwnerPermissionsMock.As<IQueryable<HomeOwnerPermission>>().Setup(m => m.Expression).Returns(permissions.AsQueryable().Expression);
            homeOwnerPermissionsMock.As<IQueryable<HomeOwnerPermission>>().Setup(m => m.ElementType).Returns(permissions.AsQueryable().ElementType);
            homeOwnerPermissionsMock.As<IQueryable<HomeOwnerPermission>>().Setup(m => m.GetEnumerator()).Returns(permissions.GetEnumerator());

            var contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.HomeOwnerPermissions).Returns(homeOwnerPermissionsMock.Object);

            var homeRepository = new HomeRepository(contextMock.Object);

            // Act
            List<HomeMemberDto> result = homeRepository.GetHomeMembers(homeId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("john.doe@mail.com", result[0].Email);
            Assert.IsTrue(result[0].IsNotificationsOn); 
        }

        [TestMethod]
        public void GetDevicesByHomeId_ShouldReturnDeviceList_WithList()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            List<HomeDevice> homeDevices = new List<HomeDevice>
            {
                new HomeDevice
                {
                    HomeId = homeId,
                    HardwareId = Guid.NewGuid(),
                    Name = "Device 1",
                    Device = new Camera("Device 1", "Model 1", "Una desc", new List<string> { "photo1.jpg" }, true, true, false, true)
                    {
                        Company = new Company("123456789011", "Company 1", "logo1.png")
                    },
                    Connected = true,
                    Room = new Room { Name = "Living Room" }
                },
                new HomeDevice
                {
                    HomeId = homeId,
                    HardwareId = Guid.NewGuid(),
                    Name = "Device 2",
                    Device = new Camera("Device 2", "Model 2", "Una desc", new List<string> { "photo1.jpg" }, true, true, false, true)
                    {
                        Company = new Company("987654321098", "Company 2", "logo2.png")
                    },
                    Connected = false,
                    Room = new Room { Name = "Kitchen" }
                }
            };

            var homeDevicesMock = new Mock<DbSet<HomeDevice>>();
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.Provider).Returns(homeDevices.AsQueryable().Provider);
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.Expression).Returns(homeDevices.AsQueryable().Expression);
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.ElementType).Returns(homeDevices.AsQueryable().ElementType);
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.GetEnumerator()).Returns(homeDevices.AsQueryable().GetEnumerator());

            var contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.HomeDevices).Returns(homeDevicesMock.Object);

            var homeRepository = new HomeRepository(contextMock.Object);

            // Act
            List<DeviceDto> result = homeRepository.GetDevicesByHomeId(homeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Device 1", result[0].Name);
            Assert.AreEqual("Device 2", result[1].Name);
            Assert.AreEqual("Living Room", result[0].RoomName);
            Assert.AreEqual("Kitchen", result[1].RoomName);
            Assert.AreEqual("Company 1", result[0].Company);
            Assert.AreEqual("Company 2", result[1].Company);
        }

        [TestMethod]
        public void GetDevicesByHomeId_WhenDeviceIsWindow_ShouldReturnIsOpenOrOnStatus()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            var windowDevice = new HomeDevice
            {
                HomeId = homeId,
                Device = new WindowSensor("Window Sensor", "Window Model", "Una ventana inteligente", new List<string> { "windowPhoto.jpg" })
                {
                    Company = new Company("123456789011", "TechCorp", "windowLogo.png") 
                },
                Connected = true,
                IsOpenOrOn = true, 
                Room = new Room { Name = "Living Room" } 
            };

            List<HomeDevice> homeDevices = new List<HomeDevice> { windowDevice };

            Mock<DbSet<HomeDevice>> homeDevicesMock = new Mock<DbSet<HomeDevice>>();
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.Provider).Returns(homeDevices.AsQueryable().Provider);
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.Expression).Returns(homeDevices.AsQueryable().Expression);
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.ElementType).Returns(homeDevices.AsQueryable().ElementType);
            homeDevicesMock.As<IQueryable<HomeDevice>>().Setup(m => m.GetEnumerator()).Returns(homeDevices.GetEnumerator());

            Mock<SmartHomeContext> contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.HomeDevices).Returns(homeDevicesMock.Object);

            HomeRepository homeRepository = new HomeRepository(contextMock.Object);

            // Act
            List<DeviceDto> result = homeRepository.GetDevicesByHomeId(homeId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(true, result[0].OpenOrOn);
        }

        [TestMethod]
        public void GetDevicesByHomeId_ShouldReturnDevicesInSpecificRoom_WhenRoomFilterIsApplied()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var roomName = "Living Room";
            var devices = new List<HomeDevice>
            {
                new HomeDevice
                {
                    HomeId = homeId,
                    HardwareId = Guid.NewGuid(),
                    Name = "Lamp 1",
                    Device = new SmartLamp("Lamp 1", "LMP123", "Lámpara inteligente", new List<string> { "lamp.jpg" })
                    {
                        Company = new Company("123456789011", "TechCorp", "logo1.png")
                    },
                    Room = new Room { Name = roomName },
                    Connected = true,
                    IsOpenOrOn = true
                },
                new HomeDevice
                {
                    HomeId = homeId,
                    HardwareId = Guid.NewGuid(),
                    Name = "Window Sensor 1",
                    Device = new WindowSensor("Window Sensor 1", "SEN123", "Sensor para apertura y cierre", new List<string> { "photo1.jpg", "photo2.jpg" })
                    {
                        Company = new Company("987654321098", "SmartCo", "logo2.png")
                    },
                    Room = new Room { Name = "Bedroom" },
                    Connected = true,
                    IsOpenOrOn = false
                }
            };

            var homeDeviceSetMock = new Mock<DbSet<HomeDevice>>();
            homeDeviceSetMock.As<IQueryable<HomeDevice>>().Setup(m => m.Provider).Returns(devices.AsQueryable().Provider);
            homeDeviceSetMock.As<IQueryable<HomeDevice>>().Setup(m => m.Expression).Returns(devices.AsQueryable().Expression);
            homeDeviceSetMock.As<IQueryable<HomeDevice>>().Setup(m => m.ElementType).Returns(devices.AsQueryable().ElementType);
            homeDeviceSetMock.As<IQueryable<HomeDevice>>().Setup(m => m.GetEnumerator()).Returns(devices.AsQueryable().GetEnumerator());

            var contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.HomeDevices).Returns(homeDeviceSetMock.Object);

            var homeRepository = new HomeRepository(contextMock.Object);

            // Act
            var result = homeRepository.GetDevicesByHomeId(homeId, roomName);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Lamp 1", result[0].Name);
            Assert.AreEqual("Living Room", result[0].RoomName);
            Assert.AreEqual("TechCorp", result[0].Company);
        }

        [TestMethod]
        public void SaveHomeMockTest()
        {
            HomeOwner homeOwner = new HomeOwner("John", "Doe", "owner@mail.com", "password123!", "photo.com");
            Home expectedHome = new Home
            {
                Address = "123 Main St",
                Location = "(1,1)",
                Capacity = 5,
                HomeOwnerId = homeOwner.Id,
                Owner = homeOwner
            };
            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            Mock<DbSet<Home>> mockDbSet = new Mock<DbSet<Home>>();

            mockContext.Setup(ctx => ctx.Homes).Returns(mockDbSet.Object);
            IHomeRepository homeRepository = new HomeRepository(mockContext.Object);

            homeRepository.Save();

            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetHomeDeviceById_ExistingHardwareId_ShouldReturnHomeDevice()
        {
            Guid hardwareId = Guid.NewGuid();
            HomeDevice expectedDevice = new HomeDevice { HardwareId = hardwareId };

            var homeDevices = new List<HomeDevice>
            {
                expectedDevice,
                new HomeDevice { HardwareId = Guid.NewGuid() },
                new HomeDevice { HardwareId = Guid.NewGuid() }
            }.AsQueryable();

            Mock<DbSet<HomeDevice>> mockDbSet = new Mock<DbSet<HomeDevice>>();
            mockDbSet.As<IQueryable<HomeDevice>>().Setup(m => m.Provider).Returns(homeDevices.Provider);
            mockDbSet.As<IQueryable<HomeDevice>>().Setup(m => m.Expression).Returns(homeDevices.Expression);
            mockDbSet.As<IQueryable<HomeDevice>>().Setup(m => m.ElementType).Returns(homeDevices.ElementType);
            mockDbSet.As<IQueryable<HomeDevice>>().Setup(m => m.GetEnumerator()).Returns(homeDevices.GetEnumerator());

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            mockContext.Setup(c => c.HomeDevices).Returns(mockDbSet.Object);

            HomeRepository homeRepository = new HomeRepository(mockContext.Object);

            HomeDevice? result = homeRepository.GetHomeDeviceById(hardwareId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDevice.HardwareId, result.HardwareId);
            mockContext.Verify(c => c.HomeDevices, Times.Once);
        }

        [TestMethod]
        public void UpdateHomeName_ShouldSaveChanges_WhenHomeExists()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            Home originalHome = new("Old Name", "123 Street", "City", 4) { Id = homeId };
            Home updatedHome = new("New Name", "123 Street", "City", 4) { Id = homeId };

            var homeSetMock = new Mock<DbSet<Home>>();
            homeSetMock.Setup(m => m.Find(homeId)).Returns(originalHome);

            var contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.Homes).Returns(homeSetMock.Object);
            contextMock.Setup(c => c.SaveChanges());

            var homeRepository = new HomeRepository(contextMock.Object);

            // Act
            homeRepository.UpdateHomeName(updatedHome);

            // Assert
            Assert.AreEqual("New Name", originalHome.Name);
            contextMock.Verify(c => c.SaveChanges(), Times.Once);
            contextMock.VerifyAll();
        }

        [TestMethod]
        public void AddRoom_ShouldSaveRoomToDatabase()
        {
            // Arrange
            Room room = new Room { Id = Guid.NewGuid(), Name = "Living Room", HomeId = Guid.NewGuid() };

            var roomSetMock = new Mock<DbSet<Room>>();
            roomSetMock.Setup(m => m.Add(room));

            var contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.Room).Returns(roomSetMock.Object);
            contextMock.Setup(c => c.SaveChanges());

            var homeRepository = new HomeRepository(contextMock.Object);

            // Act
            homeRepository.AddRoom(room);

            // Assert
            roomSetMock.Verify(m => m.Add(room), Times.Once); 
            contextMock.Verify(c => c.SaveChanges(), Times.Once); 
        }

        [TestMethod]
        public void GetRoomByName_ShouldReturnRoom_WhenExists()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var roomName = "Living Room";
            var room = new Room { Id = Guid.NewGuid(), Name = roomName, HomeId = homeId };

            var roomSetMock = new Mock<DbSet<Room>>();
            roomSetMock.As<IQueryable<Room>>().Setup(m => m.Provider).Returns(new List<Room> { room }.AsQueryable().Provider);
            roomSetMock.As<IQueryable<Room>>().Setup(m => m.Expression).Returns(new List<Room> { room }.AsQueryable().Expression);
            roomSetMock.As<IQueryable<Room>>().Setup(m => m.ElementType).Returns(new List<Room> { room }.AsQueryable().ElementType);
            roomSetMock.As<IQueryable<Room>>().Setup(m => m.GetEnumerator()).Returns(new List<Room> { room }.AsQueryable().GetEnumerator());

            var contextMock = new Mock<SmartHomeContext>();
            contextMock.Setup(c => c.Room).Returns(roomSetMock.Object);

            var homeRepository = new HomeRepository(contextMock.Object);

            // Act
            var result = homeRepository.GetRoomByName(homeId, roomName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(room.Id, result.Id);
            Assert.AreEqual(room.Name, result.Name);
        }

        [TestMethod]
        public void GetHomesByOwner_ReturnsCorrectHomes()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var homes = new List<Home>
            {
                new Home { Id = Guid.NewGuid(), Name = "Home 1", HomeOwnerId = userId, Address = "Address 1", Capacity = 4 },
                new Home { Id = Guid.NewGuid(), Name = "Home 2", HomeOwnerId = userId, Address = "Address 2", Capacity = 3 },
                new Home { Id = Guid.NewGuid(), Name = "Home 3", HomeOwnerId = Guid.NewGuid(), Address = "Address 3", Capacity = 2 }
            };

            var mockContext = new Mock<SmartHomeContext>();
            var mockDbSet = new Mock<DbSet<Home>>();

            mockDbSet.As<IQueryable<Home>>().Setup(m => m.Provider).Returns(homes.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Home>>().Setup(m => m.Expression).Returns(homes.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Home>>().Setup(m => m.ElementType).Returns(homes.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Home>>().Setup(m => m.GetEnumerator()).Returns(homes.AsQueryable().GetEnumerator());

            mockContext.Setup(c => c.Homes).Returns(mockDbSet.Object);

            var homeRepository = new HomeRepository(mockContext.Object);

            // Act
            var result = homeRepository.GetHomesByOwner(userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(h => h.HomeOwnerId == userId)); 
        }

    }
}
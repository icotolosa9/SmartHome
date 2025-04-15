using BusinessLogic;
using Domain;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Moq;
using Models.In;
using Models.Out;
using System.Data;

namespace TestBusinessLogic
{
    [TestClass]
    public class HomeLogicTest
    {
        private Mock<IHomeRepository> _homeRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IDeviceRepository> _deviceRepositoryMock;
        private HomeLogic _homeLogic;

        private Home _home;
        private HomeOwner _homeOwner;

        [TestInitialize]
        public void Setup()
        {
            _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);

            _homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, _deviceRepositoryMock.Object);

            _homeOwner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "homeOwner");
            _home = new Home("My Home", "123 Main St", "(37.7749, -122.4194)", 5);
            _homeOwner.Homes.Add(_home);
        }

        [TestMethod]
        public void CreateHome_AssociatesOwnerToHome_ReturnsCreatedHome()
        {
            // Arrange
            string ownerEmail = "john.doe@mail.com";
            var homeOwner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "homeOwner");
            var home = new Home("My Home", "123 Main St", "(37.7749, -122.4194)", 5);

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(homeOwner);
            _userRepositoryMock.Setup(repo => repo.Update(homeOwner)).Returns(homeOwner);
            _homeRepositoryMock.Setup(repo => repo.CreateHome(home)).Returns(home);

            Home createdHome = _homeLogic.CreateHome(home, ownerEmail);

            Assert.AreEqual(home, createdHome);
            _userRepositoryMock.Verify(repo => repo.Update(homeOwner), Times.Once);
        }

        [TestMethod]
        public void CreateHome_WhenCalled_ShouldSaveHomeWithGivenName()
        {
            // Arrange
            string ownerEmail = "owner@example.com";
            string homeName = "My New Home"; 
            var home = new Home { Name = homeName };

            var owner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto")
            {
                Id = Guid.NewGuid(),
                Homes = new List<Home>()
            };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);

            var homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.CreateHome(home)).Returns(home);

            var homeLogic = new HomeLogic(homeRepoMock.Object, userRepoMock.Object, null);

            // Act
            var createdHome = homeLogic.CreateHome(home, ownerEmail);

            // Assert
            Assert.IsNotNull(createdHome);
            Assert.AreEqual(homeName, createdHome.Name); 
            homeRepoMock.Verify(repo => repo.CreateHome(home), Times.Once);
            userRepoMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once); 
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException), "Ya existe un hogar con el mismo nombre para este propietario.")]
        public void CreateHome_ShouldThrowException_WhenDuplicateHomeNameExistsForOwner()
        {
            // Arrange
            string ownerEmail = "owner@example.com";
            string homeName = "My Home";
            Home existingHome = new(homeName, "123 Street", "City", 4);

            var owner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto")
            {
                Id = Guid.NewGuid(),
                Homes = new List<Home> { existingHome } 
            };

            Home newHome = new(homeName, "456 Avenue", "New City", 3);

            Mock<IUserRepository> userRepoMock = new();
            userRepoMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);

            Mock<IHomeRepository> homeRepoMock = new();

            HomeLogic homeLogic = new(homeRepoMock.Object, userRepoMock.Object, null);

            // Act
            homeLogic.CreateHome(newHome, ownerEmail);

            // Assert: Verificado con ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(HomeNotFoundException))]
        public void AddMemberToHome_HomeNotFound_ThrowsException()
        {
            Guid homeId = Guid.NewGuid();
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns((Home)null);

            // Act
            _homeLogic.AddMemberToHome(homeId, new AddHomeMemberRequest(), "john.doe@mail.com");
        }

        [TestMethod]
        [ExpectedException(typeof(HomeCapacityExceededException))]
        public void AddMemberToHome_CapacityExceeded_ThrowsException()
        {
            // Arrange
            var homeId = _home.Id;
            _home.Capacity = 1;
            _home.HomeOwnerId = _homeOwner.Id; 
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(_home);
            _homeRepositoryMock.Setup(repo => repo.GetHomeMembers(homeId)).Returns(new List<HomeMemberDto>
            {
                new HomeMemberDto { Email = "existingMember@mail.com" }
            });
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail("john.doe@mail.com")).Returns(_homeOwner);

            // Act
            _homeLogic.AddMemberToHome(homeId, new AddHomeMemberRequest
            {
                Members = new List<AddHomeMemberDto> { new AddHomeMemberDto { UserEmail = "newMember@mail.com" } }
            }, "john.doe@mail.com");
        }

        [TestMethod]
        public void AddMemberToHome_ValidRequest_AddsMemberSuccessfully()
        {
            // Arrange
            Guid homeId = _home.Id;

            HomeOwner owner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "photo.com") { Id = Guid.NewGuid() };
            _home.HomeOwnerId = owner.Id;
            owner.Homes = new List<Home> { _home };

            AddHomeMemberRequest addHomeMemberRequest = new AddHomeMemberRequest
            {
                Members = new List<AddHomeMemberDto>
                {
                    new AddHomeMemberDto { UserEmail = "newmember@mail.com", Permissions = new List<string> { "AddDevice" } }
                }
            };

            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(_home);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail("john.doe@mail.com")).Returns(owner);

            HomeOwner newMember = new HomeOwner("New", "Member", "newmember@mail.com", "password123!", "homeOwner");
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail("newmember@mail.com")).Returns(newMember);

            _homeRepositoryMock.Setup(repo => repo.GetHomeMembers(homeId)).Returns(new List<HomeMemberDto>());

            _homeRepositoryMock
                .Setup(repo => repo.AddMemberToHome(It.IsAny<Home>(), It.IsAny<AddHomeMemberRequest>()))
                .Returns(true)
                .Verifiable();

            // Act
            bool result = _homeLogic.AddMemberToHome(homeId, addHomeMemberRequest, "john.doe@mail.com");

            // Assert
            Assert.IsTrue(result);
            _homeRepositoryMock.Verify(repo => repo.AddMemberToHome(It.IsAny<Home>(), It.IsAny<AddHomeMemberRequest>()), Times.Once);
        }

        [TestMethod]
        public void AssociateDeviceToHome_ValidRequest_AssociatesDeviceSuccessfully()
        {
            Guid homeId = _home.Id;
            var deviceRequest = new AssociateDeviceRequest
            {
                DeviceName = "Camera",
                DeviceModel = "CAM123",
                Connected = false
            };

            Camera device = new Camera("Camera", "CAM123", "Security Camera", new List<string> { "photo1.jpg" }, true, true, true, true);

            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(_home);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(deviceRequest.DeviceName, deviceRequest.DeviceModel)).Returns(device);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail("john.doe@mail.com")).Returns(_homeOwner);
            _home.MemberPermissions.Add(new HomeOwnerPermission
            {
                HomeOwnerId = _homeOwner.Id,
                Permission = "AddDevice"
            });
            _homeRepositoryMock.Setup(repo => repo.AssociateDeviceToHome(_home, It.IsAny<HomeDevice>())).Verifiable();

            // Act
            _homeLogic.AssociateDeviceToHome(homeId, deviceRequest, "john.doe@mail.com");

            _homeRepositoryMock.Verify(repo => repo.AssociateDeviceToHome(_home, It.IsAny<HomeDevice>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(HomeNotFoundException))]
        public void AssociateDeviceToHome_HomeNotFound_ThrowsException()
        {
            Guid homeId = Guid.NewGuid();
            AssociateDeviceRequest deviceRequest = new AssociateDeviceRequest
            {
                DeviceName = "Camera",
                DeviceModel = "CAM123",
                Connected = false
            };

            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns((Home)null);

            _homeLogic.AssociateDeviceToHome(homeId, deviceRequest, "john.doe@mail.com");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssociateDeviceToHome_DeviceNotFound_ThrowsException()
        {
            Guid homeId = _home.Id;
            AssociateDeviceRequest deviceRequest = new AssociateDeviceRequest
            {
                DeviceName = "Camera",
                DeviceModel = "CAM123",
                Connected = false
            };

            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(_home);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(deviceRequest.DeviceName, deviceRequest.DeviceModel)).Returns((Device)null);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail("john.doe@mail.com")).Returns(_homeOwner);

            _homeLogic.AssociateDeviceToHome(homeId, deviceRequest, "john.doe@mail.com");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssociateDeviceToHome_UserWithoutPermission_ThrowsException()
        {
            Guid homeId = Guid.NewGuid();
            Guid memberId = Guid.NewGuid();
            HomeOwner owner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "photo.com");
            Home home = new Home
            {
                HomeOwnerId = Guid.NewGuid(), 
                Address = "123 Home Street",
                Members = new List<User> { owner } 
            };

            Mock<IHomeRepository> homeMock = new Mock<IHomeRepository>();
            Mock<IUserRepository> userMock = new Mock<IUserRepository>();
            Mock<IDeviceRepository> deviceMock = new Mock<IDeviceRepository>();

            homeMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            homeMock.Setup(repo => repo.GetHomeOwnerPermissions(homeId, memberId)).Returns("viewStatus"); 

            userMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);

            HomeLogic homeLogic = new HomeLogic(homeMock.Object, userMock.Object, deviceMock.Object);

            AssociateDeviceRequest request = new AssociateDeviceRequest
            {
                DeviceName = "Test Device",
                DeviceModel = "Model X",
                Connected = true
            };

            homeLogic.AssociateDeviceToHome(homeId, request, owner.Email); 
        }

        [TestMethod]
        public void GetHomeDevices_UserIsHomeOwner_ShouldReturnDeviceList()
        {
            Guid homeId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Home home = new Home
            {
                HomeOwnerId = userId,
                Address = "123 Home Street"
            };
            List<DeviceDto> devices = new List<DeviceDto>
            {
                new DeviceDto { Name = "Device 1", Model = "Model 1", MainPicture = "pic1.jpg", Connected = true },
                new DeviceDto { Name = "Device 2", Model = "Model 2", MainPicture = "pic2.jpg", Connected = false }
            };

            Mock<IHomeRepository> homeMock = new Mock<IHomeRepository>();
            homeMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            homeMock.Setup(repo => repo.GetDevicesByHomeId(homeId, null)).Returns(devices);

            homeMock.Setup(repo => repo.GetHomeOwnerPermissions(homeId, userId)).Returns("listDevices");

            HomeLogic homeLogic = new HomeLogic(homeMock.Object, null, null);

            List<DeviceDto> result = homeLogic.GetHomeDevices(homeId, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("Device 1", result[0].Name);
            Assert.AreEqual("Device 2", result[1].Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetHomeDevices_UserIsMemberWithoutPermission_ShouldThrowException()
        {
            Guid homeId = Guid.NewGuid();
            HomeOwner owner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "photo.com");
            Home home = new Home
            {
                HomeOwnerId = Guid.NewGuid(), 
                Address = "123 Home Street",
                Members = new List<User> { owner }
            };

            Mock<IHomeRepository> homeMock = new Mock<IHomeRepository>();
            homeMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            homeMock.Setup(repo => repo.GetHomeOwnerPermissions(homeId, owner.Id)).Returns("viewStatus");

            HomeLogic homeLogic = new HomeLogic(homeMock.Object, null, null);

            homeLogic.GetHomeDevices(homeId, owner.Id); 
        }

        [TestMethod]
        public void GetHomeDevices_ShouldReturnDevicesWithRoomFilter()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var roomName = "Living Room";
            var userPermissions = "listDevices";

            var devices = new List<DeviceDto>
            {
                new DeviceDto { Name = "Device 1", Model = "Model 1", MainPicture = "pic1.jpg", Connected = true }
            };

            var homeMock = new Mock<IHomeRepository>();

            homeMock.Setup(repo => repo.GetHomeById(homeId)).Returns(new Home { HomeOwnerId = userId });
            homeMock.Setup(repo => repo.GetHomeOwnerPermissions(homeId, userId)).Returns(userPermissions);
            homeMock.Setup(repo => repo.GetDevicesByHomeId(homeId, roomName)).Returns(devices);

            var homeLogic = new HomeLogic(homeMock.Object, null, null);

            // Act
            var result = homeLogic.GetHomeDevices(homeId, userId, roomName);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Device 1", result[0].Name);
            homeMock.Verify(repo => repo.GetDevicesByHomeId(homeId, roomName), Times.Once);
        }


        [TestMethod]
        public void UpdateMemberNotifications_UserIsHomeOwner_ShouldUpdateNotifications()
        {
            Guid homeId = Guid.NewGuid();
            HomeOwner owner = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "photo.com");

            Home home = new Home
            {
                Id = homeId,
                HomeOwnerId = owner.Id,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = owner.Id, IsNotificationEnabled = false },
                    new HomeOwnerPermission { HomeOwnerId = Guid.NewGuid(), IsNotificationEnabled = false }
                }
            };

            var updates = new List<MemberNotificationUpdateDto>
            {
                new MemberNotificationUpdateDto { HomeOwnerEmail = "john.doe@mail.com", IsNotificationEnabled = true }
            };

            Mock<IHomeRepository> homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);

            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserByEmail("john.doe@mail.com")).Returns(owner);

            HomeLogic homeLogic = new HomeLogic(homeRepoMock.Object, userRepoMock.Object, null);

            bool result = homeLogic.UpdateMemberNotifications(homeId, owner.Id, updates);

            Assert.IsTrue(home.MemberPermissions[0].IsNotificationEnabled); 
            homeRepoMock.Verify(repo => repo.GetHomeById(homeId), Times.Once); 
            userRepoMock.Verify(repo => repo.GetUserByEmail("john.doe@mail.com"), Times.Once); 
        }

        [TestMethod]
        public void SetStatusHomeDevice_ValidRequest_UpdatesDeviceStatus()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            var homeId = _home.Id;
            bool status = true;

            var homeDevice = new HomeDevice { HardwareId = hardwareId, HomeId = homeId, Connected = false };
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            _userRepositoryMock.Setup(repo => repo.GetUserById(_homeOwner.Id)).Returns(_homeOwner);
            _homeRepositoryMock.Setup(repo => repo.UpdateHomeDevice(It.IsAny<HomeDevice>())).Verifiable();

            // Act
            _homeLogic.SetStatusHomeDevice(hardwareId, status, _homeOwner.Id);

            // Assert
            Assert.IsTrue(homeDevice.Connected); 
            _homeRepositoryMock.Verify(repo => repo.UpdateHomeDevice(homeDevice), Times.Once);
            _homeRepositoryMock.VerifyAll(); 
            _userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDevice))]
        public void SetStatusHomeDevice_DeviceNotFound_ThrowsNotFoundDeviceException()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            bool status = true;

            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns((HomeDevice)null); 

            // Act
            _homeLogic.SetStatusHomeDevice(hardwareId, status, _homeOwner.Id);

            // Assert
            _homeRepositoryMock.VerifyAll(); 
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedOwnerException))]
        public void SetStatusHomeDevice_UserNotAuthorized_ThrowsUnauthorizedOwnerException()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            bool status = true;
            var unauthorizedUser = new HomeOwner("Unauthorized", "User", "unauthorized@mail.com", "password123!", "homeOwner");


            var homeDevice = new HomeDevice { HardwareId = hardwareId, HomeId = _home.Id, Connected = false };
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            _userRepositoryMock.Setup(repo => repo.GetUserById(unauthorizedUser.Id)).Returns(unauthorizedUser); 

            // Act
            _homeLogic.SetStatusHomeDevice(hardwareId, status, unauthorizedUser.Id);

            // Assert
            _homeRepositoryMock.VerifyAll(); 
            _userRepositoryMock.VerifyAll(); 
        }

        [TestMethod]
        public void UpdateHomeName_ShouldUpdateName_WhenCalledByOwnerAndNoDuplicate()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            string newName = "Updated Home Name";
            string ownerEmail = "owner@example.com";

            var owner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto")
            {
                Id = Guid.NewGuid(),
                Homes = new List<Home> { new Home("Existing Home", "Address", "City", 4) { Id = Guid.NewGuid() } }
            };

            Home home = new("Old Home Name", "Address", "City", 4) { Id = homeId, HomeOwnerId = owner.Id };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);

            var homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            homeRepoMock.Setup(repo => repo.UpdateHomeName(home));

            var homeLogic = new HomeLogic(homeRepoMock.Object, userRepoMock.Object, null);

            // Act
            homeLogic.UpdateHomeName(homeId, new UpdateHomeNameRequest { NewName = newName }, ownerEmail);

            // Assert
            Assert.AreEqual(newName, home.Name);
            userRepoMock.VerifyAll();
            homeRepoMock.VerifyAll();
        }

        [TestMethod]
        public void CreateRoom_ShouldCreateRoom_WhenCalledByOwner()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            string roomName = "Living Room";
            string ownerEmail = "owner@example.com";

            HomeOwner owner = new("John", "Doe", ownerEmail, "password123!", "ownerPhoto") { Id = Guid.NewGuid() };
            Home home = new Home { Id = homeId, HomeOwnerId = owner.Id };
            RoomDto roomDto = new RoomDto { Id = Guid.NewGuid(), Name = roomName, HomeId = homeId };

            var request = new CreateRoomRequest { Name = roomName, HomeId = homeId };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            _homeRepositoryMock.Setup(repo => repo.GetRoomByName(homeId, roomName)).Returns((Room)null);
            _homeRepositoryMock.Setup(repo => repo.AddRoom(It.Is<Room>(r => r.Name == roomName && r.HomeId == homeId)));

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            var result = homeLogic.CreateRoom(request, ownerEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roomDto.Name, result.Name);
            Assert.AreEqual(homeId, result.HomeId);
            _userRepositoryMock.VerifyAll();
            _homeRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedOwnerException))]
        public void CreateRoom_ShouldThrowUnauthorizedOwnerException_WhenUserIsNotOwner()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            string nonOwnerEmail = "nonowner@example.com";
            string ownerEmail = "owner@example.com";
            string roomName = "Living Room";

            var owner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto") { Id = Guid.NewGuid() };
            var nonOwner = new HomeOwner("Jane", "Smith", nonOwnerEmail, "password123!", "nonOwnerPhoto") { Id = Guid.NewGuid() };
            var home = new Home { Id = homeId, HomeOwnerId = owner.Id };

            var request = new CreateRoomRequest { Name = roomName, HomeId = homeId };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(nonOwnerEmail)).Returns(nonOwner);
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            homeLogic.CreateRoom(request, nonOwnerEmail);

            // Assert
            // Se espera que lance UnauthorizedOwnerException
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void CreateRoom_ShouldThrowException_WhenRoomNameAlreadyExistsInHome()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            string roomName = "Living Room";
            string ownerEmail = "owner@example.com";

            HomeOwner owner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto") { Id = Guid.NewGuid() };
            var home = new Home { Id = homeId, HomeOwnerId = owner.Id };
            var existingRoom = new Room { Name = roomName, HomeId = homeId };

            var request = new CreateRoomRequest { Name = roomName, HomeId = homeId };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            _homeRepositoryMock.Setup(repo => repo.GetRoomByName(homeId, roomName)).Returns(existingRoom);

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            homeLogic.CreateRoom(request, ownerEmail);
        }


        [TestMethod]
        public void AssignRoomToDevice_ShouldAssignRoom_WhenCalledByOwner()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var hardwareId = Guid.NewGuid();
            var roomName = "Living Room";
            var ownerEmail = "owner@example.com";

            var owner = new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto") { Id = Guid.NewGuid() };
            var home = new Home { Id = homeId, HomeOwnerId = owner.Id };
            var room = new Room { Id = Guid.NewGuid(), HomeId = homeId, Name = roomName };
            var homeDevice = new HomeDevice { HardwareId = hardwareId, HomeId = homeId };

            var request = new AssignDeviceToRoomRequest { HardwareId = hardwareId, RoomName = roomName };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            _homeRepositoryMock.Setup(repo => repo.GetRoomByName(homeId, roomName)).Returns(room);
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            _homeRepositoryMock.Setup(repo => repo.UpdateHomeDevice(homeDevice));

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            homeLogic.AssignDeviceToRoom(homeId, request, ownerEmail);

            // Assert
            Assert.AreEqual(room.Id, homeDevice.RoomId);
            _homeRepositoryMock.Verify(repo => repo.UpdateHomeDevice(homeDevice), Times.Once);
        }

        [TestMethod]
        public void HasPermission_ShouldReturnTrue_WhenPermissionIsPresent()
        {
            // Arrange
            var permissions = "AssignDevice, ViewDevice, EditDevice";
            var requiredPermission = "AssignDevice";

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            var result = homeLogic.HasPermission(permissions, requiredPermission);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasPermission_ShouldReturnFalse_WhenPermissionIsNotPresent()
        {
            // Arrange
            var permissions = "ViewDevice, EditDevice";
            var requiredPermission = "AssignDevice";

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            var result = homeLogic.HasPermission(permissions, requiredPermission);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateDeviceName_ShouldUpdate_WhenUserIsOwnerOrAuthorized()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var userEmail = "owner@example.com";
            var homeDevice = new HomeDevice { HardwareId = hardwareId, HomeId = Guid.NewGuid(), Name = "Original Name" };
            var home = new Home { Id = homeDevice.HomeId, HomeOwnerId = Guid.NewGuid() };
            var user = new HomeOwner("John", "Doe", userEmail, "password123!", "ownerPhoto") { Id = home.HomeOwnerId };

            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeDevice.HomeId)).Returns(home);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(userEmail)).Returns(user);
            _homeRepositoryMock.Setup(repo => repo.UpdateHomeDevice(homeDevice));

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            homeLogic.UpdateDeviceName(hardwareId, "Portón Frente", userEmail);

            // Assert
            Assert.AreEqual("Portón Frente", homeDevice.Name);
            _homeRepositoryMock.Verify(repo => repo.UpdateHomeDevice(homeDevice), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDevice))]
        public void UpdateDeviceName_ShouldThrowException_WhenDeviceDoesNotExist()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var userEmail = "owner@example.com";

            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns((HomeDevice)null);

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, null);

            // Act
            homeLogic.UpdateDeviceName(hardwareId, "Portón Frente", userEmail);
        }

        [TestMethod]
        public void GetHomesByUser_ValidUserId_ReturnsHomesSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var homes = new List<HomeDto>
            {
                new HomeDto { Id = Guid.NewGuid(), Name = "Home 1", Address = "Address 1", Capacity = 4 },
                new HomeDto { Id = Guid.NewGuid(), Name = "Home 2", Address = "Address 2", Capacity = 3 }
            };

            _userRepositoryMock.Setup(repo => repo.GetHomesByUser(userId)).Returns(homes);

            var homeLogic = new HomeLogic(_homeRepositoryMock.Object, _userRepositoryMock.Object, _deviceRepositoryMock.Object);

            // Act
            var result = homeLogic.GetHomesByUser(userId);

            // Assert
            Assert.AreEqual(homes, result);  
            _userRepositoryMock.VerifyAll(); 
        }

        [TestMethod]
        public void GetHomesByOwner_Ok()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var homes = new List<Home>
            {
                new Home { Id = Guid.NewGuid(), Name = "Home 1", Address = "Address 1", Capacity = 4 },
                new Home { Id = Guid.NewGuid(), Name = "Home 2", Address = "Address 2", Capacity = 3 }
            };

            _homeRepositoryMock.Setup(repo => repo.GetHomesByOwner(userId)).Returns(homes);

            // Act
            var result = _homeLogic.GetHomesByOwner(userId);

            // Assert
            Assert.AreEqual(2, result.Count); 
            Assert.AreEqual("Home 1", result[0].Name); 
            Assert.AreEqual("Home 2", result[1].Name); 

            _homeRepositoryMock.Verify(repo => repo.GetHomesByOwner(userId), Times.Once);
            _homeRepositoryMock.VerifyAll(); 
        }
    }
}
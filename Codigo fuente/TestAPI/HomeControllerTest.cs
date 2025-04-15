using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.In;
using Models.Out;
using Moq;
using SmartHome.Controllers;

namespace TestAPI
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _homeController;
        private Mock<IHomeLogic> _homeLogicMock;
        private Mock<IUserLogic> _userLogicMock;
        private CreateHomeRequest _createHomeRequest;
        private CreateHomeResponse _createHomeResponse;
        private Home _home;
        private HomeOwner _homeOwner;
        private string _userEmail;

        [TestInitialize]
        public void Setup()
        {
            _homeLogicMock = new Mock<IHomeLogic>(MockBehavior.Strict);
            _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _homeController = new HomeController(_homeLogicMock.Object, _userLogicMock.Object);

            _userEmail = "john.doe@gmail.com";
            _homeOwner = new HomeOwner("John", "Doe", _userEmail, "password123!", "homeOwner");

            var httpContext = new DefaultHttpContext();
            _homeController.ControllerContext.HttpContext = httpContext;

            _createHomeRequest = new CreateHomeRequest
            {
                Address = "Obligado 1123",
                Location = "(2, 2)",
                Capacity = 5,
            };

            _createHomeResponse = new CreateHomeResponse
            {
                Id = Guid.NewGuid(),
                Address = "Obligado 1123",
                Location = "(2, 2)",
                Capacity = 5,
            };

            _home = new Home("My Home", "Obligado 1123", "(2, 2)", 5) { Owner = _homeOwner };
        }

        [TestMethod]
        public void CreateHome_Ok()
        {
            // Arrange
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_homeOwner);
            _homeLogicMock.Setup(hl => hl.CreateHome(It.IsAny<Home>(), _userEmail)).Returns(_home);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString(); 

            // Act
            CreatedResult result = _homeController.CreateHome(_createHomeRequest) as CreatedResult;
            var objectResult = result?.Value as CreateHomeResponse;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual("Obligado 1123", objectResult.Address);
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void AddMemberToHome_Ok()
        {
            // Arrange
            var token = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();
            AddHomeMemberRequest memberRequest = new AddHomeMemberRequest
            {
                Members = new List<AddHomeMemberDto>
                {
                    new AddHomeMemberDto { UserEmail = "new@mail.com", Permissions = new List<string> { "AddDevice", "ListDevices" } }
                }
            };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_homeOwner);
            _homeLogicMock.Setup(hl => hl.AddMemberToHome(homeId, memberRequest, _homeOwner.Email))
                          .Returns(true);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.AddMemberToHome(homeId, memberRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _homeLogicMock.Verify(hl => hl.AddMemberToHome(homeId, memberRequest, _homeOwner.Email), Times.Once);
        }

        [TestMethod]
        public void GetHomeMembers_Ok()
        {
            // Arrange
            var token = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();
            List<HomeMemberDto> members = new List<HomeMemberDto>
            {
                new HomeMemberDto { FirstName = "Jane", LastName = "Smith", Email = "jane@mail.com", Permissions = new List<string> { "Admin" } }
            };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_homeOwner);
            _homeLogicMock.Setup(hl => hl.GetHomeMembers(homeId, _homeOwner.Email)).Returns(members);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.GetHomeMembers(homeId);

            // Assert
            OkObjectResult okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            List<HomeMemberDto> resultValue = okResult?.Value as List<HomeMemberDto>;
            Assert.AreEqual(members[0], resultValue[0]);
        }

        [TestMethod]
        public void AssociateDevice_Ok()
        {
            // Arrange
            var token = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();
            AssociateDeviceRequest deviceRequest = new AssociateDeviceRequest
            {
                DeviceName = "Camera",
                DeviceModel = "12345",
                Connected = false
            };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_homeOwner);
            _homeLogicMock.Setup(hl => hl.AssociateDeviceToHome(homeId, deviceRequest, _homeOwner.Email)).Verifiable();

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.AssociateDevice(homeId, deviceRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void GetDevicesByHomeId_Ok_ReturnsDeviceList()
        {
            // Arrange
            var token = Guid.NewGuid();
            var homeId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var devices = new List<DeviceDto>
            {
                new DeviceDto { Name = "Camera", Model = "CAM123", MainPicture = "photo1.jpg", Company = "Sony" },
                new DeviceDto { Name = "Window Sensor", Model = "SEN123", MainPicture = "photo2.jpg", Company = "Samsung" }
            };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "homeOwner") { Id = userId });
            _homeLogicMock.Setup(hl => hl.GetHomeDevices(homeId, userId, null)).Returns(devices);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.GetDevicesByHomeId(homeId);

            // Assert
            OkObjectResult okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedDevices = okResult.Value as List<DeviceDto>;
            Assert.IsNotNull(returnedDevices);
            Assert.AreEqual("Camera", returnedDevices[0].Name); 
            _homeLogicMock.Verify(hl => hl.GetHomeDevices(homeId, userId, null), Times.Once); 
        }

        [TestMethod]
        public void GetDevicesByHomeId_WithRoomFilter()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var roomName = "Living Room";
            var token = Guid.NewGuid();
            var user = new HomeOwner("John", "Doe", "owner@example.com", "password123!", "ownerPhoto") { Id = token };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(user);
            _homeLogicMock.Setup(hl => hl.GetHomeDevices(homeId, user.Id, roomName)).Returns(new List<DeviceDto>());

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = _homeController.GetDevicesByHomeId(homeId, roomName);

            // Assert
            _homeLogicMock.Verify(logic => logic.GetHomeDevices(homeId, user.Id, roomName), Times.Once);
        }


        [TestMethod]
        public void UpdateMemberNotifications_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var token = Guid.NewGuid();
            var homeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            string userEmail = "user@mail.com";
            var updates = new List<MemberNotificationUpdateDto>
            {
                new MemberNotificationUpdateDto { HomeOwnerEmail = userEmail, IsNotificationEnabled = true }
            };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(new HomeOwner("John", "Doe", "user@mail.com", "password123!", "homeOwner") { Id = userId });
            _homeLogicMock.Setup(hl => hl.UpdateMemberNotifications(homeId, userId, updates)).Returns(true).Verifiable();
            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.UpdateMemberNotifications(homeId, updates);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void SetStatusHomeDevice_ConnectDevice_Ok()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_homeOwner);
            _homeLogicMock.Setup(hl => hl.SetStatusHomeDevice(hardwareId, true, _homeOwner.Id)).Verifiable();
            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.SetStatusHomeDevice(hardwareId, true);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void SetStatusHomeDevice_DisconnectDevice_Ok()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_homeOwner);
            _homeLogicMock.Setup(hl => hl.SetStatusHomeDevice(hardwareId, false, _homeOwner.Id)).Verifiable();
            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.SetStatusHomeDevice(hardwareId, false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateHomeName_ShouldCallHomeLogicAndReturnOk()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            string newName = "Updated Home Name";
            string ownerEmail = "owner@example.com";
            var request = new UpdateHomeNameRequest { NewName = newName };
            Guid token = Guid.NewGuid();

            _userLogicMock.Setup(logic => logic.GetCurrentUser(token)).Returns(new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto") { Id = Guid.NewGuid() });
            _homeLogicMock.Setup(logic => logic.UpdateHomeName(homeId, request, ownerEmail));
            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            IActionResult result = _homeController.UpdateHomeName(homeId, request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _userLogicMock.VerifyAll();
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void CreateRoom_ShouldCallHomeLogicAndReturnCreated()
        {
            // Arrange
            Guid homeId = Guid.NewGuid();
            string roomName = "Living Room";
            string ownerEmail = "owner@example.com";
            var token = Guid.NewGuid();

            var request = new CreateRoomRequest { Name = roomName, HomeId = homeId };
            var roomDto = new RoomDto { Id = Guid.NewGuid(), Name = roomName, HomeId = homeId };

            _userLogicMock.Setup(logic => logic.GetCurrentUser(token)).Returns(new HomeOwner("John", "Doe", ownerEmail, "password123!", "ownerPhoto") { Id = Guid.NewGuid() });
            _homeLogicMock.Setup(logic => logic.CreateRoom(It.Is<CreateRoomRequest>(r => r.Name == roomName && r.HomeId == homeId), ownerEmail)).Returns(roomDto);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            IActionResult result = _homeController.CreateRoom(request);

            // Assert
            CreatedResult? createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(roomDto, createdResult.Value);
            _userLogicMock.VerifyAll();
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void AssignRoomToDevice_ShouldCallHomeLogicAndReturnOk()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var hardwareId = Guid.NewGuid();
            var token = Guid.NewGuid();
            var request = new AssignDeviceToRoomRequest { HardwareId = hardwareId, RoomName = "Living Room" };

            var user = new HomeOwner("John", "Doe", "owner@example.com", "password123!", "ownerPhoto") { Id = token };

            _userLogicMock.Setup(logic => logic.GetCurrentUser(token)).Returns(user);
            _homeLogicMock.Setup(logic => logic.AssignDeviceToRoom(homeId, request, user.Email));

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            IActionResult result = _homeController.AssignRoomToDevice(homeId, request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _userLogicMock.VerifyAll();
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateDeviceName_Ok()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            var token = Guid.NewGuid();
            var user = new HomeOwner("John", "Doe", "owner@example.com", "password123!", "ownerPhoto") { Id = token };
            var request = new UpdateDeviceNameRequest { NewName = "Portón Frente" };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(user);
            _homeLogicMock.Setup(hl => hl.UpdateDeviceName(hardwareId, request.NewName, user.Email)).Verifiable();
            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = _homeController.UpdateDeviceName(hardwareId, request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void GetMyHomes_ValidToken_ReturnsHomesSuccessfully()
        {
            // Arrange
            var token = Guid.NewGuid();
            var user = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "homeOwner");
            var homes = new List<HomeDto>
            {
                new HomeDto { Id = Guid.NewGuid(), Name = "Home 1", Address = "Address 1", Capacity = 4 },
                new HomeDto { Id = Guid.NewGuid(), Name = "Home 2", Address = "Address 2", Capacity = 3 }
            };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(user);
            _homeLogicMock.Setup(hl => hl.GetHomesByUser(user.Id)).Returns(homes);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();

            // Act
            IActionResult result = _homeController.GetMyHomes();

            // Assert
            OkObjectResult okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedHomes = okResult.Value as List<HomeDto>;
            Assert.IsNotNull(returnedHomes);
            Assert.AreEqual(2, returnedHomes.Count); 
            Assert.AreEqual("Home 1", returnedHomes[0].Name);
            Assert.AreEqual("Home 2", returnedHomes[1].Name);
            _userLogicMock.VerifyAll();
            _homeLogicMock.VerifyAll();
        }

        [TestMethod]
        public void GetMyOwnedHomes_Ok()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var homes = new List<HomeDto>
            {
                new HomeDto { Id = Guid.NewGuid(), Name = "Home 1", Address = "Address 1", Capacity = 4 },
                new HomeDto { Id = Guid.NewGuid(), Name = "Home 2", Address = "Address 2", Capacity = 3 }
            };

            var user = new HomeOwner("John", "Doe", "owner@example.com", "password123!", "ownerPhoto") { Id = userId };

            _userLogicMock.Setup(ul => ul.GetCurrentUser(userId)).Returns(user);
            _homeLogicMock.Setup(hl => hl.GetHomesByOwner(userId)).Returns(homes);

            _homeController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {userId}";

            // Act
            var result = _homeController.GetMyOwnedHomes();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(homes, okResult?.Value);
            _homeLogicMock.VerifyAll();
        }

    }
}

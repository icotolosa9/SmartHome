using Domain;
using Models.Out;
using Models.In;
using SmartHome.Controllers;
using Microsoft.AspNetCore.Mvc;
using IBusinessLogic;
using Moq;
using BusinessLogic;
using IDataAccess;
using Microsoft.AspNetCore.Http;

namespace TestAPI
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController _userController;
        private CreateAdminRequest _createAdminRequest;
        private CreateAdminResponse _createAdminResponse;

        private CreateCompanyOwnerRequest _createCompanyOwnerRequest;
        private CreateCompanyOwnerResponse _createCompanyOwnerResponse;

        private CreateHomeOwnerRequest _createHomeOwnerRequest;
        private CreateHomeOwnerResponse _createHomeOwnerResponse;

        [TestInitialize]
        public void Setup()
        {
            _createAdminRequest = new CreateAdminRequest
            {
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan@gmail.com",
                Password = "Password123!"
            };

            _createAdminResponse = new CreateAdminResponse
            {
                Id = Guid.NewGuid(),
                FirstName = "Juan",
                LastName = "Perez",
                Email = "juan@gmail.com",
                Role = "admin"
            };

            _createCompanyOwnerRequest = new CreateCompanyOwnerRequest
            {
                FirstName = "Pedro",
                LastName = "Rodriguez",
                Email = "pedro@gmail.com",
                Password = "Password123!"
            };

            _createCompanyOwnerResponse = new CreateCompanyOwnerResponse
            {
                Id = Guid.NewGuid(),
                FirstName = "Pedro",
                LastName = "Rodriguez",
                Email = "pedro@gmail.com",
                Role = "homeOwner"
            };

            _createHomeOwnerRequest = new CreateHomeOwnerRequest
            {
                FirstName = "Franco",
                LastName = "Colapinto",
                Email = "franco@gmail.com",
                Password = "Password123!",
                ProfilePhoto = "photo.com"
            };

            _createHomeOwnerResponse = new CreateHomeOwnerResponse
            {
                Id = Guid.NewGuid(),
                FirstName = "Franco",
                LastName = "Colapinto",
                Email = "franco@gmail.com",
                Role = "HomeOwner",
                ProfilePhoto = "photo.com"
            };
        }

        [TestMethod]
        public void CreateAdmin_Ok()
        {
            // Arrange
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(logic => logic.CreateAdmin(It.IsAny<Admin>())).Returns(new Admin("Juan", "Perez", "juan@gmail.com", "Password123!"));
            _userController = new UserController(userLogicMock.Object);

            // Act
            CreatedResult result = _userController.CreateAdmin(_createAdminRequest) as CreatedResult;
            var objectResult = result.Value as CreateAdminResponse;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual("juan@gmail.com", objectResult.Email);
        }

        [TestMethod]
        public void CreateHomeOwner_Ok()
        {
            // Arrange
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(logic => logic.CreateHomeOwner(It.IsAny<HomeOwner>())).Returns(new HomeOwner("Franco", "Colapinto", "franco@gmail.com", "Password123!", "photo.com"));
            _userController = new UserController(userLogicMock.Object);

            // Act
            CreatedResult result = _userController.CreateHomeOwner(_createHomeOwnerRequest) as CreatedResult;
            var objectResult = result.Value as CreateHomeOwnerResponse;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual("franco@gmail.com", objectResult.Email);
        }

        [TestMethod]
        public void CreateCompanyOwner_Ok()
        {
            // Arrange
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(logic => logic.CreateCompanyOwner(It.IsAny<CompanyOwner>())).Returns(new CompanyOwner("Pedro", "Rodriguez", "pedro@gmail.com", "Password123!"));
            _userController = new UserController(userLogicMock.Object);

            // Act
            CreatedResult result = _userController.CreateCompanyOwner(_createCompanyOwnerRequest) as CreatedResult;
            var objectResult = result.Value as CreateCompanyOwnerResponse;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual("pedro@gmail.com", objectResult.Email);
        }

        [TestMethod]
        public void DeleteAdmin_AdminExists_ReturnsOk()
        {
            // Arrange
            Guid adminId = Guid.NewGuid();
            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(logic => logic.DeleteAdmin(adminId)).Returns(true);
            _userController = new UserController(userLogicMock.Object);

            DeleteAdminResponse expectedResponse = new DeleteAdminResponse
            {
                Message = $"Usuario con id {adminId} eliminado correctamente"
            };

            // Act
            IActionResult result = _userController.DeleteAdmin(adminId);
            OkObjectResult okResult = result as OkObjectResult;

            // Assert
            Assert.AreEqual(expectedResponse.Message, ((DeleteAdminResponse)okResult.Value).Message);
        }

        [TestMethod]
        public void ListAccounts_ReturnsPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResult<UserDto>(new List<UserDto>
            {
                new UserDto { Id = Guid.NewGuid(), FirstName = "Juan", LastName = "Perez", Role = "admin", CreationDate = DateTime.Now },
                new UserDto { Id = Guid.NewGuid(), FirstName = "Maria", LastName = "Garcia", Role = "companyOwner", CreationDate = DateTime.Now }
            }, 2, 1, 10);

            var request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Role = null,
                FullName = null
            };

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(logic => logic.ListAccounts(request)).Returns(pagedResult);

            _userController = new UserController(userLogicMock.Object);

            // Act
            IActionResult result = _userController.ListAccounts(request);
            OkObjectResult okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult, "Expected an OkObjectResult.");
            Assert.AreEqual(pagedResult, okResult.Value);
        }

        [TestMethod]
        public void ListAccounts_InvalidPageNumber_ReturnsBadRequest()
        {
            // Arrange
            ListAccountsRequest request = new ListAccountsRequest
            {
                PageNumber = 0,
                PageSize = 10
            };

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _userController = new UserController(userLogicMock.Object);

            // Act
            IActionResult result = _userController.ListAccounts(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void ListAccounts_InvalidPageSize_ReturnsBadRequest()
        {
            // Arrange
            ListAccountsRequest request = new ListAccountsRequest
            {
                PageNumber = 1,
                PageSize = 0
            };

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _userController = new UserController(userLogicMock.Object);

            // Act
            IActionResult result = _userController.ListAccounts(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void ListNotifications_ValidRequest_ReturnsOkWithNotifications()
        {
            // Arrange
            var token = Guid.NewGuid();
            var request = new ListNotificationsRequest
            {
                DeviceType = "Camera",
                CreationDate = DateTime.Now.AddDays(-7),
                Read = false
            };

            var notifications = new List<NotificationResponse>
            {
                new NotificationResponse
                {
                    Event = "Person detected",
                    DeviceType = "Camera",
                    HardwareId = Guid.NewGuid(),
                    IsRead = false,
                    Date = DateTime.Now.AddMinutes(-10)
                }
            };

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(ul => ul.GetCurrentUser(token))
                .Returns(new HomeOwner("Franco", "Colapinto", "franco@gmail.com", "Password123!", "photo.com"));

            userLogicMock.Setup(ul => ul.GetNotificationsByUser(request, "franco@gmail.com"))
                .Returns(notifications);

            _userController = new UserController(userLogicMock.Object);
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            _userController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();
            var result = _userController.ListNotifications(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result, "Expected OkObjectResult");
            var response = result.Value as List<NotificationResponse>;
            Assert.IsNotNull(response);
            Assert.AreEqual(notifications.Count, response.Count);
            userLogicMock.VerifyAll();
        }

        [TestMethod]
        public void MarkNotificationAsRead_Ok()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            var token = Guid.NewGuid();
            var user = new HomeOwner("Juan", "Perez", "juan.perez@gmail.com", "password123!", "photo") { Id = token };

            var userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(user);
            userLogicMock.Setup(ul => ul.MarkNotificationAsRead(notificationId, user.Email)).Verifiable();

            // Asegúrate de que _userController está inicializado correctamente
            var _userController = new UserController(userLogicMock.Object);

            // Configuración del encabezado de autorización
            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext();
            _userController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            // Act
            var result = _userController.MarkNotificationAsRead(notificationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            userLogicMock.VerifyAll();  // Verifica que los métodos configurados se llamaron correctamente
        }
    }
}

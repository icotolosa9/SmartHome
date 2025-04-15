using BusinessLogic;
using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models.In;
using Models.Out;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SmartHome.Controllers;

namespace TestAPI
{
    [TestClass]
    public class SessionControllerTest
    {
        private SessionController _sessionController;
        private Mock<IUserLogic> _userLogicMock;
        private LoginRequest _loginRequest;
        private User _adminUser;
        private Session _session;

        [TestInitialize]
        public void Setup()
        {
            _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);

            _loginRequest = new LoginRequest
            {
                Email = "admin@example.com",
                Password = "password123!"
            };

            _adminUser = new Admin("Juan", "Perez", "admin@example.com", "password123!");

            _session = new Session
            {
                UserEmail = _adminUser.Email,
                Token = Guid.NewGuid()
            };

            _sessionController = new SessionController(_userLogicMock.Object);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            _userLogicMock.Setup(logic => logic.AuthenticateUser(_loginRequest.Email, _loginRequest.Password))
                          .Returns(_adminUser);
            _userLogicMock.Setup(logic => logic.CreateSession(It.IsAny<Session>())).Returns(_session.Token);

            // Act
            IActionResult result = _sessionController.Login(_loginRequest);
            OkObjectResult okResult = result as OkObjectResult;
            var response = okResult.Value as LoginResponse;

            // Assert
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(_session.Token, response.Token);
            Assert.AreEqual(_adminUser.Email, response.Email);
            Assert.AreEqual(_adminUser.Role, response.Role);

            _userLogicMock.Verify(logic => logic.AuthenticateUser(_loginRequest.Email, _loginRequest.Password), Times.Once);
            _userLogicMock.Verify(logic => logic.CreateSession(It.IsAny<Session>()), Times.Once);
        }

        [TestMethod]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            _userLogicMock.Setup(logic => logic.AuthenticateUser(_loginRequest.Email, _loginRequest.Password))
                          .Returns((User)null);

            // Act
            IActionResult result = _sessionController.Login(_loginRequest);

            // Assert
            UnauthorizedObjectResult unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult.");
            Assert.AreEqual("Email o contraseña incorrectos.", unauthorizedResult.Value);

            _userLogicMock.Verify(logic => logic.AuthenticateUser(_loginRequest.Email, _loginRequest.Password), Times.Once);
            _userLogicMock.Verify(logic => logic.CreateSession(It.IsAny<Session>()), Times.Never);
        }

        [TestMethod]
        public void Login_EmptyRequest_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new LoginRequest { Email = "", Password = "" };

            Mock<IUserLogic> userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _sessionController = new SessionController(userLogicMock.Object);

            // Act
            IActionResult result = _sessionController.Login(invalidRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult), "Expected BadRequestObjectResult.");
            Assert.AreEqual("El email y la contraseña son obligatorios.", ((BadRequestObjectResult)result).Value);
        }
    }
}
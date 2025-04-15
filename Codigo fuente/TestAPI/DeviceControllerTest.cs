using BusinessLogic;
using Domain;
using IBusinessLogic.Exceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models.In;
using Models.Out;
using System;
using System.Collections.Generic;
using SmartHome.Controllers;
using Microsoft.AspNetCore.Http;
using IImporter;

namespace TestAPI
{
    [TestClass]
    public class DeviceControllerTest
    {
        private Mock<IDeviceLogic> _deviceLogicMock;
        private Mock<IUserLogic> _userLogicMock;
        private Mock<IActionLogic> _actionLogicMock;
        private DeviceController _deviceController;

        private CreateCameraRequest _createCameraRequest;
        private CreateWindowSensorRequest _createWindowSensorRequest;
        private CreateSmartLampRequest _createSmartLampRequest;
        private CreateMotionSensorRequest _createMotionSensorRequest;

        private const string UserEmail = "john.doe@gmail.com";
        private CompanyOwner _owner;
        private Company _company;

        [TestInitialize]
        public void Setup()
        {
            _deviceLogicMock = new Mock<IDeviceLogic>(MockBehavior.Strict);
            _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _actionLogicMock = new Mock<IActionLogic>(MockBehavior.Strict);
            _deviceController = new DeviceController(_deviceLogicMock.Object, _userLogicMock.Object, _actionLogicMock.Object);

            var httpContext = new DefaultHttpContext();
            _deviceController.ControllerContext.HttpContext = httpContext;

            _createCameraRequest = new CreateCameraRequest
            {
                Name = "Camara de seguridad",
                ModelNumber = "CAM123",
                Description = "Cámara para interiores y exteriores",
                Photographs = new List<string> { "photo1.jpg", "photo2.jpg" },
                IndoorUse = true,
                OutdoorUse = true,
                MotionDetection = true,
                PersonDetection = true
            };

            _createWindowSensorRequest = new CreateWindowSensorRequest
            {
                Name = "Sensor de ventana",
                ModelNumber = "SEN123",
                Description = "Sensor para apertura y cierre",
                Photographs = new List<string> { "photo1.jpg", "photo2.jpg" }
            };

            _createSmartLampRequest = new CreateSmartLampRequest
            {
                Name = "Smart Lamp",
                ModelNumber = "SL123",
                Description = "Lámpara inteligente para el hogar",
                Photographs = new List<string> { "lamp1.jpg" }
            };

            _createMotionSensorRequest = new CreateMotionSensorRequest
            {
                Name = "Motion Sensor",
                ModelNumber = "MS123",
                Description = "Sensor de movimiento para seguridad",
                Photographs = new List<string> { "sensor1.jpg" }
            };

            _company = new Company("123456789012", "TechCorp", "logo.png");

            _owner = new CompanyOwner("John", "Doe", UserEmail, "password123!");
            _owner.Company = _company;
        }

        [TestMethod]
        public void CreateCamera_ValidRequest_ReturnsCreatedCamera()
        {
            // Arrange
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true);
            camera.Company = _company;
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_owner);
            _deviceLogicMock.Setup(dl => dl.CreateCamera(It.IsAny<Camera>(), UserEmail)).Returns(camera);

            // Act
            _deviceController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString(); 
            CreatedResult result = _deviceController.CreateCamera(_createCameraRequest) as CreatedResult;

            // Assert
            Assert.IsNotNull(result, "Expected CreatedResult.");
            var response = result.Value as CreateCameraResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual("Camara de seguridad", response.Name);
            _deviceLogicMock.VerifyAll();
        }

        [TestMethod]
        public void CreateWindowSensor_ValidRequest_ReturnsCreatedWindowSensor()
        {
            // Arrange
            var windowSensor = new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre",
                                                 new List<string> { "photo1.jpg", "photo2.jpg" });
            windowSensor.Company = _company;
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_owner);
            _deviceLogicMock.Setup(dl => dl.CreateWindowSensor(It.IsAny<WindowSensor>(), UserEmail)).Returns(windowSensor);

            // Act
            _deviceController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString(); 
            CreatedResult result = _deviceController.CreateWindowSensor(_createWindowSensorRequest) as CreatedResult;

            // Assert
            Assert.IsNotNull(result, "Expected CreatedResult.");
            var response = result.Value as CreateWindowSensorResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual("Sensor de ventana", response.Name);
            _deviceLogicMock.VerifyAll();
        }

        [TestMethod]
        public void CreateSmartLamp_ValidRequest_ReturnsCreatedSmartLamp()
        {
            // Arrange
            var smartLamp = new SmartLamp("Smart Lamp", "SL123", "Lámpara inteligente para el hogar",
                                           new List<string> { "lamp1.jpg" });
            smartLamp.Company = _company;
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_owner);
            _deviceLogicMock.Setup(dl => dl.CreateSmartLamp(It.IsAny<SmartLamp>(), UserEmail)).Returns(smartLamp);

            // Act
            _deviceController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();
            CreatedResult result = _deviceController.CreateSmartLamp(_createSmartLampRequest) as CreatedResult;

            // Assert
            Assert.IsNotNull(result, "Expected CreatedResult.");
            var response = result.Value as CreateSmartLampResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual("Smart Lamp", response.Name);
            _deviceLogicMock.VerifyAll();
        }

        // Nueva prueba para crear MotionSensor
        [TestMethod]
        public void CreateMotionSensor_ValidRequest_ReturnsCreatedMotionSensor()
        {
            // Arrange
            var motionSensor = new MotionSensor("Motion Sensor", "MS123", "Sensor de movimiento para seguridad",
                                                new List<string> { "sensor1.jpg" });
            motionSensor.Company = _company;
            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_owner);
            _deviceLogicMock.Setup(dl => dl.CreateMotionSensor(It.IsAny<MotionSensor>(), UserEmail)).Returns(motionSensor);

            // Act
            _deviceController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();
            CreatedResult result = _deviceController.CreateMotionSensor(_createMotionSensorRequest) as CreatedResult;

            // Assert
            Assert.IsNotNull(result, "Expected CreatedResult.");
            var response = result.Value as CreateMotionSensorResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual("Motion Sensor", response.Name);
            _deviceLogicMock.VerifyAll();
        }

        [TestMethod]
        public void ListDevices_ReturnsPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResult<DeviceDto>(new List<DeviceDto>
            {
                new DeviceDto { Name = "Camara de seguridad", Model = "CAM123", MainPicture = "photo1.jpg", Company = "Sony" },
                new DeviceDto { Name = "Sensor de ventana", Model = "SEN123", MainPicture = "photo2.jpg", Company = "Sony" }
            }, 2, 1, 10);

            var request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Name = null,
                Model = null,
                Company = null,
                DeviceType = null
            };

            _deviceLogicMock.Setup(dl => dl.ListDevices(request)).Returns(pagedResult);

            // Act
            IActionResult result = _deviceController.ListDevices(request);
            OkObjectResult okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult, "Expected an OkObjectResult.");
            var returnedResult = okResult.Value as PagedResult<DeviceDto>;
            Assert.IsNotNull(returnedResult);
            Assert.AreEqual(2, returnedResult.Results.Count());
            _deviceLogicMock.VerifyAll();
        }

        [TestMethod]
        public void GetSupportedDeviceTypes_ReturnsSupportedTypes()
        {
            // Arrange
            var supportedTypes = new List<string> { "Camera", "WindowSensor" };
            _deviceLogicMock.Setup(dl => dl.GetSupportedDeviceTypes()).Returns(supportedTypes);

            // Act
            IActionResult result = _deviceController.GetSupportedDeviceTypes();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(supportedTypes, okResult.Value);
        }

        [TestMethod]
        public void DetectPersonCamera_ValidRequest_ReturnsOk()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            string detectedEmail = "detected_person@mail.com";

            var _actionLogicMock = new Mock<IActionLogic>(MockBehavior.Strict);
            _actionLogicMock.Setup(al => al.DetectPersonCamera(hardwareId, detectedEmail)).Verifiable();

            var _deviceController = new DeviceController(_deviceLogicMock.Object, _userLogicMock.Object, _actionLogicMock.Object);

            // Act
            IActionResult result = _deviceController.DetectPersonCamera(hardwareId, detectedEmail);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _actionLogicMock.VerifyAll();
        }

        [TestMethod]
        public void OperOrCloseWindow_ShouldCallActionLogicAndReturnOk()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            bool isOpen = true;

            Mock<IActionLogic> actionLogicMock = new(MockBehavior.Strict);
            actionLogicMock.Setup(logic => logic.OpenOrCloseWindow(hardwareId, isOpen)).Verifiable();

            DeviceController controller = new(null, null, actionLogicMock.Object);

            // Act
            var result = controller.OpenOrCloseWindow(hardwareId, isOpen) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result, "Expected an OkObjectResult.");
            actionLogicMock.VerifyAll();
        }

        [TestMethod]
        public void TurnSmartLampOnOrOff_ShouldCallActionLogicAndReturnOk()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            bool isOn = true;

            Mock<IActionLogic> actionLogicMock = new(MockBehavior.Strict);
            actionLogicMock.Setup(logic => logic.TurnSmartLampOnOrOff(hardwareId, isOn)).Verifiable();

            DeviceController controller = new(null, null, actionLogicMock.Object);

            // Act
            var result = controller.TurnSmartLampOnOrOff(hardwareId, isOn) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result, "Expected an OkObjectResult.");
            actionLogicMock.VerifyAll();
        }

        [TestMethod]
        public void DetectMotionCamera_ValidRequest_ReturnsOk()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            _actionLogicMock.Setup(al => al.DetectMotionCamera(hardwareId)).Verifiable();

            // Act
            IActionResult result = _deviceController.DetectMotionCamera(hardwareId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _actionLogicMock.VerifyAll();
        }

        [TestMethod]
        public void DetectMotionSensor_ValidRequest_ReturnsOk()
        {
            // Arrange
            var hardwareId = Guid.NewGuid();
            _actionLogicMock.Setup(al => al.DetectMotionSensor(hardwareId)).Verifiable();

            // Act
            IActionResult result = _deviceController.DetectMotionSensor(hardwareId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected an OkObjectResult.");
            _actionLogicMock.VerifyAll();
        }

        [TestMethod]
        public void ImportDevices_ImporterNotFound_ReturnsBadRequest()
        {
            // Arrange
            string importerType = "JSON Importer";

            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns((CompanyOwner)null); 

            _deviceLogicMock.Setup(dl => dl.GetAllImporters()).Returns(new List<ImporterInterface>());

            // Act
            _deviceController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";
            var result = _deviceController.ImportDevices(importerType);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Importador no encontrado.", badRequestResult.Value);

            _deviceLogicMock.VerifyAll();
            _userLogicMock.VerifyAll();
        }

        [TestMethod]
        public void ImportDevices_ValidImporter_ReturnsOk()
        {
            // Arrange
            string importerType = "JSON Importer";
            var mockImporter = new Mock<ImporterInterface>();
            var mockDevices = new List<ImportedDevice>
            {
                new ImportedDevice { Id = Guid.NewGuid(), Tipo = "camera", Nombre = "Test Camera", Modelo = "Model X" }
            };

            mockImporter.Setup(i => i.GetName()).Returns(importerType);
            mockImporter.Setup(i => i.ImportDevice()).Returns(mockDevices);

            _deviceLogicMock.Setup(dl => dl.GetAllImporters()).Returns(new List<ImporterInterface> { mockImporter.Object });

            var token = Guid.NewGuid();
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_owner);

            _deviceLogicMock.Setup(dl => dl.ImportDevices(It.IsAny<List<ImportedDevice>>(), UserEmail));

            // Act
            _deviceController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";
            var result = _deviceController.ImportDevices(importerType);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            _deviceLogicMock.Verify(dl => dl.ImportDevices(mockDevices, UserEmail), Times.Once);

            _deviceLogicMock.VerifyAll();
            _userLogicMock.VerifyAll();
            mockImporter.VerifyAll();
        }
    }
}

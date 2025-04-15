using BusinessLogic;
using Domain;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.In;
using Models.Out;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBusinessLogic
{
    [TestClass]
    public class DeviceLogicTest
    {
        private DeviceLogic _deviceLogic;
        private Mock<IDeviceRepository> _deviceRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ICompanyRepository> _companyRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _companyRepositoryMock = new Mock<ICompanyRepository>(MockBehavior.Strict);
            _deviceLogic = new DeviceLogic(_deviceRepositoryMock.Object, _userRepositoryMock.Object, _companyRepositoryMock.Object);
        }

        [TestMethod]
        public void CreateCamera_ValidCamera_ReturnsCreatedCamera()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            {
                Company = new Company("123456789012", "TechCorp", "logo.png"),
                CompanyId = companyId
            };

            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true);

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(camera.Name, camera.ModelNumber)).Returns((Device)null);
            _deviceRepositoryMock.Setup(repo => repo.Save(camera)).Returns(camera);
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(owner.Company); 

            // Act
            var result = _deviceLogic.CreateCamera(camera, owner.Email);

            // Assert
            Assert.AreEqual(camera, result);
            _deviceRepositoryMock.Verify(repo => repo.Save(camera), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(owner.Email), Times.Once);
            _companyRepositoryMock.Verify(repo => repo.GetCompanyById(companyId), Times.Once); 
        }


        [TestMethod]
        [ExpectedException(typeof(IncompleteCompanyOwnerAccountException))]
        public void CreateCamera_NoCompanyOwner_ThrowsException()
        {
            // Arrange
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!") { Company = null };
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores", new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true);

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);

            // Act
            _deviceLogic.CreateCamera(camera, owner.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(DeviceAlreadyExistsException))]
        public void CreateCamera_DeviceAlreadyExists_ThrowsException()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            {
                Company = new Company("123456789012", "TechCorp", "logo.png"),
                CompanyId = companyId
            };

            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true);

            // Configuración del mock
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(camera.Name, camera.ModelNumber)).Returns(camera); 
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(owner.Company); 

            // Act
            _deviceLogic.CreateCamera(camera, owner.Email);
        }

        [TestMethod]
        public void CreateWindowSensor_ValidSensor_ReturnsCreatedWindowSensor()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            {
                Company = new Company("123456789012", "TechCorp", "logo.png"),
                CompanyId = companyId
            };

            var windowSensor = new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre",
                                                new List<string> { "photo1.jpg", "photo2.jpg" });

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(windowSensor.Name, windowSensor.ModelNumber)).Returns((Device)null);
            _deviceRepositoryMock.Setup(repo => repo.Save(windowSensor)).Returns(windowSensor);
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(owner.Company);

            // Act
            var result = _deviceLogic.CreateWindowSensor(windowSensor, owner.Email);

            // Assert
            Assert.AreEqual(windowSensor, result);
            _deviceRepositoryMock.Verify(repo => repo.Save(windowSensor), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(owner.Email), Times.Once);
            _companyRepositoryMock.Verify(repo => repo.GetCompanyById(companyId), Times.Once); 
        }

        [TestMethod]
        [ExpectedException(typeof(IncompleteCompanyOwnerAccountException))]
        public void CreateWindowSensor_NoCompanyOwner_ThrowsException()
        {
            // Arrange
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!") { Company = null };
            var windowSensor = new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre", new List<string> { "photo1.jpg", "photo2.jpg" });

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);

            // Act
            _deviceLogic.CreateWindowSensor(windowSensor, owner.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(DeviceAlreadyExistsException))]
        public void CreateWindowSensor_DeviceAlreadyExists_ThrowsException()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            {
                Company = new Company("123456789012", "TechCorp", "logo.png"),
                CompanyId = companyId
            };

            var windowSensor = new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre",
                                                 new List<string> { "photo1.jpg", "photo2.jpg" });

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(windowSensor.Name, windowSensor.ModelNumber)).Returns(windowSensor); 
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(owner.Company); 

            // Act
            _deviceLogic.CreateWindowSensor(windowSensor, owner.Email);
        }

        [TestMethod]
        public void CreateSmartLamp_ValidSmartLamp_ReturnsCreatedSmartLamp()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            {
                Company = new Company("123456789012", "TechCorp", "logo.png"),
                CompanyId = companyId
            };

            var smartLamp = new SmartLamp("Smart Lamp", "SL123", "Lámpara inteligente para el hogar",
                                           new List<string> { "lamp1.jpg" });

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(smartLamp.Name, smartLamp.ModelNumber)).Returns((Device)null);
            _deviceRepositoryMock.Setup(repo => repo.Save(smartLamp)).Returns(smartLamp);
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(owner.Company);

            // Act
            var result = _deviceLogic.CreateSmartLamp(smartLamp, owner.Email);

            // Assert
            Assert.AreEqual(smartLamp, result);
            _deviceRepositoryMock.Verify(repo => repo.Save(smartLamp), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(owner.Email), Times.Once);
        }

        [TestMethod]
        public void CreateMotionSensor_ValidMotionSensor_ReturnsCreatedMotionSensor()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            {
                Company = new Company("123456789012", "TechCorp", "logo.png"),
                CompanyId = companyId
            };

            var motionSensor = new MotionSensor("Motion Sensor", "MS123", "Sensor de movimiento para seguridad",
                                                new List<string> { "sensor1.jpg" });

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceByNameAndModel(motionSensor.Name, motionSensor.ModelNumber)).Returns((Device)null);
            _deviceRepositoryMock.Setup(repo => repo.Save(motionSensor)).Returns(motionSensor);
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(owner.Company);

            // Act
            var result = _deviceLogic.CreateMotionSensor(motionSensor, owner.Email);

            // Assert
            Assert.AreEqual(motionSensor, result);
            _deviceRepositoryMock.Verify(repo => repo.Save(motionSensor), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(owner.Email), Times.Once);
        }

        [TestMethod]
        public void ListDevices_ReturnsPagedResult()
        {
            // Arrange
            var company = new Company("123456789012", "Sony", "logoUrl");

            List<Device> devices = new List<Device>
            {
                new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores", new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
                {
                    Company = company
                },
                new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre", new List<string> { "photo1.jpg", "photo2.jpg" })
                {
                    Company = company
                }
            };

            _deviceRepositoryMock.Setup(repo => repo.GetPagedDevices(It.IsAny<ListDevicesRequest>()))
                .Returns(new PagedResult<Device>(devices, devices.Count, 1, devices.Count));

            ListDevicesRequest request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 2
            };

            // Act
            PagedResult<DeviceDto> result = _deviceLogic.ListDevices(request);

            // Assert
            Assert.AreEqual(2, result.Results.Count());
            Assert.AreEqual("Camara de seguridad", result.Results.First().Name);
            _deviceRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void ListDevices_WithNoResults_ReturnsEmptyPagedResult()
        {
            // Arrange
            _deviceRepositoryMock.Setup(repo => repo.GetPagedDevices(It.IsAny<ListDevicesRequest>()))
                .Returns(new PagedResult<Device>(new List<Device>(), 0, 1, 10));

            ListDevicesRequest request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            PagedResult<DeviceDto> result = _deviceLogic.ListDevices(request);

            // Assert
            Assert.AreEqual(0, result.Results.Count());
            _deviceRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetSupportedDeviceTypes_ReturnsSupportedTypes()
        {
            // Arrange
            var supportedTypes = new List<string> { "Camera", "WindowSensor" };
            _deviceRepositoryMock.Setup(repo => repo.GetSupportedDeviceTypes()).Returns(supportedTypes);

            // Act
            var result = _deviceLogic.GetSupportedDeviceTypes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("Camera"));
            Assert.IsTrue(result.Contains("WindowSensor"));

            _deviceRepositoryMock.Verify(repo => repo.GetSupportedDeviceTypes(), Times.Once);
        }

        [TestMethod]
        public void ImportDevices_CompanyOwnerWithoutCompanyId_ThrowsIncompleteCompanyOwnerAccountException()
        {
            // Arrange
            string emailCompanyOwner = "owner@example.com";
            var importedDevices = new List<ImportedDevice>
            {
                new ImportedDevice { Tipo = "camera", Nombre = "Camera 1", Modelo = "Model 1" }
            };

            var owner = new CompanyOwner("John", "Doe", emailCompanyOwner, "password123!") { CompanyId = null };

            _userRepositoryMock.Setup(ur => ur.GetUserByEmail(emailCompanyOwner)).Returns(owner);

            // Act & Assert
            Assert.ThrowsException<IncompleteCompanyOwnerAccountException>(() =>
                _deviceLogic.ImportDevices(importedDevices, emailCompanyOwner));
        }

        [TestMethod]
        public void ImportDevices_ValidOwner_SavesTransformedDevices()
        {
            // Arrange
            string emailCompanyOwner = "owner@example.com";
            var importedDevices = new List<ImportedDevice>
            {
                new ImportedDevice
                {
                    Tipo = "camera",
                    Nombre = "Camera 1",
                    Modelo = "Model 1",
                    Fotos = new List<Photo>
                    {
                        new Photo { Path = "photo1.jpg", EsPrincipal = true }
                    }
                }
            };

            var owner = new CompanyOwner("John", "Doe", emailCompanyOwner, "password123!")
            {
                CompanyId = Guid.NewGuid()  
            };
  
            _userRepositoryMock.Setup(ur => ur.GetUserByEmail(emailCompanyOwner)).Returns(owner);
 
            _deviceRepositoryMock.Setup(dr => dr.Save(It.IsAny<Device>())).Returns(It.IsAny<Device>());

            // Act
            _deviceLogic.ImportDevices(importedDevices, emailCompanyOwner);

            // Assert
            _deviceRepositoryMock.Verify(dr => dr.Save(It.IsAny<Device>()), Times.Once);
        }
    }
}

using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.In;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataAccess
{
    [TestClass]
    public class DeviceRepositoryTest
    {
        private Mock<SmartHomeContext> _mockContext;
        private Mock<DbSet<Device>> _mockDbSet;
        private DeviceRepository _deviceRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<SmartHomeContext>();
            _mockDbSet = new Mock<DbSet<Device>>();
            _deviceRepository = new DeviceRepository(_mockContext.Object);
        }

        [TestMethod]
        public void Save_ValidDevice_ReturnsSavedDevice()
        {
            // Arrange
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true);

            _mockDbSet.Setup(m => m.Add(It.IsAny<Device>())).Callback<Device>(d => d.Id = Guid.NewGuid());
            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            // Act
            var result = _deviceRepository.Save(camera);

            // Assert
            Assert.IsNotNull(result, "Expected the device to be saved and returned.");
            Assert.AreEqual("Camara de seguridad", result.Name);
            Assert.AreEqual("CAM123", result.ModelNumber);
            _mockDbSet.Verify(m => m.Add(It.IsAny<Device>()), Times.Once);
            _mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetDeviceByNameAndModel_ExistingDevice_ReturnsDevice()
        {
            // Arrange
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
            {
                Id = Guid.NewGuid()  
            };

            var devices = new List<Device> { camera }.AsQueryable();

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            // Act
            var result = _deviceRepository.GetDeviceByNameAndModel("Camara de seguridad", "CAM123");

            // Assert
            Assert.IsNotNull(result, "Expected to find the saved device.");
            Assert.AreEqual(camera.Name, result.Name);
            Assert.AreEqual(camera.ModelNumber, result.ModelNumber);
        }

        [TestMethod]
        public void GetDeviceByNameAndModel_NonExistingDevice_ReturnsNull()
        {
            // Arrange
            var devices = new List<Device>().AsQueryable();

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            // Act
            var result = _deviceRepository.GetDeviceByNameAndModel("NonExistingDevice", "MODEL999");

            // Assert
            Assert.IsNull(result, "Expected no device to be found.");
        }

        [TestMethod]
        public void GetPagedDevices_ReturnsPagedResult()
        {
            // Arrange
            var company = new Company("123456789012", "Sony", "logoUrl");
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
            {
                Company = company
            };
            var windowSensor = new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre",
                                                 new List<string> { "photo1.jpg", "photo2.jpg" })
            {
                Company = company
            };

            var devices = new List<Device> { camera, windowSensor }.AsQueryable();

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            var request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = _deviceRepository.GetPagedDevices(request);

            // Assert
            Assert.AreEqual(2, result.Results.Count());
            Assert.AreEqual(2, result.TotalCount);
        }

        [TestMethod]
        public void GetPagedDevices_WithNameFilter_ReturnsFilteredResult()
        {
            // Arrange
            var company = new Company("123456789012", "Sony", "logoUrl");
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
            {
                Company = company
            };

            var devices = new List<Device> { camera }.AsQueryable();

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            var request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Name = "Camara de seguridad"
            };

            // Act
            var result = _deviceRepository.GetPagedDevices(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual("Camara de seguridad", result.Results.First().Name);
        }

        [TestMethod]
        public void GetPagedDevices_WithModelFilter_ReturnsFilteredResult()
        {
            // Arrange
            var company = new Company("123456789012", "Sony", "logoUrl");
            var camera = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
            {
                Company = company
            };

            var devices = new List<Device> { camera }.AsQueryable();

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            var request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Model = "CAM123"
            };

            // Act
            var result = _deviceRepository.GetPagedDevices(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual("CAM123", result.Results.First().ModelNumber);
        }

        [TestMethod]
        public void GetPagedDevices_WithCompanyFilter_ReturnsFilteredResult()
        {
            // Arrange
            var companySony = new Company("123456789012", "Sony", "logoUrl");
            var companySamsung = new Company("987654321098", "Samsung", "logoUrl");

            var sonyDevice = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                        new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
            {
                Company = companySony
            };

            var samsungDevice = new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre",
                                                  new List<string> { "photo1.jpg", "photo2.jpg" })
            {
                Company = companySamsung
            };

            var devices = new List<Device> { sonyDevice, samsungDevice }.AsQueryable();

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            var request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Company = "Sony"
            };

            // Act
            var result = _deviceRepository.GetPagedDevices(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
            Assert.AreEqual("Camara de seguridad", result.Results.First().Name);
            Assert.AreEqual("Sony", result.Results.First().Company.Name);
        }

        [TestMethod]
        public void GetPagedDevices_WithNoResults_ReturnsEmptyPagedResult()
        {
            // Arrange
            var devices = new List<Device>().AsQueryable(); 

            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            _mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            _mockContext.Setup(ctx => ctx.Devices).Returns(_mockDbSet.Object);

            var request = new ListDevicesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = _deviceRepository.GetPagedDevices(request);

            // Assert
            Assert.AreEqual(0, result.Results.Count());
            Assert.AreEqual(0, result.TotalCount);
        }

        [TestMethod]
        public void GetSupportedDeviceTypes_ReturnsSupportedTypes()
        {
            // Act
            var result = _deviceRepository.GetSupportedDeviceTypes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count); 
            Assert.IsTrue(result.Contains(nameof(Camera)));
            Assert.IsTrue(result.Contains(nameof(WindowSensor)));
            Assert.IsTrue(result.Contains(nameof(MotionSensor)));
            Assert.IsTrue(result.Contains(nameof(SmartLamp)));
        }

        [TestMethod]
        public void GetDeviceById_ExistingDeviceId_ShouldReturnDevice()
        {
            var company = new Company("123456789012", "Sony", "logoUrl");
            var expectedDevice = new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                    new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
            {
                Company = company
            };

            var devices = new List<Device>
            {
                expectedDevice,
                new Camera("Camara de seguridad", "CAM123", "Cámara para interiores y exteriores",
                                        new List<string> { "photo1.jpg", "photo2.jpg" }, true, true, true, true)
                {
                Company = company
                },

                new WindowSensor("Sensor de ventana", "SEN123", "Sensor para apertura y cierre",
                                                  new List<string> { "photo1.jpg", "photo2.jpg" })
                {
                Company = company
                }
            }.AsQueryable();

            Mock<DbSet<Device>> mockDbSet = new();
            mockDbSet.As<IQueryable<Device>>().Setup(m => m.Provider).Returns(devices.Provider);
            mockDbSet.As<IQueryable<Device>>().Setup(m => m.Expression).Returns(devices.Expression);
            mockDbSet.As<IQueryable<Device>>().Setup(m => m.ElementType).Returns(devices.ElementType);
            mockDbSet.As<IQueryable<Device>>().Setup(m => m.GetEnumerator()).Returns(devices.GetEnumerator());

            Mock<SmartHomeContext> mockContext = new Mock<SmartHomeContext>();
            mockContext.Setup(c => c.Devices).Returns(mockDbSet.Object);

            DeviceRepository deviceRepository = new DeviceRepository(mockContext.Object);

            Device? result = deviceRepository.GetDeviceById(expectedDevice.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDevice.Id, result.Id);
            Assert.AreEqual(expectedDevice.Name, result.Name);
            mockContext.Verify(c => c.Devices, Times.Once);
        }
    }
}
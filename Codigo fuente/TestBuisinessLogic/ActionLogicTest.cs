using BusinessLogic;
using Domain;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace TestBusinessLogic
{
    [TestClass]
    public class ActionLogicTest
    {
        private Mock<IDeviceRepository> _deviceRepositoryMock;
        private Mock<IHomeRepository> _homeRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private ActionLogic _actionLogic;
        private Camera _camera;
        private MotionSensor _motionSensor;
        private Home _home;
        private HomeDevice _homeDevice;
        private User _user;
        private Guid _hardwareId;
        private Guid _homeId;

        [TestInitialize]
        public void Setup()
        {
            _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);
            _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

            _actionLogic = new ActionLogic(_deviceRepositoryMock.Object, _homeRepositoryMock.Object, _userRepositoryMock.Object);

            _hardwareId = Guid.NewGuid();
            _homeId = Guid.NewGuid();

            _camera = new Camera("Camera", "CAM123", "Security Camera", new List<string> { "photo1.jpg" }, true, true, true, true);

            _motionSensor = new MotionSensor("MotionSensor", "MS3.0", "Motion Sensor", new List<string> { "photo1.jpg" });

            _home = new Home
            {
                Id = _homeId,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission
                    {
                        HomeOwnerId = Guid.NewGuid(),
                        IsNotificationEnabled = true
                    }
                }
            };

            _homeDevice = new HomeDevice
            {
                HardwareId = _hardwareId,
                HomeId = _homeId,
                DeviceId = _camera.Id,
                Connected = true
            };

            _user = new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "homeOwner");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDevice))]
        public void DetectPersonCamera_DeviceNotFound_ThrowsException()
        {
            // Arrange
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns((HomeDevice)null);

            // Act
            _actionLogic.DetectPersonCamera(_hardwareId, _user.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DetectPersonCamera_DeviceNotConnected_ThrowsException()
        {
            // Arrange
            _homeDevice.Connected = false;
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns(_homeDevice);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceById(_homeDevice.DeviceId)).Returns(_camera);

            // Act
            _actionLogic.DetectPersonCamera(_hardwareId, _user.Email);
        }

        [TestMethod]
        public void DetectPersonCamera_ValidRequest_AddsNotificationToMembers()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();

            HomeOwner homeOwner = new HomeOwner("OwnerFirstName", "OwnerLastName", "owner@example.com", "password123!", "photo");
            HomeOwner member = new HomeOwner("MemberFirstName", "MemberLastName", "member@example.com", "password123!", "photo");
            Home home = new Home
            {
                Id = homeId,
                HomeOwnerId = homeOwner.Id,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = member.Id, IsNotificationEnabled = true }
                }
            };

            Camera camera = new Camera("Camera", "CAM123", "Security Camera", new List<string> { "photo1.jpg" }, true, true, true, true);
            HomeDevice homeDevice = new HomeDevice
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                HomeId = homeId,
                Connected = true
            };

            var homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);

            var deviceRepoMock = new Mock<IDeviceRepository>();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(camera);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserById(homeOwner.Id)).Returns(homeOwner);
            userRepoMock.Setup(repo => repo.GetUserById(member.Id)).Returns(member);

            userRepoMock.Setup(repo => repo.SaveNotification(It.IsAny<User>(), It.IsAny<Notification>())).Verifiable();

            var actionLogic = new ActionLogic(deviceRepoMock.Object, homeRepoMock.Object, userRepoMock.Object);

            // Act
            actionLogic.DetectPersonCamera(hardwareId, "personDetected@example.com");

            // Assert
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == homeOwner.Id), It.IsAny<Notification>()), Times.Once);
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == member.Id), It.IsAny<Notification>()), Times.Once);
        }

        [TestMethod]
        public void OpenOrCloseWindow_WhenClosed_ShouldOpenWindowAndSendNotification()
        {
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();

            HomeDevice homeDevice = new()
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                HomeId = homeId,
                IsOpenOrOn = false, 
                Connected = true
            };

            WindowSensor windowSensor = new("Window Sensor", "Model123", "Description", new List<string>());
            windowSensor.IsOpen = false;

            Home home = new()
            {
                Id = homeId,
                HomeOwnerId = ownerId,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = ownerId, IsNotificationEnabled = true }
                }
            };

            Mock<IHomeRepository> homeRepoMock = new();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);

            Mock<IDeviceRepository> deviceRepoMock = new();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(windowSensor);

            Mock<IUserRepository> userRepoMock = new();
            userRepoMock.Setup(repo => repo.GetUserById(ownerId)).Returns(new HomeOwner("John", "Doe", "john.doe@mail.com", "password123!", "homeOwner"));

            ActionLogic actionLogic = new ActionLogic(deviceRepoMock.Object, homeRepoMock.Object, userRepoMock.Object);

            actionLogic.OpenOrCloseWindow(hardwareId, true); 

            Assert.IsTrue(homeDevice.IsOpenOrOn); 
            homeRepoMock.Verify(repo => repo.UpdateHomeDevice(homeDevice), Times.Once); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenOrCloseWindow_WhenAlreadyInDesiredState_ShouldThrowInvalidOperationException()
        {
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            HomeDevice homeDevice = new()
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                IsOpenOrOn = true, 
                Connected = true
            };

            WindowSensor windowSensor = new("Window Sensor", "Model123", "Description", new List<string>());

            Mock<IHomeRepository> homeRepoMock = new();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);

            Mock<IDeviceRepository> deviceRepoMock = new();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(windowSensor);

            ActionLogic actionLogic = new(deviceRepoMock.Object, homeRepoMock.Object, null);

            actionLogic.OpenOrCloseWindow(hardwareId, true); 
        }

        [TestMethod]
        public void OpenOrCloseWindow_WhenActionTriggered_ShouldNotifyOnlyMembersWithNotificationsEnabled()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();

            HomeDevice homeDevice = new()
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                HomeId = homeId,
                IsOpenOrOn = false,
                Connected = true
            };

            WindowSensor windowSensor = new("Window Sensor", "Model123", "Description", new List<string>());

            HomeOwner notifiedUser = new("Owner", "Notified", "notified.owner@mail.com", "password123!", "ownerPhoto") { Id = ownerId };
            HomeOwner nonNotifiedUser = new("NonNotified", "Doe", "nonnotified@mail.com", "password123!", "photo") { Id = Guid.NewGuid() };

            Home home = new()
            {
                Id = homeId,
                HomeOwnerId = ownerId,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = nonNotifiedUser.Id, IsNotificationEnabled = false }
                }
            };

            var homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);

            var deviceRepoMock = new Mock<IDeviceRepository>();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(windowSensor);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserById(ownerId)).Returns(notifiedUser);
            userRepoMock.Setup(repo => repo.GetUserById(nonNotifiedUser.Id)).Returns(nonNotifiedUser);

            userRepoMock.Setup(repo => repo.SaveNotification(notifiedUser, It.IsAny<Notification>())).Verifiable();

            var actionLogic = new ActionLogic(deviceRepoMock.Object, homeRepoMock.Object, userRepoMock.Object);

            // Act
            actionLogic.OpenOrCloseWindow(hardwareId, true);

            // Assert
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == ownerId), It.IsAny<Notification>()), Times.Once);
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == nonNotifiedUser.Id), It.IsAny<Notification>()), Times.Never);
        }

        [TestMethod]
        public void TurnSmartLampOnOrOff_WhenOff_ShouldTurnOnAndNotifyMembersWithNotificationsEnabled()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();

            var homeDevice = new HomeDevice
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                HomeId = homeId,
                IsOpenOrOn = false,
                Connected = true
            };

            SmartLamp smartLamp = new("Smart Lamp", "Model123", "Description", new List<string>());

            HomeOwner notifiedUser = new("Owner", "Notified", "notified.owner@mail.com", "password123!", "ownerPhoto") { Id = ownerId };
            HomeOwner nonNotifiedUser = new("NonNotified", "Doe", "nonnotified@mail.com", "password123!", "photo") { Id = Guid.NewGuid() };

            Home home = new()
            {
                Id = homeId,
                HomeOwnerId = ownerId,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = nonNotifiedUser.Id, IsNotificationEnabled = false }
                }
            };

            var homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            homeRepoMock.Setup(repo => repo.UpdateHomeDevice(homeDevice)).Verifiable();

            var deviceRepoMock = new Mock<IDeviceRepository>();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(smartLamp);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserById(ownerId)).Returns(notifiedUser);
            userRepoMock.Setup(repo => repo.GetUserById(nonNotifiedUser.Id)).Returns(nonNotifiedUser);

            var actionLogic = new ActionLogic(deviceRepoMock.Object, homeRepoMock.Object, userRepoMock.Object);

            // Act
            actionLogic.TurnSmartLampOnOrOff(hardwareId, true);

            // Assert
            Assert.IsTrue(homeDevice.IsOpenOrOn);
            homeRepoMock.Verify(repo => repo.UpdateHomeDevice(homeDevice), Times.Once);
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == ownerId), It.IsAny<Notification>()), Times.Once);
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == nonNotifiedUser.Id), It.IsAny<Notification>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TurnSmartLampOnOrOff_WhenAlreadyOn_ShouldThrowInvalidOperationException()
        {
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            HomeDevice homeDevice = new HomeDevice
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                IsOpenOrOn = true, 
                Connected = true
            };

            SmartLamp smartLamp = new("Smart Lamp", "Model123", "Description", new List<string>());

            Mock<IHomeRepository> homeRepoMock = new();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);

            Mock<IDeviceRepository> deviceRepoMock = new();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(smartLamp);

            ActionLogic actionLogic = new( deviceRepoMock.Object,homeRepoMock.Object, null);

            actionLogic.TurnSmartLampOnOrOff(hardwareId, true); 
        }



        [TestMethod]
        public void DetectMotionCamera_DeviceNotFound_ThrowsException()
        {
            // Arrange
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns((HomeDevice)null);

            // Act
            // Assert
            Assert.ThrowsException<NotFoundDevice>(() => _actionLogic.DetectMotionCamera(_hardwareId));
        }

        [TestMethod]
        public void DetectMotionCamera_InvalidDeviceType_ThrowsException()
        {
            // Arrange
            var nonCameraDevice = new WindowSensor("windowSensor", "DEV123", "Non Camera Device", ["sensorphoto.com"]);
            _homeDevice.DeviceId = nonCameraDevice.Id;
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns(_homeDevice);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceById(nonCameraDevice.Id)).Returns(nonCameraDevice);

            // Act
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => _actionLogic.DetectMotionCamera(_hardwareId));
        }

        [TestMethod]
        public void DetectMotionCamera_DeviceNotConnected_ThrowsException()
        {
            // Arrange
            _homeDevice.Connected = false;
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns(_homeDevice);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceById(_homeDevice.DeviceId)).Returns(_camera);

            // Act
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => _actionLogic.DetectMotionCamera(_hardwareId));
        }

        [TestMethod]
        public void DetectMotionCamera_ValidRequest_AddsNotificationToMembers()
        {
            // Arrange
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();

            HomeOwner homeOwner = new HomeOwner("OwnerFirstName", "OwnerLastName", "owner@example.com", "password123!", "photo");
            HomeOwner member = new HomeOwner("MemberFirstName", "MemberLastName", "member@example.com", "password123!", "photo");
            Home home = new Home
            {
                Id = homeId,
                HomeOwnerId = homeOwner.Id,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = member.Id, IsNotificationEnabled = true }
                }
            };

            Camera camera = new Camera("Camera", "CAM123", "Security Camera", new List<string> { "photo1.jpg" }, true, true, true, true);
            HomeDevice homeDevice = new HomeDevice
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                HomeId = homeId,
                IsOpenOrOn = false,
                Connected = true
            };

            var homeRepoMock = new Mock<IHomeRepository>();
            homeRepoMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            homeRepoMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);
            homeRepoMock.Setup(repo => repo.UpdateHomeDevice(It.IsAny<HomeDevice>())).Verifiable(); 

            var deviceRepoMock = new Mock<IDeviceRepository>();
            deviceRepoMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(camera);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetUserById(homeOwner.Id)).Returns(homeOwner); 
            userRepoMock.Setup(repo => repo.GetUserById(member.Id)).Returns(member);  

            userRepoMock.Setup(repo => repo.SaveNotification(It.IsAny<User>(), It.IsAny<Notification>())).Verifiable(); 

            var actionLogic = new ActionLogic(deviceRepoMock.Object, homeRepoMock.Object, userRepoMock.Object);

            // Act
            actionLogic.DetectMotionCamera(hardwareId);

            // Assert
            homeRepoMock.Verify(repo => repo.UpdateHomeDevice(homeDevice), Times.Never);
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == homeOwner.Id), It.IsAny<Notification>()), Times.Once);
            userRepoMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == member.Id), It.IsAny<Notification>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundDevice))]
        public void DetectMotionSensor_DeviceNotFound_ThrowsException()
        {
            // Arrange
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns((HomeDevice)null);

            // Act
            _actionLogic.DetectMotionSensor(_hardwareId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DetectMotionSensor_DeviceNotConnected_ThrowsException()
        {
            // Arrange
            _homeDevice.Connected = false;
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns(_homeDevice);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceById(_homeDevice.DeviceId)).Returns(_motionSensor);

            // Act
            _actionLogic.DetectMotionSensor(_hardwareId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DetectMotionSensor_IncorrectDeviceType_ThrowsException()
        {
            // Arrange
            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(_hardwareId)).Returns(_homeDevice);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceById(_homeDevice.DeviceId)).Returns(_camera); 

            // Act
            _actionLogic.DetectMotionSensor(_hardwareId);
        }

        [TestMethod]
        public void DetectMotionSensor_ValidRequest_AddsNotificationToMembers()
        {
            Guid hardwareId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();
            Guid homeId = Guid.NewGuid();

            HomeOwner homeOwner = new HomeOwner("OwnerFirstName", "OwnerLastName", "owner@example.com", "password123!", "photo");
            HomeOwner member = new HomeOwner("MemberFirstName", "MemberLastName", "member@example.com", "password123!", "photo");
            Home home = new Home
            {
                Id = homeId,
                HomeOwnerId = homeOwner.Id,
                MemberPermissions = new List<HomeOwnerPermission>
                {
                    new HomeOwnerPermission { HomeOwnerId = member.Id, IsNotificationEnabled = true } 
                }
            };

            MotionSensor motionSensor = new MotionSensor("MotionSensor", "MOT123", "Motion Detector", new List<string> { "sensor1.jpg" });
            HomeDevice homeDevice = new HomeDevice
            {
                HardwareId = hardwareId,
                DeviceId = deviceId,
                HomeId = homeId,
                IsOpenOrOn = false,
                Connected = true
            };

            _homeRepositoryMock.Setup(repo => repo.GetHomeDeviceById(hardwareId)).Returns(homeDevice);
            _deviceRepositoryMock.Setup(repo => repo.GetDeviceById(deviceId)).Returns(motionSensor);
            _homeRepositoryMock.Setup(repo => repo.GetHomeById(homeId)).Returns(home);

            _userRepositoryMock.Setup(repo => repo.GetUserById(homeOwner.Id)).Returns(homeOwner);
            _userRepositoryMock.Setup(repo => repo.GetUserById(member.Id)).Returns(member); 

            _userRepositoryMock.Setup(repo => repo.SaveNotification(It.IsAny<User>(), It.IsAny<Notification>())).Verifiable();

            // Act
            _actionLogic.DetectMotionSensor(hardwareId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == homeOwner.Id), It.IsAny<Notification>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.SaveNotification(It.Is<User>(u => u.Id == member.Id), It.IsAny<Notification>()), Times.Once);
        }
    }
}

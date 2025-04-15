using DataAccess.Repositories;
using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Models.In;
using Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ActionLogic : IActionLogic
    {
        private IDeviceRepository _deviceRepository;
        private IHomeRepository _homeRepository;
        private IUserRepository _userRepository;
        public ActionLogic(IDeviceRepository deviceRepository, IHomeRepository homeRepository, IUserRepository userRepository)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _homeRepository = homeRepository;
        }

        public void DetectPersonCamera(Guid hardwareId, string email)
        {
            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }

            Device device = _deviceRepository.GetDeviceById(homeDevice.DeviceId);
            if (device.DeviceType != "Camera")
            {
                throw new InvalidOperationException("No concuerda el tipo de dispositivo con la acción deseada");
            }
            Camera camera = (Camera)device;

            if (!homeDevice.Connected)
            {
                throw new InvalidOperationException("El dispositivo no está conectado.");
            }

            User user = _userRepository.GetUserByEmail(email);
            string action;
            if (user == null)
            {
                action = camera.UnknownPersonDetection();
            }
            else
            {
                action = camera.PersonDetection(email);
            }

            Home home = _homeRepository.GetHomeById(homeDevice.HomeId);

            foreach (var memberPermission in home.MemberPermissions)
            {
                if (memberPermission.IsNotificationEnabled)
                {
                    Notification notification = new Notification(action, hardwareId, "Camera");
                    User member = _userRepository.GetUserById(memberPermission.HomeOwnerId);
                    member.Notifications.Add(notification);
                    _userRepository.SaveNotification(member, notification);
                }
            }
            Notification notificationOwner = new Notification(action, hardwareId, "Motion Sensor");
            User owner = _userRepository.GetUserById(home.HomeOwnerId);
            owner.Notifications.Add(notificationOwner);
            _userRepository.SaveNotification(owner, notificationOwner);
        }

        public void OpenOrCloseWindow(Guid hardwareId, bool open)
        {
            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }

            Device device = _deviceRepository.GetDeviceById(homeDevice.DeviceId);
            if (device.DeviceType != "Window Sensor")
            {
                throw new InvalidOperationException("No concuerda el tipo de dispositivo con la acción deseada");
            }

            WindowSensor windowSensor = (WindowSensor)device;

            if (!homeDevice.Connected)
            {
                throw new InvalidOperationException("El dispositivo no está conectado.");
            }

            if (homeDevice.IsOpenOrOn == open)
            {
                string state = open ? "abierta" : "cerrada";
                throw new InvalidOperationException($"La ventana ya está {state}.");
            }

            string action;
            if (open)
            {
                action = windowSensor.OpenWindow();
                homeDevice.IsOpenOrOn = true; 
            }
            else
            {
                action = windowSensor.CloseWindow();
                homeDevice.IsOpenOrOn = false; 
            }

            _homeRepository.UpdateHomeDevice(homeDevice);

            Home home = _homeRepository.GetHomeById(homeDevice.HomeId);

            foreach (HomeOwnerPermission memberPermission in home.MemberPermissions)
            {
                if (memberPermission.IsNotificationEnabled)
                {
                    Notification notification = new Notification(action, hardwareId, "Window Sensor");
                    User member = _userRepository.GetUserById(memberPermission.HomeOwnerId);
                    member.Notifications.Add(notification);
                    _userRepository.SaveNotification(member, notification);
                }
            }
            Notification notificationOwner = new Notification(action, hardwareId, "Motion Sensor");
            User owner = _userRepository.GetUserById(home.HomeOwnerId);
            owner.Notifications.Add(notificationOwner);
            _userRepository.SaveNotification(owner, notificationOwner);
        }

        public void TurnSmartLampOnOrOff(Guid hardwareId, bool isOn)
        {
            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }

            Device device = _deviceRepository.GetDeviceById(homeDevice.DeviceId);
            if (device.DeviceType != "Smart Lamp")
            {
                throw new InvalidOperationException("No concuerda el tipo de dispositivo con la acción deseada");
            }

            SmartLamp smartLamp = (SmartLamp)device;

            if (!homeDevice.Connected)
            {
                throw new InvalidOperationException("El dispositivo no está conectado.");
            }

            if (homeDevice.IsOpenOrOn == isOn)
            {
                string state = isOn ? "encendida" : "apagada";
                throw new InvalidOperationException($"La lámpara ya está {state}.");
            }

            string action;
            if (isOn)
            {
                action = smartLamp.LightsOn();
                homeDevice.IsOpenOrOn = true;
            }
            else
            {
                action = smartLamp.LightsOff();
                homeDevice.IsOpenOrOn = false;
            }

            _homeRepository.UpdateHomeDevice(homeDevice);

            Home home = _homeRepository.GetHomeById(homeDevice.HomeId);

            foreach (HomeOwnerPermission memberPermission in home.MemberPermissions)
            {
                if (memberPermission.IsNotificationEnabled)
                {
                    Notification notification = new Notification(action, hardwareId, "Smart Lamp");
                    User member = _userRepository.GetUserById(memberPermission.HomeOwnerId);
                    member.Notifications.Add(notification);
                    _userRepository.SaveNotification(member, notification);
                }
            }
            Notification notificationOwner = new Notification(action, hardwareId, "Motion Sensor");
            User owner = _userRepository.GetUserById(home.HomeOwnerId);
            owner.Notifications.Add(notificationOwner);
            _userRepository.SaveNotification(owner, notificationOwner);
        }

        public void DetectMotionCamera(Guid hardwareId)
        {
            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }

            Device device = _deviceRepository.GetDeviceById(homeDevice.DeviceId);
            if (device.DeviceType != "Camera")
            {
                throw new InvalidOperationException("No concuerda el tipo de dispositivo con la acción deseada");
            }
            Camera camera = (Camera)device;

            if (!homeDevice.Connected)
            {
                throw new InvalidOperationException("El dispositivo no está conectado.");
            }

            string action = camera.MotionDetection();

            Home home = _homeRepository.GetHomeById(homeDevice.HomeId);

            foreach (var memberPermission in home.MemberPermissions)
            {
                if (memberPermission.IsNotificationEnabled)
                {
                    Notification notification = new Notification(action, hardwareId, "Camera");
                    User member = _userRepository.GetUserById(memberPermission.HomeOwnerId);
                    member.Notifications.Add(notification);
                    _userRepository.SaveNotification(member, notification);
                }
            }
            Notification notificationOwner = new Notification(action, hardwareId, "Motion Sensor");
            User owner = _userRepository.GetUserById(home.HomeOwnerId);
            owner.Notifications.Add(notificationOwner);
            _userRepository.SaveNotification(owner, notificationOwner);
        }

        public void DetectMotionSensor(Guid hardwareId)
        {
            HomeDevice homeDevice = _homeRepository.GetHomeDeviceById(hardwareId);
            if (homeDevice == null)
            {
                throw new NotFoundDevice();
            }

            Device device = _deviceRepository.GetDeviceById(homeDevice.DeviceId);
            if (device.DeviceType != "Motion Sensor")
            {
                throw new InvalidOperationException("No concuerda el tipo de dispositivo con la acción deseada");
            }
            MotionSensor motionSensor = (MotionSensor)device;

            if (!homeDevice.Connected)
            {
                throw new InvalidOperationException("El dispositivo no está conectado.");
            }

            string action = motionSensor.DetectMotion();

            Home home = _homeRepository.GetHomeById(homeDevice.HomeId);

            foreach (var memberPermission in home.MemberPermissions)
            {
                if (memberPermission.IsNotificationEnabled)
                {
                    Notification notification = new Notification(action, hardwareId, "Motion Sensor");
                    User member = _userRepository.GetUserById(memberPermission.HomeOwnerId);
                    member.Notifications.Add(notification);
                    _userRepository.SaveNotification(member, notification);
                }
            }
            Notification notificationOwner = new Notification(action, hardwareId, "Motion Sensor");
            User owner = _userRepository.GetUserById(home.HomeOwnerId);
            owner.Notifications.Add(notificationOwner);
            _userRepository.SaveNotification(owner, notificationOwner);
        }
    }
}

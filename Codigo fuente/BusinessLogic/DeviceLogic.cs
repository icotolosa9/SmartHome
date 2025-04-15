using DataAccess.Repositories;
using Domain;
using IBusinessLogic;
using IBusinessLogic.Exceptions;
using IDataAccess;
using IImporter;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ModeloValidador.Abstracciones;
using Models.In;
using Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogic
{
    public class DeviceLogic : IDeviceLogic
    {
        private IDeviceRepository _deviceRepository;
        private IUserRepository _userRepository;
        private ICompanyRepository _companyRepository;
        public DeviceLogic(IDeviceRepository deviceRepository, IUserRepository userRepository, ICompanyRepository companyRepository)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public Camera CreateCamera(Camera camera, string emailCompanyOwner)
        {
            User user = _userRepository.GetUserByEmail(emailCompanyOwner);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId == null)
            {
                throw new IncompleteCompanyOwnerAccountException();
            }

            Company company = _companyRepository.GetCompanyById(owner.CompanyId.Value);

            if (!string.IsNullOrEmpty(company.ValidatorModel))
            {
                var validator = LoadValidator(company.ValidatorModel);
                if (!validator.EsValido(new Modelo { Value = camera.ModelNumber }))
                {
                    throw new ArgumentException("El modelo del sensor de la cámara no es válido según la lógica de validación seleccionada.");
                }
            }

            camera.CompanyId = owner.CompanyId.Value;

            if (_deviceRepository.GetDeviceByNameAndModel(camera.Name, camera.ModelNumber) != null)
            {
                throw new DeviceAlreadyExistsException();
            }
            _deviceRepository.Save(camera);
            return camera;
        }

        public WindowSensor CreateWindowSensor(WindowSensor windowSensor, string emailCompanyOwner)
        {
            User user = _userRepository.GetUserByEmail(emailCompanyOwner);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId == null)
            {
                throw new IncompleteCompanyOwnerAccountException();
            }

            Company company = _companyRepository.GetCompanyById(owner.CompanyId.Value);

            if (!string.IsNullOrEmpty(company.ValidatorModel))
            {
                var validator = LoadValidator(company.ValidatorModel);
                if (!validator.EsValido(new Modelo { Value = windowSensor.ModelNumber }))
                {
                    throw new ArgumentException("El modelo del sensor de ventana no es válido según la lógica de validación seleccionada.");
                }
            }

            windowSensor.CompanyId = owner.CompanyId.Value;

            if (_deviceRepository.GetDeviceByNameAndModel(windowSensor.Name, windowSensor.ModelNumber) != null)
            {
                throw new DeviceAlreadyExistsException();
            }
            _deviceRepository.Save(windowSensor);
            return windowSensor;
        }

        public MotionSensor CreateMotionSensor(MotionSensor motionSensor, string emailCompanyOwner)
        {
            User user = _userRepository.GetUserByEmail(emailCompanyOwner);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId == null)
            {
                throw new IncompleteCompanyOwnerAccountException();
            }

            Company company = _companyRepository.GetCompanyById(owner.CompanyId.Value);

            if (!string.IsNullOrEmpty(company.ValidatorModel))
            {
                var validator = LoadValidator(company.ValidatorModel);
                if (!validator.EsValido(new Modelo { Value = motionSensor.ModelNumber }))
                {
                    throw new ArgumentException("El modelo del sensor de movimiento no es válido según la lógica de validación seleccionada.");
                }
            }

            motionSensor.CompanyId = owner.CompanyId.Value;

            if (_deviceRepository.GetDeviceByNameAndModel(motionSensor.Name, motionSensor.ModelNumber) != null)
            {
                throw new DeviceAlreadyExistsException();
            }
            _deviceRepository.Save(motionSensor);
            return motionSensor;
        }

        public SmartLamp CreateSmartLamp(SmartLamp smartLamp, string emailCompanyOwner)
        {
            User user = _userRepository.GetUserByEmail(emailCompanyOwner);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId == null)
            {
                throw new IncompleteCompanyOwnerAccountException();
            }

            Company company = _companyRepository.GetCompanyById(owner.CompanyId.Value);

            if (!string.IsNullOrEmpty(company.ValidatorModel))
            {
                var validator = LoadValidator(company.ValidatorModel);
                if (!validator.EsValido(new Modelo { Value = smartLamp.ModelNumber }))
                {
                    throw new ArgumentException("El modelo de la lámpara no es válido según la lógica de validación seleccionada.");
                }
            }

            smartLamp.CompanyId = owner.CompanyId.Value;

            if (_deviceRepository.GetDeviceByNameAndModel(smartLamp.Name, smartLamp.ModelNumber) != null)
            {
                throw new DeviceAlreadyExistsException();
            }
            _deviceRepository.Save(smartLamp);
            return smartLamp;
        }

        public PagedResult<DeviceDto> ListDevices(ListDevicesRequest request)
        {
            PagedResult<Device> pagedDevices = _deviceRepository.GetPagedDevices(request);

            List<DeviceDto> deviceDtos = pagedDevices.Results.Select(d => new DeviceDto
            {
                Name = d.Name,
                Model = d.ModelNumber,
                MainPicture = d.Photos[0],
                Company = d.Company.Name
            }).ToList();

            return new PagedResult<DeviceDto>(
                deviceDtos,
                pagedDevices.TotalCount,
                pagedDevices.PageNumber,
                pagedDevices.PageSize
            );
        }

        public List<string> GetSupportedDeviceTypes()
        {
            return _deviceRepository.GetSupportedDeviceTypes();
        }

        public List<ImporterInterface> GetAllImporters()
        {
            string baseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string importersPath = Path.Combine(baseDirectory, "Importers");
            if (!Directory.Exists(importersPath))
            {
                throw new DirectoryNotFoundException($"La carpeta '{importersPath}' no fue encontrada.");
            }
            string[] filePaths = Directory.GetFiles(importersPath); 

            List<ImporterInterface> availableImporters = new List<ImporterInterface>();

            foreach (string file in filePaths) 
            {
                if (FileIsDll(file))
                {
                    FileInfo dllFile = new FileInfo(file);
                    Assembly myAssembly = Assembly.LoadFile(dllFile.FullName);

                    foreach (Type type in myAssembly.GetTypes()) 
                    {
                        if (ImplementsRequiredInterface(type)) 
                        {
                            ImporterInterface instance = (ImporterInterface)Activator.CreateInstance(type);
                            availableImporters.Add(instance);
                        }
                    }
                }
            }
            return availableImporters;
        }
        private bool FileIsDll(string file)
        {
            return file.EndsWith("dll");
        }
        public bool ImplementsRequiredInterface(Type type)
        {
            return typeof(ImporterInterface).IsAssignableFrom(type) && !type.IsInterface;
        }
        public void ImportDevices(List<ImportedDevice> importedDevices, string emailCompanyOwner)
        {
            User user = _userRepository.GetUserByEmail(emailCompanyOwner);
            CompanyOwner owner = (CompanyOwner)user;

            if (owner.CompanyId == null)
            {
                throw new IncompleteCompanyOwnerAccountException();
            }

            foreach (var importedDevice in importedDevices)
            {
                Device device = ToDevice(importedDevice);

                device.CompanyId = owner.CompanyId.Value;

                _deviceRepository.Save(device);
            }
        }

        private Device ToDevice(ImportedDevice importedDevice)
        {
            List<string> photos = importedDevice.Fotos?.Select(f => f.Path).ToList() ?? new List<string>();

            return importedDevice.Tipo switch
            {
                "camera" => new Camera(
                    importedDevice.Nombre,
                    importedDevice.Modelo,
                    "Camara importada",
                    photos,
                    importedDevice.ForIndoorUse ?? false,
                    importedDevice.ForOutdoorUse ?? false,
                    importedDevice.MovementDetection ?? false,
                    importedDevice.PersonDetection ?? false
                ),
                "sensor-movement" => new MotionSensor(
                    importedDevice.Nombre,
                    importedDevice.Modelo,
                    "Sensor de movimiento importado",
                    photos
                ),
                "sensor-open-close" => new WindowSensor(
                    importedDevice.Nombre,
                    importedDevice.Modelo,
                    "Sensor de ventana importado",
                    photos
                ),
                _ => throw new ArgumentException("Tipo de dispositivo no soportado.")
            };
        }

        private List<IModeloValidador> LoadValidators()
        {
            string baseDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string validatorsPath = Path.Combine(baseDirectory, "ValidatorModels");
            if (!Directory.Exists(validatorsPath))
            {
                throw new DirectoryNotFoundException($"La carpeta '{validatorsPath}' no fue encontrada.");
            }

            string[] filePaths = Directory.GetFiles(validatorsPath, "*.dll");
            List<IModeloValidador> availableValidators = new List<IModeloValidador>();

            foreach (var file in filePaths)
            {
                try
                {
                    FileInfo dllFile = new FileInfo(file);
                    Assembly myAssembly = Assembly.LoadFile(dllFile.FullName);

                    foreach (Type type in myAssembly.GetTypes())
                    {
                        if (typeof(IModeloValidador).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            IModeloValidador validatorInstance = (IModeloValidador)Activator.CreateInstance(type);
                            availableValidators.Add(validatorInstance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar el archivo DLL: {ex.Message}");
                }
            }

            return availableValidators;
        }

        private IModeloValidador? LoadValidator(string validatorType)
        {
            var validators = LoadValidators();

            var validator = validators.FirstOrDefault(v => v.GetType().Name == validatorType);

            if (validator == null)
            {
                return null;
            }

            return validator;
        }
    }
}

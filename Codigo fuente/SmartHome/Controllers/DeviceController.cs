using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models.In;
using Models.Out;
using SmartHome.Filters;

namespace SmartHome.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly IDeviceLogic _deviceLogic;
        private readonly IUserLogic _userLogic;
        private readonly IActionLogic _actionLogic;

        public DeviceController(IDeviceLogic deviceLogic, IUserLogic userLogic, IActionLogic actionLogic)
        {
            _deviceLogic = deviceLogic;
            _userLogic = userLogic;
            _actionLogic = actionLogic;
        }

        [AuthenticationFilter("companyOwner")]
        [HttpPost("cameras")]
        public IActionResult CreateCamera([FromBody] CreateCameraRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            CreateCameraResponse response = new CreateCameraResponse(_deviceLogic.CreateCamera(request.ToEntity(), user.Email));

            return Created(string.Empty, response);
        }

        [AuthenticationFilter("companyOwner")]
        [HttpPost("window-sensors")]
        public IActionResult CreateWindowSensor([FromBody] CreateWindowSensorRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            CreateWindowSensorResponse response = new CreateWindowSensorResponse(_deviceLogic.CreateWindowSensor(request.ToEntity(), user.Email));

            return Created(string.Empty, response);
        }

        [AuthenticationFilter("companyOwner")]
        [HttpPost("motion-sensors")]
        public IActionResult CreateMotionSensor([FromBody] CreateMotionSensorRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            CreateMotionSensorResponse response = new CreateMotionSensorResponse(_deviceLogic.CreateMotionSensor(request.ToEntity(), user.Email));

            return Created(string.Empty, response);
        }

        [AuthenticationFilter("companyOwner")]
        [HttpPost("smart-lamps")]
        public IActionResult CreateSmartLamp([FromBody] CreateSmartLampRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            CreateSmartLampResponse response = new CreateSmartLampResponse(_deviceLogic.CreateSmartLamp(request.ToEntity(), user.Email));

            return Created(string.Empty, response);
        }

        [AuthenticationFilter]
        [HttpGet]
        public IActionResult ListDevices([FromQuery] ListDevicesRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return BadRequest("El número y el tamaño de página deben ser mayor que 0.");
            }

            PagedResult<DeviceDto> pagedResult = _deviceLogic.ListDevices(request);

            return Ok(pagedResult);
        }

        [AuthenticationFilter]
        [HttpGet("types")]
        public IActionResult GetSupportedDeviceTypes()
        {
            var deviceTypes = _deviceLogic.GetSupportedDeviceTypes();
            return Ok(deviceTypes);
        }

        [HttpPost("{hardwareId}/person-detected-actions")]
        public IActionResult DetectPersonCamera([FromRoute] Guid hardwareId, [FromBody] string personDetectedEmail)
        {
            _actionLogic.DetectPersonCamera(hardwareId, personDetectedEmail);
            return Ok(new { message = "Se detectó persona y se envió la notificación correspondiente a los miembros del hogar." });
        }

        [HttpPost("{hardwareId}/window-actions")]
        public IActionResult OpenOrCloseWindow([FromRoute] Guid hardwareId, [FromBody] bool isOpen)
        {
            _actionLogic.OpenOrCloseWindow(hardwareId, isOpen);
            string action = isOpen ? "abrió" : "cerró";
            return Ok(new { message = $"La ventana se {action} correctamente." });
        }

        [HttpPost("{hardwareId}/smart-light-actions")]
        public IActionResult TurnSmartLampOnOrOff([FromRoute] Guid hardwareId, [FromBody] bool isOn)
        {
            _actionLogic.TurnSmartLampOnOrOff(hardwareId, isOn);
            string action = isOn ? "prendida" : "apagada";
            return Ok(new { message = $"La lámpara fue {action} correctamente." });
        }

        [HttpPost("{hardwareId}/motion-detected-actions")]
        public IActionResult DetectMotionCamera([FromRoute] Guid hardwareId)
        {
            _actionLogic.DetectMotionCamera(hardwareId);
            return Ok(new { message = "Se detectó movimiento." });
        }

        [HttpPost("{hardwareId}/sensor-motion-actions")]
        public IActionResult DetectMotionSensor([FromRoute] Guid hardwareId)
        {
            _actionLogic.DetectMotionSensor(hardwareId);
            return Ok(new { message = "Se detectó movimiento." });
        }

        [HttpGet("importers")]
        public IActionResult ImportersName()
        {
            var availableImporters = _deviceLogic.GetAllImporters();
            return Ok(availableImporters.Select(i => i.GetName()).ToList());
        }

        [AuthenticationFilter("companyOwner")]
        [HttpPost("imported-devices")]
        public IActionResult ImportDevices(string importerType)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            var availableImporters = _deviceLogic.GetAllImporters();
            var importer = availableImporters.FirstOrDefault(i => i.GetName() == importerType);

            if (importer == null)
            {
                return BadRequest("Importador no encontrado.");
            }

            List<ImportedDevice> importedDevices = importer.ImportDevice();

            _deviceLogic.ImportDevices(importedDevices,user.Email);

            return Ok(new { message = "Se importaron correctamente los dispositivos" });
        }
    }
}

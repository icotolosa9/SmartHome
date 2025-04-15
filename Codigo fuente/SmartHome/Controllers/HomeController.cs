using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models.Out;
using Models.In;
using IBusinessLogic.Exceptions;
using Domain;
using SmartHome.Filters;

namespace SmartHome.Controllers
{
    [Route("api/homes")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IHomeLogic _homeLogic;
        private readonly IUserLogic _userLogic;

        public HomeController(IHomeLogic homeLogic, IUserLogic userLogic)
        {
            _homeLogic = homeLogic;
            _userLogic = userLogic;
        }

        [HttpPost]
        [AuthenticationFilter]
        public IActionResult CreateHome([FromBody] CreateHomeRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            CreateHomeResponse response = new CreateHomeResponse(_homeLogic.CreateHome(request.ToEntity(), user.Email));
            return Created(string.Empty, response);
        }

        [HttpPost("{homeId}/members")]
        [AuthenticationFilter]
        public IActionResult AddMemberToHome([FromRoute] Guid homeId, [FromBody] AddHomeMemberRequest addHomeMemberRequest)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            _homeLogic.AddMemberToHome(homeId, addHomeMemberRequest, user.Email);
            return Ok(new { message = $"Miembro(s) asociado(s) a hogar con id {homeId} correctamente."});
        }

        [HttpGet("{homeId}/members")]
        [AuthenticationFilter]
        public IActionResult GetHomeMembers([FromRoute] Guid homeId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            List<HomeMemberDto> members = _homeLogic.GetHomeMembers(homeId,user.Email);
            return Ok(members);
        }

        [HttpPost("{homeId}/devices")]
        [AuthenticationFilter]
        public IActionResult AssociateDevice([FromRoute] Guid homeId, [FromBody] AssociateDeviceRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            _homeLogic.AssociateDeviceToHome(homeId, request,user.Email);
            return Ok(new { message = "Dispositivo asociado correctamente." });
        }

        [HttpGet("{homeId}/devices")]
        [AuthenticationFilter]
        public IActionResult GetDevicesByHomeId([FromRoute] Guid homeId, [FromQuery] string? roomName = null)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            List<DeviceDto> devices = _homeLogic.GetHomeDevices(homeId, user.Id, roomName);
            return Ok(devices);
        }

        [HttpPut("{homeId}/notification-settings")]
        [AuthenticationFilter]
        public IActionResult UpdateMemberNotifications([FromRoute] Guid homeId, List<MemberNotificationUpdateDto> updates)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            bool result = _homeLogic.UpdateMemberNotifications(homeId, user.Id, updates);
            if (result)
            {
                return Ok(new { message = "Notificaciones actualizadas correctamente." });
            }
            return BadRequest(new { message = "No se pudo actualizar las notificaciones." });
        }

        [HttpPut("{hardwareId}/status")]
        [AuthenticationFilter]
        public IActionResult SetStatusHomeDevice([FromRoute] Guid hardwareId, bool status)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            _homeLogic.SetStatusHomeDevice(hardwareId, status ,user.Id);
            if (status)
                return Ok(new { message = "Dispositivo conectado." });
            else 
                return Ok(new { message = "Dispositivo desconectado." }); 
        }

        [HttpPut("{homeId}")]
        [AuthenticationFilter]
        public IActionResult UpdateHomeName([FromRoute] Guid homeId, [FromBody] UpdateHomeNameRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            _homeLogic.UpdateHomeName(homeId, request, user.Email);

            return Ok(new { message = "El nombre del hogar se actualizó correctamente." });
        }

        [HttpPost("rooms")]
        [AuthenticationFilter]
        public IActionResult CreateRoom([FromBody] CreateRoomRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            User user = _userLogic.GetCurrentUser(parsedToken);

            RoomDto room = _homeLogic.CreateRoom(request, user.Email);
            return Created(string.Empty, room); 
        }

        [HttpPut("{homeId}/device-to-room")]
        [AuthenticationFilter]
        public IActionResult AssignRoomToDevice(Guid homeId, [FromBody] AssignDeviceToRoomRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            User user = _userLogic.GetCurrentUser(parsedToken);

            _homeLogic.AssignDeviceToRoom(homeId, request, user.Email);

            return Ok(new { message = "El dispositivo fue asignado al cuarto correctamente." });
        }

        [HttpPut("{hardwareId}/name")]
        [AuthenticationFilter]
        public IActionResult UpdateDeviceName(Guid hardwareId, [FromBody] UpdateDeviceNameRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            _homeLogic.UpdateDeviceName(hardwareId, request.NewName, user.Email);

            return Ok(new { message = "El nombre del dispositivo se actualizó correctamente." });
        }

        [HttpGet("my-homes")]
        [AuthenticationFilter]
        public IActionResult GetMyHomes()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            token = token.Trim('"');
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            List<HomeDto> homes = _homeLogic.GetHomesByUser(user.Id);

            return Ok(homes);
        }

        [HttpGet("my-owned-homes")]
        [AuthenticationFilter]
        public IActionResult GetMyOwnedHomes()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            token = token.Trim('"');
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            List<HomeDto> homes = _homeLogic.GetHomesByOwner(user.Id);

            return Ok(homes);
        }
    }
}
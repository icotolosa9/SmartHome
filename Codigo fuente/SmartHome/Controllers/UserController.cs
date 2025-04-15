using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.In;
using Models.Out;
using SmartHome.Filters;
using System.Text.RegularExpressions;

namespace SmartHome.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [Route("admins")]
        [AuthenticationFilter("admin")]
        [HttpPost]
        public IActionResult CreateAdmin([FromBody] CreateAdminRequest request)
        {
            CreateAdminResponse response = new CreateAdminResponse(_userLogic.CreateAdmin(request.ToEntity()));
            return Created(string.Empty, response);
        }

        [AuthenticationFilter("admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin([FromRoute] Guid id)
        {
            _userLogic.DeleteAdmin(id);

            var response = new DeleteAdminResponse
            {
                Message = $"Usuario con id {id} eliminado correctamente"
            };
            return Ok(response);
        }

        [Route("company-owners")]
        [AuthenticationFilter("admin")]
        [HttpPost]
        public IActionResult CreateCompanyOwner([FromBody] CreateCompanyOwnerRequest request)
        {
            CreateCompanyOwnerResponse response = new CreateCompanyOwnerResponse(_userLogic.CreateCompanyOwner(request.ToEntity()));
            return Created(string.Empty, response);
        }

        [Route("home-owners")]
        [HttpPost]
        public IActionResult CreateHomeOwner([FromBody] CreateHomeOwnerRequest request)
        {
            CreateHomeOwnerResponse response = new CreateHomeOwnerResponse(_userLogic.CreateHomeOwner(request.ToEntity()));
            return Created(string.Empty, response);
        }

        [AuthenticationFilter("admin")]
        [HttpGet]
        public IActionResult ListAccounts([FromQuery] ListAccountsRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return BadRequest("El número de página y el tamaño de página deben ser mayores que 0.");
            }

            PagedResult<UserDto> pagedResult = _userLogic.ListAccounts(request);

            return Ok(pagedResult);
        }

        [AuthenticationFilter]
        [HttpGet("notifications")]
        public IActionResult ListNotifications([FromQuery] ListNotificationsRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            List<NotificationResponse> response = _userLogic.GetNotificationsByUser(request, user.Email);

            return Ok(response);
        }

        [AuthenticationFilter]
        [HttpPut("notifications/{notificationId}")]
        public IActionResult MarkNotificationAsRead([FromRoute] Guid notificationId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            _userLogic.MarkNotificationAsRead(notificationId, user.Email);

            return Ok(new { message = "Notification marcada como leída." });
        }
    }
}

using BusinessLogic;
using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models.In;
using Models.Out;

namespace SmartHome.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly IUserLogic _userLogic;

        public SessionController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("El email y la contraseña son obligatorios.");
            }

            var user = _userLogic.AuthenticateUser(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized("Email o contraseña incorrectos.");
            }

            var newSession = new Session();
            newSession.UserEmail = user.Email;
            var token = _userLogic.CreateSession(newSession);
            var response = new LoginResponse(token, user.Email, user.Role);
            return Ok(response);
        }
    }
}
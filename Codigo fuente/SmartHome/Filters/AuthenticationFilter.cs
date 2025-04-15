using BusinessLogic;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace SmartHome.Filters
{
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {

        public string? RequiredRole { get; set; }

        public AuthenticationFilter(string? requiredRole = null)
        {
            RequiredRole = requiredRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string tokenValue = context.HttpContext.Request.Headers["Authorization"];

            if (!isValidTokenFormat(tokenValue))
            {
                if (String.IsNullOrEmpty(tokenValue))
                {
                    context.Result = new ObjectResult("Empty authorization header") { StatusCode = 401 };
                }
                else
                {
                    context.Result = new ObjectResult("Invalid token format") { StatusCode = 401 };
                }
                return;
            }

            string token = tokenValue.Substring("Bearer ".Length).Trim('"');

            if (!Guid.TryParse(token, out Guid parsedToken))
            {
                context.Result = new ObjectResult("Invalid token format") { StatusCode = 401 };
                return;
            }

            var currentUser = GetUserLogic(context).GetCurrentUser(parsedToken);

            if (currentUser == null)
            {
                context.Result = new ObjectResult("Log in please.") { StatusCode = 401 };
                return;
            }

            if (!string.IsNullOrEmpty(RequiredRole) && !RequiredRole.Equals(currentUser.Role))
            {
                context.Result = new ObjectResult($"Access Denied for {RequiredRole} only") { StatusCode = 403 };
                return;
            }
        }

        private IUserLogic GetUserLogic(AuthorizationFilterContext context)
        {
            var sessionManagerObject = context.HttpContext.RequestServices.GetService(typeof(IUserLogic));
            var sessionService = sessionManagerObject as IUserLogic;

            return sessionService;
        }

        private bool isValidTokenFormat(String token) {
            return !String.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase);
        }
    }
}

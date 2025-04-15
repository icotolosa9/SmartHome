using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IBusinessLogic.Exceptions;

namespace SmartHome.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string message = "Ocurrió un error inesperado. Intente nuevamente más tarde.";
            int statusCode = 500; 

            switch (context.Exception)
            {
                case NotFoundException e:
                    message = e.Message;
                    statusCode = 404;
                    break;

                case HomeNotFoundException e:
                    message = e.Message;
                    statusCode = 404;
                    break;

                case AdminNotExistException e:
                    message = e.Message;
                    statusCode = 400;
                    break;

                case ArgumentException e:
                    message = e.Message;
                    statusCode = 400;
                    break;

                case HomeCapacityExceededException e: message = e.Message;
                    statusCode = 400;
                    break;

                case UserAlreadyExistsException e:
                    message = e.Message;
                    statusCode = 400;
                    break;

                case InvalidOperationException e:
                    message = e.Message;
                    statusCode = 409;
                    break;

                case CompanyAlreadyExistsException e:
                    message = e.Message;
                    statusCode = 409;
                    break;

                case DeviceAlreadyExistsException e:
                    message = e.Message;
                    statusCode = 409;
                    break;

                case CompanyOwnerAlreadyHasACompanyException e:
                    message = e.Message;
                    statusCode = 403;
                    break;

                case IncompleteCompanyOwnerAccountException e:
                    message = e.Message;
                    statusCode = 403;
                    break;

                case NotFoundDevice e:
                    message = e.Message;
                    statusCode = 404;
                    break;

                case AddMembersInvalidException e:
                    message = e.Message;
                    statusCode = 404;
                    break;

                case UnauthorizedOwnerException e:
                    message = e.Message;
                    statusCode = 401;
                    break;

                case AdminWithHomesException e:
                    message = e.Message;
                    statusCode = 401;
                    break;

                default:
                    message = "Ocurrió un error inesperado. Intente nuevamente más tarde.";
                    statusCode = 500;
                    break;
            }

            context.Result = new ObjectResult(new { message })
            {
                StatusCode = statusCode
            };
        }
    }
}

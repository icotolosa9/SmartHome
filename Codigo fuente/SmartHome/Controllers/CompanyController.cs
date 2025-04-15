using BusinessLogic;
using IBusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModeloValidador.Abstracciones;
using Models.In;
using Models.Out;
using SmartHome.Filters;
using System.Text.RegularExpressions;

namespace SmartHome.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyLogic _companyLogic;
        private readonly IUserLogic _userLogic;

        public CompanyController(ICompanyLogic companyLogic, IUserLogic userLogic)
        {
            _companyLogic = companyLogic;
            _userLogic = userLogic;
        }

        [AuthenticationFilter("companyOwner")]
        [HttpPost]
        public IActionResult CreateCompany([FromBody] CreateCompanyRequest request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            Guid parsedToken = Guid.Parse(token); 
            var user = _userLogic.GetCurrentUser(parsedToken);

            CreateCompanyResponse response = new CreateCompanyResponse(_companyLogic.CreateCompany(request.ToEntity(), user.Email));
            return Created(string.Empty, response);
        }

        [AuthenticationFilter("admin")]
        [HttpGet]
        public IActionResult ListCompanies([FromQuery] ListCompaniesRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return BadRequest("El número y el tamaño de página deben ser mayor que 0.");
            }

            PagedResult<CompanyDto> pagedResult = _companyLogic.ListCompanies(request);

            return Ok(pagedResult);
        }

        [HttpGet("validators")]
        public IActionResult GetValidators()
        {
            var validators = _companyLogic.LoadValidators();

            var validatorNames = validators.Select(v => v.GetType().Name).ToList();
            return Ok(validatorNames);
        }

        [AuthenticationFilter("companyOwner")]
        [HttpGet("my-company")]
        public IActionResult GetMyCompany()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim('"');
            }
            token = token.Trim('"');
            Guid parsedToken = Guid.Parse(token);
            var user = _userLogic.GetCurrentUser(parsedToken);

            CompanyDto company = _companyLogic.GetCompanyByOwner(user.Id);

            return Ok(company);
        }
    }
}

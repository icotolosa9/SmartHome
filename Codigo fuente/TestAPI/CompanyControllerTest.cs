using Domain;
using Models.Out;
using Models.In;
using SmartHome.Controllers;
using Microsoft.AspNetCore.Mvc;
using IBusinessLogic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;

namespace TestAPI
{
    [TestClass]
    public class CompanyControllerTest
    {
        private Mock<ICompanyLogic> _companyLogicMock;
        private Mock<IUserLogic> _userLogicMock;
        private CompanyController _companyController;
        private CreateCompanyRequest _createCompanyRequest;
        private const string ValidRut = "012345678901";
        private const string ValidName = "Dominguez";
        private const string ValidLogoURL = "https://example.com/photos/franco.jpg";
        private CompanyOwner _owner;
        private Guid UserId = Guid.NewGuid();
        private const string UserEmail = "franquito@gmail.com";

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            _companyLogicMock = new Mock<ICompanyLogic>(MockBehavior.Strict);
            _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _companyController = new CompanyController(_companyLogicMock.Object, _userLogicMock.Object);

            var httpContext = new DefaultHttpContext();
            _companyController.ControllerContext.HttpContext = httpContext;

            _createCompanyRequest = new CreateCompanyRequest
            {
                Rut = ValidRut,
                Name = ValidName,
                LogoURL = ValidLogoURL
            };

            _owner = new CompanyOwner("Franco", "Colapinto", UserEmail, "Password123!");
        }

        [TestMethod]
        public void CreateCompany_Ok()
        {
            // Arrange
            var token = Guid.NewGuid(); 
            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(_owner);
            _companyLogicMock.Setup(cl => cl.CreateCompany(It.IsAny<Company>(), _owner.Email)).Returns(new Company(ValidRut, ValidName, ValidLogoURL));

            // Act
            _companyController.ControllerContext.HttpContext.Request.Headers["Authorization"] = token.ToString();
            CreatedResult result = _companyController.CreateCompany(_createCompanyRequest) as CreatedResult;
            var objectResult = result?.Value as CreateCompanyResponse;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(ValidName, objectResult.Name);
        }

        [TestMethod]
        public void ListCompanies_ReturnsPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResult<CompanyDto>(
                new List<CompanyDto>
                {
                    new CompanyDto { CompanyId = Guid.NewGuid(), CompanyRut = ValidRut, CompanyName = ValidName, CompanyOwnerFullName = "Owner Name", CompanyOwnerEmail = UserEmail }
                },
                1, 
                1,
                10 
            );

            var request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            _companyLogicMock.Setup(cl => cl.ListCompanies(request)).Returns(pagedResult);

            // Act
            IActionResult result = _companyController.ListCompanies(request);
            OkObjectResult okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult, "Expected an OkObjectResult.");
            var returnedPagedResult = okResult.Value as PagedResult<CompanyDto>;
            Assert.IsNotNull(returnedPagedResult);
            Assert.AreEqual(1, returnedPagedResult.TotalCount); 
        }

        [TestMethod]
        public void ListCompanies_InvalidPageNumber_ReturnsBadRequest()
        {
            // Arrange
            ListCompaniesRequest request = new ListCompaniesRequest
            {
                PageNumber = 0,
                PageSize = 10
            };

            // Act
            IActionResult result = _companyController.ListCompanies(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void ListCompanies_InvalidPageSize_ReturnsBadRequest()
        {
            // Arrange
            ListCompaniesRequest request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 0
            };

            // Act
            IActionResult result = _companyController.ListCompanies(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetMyCompany_ValidToken_ReturnsCompany()
        {
            // Arrange
            var token = Guid.NewGuid();
            var user = new CompanyOwner("Franco", "Colapinto", UserEmail, "Password123!") { Id = UserId };
            var expectedCompany = new CompanyDto { CompanyId = Guid.NewGuid(), CompanyName = ValidName, CompanyRut = ValidRut, CompanyOwnerFullName = $"{user.FirstName} {user.LastName}" };

            _companyController.ControllerContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

            _userLogicMock.Setup(ul => ul.GetCurrentUser(token)).Returns(user).Verifiable();
            _companyLogicMock.Setup(cl => cl.GetCompanyByOwner(user.Id)).Returns(expectedCompany).Verifiable();

            // Act
            IActionResult result = _companyController.GetMyCompany();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "Expected OkObjectResult.");
            _userLogicMock.VerifyAll();
            _companyLogicMock.VerifyAll();
        }
    }
}

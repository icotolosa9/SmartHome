using BusinessLogic;
using Domain;
using IBusinessLogic.Exceptions;
using IDataAccess;
using Models.In;
using Models.Out;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestBusinessLogic
{
    [TestClass]
    public class CompanyLogicTest
    {
        private Mock<ICompanyRepository> _companyRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private CompanyLogic _companyLogic;

        [TestInitialize]
        public void Setup()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _companyLogic = new CompanyLogic(_companyRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [TestMethod]
        public void CreateCompany_ReturnsCreatedCompany()
        {
            // Arrange
            var ownerEmail = "john.doe@gmail.com";
            var owner = new CompanyOwner("John", "Doe", ownerEmail, "password123!") { Company = null };
            var company = new Company("123456789101", "TechCorp", "logo.png");

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(ownerEmail)).Returns(owner);
            _companyRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Company>());
            _companyRepositoryMock.Setup(repo => repo.Save(company)).Returns(company);
            _userRepositoryMock.Setup(repo => repo.Update(owner)).Returns(owner);

            // Act
            var createdCompany = _companyLogic.CreateCompany(company, ownerEmail);

            // Assert
            Assert.AreEqual(company, createdCompany);
            _userRepositoryMock.Verify(repo => repo.Update(owner), Times.Once); 
        }

        [TestMethod]
        [ExpectedException(typeof(CompanyOwnerAlreadyHasACompanyException))]
        public void CreateCompany_ExistingCompanyOwner_ThrowsException()
        {
            // Arrange
            var company1 = new Company("123456789101", "TechCorp", "logo.png");
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!") 
            { 
                Company = company1,
                CompanyId = company1.CompanyId 
            };
            var company2 = new Company("876543217522", "InnoCorp", "logo2.png");

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);

            // Act
            _companyLogic.CreateCompany(company2, owner.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(CompanyAlreadyExistsException))]
        public void CreateCompany_ExistingCompanyByRUT_ThrowsException()
        {
            // Arrange
            var existingCompany = new Company("123456789101", "TechCorp", "logo.png");
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!") { Company = null };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(owner.Email)).Returns(owner);
            _companyRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Company> { existingCompany });

            // Act
            _companyLogic.CreateCompany(existingCompany, owner.Email);
        }

        [TestMethod]
        public void ListCompanies_ReturnsPagedCompanies()
        {
            // Arrange
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!");
            owner.Id = new Guid();
            var companies = new List<Company>
            {
                new Company("123456789101", "TechCorp", "logo.png") { Owner = owner },
                new Company("876543217522", "InnoCorp", "logo2.png") { Owner = owner }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(owner.Id)).Returns(owner);
            _companyRepositoryMock.Setup(repo => repo.GetPagedCompanies(It.IsAny<ListCompaniesRequest>()))
                                  .Returns(new PagedResult<Company>(companies, companies.Count, 1, companies.Count));

            var request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = _companyLogic.ListCompanies(request);

            // Assert
            Assert.AreEqual(2, result.Results.Count());
        }

        [TestMethod]
        public void ListCompanies_WithNameFilter_ReturnsFilteredResults()
        {
            // Arrange
            var owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!");
            owner.Id = new Guid();
            var companies = new List<Company>
            {
                new Company("123456789101", "TechCorp", "logo.png") { Owner = owner },
                new Company("876543217522", "InnoCorp", "logo2.png") { Owner = owner }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(owner.Id)).Returns(owner);
            _companyRepositoryMock.Setup(repo => repo.GetPagedCompanies(It.IsAny<ListCompaniesRequest>()))
                                  .Returns(new PagedResult<Company>(new List<Company> { companies[0] }, 1, 1, 1));

            var request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                CompanyName = "TechCorp"
            };

            // Act
            var result = _companyLogic.ListCompanies(request);

            // Assert
            Assert.AreEqual(1, result.Results.Count());
        }

        [TestMethod]
        public void ListCompanies_WithNoResults_ReturnsEmptyPagedResult()
        {
            // Arrange
            _companyRepositoryMock.Setup(repo => repo.GetPagedCompanies(It.IsAny<ListCompaniesRequest>()))
                                  .Returns(new PagedResult<Company>(new List<Company>(), 0, 1, 10));

            var request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = _companyLogic.ListCompanies(request);

            // Assert
            Assert.AreEqual(0, result.Results.Count());
        }

        [TestMethod]
        public void GetCompanyByOwner_UserIsOwner_ReturnsCompanyDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "johndoe@example.com", "Password123!") { Id = userId, CompanyId = companyId };
            var company = new Company("123456789011", "TechCorp", "logo.png") { CompanyId = companyId };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(owner).Verifiable();
            _companyRepositoryMock.Setup(repo => repo.GetCompanyById(companyId)).Returns(company).Verifiable();

            // Act
            CompanyDto? result = _companyLogic.GetCompanyByOwner(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(company.CompanyId, result.CompanyId);
            Assert.AreEqual(company.Name, result.CompanyName);
            Assert.AreEqual(company.Rut, result.CompanyRut);
            Assert.AreEqual($"{owner.FirstName} {owner.LastName}", result.CompanyOwnerFullName);
            Assert.AreEqual(owner.Email, result.CompanyOwnerEmail);
            _userRepositoryMock.VerifyAll();
            _companyRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetCompanyByOwner_UserIsNotOwner_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var owner = new CompanyOwner("John", "Doe", "johndoe@example.com", "Password123!") { Id = userId, CompanyId = null };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(owner).Verifiable();

            // Act
            CompanyDto? result = _companyLogic.GetCompanyByOwner(userId);

            // Assert
            Assert.IsNull(result);
            _userRepositoryMock.VerifyAll();
        }
    }
}

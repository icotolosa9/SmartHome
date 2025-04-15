using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.In;
using Models.Out;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDataAccess
{
    [TestClass]
    public class CompanyRepositoryTest
    {
        private SqliteConnection _connection;
        private SmartHomeContext _context;
        private CompanyRepository _companyRepository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var contextOptions = new DbContextOptionsBuilder<SmartHomeContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new SmartHomeContext(contextOptions);
            _context.Database.EnsureCreated();

            _companyRepository = new CompanyRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _connection.Close();
        }

        [TestMethod]
        public void SaveCompany_Success()
        {
            // Arrange
            Company company = new Company("123456789011", "TechCorp", "logo.png");
            company.Owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!");

            // Act
            Company savedCompany = _companyRepository.Save(company);

            // Assert
            Assert.IsNotNull(savedCompany);
            Assert.AreEqual("TechCorp", savedCompany.Name);
        }

        [TestMethod]
        public void GetAllCompanies_ReturnsList()
        {
            // Arrange
            Company company1 = new Company("123456789011", "TechCorp", "logo.png")
            {
                Owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            };
            Company company2 = new Company("123456789012", "InnoCorp", "logo2.png")
            {
                Owner = new CompanyOwner("Jane", "Doe", "jane.doe@gmail.com", "password456!")
            };

            _companyRepository.Save(company1);
            _companyRepository.Save(company2);

            // Act
            List<Company> companies = _companyRepository.GetAll();

            // Assert
            Assert.AreEqual(2, companies.Count);
        }

        [TestMethod]
        public void GetPagedCompanies_ReturnsPagedResult()
        {
            // Arrange
            Company company1 = new Company("123456789011", "TechCorp", "logo.png")
            {
                Owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            };
            Company company2 = new Company("123456789012", "InnoCorp", "logo2.png")
            {
                Owner = new CompanyOwner("Jane", "Doe", "jane.doe@gmail.com", "password456!")
            };

            _companyRepository.Save(company1);
            _companyRepository.Save(company2);

            ListCompaniesRequest request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 1,
                CompanyName = "TechCorp"
            };

            // Act
            PagedResult<Company> pagedResult = _companyRepository.GetPagedCompanies(request);

            // Assert
            Assert.AreEqual(1, pagedResult.Results.Count());
            Assert.AreEqual("TechCorp", pagedResult.Results.First().Name);
        }

        [TestMethod]
        public void GetPagedCompanies_WithOwnerFullName_ReturnsFilteredResult()
        {
            // Arrange
            Company company1 = new Company("123456789012", "TechCorp", "logo.png")
            {
                Owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            };
            Company company2 = new Company("123456789011", "InnoCorp", "logo2.png")
            {
                Owner = new CompanyOwner("Jane", "Doe", "jane.doe@gmail.com", "password456!")
            };

            _companyRepository.Save(company1);
            _companyRepository.Save(company2);

            ListCompaniesRequest request = new ListCompaniesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                CompanyOwnerFullName = "John Doe"
            };

            // Act
            PagedResult<Company> pagedResult = _companyRepository.GetPagedCompanies(request);

            // Assert
            Assert.AreEqual(1, pagedResult.Results.Count());
            Assert.AreEqual("John", pagedResult.Results.First().Owner.FirstName);
        }

        [TestMethod]
        public void GetCompanyById_ValidId_ReturnsCompany()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            var company = new Company("123456789011", "TechCorp", "logo.png")
            {
                CompanyId = companyId,
                Owner = new CompanyOwner("John", "Doe", "john.doe@gmail.com", "password123!")
            };
            _context.Companies.Add(company);
            _context.SaveChanges();

            // Act
            var result = _companyRepository.GetCompanyById(companyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(companyId, result.CompanyId);
        }

        [TestMethod]
        public void GetCompanyById_InvalidId_ReturnsNull()
        {
            // Arrange
            Guid invalidCompanyId = Guid.NewGuid(); 

            // Act
            var result = _companyRepository.GetCompanyById(invalidCompanyId);

            // Assert
            Assert.IsNull(result);
        }

    }
}
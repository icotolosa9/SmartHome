using DataAccess.Context;
using DataAccess.Migrations;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using Models.In;
using Models.Out;
using System;

namespace DataAccess.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly SmartHomeContext _smartHomeContext;

        public CompanyRepository(SmartHomeContext smartHomeContext)
        {
            _smartHomeContext = smartHomeContext;
        }

        public Company Save(Company company)
        {
            _smartHomeContext.Companies.Add(company);
            _smartHomeContext.SaveChanges();
            return company;
        }

        public List<Company> GetAll()
        {
            return _smartHomeContext.Companies.ToList();
        }

        public PagedResult<Company> GetPagedCompanies(ListCompaniesRequest request)
        {
            IQueryable<Company> query = _smartHomeContext.Companies;

            if (!string.IsNullOrEmpty(request.CompanyName))
            {
                query = query.Where(company => company.Name == request.CompanyName);
            }

            if (!string.IsNullOrEmpty(request.CompanyOwnerFullName))
            {
                string requestOwnerFullName = request.CompanyOwnerFullName.ToLower();
                query = query.Where(company => (company.Owner.FirstName + " " + company.Owner.LastName).ToLower().Contains(requestOwnerFullName));
            }

            int totalCount = query.Count();

            List<Company> companies = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<Company>(companies, totalCount, request.PageNumber, request.PageSize);
        }

        public Company? GetCompanyById(Guid companyId)
        {
            return _smartHomeContext.Companies.FirstOrDefault(c => c.CompanyId == companyId);
        }
    }
}

using Domain;
using Models.In;
using Models.Out;

namespace IDataAccess;

public interface ICompanyRepository
{
    Company Save(Company company);
    List<Company> GetAll();
    PagedResult<Company> GetPagedCompanies(ListCompaniesRequest request);
    Company? GetCompanyById(Guid companyId);
}

using Domain;
using ModeloValidador.Abstracciones;
using Models.In;
using Models.Out;

namespace IBusinessLogic;

public interface ICompanyLogic
{
    Company CreateCompany(Company company, string emailOwner);
    PagedResult<CompanyDto> ListCompanies(ListCompaniesRequest request);
    List<IModeloValidador> LoadValidators();
    CompanyDto? GetCompanyByOwner(Guid userId);
}

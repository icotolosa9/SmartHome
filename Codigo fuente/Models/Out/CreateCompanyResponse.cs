using Domain;

namespace Models.Out;

public class CreateCompanyResponse
{
    public Guid CompanyId { get; set; }
    public string Rut { get; set; }
    public string Name { get; set; }
    public string LogoURL { get; set; }

    public CreateCompanyResponse(Company company)
    {
        CompanyId = company.CompanyId;
        Rut = company.Rut;
        Name = company.Name;
        LogoURL = company.LogoURL;
    }

    public override bool Equals(object obj)
    {
        return obj is CreateCompanyResponse response &&
               Name == response.Name;
    }
}

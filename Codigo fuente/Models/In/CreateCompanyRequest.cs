using Domain;
using ModeloValidador.Abstracciones;

namespace Models.In;

public class CreateCompanyRequest
{
    public string Rut { get; set; }
    public string Name { get; set; }
    public string LogoURL { get; set; }
    public string? ValidatorModel { get; set; }

    public Company ToEntity()
    {
        var company = new Company(Rut, Name, LogoURL);
        company.ValidatorModel = ValidatorModel; 
        return company;
    }
}

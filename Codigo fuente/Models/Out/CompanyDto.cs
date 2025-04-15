namespace Models.Out;

public class CompanyDto
{
    public Guid CompanyId { get; set; }
    public string CompanyRut { get; set; }
    public string CompanyName { get; set; }
    public string CompanyOwnerFullName { get; set; }
    public string CompanyOwnerEmail { get; set; }
}

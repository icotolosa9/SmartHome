namespace Models.In;

public class ListCompaniesRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? CompanyName { get; set; } = null;
    public string? CompanyOwnerFullName { get; set; } = null;
}

using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class CreateCompanyOwnerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public CreateCompanyOwnerResponse() { }

        public CreateCompanyOwnerResponse(CompanyOwner companyOwner)
        {
            Id = companyOwner.Id;
            FirstName = companyOwner.FirstName;
            LastName = companyOwner.LastName;
            Email = companyOwner.Email;
            Role = companyOwner.Role;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateCompanyOwnerResponse response &&
                   Email == response.Email;
        }
    }
}

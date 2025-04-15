using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class CreateCompanyOwnerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public CompanyOwner ToEntity()
        {
            return new CompanyOwner(FirstName, LastName, Email, Password);
        }
    }
}
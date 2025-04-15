using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CompanyOwner : User
    {
        public Company? Company { get; set; }
        public Guid? CompanyId { get; set; }
        public CompanyOwner(string firstName, string lastName, string email, string password)
        : base(firstName, lastName, email, password)
        {
            Role = "companyOwner";
        }
    }
}

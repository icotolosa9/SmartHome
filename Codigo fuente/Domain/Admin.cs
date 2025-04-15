using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Admin: User
    {
        public Admin(string firstName, string lastName, string email, string password)
        : base(firstName, lastName, email, password)
        {
            Role = "admin";
        }

        public Admin() { }
    }
}

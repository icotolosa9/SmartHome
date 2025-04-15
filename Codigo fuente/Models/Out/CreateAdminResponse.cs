using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class CreateAdminResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public CreateAdminResponse() { }

        public CreateAdminResponse(Admin admin)
        {
            Id = admin.Id;
            FirstName = admin.FirstName;
            LastName = admin.LastName;
            Email = admin.Email;
            Role = admin.Role;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateAdminResponse response &&
                   Email == response.Email;
        }
    }
}

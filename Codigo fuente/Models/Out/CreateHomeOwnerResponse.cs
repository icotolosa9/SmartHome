using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class CreateHomeOwnerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfilePhoto { get; set; }

        public CreateHomeOwnerResponse() { }

        public CreateHomeOwnerResponse(HomeOwner user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Role = user.Role;
            ProfilePhoto = user.ProfilePhoto;
        }

        public override bool Equals(object obj)
        {
            return obj is CreateHomeOwnerResponse response &&
                   Email == response.Email;
        }
    }
}

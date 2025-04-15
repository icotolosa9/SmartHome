using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class CreateHomeOwnerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePhoto { get; set; }
        
        public HomeOwner ToEntity()
        {
            return new HomeOwner(FirstName, LastName, Email, Password, ProfilePhoto);
        }
    }
}

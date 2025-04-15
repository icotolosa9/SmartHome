using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class LoginResponse
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid Token { get; set; }

        public LoginResponse(Guid token, string email, string role)
        {
            Token = token;
            Email = email;
            Role = role;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class HomeMemberDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string>? Permissions { get; set; } = new List<string>();
        public bool IsNotificationsOn { get; set; } = true;

        public override bool Equals(object obj)
        {
            return obj is HomeMemberDto response &&
                   Email == response.Email;
        }
    }   
}

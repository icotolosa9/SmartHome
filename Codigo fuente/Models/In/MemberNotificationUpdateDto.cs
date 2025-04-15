using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class MemberNotificationUpdateDto
    {
        public string HomeOwnerEmail { get; set; }
        public bool IsNotificationEnabled { get; set; }
    }
}

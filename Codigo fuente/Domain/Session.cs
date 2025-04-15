using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Session
    {
        public int Id { get; set; }
        public Guid Token { get; set; }
        public string UserEmail { get; set; }

        public Session()
        {
            Token = Guid.NewGuid();
        }
    }
}

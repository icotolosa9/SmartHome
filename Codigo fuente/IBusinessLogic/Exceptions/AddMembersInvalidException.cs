using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class AddMembersInvalidException : Exception
    {
        public AddMembersInvalidException()
            : base("Solo se pueden agregar usuarios que sean Dueños de hogares.")
        {
        }
    }
}

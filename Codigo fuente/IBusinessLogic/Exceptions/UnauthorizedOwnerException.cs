using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class UnauthorizedOwnerException : Exception
    {
        public UnauthorizedOwnerException()
            : base("No eres el dueño de este hogar para realizar acciones con él.")
        {
        }
    }
}

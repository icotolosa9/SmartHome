using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException()
            : base("Ya existe un usuario con este correo electrónico.")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class AdminNotExistException : Exception
    {
        public AdminNotExistException()
            : base("No existe un administrador con ese id.")
        {
        }
    }
}

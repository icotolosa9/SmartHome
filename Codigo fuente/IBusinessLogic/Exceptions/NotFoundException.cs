using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("No se encontro usuario con esas credenciales.")
        {
        }

    }
}

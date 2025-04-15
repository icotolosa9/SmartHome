using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class NotFoundDevice: Exception
    {
        public NotFoundDevice()
            : base("No se encontro un device con ese id.")
        {
        }

    }
}

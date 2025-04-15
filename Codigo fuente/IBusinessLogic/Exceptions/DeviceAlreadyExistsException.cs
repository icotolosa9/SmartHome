using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class DeviceAlreadyExistsException : Exception
    {
        public DeviceAlreadyExistsException()
            : base("Ya existe un device con ese nombre y modelo.")
        {
        }
    }
}

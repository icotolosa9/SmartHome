using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class IncompleteCompanyOwnerAccountException : Exception
    {
        public IncompleteCompanyOwnerAccountException()
            : base("Para registrar un dispositivo el dueño de empresa debe tener una empresa asociada.")
        {
        }
    }
}

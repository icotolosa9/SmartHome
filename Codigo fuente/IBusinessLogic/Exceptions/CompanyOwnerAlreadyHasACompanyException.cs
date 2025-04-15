using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class CompanyOwnerAlreadyHasACompanyException : Exception
    {
        public CompanyOwnerAlreadyHasACompanyException()
            : base("Este user ya tiene una compañía asociada.")
        {
        }
    }
}

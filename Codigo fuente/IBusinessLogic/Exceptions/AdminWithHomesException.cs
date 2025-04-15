using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBusinessLogic.Exceptions
{
    public class AdminWithHomesException : Exception
    {
        public AdminWithHomesException()
            : base("No se puede eliminar un Admin que tenga o pertenezca a un hogar.")
        {
        }
    }
}

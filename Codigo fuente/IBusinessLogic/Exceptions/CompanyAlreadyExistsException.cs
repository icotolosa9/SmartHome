namespace IBusinessLogic.Exceptions
{
    public class CompanyAlreadyExistsException : Exception
    {
        public CompanyAlreadyExistsException()
            : base("Ya existe una compañía con estas credenciales.")
        {
        }
    }

}
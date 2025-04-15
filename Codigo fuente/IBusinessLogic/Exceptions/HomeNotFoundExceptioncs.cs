namespace IBusinessLogic.Exceptions
{
    public class HomeNotFoundException : Exception
    {
        public HomeNotFoundException()
            : base("No se encontró el hogar.")
        {
        }
    }
}

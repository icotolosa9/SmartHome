namespace IBusinessLogic.Exceptions
{
    public class HomeCapacityExceededException : Exception
    {
        public HomeCapacityExceededException() : base("Capacidad del hogar excedida.")
        {
        }
    }
}

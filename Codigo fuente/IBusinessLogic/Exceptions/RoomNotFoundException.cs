namespace IBusinessLogic.Exceptions
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException()
            : base("No se encontró el cuarto.")
        {
        }
    }
}

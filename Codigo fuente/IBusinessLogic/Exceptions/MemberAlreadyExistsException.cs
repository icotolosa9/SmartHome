namespace IBusinessLogic.Exceptions
{
    public class MemberAlreadyExistsException : Exception
    {
        public MemberAlreadyExistsException(string v) : base(message: v)
        {
        }
    }
}
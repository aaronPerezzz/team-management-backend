namespace team_management_backend.Exceptions
{
    public class CustomException: Exception
    {
        public CustomException() { }

        public CustomException(string message): base(message)
        {
            
        }
    }
}

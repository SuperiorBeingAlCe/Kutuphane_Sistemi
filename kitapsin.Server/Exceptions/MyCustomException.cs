namespace kitapsin.Server.Exceptions
{
    public class MyCustomException : System.Exception
    {
        public MyCustomException(string message) : base(message) { }
   

     public MyCustomException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    } }

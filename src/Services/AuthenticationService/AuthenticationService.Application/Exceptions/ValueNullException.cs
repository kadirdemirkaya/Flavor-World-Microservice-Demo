namespace AuthenticationService.Application.Exceptions
{
    [Serializable]
    public class ValueNullException : Exception
    {
        public string Username { get; }

        public ValueNullException()
            : base($"Value is null !")
        {

        }

        public ValueNullException(string message)
            : base(message)
        {

        }
    }
}

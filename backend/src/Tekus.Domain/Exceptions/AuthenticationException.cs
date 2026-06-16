namespace Tekus.Domain.Exceptions
{
    /// <summary>
    /// Thrown when authentication credentials are missing or invalid.
    /// </summary>
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base("Invalid username or password.") { }

        public AuthenticationException(string message) : base(message) { }
    }
}

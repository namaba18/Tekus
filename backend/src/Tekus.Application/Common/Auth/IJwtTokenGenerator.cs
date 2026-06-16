namespace Tekus.Application.Common.Auth
{
    public interface IJwtTokenGenerator
    {
        /// <summary>Generates a signed JWT for the given username and returns the token along with its expiration date.</summary>
        (string Token, DateTime ExpiresAtUtc) GenerateToken(string username);
    }
}

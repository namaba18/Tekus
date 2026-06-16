namespace Tekus.Application.Features.Auth.Login
{
    public record LoginResponse(string Username, string Token, DateTime ExpiresAtUtc);
}

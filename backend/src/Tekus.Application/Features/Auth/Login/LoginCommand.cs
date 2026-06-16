using MediatR;

namespace Tekus.Application.Features.Auth.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
}

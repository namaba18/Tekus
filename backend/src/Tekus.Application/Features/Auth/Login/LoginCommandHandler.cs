using MediatR;
using Microsoft.Extensions.Options;
using Tekus.Application.Common.Auth;
using Tekus.Application.Common.Options;
using Tekus.Domain.Exceptions;

namespace Tekus.Application.Features.Auth.Login
{
    public class LoginCommandHandler(
        IOptions<DefaultUserSettings> defaultUserOptions,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly DefaultUserSettings _defaultUser = defaultUserOptions.Value;

        public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var isValidUser =
                string.Equals(request.Username, _defaultUser.Username, StringComparison.OrdinalIgnoreCase)
                && passwordHasher.Verify(request.Password, _defaultUser.PasswordHash);

            if (!isValidUser)
                throw new AuthenticationException();

            var (token, expiresAtUtc) = jwtTokenGenerator.GenerateToken(_defaultUser.Username);

            return Task.FromResult(new LoginResponse(_defaultUser.Username, token, expiresAtUtc));
        }
    }
}

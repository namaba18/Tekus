using Microsoft.Extensions.Options;
using Moq;
using Tekus.Application.Common.Auth;
using Tekus.Application.Common.Options;
using Tekus.Application.Features.Auth.Login;
using Tekus.Domain.Exceptions;

namespace UnitTests.Features.Auth.Login
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new();
        private readonly DefaultUserSettings _defaultUser = new() { Username = "admin", PasswordHash = "hashed-password" };

        private LoginCommandHandler CreateHandler() =>
            new(Options.Create(_defaultUser), _passwordHasher.Object, _jwtTokenGenerator.Object);

        [Fact]
        public async Task Handle_ReturnsToken_WhenCredentialsAreValid()
        {
            _passwordHasher.Setup(p => p.Verify("Admin123!", _defaultUser.PasswordHash)).Returns(true);
            var expiresAt = DateTime.UtcNow.AddHours(1);
            _jwtTokenGenerator.Setup(j => j.GenerateToken(_defaultUser.Username)).Returns(("token-123", expiresAt));

            var handler = CreateHandler();
            var result = await handler.Handle(new LoginCommand("admin", "Admin123!"), CancellationToken.None);

            Assert.Equal("admin", result.Username);
            Assert.Equal("token-123", result.Token);
            Assert.Equal(expiresAt, result.ExpiresAtUtc);
        }

        [Fact]
        public async Task Handle_ThrowsAuthenticationException_WhenPasswordIsInvalid()
        {
            _passwordHasher.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                handler.Handle(new LoginCommand("admin", "wrong-password"), CancellationToken.None));
        }
    }
}

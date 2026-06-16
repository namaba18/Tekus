using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tekus.Application.Features.Auth.Login;
using Tekus.Domain.Exceptions;

namespace Tekus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>Authenticates against the default application user and returns a JWT.</summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await mediator.Send(new LoginCommand(request.Username, request.Password), cancellationToken);
                return Ok(response);
            }
            catch (AuthenticationException)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }
        }
    }

    public record LoginRequest(string Username, string Password);
}

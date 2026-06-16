using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tekus.Application.Features.Services.Create;
using Tekus.Domain.Exceptions;

namespace Tekus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController(IMediator mediator) : ControllerBase
    {
        /// <summary>Creates a new service for a supplier and notifies the configured recipient by email.</summary>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Create(CreateServiceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tekus.Application.Features.Services.Create;
using Tekus.Application.Features.Services.GetBySupplier;
using Tekus.Application.Features.Suppliers.GetAll;
using Tekus.Application.Features.Suppliers.GetById;
using Tekus.Domain.Exceptions;

namespace Tekus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController(IMediator mediator) : ControllerBase
    {
        /// <summary>Lists all suppliers.</summary>
        [HttpGet]
        public async Task<ActionResult<List<SupplierResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetSuppliersQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>Gets a single supplier by id.</summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>Lists the services associated with a supplier.</summary>
        [HttpGet("{id:guid}/services")]
        public async Task<ActionResult<List<ServiceResponse>>> GetServices(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await mediator.Send(new GetServicesBySupplierQuery(id), cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

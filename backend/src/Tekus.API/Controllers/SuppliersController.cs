using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tekus.Application.Features.Suppliers.GetAll;

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
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tekus.Application.Common.Models;
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
        /// <summary>Lists suppliers with pagination, sorting and search.</summary>
        [HttpGet]
        public async Task<ActionResult<PagedResult<SupplierResponse>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false,
            CancellationToken cancellationToken = default)
        {
            var query = new GetSuppliersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = search,
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            var response = await mediator.Send(query, cancellationToken);
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

        /// <summary>Lists the services associated with a supplier, with pagination, sorting and search.</summary>
        [HttpGet("{id:guid}/services")]
        public async Task<ActionResult<PagedResult<ServiceResponse>>> GetServices(
            Guid id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new GetServicesBySupplierQuery(id)
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = search,
                    SortBy = sortBy,
                    SortDescending = sortDescending
                };

                var response = await mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

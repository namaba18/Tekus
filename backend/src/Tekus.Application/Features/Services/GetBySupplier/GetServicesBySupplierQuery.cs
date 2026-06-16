using MediatR;
using Tekus.Application.Common.Models;
using Tekus.Application.Features.Services.Create;

namespace Tekus.Application.Features.Services.GetBySupplier
{
    public record GetServicesBySupplierQuery(Guid SupplierId) : PagedQuery, IRequest<PagedResult<ServiceResponse>>;
}

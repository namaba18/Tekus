using MediatR;
using Tekus.Application.Common.Models;

namespace Tekus.Application.Features.Suppliers.GetAll
{
    public record GetSuppliersQuery : PagedQuery, IRequest<PagedResult<SupplierResponse>>;
}

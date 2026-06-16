using MediatR;

namespace Tekus.Application.Features.Suppliers.GetAll
{
    public record GetSuppliersQuery : IRequest<List<SupplierResponse>>;
}

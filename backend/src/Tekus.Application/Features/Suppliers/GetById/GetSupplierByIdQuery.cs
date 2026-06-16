using MediatR;
using Tekus.Application.Features.Suppliers.GetAll;

namespace Tekus.Application.Features.Suppliers.GetById
{
    public record GetSupplierByIdQuery(Guid Id) : IRequest<SupplierResponse>;
}

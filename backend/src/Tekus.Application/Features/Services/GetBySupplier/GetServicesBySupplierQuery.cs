using MediatR;
using Tekus.Application.Features.Services.Create;

namespace Tekus.Application.Features.Services.GetBySupplier
{
    public record GetServicesBySupplierQuery(Guid SupplierId) : IRequest<List<ServiceResponse>>;
}

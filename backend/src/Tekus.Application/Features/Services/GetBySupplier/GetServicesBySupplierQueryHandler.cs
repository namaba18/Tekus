using MediatR;
using Tekus.Application.Features.Services.Create;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Services.GetBySupplier
{
    public class GetServicesBySupplierQueryHandler(
        ISupplierRepository supplierRepository,
        IServiceRepository serviceRepository) : IRequestHandler<GetServicesBySupplierQuery, List<ServiceResponse>>
    {
        public async Task<List<ServiceResponse>> Handle(GetServicesBySupplierQuery request, CancellationToken cancellationToken)
        {
            _ = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken)
                ?? throw new NotFoundException(nameof(Supplier), request.SupplierId);

            var services = await serviceRepository.GetBySupplierIdAsync(request.SupplierId, cancellationToken);

            return services
                .Select(s => new ServiceResponse(s.Id, s.Name, s.HourlyRate, s.SupplierId))
                .ToList();
        }
    }
}

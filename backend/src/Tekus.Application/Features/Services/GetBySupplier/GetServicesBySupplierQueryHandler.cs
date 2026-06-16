using MediatR;
using Tekus.Application.Common.Models;
using Tekus.Application.Features.Services.Create;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Services.GetBySupplier
{
    public class GetServicesBySupplierQueryHandler(
        ISupplierRepository supplierRepository,
        IServiceRepository serviceRepository) : IRequestHandler<GetServicesBySupplierQuery, PagedResult<ServiceResponse>>
    {
        public async Task<PagedResult<ServiceResponse>> Handle(GetServicesBySupplierQuery request, CancellationToken cancellationToken)
        {
            _ = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken)
                ?? throw new NotFoundException(nameof(Supplier), request.SupplierId);

            var (items, totalCount) = await serviceRepository.GetPagedBySupplierIdAsync(
                request.SupplierId,
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.SortBy,
                request.SortDescending,
                cancellationToken);

            var services = items
                .Select(s => new ServiceResponse(s.Id, s.Name, s.HourlyRate, s.SupplierId))
                .ToList();

            return new PagedResult<ServiceResponse>(services, totalCount, request.PageNumber, request.PageSize);
        }
    }
}

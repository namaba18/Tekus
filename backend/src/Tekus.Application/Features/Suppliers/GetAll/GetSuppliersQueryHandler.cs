using MediatR;
using Tekus.Application.Common.Models;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Suppliers.GetAll
{
    public class GetSuppliersQueryHandler(ISupplierRepository supplierRepository)
        : IRequestHandler<GetSuppliersQuery, PagedResult<SupplierResponse>>
    {
        public async Task<PagedResult<SupplierResponse>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await supplierRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.SortBy,
                request.SortDescending,
                cancellationToken);

            var suppliers = items
                .Select(s => new SupplierResponse(s.Id, s.NIT, s.Name, s.WebPage, s.Email))
                .ToList();

            return new PagedResult<SupplierResponse>(suppliers, totalCount, request.PageNumber, request.PageSize);
        }
    }
}

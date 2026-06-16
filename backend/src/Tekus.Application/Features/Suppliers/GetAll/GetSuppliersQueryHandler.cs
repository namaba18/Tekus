using MediatR;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Suppliers.GetAll
{
    public class GetSuppliersQueryHandler(ISupplierRepository supplierRepository)
        : IRequestHandler<GetSuppliersQuery, List<SupplierResponse>>
    {
        public async Task<List<SupplierResponse>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await supplierRepository.GetAllAsync(cancellationToken);

            return suppliers
                .Select(s => new SupplierResponse(s.Id, s.NIT, s.Name, s.WebPage, s.Email))
                .ToList();
        }
    }
}

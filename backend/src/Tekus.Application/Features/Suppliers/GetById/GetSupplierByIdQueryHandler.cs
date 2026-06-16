using MediatR;
using Tekus.Application.Features.Suppliers.GetAll;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Suppliers.GetById
{
    public class GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository)
        : IRequestHandler<GetSupplierByIdQuery, SupplierResponse>
    {
        public async Task<SupplierResponse> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Supplier), request.Id);

            return new SupplierResponse(supplier.Id, supplier.NIT, supplier.Name, supplier.WebPage, supplier.Email);
        }
    }
}

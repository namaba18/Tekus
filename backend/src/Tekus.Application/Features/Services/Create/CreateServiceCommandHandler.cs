using MediatR;
using Tekus.Application.Common.Interfaces;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Services.Create
{
    public class CreateServiceCommandHandler(
        ISupplierRepository supplierRepository,
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateServiceCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken)
                ?? throw new NotFoundException(nameof(Supplier), request.SupplierId);

            var service = Service.Create(request.Name, request.HourlyRate, supplier.Id);

            serviceRepository.Add(service);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ServiceResponse(service.Id, service.Name, service.HourlyRate, service.SupplierId);
        }
    }
}

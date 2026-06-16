using Tekus.Domain.Entities;

namespace Tekus.Domain.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Service>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);

        void Add(Service service);
    }
}

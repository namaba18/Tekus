using Tekus.Domain.Entities;

namespace Tekus.Domain.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<(List<Service> Items, int TotalCount)> GetPagedBySupplierIdAsync(
            Guid supplierId,
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken = default);

        void Add(Service service);
    }
}

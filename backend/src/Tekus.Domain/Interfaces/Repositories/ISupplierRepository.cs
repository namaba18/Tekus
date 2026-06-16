using Tekus.Domain.Entities;

namespace Tekus.Domain.Interfaces.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<(List<Supplier> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken = default);
    }
}

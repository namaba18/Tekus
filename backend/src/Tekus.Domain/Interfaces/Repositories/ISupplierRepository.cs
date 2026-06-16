using Tekus.Domain.Entities;

namespace Tekus.Domain.Interfaces.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

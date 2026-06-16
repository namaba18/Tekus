using Tekus.Domain.Entities;

namespace Tekus.Domain.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Add(Service service);
    }
}

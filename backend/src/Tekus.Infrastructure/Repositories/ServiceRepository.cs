using Microsoft.EntityFrameworkCore;
using Tekus.Domain.Entities;
using Tekus.Domain.Interfaces.Repositories;
using Tekus.Infrastructure.Data;

namespace Tekus.Infrastructure.Repositories
{
    public class ServiceRepository(AppDbContext context) : IServiceRepository
    {
        public Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Services.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public void Add(Service service) => context.Services.Add(service);
    }
}

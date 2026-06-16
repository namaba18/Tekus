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

        public Task<List<Service>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default) =>
            context.Services.AsNoTracking()
                .Where(s => s.SupplierId == supplierId)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);

        public void Add(Service service) => context.Services.Add(service);
    }
}

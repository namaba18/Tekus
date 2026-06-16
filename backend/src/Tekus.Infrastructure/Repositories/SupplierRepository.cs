using Microsoft.EntityFrameworkCore;
using Tekus.Domain.Entities;
using Tekus.Domain.Interfaces.Repositories;
using Tekus.Infrastructure.Data;

namespace Tekus.Infrastructure.Repositories
{
    public class SupplierRepository(AppDbContext context) : ISupplierRepository
    {
        public Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Suppliers.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Suppliers.AsNoTracking().OrderBy(s => s.Name).ToListAsync(cancellationToken);
    }
}

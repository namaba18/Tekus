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

        public async Task<(List<Service> Items, int TotalCount)> GetPagedBySupplierIdAsync(
            Guid supplierId,
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken = default)
        {
            var query = context.Services.AsNoTracking()
                .Where(s => s.SupplierId == supplierId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(s => EF.Functions.Like(s.Name, $"%{term}%"));
            }

            query = sortBy?.Trim().ToLowerInvariant() switch
            {
                "hourlyrate" => sortDescending ? query.OrderByDescending(s => s.HourlyRate) : query.OrderBy(s => s.HourlyRate),
                _ => sortDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public void Add(Service service) => context.Services.Add(service);
    }
}

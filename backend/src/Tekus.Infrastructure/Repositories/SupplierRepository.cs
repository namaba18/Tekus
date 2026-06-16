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

        public async Task<(List<Supplier> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken = default)
        {
            var query = context.Suppliers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(s =>
                    EF.Functions.Like(s.Name, $"%{term}%") ||
                    EF.Functions.Like(s.NIT, $"%{term}%") ||
                    EF.Functions.Like(s.Email, $"%{term}%"));
            }

            query = sortBy?.Trim().ToLowerInvariant() switch
            {
                "nit" => sortDescending ? query.OrderByDescending(s => s.NIT) : query.OrderBy(s => s.NIT),
                "email" => sortDescending ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                "webpage" => sortDescending ? query.OrderByDescending(s => s.WebPage) : query.OrderBy(s => s.WebPage),
                _ => sortDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}

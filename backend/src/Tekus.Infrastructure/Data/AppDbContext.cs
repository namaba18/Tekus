using Microsoft.EntityFrameworkCore;
using Tekus.Application.Common.Interfaces;
using Tekus.Domain.Entities;
using Tekus.Domain.Entities.Common;
using Tekus.Domain.Interfaces;

namespace Tekus.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IEventDispatcher eventDispatcher)
        : DbContext(options), IUnitOfWork
    {
        internal DbSet<Supplier> Suppliers => Set<Supplier>();
        internal DbSet<Service> Services => Set<Service>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Count != 0)
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    await eventDispatcher.DispatchAsync(domainEvent);
                }
            }

            return result;
        }
    }
}

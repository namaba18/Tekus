using IntegrationTests.Common;
using Tekus.Domain.Entities;
using Tekus.Infrastructure.Repositories;

namespace IntegrationTests.Repositories
{
    public class ServiceRepositoryTests : SqliteIntegrationTestBase
    {
        private readonly ServiceRepository _repository;
        private readonly Supplier _supplier;

        public ServiceRepositoryTests()
        {
            _repository = new ServiceRepository(Context);

            _supplier = new Supplier { NIT = "900000000-1", Name = "Proveedor X", WebPage = "", Email = "x@x.com" };
            Context.Suppliers.Add(_supplier);
            Context.SaveChanges();
        }

        [Fact]
        public async Task Add_PersistsServiceAssociatedToSupplier()
        {
            var service = Service.Create("Soporte", 50m, _supplier.Id);

            _repository.Add(service);
            await Context.SaveChangesAsync();

            var stored = await _repository.GetByIdAsync(service.Id);

            Assert.NotNull(stored);
            Assert.Equal(_supplier.Id, stored!.SupplierId);
            Assert.Equal("Soporte", stored.Name);
        }

        [Fact]
        public async Task GetPagedBySupplierIdAsync_FiltersSortsAndScopesResultsToSupplier()
        {
            var otherSupplier = new Supplier { NIT = "other", Name = "Otro", WebPage = "", Email = "o@o.com" };
            Context.Suppliers.Add(otherSupplier);

            Context.Services.AddRange(
                Service.Create("Mantenimiento", 30m, _supplier.Id),
                Service.Create("Consultoría", 80m, _supplier.Id),
                Service.Create("Soporte ajeno", 10m, otherSupplier.Id));
            await Context.SaveChangesAsync();

            var (items, totalCount) = await _repository.GetPagedBySupplierIdAsync(
                _supplier.Id, 1, 10, null, "hourlyrate", sortDescending: true);

            Assert.Equal(2, totalCount);
            Assert.Equal("Consultoría", items[0].Name);
            Assert.Equal("Mantenimiento", items[1].Name);
            Assert.All(items, s => Assert.Equal(_supplier.Id, s.SupplierId));
        }
    }
}

using IntegrationTests.Common;
using Tekus.Domain.Entities;
using Tekus.Infrastructure.Repositories;

namespace IntegrationTests.Repositories
{
    public class SupplierRepositoryTests : SqliteIntegrationTestBase
    {
        private readonly SupplierRepository _repository;

        public SupplierRepositoryTests()
        {
            _repository = new SupplierRepository(Context);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSupplier_WhenItExists()
        {
            var supplier = new Supplier { NIT = "900111222-1", Name = "Acme", WebPage = "https://acme.com", Email = "info@acme.com" };
            Context.Suppliers.Add(supplier);
            await Context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(supplier.Id);

            Assert.NotNull(result);
            Assert.Equal("Acme", result!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenItDoesNotExist()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPagedAsync_FiltersBySearchTermAcrossNitNameAndEmail()
        {
            Context.Suppliers.AddRange(
                new Supplier { NIT = "1", Name = "Acme Corp", WebPage = "", Email = "a@a.com" },
                new Supplier { NIT = "2", Name = "Globex", WebPage = "", Email = "b@b.com" });
            await Context.SaveChangesAsync();

            var (items, totalCount) = await _repository.GetPagedAsync(1, 10, "Acme", null, false);

            Assert.Equal(1, totalCount);
            Assert.Single(items);
            Assert.Equal("Acme Corp", items[0].Name);
        }

        [Fact]
        public async Task GetPagedAsync_SortsDescendingAndPaginatesResults()
        {
            for (var i = 1; i <= 3; i++)
            {
                Context.Suppliers.Add(new Supplier { NIT = $"NIT-{i}", Name = $"Supplier {i}", WebPage = "", Email = $"s{i}@test.com" });
            }
            await Context.SaveChangesAsync();

            var (items, totalCount) = await _repository.GetPagedAsync(1, 2, null, "name", sortDescending: true);

            Assert.Equal(3, totalCount);
            Assert.Equal(2, items.Count);
            Assert.Equal("Supplier 3", items[0].Name);
            Assert.Equal("Supplier 2", items[1].Name);
        }
    }
}

using Moq;
using Tekus.Application.Features.Suppliers.GetAll;
using Tekus.Domain.Entities;
using Tekus.Domain.Interfaces.Repositories;

namespace UnitTests.Features.Suppliers.GetAll
{
    public class GetSuppliersQueryHandlerTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepository = new();

        private GetSuppliersQueryHandler CreateHandler() => new(_supplierRepository.Object);

        [Fact]
        public async Task Handle_ReturnsMappedPagedResult()
        {
            var suppliers = new List<Supplier>
            {
                new() { NIT = "1", Name = "Acme", WebPage = "https://acme.com", Email = "a@a.com" }
            };
            _supplierRepository
                .Setup(r => r.GetPagedAsync(1, 10, "acme", "name", false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((suppliers, 1));

            var handler = CreateHandler();
            var query = new GetSuppliersQuery { PageNumber = 1, PageSize = 10, SearchTerm = "acme", SortBy = "name" };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, result.TotalCount);
            Assert.Equal("Acme", result.Items[0].Name);
        }

        [Fact]
        public async Task Handle_PassesPagingSortingAndSearchParametersToRepository()
        {
            _supplierRepository
                .Setup(r => r.GetPagedAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<Supplier>(), 0));

            var handler = CreateHandler();
            var query = new GetSuppliersQuery { PageNumber = 2, PageSize = 5, SearchTerm = "globex", SortBy = "email", SortDescending = true };

            await handler.Handle(query, CancellationToken.None);

            _supplierRepository.Verify(r => r.GetPagedAsync(2, 5, "globex", "email", true, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

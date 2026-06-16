using Moq;
using Tekus.Application.Features.Services.GetBySupplier;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace UnitTests.Features.Services.GetBySupplier
{
    public class GetServicesBySupplierQueryHandlerTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepository = new();
        private readonly Mock<IServiceRepository> _serviceRepository = new();

        private GetServicesBySupplierQueryHandler CreateHandler() =>
            new(_supplierRepository.Object, _serviceRepository.Object);

        [Fact]
        public async Task Handle_ReturnsPagedServices_WhenSupplierExists()
        {
            var supplier = new Supplier { NIT = "1", Name = "Acme", WebPage = "", Email = "a@a.com" };
            _supplierRepository.Setup(r => r.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>())).ReturnsAsync(supplier);

            var services = new List<Service> { Service.Create("Soporte", 40m, supplier.Id) };
            _serviceRepository
                .Setup(r => r.GetPagedBySupplierIdAsync(supplier.Id, 1, 10, null, "name", false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((services, 1));

            var handler = CreateHandler();
            var query = new GetServicesBySupplierQuery(supplier.Id) { PageNumber = 1, PageSize = 10, SortBy = "name" };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
            Assert.Equal("Soporte", result.Items[0].Name);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenSupplierDoesNotExist()
        {
            _supplierRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Supplier?)null);

            var handler = CreateHandler();
            var query = new GetServicesBySupplierQuery(Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}

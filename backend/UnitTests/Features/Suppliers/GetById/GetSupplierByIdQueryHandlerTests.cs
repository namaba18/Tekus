using Moq;
using Tekus.Application.Features.Suppliers.GetById;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace UnitTests.Features.Suppliers.GetById
{
    public class GetSupplierByIdQueryHandlerTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepository = new();

        private GetSupplierByIdQueryHandler CreateHandler() => new(_supplierRepository.Object);

        [Fact]
        public async Task Handle_ReturnsMappedSupplier_WhenItExists()
        {
            var supplier = new Supplier { NIT = "900-1", Name = "Acme", WebPage = "https://acme.com", Email = "a@a.com" };
            _supplierRepository.Setup(r => r.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>())).ReturnsAsync(supplier);

            var handler = CreateHandler();
            var result = await handler.Handle(new GetSupplierByIdQuery(supplier.Id), CancellationToken.None);

            Assert.Equal(supplier.Id, result.Id);
            Assert.Equal("Acme", result.Name);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenSupplierDoesNotExist()
        {
            _supplierRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Supplier?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(new GetSupplierByIdQuery(Guid.NewGuid()), CancellationToken.None));
        }
    }
}

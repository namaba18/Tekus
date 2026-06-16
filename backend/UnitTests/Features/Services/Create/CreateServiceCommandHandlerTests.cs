using Moq;
using Tekus.Application.Common.Interfaces;
using Tekus.Application.Features.Services.Create;
using Tekus.Domain.Entities;
using Tekus.Domain.Exceptions;
using Tekus.Domain.Interfaces.Repositories;

namespace UnitTests.Features.Services.Create
{
    public class CreateServiceCommandHandlerTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepository = new();
        private readonly Mock<IServiceRepository> _serviceRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private CreateServiceCommandHandler CreateHandler() =>
            new(_supplierRepository.Object, _serviceRepository.Object, _unitOfWork.Object);

        [Fact]
        public async Task Handle_CreatesAndSavesService_WhenSupplierExists()
        {
            var supplier = new Supplier { NIT = "1", Name = "Acme", WebPage = "", Email = "a@a.com" };
            _supplierRepository.Setup(r => r.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>())).ReturnsAsync(supplier);

            var handler = CreateHandler();
            var command = new CreateServiceCommand("Soporte", 75m, supplier.Id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal("Soporte", result.Name);
            Assert.Equal(75m, result.HourlyRate);
            Assert.Equal(supplier.Id, result.SupplierId);
            _serviceRepository.Verify(r => r.Add(It.Is<Service>(s => s.Name == "Soporte" && s.SupplierId == supplier.Id)), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenSupplierDoesNotExist()
        {
            _supplierRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Supplier?)null);

            var handler = CreateHandler();
            var command = new CreateServiceCommand("Soporte", 75m, Guid.NewGuid());

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

            _serviceRepository.Verify(r => r.Add(It.IsAny<Service>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Tekus.Application.Common.Email;
using Tekus.Application.Common.Options;
using Tekus.Application.Features.Services.Events;
using Tekus.Domain.Entities;
using Tekus.Domain.Events;
using Tekus.Domain.Interfaces.Repositories;

namespace UnitTests.Features.Services.Events
{
    public class ServiceCreatedEmailNotificationHandlerTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepository = new();
        private readonly Mock<IEmailSender> _emailSender = new();

        private ServiceCreatedEmailNotificationHandler CreateHandler(string recipient) =>
            new(
                _supplierRepository.Object,
                _emailSender.Object,
                Options.Create(new NotificationSettings { NewServiceRecipientEmail = recipient }),
                NullLogger<ServiceCreatedEmailNotificationHandler>.Instance);

        [Fact]
        public async Task Handle_SendsEmailToConfiguredRecipient_WhenRecipientIsConfigured()
        {
            var supplier = new Supplier { NIT = "1", Name = "Acme", WebPage = "", Email = "a@a.com" };
            _supplierRepository.Setup(r => r.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>())).ReturnsAsync(supplier);

            var handler = CreateHandler("compras@tekus.com");
            var domainEvent = new ServiceCreatedEvent(Guid.NewGuid(), "Soporte", 50m, supplier.Id);

            await handler.Handle(domainEvent);

            _emailSender.Verify(e => e.SendAsync(
                "compras@tekus.com",
                "Nuevo servicio habilitado",
                It.Is<string>(body => body.Contains("Acme") && body.Contains("Soporte")),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DoesNotSendEmail_WhenRecipientIsNotConfigured()
        {
            var handler = CreateHandler(string.Empty);
            var domainEvent = new ServiceCreatedEvent(Guid.NewGuid(), "Soporte", 50m, Guid.NewGuid());

            await handler.Handle(domainEvent);

            _emailSender.Verify(e => e.SendAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

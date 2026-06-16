using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tekus.Application.Common.Email;
using Tekus.Application.Common.Options;
using Tekus.Domain.Events;
using Tekus.Domain.Interfaces;
using Tekus.Domain.Interfaces.Repositories;

namespace Tekus.Application.Features.Services.Events
{
    /// <summary>
    /// Notifies the recipient configured in system preferences whenever a supplier enables a new service.
    /// </summary>
    public class ServiceCreatedEmailNotificationHandler(
        ISupplierRepository supplierRepository,
        IEmailSender emailSender,
        IOptions<NotificationSettings> notificationOptions,
        ILogger<ServiceCreatedEmailNotificationHandler> logger) : IDomainEventHandler<ServiceCreatedEvent>
    {
        public async Task Handle(ServiceCreatedEvent domainEvent)
        {
            var recipient = notificationOptions.Value.NewServiceRecipientEmail;
            if (string.IsNullOrWhiteSpace(recipient))
            {
                logger.LogWarning("Skipped new service notification: no recipient configured in Notifications:NewServiceRecipientEmail.");
                return;
            }

            var supplier = await supplierRepository.GetByIdAsync(domainEvent.SupplierId);
            var supplierName = supplier?.Name ?? "un proveedor";

            const string subject = "Nuevo servicio habilitado";
            var body = $"""
                El proveedor "{supplierName}" ha habilitado un nuevo servicio.

                Servicio: {domainEvent.ServiceName}
                Tarifa por hora: {domainEvent.HourlyRate:C} USD
                """;

            try
            {
                await emailSender.SendAsync(recipient, subject, body);
            }
            catch (Exception ex)
            {
                // The service was already created successfully; a notification failure should not roll that back.
                logger.LogError(ex, "Failed to send new service notification email for service {ServiceId}.", domainEvent.ServiceId);
            }
        }
    }
}

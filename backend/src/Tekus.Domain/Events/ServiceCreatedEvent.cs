namespace Tekus.Domain.Events
{
    public record ServiceCreatedEvent(Guid ServiceId, string ServiceName, decimal HourlyRate, Guid SupplierId);
}

namespace Tekus.Application.Features.Services.Create
{
    public record ServiceResponse(Guid Id, string Name, decimal HourlyRate, Guid SupplierId);
}

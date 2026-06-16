using MediatR;

namespace Tekus.Application.Features.Services.Create
{
    public record CreateServiceCommand(string Name, decimal HourlyRate, Guid SupplierId) : IRequest<ServiceResponse>;
}
